using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;
using AviFile;

namespace Ultima
{
    public sealed class AnimationEdit
    {
        private static FileIndex m_FileIndex = new FileIndex("Anim.idx", "Anim.mul", 6);
        private static FileIndex m_FileIndex2 = new FileIndex("Anim2.idx", "Anim2.mul", -1);
        private static FileIndex m_FileIndex3 = new FileIndex("Anim3.idx", "Anim3.mul", -1);
        private static FileIndex m_FileIndex4 = new FileIndex("Anim4.idx", "Anim4.mul", -1);
        private static FileIndex m_FileIndex5 = new FileIndex("Anim5.idx", "Anim5.mul", -1);

        private static AnimIdx[] animcache;
        private static AnimIdx[] animcache2;
        private static AnimIdx[] animcache3;
        private static AnimIdx[] animcache4;
        private static AnimIdx[] animcache5;
        static AnimationEdit()
        {
            if (m_FileIndex.IdxLength > 0)
                animcache = new AnimIdx[m_FileIndex.IdxLength / 12];
            if (m_FileIndex2.IdxLength > 0)
                animcache2 = new AnimIdx[m_FileIndex2.IdxLength / 12];
            if (m_FileIndex3.IdxLength > 0)
                animcache3 = new AnimIdx[m_FileIndex3.IdxLength / 12];
            if (m_FileIndex4.IdxLength > 0)
                animcache4 = new AnimIdx[m_FileIndex4.IdxLength / 12];
            if (m_FileIndex5.IdxLength > 0)
                animcache5 = new AnimIdx[m_FileIndex5.IdxLength / 12];
        }
        /// <summary>
        /// Rereads AnimX files
        /// </summary>
        public static void Reload()
        {
            m_FileIndex = new FileIndex("Anim.idx", "Anim.mul", 6);
            m_FileIndex2 = new FileIndex("Anim2.idx", "Anim2.mul", -1);
            m_FileIndex3 = new FileIndex("Anim3.idx", "Anim3.mul", -1);
            m_FileIndex4 = new FileIndex("Anim4.idx", "Anim4.mul", -1);
            m_FileIndex5 = new FileIndex("Anim5.idx", "Anim5.mul", -1);
            if (m_FileIndex.IdxLength > 0)
                animcache = new AnimIdx[m_FileIndex.IdxLength / 12];
            if (m_FileIndex2.IdxLength > 0)
                animcache = new AnimIdx[m_FileIndex2.IdxLength / 12];
            if (m_FileIndex3.IdxLength > 0)
                animcache = new AnimIdx[m_FileIndex3.IdxLength / 12];
            if (m_FileIndex4.IdxLength > 0)
                animcache = new AnimIdx[m_FileIndex4.IdxLength / 12];
            if (m_FileIndex5.IdxLength > 0)
                animcache = new AnimIdx[m_FileIndex5.IdxLength / 12];
        }

        private static void GetFileIndex(int body, int fileType, int action, int direction, out FileIndex fileIndex, out int index)
        {
            switch (fileType)
            {
                default:
                case 1:
                    fileIndex = m_FileIndex;
                    if (body < 200)
                        index = body * 110;
                    else if (body < 400)
                        index = 22000 + ((body - 200) * 65);
                    else
                        index = 35000 + ((body - 400) * 175);
                    break;
                case 2:
                    fileIndex = m_FileIndex2;
                    if (body < 200)
                        index = body * 110;
                    else
                        index = 22000 + ((body - 200) * 65);
                    break;
                case 3:
                    fileIndex = m_FileIndex3;
                    if (body < 200)
                        index = 9000 + (body * 65);
                    else if (body < 400)
                        index = 22000 + ((body - 200) * 110);
                    else
                        index = 35000 + ((body - 400) * 175);
                    break;
                case 4:
                    fileIndex = m_FileIndex4;
                    if (body < 200)
                        index = body * 110;
                    else if (body < 400)
                        index = 22000 + ((body - 200) * 65);
                    else
                        index = 35000 + ((body - 400) * 175);
                    break;
                case 5:
                    fileIndex = m_FileIndex5;
                    if ((body < 200) && (body != 34)) // looks strange, though it works.
                        index = body * 110;
                    else if (body < 400)
                        index = 22000 + ((body - 200) * 65);
                    else
                        index = 35000 + ((body - 400) * 175);
                    break;
            }

            index += action * 5;

            if (direction <= 4)
                index += direction;
            else
                index += direction - (direction - 4) * 2;
        }

        private static AnimIdx[] GetCache(int filetype)
        {
            switch (filetype)
            {
                case 1:
                    return animcache;
                case 2:
                    return animcache2;
                case 3:
                    return animcache3;
                case 4:
                    return animcache4;
                case 5:
                    return animcache5;
                default:
                    return animcache;
            }
        }

        public static AnimIdx GetAnimation(int filetype, int body, int action, int dir)
        {
            AnimIdx[] cache = GetCache(filetype);
            FileIndex fileIndex;
            int index;
            GetFileIndex(body, filetype, action, dir, out fileIndex, out index);

            if (cache != null)
            {
                if (cache[index] != null)
                    return cache[index];
            }
            return cache[index] = new AnimIdx(index, fileIndex, filetype);
        }

        public static bool IsActionDefinied(int filetype, int body, int action)
        {
            AnimIdx[] cache = GetCache(filetype);
            FileIndex fileIndex;
            int index;
            GetFileIndex(body, filetype, action, 0, out fileIndex, out index);

            if (cache != null)
            {
                if (cache[index] != null)
                {
                    if ((cache[index].Frames != null) && (cache[index].Frames.Count > 0))
                        return true;
                    else
                        return false;
                }
            }

            int AnimCount = Animations.GetAnimLength(body, filetype);
            if (AnimCount < action)
                return false;

            int length, extra;
            bool patched;
            bool valid = fileIndex.Valid(index, out length, out extra, out patched);
            if ((!valid) || (length < 1))
                return false;
            return true;
        }

        public static void LoadFromVD(int filetype, int body, BinaryReader bin)
        {
            AnimIdx[] cache = GetCache(filetype);
            FileIndex fileIndex;
            int index;
            GetFileIndex(body, filetype, 0, 0, out fileIndex, out index);
            int animlength = Animations.GetAnimLength(body, filetype) * 5;
            Entry3D[] entries = new Entry3D[animlength];

            for (int i = 0; i < animlength; ++i)
            {
                entries[i].lookup = bin.ReadInt32();
                entries[i].length = bin.ReadInt32();
                entries[i].extra = bin.ReadInt32();
            }
            foreach (Entry3D entry in entries)
            {
                if ((entry.lookup > 0) && (entry.lookup < bin.BaseStream.Length) && (entry.length > 0))
                {
                    bin.BaseStream.Seek(entry.lookup, SeekOrigin.Begin);
                    cache[index] = new AnimIdx(bin, entry.extra);
                }
                ++index;
            }
        }

        public static void ExportToVD(int filetype, int body, BinaryWriter bin)
        {
            AnimIdx[] cache = GetCache(filetype);
            FileIndex fileIndex;
            int index;
            GetFileIndex(body, filetype, 0, 0, out fileIndex, out index);
            
            int animlength = Animations.GetAnimLength(body, filetype);
            long indexpos = bin.BaseStream.Position;
            long animpos = bin.BaseStream.Position + 12 * animlength * 5;
            for (int i = index; i < index + animlength * 5; i++)
            {
                AnimIdx anim;
                if (cache != null)
                {
                    if (cache[i] != null)
                        anim = cache[i];
                    else
                        anim = cache[i] = new AnimIdx(i, fileIndex, filetype);
                }
                else
                    anim = cache[i] = new AnimIdx(i, fileIndex, filetype);

                if (anim == null)
                {
                    bin.BaseStream.Seek(indexpos, SeekOrigin.Begin);
                    bin.Write((int)-1);
                    bin.Write((int)-1);
                    bin.Write((int)-1);
                    indexpos = bin.BaseStream.Position;
                }
                else
                    anim.ExportToVD(bin, ref indexpos, ref animpos);
            }
        }

        public static void ExportToVD(int filetype, int body, string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.Write)) {
                using (BinaryWriter bin = new BinaryWriter(fs)) {
                    bin.Write((short)6);
                    int animlength = Animations.GetAnimLength(body, filetype);
                    int currtype = animlength == 22 ? 0 : animlength == 13 ? 1 : 2;
                    bin.Write((short)currtype);

                    ExportToVD(filetype, body, bin);
                }
            }
        }

        public static void Save(int filetype, string path)
        {
            string filename;
            AnimIdx[] cache;
            FileIndex fileindex;
            switch (filetype)
            {
                case 1: filename = "anim"; cache = animcache; fileindex = m_FileIndex; break;
                case 2: filename = "anim2"; cache = animcache2; fileindex = m_FileIndex2; break;
                case 3: filename = "anim3"; cache = animcache3; fileindex = m_FileIndex3; break;
                case 4: filename = "anim4"; cache = animcache4; fileindex = m_FileIndex4; break;
                case 5: filename = "anim5"; cache = animcache5; fileindex = m_FileIndex5; break;
                default: filename = "anim"; cache = animcache; fileindex = m_FileIndex; break;
            }
            string idx = Path.Combine(path, filename + ".idx");
            string mul = Path.Combine(path, filename + ".mul");
            using (FileStream fsidx = new FileStream(idx, FileMode.Create, FileAccess.Write, FileShare.Write),
                              fsmul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (BinaryWriter binidx = new BinaryWriter(fsidx),
                                    binmul = new BinaryWriter(fsmul))
                {
                    for (int idxc = 0; idxc < cache.Length; ++idxc)
                    {
                        AnimIdx anim;
                        if (cache[idxc] != null)
                            anim = cache[idxc];
                        else
                            anim = cache[idxc] = new AnimIdx(idxc, fileindex, filetype);

                        if (anim == null) {
                            // WTF? Вообще возможно ли это???
                            throw new Exception("AnimationEdit saving null error.");
                            binidx.Write((int)-1);
                            binidx.Write((int)-1);
                            binidx.Write((int)-1);
                        } else {
                            anim.Save(binmul, binidx);
                        }
                    }
                }
            }
        }
    }

    public sealed class AnimIdx
    {
        public int idxextra;
        public ushort[] Palette { get; private set; }
        public List<FrameEdit> Frames { get; private set; }

        public AnimIdx(int index, FileIndex fileIndex, int filetype)
        {
            Palette = new ushort[0x100];
            int length, extra;
            bool patched;
            Stream stream = fileIndex.Seek(index, out length, out extra, out patched);
            if ((stream == null) || (length < 1))
                return;

            idxextra = extra;
            using (BinaryReader bin = new BinaryReader(stream))
            {
                for (int i = 0; i < 0x100; ++i)
                    Palette[i] = (ushort)(bin.ReadUInt16() ^ 0x8000);

                int start = (int)bin.BaseStream.Position;
                int frameCount = bin.ReadInt32();

                int[] lookups = new int[frameCount];

                for (int i = 0; i < frameCount; ++i)
                    lookups[i] = start + bin.ReadInt32();

                Frames = new List<FrameEdit>();

                for (int i = 0; i < frameCount; ++i)
                {
                    stream.Seek(lookups[i], SeekOrigin.Begin);
                    Frames.Add(new FrameEdit(bin));
                }
            }
            stream.Close();
        }

        public AnimIdx(BinaryReader bin, int extra)
        {
            Palette = new ushort[0x100];
            idxextra = extra;
            for (int i = 0; i < 0x100; ++i)
                Palette[i] = (ushort)(bin.ReadUInt16() ^ 0x8000);

            int start = (int)bin.BaseStream.Position;
            int frameCount = bin.ReadInt32();

            int[] lookups = new int[frameCount];

            for (int i = 0; i < frameCount; ++i)
                lookups[i] = start + bin.ReadInt32();

            Frames = new List<FrameEdit>();

            for (int i = 0; i < frameCount; ++i)
            {
                bin.BaseStream.Seek(lookups[i], SeekOrigin.Begin);
                Frames.Add(new FrameEdit(bin));
            }
        }

        public unsafe Bitmap[] GetFrames(bool centred = false)
        {
            if ((Frames == null) || (Frames.Count == 0))
                return null;
            Bitmap[] bits = new Bitmap[Frames.Count];
            int _width = 0, _height = 0;
            if (centred) {
                for (int i = 0; i < bits.Length; ++i) {
                    _width  = Math.Max(2 * Math.Max(Frames[i].Center.X, Frames[i].width - Frames[i].Center.X), _width);
                    _height = Math.Max(Math.Max(-4 * Frames[i].Center.Y, (Frames[i].height + Frames[i].Center.Y) * 4/3), _height);
                }
                _width  = (int)(Math.Max(1.2, (1.6 - _width  / 640)) * _width);
                _height = (int)(Math.Max(1.2, (1.6 - _height / 640)) * _height);

                     if (_width <=   64) _width = 64;
                else if (_width <=   96) _width = 96;
                else if (_width <=  128) _width = 128;
                else if (_width <=  192) _width = 192;
                else if (_width <=  256) _width = 256;
                else if (_width <=  384) _width = 384;
                else if (_width <=  512) _width = 512;
                else if (_width <=  768) _width = 768;
                else if (_width <= 1024) _width = 1024;

                     if (_height <=   64) _width = 64;
                else if (_height <=   96) _height = 96;
                else if (_height <=  128) _height = 128;
                else if (_height <=  192) _height = 192;
                else if (_height <=  256) _height = 256;
                else if (_height <=  384) _height = 384;
                else if (_height <=  512) _height = 512;
                else if (_height <=  768) _height = 768;
                else if (_height <= 1024) _width = 1024;
            }         

            for (int i = 0; i < bits.Length; ++i)
            {
                FrameEdit frame = (FrameEdit)Frames[i];
                int width  = centred ? _width  : frame.width;
                int height = centred ? _height : frame.height;
                if (height == 0 || width == 0)
                    continue;
                Bitmap bmp = new Bitmap(width, height, PixelFormat.Format16bppArgb1555);
                BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
                ushort* line = (ushort*)bd.Scan0;
                int delta = bd.Stride >> 1;

                int xBase = centred ? ( width/2   - 0x200) : (frame.Center.X - 0x200);
                int yBase = centred ? (height*3/4 - 0x200) : (frame.Center.Y + height - 0x200);

                line += xBase;
                line += yBase * delta;
                for (int j = 0; j < frame.RawData.Length; ++j)
                {
                    FrameEdit.Raw raw = frame.RawData[j];

                    ushort* cur = line + (((raw.offy) * delta) + ((raw.offx) & 0x3FF));
                    ushort* end = cur + (raw.run);

                    int ii = 0;
                    while (cur < end)
                    {
                        *cur++ = Palette[raw.data[ii++]];
                    }
                }
                bmp.UnlockBits(bd);
                bits[i] = bmp;
            }
            return bits;
        }

        public void AddFrame(Bitmap bit)
        {
            if (Frames == null)
                Frames = new List<FrameEdit>();
            Frames.Add(new FrameEdit(bit, Rectangle.FromLTRB(0, 0, bit.Width, bit.Height), Palette, 0, 0));
        }

        public void ReplaceFrame(Bitmap bit, int index)
        {
            if ((Frames == null) || (Frames.Count == 0))
                return;
            if (index > Frames.Count)
                return;
            Frames[index] = new FrameEdit(bit, Rectangle.FromLTRB(0, 0, bit.Width, bit.Height), Palette, ((FrameEdit)Frames[index]).Center.X, ((FrameEdit)Frames[index]).Center.Y);
        }

        public void RemoveFrame(int index)
        {
            if (Frames == null)
                return;
            if (index > Frames.Count)
                return;
            Frames.RemoveAt(index);
        }

        public void ClearFrames()
        {
            if (Frames == null)
                return;
            Frames.Clear();
        }

        //Soulblighter Modification
        public void GetGifPalette(Bitmap bit)
        {
            using (MemoryStream imageStreamSource = new MemoryStream())
            {
                System.Drawing.ImageConverter ic = new System.Drawing.ImageConverter();
                byte[] btImage = (byte[])ic.ConvertTo(bit, typeof(byte[]));
                imageStreamSource.Write(btImage, 0, btImage.Length);
                GifBitmapDecoder decoder = new GifBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                BitmapPalette pal = decoder.Palette;
                int i;
                for (i = 0; i < 0x100; i++)
                {
                    this.Palette[i] = 0;
                }
                try
                {
                    i = 0;
                    while (i < 0x100)//&& i < pal.Colors.Count)
                    {

                        int Red = pal.Colors[i].R / 8;
                        int Green = pal.Colors[i].G / 8;
                        int Blue = pal.Colors[i].B / 8;
                        int contaFinal = (((0x400 * Red) + (0x20 * Green)) + Blue) + 0x8000;
                        if (contaFinal == 0x8000)
                            contaFinal = 0x8001;
                        this.Palette[i] = (ushort)contaFinal;
                        i++;
                    }
                }
                catch (System.IndexOutOfRangeException)
                { }
                catch (System.ArgumentOutOfRangeException)
                { }
                for (i = 0; i < 0x100; i++)
                {
                    if (this.Palette[i] < 0x8000)
                        this.Palette[i] = 0x8000;
                }
            }
        }

        public void GetImagePalette(Bitmap bit)
        {
            GetImagePalette(new[]{bit});
        }

        public void GetImagePalette(Bitmap[] bit)
        {
            var pallete = GetUniqeColors(bit);
            for (int i = 0; i < 0x100; i++)
                Palette[i] = 0x8000;
            if (Palette.Length <= 0x100)
                Array.Copy(pallete, Palette, pallete.Length);
            else {
                throw new NotImplementedException("Генерация палитры из кадра(ов) содержащих более 256 различных цветов еще не реализована.");
            }
        }

        public unsafe ushort[] GetUniqeColors(Bitmap[] bit)
        {
            List<ushort> colors = new List<ushort>(256);
            colors.Add(0x8000);

            for (int b = 0; b < bit.Length; ++b)
            {
                Bitmap bmp = new Bitmap(bit[b]);
                BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
                ushort* line = (ushort*)bd.Scan0;
                int delta = bd.Stride >> 1;
                ushort* cur = line;

                int y = 0;
                while (y < bmp.Height) {
                    cur = line;
                    for (int x = 0; x < bmp.Width; x++) {
                        ushort c = cur[x];
                        if (c != 0) {
                            bool found = false;
                            int i = 0;
                            while (i < colors.Count) {
                                if (colors[i] == c) {
                                    found = true;
                                    break;
                                }
                                i++;
                            }
                            if (!found)
                                colors.Add((ushort)(0x8000|c));
                        }
                    }
                    y++;
                    line += delta;
                } 
            }
            return colors.ToArray();
        }

        public void GetPalPalette(string file)
        {
            var content = File.ReadAllLines(file, Encoding.ASCII);
            if (content[0].ToUpper() != "JASC-PAL" || content[1].ToUpper() != "0100"  || content[2].ToUpper() != "256")
                throw new FileFormatException();
            Array.Clear(Palette, 0, Palette.Length);
            for (int i = 0; i < 0x100; ++i) {
                var rgb = content[3+i].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                Palette[i] = (ushort)(byte.Parse(rgb[0])>>3<<10 | byte.Parse(rgb[1])>>3<<5 | byte.Parse(rgb[2])>>3<<0);
                if (Palette[i] > 0) Palette[i] |= 0x8000;
            }
        }

        public void SavePalPalette(string file)
        {
            var content = String.Format("JASC-PAL{0}0100{0}256{0}", Environment.NewLine);
            for (int i = 0; i < 0x100; ++i)
                content += String.Format("{1} {2} {3} {0}", Environment.NewLine, 
                                ((Palette[i]>>10)&0x1F)<<3, ((Palette[i]>>5)&0x1F)<<3, ((Palette[i]>>0)&0x1F)<<3);
            File.WriteAllText(file, content, Encoding.ASCII);
        }

        public void PaletteConversor(int seletor)
        {
            int i;
            for (i = 0; i < 0x100; i++)
            {
                int BlueTemp = (this.Palette[i] - 0x8000) / 0x20;
                BlueTemp *= 0x20;
                BlueTemp = (this.Palette[i] - 0x8000) - BlueTemp;
                int GreenTemp = (this.Palette[i] - 0x8000) / 0x400;
                GreenTemp *= 0x400;
                GreenTemp = ((this.Palette[i] - 0x8000) - GreenTemp) - BlueTemp;
                GreenTemp /= 0x20;
                int RedTemp = (this.Palette[i] - 0x8000) / 0x400;
                int contaFinal = 0;
                switch (seletor)
                {
                    case 1: contaFinal = (((0x400 * RedTemp) + (0x20 * GreenTemp)) + BlueTemp) + 0x8000;
                        break;
                    case 2: contaFinal = (((0x400 * RedTemp) + (0x20 * BlueTemp)) + GreenTemp) + 0x8000;
                        break;
                    case 3: contaFinal = (((0x400 * GreenTemp) + (0x20 * RedTemp)) + BlueTemp) + 0x8000;
                        break;
                    case 4: contaFinal = (((0x400 * GreenTemp) + (0x20 * BlueTemp)) + RedTemp) + 0x8000;
                        break;
                    case 5: contaFinal = (((0x400 * BlueTemp) + (0x20 * GreenTemp)) + RedTemp) + 0x8000;
                        break;
                    case 6: contaFinal = (((0x400 * BlueTemp) + (0x20 * RedTemp)) + GreenTemp) + 0x8000;
                        break;
                }
                if (contaFinal == 0x8000)
                    contaFinal = 0x8001;
                this.Palette[i] = (ushort)contaFinal;
            }
            for (i = 0; i < 0x100; i++)
            {
                if (this.Palette[i] < 0x8000)
                    this.Palette[i] = 0x8000;
            }
        }

        public void PaletteReductor(int Redp, int Greenp, int Bluep)
        {
            int i;
            Redp /= 8;
            Greenp /= 8;
            Bluep /= 8;
            for (i = 0; i < 0x100; i++)
            {
                int BlueTemp = (this.Palette[i] - 0x8000) / 0x20;
                BlueTemp *= 0x20;
                BlueTemp = (this.Palette[i] - 0x8000) - BlueTemp;
                int GreenTemp = (this.Palette[i] - 0x8000) / 0x400;
                GreenTemp *= 0x400;
                GreenTemp = ((this.Palette[i] - 0x8000) - GreenTemp) - BlueTemp;
                GreenTemp /= 0x20;
                int RedTemp = (this.Palette[i] - 0x8000) / 0x400;
                RedTemp += Redp;
                GreenTemp += Greenp;
                BlueTemp += Bluep;
                if (RedTemp < 0)
                    RedTemp = 0;
                if (RedTemp > 0x1f)
                    RedTemp = 0x1f;
                if (GreenTemp < 0)
                    GreenTemp = 0;
                if (GreenTemp > 0x1f)
                    GreenTemp = 0x1f;
                if (BlueTemp < 0)
                    BlueTemp = 0;
                if (BlueTemp > 0x1f)
                    BlueTemp = 0x1f;
                int contaFinal = (((0x400 * RedTemp) + (0x20 * GreenTemp)) + BlueTemp) + 0x8000;
                if (contaFinal == 0x8000)
                    contaFinal = 0x8001;
                this.Palette[i] = (ushort)contaFinal;
            }
            for (i = 0; i < 0x100; i++)
            {
                if (this.Palette[i] < 0x8000)
                    this.Palette[i] = 0x8000;
            }
        }
        //End of Soulblighter Modification

        public unsafe void ExportPalette(string filename, int type)
        {
            switch (type)
            {
                case 0:
                    using (StreamWriter Tex = new StreamWriter(new FileStream(filename, FileMode.Create, FileAccess.ReadWrite)))
                    {
                        for (int i = 0; i < 0x100; ++i)
                        {
                            Tex.WriteLine(Palette[i]);
                        }
                    }
                    break;
                case 1:
                    {
                        Bitmap bmp = new Bitmap(0x100, 20, PixelFormat.Format16bppArgb1555);
                        BitmapData bd = bmp.LockBits(new Rectangle(0, 0, 0x100, 20), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
                        ushort* line = (ushort*)bd.Scan0;
                        int delta = bd.Stride >> 1;
                        for (int y = 0; y < bd.Height; ++y, line += delta)
                        {
                            ushort* cur = line;
                            for (int i = 0; i < 0x100; ++i)
                            {
                                *cur++ = Palette[i];
                            }
                        }
                        bmp.UnlockBits(bd);
                        Bitmap b = new Bitmap(bmp);
                        b.Save(filename, ImageFormat.Bmp);
                        b.Dispose();
                        bmp.Dispose();
                        break;
                    }
                case 2:
                    {
                        Bitmap bmp = new Bitmap(0x100, 20, PixelFormat.Format16bppArgb1555);
                        BitmapData bd = bmp.LockBits(new Rectangle(0, 0, 0x100, 20), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
                        ushort* line = (ushort*)bd.Scan0;
                        int delta = bd.Stride >> 1;
                        for (int y = 0; y < bd.Height; ++y, line += delta)
                        {
                            ushort* cur = line;
                            for (int i = 0; i < 0x100; ++i)
                            {
                                *cur++ = Palette[i];
                            }
                        }
                        bmp.UnlockBits(bd);
                        Bitmap b = new Bitmap(bmp);
                        b.Save(filename, ImageFormat.Tiff);
                        b.Dispose();
                        bmp.Dispose();
                        break;
                    }
            }
        }

        public void ReplacePalette(ushort[] palette)
        {
            Palette = palette;
        }

        public void Save(BinaryWriter bin, BinaryWriter idx)
        {
            if ((Frames == null) || (Frames.Count == 0))
            {
                idx.Write((int)-1);
                idx.Write((int)-1);
                idx.Write((int)-1);
                return;
            }
            long start = bin.BaseStream.Position;
            idx.Write((int)start);

            for (int i = 0; i < 0x100; ++i)
                bin.Write((ushort)(Palette[i] ^ 0x8000));
            long startpos = bin.BaseStream.Position;
            bin.Write((int)Frames.Count);
            long seek = bin.BaseStream.Position;
            long curr = bin.BaseStream.Position + 4 * Frames.Count;
            foreach (FrameEdit frame in Frames)
            {
                bin.BaseStream.Seek(seek, SeekOrigin.Begin);
                bin.Write((int)(curr - startpos));
                seek = bin.BaseStream.Position;
                bin.BaseStream.Seek(curr, SeekOrigin.Begin);
                frame.Save(bin);
                curr = bin.BaseStream.Position;
            }

            start = bin.BaseStream.Position - start;
            idx.Write((int)start);
            idx.Write((int)idxextra);
        }

        public void ExportToVD(BinaryWriter bin, ref long indexpos, ref long animpos)
        {
            bin.BaseStream.Seek(indexpos, SeekOrigin.Begin);
            if ((Frames == null) || (Frames.Count == 0))
            {
                bin.Write((int)-1);
                bin.Write((int)-1);
                bin.Write((int)-1);
                indexpos = bin.BaseStream.Position;
                return;
            }
            bin.Write((int)animpos);
            indexpos = bin.BaseStream.Position;
            bin.BaseStream.Seek(animpos, SeekOrigin.Begin);

            for (int i = 0; i < 0x100; ++i)
                bin.Write((ushort)(Palette[i] ^ 0x8000));
            long startpos = (int)bin.BaseStream.Position;
            bin.Write((int)Frames.Count);
            long seek = (int)bin.BaseStream.Position;
            long curr = bin.BaseStream.Position + 4 * Frames.Count;
            foreach (FrameEdit frame in Frames)
            {
                bin.BaseStream.Seek(seek, SeekOrigin.Begin);
                bin.Write((int)(curr - startpos));
                seek = bin.BaseStream.Position;
                bin.BaseStream.Seek(curr, SeekOrigin.Begin);
                frame.Save(bin);
                curr = bin.BaseStream.Position;
            }

            long length = bin.BaseStream.Position - animpos;
            animpos = bin.BaseStream.Position;
            bin.BaseStream.Seek(indexpos, SeekOrigin.Begin);
            bin.Write((int)length);
            bin.Write((int)idxextra);
            indexpos = bin.BaseStream.Position;
        }

        public void ExportToGIF(string file)
        {
            var frames = this.GetFrames(true);
            if (frames == null || frames.Length <= 0) return;

            // GetImageEncoder
            ImageCodecInfo gifCodec = null;
            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
                //if (codec.FilenameExtension.ToLower().IndexOf("*.gif") > -1) {
                if (codec.MimeType == "image/gif") {
                    gifCodec = codec;
                    break;
                }
            var encparams = new EncoderParameters(1);
            //encparams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
            encparams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)EncoderValue.VersionGif89);

            frames[0].Save(file, gifCodec, encparams);

            for (int i = 1; i < frames.Length; i++) {
                encparams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
                frames[0].SaveAdd(frames[i], encparams);
            }
            encparams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.Flush);
            frames[0].SaveAdd(encparams);

            throw new NotImplementedException();
        }

        public void ExportToAVI(string file, bool createPal = false)
        {
            var frames = this.GetFrames(true);
            if (frames == null || frames.Length <= 0) return;
            if (createPal) SavePalPalette(Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)+".pal"));

            var avi = new AviManager(file, false);
            var bitdat = frames[0].LockBits(new Rectangle(0, 0, frames[0].Width, frames[0].Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
            var size = bitdat.Stride * frames[0].Height;
            frames[0].UnlockBits(bitdat);
            var stream = avi.AddVideoStream(false, 10.0, size, frames[0].Width, frames[0].Height, PixelFormat.Format16bppArgb1555); 
            foreach (var frame in frames) {
                stream.AddFrame(frame);
            }
            avi.Close();
        }

        /// <summary>
        /// Создание анимации из файла *.avi (при этом палитра если возможно берется из одноименного файла *.pal) (TODO: поддрежка формата *.gif)
        /// </summary>
        /// <param name="file"></param>
        public void ImportFromFile(string file)
        {
            if (!File.Exists(file)) return;
            //Palette = new ushort[0x100];
            //Frames = new List<FrameEdit>();
            //idxextra = 0x00000000; // хз

            ClearFrames();
            switch(Path.GetExtension(file)) {
                case ".avi" :
                    var filename = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
                    var avi = new AviManager(filename+".avi", true);
                    var stream = avi.GetVideoStream();
                    stream.GetFrameOpen();
                    var frames = new Bitmap[stream.CountFrames];
                    for (int pos = 0; pos < stream.CountFrames; ++pos)
                        frames[pos] = stream.GetBitmap(pos);
                    stream.GetFrameClose();
                    avi.Close();

                    if (File.Exists(filename+".pal"))
                        this.GetPalPalette(filename+".pal");
                    else this.GetImagePalette(frames);

                    if (Frames == null)
                        Frames = new List<FrameEdit>();
                    foreach (var frame in frames)
                        Frames.Add(new FrameEdit(frame, FrameEdit.GetSingleRect(frame), Palette, frame.Width/2, -frame.Height/4));
                        //this.AddFrame(frame);
                         
                    break;
                case ".gif" :
                    throw new NotImplementedException();
                    break;
                default : throw new FileFormatException();
            }
        }
    }

    public sealed class FrameEdit
    {
        private const int DoubleXor = (0x200 << 22) | (0x200 << 12);
        public struct Raw
        {
            public int run;
            public int offx;
            public int offy;
            public byte[] data;
        }
        public Raw[] RawData { get; private set; }
        public Point Center { get; set; }
        public int width;
        public int height;

        public FrameEdit(BinaryReader bin)
        {
            int xCenter = bin.ReadInt16();
            int yCenter = bin.ReadInt16();

            width = bin.ReadUInt16();
            height = bin.ReadUInt16();
            if (height == 0 || width == 0)
                return;
            int header;

            List<Raw> tmp = new List<Raw>();
            while ((header = bin.ReadInt32()) != 0x7FFF7FFF)
            {
                Raw raw = new Raw();
                header ^= DoubleXor;
                raw.run = (header & 0xFFF);
                raw.offy = ((header >> 12) & 0x3FF);
                raw.offx = ((header >> 22) & 0x3FF);

                int i = 0;
                raw.data = new byte[raw.run];
                while (i < raw.run)
                {
                    raw.data[i++] = bin.ReadByte();
                }
                tmp.Add(raw);
            }
            RawData = tmp.ToArray();
            Center = new Point(xCenter, yCenter);
        }

        public unsafe FrameEdit(Bitmap bit, Rectangle rect, ushort[] palette, int centerx, int centery)
        {
            Center = new Point(centerx - rect.Left/**/, centery + bit.Height-rect.Bottom/**/);
            width = rect.Width/*bit.Width*/;
            height = rect.Height/*bit.Height*/;
            BitmapData bd = bit.LockBits(new Rectangle(0, 0, bit.Width/*width*/, bit.Height/*height*/), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555); 
            int delta = bd.Stride >> 1;
            ushort* line = (ushort*)bd.Scan0 + rect.Top*delta;
            List<Raw> tmp = new List<Raw>();

            int X = rect.Left/*0*/;
            for (int Y = rect.Top/*0*/; Y < rect.Bottom/*bit.Height*/; ++Y, line += delta)
            {
                ushort* cur = line;
                int i = 0;
                int j = 0;
                X = rect.Left/*0*/;
                while (i < rect.Right/*bit.Width*/)
                {
                    i = X;
                    for (i = X; i <= rect.Right/*bit.Width*/; ++i)
                    {
                        //first pixel set
                        if (i < rect.Right/*bit.Width*/)
                        {
                            if (cur[i] != 0)
                                break;
                        }
                    }
                    if (i < rect.Right/*bit.Width*/)
                    {
                        for (j = (i + 1); j < rect.Right/*bit.Width*/; ++j)
                        {
                            //next non set pixel
                            if (cur[j] == 0)
                                break;
                        }
                        Raw raw = new Raw();
                        raw.run = j - i;
                        raw.offx = j - raw.run - centerx;
                        raw.offx += 512;
                        raw.offy = Y - rect.Top/**/ - Center.Y/*centery*/ - rect.Height/*bit.Height*/;
                        //raw.offy = Y - centery - bit.Height;
                        raw.offy += 512;

                        int r = 0;
                        raw.data = new byte[raw.run];
                        while (r < raw.run)
                        {
                            ushort col = (ushort)(cur[r + i]);
                            raw.data[r++] = GetPaletteIndex(palette, col);
                        }
                        tmp.Add(raw);
                        X = j + 1;
                        i = X;
                    }
                }
            }

            RawData = tmp.ToArray();
            bit.UnlockBits(bd);
        }

        public void ChangeCenter(int x, int y)
        {
            for (int i = 0; i < RawData.Length; i++)
            {
                RawData[i].offx += Center.X;
                RawData[i].offx -= x;
                RawData[i].offy += Center.Y;
                RawData[i].offy -= y;
            }
            Center = new Point(x, y);
        }

        private static byte GetPaletteIndex(ushort[] palette, ushort col)
        {
            for (int i = 0; i < palette.Length; i++)
            {
                if (palette[i] == col)
                    return (byte)i;
            }
            return (byte)0;
        }

        public static unsafe Rectangle GetSingleRect(Bitmap bit)
        {
            int x1 = int.MaxValue, x2 = int.MinValue, y1 = int.MaxValue, y2 = int.MinValue;
            BitmapData bd = bit.LockBits(new Rectangle(0, 0, bit.Width, bit.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
            ushort* line = (ushort*)bd.Scan0;
            int delta = bd.Stride >> 1;
            
            for (int _y = 0; _y < bit.Height; ++_y, line += delta)
                for (int _x = 0; _x < bit.Width; ++_x)
                    if (line[_x] != 0x0000 && line[_x] != 0x8000)
                    {
                        x1 = Math.Min(x1, _x);
                        y1 = Math.Min(y1, _y);
                        x2 = Math.Max(x2, _x);
                        y2 = Math.Max(y2, _y);
                    }

            bit.UnlockBits(bd);
            return Rectangle.FromLTRB(x1, y1, x2+1, y2+1);
        }

        public void Save(BinaryWriter bin)
        {
            bin.Write((short)Center.X);
            bin.Write((short)Center.Y);
            bin.Write((ushort)width);
            bin.Write((ushort)height);
            if (RawData != null)
            {
                for (int j = 0; j < RawData.Length; j++)
                {
                    int newHeader = RawData[j].run | (RawData[j].offy << 12) | (RawData[j].offx << 22);
                    newHeader ^= DoubleXor;
                    bin.Write((int)newHeader);
                    foreach (byte b in RawData[j].data)
                        bin.Write(b);
                }
            }
            bin.Write((int)0x7FFF7FFF);
        }
    }
}