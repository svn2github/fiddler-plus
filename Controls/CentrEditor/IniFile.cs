using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace FiddlerControls.CentrEditor
{
    internal sealed class IniFile
    {
        public static string FilePath { get; private set; }

        public IniFile(string inifile)
        {
            FilePath = inifile;
        }

        [DllImport("Kernel32", CharSet = CharSet.Unicode)]
        private static extern Int32 GetPrivateProfileInt(string appName, string keyName, Int32 valDefault, string filePath);

        [DllImport("Kernel32", CharSet = CharSet.Unicode)]
        private static extern UInt32 GetPrivateProfileString(string appName, string keyName, string valDefault, string valReturn, UInt32 size, string filePath);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        [DllImport("Wininet", CharSet = CharSet.Unicode)]
        private static extern Int32 InternetGetConnectedState(IntPtr lpdwFlags, UInt32 dwReserved);

        public bool ReadBool(string appName, string keyName, bool valDefault = false, bool write = true)
        {
            int getint = GetPrivateProfileInt(appName, keyName, valDefault ? 1 : 0, FilePath);
            bool result = !(getint <= 0);

            if (write && result == valDefault)
                WriteBool(appName, keyName, valDefault);

            return result;
        }

        public int ReadInt(string appName, string keyName, int valDefault = 0, bool write = true)
        {
            int result = GetPrivateProfileInt(appName, keyName, valDefault, FilePath);

            if (write && result == valDefault)
                WriteInt(appName, keyName, valDefault);

            return result;
        }

        public string ReadString(string appName, string keyName, string valDefault = "", bool write = true)
        {
            string getstr = new string('\0', 0x4000);
            GetPrivateProfileString(appName, keyName, valDefault, getstr, 0x4000, FilePath);
            string result = getstr.TrimEnd('\0');
            if (String.IsNullOrEmpty(result))
                result = valDefault;

            if (write && result == valDefault)
                WriteString(appName, keyName, valDefault);

            return result;
        }

        public bool WriteBool(string appName, string keyName, bool val)
        {
            bool result = WritePrivateProfileString(appName, keyName, val ? "1" : "0", FilePath);
            return result;
        }

        public bool WriteInt(string appName, string keyName, int val)
        {
            bool result = WritePrivateProfileString(appName, keyName, String.Format("{0}", val), FilePath);
            return result;
        }

        public bool WriteString(string appName, string keyName, string val)
        {
            bool result = WritePrivateProfileString(appName, keyName, String.Format("\"{0}\"", val), FilePath);
            return result;
        }
    }  

}
