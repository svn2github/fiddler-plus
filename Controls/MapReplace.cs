/***************************************************************************
 *
 * $Author: Turley
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/

using System;
using System.IO;
using System.Windows.Forms;
using Ultima;

namespace FiddlerControls
{
    public partial class MapReplace : Form
    {
        private Ultima.Map workingmap;
        public MapReplace(Ultima.Map currmap)
        {
            InitializeComponent();
            this.Icon = FiddlerControls.Options.GetFiddlerIcon();
            workingmap = currmap;
            numericUpDownX1.Maximum = workingmap.Width;
            numericUpDownX2.Maximum = workingmap.Width;
            numericUpDownY1.Maximum = workingmap.Height;
            numericUpDownY2.Maximum = workingmap.Height;
            numericUpDownToX1.Maximum = workingmap.Width;
            numericUpDownToY1.Maximum = workingmap.Height;
            this.Text = String.Format("MapReplace ID:{0}",workingmap.FileIndex);
        }

        private void OnClickBrowse(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select directory containing the map files";
            dialog.ShowNewFolderButton = false;
            if (dialog.ShowDialog() == DialogResult.OK)
                textBox1.Text = dialog.SelectedPath;
            dialog.Dispose();
        }

        private void OnClickCopy(object sender, EventArgs e)
        {
            string path = textBox1.Text;
            if (!Directory.Exists(path)) {
                MessageBox.Show(this, String.Format("Указан не существующие каталог: '{0}'", path), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int x1 = (int)numericUpDownX1.Value;
            int x2 = (int)numericUpDownX2.Value;
            int y1 = (int)numericUpDownY1.Value;
            int y2 = (int)numericUpDownY2.Value;
            int tox = (int)numericUpDownToX1.Value;
            int toy = (int)numericUpDownToY1.Value;
            int w = (int)numericUpDownWidth.Value;
            int h = (int)numericUpDownHeight.Value;
            int id = (int)numericUpDownId.Value;

            if ((x1 >= x2) || (y1 >= y2)) {
                MessageBox.Show(this, "Не правильно заданы координаты области.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((x1 < 0) || (x1 >= w) || (x2 < 0) || (x2 >= w) ||
                (y1 < 0) || (y1 >= h) || (y2 < 0) || (y2 >= h) ) {
                MessageBox.Show(this, "Координаты области копируемой карты выходят за границу карты.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((tox < 0) || (tox >= workingmap.Width)  || ((tox + (x2 - x1)) >= workingmap.Width) ||
                (toy < 0) || (toy >= workingmap.Height) || ((toy + (y2 - y1)) >= workingmap.Height)) {
                MessageBox.Show(this, "Координаты области теущей карты выходят за границу карты.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (id < 0)
                return;

            x1 >>= 3;
            x2 >>= 3;
            y1 >>= 3;
            y2 >>= 3;

            tox >>= 3;
            toy >>= 3;

            int tox2 = (x2 - x1) + tox;
            int toy2 = (y2 - y1) + toy;

            int blocky = workingmap.Height >> 3;
            int blockx = workingmap.Width >> 3;

            int blocky_copy = h >> 3;
            int blockx_copy = w >> 3;

            progressBar1.Step = 1;
            progressBar1.Value = 0;
            progressBar1.Maximum=0;
            if (checkBoxMap.Checked)
                progressBar1.Maximum += blocky * blockx;
            if (checkBoxStatics.Checked)
                progressBar1.Maximum += blocky * blockx;
            
            if (checkBoxMap.Checked)
            {
                string copymap = Path.Combine(path, String.Format("map{0}.mul", id));
                if (!File.Exists(copymap)) {
                    MessageBox.Show(this, String.Format("Файл: '{0}' не существует.", copymap), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                FileStream m_map_copy = new FileStream(copymap, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader m_mapReader_copy = new BinaryReader(m_map_copy);

                string mapPath = Ultima.Files.GetFilePath(String.Format("map{0}.mul", workingmap.FileIndex));
                FileStream m_map;
                BinaryReader m_mapReader;
                if (mapPath != null) {
                    m_map = new FileStream(mapPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    m_mapReader = new BinaryReader(m_map);
                }
                else
                    return;

                string mul = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, String.Format("map{0}.mul", workingmap.FileIndex));
                using (FileStream fsmul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    using (BinaryWriter binmul = new BinaryWriter(fsmul))
                    {
                        for (int x = 0; x < blockx; ++x)
                        {
                            for (int y = 0; y < blocky; ++y)
                            {
                                if ((tox <= x) && (x <= tox2) && (toy <= y) && (y <= toy2))
                                {
                                    m_mapReader_copy.BaseStream.Seek((((x-tox+x1) * blocky_copy) + (y-toy+y1)) * 196, SeekOrigin.Begin);
                                    int header = m_mapReader_copy.ReadInt32();
                                    binmul.Write(header);
                                }
                                else
                                {
                                    m_mapReader.BaseStream.Seek(((x * blocky) + y) * 196, SeekOrigin.Begin);
                                    int header = m_mapReader.ReadInt32();
                                    binmul.Write(header);
                                }
                                for (int i = 0; i < 64; ++i)
                                {
                                    ushort tileid;
                                    sbyte z;
                                    if ((tox <= x) && (x <= tox2) && (toy <= y) && (y <= toy2))
                                    {
                                        tileid = m_mapReader_copy.ReadUInt16();
                                        z = m_mapReader_copy.ReadSByte();
                                    }
                                    else
                                    {
                                        tileid = m_mapReader.ReadUInt16();
                                        z = m_mapReader.ReadSByte();
                                    }
                                    if ((tileid < 0) || (tileid >= Art.StaticLength))
                                        tileid = 0;
                                    if (z < -128)
                                        z = -128;
                                    if (z > 127)
                                        z = 127;
                                    binmul.Write(tileid);
                                    binmul.Write(z);
                                }
                                progressBar1.PerformStep();
                            }
                        }
                    }
                }
                m_mapReader.Close();
                m_mapReader_copy.Close();
            }
            if (checkBoxStatics.Checked)
            {
                string indexPath = Files.GetFilePath(String.Format("staidx{0}.mul", workingmap.FileIndex));
                FileStream m_Index;
                BinaryReader m_IndexReader;
                if (indexPath != null)
                {
                    m_Index = new FileStream(indexPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    m_IndexReader = new BinaryReader(m_Index);
                }
                else
                    return;

                string staticsPath = Files.GetFilePath(String.Format("statics{0}.mul", workingmap.FileIndex));

                FileStream m_Statics;
                BinaryReader m_StaticsReader;
                if (staticsPath != null)
                {
                    m_Statics = new FileStream(staticsPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    m_StaticsReader = new BinaryReader(m_Statics);
                }
                else
                    return;


                string copyindexPath = Path.Combine(path, String.Format("staidx{0}.mul", id));
                if (!File.Exists(copyindexPath)) {
                    MessageBox.Show(this, String.Format("Файл: '{0}' не существует.", copyindexPath), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                FileStream m_Index_copy = new FileStream(copyindexPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader m_IndexReader_copy = new BinaryReader(m_Index_copy);

                string copystaticsPath = Path.Combine(path, String.Format("statics{0}.mul", id));
                if (!File.Exists(copystaticsPath)) {
                    MessageBox.Show(this, String.Format("Файл: '{0}' не существует.", copystaticsPath), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                FileStream m_Statics_copy = new FileStream(copystaticsPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader m_StaticsReader_copy = new BinaryReader(m_Statics_copy);

                string idx = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, String.Format("staidx{0}.mul", workingmap.FileIndex));
                string mul = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, String.Format("statics{0}.mul", workingmap.FileIndex));
                using (FileStream fsidx = new FileStream(idx, FileMode.Create, FileAccess.Write, FileShare.Write),
                                  fsmul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    using (BinaryWriter binidx = new BinaryWriter(fsidx),
                                        binmul = new BinaryWriter(fsmul))
                    {
                        for (int x = 0; x < blockx; ++x)
                        {
                            for (int y = 0; y < blocky; ++y)
                            {
                                int lookup, length, extra;
                                if ((tox <= x) && (x <= tox2) && (toy <= y) && (y <= toy2))
                                {
                                    m_IndexReader_copy.BaseStream.Seek((((x - tox + x1) * blocky_copy) + (y - toy + y1)) * 12, SeekOrigin.Begin);
                                    lookup = m_IndexReader_copy.ReadInt32();
                                    length = m_IndexReader_copy.ReadInt32();
                                    extra = m_IndexReader_copy.ReadInt32();
                                }
                                else
                                {
                                    m_IndexReader.BaseStream.Seek(((x * blocky) + y) * 12, SeekOrigin.Begin);
                                    lookup = m_IndexReader.ReadInt32();
                                    length = m_IndexReader.ReadInt32();
                                    extra = m_IndexReader.ReadInt32();
                                }

                                if (lookup < 0 || length <= 0)
                                {
                                    binidx.Write((int)-1); // lookup
                                    binidx.Write((int)-1); // length
                                    binidx.Write((int)-1); // extra
                                }
                                else
                                {
                                    if ((tox <= x) && (x <= tox2) && (toy <= y) && (y <= toy2))
                                        m_Statics_copy.Seek(lookup, SeekOrigin.Begin);
                                    else
                                        m_Statics.Seek(lookup, SeekOrigin.Begin);

                                    int fsmullength = (int)fsmul.Position;
                                    int count = length / 7;
                                    if (RemoveDupl.Checked)
                                    {
                                        StaticTile[] tilelist = new StaticTile[count];
                                        int j = 0;
                                        for (int i = 0; i < count; ++i)
                                        {
                                            StaticTile tile = new StaticTile();
                                            if ((tox <= x) && (x <= tox2) && (toy <= y) && (y <= toy2))
                                            {
                                                tile.m_ID = m_StaticsReader_copy.ReadUInt16();
                                                tile.m_X = m_StaticsReader_copy.ReadByte();
                                                tile.m_Y = m_StaticsReader_copy.ReadByte();
                                                tile.m_Z = m_StaticsReader_copy.ReadSByte();
                                                tile.m_Hue = m_StaticsReader_copy.ReadInt16();
                                            }
                                            else
                                            {
                                                tile.m_ID = m_StaticsReader.ReadUInt16();
                                                tile.m_X = m_StaticsReader.ReadByte();
                                                tile.m_Y = m_StaticsReader.ReadByte();
                                                tile.m_Z = m_StaticsReader.ReadSByte();
                                                tile.m_Hue = m_StaticsReader.ReadInt16();
                                            }

                                            if ((tile.m_ID >= 0) && (tile.m_ID < Art.StaticLength))
                                            {
                                                if (tile.m_Hue < 0)
                                                    tile.m_Hue = 0;
                                                bool first = true;
                                                for (int k = 0; k < j; ++k)
                                                {
                                                    if ((tilelist[k].m_ID == tile.m_ID)
                                                        && ((tilelist[k].m_X == tile.m_X) && (tilelist[k].m_Y == tile.m_Y))
                                                        && (tilelist[k].m_Z == tile.m_Z)
                                                        && (tilelist[k].m_Hue == tile.m_Hue))
                                                    {
                                                        first = false;
                                                        break;
                                                    }
                                                }
                                                if (first)
                                                    tilelist[j++] = tile;
                                            }
                                        }
                                        if (j > 0)
                                        {
                                            binidx.Write((int)fsmul.Position); //lookup
                                            for (int i = 0; i < j; ++i)
                                            {
                                                binmul.Write(tilelist[i].m_ID);
                                                binmul.Write(tilelist[i].m_X);
                                                binmul.Write(tilelist[i].m_Y);
                                                binmul.Write(tilelist[i].m_Z);
                                                binmul.Write(tilelist[i].m_Hue);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        bool firstitem = true;
                                        for (int i = 0; i < count; ++i)
                                        {
                                            ushort graphic;
                                            short shue;
                                            byte sx, sy;
                                            sbyte sz;
                                            if ((tox <= x) && (x <= tox2) && (toy <= y) && (y <= toy2))
                                            {
                                                graphic = m_StaticsReader_copy.ReadUInt16();
                                                sx = m_StaticsReader_copy.ReadByte();
                                                sy = m_StaticsReader_copy.ReadByte();
                                                sz = m_StaticsReader_copy.ReadSByte();
                                                shue = m_StaticsReader_copy.ReadInt16();
                                            }
                                            else
                                            {
                                                graphic = m_StaticsReader.ReadUInt16();
                                                sx = m_StaticsReader.ReadByte();
                                                sy = m_StaticsReader.ReadByte();
                                                sz = m_StaticsReader.ReadSByte();
                                                shue = m_StaticsReader.ReadInt16();
                                            }

                                            if ((graphic >= 0) && (graphic < Ultima.Art.StaticLength))
                                            {
                                                if (shue < 0)
                                                    shue = 0;
                                                if (firstitem)
                                                {
                                                    binidx.Write((int)fsmul.Position); //lookup
                                                    firstitem = false;
                                                }
                                                binmul.Write(graphic);
                                                binmul.Write(sx);
                                                binmul.Write(sy);
                                                binmul.Write(sz);
                                                binmul.Write(shue);
                                            }
                                        }
                                    }
                                    fsmullength = (int)fsmul.Position - fsmullength;
                                    if (fsmullength > 0)
                                    {
                                        binidx.Write(fsmullength); //length
                                        binidx.Write(extra); //extra
                                    }
                                    else
                                    {
                                        binidx.Write((int)-1); //lookup
                                        binidx.Write((int)-1); //length
                                        binidx.Write((int)-1); //extra
                                    }
                                }
                                progressBar1.PerformStep();
                            }
                        }
                    }
                }
                m_IndexReader.Close();
                m_StaticsReader.Close();
                m_Index_copy.Close();
                m_StaticsReader_copy.Close();
            }

            MessageBox.Show(this, String.Format("Files saved to {0}", AppDomain.CurrentDomain.SetupInformation.ApplicationBase), "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void OnClickRefresh(object sender, EventArgs e)
        {
            this.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            string sourMap = Ultima.Files.GetFilePath(String.Format("map{0}.mul", workingmap.FileIndex));
            string destMap = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, String.Format("map{0}_bak.mul", workingmap.FileIndex));

            string sourNewmap = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, String.Format("map{0}.mul", workingmap.FileIndex));
            string destNewmap = Ultima.Files.GetFilePath(String.Format("map{0}.mul", workingmap.FileIndex));

            string sourStamul = Ultima.Files.GetFilePath(String.Format("statics{0}.mul", workingmap.FileIndex));
            string destStamul = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, String.Format("statics{0}_bak.mul", workingmap.FileIndex));

            string sourNewstamul = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, String.Format("statics{0}.mul", workingmap.FileIndex));
            string destNewstamul = Ultima.Files.GetFilePath(String.Format("statics{0}.mul", workingmap.FileIndex));

            string sourMapStaidx = Ultima.Files.GetFilePath(String.Format("staidx{0}.mul", workingmap.FileIndex));
            string destMapStaidx = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, String.Format("staidx{0}_bak.mul", workingmap.FileIndex));

            string sourNewstaidx = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, String.Format("staidx{0}.mul", workingmap.FileIndex));
            string destNewstaidx = Ultima.Files.GetFilePath(String.Format("staidx{0}.mul", workingmap.FileIndex));

            if (!File.Exists(sourMap) || !File.Exists(sourNewmap) || !File.Exists(sourStamul) || !File.Exists(sourNewstamul) || !File.Exists(sourMapStaidx) || !File.Exists(sourNewstaidx))
            {
                Cursor.Current = Cursors.Default;
                this.Enabled = true;
                return;
            }

            if (File.Exists(destMap))
                File.Delete(destMap);
            if (File.Exists(destStamul))
                File.Delete(destStamul);
            if (File.Exists(destMapStaidx))
                File.Delete(destMapStaidx);

            try
            {
                System.IO.File.Move(sourMap, destMap);
                System.IO.File.Move(sourNewmap, destNewmap);

                System.IO.File.Move(sourStamul, destStamul);
                System.IO.File.Move(sourNewstamul, destNewstamul);

                System.IO.File.Move(sourMapStaidx, destMapStaidx);
                System.IO.File.Move(sourNewstaidx, destNewstaidx);

                if (FiddlerControls.Options.LoadedUltimaClass["Map"])
                {
                    Ultima.Files.CheckForNewMapSize();
                    Ultima.Map.Reload();
                }

                FiddlerControls.Events.FireFilePathChangeEvent();
            }
            catch (Exception exception)
            {
                throw;
            }

            Cursor.Current = Cursors.Default;
            this.Enabled = true;
        }

        #region ValueChanged 

        private void numericUpDownId_ValueChanged(object sender, EventArgs e)
        {
            string copymap = Path.Combine(textBox1.Text, String.Format("map{0}.mul", numericUpDownId.Value));
            if (!File.Exists(copymap))
                return;
            int w, h;
            switch(new FileInfo(copymap).Length) {
                case  77070336: w =  6144; h = 4096; break; // old Felucca\Trammel
                case  89915392: w =  7168; h = 4096; break; // new Felucca\Trammel & uoquint Dangeon
                case  11289600: w =  2304; h = 1600; break; // Ilshenar
                case  16056320: w =  2560; h = 2048; break; // Malas
                case   6421156: w =  1448; h = 1448; break; // Tokuno
                //case  16056320: w =  1280; h = 4096; break; // TerMur
                case 308281344: w = 12288; h = 8192; break; // uoquint Sossaria
                default       : w =     0; h =    0; break;
            }
            if (w > 0) numericUpDownWidth.Value  = w;
            if (h > 0) numericUpDownHeight.Value = h;
        }

        private void numericUpDownWidth_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownX1.Value = Math.Min(numericUpDownX1.Value, numericUpDownWidth.Value);
            numericUpDownX2.Value = Math.Min(numericUpDownX2.Value, numericUpDownWidth.Value);
            numericUpDownX1.Maximum = numericUpDownX2.Maximum = numericUpDownWidth.Value;
        }

        private void numericUpDownHeight_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownY1.Value = Math.Min(numericUpDownY1.Value, numericUpDownHeight.Value);
            numericUpDownY2.Value = Math.Min(numericUpDownY2.Value, numericUpDownHeight.Value);
            numericUpDownY1.Maximum = numericUpDownY2.Maximum = numericUpDownHeight.Value;
        }

        private void numericUpDownW_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownW.Value = Math.Min(((int)numericUpDownW.Value / 8) * 8, numericUpDownWidth.Value - numericUpDownX1.Value);           
            if (numericUpDownW.Value > 0)
                numericUpDownX2.Value = numericUpDownX1.Value + numericUpDownW.Value - 1;          
        }

        private void numericUpDownH_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownH.Value = Math.Min(((int)numericUpDownH.Value / 8) * 8, numericUpDownHeight.Value - numericUpDownY1.Value);
            if (numericUpDownH.Value > 0)
                numericUpDownY2.Value = numericUpDownY1.Value + numericUpDownH.Value - 1;    
        }

        private void numericUpDownX1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownX1.Value = ((int)numericUpDownX1.Value / 8) * 8;
            numericUpDownX2.Value = numericUpDownX1.Value + numericUpDownW.Value - 1;
        }

        private void numericUpDownX2_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownX2.Value = ((int)numericUpDownX2.Value / 8) * 8 + 7;
            numericUpDownW.Value = ((numericUpDownX2.Value - numericUpDownX1.Value) / 8) * 8 + 1;
        }

        private void numericUpDownY1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownY1.Value = ((int)numericUpDownY1.Value / 8) * 8;
            numericUpDownY2.Value = numericUpDownY1.Value + numericUpDownH.Value - 1;
        }

        private void numericUpDownY2_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownY2.Value = ((int)numericUpDownY2.Value / 8) * 8 + 7;
            numericUpDownH.Value = ((numericUpDownY2.Value - numericUpDownY1.Value) / 8) * 8 + 1;
        }

        private void numericUpDownToX1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownToX1.Value = ((int)numericUpDownToX1.Value / 8) * 8;
        }

        private void numericUpDownToY1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownToY1.Value = ((int)numericUpDownToY1.Value / 8) * 8;
        }

        #endregion
        
    }
}
