/***************************************************************************
 *                                               Created by :        StaticZ
 *                    Facet.cs                   UO Quintessense server team
 *              ____________________             url   :   http://uoquint.ru
 *              Version : 01/08/2010             email :    staff@uoquint.ru
 *                                               ---------------------------
 * History :
 *   01/08/2010 First realize
 *
 * Todo :
 *   add new FacetGenAlgorithm generates facet in osi style
 ***************************************************************************/

using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Collections.Generic;

namespace Ultima
{
    public enum FacetGenAlgorithm : byte
    {
        /// <summary>
        /// Algorithm based on using Ultima.Map.GetImage() method, to generate simple style facet.
        /// Note recomended to use, because of too much memmory requiment.
        /// </summary>
        UltimaMap = 0x00,

        /// <summary>
        /// Optimised realization of UltimaMap simple algorithm, to generate simple style facet.
        /// Little faster then UltimaMap.
        /// </summary>
        Simple = 0x01,

        /// <summary>
        /// Based on Simple algorithm. Generates simple style facet, with using altitude mask.
        /// </summary>
        AltMask = 0x02,

        /// <summary>
        /// Based on AltMask algorithm. Generates simple style facet, with using altitude mask and noises effect.
        /// </summary>
        AltMaskNoise = 0x03,

        /// <summary>
        /// Improved algorithm generates osi style facet.
        /// It's not exactly osi algorithm and is the slowest one.
        /// </summary>
        Improved = 0x04
    }

    public sealed class Facet
    {
        /// <summary>
        /// Returns Felucca Facet class (facet00.mul)
        /// </summary>
        public static Facet Felucca
        {
            get { return m_Felucca ?? (m_Felucca = new Facet(0)); }
        }
        private static Facet m_Felucca = null;

        /// <summary>
        /// Returns Trammel Facet class (facet01.mul)
        /// </summary>
        public static Facet Trammel
        {
            get { return m_Trammel ?? (m_Trammel = new Facet(1)); }
        }
        private static Facet m_Trammel = null;

        /// <summary>
        /// Returns Ilshenar Facet class (facet02.mul)
        /// </summary>
        public static Facet Ilshenar
        {
            get { return m_Ilshenar ?? (m_Ilshenar = new Facet(2)); }
        }
        private static Facet m_Ilshenar = null;

        /// <summary>
        /// Returns Malas Facet class (facet03.mul)
        /// </summary>
        public static Facet Malas
        {
            get { return m_Malas ?? (m_Malas = new Facet(3)); }
        }
        private static Facet m_Malas = null;

        /// <summary>
        /// Return Tokuno Facet class (facet04.mul)
        /// </summary>
        public static Facet Tokuno
        {
            get { return m_Tokuno ?? (m_Tokuno = new Facet(4)); }
        }
        private static Facet m_Tokuno = null;

        /// <summary>
        /// Return TerMur Facet class (facet05.mul)
        /// </summary>
        public static Facet TerMur
        {
            get { return m_TerMur ?? (m_TerMur = new Facet(5)); }
        }
        private static Facet m_TerMur = null;

        /// <summary>
        /// Return uoquint.ru Dangeon Facet class (facet00.mul)
        /// </summary>
        public static Facet Dangeon
        {
            get { return m_Dangeon ?? (m_Dangeon = new Facet(0)); }
        }
        private static Facet m_Dangeon = null;

        /// <summary>
        /// Return uoquint.ru Sosaria Facet class (facet01.mul)
        /// </summary>
        public static Facet Sosaria
        {
            get { return m_Sosaria ?? (m_Sosaria = new Facet(1)); }
        }
        private static Facet m_Sosaria = null;

        public static Facet Custom
        {
            get { return m_Custom; }
            set { m_Custom = value; }
        }
        private static Facet m_Custom = null;

        /// <summary>
        /// File index of facet class, also used for cread default FileName if it's null
        /// </summary>
        public int FileIndex
        {
            get { return m_FileIndex; }
        }
        private int m_FileIndex = -1;

        /// <summary>
        /// Full path to file from what Facet was created or default path to Save()
        /// </summary>
        public string FileName
        {
            get { return m_FileName; }
        }
        private string m_FileName = null;

        /// <summary>
        /// Width of Facing (in tiles) and/or Bitmap (in pixels)
        /// </summary>
        public int Width
        {
            get { return m_Width; }
        }
        private int m_Width = -1;

        /// <summary>
        /// Height of Facing (in tiles) and/or Bitmap (in pixels)
        /// </summary>
        public int Height
        {
            get { return m_Height; }
        }
        private int m_Height = -1;

        /// <summary>
        /// Facet Image, avilible only after Preload() or creating Facet class from Bitmap or Map.
        /// In other cases it will return null.
        /// Note: Bitmap always has X1R5G5B5 pixel format.
        /// </summary>
        public Bitmap Bitmap
        {
            get { return m_Bitmap; }
        }
        private Bitmap m_Bitmap = null;

        private BinaryReader bin = null;
        private uint[] offset = null;
        private uint[] length = null;

        // nodraw tiles
        private static readonly ushort[] nodrawland = new ushort[] { 0x0002, 0x01AF, 0x01B0, 0x01B1, 0x01B2, 0x01B3, 0x01B4, 0x01B5,
                                                                     0x0244, 0x01AE, 0x01DB }; // lasts is NoName(BlackTiles)
        private static readonly ushort[] nodrawitem = new ushort[] { 0x0001, 0x2198, 0x2199, 0x219A, 0x219B, 0x219C, 0x219D, 0x219E, 
                                                                     0x219F, 0x21A0, 0x21A1, 0x21A2, 0x21A3, 0x21A4, 0x21BC, 0x5690,
                                                                     0x0490 }; // last is BlackTile

        /// <summary>
        /// Initialize (opens for reading) Facet mul file from UO client folder
        /// </summary>
        /// <param name="fileindex">file index of facetID.mul file</param>
        public Facet(int fileindex)
        {
            if (fileindex < 0 || fileindex > 99)
                throw new ArgumentOutOfRangeException();

            m_FileName = Files.GetFilePath(String.Format("facet{0:D2}.mul", fileindex));
            if (String.IsNullOrEmpty(FileName))
                throw new Exception(String.Format("File: \"facet{0:D2}.mul\" wasn't found.", fileindex));

            m_FileIndex = fileindex;
            Initialize();
            Reload();
        }

        /// <summary>
        /// Initialize (opens for reading) selected Facet mul file
        /// </summary>
        /// <param name="fileindex">full path to facetID.mul file</param>
        public unsafe Facet(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentNullException();

            if (m_FileIndex < 0 && filename.Length >= 6)
            {
                string number = filename.Substring(filename.Length - 6, 2);
                if (Int32.TryParse(number, out m_FileIndex)) ;
                if (m_FileIndex < 0 || m_FileIndex > 99)
                    m_FileIndex = -1;
            }

            m_FileName = filename;
            Initialize();
            Reload();
        }

        private void Initialize()
        {
            if (String.Compare(Path.GetExtension(FileName), ".mul", true) != 0)
                throw new Exception("Unknown facet file extension.");

            if (!File.Exists(FileName))
                throw new Exception(String.Format("File: \"{0}\" doesn't exists.", FileName));

            try
            {
                bin = new BinaryReader(new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read));
            }
            catch
            {
                throw new IOException(String.Format("IO Error occures while openning file: \"{0}\".", FileName));
            }
        }

        /// <summary>
        /// Free Bitmap memmory and realise open files
        /// </summary>
        public void Dispose()
        {
            if (Bitmap != null)
            {
                Bitmap.Dispose();
                m_Bitmap = null;
            }

            if (bin != null)
            {
                bin.BaseStream.Dispose();
                bin.Close();
                bin = null;
            }
        }

        /// <summary>
        /// Reload facetId.mul file. 
        /// Important: for facet classes created in memmory, not from file, it will realise created facet.
        /// Note: if you want to use preload Facet, you have to invoke Preload() method again after calling Reload() method.
        /// </summary>
        public unsafe void Reload()
        {
            if (bin == null)
                Initialize();

            if (bin.BaseStream.Length <= 8)
                return; // empty file

            if (Bitmap != null)
            {
                Bitmap.Dispose();
                m_Bitmap = null;
            }

            bin.BaseStream.Seek(0, SeekOrigin.Begin);
            m_Width = bin.ReadUInt16();
            m_Height = bin.ReadUInt16();
            offset = new uint[Height];
            length = new uint[Height];

            uint size = bin.ReadUInt32();
            offset[0] = 8;
            length[0] = size / 3;

            for (int h = 1; h < m_Height; ++h)
            {
                bin.BaseStream.Seek(size, SeekOrigin.Current);

                offset[h] = offset[h - 1] + size + 4;
                size = bin.ReadUInt32();
                length[h] = size / 3;
            }
        }

        /// <summary>
        /// Preload facetId.mul file in memmory, so its work faster, but use more memmory.
        /// Important: for facet classes created in memmory, not from file, it will realise created facet.
        /// </summary>
        public unsafe void Preload()
        {
            if (bin == null)
                throw new NullReferenceException();

            bin.BaseStream.Seek(4, SeekOrigin.Begin);
            m_Bitmap = new Bitmap(Width, Height, PixelFormat.Format16bppRgb555);
            BitmapData data = m_Bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, PixelFormat.Format16bppRgb555);
            ushort* cur = (ushort*)data.Scan0;
            int stride = data.Stride >> 1;

            for (int h = 0; h < Height; ++h)
            {
                int x = -1;
                bin.BaseStream.Seek(4, SeekOrigin.Current);
                for (uint k = 0; k < length[h]; ++k)
                {
                    byte count = bin.ReadByte();
                    ushort color = bin.ReadUInt16();
                    for (byte i = 0; i < count; ++i)
                        cur[++x] = color;
                }
                cur += stride;
            }

            m_Bitmap.UnlockBits(data);
        }

        /// <summary>
        /// Return part of facet image
        /// </summary>
        /// <param name="x">x coordinate top-left corner of rectangle</param>
        /// <param name="y">y coordinate top-left corner of rectangle</param>
        /// <param name="width">width of rectangle</param>
        /// <param name="height">height of rectangle</param>
        /// <returns>Bitmap with rectangle of facet in X1R5G5B5 format</returns>
        public Bitmap GetImage(int x, int y, int width, int height)
        {
            return GetImage(new Rectangle(x, y, width, height));
        }

        /// <summary>
        /// Return part of facet image
        /// </summary>
        /// <param name="rect">facet rectangle</param>
        /// <returns>Bitmap with rectangle of facet in X1R5G5B5 format</returns>
        public unsafe Bitmap GetImage(Rectangle rect)
        {
            if (rect.X < 0 || rect.Y < 0 || rect.X + rect.Width > Width || rect.Y + rect.Height > Height)
                throw new ArgumentOutOfRangeException();

            if (Bitmap != null)
                return Bitmap.Clone(rect, PixelFormat.Format16bppRgb555);

            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format16bppRgb555);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, rect.Width, rect.Height), ImageLockMode.WriteOnly, PixelFormat.Format16bppRgb555);
            ushort* cur = (ushort*)data.Scan0;
            int stride = data.Stride >> 1;

            for (int h = rect.Y; h < rect.Y + rect.Height; ++h)
            {
                int x = -1;
                uint pixels = 0;
                bin.BaseStream.Seek(offset[h], SeekOrigin.Begin);
                for (uint k = 0; k < length[h]; ++k)
                {
                    byte count = bin.ReadByte();
                    if (pixels < rect.X)
                    {
                        pixels += count;
                        if (pixels < rect.X)
                            count = 0;
                        else
                            count = (byte)(pixels - rect.X);
                    }
                    else if (x + count >= rect.Width)
                    {
                        count = (byte)(rect.Width - x);
                        if (--count == 0)
                            break;
                    }

                    ushort color = bin.ReadUInt16();
                    for (byte i = 0; i < count; ++i)
                        cur[++x] = color;
                }
                cur += stride;
            }

            bmp.UnlockBits(data);
            return bmp;
        }

        /// <summary>
        /// Create in memmory new Facet from image bitmap
        /// </summary>
        /// <param name="fileindex">index of new Facet</param>
        /// <param name="bitmap">source image to create Facet class from</param>
        public Facet(int fileindex, Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException();

            if (fileindex < 0 || fileindex > 99)
                throw new ArgumentOutOfRangeException();
            m_FileIndex = fileindex;
            m_FileName = Files.GetFilePath(String.Format("facet{0:D2}.mul", fileindex));

            m_Width = bitmap.Width;
            m_Height = bitmap.Height;
            m_Bitmap = bitmap.Clone(new Rectangle(0, 0, Width, Height), PixelFormat.Format16bppRgb555);
        }

        /// <summary>
        /// Generate in memmory new Facet from map & statics mul files
        /// More fast and much less memmory needed than Map.GetImage()
        /// </summary>
        /// <param name="map">source map for generating facet</param>
        public Facet(Map map) : this(map, FacetGenAlgorithm.Simple)
        {
        }

        /// <summary>
        /// Generate in memmory new Facet from map & statics mul files		
        /// </summary>
        /// <param name="map">source map for generating facet</param>
        /// <param name="algorithm">type of algorithm which will be used for converting map to facet</param>
        public unsafe Facet(Map map, FacetGenAlgorithm algorithm)
        {
            if (map == null)
                throw new ArgumentNullException();

            m_Width = map.Width;
            m_Height = map.Height;
            m_FileIndex = map.FileIndex;
            m_FileName = Files.GetFilePath(String.Format("facet{0:D2}.mul", FileIndex));

            #region FacetGenAlgorithm.UltimaMap
            if (algorithm == FacetGenAlgorithm.UltimaMap)
            {
                Bitmap bmp = map.GetImage(0, 0, Width, Height, true);
                m_Bitmap = bmp.Clone(new Rectangle(0, 0, Width, Height), PixelFormat.Format16bppRgb555);
                bmp.Dispose();
                return;
            }
            #endregion FacetGenAlgorithm.UltimaMap

            m_Bitmap = new Bitmap(Width, Height, PixelFormat.Format16bppRgb555);
            BitmapData data = m_Bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, PixelFormat.Format16bppRgb555);
            ushort* cur = (ushort*)data.Scan0;
            int stride = data.Stride >> 1;

            switch (algorithm)
            {
                #region FacetGenAlgorithm.Simple
                case FacetGenAlgorithm.Simple:
                    {
                        TileMatrix matrix = new TileMatrix(FileIndex, FileIndex, Width, Height, null);
                        int blocklength = Width >> 3;
                        Tile[][] land = new Tile[blocklength][];
                        HuedTile[][][][] item = new HuedTile[blocklength][][][];
                        HuedTileComparer comparator = new HuedTileComparer();

                        for (int h = 0; h < (Height >> 3); ++h)
                        {
                            for (int b = 0; b < blocklength; ++b)
                            {
                                land[b] = matrix.GetLandBlock(b, h);
                                item[b] = matrix.GetStaticBlock(b, h);
                            }

                            for (int y = 0; y < 8; ++y)
                            {
                                int i = -1;
                                for (int b = 0; b < blocklength; ++b)
                                {
                                    for (int x = 0; x < 8; ++x)
                                    {
                                        Tile landtile = land[b][(y << 3) + x];
                                        HuedTile[] itemtile = item[b][x][y];
                                        if (itemtile == null || itemtile.Length == 0)
                                        {
                                            if (Array.BinarySearch(nodrawland, landtile.ID) < 0)
                                                cur[++i] = (ushort)RadarCol.Colors[landtile.ID];
                                            else
                                                cur[++i] = 0x0000;
                                            continue;
                                        }

                                        Array.Sort(itemtile, comparator);

                                        cur[++i] = itemtile[0].Z < landtile.Z
                                                 ? (ushort)RadarCol.Colors[landtile.ID]
                                                 : (ushort)RadarCol.Colors[0x4000 + itemtile[0].ID];

                                        if (cur[i] == 0x0421 || cur[i] == 0x0000)
                                            if (Array.BinarySearch(nodrawland, landtile.ID) < 0)
                                                cur[++i] = (ushort)RadarCol.Colors[landtile.ID];
                                            else
                                                cur[++i] = 0x0000;
                                    }
                                }
                                cur += stride;
                            }
                        }
                        break;
                    }
                #endregion FacetGenAlgorithm.Simple

                #region FacetGenAlgorithm.AltMask
                case FacetGenAlgorithm.AltMask:
                    {
                        TileMatrix matrix = new TileMatrix(FileIndex, FileIndex, Width, Height, null);
                        int blocklength = Width >> 3;
                        Tile[][] land = new Tile[blocklength][];
                        HuedTile[][][][] item = new HuedTile[blocklength][][][];
                        HuedTileComparer comparator = new HuedTileComparer();

                        for (int h = 0; h < (Height >> 3); ++h)
                        {
                            for (int b = 0; b < blocklength; ++b)
                            {
                                land[b] = matrix.GetLandBlock(b, h);
                                item[b] = matrix.GetStaticBlock(b, h);
                            }

                            matrix.Dispose();

                            for (int y = 0; y < 8; ++y)
                            {
                                int i = -1;
                                for (int b = 0; b < blocklength; ++b)
                                {
                                    for (int x = 0; x < 8; ++x)
                                    {
                                        Tile landtile = land[b][(y << 3) + x];
                                        HuedTile[] itemtile = item[b][x][y];
                                        bool drawland = false;

                                        if (itemtile == null || itemtile.Length == 0)
                                        {
                                            if (Array.BinarySearch(nodrawland, landtile.ID) >= 0)
                                                cur[++i] = 0x0000;
                                            else
                                            {
                                                cur[++i] = (ushort)RadarCol.Colors[landtile.ID];
                                                drawland = true;
                                            } 
                                        }
                                        else
                                        {
                                            Array.Sort(itemtile, comparator);

                                            ushort landcolor = (ushort)RadarCol.Colors[landtile.ID];
                                            ushort itemcolor = 0x0000;
                                            int iu = 0;
                                            for (int u = 0; u < itemtile.Length; ++u)
                                            {
                                                iu = u;
                                                if (Array.BinarySearch(nodrawitem, itemtile[u].ID) >= 0)
                                                    continue;
                                                itemcolor = (ushort)RadarCol.Colors[0x4000 + itemtile[u].ID];
                                                if (itemcolor == 0x0000 || itemcolor == 0x0421)
                                                    if ((u + 1 < itemtile.Length) && (itemtile[u].Z >= landtile.Z))
                                                        continue;
                                                    else
                                                    {
                                                        itemcolor = 0x0000;
                                                        break;
                                                    }
                                                break;
                                            }

                                            if (itemcolor == 0x0000 || itemtile[iu].Z < landtile.Z)
                                            {
                                                if (Array.BinarySearch(nodrawland, landtile.ID) >= 0)
                                                    cur[++i] = 0x0000;
                                                else
                                                {
                                                    cur[++i] = landcolor;
                                                    drawland = true;
                                                } 
                                            }
                                            else
                                                cur[++i] = itemcolor;
                                        }

                                        if (drawland && ((TileData.LandTable[landtile.ID].Flags & TileFlag.Wet) == 0))
                                        {
                                            short inc = (short)(0 - (landtile.Z / 20));

                                            short maskR = (short)(((cur[i] & 0x7C00) >> 10) + inc);
                                            maskR = Math.Min(Math.Max((short)0x00, maskR), (short)0x1F);
                                            short maskG = (short)(((cur[i] & 0x03E0) >> 5) + inc);
                                            maskG = Math.Min(Math.Max((short)0x00, maskG), (short)0x1F);
                                            short maskB = (short)(((cur[i] & 0x001F) >> 0) + inc);
                                            maskB = Math.Min(Math.Max((short)0x00, maskB), (short)0x1F);

                                            cur[i] = (ushort)((maskR << 10) | (maskG << 5) | (maskB << 0));
                                        }
                                        else
                                        {
                                            ushort maskR = (ushort)((cur[i] & 0x7C00) >> 10);
                                            if (maskR > 0)
                                                --maskR;
                                            ushort maskG = (ushort)((cur[i] & 0x03E0) >> 5);
                                            if (maskG > 0)
                                                --maskG;
                                            ushort maskB = (ushort)((cur[i] & 0x001F) >> 0);
                                            if (maskB > 0)
                                                --maskB;
                                            cur[i] = (ushort)((maskR << 10) | (maskG << 5) | (maskB << 0));
                                        }
                                    }
                                }
                                cur += stride;
                            }
                        }
                        break;
                    }
                #endregion FacetGenAlgorithm.AltMask

                #region FacetGenAlgorithm.AltMaskNoise
                case FacetGenAlgorithm.AltMaskNoise:
                    {
                        Random random = new Random();
                        TileMatrix matrix = new TileMatrix(FileIndex, FileIndex, Width, Height, null);
                        int blocklength = Width >> 3;
                        Tile[][] land = new Tile[blocklength][];
                        HuedTile[][][][] item = new HuedTile[blocklength][][][];
                        HuedTileComparer comparator = new HuedTileComparer();

                        for (int h = 0; h < (Height >> 3); ++h)
                        {
                            for (int b = 0; b < blocklength; ++b)
                            {
                                land[b] = matrix.ReadLandBlock(b, h);
                                item[b] = matrix.ReadStaticBlock(b, h);
                            }

                            for (int y = 0; y < 8; ++y)
                            {
                                int i = -1;
                                for (int b = 0; b < blocklength; ++b)
                                {
                                    for (int x = 0; x < 8; ++x)
                                    {
                                        Tile landtile = land[b][(y << 3) + x];
                                        HuedTile[] itemtile = item[b][x][y];
                                        bool drawland = false;

                                        if (itemtile == null || itemtile.Length == 0)
                                        {
                                            if (Array.BinarySearch(nodrawland, landtile.ID) >= 0)
                                                cur[++i] = 0x0000;
                                            else
                                            {
                                                cur[++i] = (ushort)RadarCol.Colors[landtile.ID];
                                                drawland = true;
                                            } 
                                        }
                                        else
                                        {
                                            Array.Sort(itemtile, comparator);

                                            ushort landcolor = (ushort)RadarCol.Colors[landtile.ID];
                                            ushort itemcolor = 0x0000;
                                            int iu = 0;
                                            for (int u = 0; u < itemtile.Length; ++u)
                                            {
                                                iu = u;
                                                if (Array.BinarySearch(nodrawitem, itemtile[u].ID) >= 0)
                                                    continue;
                                                itemcolor = (ushort)RadarCol.Colors[0x4000 + itemtile[u].ID];
                                                if (itemcolor == 0x0000 || itemcolor == 0x0421 )
                                                    if ((u + 1 < itemtile.Length) && (itemtile[u].Z >= landtile.Z))
                                                        continue;
                                                    else
                                                    {
                                                        itemcolor = 0x0000;
                                                        break;
                                                    }
                                                break;
                                            }

                                            if (itemcolor == 0x0000 || itemtile[iu].Z < landtile.Z)
                                            {
                                                if (Array.BinarySearch(nodrawland, landtile.ID) >= 0)
                                                    cur[++i] = 0x0000;
                                                else
                                                {
                                                    cur[++i] = landcolor;
                                                    drawland = true;
                                                } 
                                            }
                                            else
                                                cur[++i] = itemcolor;
                                        }

                                        if (drawland && ((TileData.LandTable[landtile.ID].Flags & TileFlag.Wet) == 0))
                                        {
                                            short inc = (short)(0 - (landtile.Z / 25));

                                            short maskR = (short)(((cur[i] & 0x7C00) >> 10) + inc);
                                            short maskG = (short)(((cur[i] & 0x03E0) >> 5) + inc);
                                            short maskB = (short)(((cur[i] & 0x001F) >> 0) + inc);

                                            inc = (short)(random.Next(-1, 2));
                                            maskR += inc;
                                            inc = (short)(random.Next(-1, 2));
                                            maskG += inc;
                                            inc = (short)(random.Next(-1, 2));
                                            maskB += inc;

                                            maskR = Math.Min(Math.Max((short)0x00, maskR), (short)0x1F);
                                            maskG = Math.Min(Math.Max((short)0x00, maskG), (short)0x1F);
                                            maskB = Math.Min(Math.Max((short)0x00, maskB), (short)0x1F);

                                            cur[i] = (ushort)((maskR << 10) | (maskG << 5) | (maskB << 0));
                                        }
                                        else
                                        {
                                            ushort maskR = (ushort)((cur[i] & 0x7C00) >> 10);
                                            if (maskR > 0)
                                                --maskR;
                                            ushort maskG = (ushort)((cur[i] & 0x03E0) >> 5);
                                            if (maskG > 0)
                                                --maskG;
                                            ushort maskB = (ushort)((cur[i] & 0x001F) >> 0);
                                            if (maskB > 0)
                                                --maskB;
                                            cur[i] = (ushort)((maskR << 10) | (maskG << 5) | (maskB << 0));
                                        }
                                    }
                                }
                                cur += stride;
                            }

                        }
                        break;
                    }
                #endregion FacetGenAlgorithm.AltMaskNoise

                #region FacetGenAlgorithm.Improved
                case FacetGenAlgorithm.Improved:
                    {
                        Random random = new Random();
                        TileMatrix matrix = new TileMatrix(FileIndex, FileIndex, Width, Height, null);
                        int blocklength = Width >> 3;
                        Tile[][] land = new Tile[blocklength][];
                        HuedTile[][][][] item = new HuedTile[blocklength][][][];
                        HuedTileComparer comparator = new HuedTileComparer();

                        for (int h = 0; h < (Height >> 3); ++h)
                        {
                            for (int b = 0; b < blocklength; ++b)
                            {
                                land[b] = matrix.GetLandBlock(b, h);
                                item[b] = matrix.GetStaticBlock(b, h);
                            }

                            for (int y = 0; y < 8; ++y)
                            {
                                int i = -1;
                                for (int b = 0; b < blocklength; ++b)
                                {
                                    for (int x = 0; x < 8; ++x)
                                    {
                                        Tile landtile = land[b][(y << 3) + x];
                                        HuedTile[] itemtile = item[b][x][y];
                                        bool drawland = false;

                                        if (itemtile == null || itemtile.Length == 0)
                                        {
                                            cur[++i] = (ushort)RadarCol.Colors[landtile.ID];
                                            drawland = true;
                                        }
                                        else
                                        {
                                            Array.Sort(itemtile, comparator);

                                            ushort landcolor = (ushort)RadarCol.Colors[landtile.ID];
                                            ushort itemcolor = 0x0000;
                                            int iu = 0;
                                            for (int u = 0; u < itemtile.Length; ++u)
                                            {
                                                iu = u;
                                                itemcolor = (ushort)RadarCol.Colors[0x4000 + itemtile[u].ID];
                                                if (itemcolor == 0x0000 || itemcolor == 0x0421)
                                                    if ((u + 1 < itemtile.Length) && (itemtile[u].Z >= landtile.Z))
                                                        continue;
                                                    else
                                                    {
                                                        itemcolor = 0x0000;
                                                        break;
                                                    }
                                                break;
                                            }

                                            if (itemcolor == 0x0000 || itemtile[iu].Z < landtile.Z)
                                            {
                                                cur[++i] = landcolor;
                                                drawland = true;
                                            }
                                            else
                                                cur[++i] = itemcolor;
                                        }

                                        if (drawland)
                                        {
                                            short inc = (short)(0 - (landtile.Z / 20));
                                            //
                                            inc *= 3;
                                            if (inc == 0)
                                                switch (random.Next(0, 2))
                                                {
                                                    case 0: inc = -1; break;
                                                    case 1: inc = 1; break;
                                                }
                                            short dir = (short)((inc >= 0) ? 1 : -1);

                                            short maskR = (short)((cur[i] & 0x7C00) >> 10);
                                            short maskG = (short)((cur[i] & 0x03E0) >> 5);
                                            short maskB = (short)((cur[i] & 0x001F) >> 0);

                                            for (short rand = 0; rand < dir * inc; ++rand)
                                                switch (random.Next(0, 3))
                                                {
                                                    case 0: maskR += dir; break;
                                                    case 1: maskG += dir; break;
                                                    case 2: maskB += dir; break;
                                                }

                                            maskR = Math.Min(Math.Max((short)0x00, maskR), (short)0x1F);
                                            maskG = Math.Min(Math.Max((short)0x00, maskG), (short)0x1F);
                                            maskB = Math.Min(Math.Max((short)0x00, maskB), (short)0x1F);
                                            /*
                                            short maskR = (short)(((cur[i] & 0x7C00) >> 10) + inc);
                                            maskR = Math.Min(Math.Max((short)0x00, maskR), (short)0x1F);
                                            short maskG = (short)(((cur[i] & 0x03E0) >> 5) + inc);
                                            maskG = Math.Min(Math.Max((short)0x00, maskG), (short)0x1F);
                                            short maskB = (short)(((cur[i] & 0x001F) >> 0) + inc);
                                            maskB = Math.Min(Math.Max((short)0x00, maskB), (short)0x1F);
                                            */
                                            cur[i] = (ushort)((maskR << 10) | (maskG << 5) | (maskB << 0));
                                        }
                                        else
                                        {
                                            ushort maskR = (ushort)((cur[i] & 0x7C00) >> 10);
                                            if (maskR > 0)
                                                --maskR;
                                            ushort maskG = (ushort)((cur[i] & 0x03E0) >> 5);
                                            if (maskG > 0)
                                                --maskG;
                                            ushort maskB = (ushort)((cur[i] & 0x001F) >> 0);
                                            if (maskB > 0)
                                                --maskB;
                                            cur[i] = (ushort)((maskR << 10) | (maskG << 5) | (maskB << 0));
                                        }
                                    }
                                }
                                cur += stride;
                            }
                        }
                        break;
                    }
                #endregion FacetGenAlgorithm.Improved
            }

            m_Bitmap.UnlockBits(data);
        }

        /// <summary>
        /// Save curent Facet class to facet mul file, located in FileName.
        /// If FileName is null, it will be created from FileIndex.
        /// </summary>
        public void Save()
        {
            Save(m_FileName);
        }

        /// <summary>
        /// Save curent Facet class to facet mul file.
        /// </summary>
        /// <param name="filename">full path to *.mul file to save Facet/</param>
        public unsafe void Save(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentNullException();

            if (Bitmap == null)
            {
                Preload();
                if (Bitmap == null)
                    throw new NullReferenceException();
            }

            bool update = (bin != null);
            if (update)
            {
                bin.BaseStream.Dispose();
                bin.Close();
                bin = null;
            }

            using (BinaryWriter binw = new BinaryWriter(new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None)))
            {
                binw.Write((ushort)m_Width);
                binw.Write((ushort)m_Height);

                BitmapData data = m_Bitmap.LockBits(new Rectangle(0, 0, m_Width, m_Height), ImageLockMode.WriteOnly, PixelFormat.Format16bppRgb555);
                ushort* cur = (ushort*)data.Scan0;
                int stride = data.Stride >> 1;

                for (int h = 0; h < m_Height; ++h)
                {
                    int x = -1;
                    uint length = 0;
                    binw.Write((uint)0xCDCDCDCD);

                    byte count = 0;
                    ushort color = 0x8000;
                    for (uint w = 0; w < m_Width; ++w)
                    {
                        ushort temp = cur[++x];
                        if (temp == color && count < 0xFF) // есть повторные цвета
                        {
                            ++count;
                            if (count == 0xFF || w == m_Width - 1)	// достигнут предел повторяющихся цветов или конец строки
                            {
                                binw.Write((byte)count);
                                binw.Write((ushort)color);
                            }
                            continue;
                        }
                        else if (count != 0xFF && w != 0) // только в случае если это не первый прогон
                        {
                            binw.Write((byte)count);
                            binw.Write((ushort)color);
                        }

                        ++length;
                        count = 1;
                        color = temp;

                        if (w == m_Width - 1) // только в случае если это не первый прогон
                        {
                            binw.Write((byte)count);
                            binw.Write((ushort)color);
                        }
                    }
                    cur += stride;

                    length *= 3;
                    binw.Seek(-4 - (int)length, SeekOrigin.Current);
                    binw.Write((uint)length);
                    binw.Seek(0, SeekOrigin.End);
                    binw.Flush();
                }

                m_Bitmap.UnlockBits(data);
            }

            if (update)
            {
                Initialize();
                Reload();
            }
        }

        private class HuedTileComparer : IComparer
        {
            public HuedTileComparer()
            {
            }

            public int Compare(object x, object y)
            {
                HuedTile a = (HuedTile)x;
                HuedTile b = (HuedTile)y;

                if (a.Z > b.Z)
                    return -1;
                else if (a.Z == b.Z)
                    return 0;
                else
                    return 1;
            }
        }
    }
}
