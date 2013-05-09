using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Ultima;

namespace FiddlerControls
{
    public partial class MapCombine : Form
    {
        public MapCombine(Ultima.Map currmap)
        {
            InitializeComponent();
            textBoxPath0.Text = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        }

        private void buttonBrowser_Click(object sender, EventArgs e)
        {
            Button element = sender as Button;
            if (element == null)
                return;

            int mapnum = (element == buttonBrowser0) ? 0 : (element == buttonBrowser1) ? 1 : (element == buttonBrowser2) ? 2 : (element == buttonBrowser3) ? 3 : (element == buttonBrowser4) ? 4 : -1;
            if (mapnum < 0)
                return;

            TextBox textBox = (mapnum == 0) ? textBoxPath0 : (mapnum == 1) ? textBoxPath1 : (mapnum == 2) ? textBoxPath2 : (mapnum == 3) ? textBoxPath3 : (mapnum == 4) ? textBoxPath4 : null;

            if(mapnum != 0)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.Title = "Выбирите *.mul файл";
                dialog.CheckFileExists = true;
                dialog.Filter = "map files|map*.mul|staidx files|staidx*.mul|statics files|statics*.mul|All mul files(*.mull)|*.mul";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (!File.Exists(dialog.FileName))
                        return;
                    textBox.Text = dialog.FileName;
                }
                dialog.Dispose();
            }
            else
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "Выбирите *.mul файл";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = dialog.SelectedPath;
                }
                dialog.Dispose();
            }
        }

        private void OnLeave_MapSizeEdit(object sender, EventArgs e)
        {
            NumericUpDown element = sender as NumericUpDown;
            if (element == null)
                return;
            
            int value = (int)element.Value;
            element.Value = (value >> 3) << 3;

            textBoxSize0.Text = String.Format("{0} x {1}",
                                              Math.Max(numericUpDownWidth1.Value + numericUpDownWidth3.Value,
                                                       numericUpDownWidth2.Value + numericUpDownWidth4.Value),
                                              Math.Max(numericUpDownHeight1.Value + numericUpDownHeight3.Value,
                                                       numericUpDownHeight2.Value + numericUpDownHeight4.Value));
        }

        private void OnLeave_PathEdit(object sender, EventArgs e)
        {
            TextBox element = sender as TextBox;
            if (element == null)
                return;
            
            int mapnum = (element == textBoxPath0) ? 0 : (element == textBoxPath1) ? 1 : (element == textBoxPath2) ? 2 : (element == textBoxPath3) ? 3 : (element == textBoxPath4) ? 4 : -1;
            if(mapnum <= 0)
                return;
            
            CheckBox checkBoxMap = (mapnum == 1) ? checkBoxMap1 : (mapnum == 2) ? checkBoxMap2 : (mapnum == 3) ? checkBoxMap3 : (mapnum == 4) ? checkBoxMap4 : null;
            CheckBox checkBoxStatics = (mapnum == 1) ? checkBoxStatics1 : (mapnum == 2) ? checkBoxStatics2 : (mapnum == 3) ? checkBoxStatics3 : (mapnum == 4) ? checkBoxStatics4 : null;
            NumericUpDown numericUpDownWidth = (mapnum == 1) ? numericUpDownWidth1 : (mapnum == 2) ? numericUpDownWidth2 : (mapnum == 3) ? numericUpDownWidth3 : (mapnum == 4) ? numericUpDownWidth4 : null;
            NumericUpDown numericUpDownHeight = (mapnum == 1) ? numericUpDownHeight1 : (mapnum == 2) ? numericUpDownHeight2 : (mapnum == 3) ? numericUpDownHeight3 : (mapnum == 4) ? numericUpDownHeight4 : null;
            NumericUpDown numericUpDownAlt = (mapnum == 1) ? numericUpDownAlt1 : (mapnum == 2) ? numericUpDownAlt2 : (mapnum == 3) ? numericUpDownAlt3 : (mapnum == 4) ? numericUpDownAlt4 : null;
            Label labelWidth = (mapnum == 1) ? label2 : (mapnum == 2) ? label7 : (mapnum == 3) ? label11 : (mapnum == 4) ? label15 : null;
            Label labelHeight = (mapnum == 1) ? label3 : (mapnum == 2) ? label6 : (mapnum == 3) ? label10 : (mapnum == 4) ? label14 : null;
            Label labelAlt = (mapnum == 1) ? label4 : (mapnum == 2) ? label5 : (mapnum == 3) ? label9 : (mapnum == 4) ? label13 : null;

            bool enabled = !String.IsNullOrEmpty(element.Text);
            checkBoxMap.Enabled = enabled;
            checkBoxStatics.Enabled = enabled;
            numericUpDownWidth.Enabled = enabled;
            numericUpDownHeight.Enabled = enabled;
            numericUpDownAlt.Enabled = enabled;
            labelWidth.Enabled = enabled;
            labelHeight.Enabled = enabled;
            labelAlt.Enabled = enabled;
        }

        private class MapInfo
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

                    for(int i = 0; i < filename.Length; ++i)
                    {
                        if( Char.IsDigit(filename[i]) )
                        {
                            name += filename[i];
                            if(i + 1 == filename.Length)
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
                set { _width = (value>>3)<<3; }
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
            public bool CopyMap;
            public bool CopyStatics;

            public MapInfo(MapCombine form, int mapnum)
            {
                if (mapnum < 0)
                    return;

                if (mapnum == 0)
                {
                    Index = (int)form.numericUpDownIndex0.Value;
                    Width = (int) Math.Max(form.numericUpDownWidth1.Value + form.numericUpDownWidth3.Value,
                                           form.numericUpDownWidth2.Value + form.numericUpDownWidth4.Value);
                    Height = (int) Math.Max(form.numericUpDownHeight1.Value + form.numericUpDownHeight3.Value,
                                            form.numericUpDownHeight2.Value + form.numericUpDownHeight4.Value);
                    CopyMap = form.checkBoxMap1.Checked | form.checkBoxMap2.Checked | form.checkBoxMap3.Checked | form.checkBoxMap4.Checked;
                    CopyStatics = form.checkBoxStatics1.Checked | form.checkBoxStatics2.Checked | form.checkBoxStatics3.Checked | form.checkBoxStatics4.Checked;
                }
                else
                {
                    CheckBox checkBoxMap = (mapnum == 1) ? form.checkBoxMap1 : (mapnum == 2) ? form.checkBoxMap2 : (mapnum == 3) ? form.checkBoxMap3 : (mapnum == 4) ? form.checkBoxMap4 : null;
                    CheckBox checkBoxStatics = (mapnum == 1) ? form.checkBoxStatics1 : (mapnum == 2) ? form.checkBoxStatics2 : (mapnum == 3) ? form.checkBoxStatics3 : (mapnum == 4) ? form.checkBoxStatics4 : null;
                    NumericUpDown numericUpDownWidth = (mapnum == 1) ? form.numericUpDownWidth1 : (mapnum == 2) ? form.numericUpDownWidth2 : (mapnum == 3) ? form.numericUpDownWidth3 : (mapnum == 4) ? form.numericUpDownWidth4 : null;
                    NumericUpDown numericUpDownHeight = (mapnum == 1) ? form.numericUpDownHeight1 : (mapnum == 2) ? form.numericUpDownHeight2 : (mapnum == 3) ? form.numericUpDownHeight3 : (mapnum == 4) ? form.numericUpDownHeight4 : null;
                    NumericUpDown numericUpDownAlt = (mapnum == 1) ? form.numericUpDownAlt1 : (mapnum == 2) ? form.numericUpDownAlt2 : (mapnum == 3) ? form.numericUpDownAlt3 : (mapnum == 4) ? form.numericUpDownAlt4 : null;

                    if (checkBoxMap == null || checkBoxStatics == null || numericUpDownWidth == null || numericUpDownHeight == null || numericUpDownAlt == null)
                        throw new NullReferenceException();
                    CopyMap = checkBoxMap.Checked;
                    CopyStatics = checkBoxStatics.Checked;
                    Width = (int) numericUpDownWidth.Value;
                    Height = (int) numericUpDownHeight.Value;
                    Alt = (int) numericUpDownAlt.Value;
                }

                TextBox textBox = (mapnum == 0) ? form.textBoxPath0 : (mapnum == 1) ? form.textBoxPath1 : (mapnum == 2) ? form.textBoxPath2 : (mapnum == 3) ? form.textBoxPath3 : (mapnum == 4) ? form.textBoxPath4 : null;

                if (textBox == null)
                    throw new NullReferenceException();
                if (!String.IsNullOrEmpty(textBox.Text))
                    FilesPaths = textBox.Text;
                else
                {
                    CopyMap = CopyStatics = false;
                    Width = Height = Alt = 0;
                    return;
                }
            }

            internal struct Data
            {
                public Block[]  m_Bloak;
            }

            //Block Number = (XBlock * 512) + YBlock 
            internal struct Block
            {
                public uint     m_Header;
                public Tile[]   m_Tiles;

                public uint     m_Offset;
                public uint     m_Length;
                public uint     m_Unknown;
                public Static[] m_Statics;
            }

            internal struct Tile
            {
                public ushort   m_ID;
                public sbyte    m_Z;
            }

            internal struct Static
            {
                public ushort   m_ID;
                public byte     m_X;
                public byte     m_Y;
                public sbyte    m_Z;
                public short    m_Hue;
            }

            private BinaryReader MapMul;
            private BinaryReader StaIdx;
            private BinaryReader StaMul;
            private BinaryWriter MapMulW;
            private BinaryWriter StaIdxW;
            private BinaryWriter StaMulW;
            public void Create()
            {
                MapMulW = new BinaryWriter(new FileStream(MapMulPath, FileMode.Create, FileAccess.Write, FileShare.None));
                StaIdxW = new BinaryWriter(new FileStream(StaIdxPath, FileMode.Create, FileAccess.Write, FileShare.None));
                StaMulW = new BinaryWriter(new FileStream(StaMulPath, FileMode.Create, FileAccess.Write, FileShare.None));
            }
            
            public void Open()
            {
                if (CopyMap)
                    MapMul = new BinaryReader(new FileStream( MapMulPath, FileMode.Open, FileAccess.Read, FileShare.Read ));
                if (CopyStatics)
                {
                    StaIdx = new BinaryReader(new FileStream( StaIdxPath, FileMode.Open, FileAccess.Read, FileShare.Read ));
                    StaMul = new BinaryReader(new FileStream( StaMulPath, FileMode.Open, FileAccess.Read, FileShare.Read ));
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

            public Block[] ReadLine()
            {
                if( MapMul.BaseStream.Position == MapMul.BaseStream.Length - 1)
                    return null;

                Block[] block = new Block[Height >> 3];
                for (int i = 0; i < (Height >> 3); ++i)
                {
                    block[i] = new Block();

                    block[i].m_Header = MapMul.ReadUInt32();
                    block[i].m_Tiles = new Tile[64];
                    for (int k = 0; k < 64; ++k)
                    {
                        block[i].m_Tiles[k].m_ID = MapMul.ReadUInt16();
                        block[i].m_Tiles[k].m_Z = MapMul.ReadSByte();
                        block[i].m_Tiles[k].m_Z += (sbyte)Alt;
                    }

                    block[i].m_Offset = StaIdx.ReadUInt32();
                    block[i].m_Length = StaIdx.ReadUInt32();
                    block[i].m_Unknown = StaIdx.ReadUInt32();

                    if (block[i].m_Offset == 0xFFFFFFFF || block[i].m_Length == 0)
                        continue;
                    StaMul.BaseStream.Position = block[i].m_Offset;
                    block[i].m_Statics = new Static[block[i].m_Length/7];
                    for (int k = 0; k < block[i].m_Length / 7; ++k)
                    {
                        block[i].m_Statics[k].m_ID = StaMul.ReadUInt16();
                        block[i].m_Statics[k].m_X = StaMul.ReadByte();
                        block[i].m_Statics[k].m_Y = StaMul.ReadByte();
                        block[i].m_Statics[k].m_Z = StaMul.ReadSByte();
                        block[i].m_Statics[k].m_Z += (sbyte)Alt;
                        block[i].m_Statics[k].m_Hue = StaMul.ReadInt16();
                    }
                }

                return block;
            }

            public void Write(Block[] block)
            {
                for (int i = 0; i < block.Length; ++i)
                {
                    MapMulW.Write(block[i].m_Header);
                    for (int k = 0; k < 64; ++k)
                    {
                        MapMulW.Write(block[i].m_Tiles[k].m_ID);
                        MapMulW.Write(block[i].m_Tiles[k].m_Z);
                    }

                    if (block[i].m_Offset != 0xFFFFFFFF && block[i].m_Length != 0)
                        block[i].m_Offset = (uint)StaMulW.BaseStream.Position;
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
        }
       
        private void buttonCombine_Click(object sender, EventArgs e)
        {
            MapInfo mapInf0 = new MapInfo(this, 0);
            MapInfo mapInf1 = new MapInfo(this, 1);
            MapInfo mapInf2 = new MapInfo(this, 2);
            MapInfo mapInf3 = new MapInfo(this, 3);
            MapInfo mapInf4 = new MapInfo(this, 4);

            mapInf0.Create();
            mapInf1.Open();
            mapInf2.Open();
            mapInf3.Open();
            mapInf4.Open();

            MapInfo.Data[] data1 = new MapInfo.Data[mapInf1.Width >> 3];
            MapInfo.Data[] data2 = new MapInfo.Data[mapInf2.Width >> 3];
            MapInfo.Data[] data3 = new MapInfo.Data[mapInf3.Width >> 3];
            MapInfo.Data[] data4 = new MapInfo.Data[mapInf4.Width >> 3];
            
            for(int f = 0; f < data1.Length; ++f)
                data1[f].m_Bloak = mapInf1.ReadLine();
            for (int f = 0; f < data2.Length; ++f)
                data2[f].m_Bloak = mapInf2.ReadLine();
            for (int f = 0; f < data3.Length; ++f)
                data3[f].m_Bloak = mapInf3.ReadLine();
            for (int f = 0; f < data4.Length; ++f)
                data4[f].m_Bloak = mapInf4.ReadLine();

            for (int f = 0; f < data1.Length; ++f)
            {
                mapInf0.Write(data1[f].m_Bloak);
                mapInf0.Write(data3[f].m_Bloak);
            }
            for (int f = 0; f < data1.Length; ++f)
            {
                mapInf0.Write(data2[f].m_Bloak);
                mapInf0.Write(data4[f].m_Bloak);
            }

            mapInf0.Close();
            mapInf1.Close();
            mapInf2.Close();
            mapInf3.Close();
            mapInf4.Close();

        }
    }
}
