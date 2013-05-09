using System;
using System.Runtime.InteropServices;

namespace FiddlerControls.RegionEditor.Locations
{
	/// <summary>
	/// This is a collection of commonly used static functions. This class should not be instantiated
	/// </summary>
	public class Common
	{
		#region Constants

		private const int WM_CHAR = 0x0102;

		#endregion

		#region Win32 Functions

		[DllImport( "User32" )] 
        private static extern IntPtr FindWindow(string lpszClassName, string lpszWindowName);
		[DllImport("user32.dll")]
		private static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam );
		[DllImport("user32.dll")]
		private static extern int SetForegroundWindow( int hWnd );

		#endregion

		/// <summary>
		/// Sends a text message to the Ultima Online window
		/// </summary>
		/// <param name="message">The text that will be sent to the UO window</param>
		public static void SendToUO( string message )
		{
			IntPtr handle = FindWindow( "Ultima Online", null );
			if ( handle.ToInt32() == 0 )
			{
				handle = FindWindow( "Ultima Online Third Dawn", null );
				if ( handle.ToInt32() == 0 )
					return;
			}

			// We have a window
			foreach ( char c in message.ToCharArray() )
				SendMessage( handle.ToInt32(), WM_CHAR, c, 0 );

			SetForegroundWindow( handle.ToInt32() );
		}
	}
}
