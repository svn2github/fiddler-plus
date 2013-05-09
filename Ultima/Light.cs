using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Ultima
{
    public sealed class Light
    {
        private static FileIndex m_FileIndex = new FileIndex("lightidx.mul", "light.mul", 100, -1);
        private static Bitmap[] m_Cache = new Bitmap[100];
        private static bool[] m_Removed = new bool[100];
        private static byte[] m_StreamBuffer;

        /// <summary>
        /// ReReads light.mul
        /// </summary>
        public static void Reload()
        {
            m_FileIndex = new FileIndex("lightidx.mul", "light.mul", 100, -1);
            m_Cache = new Bitmap[100];
            m_Removed = new bool[100];
        }

        /// <summary>
        /// Gets count of definied lights
        /// </summary>
        /// <returns></returns>
        public static int GetCount()
        {
            string idxPath = Files.GetFilePath("lightidx.mul");
            if (idxPath == null)
                return 0;
            using (FileStream index = new FileStream(idxPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return (int)(index.Length / 12);
            }
        }

        /// <summary>
        /// Tests if given index is valid
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool TestLight(int index)
        {
            if (m_Removed[index])
                return false;
            if (m_Cache[index] != null)
                return true;

            int length, extra;
            bool patched;

            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);

            if (stream == null)
                return false;
            stream.Close();
            int width = (extra & 0xFFFF);
            int height = ((extra >> 16) & 0xFFFF);
            if ((width > 0) && (height > 0))
                return true;

            return false;
        }

        /// <summary>
        /// Removes Light <see cref="m_Removed"/>
        /// </summary>
        /// <param name="index"></param>
        public static void Remove(int index)
        {
            m_Removed[index] = true;
        }

        /// <summary>
        /// Replaces Light
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bmp"></param>
        public static void Replace(int index, Bitmap bmp)
        {
            m_Cache[index] = bmp;
            m_Removed[index] = false;
        }

        public unsafe static byte[] GetRawLight(int index, out int width, out int height)
        {
            width = 0;
            height = 0;
            if (m_Removed[index])
                return null;
            int length, extra;
            bool patched;

            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);

            if (stream == null)
                return null;

            width = (extra & 0xFFFF);
            height = ((extra >> 16) & 0xFFFF);
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);
            stream.Close();
            return buffer;
        }
        /// <summary>
        /// Returns Bitmap of given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public unsafe static Bitmap GetLight(int index)
        {
            if (m_Removed[index])
                return null;
            if (m_Cache[index] != null)
                return m_Cache[index];

            int length, extra;
            bool patched;

            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);

            if (stream == null)
                return null;

            int width = (extra & 0xFFFF);
            int height = ((extra >> 16) & 0xFFFF);

            if (m_StreamBuffer == null || m_StreamBuffer.Length < length)
                m_StreamBuffer = new byte[length];
            stream.Read(m_StreamBuffer, 0, length);

            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format16bppArgb1555);
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);

            ushort* line = (ushort*)bd.Scan0;
            int delta = bd.Stride >> 1;

            fixed (byte* data = m_StreamBuffer)
            {
                sbyte* bindat = (sbyte*)data;
                for (int y = 0; y < height; ++y, line += delta)
                {
                    ushort* cur = line;
                    ushort* end = cur + width;

                    while (cur < end)
                    {
                        sbyte value = *bindat++;   // диапазон оси [-30; 31], прозрачный цвет = 0
                        // чертов извращенец посути хранит значение маски в первых 6 байтах (т.е. в каналах a + r, причем a = 0 посути означает отрицательное число), 
                        // ну а в случае если значение больше нуля, то оно оказывается еще и на единицу больше, изза переноса старшего разряда из младших слагаемых (b, g)
                        // Для осветления 0 - прозрачный цвет, градиент осветления от 1 до 31. Для затемнения прозрачный цвет 1, градиент от 0 до -30 (а может и от -1 или -2)
                        *cur++ = (ushort)(((0x1f + value) << 10) + ((0x1F + value) << 5) + (0x1F + value));
                    }
                }
            }

            bmp.UnlockBits(bd);
            stream.Close();
            if (!Files.CacheData)
                return m_Cache[index] = bmp;
            else
                return bmp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="light">Light Bitmap in ARGB1555 format</param>
        /// <returns>Grayscale light image in XRGB555 format</returns>
        public unsafe static Bitmap GetImage(Bitmap light)
        {
            int typo = 0;
            Bitmap img = new Bitmap(light.Width, light.Height, PixelFormat.Format16bppRgb555);

            BitmapData img_bd = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.WriteOnly, PixelFormat.Format16bppRgb555);
            BitmapData bit_bd = light.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
               
            int img_delta = img_bd.Stride >> 1;
            int bit_delta = bit_bd.Stride >> 1;

            ushort* img_line = (ushort*)img_bd.Scan0;
            ushort* bit_line = (ushort*)bit_bd.Scan0;

            for (int Y = 0; Y < img.Height; ++Y, bit_line += bit_delta) {
                ushort* bit_cur = bit_line;
                ushort* bit_end = bit_cur + img.Width;
                while (bit_cur < bit_end) {
                    if (*bit_cur != 0x7FFF && *bit_cur != 0x8420)
                        typo += (*bit_cur & 0x8000) > 0 ? +1 : -1;
                    else
                        typo += (*bit_cur) == 0x7FFF ? +1 : -1;
                    ++bit_cur;
                }
            }

            bit_line = (ushort*)bit_bd.Scan0;
            for (int Y = 0; Y < img.Height; ++Y, img_line += img_delta, bit_line += bit_delta) {
                ushort* img_cur = img_line;
                ushort* bit_cur = bit_line;

                ushort* img_end = img_cur + img.Width;
                while (img_cur < img_end) {
                    if (*bit_cur == 0x7FFF || *bit_cur == 0x8420)
                        *img_cur = (ushort)(typo < 0 ? 0xFFFF : 0x8000);
                    else {
                        if ((typo < 0 && (*bit_cur & 0x8000) > 0) || (typo > 0 && (*bit_cur & 0x8000) == 0))
                            *img_cur = (ushort)(*bit_cur & 0x7C00); // 0xFFE0);
                        else
                            *img_cur = *bit_cur;

                        if ((*bit_cur & 0x8000) > 0)
                            *img_cur += 1;
                        else
                            *img_cur |= 0x8000;
                    }
                    ++img_cur;
                    ++bit_cur;
                }
            }

            light.UnlockBits(bit_bd);
            img.UnlockBits(img_bd);
            return img;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="light">Grayscale light image in XRGB555 format</param>
        /// <returns>Light Bitmap in ARGB1555 format</returns>
        public unsafe static Bitmap GetLight(Bitmap image)
        {
            int typo = 0;
            Bitmap img = image;// new Bitmap(image.Width, image.Height, PixelFormat.Format16bppRgb555);
            Bitmap bit = new Bitmap(image.Width, image.Height, PixelFormat.Format16bppArgb1555);
            //using (Graphics g = Graphics.FromImage(img)) {
            //    g.DrawImage(image, 0, 0);
            //}

            BitmapData img_bd = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);//PixelFormat.Format16bppRgb555);
            BitmapData bit_bd = bit.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
               
            int img_delta = img_bd.Stride >> 1;
            int bit_delta = bit_bd.Stride >> 1;

            ushort* img_line = (ushort*)img_bd.Scan0;
            ushort* bit_line = (ushort*)bit_bd.Scan0;

            for (int Y = 0; Y < img.Height; ++Y, img_line += img_delta) {
                ushort* img_cur = img_line;
                ushort* img_end = img_cur + img.Width;
                while (img_cur < img_end) {
                    typo += (*img_cur & 0x7FFF) == 0x0000 ? +1 : (*img_cur & 0x7FFF) == 0x7FFF ? - 1 : 0;
                    ++img_cur;
                }
            }

            img_line = (ushort*)img_bd.Scan0;
            for (int Y = 0; Y < img.Height; ++Y, img_line += img_delta, bit_line += bit_delta) {
                ushort* img_cur = img_line;
                ushort* bit_cur = bit_line;

                ushort* img_end = img_cur + img.Width;
                while (img_cur < img_end) {
                    if (typo >= 0 && (*img_cur & 0x7FFF) == 0x0000)
                        *bit_cur = 0x7FFF;
                    else if (typo < 0 && (*img_cur & 0x7FFF) == 0x7FFF)
                        *bit_cur = 0x8420;
                    else {
                        *bit_cur = (ushort)(*img_cur & 0x7FFF);
                        if (typo >= 0) {
                            *bit_cur |= 0x8000;
                            if ((*bit_cur & 0x7C00) == ((*bit_cur << 10) & 0x7C00))
                                *bit_cur -= 1;
                        }       
                    }

                    ++img_cur;
                    ++bit_cur;
                }
            }

            bit.UnlockBits(bit_bd);
            img.UnlockBits(img_bd);
            return bit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="light">Grayscale light image in XRGB555 format</param>
        public unsafe static void FixLight(ref Bitmap light)
        {
            int typo = 0;
            BitmapData bit_bd = light.LockBits(new Rectangle(0, 0, light.Width, light.Height), ImageLockMode.ReadWrite, PixelFormat.Format16bppArgb1555);
               
            int bit_delta = bit_bd.Stride >> 1;
            ushort* bit_line = (ushort*)bit_bd.Scan0;

            for (int Y = 0; Y < light.Height; ++Y, bit_line += bit_delta) {
                ushort* bit_cur = bit_line;
                ushort* bit_end = bit_cur + light.Width;
                while (bit_cur < bit_end) {
                    if (*bit_cur != 0x7FFF && *bit_cur != 0x8420)
                        typo += (*bit_cur & 0x8000) > 0 ? +1 : -1;
                    else
                        typo += (*bit_cur) == 0x7FFF ? +1 : -1;
                    ++bit_cur;
                }
            }

            bit_line = (ushort*)bit_bd.Scan0;
            for (int Y = 0; Y < light.Height; ++Y, bit_line += bit_delta) {
                ushort* bit_cur = bit_line;
                ushort* bit_end = bit_cur + light.Width;
                while (bit_cur < bit_end) {
                    if (*bit_cur == 0x7FFF || *bit_cur == 0x8420) {
                        ++bit_cur;
                        continue;
                    }
                        
                    sbyte value = (sbyte)(((*bit_cur >> 10) & 0xFFFF) - 0x1F);
                    if (value > 0)
                        --value;
                    if (value >= 0 && typo < 0)
                        value = (sbyte)(0 - System.Math.Abs(value));
                    else if (value < 0 && typo >= 0)
                        value = (sbyte)(0 + System.Math.Abs(value));
  
                    //value = 3;
                    *bit_cur++ = (ushort)(((0x1F + value) << 10) + ((0x1F + value) << 5) + (0x1F + value));
                }
            }

            light.UnlockBits(bit_bd);
        }


        public unsafe static void Save(string path)
        {
            string idx = Path.Combine(path, "lightidx.mul");
            string mul = Path.Combine(path, "light.mul");
            using (FileStream fsidx = new FileStream(idx, FileMode.Create, FileAccess.Write, FileShare.Write),
                              fsmul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (BinaryWriter binidx = new BinaryWriter(fsidx),
                                    binmul = new BinaryWriter(fsmul))
                {
                    for (int index = 0; index < m_Cache.Length; index++)
                    {
                        if (m_Cache[index] == null)
                            m_Cache[index] = GetLight(index);
                        Bitmap bmp = m_Cache[index];

                        if ((bmp == null) || (m_Removed[index]))
                        {
                            binidx.Write((int)-1); // lookup
                            binidx.Write((int)-1); // length
                            binidx.Write((int)-1); // extra
                        }
                        else
                        {
                            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
                            ushort* line = (ushort*)bd.Scan0;
                            int delta = bd.Stride >> 1;

                            binidx.Write((int)fsmul.Position); //lookup
                            int length = (int)fsmul.Position;

                            for (int Y = 0; Y < bmp.Height; ++Y, line += delta)
                            {
                                ushort* cur = line;
                                ushort* end = cur + bmp.Width;
                                while (cur < end)
                                {
                                    sbyte value = (sbyte)(((*cur++ >> 10) & 0xffff) - 0x1f);
                                    if (value > 0) // wtf? but it works...
                                        --value;
                                    binmul.Write(value);
                                }
                            }
                            length = (int)fsmul.Position - length;
                            binidx.Write(length);
                            binidx.Write((int)(bmp.Width << 16) + bmp.Height);
                            bmp.UnlockBits(bd);
                        }
                    }
                }
            }
        }
    }
}
