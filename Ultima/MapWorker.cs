using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace Ultima
{
    public class MapWorker
    {
        public string MapMulPath { get { return _mapMulPath; } }
        private string _mapMulPath;
        public string StaMulPath { get { return _staMulPath; } }
        private string _staMulPath;
        public string StaIdxPath { get { return _staIdxPath; } }
        private string _staIdxPath;
        public string FilesPaths
        {
            set
            {
                string filename = Path.GetFileNameWithoutExtension(value);
                string path = Path.GetDirectoryName(value);
                string name = String.Empty;

                for (int i = 0; i < filename.Length; ++i)
                {
                    if (Char.IsDigit(filename[i]))
                    {
                        name += filename[i];
                        if (i + 1 == filename.Length)
                        {
                            Index = Int32.Parse(name);
                            name = String.Empty;
                        }
                    }
                    else if (!String.IsNullOrEmpty(name))
                    {
                        Index = Int32.Parse(name);
                        name = filename.Substring(i);
                        break;
                    }
                }
                _mapMulPath = Path.Combine(path, String.Format("map{0}{1}.mul", Index, name));
                _staMulPath = Path.Combine(path, String.Format("statics{0}{1}.mul", Index, name));
                _staIdxPath = Path.Combine(path, String.Format("staidx{0}{1}.mul", Index, name));
            }
        }
        public int Width
        {
            set { _width = (value >> 3) << 3; }
            get { return _width; }
        }
        private int _width;
        public int Height
        {
            set { _height = (value >> 3) << 3; }
            get { return _height; }
        }
        private int _height;
        public int Alt = 0;
        public int Index = 0;

        public bool UseMap { get { return _UseMap; } }
        private bool _UseMap;
        public bool UseStatics { get { return _UseStatics; } }
        private bool _UseStatics;

        public MapWorker(string folder, int index, int width, int height, bool usemap = true, bool usestatic = true)
        {
            _mapMulPath = Path.Combine(folder, String.Format("map{0}.mul", index));
            _staIdxPath = Path.Combine(folder, String.Format("staidx{0}.mul", index));
            _staMulPath = Path.Combine(folder, String.Format("statics{0}.mul", index));
            Width = width;
            Height = height;
            _UseMap = usemap;
            _UseStatics = usestatic;

        }

        public struct Data
        {
            public Block[] m_Bloak;
        }

        //Block Number = (XBlock * 512) + YBlock 
        public struct Block
        {
            public uint m_Header;
            public Tile[] m_Tiles;

            public uint m_Offset;
            public uint m_Length;
            public uint m_Unknown;
            public Static[] m_Statics;
            public bool m_Ocean;
        }

        public struct Tile
        {
            public ushort m_ID;
            public sbyte m_Z;
        }

        public struct Static
        {
            public ushort m_ID;
            public byte m_X;
            public byte m_Y;
            public sbyte m_Z;
            public short m_Hue;
        }

        private BinaryReader MapMul;
        private BinaryReader StaIdx;
        private BinaryReader StaMul;
        private BinaryWriter MapMulW;
        private BinaryWriter StaIdxW;
        private BinaryWriter StaMulW;
        public void Create()
        {
            if (UseMap)
                MapMulW = new BinaryWriter(new FileStream(MapMulPath, FileMode.Create, FileAccess.Write, FileShare.None));
            if (UseStatics)
            {
                StaIdxW = new BinaryWriter(new FileStream(StaIdxPath, FileMode.Create, FileAccess.Write, FileShare.None));
                StaMulW = new BinaryWriter(new FileStream(StaMulPath, FileMode.Create, FileAccess.Write, FileShare.None));
            }
        }

        public void Open()
        {
            if (UseMap)
                MapMul = new BinaryReader(new FileStream(MapMulPath, FileMode.Open, FileAccess.Read, FileShare.Read));
            if (UseStatics)
            {
                StaIdx = new BinaryReader(new FileStream(StaIdxPath, FileMode.Open, FileAccess.Read, FileShare.Read));
                StaMul = new BinaryReader(new FileStream(StaMulPath, FileMode.Open, FileAccess.Read, FileShare.Read));
            }
        }

        public void Close()
        {
            if (MapMul != null)
                MapMul.Close();
            if (StaIdx != null)
                StaIdx.Close();
            if (StaMul != null)
                StaMul.Close();
            if (MapMulW != null)
                MapMulW.Close();
            if (StaIdxW != null)
                StaIdxW.Close();
            if (StaMulW != null)
                StaMulW.Close();
        }

        private int line = 0;
        private static Bitmap bmp = null;
        public Block[] ReadLine0()
        {
            if (bmp == null)
                bmp = new Bitmap(@"C:\UltimaOnline\source\_build\uoFiddler\altmap.bmp");

            //Random rand = new Random();
            if (MapMul.BaseStream.Position == MapMul.BaseStream.Length - 1)
                return null;

            Static[] ocean = new Static[64];
            for (int k = 0; k < 64; ++k)
            {
                ocean[k].m_ID = (ushort)rand.Next(0x1797, 0x179D);
                ocean[k].m_Hue = 0;
                ocean[k].m_X = (byte)(k / 8);
                ocean[k].m_Y = (byte)(k % 8);
                ocean[k].m_Z = -45;//(unchecked(int)(0-45));
            }

            Block[] block = new Block[Height >> 3];
            for (int i = 0; i < (Height >> 3); ++i)
            {
                block[i] = new Block();
                block[i].m_Ocean = false;

                block[i].m_Offset = StaIdx.ReadUInt32();
                block[i].m_Length = StaIdx.ReadUInt32();
                block[i].m_Unknown = StaIdx.ReadUInt32();

                if (block[i].m_Offset != 0xFFFFFFFF && block[i].m_Length != 0)
                {
                    StaMul.BaseStream.Position = block[i].m_Offset;
                    block[i].m_Statics = new Static[block[i].m_Length / 7];
                    for (int k = 0; k < block[i].m_Length / 7; ++k)
                    {
                        block[i].m_Statics[k].m_ID = StaMul.ReadUInt16();
                        if (block[i].m_Statics[k].m_ID > 0x6500)
                            block[i].m_Statics[k].m_ID += 0x7000;
                        block[i].m_Statics[k].m_X = StaMul.ReadByte();
                        block[i].m_Statics[k].m_Y = StaMul.ReadByte();
                        block[i].m_Statics[k].m_Z = StaMul.ReadSByte();
                        //block[i].m_Statics[k].m_Z -= 40;
                        block[i].m_Statics[k].m_Hue = StaMul.ReadInt16();
                    }
                }
                else
                {
                    block[i].m_Statics = new Static[0];
                }
                
                block[i].m_Header = MapMul.ReadUInt32();
                block[i].m_Tiles = new Tile[64];
                for (int k = 0; k < 64; ++k)
                {
                    block[i].m_Tiles[k].m_ID = MapMul.ReadUInt16();
                    block[i].m_Tiles[k].m_Z = MapMul.ReadSByte();
                    //block[i].m_Tiles[k].m_Z -= 40;
                    ///*
                    if (block[i].m_Tiles[k].m_ID == 168 || block[i].m_Tiles[k].m_ID == 169 || block[i].m_Tiles[k].m_ID == 170 || block[i].m_Tiles[k].m_ID == 171 || block[i].m_Tiles[k].m_ID == 310 || block[i].m_Tiles[k].m_ID == 311)
                    {
                        //*
                        Array.Resize<Ultima.MapWorker.Static>(ref block[i].m_Statics, block[i].m_Statics.Length + 1);
                        //Static[] temp = new Static[block[i].m_Statics.Length + 1];
                        //Array.Copy(block[i].m_Statics, temp, block[i].m_Statics.Length);
                        //block[i].m_Statics = temp;

                        //int a = block[i].m_Statics.Length - 1;
                        //block[i].m_Statics[a].m_ID = 0;
                        //block[i].m_Statics[a].m_Hue = 0;
                        //block[i].m_Statics[a].m_Z = 0;
                        //block[i].m_Statics[a].m_X = (byte)(k / 8);
                        //block[i].m_Statics[a].m_Y = (byte)(k % 8);

                        
                        int a = block[i].m_Statics.Length - 1;
                        block[i].m_Statics[a].m_ID = (ushort)rand.Next(0x1797, 0x179D);
                        block[i].m_Statics[a].m_Hue = 0;
                        block[i].m_Statics[a].m_Z = block[i].m_Tiles[k].m_Z;
                        block[i].m_Statics[a].m_X = (byte)(k % 8);
                        block[i].m_Statics[a].m_Y = (byte)(k / 8);
                        block[i].m_Length += 7;
                        //*/
                        byte alt = bmp.GetPixel(8 * line + k % 8, 8 * i + k / 8).G;
                        block[i].m_Tiles[k].m_ID = (ushort)rand.Next(0x3189, 0x318D);
                        
                        int z = (int)(block[i].m_Tiles[k].m_Z - (int)(30 - rand.Next(0, 7)));
                        if(z < -127)
                            block[i].m_Tiles[k].m_Z = -127;
                        else if(z > 127)
                            block[i].m_Tiles[k].m_Z = 127;
                        else
                            block[i].m_Tiles[k].m_Z = (sbyte)z;
                        //sbyte z = (sbyte)(block[i].m_Tiles[k].m_Z - (sbyte)(30 - rand.Next(0, 7)));//(sbyte)(80 - alt - rand.Next(0, 7));
                        /*
                        if (block[i].m_Tiles[k].m_Z < 0 && z > 0)
                            block[i].m_Tiles[k].m_Z = -127;
                        else if (block[i].m_Tiles[k].m_Z > 0 && z < 0)
                            block[i].m_Tiles[k].m_Z = 127;
                        else
                            block[i].m_Tiles[k].m_Z = z;
                        */
                        //block[i].m_Statics = ocean;
                    }
                    //*/  
                }
                /*
                if (block[i].m_Statics.Length == 64)
                    for (int k = 0; k < 64; ++k)
                    {
                        if (block[i].m_Statics[k].m_X != (byte)(k % 8) || block[i].m_Statics[k].m_Y != (byte)(k / 8))
                            break;
                        if (block[i].m_Statics[k].m_ID != 0x1797 && block[i].m_Statics[k].m_ID != 0x1798 && block[i].m_Statics[k].m_ID != 0x1799 && block[i].m_Statics[k].m_ID != 0x179A && block[i].m_Statics[k].m_ID != 0x179B && block[i].m_Statics[k].m_ID != 0x179C)
                            break;
                        if (k == 63)
                        {
                            block[i].m_Ocean = true;
                            block[i].m_Statics = ocean;
                        }
                    }
                */
            }

            ++line;
            return block;
        }

        public Block[] ReadLine1()
        {
            if (bmp == null)
                bmp = new Bitmap(@"C:\UltimaOnline\source\_build\uoFiddler\altmap.bmp");

            if (MapMul.BaseStream.Position == MapMul.BaseStream.Length - 1)
                return null;

            Static[] ocean = new Static[64];
            for (int k = 0; k < 64; ++k)
            {
                ocean[k].m_ID = (ushort)rand.Next(0x1797, 0x179D);
                ocean[k].m_Hue = 0;
                ocean[k].m_X = (byte)(k / 8);
                ocean[k].m_Y = (byte)(k % 8);
                ocean[k].m_Z = -45;
            }

            Block[] block = new Block[Height >> 3];
            for (int i = 0; i < (Height >> 3); ++i)
            {
                block[i] = new Block();
                block[i].m_Ocean = false;

                block[i].m_Offset = StaIdx.ReadUInt32();
                block[i].m_Length = StaIdx.ReadUInt32();
                block[i].m_Unknown = StaIdx.ReadUInt32();

                if (block[i].m_Offset != 0xFFFFFFFF && block[i].m_Length != 0)
                {
                    StaMul.BaseStream.Position = block[i].m_Offset;
                    block[i].m_Statics = new Static[block[i].m_Length / 7];
                    for (int k = 0; k < block[i].m_Length / 7; ++k)
                    {
                        block[i].m_Statics[k].m_ID = StaMul.ReadUInt16();
                        if (block[i].m_Statics[k].m_ID > 0x6500)
                            block[i].m_Statics[k].m_ID += 0x7000;
                        block[i].m_Statics[k].m_X = StaMul.ReadByte();
                        block[i].m_Statics[k].m_Y = StaMul.ReadByte();
                        block[i].m_Statics[k].m_Z = StaMul.ReadSByte();
                        block[i].m_Statics[k].m_Z -= 40;
                        block[i].m_Statics[k].m_Hue = StaMul.ReadInt16();
                    }
                }
                else
                {
                    block[i].m_Statics = new Static[0];
                }

                block[i].m_Header = MapMul.ReadUInt32();
                block[i].m_Tiles = new Tile[64];
                for (int k = 0; k < 64; ++k)
                {
                    block[i].m_Tiles[k].m_ID = MapMul.ReadUInt16();
                    block[i].m_Tiles[k].m_Z = MapMul.ReadSByte();
                    block[i].m_Tiles[k].m_Z -= 40;
                    ///*
                    if (block[i].m_Tiles[k].m_ID == 168 || block[i].m_Tiles[k].m_ID == 169 || block[i].m_Tiles[k].m_ID == 170 || block[i].m_Tiles[k].m_ID == 171 || block[i].m_Tiles[k].m_ID == 310 || block[i].m_Tiles[k].m_ID == 311)
                    {
                        //*
                        Array.Resize<Ultima.MapWorker.Static>(ref block[i].m_Statics, block[i].m_Statics.Length + 1);
                        int a = block[i].m_Statics.Length - 1;
                        block[i].m_Statics[a].m_ID = (ushort)rand.Next(0x1797, 0x179D);
                        block[i].m_Statics[a].m_Hue = 0;
                        block[i].m_Statics[a].m_Z = block[i].m_Tiles[k].m_Z;
                        block[i].m_Statics[a].m_X = (byte)(k % 8);
                        block[i].m_Statics[a].m_Y = (byte)(k / 8);
                        block[i].m_Length += 7;
                        //*/
                        byte alt = bmp.GetPixel(8 * line + k % 8, 8 * i + k / 8).G;
                        block[i].m_Tiles[k].m_ID = (ushort)rand.Next(0x3189, 0x318D);

                        int z = (int)((int)block[i].m_Tiles[k].m_Z - (int)(80 - alt - rand.Next(0, 7)));
                        if (z < -127)
                            block[i].m_Tiles[k].m_Z = -127;
                        else if (z > 127)
                            block[i].m_Tiles[k].m_Z = 127;
                        else
                            block[i].m_Tiles[k].m_Z = (sbyte)z;
                    }
                    //*/  
                }                
                if (block[i].m_Statics.Length == 64)
                    for (int k = 0; k < 64; ++k)
                    {
                        if (block[i].m_Statics[k].m_X != (byte)(k % 8) || block[i].m_Statics[k].m_Y != (byte)(k / 8))
                            break;
                        if (block[i].m_Statics[k].m_ID != 0x1797 && block[i].m_Statics[k].m_ID != 0x1798 && block[i].m_Statics[k].m_ID != 0x1799 && block[i].m_Statics[k].m_ID != 0x179A && block[i].m_Statics[k].m_ID != 0x179B && block[i].m_Statics[k].m_ID != 0x179C)
                            break;
                        if (k == 63)
                        {
                            block[i].m_Ocean = true;
                            block[i].m_Statics = ocean;
                        }
                    }
            }

            ++line;
            return block;
        }

        private uint m_OceanOffset = 0xFFFFFFFF;
        private uint m_OceanLength = 0;
        private bool m_OceanWrited = false;
        private Random rand = new Random();

        public void Write0(Block[] block)
        {            
            for (int i = 0; i < block.Length; ++i)
            {
                MapMulW.Write(block[i].m_Header);
                for (int k = 0; k < 64; ++k)
                {
                    MapMulW.Write(block[i].m_Tiles[k].m_ID);
                    MapMulW.Write(block[i].m_Tiles[k].m_Z);
                }

                if (block[i].m_Statics == null || block[i].m_Statics.Length == 0)
                {
                    block[i].m_Offset = 0xFFFFFFFF;
                    block[i].m_Length = 0;
                }
                else
                {
                    block[i].m_Offset = (uint)StaMulW.BaseStream.Position;
                    block[i].m_Length = (uint)(7 * block[i].m_Statics.Length);
                }
                StaIdxW.Write(block[i].m_Offset);
                StaIdxW.Write(block[i].m_Length);
                StaIdxW.Write(block[i].m_Unknown);

                if (block[i].m_Offset == 0xFFFFFFFF || block[i].m_Length == 0)
                    continue;

                for (int k = 0; k < block[i].m_Statics.Length; ++k)
                {
                    StaMulW.Write(block[i].m_Statics[k].m_ID);
                    StaMulW.Write(block[i].m_Statics[k].m_X);
                    StaMulW.Write(block[i].m_Statics[k].m_Y);
                    StaMulW.Write(block[i].m_Statics[k].m_Z);
                    StaMulW.Write(block[i].m_Statics[k].m_Hue);
                }
            }
        }

        public void Write1(Block[] block)
        {
            if(!m_OceanWrited)
            {
                StaMulW.BaseStream.Position = 0;
                m_OceanOffset = (uint)StaMulW.BaseStream.Position;
                m_OceanLength = (uint)(7 * 64);
                Static[] ocean = new Static[64];
                for (int k = 0; k < 64; ++k)
                {
                    ocean[k].m_ID = (ushort)rand.Next(0x1797, 0x179D);
                    ocean[k].m_Hue = 0;
                    ocean[k].m_X = (byte)(k % 8);
                    ocean[k].m_Y = (byte)(k / 8);
                    ocean[k].m_Z = -45;

                    StaMulW.Write(ocean[k].m_ID);
                    StaMulW.Write(ocean[k].m_X);
                    StaMulW.Write(ocean[k].m_Y);
                    StaMulW.Write(ocean[k].m_Z);
                    StaMulW.Write(ocean[k].m_Hue);
                }
                m_OceanWrited = true;
            }

            for (int i = 0; i < block.Length; ++i)
            {
                MapMulW.Write(block[i].m_Header);
                for (int k = 0; k < 64; ++k)
                {
                    MapMulW.Write(block[i].m_Tiles[k].m_ID);
                    MapMulW.Write(block[i].m_Tiles[k].m_Z);
                }

                if (block[i].m_Statics == null || block[i].m_Statics.Length == 0)
                {
                    block[i].m_Offset = 0xFFFFFFFF;
                    block[i].m_Length = 0;
                }
                else if (block[i].m_Ocean)
                {
                    block[i].m_Offset = ((m_OceanOffset != 0xFFFFFFFF) ? m_OceanOffset : (m_OceanOffset = (uint)StaMulW.BaseStream.Position));
                    block[i].m_Length = ((m_OceanLength != 0) ? m_OceanLength : (m_OceanLength = (uint)(7 * 64)));
                }
                else
                {
                    block[i].m_Offset = (uint)StaMulW.BaseStream.Position;
                    block[i].m_Length = (uint)(7 * block[i].m_Statics.Length);
                }
                StaIdxW.Write(block[i].m_Offset);
                StaIdxW.Write(block[i].m_Length);
                StaIdxW.Write(block[i].m_Unknown);

                if (block[i].m_Offset == 0xFFFFFFFF || block[i].m_Length == 0)
                    continue;

                if (block[i].m_Ocean && m_OceanWrited)
                    continue;

                for (int k = 0; k < block[i].m_Statics.Length; ++k)
                {
                    StaMulW.Write(block[i].m_Statics[k].m_ID);
                    StaMulW.Write(block[i].m_Statics[k].m_X);
                    StaMulW.Write(block[i].m_Statics[k].m_Y);
                    StaMulW.Write(block[i].m_Statics[k].m_Z);
                    StaMulW.Write(block[i].m_Statics[k].m_Hue);
                }

                if (block[i].m_Ocean && !m_OceanWrited)
                    m_OceanWrited = true;
            }

            //m_OceanWrited = false;
        }
    }
}
