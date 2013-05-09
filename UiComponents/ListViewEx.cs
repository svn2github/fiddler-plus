using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using System.Collections.Generic;
using System.Text;

namespace GuiControls
{
    public class ListViewEx : ListView
    {
        [Bindable(true), Category("Appearance"), DefaultValue(false), Description("Если свойство равно true, то используется двойная буфферизация для предотвращения мерцания при перерисовке.")]
        public bool AntiBlink { get { return _AntiBlink; } set { if (_AntiBlink == value) return;
            //Activate double buffering
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, value);

            //Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.EnableNotifyMessage, value);

            _AntiBlink = value;
        } }
        private bool _AntiBlink = false;

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (AntiBlink && m.Msg != 0x14) {
                base.OnNotifyMessage(m);
            }
        }

        #region "ScrollToGroup methods"

        /// <summary>
        /// Scroll to a group.
        /// </summary>
        /// <param name="k">The name of the group to scroll to.</param>
        public void ScrollToGroup(String k)
        {
            ScrollToGroup(Groups[k]);
        }

        /// <summary>
        /// Scroll to a group.
        /// </summary>
        /// <param name="i">The index of the group to scroll to.</param>
        public void ScrollToGroup(int i)
        {
            ScrollToGroup(Groups[i]);
        }

        /// <summary>
        /// Scrolls to a group.
        /// </summary>
        /// <param name="lvg">A ListViewGroup object. If it does not contain ListViewItems,
        /// it will not be possible to scroll to it, as its scroll position is determined
        /// by the y-coordinate of the group's first item.</param>
        private void ScrollToGroup(ListViewGroup lvg)
        {
            if (lvg.Items.Count > 0)
            {
                //Need to find a better way to programmatically determine the height
                //of a group header. 30 seems to work, but that will change if fonts, styles, etc... change. :(
                SetVScrollPos(lvg.Items[0].Position.Y - 30);
            }
        }

        #endregion

        // Event declaration  
        [Bindable(true), Category("Appearance"), DefaultValue(null), Description("Событие возбуждаемое при прокрутке области списка.")]
        public event ListViewExArgsScrollDelegate Scroll;
        public delegate void ListViewExArgsScrollDelegate(object Sender, ListViewExArgs e);

        #region Invoke Scroll Event

        // WM_VSCROLL message constants
        private const int WM_VSCROLL = 0x0115;
        private const int SB_THUMBTRACK = 5;
        private const int SB_ENDSCROLL = 8;

        protected override void DefWndProc(ref Message m)
        {
            base.DefWndProc(ref m);
        }

        protected override void WndProc(ref Message m)
        {
            // Trap the WM_VSCROLL message to generate the Scroll event
            base.WndProc(ref m);

            if (m.Msg == WM_VSCROLL)
            {
                int nfy = m.WParam.ToInt32() & 0xFFFF;
                if (Scroll != null && (nfy == SB_THUMBTRACK || nfy == SB_ENDSCROLL))
                    Scroll(this, new ListViewExArgs(GetVScrollPos()));
                    //Scroll(this, new ListViewExArgs(prevPos, curPos, nfy == SB_THUMBTRACK));
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (Scroll != null)
                Scroll(this, new ListViewExArgs(GetVScrollPos()));
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (Scroll != null && (e.KeyCode == Keys.PageUp || e.KeyCode == Keys.PageDown || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Home || e.KeyCode == Keys.End))
                Scroll(this, new ListViewExArgs(GetVScrollPos()));
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (Scroll != null && (e.KeyCode == Keys.PageUp || e.KeyCode == Keys.PageDown || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Home || e.KeyCode == Keys.End))
                Scroll(this, new ListViewExArgs(GetVScrollPos()));
        }

        private readonly char[] mKeyCode = new char[] { (char)Keys.PageUp, (char)Keys.PageDown, (char)Keys.Up, (char)Keys.Down, (char)Keys.Home, (char)Keys.End };

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (Scroll != null && e.Handled && (e.KeyChar == mKeyCode[0] || e.KeyChar == mKeyCode[1] || e.KeyChar == mKeyCode[2] || e.KeyChar == mKeyCode[3] || e.KeyChar == mKeyCode[4] || e.KeyChar == mKeyCode[5]))
                Scroll(this, new ListViewExArgs(GetVScrollPos()));
        }

        #endregion

        public int GetVScrollPos()
        {
            SCROLLINFO currentInfo = new SCROLLINFO();
            currentInfo.cbSize = Marshal.SizeOf(currentInfo);
            currentInfo.fMask = (int)ScrollInfoMask.SIF_ALL;

            if (GetScrollInfo(this.Handle, (int)ScrollBarDirection.SB_VERT, ref currentInfo) != 0)
                return currentInfo.nPos;
            //else {
            //Debug.WriteLine("Error getting scroll info");
            //prevScrollPos = 0;
            //}
            return 0;
        }

        /// <summary>
		/// Vertically scroll to an absolute position.
		/// </summary>
		/// <param name="scrollPos">The position to scroll to.</param>
        public void SetVScrollPos(int scrollPos)
		{
			int prevScrollPos = 0;
			SCROLLINFO currentInfo = new SCROLLINFO();
			currentInfo.cbSize = Marshal.SizeOf(currentInfo);
			currentInfo.fMask = (int)ScrollInfoMask.SIF_ALL;

			if (GetScrollInfo(this.Handle, (int)ScrollBarDirection.SB_VERT, ref currentInfo) == 0)
			{
				//Debug.WriteLine("Error getting scroll info");
				prevScrollPos = scrollPos;
			}
			else
				prevScrollPos = currentInfo.nPos;

			//The LVM_SCROLL message will take a delta-x and delta-y which tell the list view how 
			//much to scroll, relative to the current scroll positions. We are getting the scroll
			//position as an absolute position, so some adjustments are necessary:
			scrollPos -= prevScrollPos;
			//Send the LVM_SCROLL message to scroll the list view.			
			SendMessage(new HandleRef(null, this.Handle), (uint)ListViewMessages.LVM_SCROLL, (IntPtr)0, (IntPtr)scrollPos);
		}

        #region "Windows API Stuff"
		[StructLayout(LayoutKind.Sequential)]
		private struct SCROLLINFO
		{
			public int  cbSize;
			public uint fMask;
			public int  nMin;
			public int  nMax;
			public uint nPage;
			public int  nPos;
			public int  nTrackPos;
		}

		private enum ScrollBarDirection
		{
			SB_HORZ = 0,
			SB_VERT = 1,
			SB_CTL  = 2,
			SB_BOTH = 3
		}

		[System.Flags]
		private enum ScrollInfoMask
		{
			SIF_RANGE           = 0x1,
			SIF_PAGE            = 0x2,
			SIF_POS             = 0x4,
			SIF_DISABLENOSCROLL = 0x8,
			SIF_TRACKPOS        = 0x10,
			SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS
		}

		//There are many more ListViewMessages, only need LVM_SCROLL ...for now!
		private enum ListViewMessages : int  
		{   
			LVM_FIRST   = 0x1000,   
			LVM_SCROLL  = (LVM_FIRST + 20)   
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
		private static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		private static extern int GetScrollInfo(IntPtr hwnd, int fnBar, ref SCROLLINFO lpsi);

		#endregion
    }

    public class ListViewExArgs : EventArgs
    {
        // Scroll event argument
        /*
        public int PrevVScrollPos
        { get { return mPrevVScrollPos; } }
        private int mPrevVScrollPos;

        public int CurVScrollPos
        { get { return mCurVScrollPos; } }
        private int mCurVScrollPos;

        public bool Tracking
        { get { return mTracking; } }
        private bool mTracking;

        

        public ListViewExArgs(int prevscrollpos, int curscrollpos, bool tracking) : base()
        {
            mPrevVScrollPos = prevscrollpos;
            mCurVScrollPos = curscrollpos;
            mTracking = tracking;
        }
        */

        public int VScrollPos
        { get { return mVScrollPos; } }
        private int mVScrollPos;

        public ListViewExArgs(int scrollpos) : base()
        {
            mVScrollPos = scrollpos;
        }
    }
}
