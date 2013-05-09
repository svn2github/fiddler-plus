using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace tevton.SyntaxHighlight {
    public class RichTextBoxHighlighter {
        private SyntaxHighlightDictionary dictionary;
        public SyntaxHighlightDictionary Dictionary {
            get { return dictionary; }
            set { dictionary = value; }
        }

        private int delay = 50;
        /// <summary>
        /// Delay in milliseconds to wait before automatic highlighting
        /// </summary>
        public int Delay {
            get { return delay; }
            set { delay = value; }
        }
        /// <summary>
        /// internal struct to keep track of tracked objects
        /// </summary>
        private struct ObjectTime {
            public object obj;
            public DateTime time;
            public ObjectTime(object obj, DateTime time) {
                this.obj = obj;
                this.time = time;
            }
        }
        /// <summary>
        /// List of tracked objects
        /// </summary>
        private class ObjectTimeList: List<ObjectTime> {
            public ObjectTime FindOrInsertObject(object obj) {
                foreach(ObjectTime ot in this) {
                    if(ot.obj == obj) {
                        return ot;
                    }
                }
                ObjectTime oTime = new ObjectTime(obj, DateTime.Now);
                this.Add(oTime);
                return oTime;
            }
        }

        private ObjectTimeList objectTimes = new ObjectTimeList();

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);
        private struct SCROLLINFO {
            public int cbSize;
            public int fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;
        }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int GetScrollInfo(HandleRef hWnd, int fnBar, ref SCROLLINFO lpsi);
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SetScrollInfo(HandleRef hWnd, int fnBar, ref SCROLLINFO lpsi, bool fRedraw);
        private const int WM_SETREDRAW = 11;
        private const int SB_HORZ = 0;
        private const int SB_VERT = 1;
        private const int SIF_RANGE = 0x1;
        private const int SIF_PAGE = 0x2;
        private const int SIF_POS = 0x4;
        private const int SIF_DISABLENOSCROLL = 0x8;
        private const int SIF_TRACKPOS = 0x10;
        private const int SIF_ALL = (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS);
        private const int WM_VSCROLL = 0x0115;
        private const int WM_HSCROLL = 0x0114;
        private const int SB_THUMBPOSITION = 4;
        /// <summary>
        /// Highlights text in textbox passed as parameter
        /// according to definitions in the Dictionary
        /// </summary>
        /// <param name="textBox">RichTextBox component to highlight the text in</param>
        public void HighlightRichTextBox(RichTextBox textBox) {
            if(Dictionary == null)
                return;
            Dictionary.CreateSnapshot(textBox.Text);
            //Save selection and scrollbar positions:
            int selStart = textBox.SelectionStart;
            int selLength = textBox.SelectionLength;
            HandleRef textBoxHandle = new HandleRef(textBox, textBox.Handle);
            SCROLLINFO vsi = new SCROLLINFO();
            vsi.cbSize = Marshal.SizeOf(vsi);
            vsi.fMask = SIF_ALL;
            GetScrollInfo(textBoxHandle, SB_VERT, ref vsi);
            SCROLLINFO hsi = new SCROLLINFO();
            hsi.cbSize = Marshal.SizeOf(hsi);
            hsi.fMask = SIF_ALL;
            GetScrollInfo(textBoxHandle, SB_HORZ, ref hsi);
            //TODO: do not "freeze" the box if it's already frozen
            //Prevent the textbox from redrawing
            SendMessage(textBoxHandle, WM_SETREDRAW, 0, 0);
            try {
                textBox.SelectAll();
                textBox.SelectionFont = Dictionary.Font;
                textBox.SelectionColor = Dictionary.ForegroundColor;
                textBox.SelectionBackColor = Dictionary.BackgroundColor;
                foreach(SyntaxHighlightItem synItem in Dictionary) {
                    foreach(SyntaxHighlightSegment segment in synItem.AllSegments) {
                        textBox.Select(segment.OrderedStart, segment.Length);
                        textBox.SelectionFont = new Font(Dictionary.Font, synItem.FontStyle);
                        textBox.SelectionColor = synItem.ForegroundColor;
                        textBox.SelectionBackColor = synItem.BackgroundColor;
                    }
                }
            } finally {
                //Restore selection and scrollbar positions:
                textBox.SelectionStart = selStart;
                textBox.SelectionLength = selLength;
                SetScrollInfo(textBoxHandle, SB_VERT, ref vsi, true);
                SendMessage(textBoxHandle, WM_VSCROLL, SB_THUMBPOSITION + 0x10000 * vsi.nTrackPos, 0);
                SetScrollInfo(textBoxHandle, SB_HORZ, ref hsi, true);
                SendMessage(textBoxHandle, WM_HSCROLL, SB_THUMBPOSITION + 0x10000 * hsi.nPos, 0);
                //TODO: do not unfreeze textbox if it was frozen before entering this method
                SendMessage(textBoxHandle, WM_SETREDRAW, 1, 0);
                textBox.Refresh();
            }
        }
        public void RichBox_TextChanged(object sender, EventArgs e) {
            ObjectTime boxTime = objectTimes.FindOrInsertObject(sender);
            boxTime.time = DateTime.Now;
        }
        public void HookToRichTextBox(RichTextBox textBox) {
            textBox.TextChanged += new EventHandler(RichBox_TextChanged);
        }
        public void UnhookRichTextBox(RichTextBox textBox) {
            textBox.TextChanged -= new EventHandler(RichBox_TextChanged);
            ObjectTime boxTime = objectTimes.FindOrInsertObject(textBox);
            objectTimes.Remove(boxTime);
        }

        private void AutoHighlightWhenIdle(object sender, EventArgs e) {
            for(int i = objectTimes.Count - 1; i >= 0; i--) {
                if(objectTimes[i].time.AddMilliseconds(Delay) < DateTime.Now &&
                    objectTimes[i].obj is RichTextBox) {
                    RichTextBox textBox = (RichTextBox)objectTimes[i].obj;
                    //prevent textbox changes from triggering highlighting again
                    UnhookRichTextBox(textBox);
                    HighlightRichTextBox(textBox);
                    //restore highlighting tracking
                    HookToRichTextBox(textBox);
                }
            }
        }

        public RichTextBoxHighlighter() {
            Application.Idle += new EventHandler(AutoHighlightWhenIdle);
        }

        public RichTextBoxHighlighter(SyntaxHighlightDictionary dictionary)
            : this() {
            this.Dictionary = dictionary;
        }
    }
}