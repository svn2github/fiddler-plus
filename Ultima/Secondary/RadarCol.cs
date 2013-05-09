using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ultima.Secondary
{
    public sealed class RadarCol
    {
        private static short[] m_Colors;
        public static short[] Colors { get { return m_Colors; } }

        public static short GetItemColor(int index)
        {
            if (index + 0x4000 < m_Colors.Length)
                return m_Colors[index + 0x4000];
            return 0;
        }
        public static short GetLandColor(int index)
        {
            if (index < m_Colors.Length)
                return m_Colors[index];
            return 0;
        }

        public static void SetItemColor(int index, short value)
        {
            m_Colors[index + 0x4000] = value;
        }
        public static void SetLandColor(int index, short value)
        {
            m_Colors[index] = value;
        }

        public static bool SetFile(string mulPath)
        {
            if (!String.IsNullOrEmpty(mulPath) && !File.Exists(mulPath))
                return false;

            using (FileStream fs = new FileStream(mulPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                m_Colors = new short[fs.Length / 2];
                GCHandle gc = GCHandle.Alloc(m_Colors, GCHandleType.Pinned);
                byte[] buffer = new byte[(int)fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
                Marshal.Copy(buffer, 0, gc.AddrOfPinnedObject(), (int)fs.Length);
                gc.Free();
            }
            return true;
        }

        public static void Dispose()
        {
            m_Colors = null;
        }
    }
}
