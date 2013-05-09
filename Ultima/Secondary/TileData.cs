using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Ultima.Secondary
{
    public sealed class TileData
    {
        private static LandData[] m_LandData;
        private static ItemData[] m_ItemData;
        private static int[] m_HeightTable;

        /// <summary>
        /// Gets the list of <see cref="LandData">land tile data</see>.
        /// </summary>
        public static LandData[] LandTable
        {
            get { return m_LandData; }
            set { m_LandData = value; }
        }

        /// <summary>
        /// Gets the list of <see cref="ItemData">item tile data</see>.
        /// </summary>
        public static ItemData[] ItemTable
        {
            get { return m_ItemData; }
            set { m_ItemData = value; }
        }

        public static int[] HeightTable
        {
            get { return m_HeightTable; }
        }

        private static byte[] m_StringBuffer;
        private static string ReadNameString(BinaryReader bin)
        {
            bin.Read(m_StringBuffer, 0, 20);

            int count;

            for (count = 0; count < 20 && m_StringBuffer[count] != 0; ++count) ;

            return Encoding.GetEncoding(1251).GetString(m_StringBuffer, 0, count);
        }

        public unsafe static string ReadNameString(byte* buffer)
        {
            int count;
            for (count = 0; count < 20 && *buffer != 0; ++count)
                m_StringBuffer[count] = *buffer++;

            return Encoding.Default.GetString(m_StringBuffer, 0, count);
        }

        private static int[] landheader;
        private static int[] itemheader;

        public static unsafe bool SetFile(string mulPath)
        {
            if (!String.IsNullOrEmpty(mulPath) && !File.Exists(mulPath))
                return false;

            m_StringBuffer = new byte[20];

            using (FileStream fs = new FileStream(mulPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                landheader = new int[512];
                int j = 0;
                m_LandData = new LandData[0x4000];

                byte[] buffer = new byte[fs.Length];
                GCHandle gc = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                long currpos = 0;
                try
                {
                    fs.Read(buffer, 0, buffer.Length);

                    bool newmulFormat = Ultima.Secondary.Art.IsUOHS();

                    if (newmulFormat) // TileData после HS
                    {
                        for (int i = 0; i < 0x4000; i += 32)
                        {
                            IntPtr ptrheader = new IntPtr((long)gc.AddrOfPinnedObject() + currpos);
                            currpos += 4;
                            landheader[j++] = (int)Marshal.PtrToStructure(ptrheader, typeof(int));

                            for (int count = 0; count < 32; ++count)
                            {
                                IntPtr ptr = new IntPtr((long)gc.AddrOfPinnedObject() + currpos);
                                currpos += sizeof(LandTileDataMul);
                                LandTileDataMul cur = (LandTileDataMul)Marshal.PtrToStructure(ptr, typeof(LandTileDataMul));
                                m_LandData[i + count] = new LandData(cur);
                            }
                        }
                    }
                    else // TileData до HS
                    {
                        for (int i = 0; i < 0x4000; i += 32)
                        {
                            IntPtr ptrheader = new IntPtr((long)gc.AddrOfPinnedObject() + currpos);
                            currpos += 4;
                            landheader[j++] = (int)Marshal.PtrToStructure(ptrheader, typeof(int));

                            for (int count = 0; count < 32; ++count)
                            {
                                IntPtr ptr = new IntPtr((long)gc.AddrOfPinnedObject() + currpos);
                                currpos += sizeof(LandTileOldDataMul);
                                LandTileOldDataMul cur = (LandTileOldDataMul)Marshal.PtrToStructure(ptr, typeof(LandTileOldDataMul));
                                m_LandData[i + count] = new LandData(cur);
                            }
                        }
                    }


                    long remaining = buffer.Length - currpos;
                    int itemlength;
                    if (remaining / 41 >= 0xF000)
                    {
                        itemlength = 0x10000;//0xFFDC;
                        itemheader = new int[512 * 4];
                    }
                    else if (remaining / 37 > 0x5000)
                    {
                        itemlength = 0x8000;
                        itemheader = new int[512 * 2];
                    }
                    else
                    {
                        itemlength = 0x4000;
                        itemheader = new int[512 * 1];
                    }

                    m_ItemData = new ItemData[itemlength];
                    m_HeightTable = new int[itemlength];

                    j = 0;
                    if (newmulFormat) // TileData после HS
                    {
                        for (int i = 0; i < itemlength; i += 32)
                        {
                            IntPtr ptrheader = new IntPtr((long)gc.AddrOfPinnedObject() + currpos);
                            currpos += 4;
                            itemheader[j++] = (int)Marshal.PtrToStructure(ptrheader, typeof(int));
                            for (int count = 0; count < 32; ++count)
                            {
                                IntPtr ptr = new IntPtr((long)gc.AddrOfPinnedObject() + currpos);
                                currpos += sizeof(ItemTileDataMul);
                                ItemTileDataMul cur = (ItemTileDataMul)Marshal.PtrToStructure(ptr, typeof(ItemTileDataMul));
                                m_ItemData[i + count] = new ItemData(cur);
                                m_HeightTable[i + count] = cur.height;
                            }
                        }
                    }
                    else // TileData до HS
                    {
                        for (int i = 0; i < itemlength; i += 32)
                        {
                            IntPtr ptrheader = new IntPtr((long)gc.AddrOfPinnedObject() + currpos);
                            currpos += 4;
                            itemheader[j++] = (int)Marshal.PtrToStructure(ptrheader, typeof(int));
                            for (int count = 0; count < 32; ++count)
                            {
                                IntPtr ptr = new IntPtr((long)gc.AddrOfPinnedObject() + currpos);
                                currpos += sizeof(ItemTileOldDataMul);
                                ItemTileOldDataMul cur = (ItemTileOldDataMul)Marshal.PtrToStructure(ptr, typeof(ItemTileOldDataMul));
                                m_ItemData[i + count] = new ItemData(cur);
                                m_HeightTable[i + count] = cur.height;
                            }
                        }
                    }

                }
                finally
                {
                    gc.Free();
                }
                return true;
            }
        }

        public static void Dispose()
        {
            m_LandData = null;
            m_ItemData = null;
            m_HeightTable = null;
            landheader = null;
            itemheader = null;
            m_StringBuffer = null;
        }
    }

}
