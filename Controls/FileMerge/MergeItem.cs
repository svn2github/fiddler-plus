/***************************************************************************
 *                                               Created by :        StaticZ
 *                  MergeItem.cs                 UO Quintessense server team
 *              ____________________             url   :   http://uoquint.ru
 *              Version : 16/12/2010             email :   uoquint@gmail.com 
 *                                               ---------------------------
 * History :
 *   16/12/2010 First realize
 *
 * Todo :
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using Ultima;


namespace FiddlerControls.FileMerge
{
    public partial class MergeItem : UserControl
    {
        public MergeItem()
        {
            InitializeComponent();

            for (int i = 1; i < EnumNames.Length; ++i)
            {
                if (EnumNames[i].StartsWith("UnUsed"))
                    continue;
                checkedListBoxTileFlags1.Items.Add(EnumNames[i], false);
                checkedListBoxTileFlags2.Items.Add(EnumNames[i], false);
            }

            mPreviewPanelHeightOffset = (int)(tableLayoutPanel4.RowStyles[1].Height + 20);
            mPreviewPanelWeightOffset = 8;
        }

        public MergeItem(Control parent) : this()
        {
            Cursor.Current = Cursors.WaitCursor;
            mParentControls = new List<Control>(parent.Controls.Count);
            foreach (Control control in parent.Controls)
                if (control.Visible)
                {
                    mParentControls.Add(control);
                    control.Visible = false;
                }

            this.Parent = parent;
            this.Dock = DockStyle.Fill;
            mMinimumSize = parent.MinimumSize;
            parent.MinimumSize = new Size(MinimumSize.Width + parent.Size.Width - parent.ClientSize.Width, MinimumSize.Height + parent.Size.Height - parent.ClientSize.Height);
            Cursor.Current = Cursors.Default;
        }

        private Size mMinimumSize;
        private List<Control> mParentControls;

        public void Close()
        {
            Cursor.Current = Cursors.WaitCursor;
            if (backgroundWorker.IsBusy)
                backgroundWorker.CancelAsync();
            Ultima.Secondary.Art.Dispose();
            Ultima.Secondary.TileData.Dispose();
            Ultima.Secondary.RadarCol.Dispose();

            //this.Visible = false;
            this.DestroyHandle();
            foreach (Control control in mParentControls)
                control.Visible = true;

            this.Parent.MinimumSize = mMinimumSize;
            Cursor.Current = Cursors.Default;
        }

        ~MergeItem()
        {
            Close();
        }

        private int maxItemindex;

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }  

        #region Загрузка

        private void loadButton_Click(object sender, EventArgs e)
        {
            /*
            listViewItem1.BeginUpdate(); 
            listViewItem2.BeginUpdate();
            for (int index = 0xD500; index < 0xEC80; ++index)
            {
                if (!Ultima.Art.IsValidStatic(index))
                    continue;

                ListViewItem item1 = new ListViewItem(index.ToString(), 0);
                ListViewItem item2 = new ListViewItem(index.ToString(), 0);
                item1.Tag = item2.Tag = index;
                listViewItem1.Items.Add(item1);
                listViewItem2.Items.Add(item2);

            }
            listViewItem1.EndUpdate(); 
            listViewItem2.EndUpdate();
            return;
            */

            string path = textBoxDirectory.Text;
            if (String.IsNullOrEmpty(path))
            {
                MessageBox.Show(String.Format("Не указан путь к директории\nЗагрузка будет прервана.", AppDomain.CurrentDomain.SetupInformation.ApplicationBase), "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (!Directory.Exists(path))
            {
                MessageBox.Show(String.Format("Указанный путь не существует\nЗагрузка будет прервана.", AppDomain.CurrentDomain.SetupInformation.ApplicationBase), "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            string idxPath = Path.Combine(path, "artidx.mul");
            string mulPath = Path.Combine(path, "art.mul");
            if (!File.Exists(idxPath) || !File.Exists(mulPath))
            {
                MessageBox.Show(String.Format("В указанной папке не найдены необходимые файлы \"artidx.mul\" и \"art.mul\"\nЗагрузка будет прервана.", AppDomain.CurrentDomain.SetupInformation.ApplicationBase), "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            Ultima.Secondary.Art.Dispose();
            if (!Ultima.Secondary.Art.SetFileIndex(idxPath, mulPath))
            {
                MessageBox.Show(String.Format("Не удалось инициализировать \"artidx.mul\" и \"art.mul\"\nЗагрузка будет прервана.", AppDomain.CurrentDomain.SetupInformation.ApplicationBase), "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            Ultima.Secondary.TileData.Dispose();
            if (!Ultima.Secondary.TileData.SetFile(Path.Combine(path, "tiledata.mul")))
            {
                MessageBox.Show(String.Format("Не удалось инициализировать \"tiledata.mul\"", AppDomain.CurrentDomain.SetupInformation.ApplicationBase), "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            Ultima.Secondary.RadarCol.Dispose();
            if (!Ultima.Secondary.RadarCol.SetFile(Path.Combine(path, "radarcol.mul")))
            {
                MessageBox.Show(String.Format("Не удалось инициализировать \"radarcol.mul\"", AppDomain.CurrentDomain.SetupInformation.ApplicationBase), "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            listViewItem1.BeginUpdate();    listViewItem2.BeginUpdate();
            listViewItem1.Clear();          listViewItem2.Clear();
            listViewItem1.EndUpdate();      listViewItem2.EndUpdate();
            itemIndex.Clear();

            int maxitemindex1 = Ultima.Art.StaticLength;
            int maxitemindex2 = Ultima.Secondary.Art.IsUOHS() ? 0xFFDC : Ultima.Secondary.Art.IsUOSA() ? 0x8000 : 0x4000;
            maxItemindex = Math.Min(maxitemindex1, maxitemindex2);

            loadPercent = toolStripProgressBar.Value = 0;
            toolStripProgressStatusLabel.Text = "0%";

            textBoxDirectory.Enabled = false;
            loadButton.Enabled = false;

            backgroundWorker.RunWorkerAsync();

        }

        private struct BackgroundWorkerState
        {
            public bool AddItim;
            public int Index;
            public Differences Flags;
            public ListViewItem item1;
            public ListViewItem item2;
            public BackgroundWorkerState(int index)
            {
                AddItim = false;
                Index = index;
                Flags = Differences.Empty;
                item1 = item2 = null;
            }
            public BackgroundWorkerState(int index, Differences flags)
            {
                AddItim = true;
                Index = index;
                Flags = flags;
                item1 = new ListViewItem(index.ToString(), 0);
                item2 = new ListViewItem(index.ToString(), 0);
                item1.Tag = item2.Tag = index;
            }
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int percent;            
            for (int index = 1; index < maxItemindex; ++index)
            {
                HashComputation(index);
                percent = 50 * index / maxItemindex;
                backgroundWorker.ReportProgress(percent, new BackgroundWorkerState(index));
            }
            for (int index = 1; index < maxItemindex; ++index)
            {
                Differences diff = Compare(index);
                percent = 50 + 50 * index / maxItemindex;
                backgroundWorker.ReportProgress(percent, new BackgroundWorkerState(index, diff));
            }

            e.Result = 0;
            //System.Threading.Thread.Sleep(50);
        }        

        private int loadPercent;

        private void backgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (loadPercent != e.ProgressPercentage)
            {
                loadPercent = e.ProgressPercentage;
                toolStripProgressStatusLabel.Text = String.Format("{0:D}%", loadPercent);
                toolStripProgressBar.Value = loadPercent;
                Application.DoEvents();
            }

            BackgroundWorkerState state = (BackgroundWorkerState)e.UserState;
            if (state.AddItim && (state.Flags & (Differences.None | Differences.Unknown)) == 0)
            {
                itemIndex.Add(state.Index, listViewItem2.Items.Count);

                //listViewItem1.BeginUpdate();        
                listViewItem1.Items.Add(state.item1); 
                //listViewItem1.EndUpdate();          

                //listViewItem2.BeginUpdate();
                listViewItem2.Items.Add(state.item2);
                //listViewItem2.EndUpdate();
            }

            //Application.DoEvents();   
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            textBoxDirectory.Enabled = true;
            loadButton.Enabled = true;
            toolStripProgressBar.Value = 100;
            toolStripProgressStatusLabel.Text = "100%" + " Всего: " + listViewItem1.Items.Count.ToString() + " тайлов.";
        }

        #endregion

        #region Сравнение

        [Flags]
        private enum Differences : byte
        {
            Empty           = 0x00,
            None            = 0x01,
            Added           = 0x02,
            Replaced        = 0x04,
            Deleted         = 0x08,
            Moved           = 0x10,
            TiledataChanged = 0x20,
            RadarColChanged = 0x40,
            Unknown         = 0x80 
        }

        private Dictionary<int, string> m_HashOrg = new Dictionary<int, string>();
        private Dictionary<int, string> m_HashSec = new Dictionary<int, string>();
        private Dictionary<int, Differences> m_Compare = new Dictionary<int, Differences>();
        private SHA256Managed shaM = new SHA256Managed();
        private System.Drawing.ImageConverter ic = new System.Drawing.ImageConverter();

        private void HashComputation(int index)
        {
            if (m_Compare.ContainsKey(index))
                return;

            Bitmap bitorg = Ultima.Art.GetStatic(index);
            Bitmap bitsec = Ultima.Secondary.Art.GetStatic(index);
            if ((bitorg == null) && (bitsec == null))
                m_Compare[index] = Differences.None;
            else if ((bitorg == null) && (bitsec != null))
                m_Compare[index] = Differences.Added;
            else if ((bitorg != null) && (bitsec == null))
                m_Compare[index] = Differences.Deleted;

            if (!m_HashOrg.ContainsKey(index))
            {
                byte[] btImage1 = new byte[1];
                btImage1 = (byte[])ic.ConvertTo(bitorg, btImage1.GetType());
                m_HashOrg[index] = BitConverter.ToString(shaM.ComputeHash(btImage1));
            }
            if (!m_HashSec.ContainsKey(index))
            {
                byte[] btImage2 = new byte[1];
                btImage2 = (byte[])ic.ConvertTo(bitsec, btImage2.GetType());
                m_HashSec[index] = BitConverter.ToString(shaM.ComputeHash(btImage2));
            }
        }

        private Differences Compare(int index)
        {
            if (m_Compare.ContainsKey(index))
                return m_Compare[index];

            Differences diff = m_HashOrg[index] == m_HashSec[index] ? Differences.None : Differences.Replaced;
            if (diff == Differences.Replaced)
            {
                string hashsec = m_HashSec[index];
                foreach (string hash in m_HashOrg.Values)
                    if (hashsec == hash)
                    {
                        diff = Differences.Moved;
                        break;
                    }
            }

            return m_Compare[index] = diff;
        }

        private Differences GetDiff(int index)
        {
            if (m_Compare.ContainsKey(index))
                return m_Compare[index];
            else
                return Differences.Unknown;
        }

        private bool IsEqual(int index, Differences diff)
        {
            return IsEqual(GetDiff(index), diff);
        }

        private bool IsEqual(Differences diff1, Differences diff2)
        {
            return ((diff1 & diff2) == diff2);
        }

        #endregion

        private readonly Brush BrushWhite = Brushes.White;
        private readonly Brush BrushLightBlue = Brushes.LightBlue;
        private readonly Brush BrushLightCoral = Brushes.LightCoral;
        private readonly Brush BrushRed = Brushes.Red;
        private readonly Brush BrushBlueViolet = Brushes.BlueViolet;

        private readonly Brush BrushAdded = Brushes.LightGreen;
        private readonly Brush BrushMoved = Brushes.Yellow;
        private readonly Brush BrushDeleted = Brushes.Red;

        private readonly Pen PenGray = Pens.Gray;
        private readonly Pen PenBlack = Pens.Black;
        private readonly Pen PenCadetBlue = Pens.CadetBlue;

        private void DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            int i = (int)e.Item.Tag;
            bool patched;

            ListView listview = sender as ListView;
            Bitmap bmp = (listview == listViewItem1) ? Ultima.Art.GetStatic(i, out patched) : (listview == listViewItem2) ? Ultima.Secondary.Art.GetStatic(i) : null;

            if (bmp == null)
            {
                if (e.Item.Selected)//((e.State & ListViewItemStates.Focused) != 0)
                    e.Graphics.FillRectangle(BrushLightBlue, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                else
                    e.Graphics.DrawRectangle(PenGray, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                e.Graphics.FillRectangle(BrushRed, e.Bounds.X + 5, e.Bounds.Y + 5, e.Bounds.Width - 10, e.Bounds.Height - 10);
                return;
            }
            else
            {
                Differences diff = GetDiff(i);
                 
                if (e.Item.Selected)//((e.State & ListViewItemStates.Focused) != 0)
                    e.Graphics.FillRectangle(BrushLightBlue, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                else if (IsEqual(diff, Differences.Added))
                    e.Graphics.FillRectangle(BrushAdded, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                else if (IsEqual(diff, Differences.Moved))
                    e.Graphics.FillRectangle(BrushMoved, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                else if (IsEqual(diff, Differences.Deleted))
                    e.Graphics.FillRectangle(BrushDeleted, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

                //else if (ChangesInTiledataOrRadarCol[LandMaxIndex + i])
                //    e.Graphics.FillRectangle(BrushBlueViolet, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                //else if (patched)
                //    e.Graphics.FillRectangle(BrushLightGreenl, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                else
                    e.Graphics.FillRectangle(BrushWhite, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

                if (Options.ArtItemClip)
                {
                    e.Graphics.DrawImage(bmp, e.Bounds.X + 1, e.Bounds.Y + 1,
                                         new Rectangle(0, 0, e.Bounds.Width - 1, e.Bounds.Height - 1),
                                         GraphicsUnit.Pixel);
                }
                else
                {
                    int width = bmp.Width;
                    int height = bmp.Height;
                    if (width > e.Bounds.Width)
                    {
                        width = e.Bounds.Width;
                        height = e.Bounds.Height * bmp.Height / bmp.Width;
                    }
                    if (height > e.Bounds.Height)
                    {
                        height = e.Bounds.Height;
                        width = e.Bounds.Width * bmp.Width / bmp.Height;
                    }
                    e.Graphics.DrawImage(bmp,
                                         new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, width, height));
                }
                if (!e.Item.Selected)//((e.State & ListViewItemStates.Focused) == 0)
                    e.Graphics.DrawRectangle(PenGray, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                else
                    e.Graphics.DrawRectangle(PenBlack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
            }
        }

        private enum SelectedIndexStatus : byte
        {
            None    = 0x00,
            Added   = 0x02,
            Removed = 0x04
        }

        private List<int> listViewItemSelected = new List<int>();
        private Dictionary<int, int> itemIndex = new Dictionary<int, int>();

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = -1;
            SelectedIndexStatus status = SelectedIndexStatus.None;

            ListView listView = sender as ListView;
            ListViewItem listViewItem = null;

            // Определение элемента у которого было измененно выделение и его тега (айди предмета) 
            if (listView.SelectedItems.Count > listViewItemSelected.Count)
            {
                for(int i = listView.SelectedItems.Count - 1; i >= 0; --i)
                {
                    int tag = (int)listView.SelectedItems[i].Tag;
                    if (!listViewItemSelected.Contains(tag))
                    {
                        index = tag;
                        status = SelectedIndexStatus.Added;
                        listViewItem = listView.SelectedItems[i];
                        break;
                    }
                }
            }
            else if (listView.SelectedItems.Count < listViewItemSelected.Count)
            {
                List<int> itemSelected = new List<int>(listViewItemSelected);
                for (int i = listView.SelectedItems.Count - 1; i >= 0; --i)
                {
                    int tag = (int)listView.SelectedItems[i].Tag;
                    if (itemSelected.Contains(tag))
                        itemSelected.Remove(tag);
                }
                if (itemSelected.Count > 0)
                {
                    index = itemSelected[0];
                    status = SelectedIndexStatus.Removed;
                    listViewItem = listView.Items[itemIndex[index]];
                }
            }

            // Изменение состояния выделения во втором контроле ListView и обновление списка выделенных предметов (listViewItemSelected)
            ListView secondListView = listView == listViewItem2 ? listViewItem1 : listViewItem2;
            ListViewItem secondListViewItem = listViewItem != null ? secondListView.Items[listViewItem.Index] : null;

            if (status == SelectedIndexStatus.Removed)
            {
                if (secondListViewItem != null && secondListViewItem.Selected)
                    secondListViewItem.Selected = false;
                if (listView == listViewItem2)
                {
                    listViewItemSelected.Remove(index);
                    //ShowInfoRemoveItem(index);
                }
            }
            else if (status == SelectedIndexStatus.Added)
            {
                if (secondListViewItem != null && !secondListViewItem.Selected)
                    secondListViewItem.Selected = true;
                if (listView == listViewItem2)
                {
                    listViewItemSelected.Add(index);
                    //ShowInfoAddItem(index);
                }
            } 
        }

        #region Отображение информации о выбранных предметах

        private Bitmap multiview
        { 
            get 
            {
                if (m_multiview == null)
                {
                    string text = " Предпросмотр невозможен\n(выбранно несколько тайлов)";
                    m_multiview = new Bitmap(162, 36);
                    Graphics graph = Graphics.FromImage(m_multiview);
                    graph.Clear(Color.FromKnownColor(KnownColor.Control));
                    graph.DrawString(text, Fonts.DefaultFont, Brushes.DarkGray, 0, 0);
                    graph.Flush();
                }
                return m_multiview;
            } 
        }
        private Bitmap m_multiview = null;

        private Bitmap multicolor
        {
            get
            {
                if (m_multicolor == null)
                {
                    string text = "Разные цвета";
                    m_multicolor = new Bitmap(80, 18);
                    Graphics graph = Graphics.FromImage(m_multicolor);
                    graph.Clear(Color.FromKnownColor(KnownColor.Control));
                    graph.DrawString(text, Fonts.DefaultFont, Brushes.DarkGray, 0, 0);
                    graph.Flush();
                }
                return m_multicolor;
            }
        }
        private Bitmap m_multicolor = null;

        private readonly string[] EnumNames = System.Enum.GetNames(typeof(TileFlag));
        private readonly Array EnumValues = System.Enum.GetValues(typeof(TileFlag));

        private void ShowInfoSingleItem(int index)
        {
            // Preview
            try
            {
                pictureBoxItem1.Image = Ultima.Art.GetStatic(index);
                pictureBoxItem2.Image = Ultima.Secondary.Art.GetStatic(index);
            }
            catch
            {
                Bitmap bit = new Bitmap(pictureBoxItem1.Width, pictureBoxItem1.Height);
                pictureBoxItem1.Image = bit;
                pictureBoxItem2.Image = bit;
            }

            // TileData
            ItemData data1 = Ultima.TileData.ItemTable[index];
            textBoxName1.Text = data1.Name ?? String.Empty;
            textBoxAnim1.Text = data1.Animation.ToString();
            textBoxWeight1.Text = data1.Weight.ToString();
            textBoxQuality1.Text = data1.Quality.ToString();
            textBoxQuantity1.Text = data1.Quantity.ToString();
            textBoxHue1.Text = data1.Hue.ToString();
            textBoxStackOff1.Text = data1.StackingOffset.ToString();
            textBoxValue1.Text = data1.Value.ToString();
            textBoxHeigth1.Text = data1.Height.ToString();
            textBoxUnk11.Text = data1.MiscData.ToString();
            textBoxUnk21.Text = data1.Unk2.ToString();
            textBoxUnk31.Text = data1.Unk3.ToString();

            ItemData data2 = Ultima.Secondary.TileData.ItemTable[index];
            textBoxName2.Text = data2.Name ?? String.Empty;
            textBoxAnim2.Text = data2.Animation.ToString();
            textBoxWeight2.Text = data2.Weight.ToString();
            textBoxQuality2.Text = data2.Quality.ToString();
            textBoxQuantity2.Text = data2.Quantity.ToString();
            textBoxHue2.Text = data2.Hue.ToString();
            textBoxStackOff2.Text = data2.StackingOffset.ToString();
            textBoxValue2.Text = data2.Value.ToString();
            textBoxHeigth2.Text = data2.Height.ToString();
            textBoxUnk12.Text = data2.MiscData.ToString();
            textBoxUnk22.Text = data2.Unk2.ToString();
            textBoxUnk32.Text = data2.Unk3.ToString();

            // TileFlags
            for (int i = 1, lbid = 0; i < EnumValues.Length; ++i)
            {
                if (EnumNames[i].StartsWith("UnUsed"))
                    continue;

                TileFlag mask = (TileFlag)EnumValues.GetValue(i);
                checkedListBoxTileFlags1.SetItemChecked(lbid, ((data1.Flags & mask) != 0));
                checkedListBoxTileFlags2.SetItemChecked(lbid, ((data2.Flags & mask) != 0));

                ++lbid;
            }
 
            // RadarCol
            short color1 = Ultima.RadarCol.GetItemColor(index);
            short color2 = Ultima.Secondary.RadarCol.GetItemColor(index);
            Color col1 = Ultima.Hues.HueToColor(color1);
            Color col2 = Ultima.Hues.HueToColor(color2);

            pictureBoxItemColor1.BackColor = col1;
            numericUpDownItemR1.Value = col1.R;
            numericUpDownItemG1.Value = col1.G;
            numericUpDownItemB1.Value = col1.B;
            pictureBoxItemColor1.Image = null;
            pictureBoxItemColor1.Tag = color1;

            pictureBoxItemColor2.BackColor = col2;
            numericUpDownItemR2.Value = col2.R;
            numericUpDownItemG2.Value = col2.G;
            numericUpDownItemB2.Value = col2.B;
            pictureBoxItemColor2.Image = null;
            pictureBoxItemColor2.Tag = color2;
        }

        private void ShowInfoAddItem(int index)
        {
            if (listViewItemSelected.Count == 1)
            {
                ShowInfoSingleItem(index);
                return;
            }
            
            // Preview
            pictureBoxItem1.Image = multiview;
            pictureBoxItem2.Image = multiview;

            // TileData
            ItemData data1 = Ultima.TileData.ItemTable[index];
            if (!String.IsNullOrEmpty(textBoxName1.Text) && String.Compare(textBoxName1.Text, data1.Name ?? String.Empty, false) != 0)
                textBoxName1.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxAnim1.Text) && String.Compare(textBoxAnim1.Text, data1.Animation.ToString(), false) != 0)
                textBoxAnim1.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxWeight1.Text) && String.Compare(textBoxWeight1.Text, data1.Weight.ToString(), false) != 0)
                textBoxWeight1.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxQuality1.Text) && String.Compare(textBoxQuality1.Text, data1.Quality .ToString(), false) != 0)
                textBoxQuality1.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxQuantity1.Text) && String.Compare(textBoxQuantity1.Text, data1.Quantity.ToString(), false) != 0)
                textBoxQuantity1.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxHue1.Text) && String.Compare(textBoxHue1.Text, data1.Hue.ToString(), false) != 0)
                textBoxHue1.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxStackOff1.Text) && String.Compare(textBoxStackOff1.Text, data1.StackingOffset.ToString(), false) != 0)
                textBoxStackOff1.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxValue1.Text) && String.Compare(textBoxValue1.Text, data1.Value.ToString(), false) != 0)
                textBoxValue1.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxHeigth1.Text) && String.Compare(textBoxHeigth1.Text, data1.Height.ToString(), false) != 0)
                textBoxHeigth1.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxUnk11.Text) && String.Compare(textBoxUnk11.Text, data1.MiscData.ToString(), false) != 0)
                textBoxUnk11.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxUnk21.Text) && String.Compare(textBoxUnk21.Text, data1.Unk2.ToString(), false) != 0)
                textBoxUnk21.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxUnk31.Text) && String.Compare(textBoxUnk31.Text, data1.Unk3.ToString(), false) != 0)
                textBoxUnk31.Text = String.Empty;

            ItemData data2 = Ultima.Secondary.TileData.ItemTable[index];
            if (!String.IsNullOrEmpty(textBoxName2.Text) && String.Compare(textBoxName2.Text, data1.Name ?? String.Empty, false) != 0)
                textBoxName2.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxAnim2.Text) && String.Compare(textBoxAnim2.Text, data1.Animation.ToString(), false) != 0)
                textBoxAnim2.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxWeight2.Text) && String.Compare(textBoxWeight2.Text, data1.Weight.ToString(), false) != 0)
                textBoxWeight2.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxQuality2.Text) && String.Compare(textBoxQuality2.Text, data1.Quality .ToString(), false) != 0)
                textBoxQuality2.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxQuantity2.Text) && String.Compare(textBoxQuantity2.Text, data1.Quantity.ToString(), false) != 0)
                textBoxQuantity2.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxHue2.Text) && String.Compare(textBoxHue2.Text, data1.Hue.ToString(), false) != 0)
                textBoxHue2.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxStackOff2.Text) && String.Compare(textBoxStackOff2.Text, data1.StackingOffset.ToString(), false) != 0)
                textBoxStackOff2.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxValue2.Text) && String.Compare(textBoxValue2.Text, data1.Value.ToString(), false) != 0)
                textBoxValue2.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxHeigth2.Text) && String.Compare(textBoxHeigth2.Text, data1.Height.ToString(), false) != 0)
                textBoxHeigth2.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxUnk12.Text) && String.Compare(textBoxUnk12.Text, data1.MiscData.ToString(), false) != 0)
                textBoxUnk12.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxUnk22.Text) && String.Compare(textBoxUnk22.Text, data1.Unk2.ToString(), false) != 0)
                textBoxUnk22.Text = String.Empty;
            if (!String.IsNullOrEmpty(textBoxUnk32.Text) && String.Compare(textBoxUnk32.Text, data1.Unk3.ToString(), false) != 0)
                textBoxUnk32.Text = String.Empty;

            // TileFlags
            for (int i = 1, lbid = 0; i < EnumValues.Length; ++i)
            {
                if (EnumNames[i].StartsWith("UnUsed"))
                    continue;

                TileFlag mask = (TileFlag)EnumValues.GetValue(i);

                CheckState state1 = checkedListBoxTileFlags1.GetItemCheckState(lbid);
                if(state1 != CheckState.Indeterminate)
                    if((data1.Flags & mask) != (state1 == CheckState.Checked ? (TileFlag)EnumValues.GetValue(i) : TileFlag.None))
                        checkedListBoxTileFlags1.SetItemCheckState(lbid, CheckState.Indeterminate);

                CheckState state2 = checkedListBoxTileFlags2.GetItemCheckState(lbid);
                if(state2 != CheckState.Indeterminate)
                    if((data2.Flags & mask) != (state2 == CheckState.Checked ? (TileFlag)EnumValues.GetValue(i) : TileFlag.None))
                        checkedListBoxTileFlags2.SetItemCheckState(lbid, CheckState.Indeterminate);

                ++lbid;
            }

            // RadarCol
            short color1 = Ultima.RadarCol.GetItemColor(index);
            if (pictureBoxItemColor1.Image == null && color1 != (short)pictureBoxItemColor1.Tag)
            {
                pictureBoxItemColor1.BackColor = Color.FromKnownColor(KnownColor.Control);
                pictureBoxItemColor1.Tag = 0;
                pictureBoxItemColor1.Image = multicolor;
                numericUpDownItemR1.Value = 0;
                numericUpDownItemG1.Value = 0;
                numericUpDownItemB1.Value = 0;
            }

            short color2 = Ultima.Secondary.RadarCol.GetItemColor(index);
            if (pictureBoxItemColor2.Image == null && color2 != (short)pictureBoxItemColor2.Tag)
            {
                pictureBoxItemColor2.BackColor = Color.FromKnownColor(KnownColor.Control);
                pictureBoxItemColor2.Tag = 0;
                pictureBoxItemColor2.Image = multicolor;
                numericUpDownItemR2.Value = 0;
                numericUpDownItemG2.Value = 0;
                numericUpDownItemB2.Value = 0;
            }
        }

        private void ShowInfoRemoveItem(int index)
        {
            if (listViewItemSelected.Count == 0)
            {
                pictureBoxItem1.Image = null;
                pictureBoxItem2.Image = null;

                textBoxName1.Text = String.Empty;
                textBoxAnim1.Text = String.Empty;
                textBoxWeight1.Text = String.Empty;
                textBoxQuality1.Text = String.Empty;
                textBoxQuantity1.Text = String.Empty;
                textBoxHue1.Text = String.Empty;
                textBoxStackOff1.Text = String.Empty;
                textBoxValue1.Text = String.Empty;
                textBoxHeigth1.Text = String.Empty;
                textBoxUnk11.Text = String.Empty;
                textBoxUnk21.Text = String.Empty;
                textBoxUnk31.Text = String.Empty;

                textBoxName2.Text = String.Empty;
                textBoxAnim2.Text = String.Empty;
                textBoxWeight2.Text = String.Empty;
                textBoxQuality2.Text = String.Empty;
                textBoxQuantity2.Text = String.Empty;
                textBoxHue2.Text = String.Empty;
                textBoxStackOff2.Text = String.Empty;
                textBoxValue2.Text = String.Empty;
                textBoxHeigth2.Text = String.Empty;
                textBoxUnk12.Text = String.Empty;
                textBoxUnk22.Text = String.Empty;
                textBoxUnk32.Text = String.Empty;

                for (int i = 1, lbid = 0; i < EnumValues.Length; ++i)
                {
                    if (EnumNames[i].StartsWith("UnUsed"))
                        continue;

                    checkedListBoxTileFlags1.SetItemCheckState(lbid, CheckState.Unchecked);
                    checkedListBoxTileFlags2.SetItemCheckState(lbid, CheckState.Unchecked);

                    ++lbid;
                }

                Color col = Color.FromArgb(-1);
                pictureBoxItemColor1.BackColor = col;
                numericUpDownItemR1.Value = 0;
                numericUpDownItemG1.Value = 0;
                numericUpDownItemB1.Value = 0;
                pictureBoxItemColor1.Image = null;
                pictureBoxItemColor1.Tag = 0;

                pictureBoxItemColor2.BackColor = col;
                numericUpDownItemR2.Value = 0;
                numericUpDownItemG2.Value = 0;
                numericUpDownItemB2.Value = 0;
                pictureBoxItemColor2.Image = null;
                pictureBoxItemColor2.Tag = 0;

                return;
            }
            if (listViewItemSelected.Count == 1)
            {
                ShowInfoSingleItem(index);
                return;
            }

            // Preview
            pictureBoxItem1.Image = multiview;
            pictureBoxItem2.Image = multiview;

            // TileData
            bool trig;
            ItemData data1 = Ultima.TileData.ItemTable[listViewItemSelected[0]];
            ItemData data2 = Ultima.Secondary.TileData.ItemTable[listViewItemSelected[0]];

            if (String.IsNullOrEmpty(textBoxName1.Text) ) {
                string value = data1.Name;       trig = false;
                foreach (int i in listViewItemSelected)
                    if(String.Compare(value, Ultima.TileData.ItemTable[i].Name, false) != 0)
                    {   trig = true; break;  }
                if (!trig) textBoxName1.Text = value;
            }
            if (String.IsNullOrEmpty(textBoxAnim1.Text) ) {
                short value = data1.Animation;   trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.TileData.ItemTable[i].Animation)
                    {   trig = true; break;  }
                if (!trig) textBoxAnim1.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxWeight1.Text) ) {
                short value = data1.Weight;      trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.TileData.ItemTable[i].Weight)
                    {   trig = true; break;  }
                if (!trig) textBoxWeight1.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxQuality1.Text) ) {
                short value = data1.Quality;     trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.TileData.ItemTable[i].Quality)
                    {   trig = true; break;  }
                if (!trig) textBoxQuality1.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxQuantity1.Text) ) {
                short value = data1.Quantity;    trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.TileData.ItemTable[i].Quantity)
                    {   trig = true; break;  }
                if (!trig) textBoxQuantity1.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxHue1.Text) ) {
                short value = data1.Hue;         trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.TileData.ItemTable[i].Hue)
                    {   trig = true; break;  }
                if (!trig) textBoxHue1.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxStackOff1.Text) ) {
                short value = data1.StackingOffset;      trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.TileData.ItemTable[i].StackingOffset)
                    {   trig = true; break;  }
                if (!trig) textBoxStackOff1.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxValue1.Text) ) {
                short value = data1.Value;       trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.TileData.ItemTable[i].Value)
                    {   trig = true; break;  }
                if (!trig) textBoxValue1.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxHeigth1.Text) ) {
                short value = data1.Height;      trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.TileData.ItemTable[i].Height)
                    {   trig = true; break;  }
                if (!trig) textBoxHeigth1.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxUnk11.Text) ) {
                short value = data1.MiscData;    trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.TileData.ItemTable[i].MiscData)
                    {   trig = true; break;  }
                if (!trig) textBoxUnk11.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxUnk21.Text) ) {
                short value = data1.Unk2;        trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.TileData.ItemTable[i].Unk2)
                    {   trig = true; break;  }
                if (!trig) textBoxUnk21.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxUnk31.Text) ) {
                short value = data1.Unk3;        trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.TileData.ItemTable[i].Unk3)
                    {   trig = true; break;  }
                if (!trig) textBoxUnk31.Text = value.ToString();
            }


            if (String.IsNullOrEmpty(textBoxName2.Text) ) {
                string value = data2.Name;     trig = false;
                foreach (int i in listViewItemSelected)
                    if(String.Compare(value, Ultima.Secondary.TileData.ItemTable[i].Name, false) != 0)
                    {   trig = true; break;  }
                if (!trig) textBoxName2.Text = value;
            }
            if (String.IsNullOrEmpty(textBoxAnim2.Text) ) {
                short value = data2.Animation; trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.Secondary.TileData.ItemTable[i].Animation)
                    {   trig = true; break;  }
                if (!trig) textBoxAnim2.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxWeight2.Text) ) {
                short value = data2.Weight;    trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.Secondary.TileData.ItemTable[i].Weight)
                    {   trig = true; break;  }
                if (!trig) textBoxWeight2.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxQuality2.Text) ) {
                short value = data2.Quality;   trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.Secondary.TileData.ItemTable[i].Quality)
                    {   trig = true; break;  }
                if (!trig) textBoxQuality2.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxQuantity2.Text) ) {
                short value = data2.Quantity;  trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.Secondary.TileData.ItemTable[i].Quantity)
                    {   trig = true; break;  }
                if (!trig) textBoxQuantity2.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxHue2.Text) ) {
                short value = data2.Hue;       trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.Secondary.TileData.ItemTable[i].Hue)
                    {   trig = true; break;  }
                if (!trig) textBoxHue2.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxStackOff2.Text) ) {
                short value = data2.StackingOffset;    trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.Secondary.TileData.ItemTable[i].StackingOffset)
                    {   trig = true; break;  }
                if (!trig) textBoxStackOff2.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxValue2.Text) ) {
                short value = data2.Value;     trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.Secondary.TileData.ItemTable[i].Value)
                    {   trig = true; break;  }
                if (!trig) textBoxValue2.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxHeigth2.Text) ) {
                short value = data2.Height;    trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.Secondary.TileData.ItemTable[i].Height)
                    {   trig = true; break;  }
                if (!trig) textBoxHeigth2.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxUnk12.Text) ) {
                short value = data2.MiscData;  trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.Secondary.TileData.ItemTable[i].MiscData)
                    {   trig = true; break;  }
                if (!trig) textBoxUnk12.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxUnk22.Text) ) {
                short value = data2.Unk2;      trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.Secondary.TileData.ItemTable[i].Unk2)
                    {   trig = true; break;  }
                if (!trig) textBoxUnk22.Text = value.ToString();
            }
            if (String.IsNullOrEmpty(textBoxUnk32.Text) ) {
                short value = data2.Unk3;      trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.Secondary.TileData.ItemTable[i].Unk3)
                    {   trig = true; break;  }
                if (!trig) textBoxUnk32.Text = value.ToString();
            }

            // TileFlags
            for (int i = 1, lbid = 0; i < EnumValues.Length; ++i)
            {
                if (EnumNames[i].StartsWith("UnUsed"))
                    continue;

                TileFlag mask = (TileFlag)EnumValues.GetValue(i);

                CheckState state1 = checkedListBoxTileFlags1.GetItemCheckState(lbid);
                if(state1 == CheckState.Indeterminate)
                {
                    TileFlag value = data1.Flags & mask;      trig = false;
                    foreach (int k in listViewItemSelected)
                        if(value != (Ultima.TileData.ItemTable[k].Flags & mask))
                        {   trig = true; break;  }
                    if (!trig) checkedListBoxTileFlags1.SetItemChecked(lbid, value != TileFlag.None);
                }

                CheckState state2 = checkedListBoxTileFlags2.GetItemCheckState(lbid);
                if(state2 == CheckState.Indeterminate)
                {
                    TileFlag value = data2.Flags & mask;      trig = false;
                    foreach (int k in listViewItemSelected)
                        if(value != (Ultima.Secondary.TileData.ItemTable[k].Flags & mask))
                        {   trig = true; break;  }
                    if (!trig) checkedListBoxTileFlags2.SetItemChecked(lbid, value != TileFlag.None);
                }

                ++lbid;
            }

            // RadarCol
            if (pictureBoxItem1.Image != null)
            {
                short value = Ultima.RadarCol.GetItemColor(listViewItemSelected[0]);      trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.RadarCol.GetItemColor(i))
                    {   trig = true; break;  }
                if (!trig) 
                {
                    Color col = Ultima.Hues.HueToColor(value);
                    pictureBoxItemColor1.BackColor = col;
                    numericUpDownItemR1.Value = col.R;
                    numericUpDownItemG1.Value = col.G;
                    numericUpDownItemB1.Value = col.B;
                    pictureBoxItemColor1.Image = null;
                    pictureBoxItemColor1.Tag = value;
                }  
            }

            if (pictureBoxItem2.Image != null)
            {
                short value = Ultima.Secondary.RadarCol.GetItemColor(listViewItemSelected[0]);      trig = false;
                foreach (int i in listViewItemSelected)
                    if(value != Ultima.Secondary.RadarCol.GetItemColor(i))
                    {   trig = true; break;  }
                if (!trig) 
                {
                    Color col = Ultima.Hues.HueToColor(value);
                    pictureBoxItemColor2.BackColor = col;
                    numericUpDownItemR2.Value = col.R;
                    numericUpDownItemG2.Value = col.G;
                    numericUpDownItemB2.Value = col.B;
                    pictureBoxItemColor2.Image = null;
                    pictureBoxItemColor2.Tag = value;
                }  
            }
        }

        #endregion

        #region Синхронизация скролла для контролов ListView и отображение ToolTip

        private int mLastScrollPos1 = 0;
        private int mLastScrollPos2 = 0;

        private void ScrollListView(object sender, GuiControls.ListViewExArgs e)
        {
            if (sender == listViewItem1 && mLastScrollPos1 != e.VScrollPos)
            {
                mLastScrollPos1 = e.VScrollPos;
                listViewItem2.SetVScrollPos(e.VScrollPos);
            }
            if (sender == listViewItem2 && mLastScrollPos2 != e.VScrollPos)
            {
                mLastScrollPos2 = e.VScrollPos;
                listViewItem1.SetVScrollPos(e.VScrollPos);
            }
        }

        private void MouseClick(object sender, MouseEventArgs e)
        {
            if (mLastScrollPos1 == mLastScrollPos2)
                return;
            if (sender == listViewItem1)
            {
                int curscrollpos1 = listViewItem1.GetVScrollPos();
                mLastScrollPos2 = curscrollpos1;
                listViewItem2.SetVScrollPos(curscrollpos1);
            }
            if (sender == listViewItem2)
            {
                int curscrollpos2 = listViewItem1.GetVScrollPos();
                mLastScrollPos1 = curscrollpos2;
                listViewItem1.SetVScrollPos(curscrollpos2);
            }
        }

        private ListViewItem mLastHoverItem1 = null;
        private ListViewItem mLastHoverItem2 = null;

        private void MouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            if (sender == listViewItem1)
                mLastHoverItem1 = e.Item;
            if (sender == listViewItem2)
                mLastHoverItem2 = e.Item;
        }

        private readonly int mPreviewPanelHeightOffset;
        private readonly int mPreviewPanelWeightOffset;

        private void MouseMove(object sender, MouseEventArgs e)
        {
            ListView listview = (ListView)sender;
            ListViewItem item = listview == listViewItem1 ? mLastHoverItem1 : mLastHoverItem2;
            if (item == null)
                return;
                //listview.FindNearestItem(SearchDirectionHint.Down | SearchDirectionHint.Right, e.X, e.Y);
            int index = (int)item.Tag;
            if (index > 0)
            {
                if (floatingPreviewPanel.Tag == null || index != (int)floatingPreviewPanel.Tag)
                {
                    Bitmap image1 = Ultima.Art.GetStatic(index);
                    Bitmap image2 = Ultima.Secondary.Art.GetStatic(index);
                    
                    floatingPreviewPanel.Size = new Size( 2 * Math.Max((image1 != null ? image1.Width : 0), (image2 != null ? image2.Width : 0)) + mPreviewPanelWeightOffset,
                                                Math.Max((image1 != null ? image1.Height : 0), (image2 != null ? image2.Height : 0)) + mPreviewPanelHeightOffset);

                    //pictureBoxPreview1.BackColor = Color.Black;
                    pictureBoxPreview1.Image = image1;
                    pictureBoxPreview2.Image = image2;
                    floatingPreviewPanel.Tag = index;
                    labelPreviewInfo.Text = String.Format("0x{0:X4} ({0:D5})", index);
                    floatingPreviewPanel.Visible = true;
                }
                floatingPreviewPanel.Left = listview == listViewItem1 ? PointToClient(MousePosition).X : PointToClient(MousePosition).X - floatingPreviewPanel.Size.Width;
                floatingPreviewPanel.Top = PointToClient(MousePosition).Y - floatingPreviewPanel.Size.Height;
                floatingPreviewPanel.Invalidate();
            }
            else
            {
                floatingPreviewPanel.Visible = false;
            }
        }

        private void MouseLeave(object sender, EventArgs e)
        {
            floatingPreviewPanel.Visible = false;
        }

        #endregion

        #region Экспорт и импорт

        private enum MergeDirection {
            For_1,
            For_2,
            From_1to2,
            From_2to1
        }

        private void CopyItem(MergeDirection dir, int fromId, int toIndex)
        {
            switch (dir)
            {
                case MergeDirection.From_1to2 :
                    throw new Exception("Данная функция не реализованна, используйте обратное объединение.");
                    break;
                case MergeDirection.From_2to1 :
                    if (!Ultima.Secondary.Art.IsValidStatic(fromId)) {
                        Ultima.Art.RemoveStatic(toIndex);
                        Ultima.RadarCol.SetItemColor(toIndex, 0x0421);
                        Ultima.TileData.ItemTable[toIndex] = new ItemData();
                    } else {
                        Ultima.Art.ReplaceStatic(toIndex, Ultima.Secondary.Art.GetStatic(fromId));
                        Ultima.RadarCol.SetItemColor(toIndex, Ultima.Secondary.RadarCol.GetItemColor(fromId));
                        Ultima.TileData.ItemTable[toIndex] = Ultima.Secondary.TileData.ItemTable[fromId];
                    }
                    break;
            }
        }        

        private void SelectedItemsMove1to2ToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (int item in listViewItemSelected)
                CopyItem(MergeDirection.From_1to2, item, item);
        }

        private void SelectedItemsCopy1to2ToolStripTextBox_KeyDown(object sender, KeyEventArgs e)  {
        }

        private void SelectedItemsMove2to1ToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (int item in listViewItemSelected)
                CopyItem(MergeDirection.From_2to1, item, item);
            listViewItem1.Update();
        }

        private void SelectedItemsCopy2to1ToolStripTextBox_KeyDown(object sender, KeyEventArgs e) {
            switch(e.KeyCode)
            {
                case Keys.Enter : 
                    int newindex = -1;
                    if (!Int32.TryParse(copyItems2to1ToolStripTextBox.Text, out newindex) || newindex < 0 || newindex >= Ultima.Art.StaticLength)
                    {
                        MessageBox.Show("Не коретно введен id");
                        return;
                    }
                    listViewItemSelected.Sort();
                    for (int i = 0, index = listViewItemSelected[0]; i < listViewItemSelected.Count; index = listViewItemSelected[++i], ++newindex)
                        CopyItem(MergeDirection.From_2to1, index, newindex);
                    break;
                case Keys.Up    : break;
                case Keys.Down  : break;
                case Keys.Left  : break;
                case Keys.Right : break;
                case Keys.D0    : break;
                case Keys.D1    : break;
                case Keys.D2    : break;
                case Keys.D3    : break;
                case Keys.D4    : break;
                case Keys.D5    : break;
                case Keys.D6    : break;
                case Keys.D7    : break;
                case Keys.D8    : break;
                case Keys.D9    : break;
                //case Keys.A     : break;
                //case Keys.B     : break;
                //case Keys.C     : break;
                //case Keys.D     : break;
                //case Keys.E     : break;
                //case Keys.F     : break;
                //case Keys.X     : break;
                default :
                    if (!String.IsNullOrEmpty(copyItems2to1ToolStripTextBox.Text))
                    {
                        string text = copyItems2to1ToolStripTextBox.Text;
                        string numb = String.Empty;
                        bool hex = false;
                        for (int i = 0; i < text.Length; ++i)
                        {
                            if (Char.IsDigit(text, i)  )
                                numb += text[i];
                            else if(numb.Length == 1 && (text[i] == 'x' || text[i] == 'X')) {
                                hex = true;
                                numb += text[i];
                            }
                            else if (hex && (text[i] == 'a' || text[i] == 'A' || text[i] == 'b' || text[i] == 'B' || text[i] == 'c' || text[i] == 'C'
                                          || text[i] == 'd' || text[i] == 'D' || text[i] == 'e' || text[i] == 'E' || text[i] == 'f' || text[i] == 'F'))
                                numb += text[i];
                        }
                        copyItems2to1ToolStripTextBox.Text = text;
                    }
                    break;
            }
        }

        private void SaveItems(MergeDirection dir)
        {
            switch (dir)
            {
                case MergeDirection.For_2 :
                    throw new Exception("Данная функция не реализованна, используйте обратное объединение.");
                    break;
                case MergeDirection.For_1 :
                    string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                    Ultima.Art.Save(path);
                    Ultima.RadarCol.Save(Path.Combine(path, "radarcol.mul"));
                    Ultima.TileData.SaveTileData(Path.Combine(path, "tiledata.mul"));
                    MessageBox.Show(String.Format("Файлы: 'art.mul', 'artidx.mul', 'radarcol.mul', 'tiledata.mul' были сохранены в {0}", path), "Сохраннено", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    break;
            }
        }

        private void SaveItems1ToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveItems(MergeDirection.For_1);
        }

        private void SaveItems2ToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveItems(MergeDirection.For_2);
        }

        private string m_LastFolder;

        private void ItemsAsImage(MergeDirection dir, ImageFormat format, bool hexnames = true)
        {
            string ext = String.Empty;
            if (format == ImageFormat.Bmp) ext = "bmp";
            else if (format == ImageFormat.Png) ext = "png";
            else if (format == ImageFormat.Gif) ext = "gif";
            else if (format == ImageFormat.Tiff) ext = "tiff";
            else if (format == ImageFormat.Jpeg) ext = "jpg";
            else throw new ArgumentException();

            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Выберите папку";
                dialog.ShowNewFolderButton = true;
                dialog.SelectedPath = m_LastFolder;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    m_LastFolder = dialog.SelectedPath;
                    switch (dir)
                    {
                        case MergeDirection.For_1:
                            if (format == ImageFormat.Gif) {
                                foreach (int index in listViewItemSelected)
                                    if (Ultima.Art.IsValidStatic(index))
                                    {
                                        string FileName = Path.Combine(dialog.SelectedPath, String.Format(hexnames ? "I0x{0:X4}.{1}" : "I{0:D5}.{1}", index, ext));
                                        Bitmap bit = new Bitmap(Ultima.Art.GetStatic(index));
                                        Utils.SaveGIFWithNewColorTable(bit, FileName, 256, true);
                                    }
                            } else {
                                foreach (int index in listViewItemSelected)
                                    if (Ultima.Art.IsValidStatic(index))
                                    {
                                        string FileName = Path.Combine(dialog.SelectedPath, String.Format(hexnames ? "I0x{0:X4}.{1}" : "I{0:D5}.{1}", index, ext));
                                        Bitmap bit = new Bitmap(Ultima.Art.GetStatic(index));
                                        bit.Save(FileName, format);
                                        bit.Dispose();
                                    }
                            }
                            break;
                        case MergeDirection.For_2:
                            if (format == ImageFormat.Gif) {
                                foreach (int index in listViewItemSelected)
                                    if (Ultima.Secondary.Art.IsValidStatic(index))
                                    {
                                        string FileName = Path.Combine(dialog.SelectedPath, String.Format(hexnames ? "I0x{0:X4}.{1}" : "I{0:D5}.{1}", index, ext));
                                        Bitmap bit = new Bitmap(Ultima.Secondary.Art.GetStatic(index));
                                        Utils.SaveGIFWithNewColorTable(bit, FileName, 256, true);
                                    }
                            } else {
                                foreach (int index in listViewItemSelected)
                                    if (Ultima.Secondary.Art.IsValidStatic(index))
                                    {
                                        string FileName = Path.Combine(dialog.SelectedPath, String.Format(hexnames ? "I0x{0:X4}.{1}" : "I{0:D5}.{1}", index, ext));
                                        Bitmap bit = new Bitmap(Ultima.Secondary.Art.GetStatic(index));
                                        bit.Save(FileName, format);
                                    
                                        bit.Dispose();
                                    }
                            }
                            break;
                        default:
                            throw new ArgumentException("Недопустимое значение MergeDirection");
                    }
                    MessageBox.Show(String.Format("Все предметы были сохранены в {0}", dialog.SelectedPath), "Сохраненно", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void SelectedItemsAsBmp1ToolStripMenuItem_Click(object sender, EventArgs e) {
            ItemsAsImage(MergeDirection.For_1, ImageFormat.Bmp);
        }

        private void SelectedItemsAsPng1ToolStripMenuItem_Click(object sender, EventArgs e) {
            ItemsAsImage(MergeDirection.For_1, ImageFormat.Png);
        }

        private void SelectedItemsAsTif1ToolStripMenuItem_Click(object sender, EventArgs e) {
            ItemsAsImage(MergeDirection.For_1, ImageFormat.Tiff);
        }

        private void SelectedItemsAsGif1ToolStripMenuItem_Click(object sender, EventArgs e) {
            ItemsAsImage(MergeDirection.For_1, ImageFormat.Gif);
        }

        private void SelectedItemsAsJpg1ToolStripMenuItem_Click(object sender, EventArgs e) {
            ItemsAsImage(MergeDirection.For_1, ImageFormat.Jpeg);
        } 

        private void SelectedItemsAsBmp2ToolStripMenuItem_Click(object sender, EventArgs e) {
            ItemsAsImage(MergeDirection.For_2, ImageFormat.Bmp);
        }

        private void SelectedItemsAsPng2ToolStripMenuItem_Click(object sender, EventArgs e) {
            ItemsAsImage(MergeDirection.For_2, ImageFormat.Png);
        }

        private void SelectedItemsAsTif2ToolStripMenuItem_Click(object sender, EventArgs e) {
            ItemsAsImage(MergeDirection.For_2, ImageFormat.Tiff);
        }

        private void SelectedItemsAsGif2ToolStripMenuItem_Click(object sender, EventArgs e) {
            ItemsAsImage(MergeDirection.For_2, ImageFormat.Gif);
        }

        private void SelectedItemsAsJpg2ToolStripMenuItem_Click(object sender, EventArgs e) {
            ItemsAsImage(MergeDirection.For_2, ImageFormat.Jpeg);
        }

        #endregion

        




        /*
        Dictionary<int, bool> m_Compare = new Dictionary<int, bool>();
        SHA256Managed shaM = new SHA256Managed();
        System.Drawing.ImageConverter ic = new System.Drawing.ImageConverter();

        private void OnLoad(object sender, EventArgs e)
        {
            listBoxOrg.Items.Clear();
            listBoxOrg.BeginUpdate();
            List<object> cache = new List<object>();
            int staticlength = 0x4000;
            if (Art.IsUOSA())
                staticlength = 0x8000;
            for (int i = 0; i < staticlength; i++)
            {
                cache.Add(i);
            }
            listBoxOrg.Items.AddRange(cache.ToArray());
            listBoxOrg.EndUpdate();
        }

        private void OnIndexChangedOrg(object sender, EventArgs e)
        {
            if ((listBoxOrg.SelectedIndex == -1) || (listBoxOrg.Items.Count < 1))
                return;

            int i = int.Parse(listBoxOrg.Items[listBoxOrg.SelectedIndex].ToString());
            if (listBoxSec.Items.Count > 0)
            {
                int pos = listBoxSec.Items.IndexOf(i);
                if (pos >= 0)
                    listBoxSec.SelectedIndex = pos;
            }
            if (Art.IsValidStatic(i))
            {
                Bitmap bmp = Art.GetStatic(i);
                if (bmp != null)
                    pictureBoxOrg.BackgroundImage = bmp;
                else
                    pictureBoxOrg.BackgroundImage = null;
            }
            else
                pictureBoxOrg.BackgroundImage = null;
            listBoxOrg.Invalidate();
        }

        private void DrawitemOrg(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            Brush fontBrush = Brushes.Gray;

            int i = int.Parse(listBoxOrg.Items[e.Index].ToString());
            if (listBoxOrg.SelectedIndex == e.Index)
                e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            if (!Art.IsValidStatic(i))
                fontBrush = Brushes.Red;
            else if (listBoxSec.Items.Count > 0)
            {
                if (!Compare(i))
                    fontBrush = Brushes.Blue;
            }

            e.Graphics.DrawString(String.Format("0x{0:X}", i), Font, fontBrush,
                new PointF((float)5,
                e.Bounds.Y + ((e.Bounds.Height / 2) -
                (e.Graphics.MeasureString(String.Format("0x{0:X}", i), Font).Height / 2))));
        }

        private void MeasureOrg(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 13;
        }

        private void LoadSecond()
        {
            m_Compare.Clear();
            listBoxSec.BeginUpdate();
            listBoxSec.Items.Clear();
            List<object> cache = new List<object>();
            int staticlength = 0x4000;
            if (SecondArt.IsUOSA())
                staticlength = 0x8000;
            for (int i = 0; i < staticlength; i++)
            {
                cache.Add(i);
            }
            listBoxSec.Items.AddRange(cache.ToArray());
            listBoxSec.EndUpdate();
        }

        private void DrawItemSec(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            Brush fontBrush = Brushes.Gray;

            int i = int.Parse(listBoxSec.Items[e.Index].ToString());
            if (listBoxSec.SelectedIndex == e.Index)
                e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            if (!SecondArt.IsValidStatic(i))
                fontBrush = Brushes.Red;
            else if (!Compare(i))
                fontBrush = Brushes.Blue;

            e.Graphics.DrawString(String.Format("0x{0:X}", i), Font, fontBrush,
                new PointF((float)5,
                e.Bounds.Y + ((e.Bounds.Height / 2) -
                (e.Graphics.MeasureString(String.Format("0x{0:X}", i), Font).Height / 2))));
        }

        private void MeasureSec(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 13;
        }

        private void OnIndexChangedSec(object sender, EventArgs e)
        {
            if ((listBoxSec.SelectedIndex == -1) || (listBoxSec.Items.Count < 1))
                return;

            int i = int.Parse(listBoxSec.Items[listBoxSec.SelectedIndex].ToString());
            int pos = listBoxOrg.Items.IndexOf(i);
            if (pos >= 0)
                listBoxOrg.SelectedIndex = pos;
            if (SecondArt.IsValidStatic(i))
            {
                Bitmap bmp = SecondArt.GetStatic(i);
                if (bmp != null)
                    pictureBoxSec.BackgroundImage = bmp;
                else
                    pictureBoxSec.BackgroundImage = null;
            }
            else
                pictureBoxSec.BackgroundImage = null;
            listBoxSec.Invalidate();
        }

        private bool Compare(int index)
        {
            if (m_Compare.ContainsKey(index))
                return m_Compare[index];
            Bitmap bitorg = Art.GetStatic(index);
            Bitmap bitsec = SecondArt.GetStatic(index);
            if ((bitorg == null) && (bitsec == null))
            {
                m_Compare[index] = true;
                return true;
            }
            if (((bitorg == null) || (bitsec == null))
                || (bitorg.Size != bitsec.Size))
            {
                m_Compare[index] = false;
                return false;
            }

            byte[] btImage1 = new byte[1];
            btImage1 = (byte[])ic.ConvertTo(bitorg, btImage1.GetType());
            byte[] btImage2 = new byte[1];
            btImage2 = (byte[])ic.ConvertTo(bitsec, btImage2.GetType());

            string hash1string = BitConverter.ToString(shaM.ComputeHash(btImage1));
            string hash2string = BitConverter.ToString(shaM.ComputeHash(btImage2));
            bool res;
            if (hash1string != hash2string)
                res = false;
            else
                res = true;

            m_Compare[index] = res;
            return res;
        }

        private void OnChangeShowDiff(object sender, EventArgs e)
        {
            if (m_Compare.Count < 1)
            {
                if (checkBox1.Checked)
                {
                    MessageBox.Show("Second Item file is not loaded!");
                    checkBox1.Checked = false;
                }
                return;
            }

            listBoxOrg.BeginUpdate();
            listBoxSec.BeginUpdate();
            listBoxOrg.Items.Clear();
            listBoxSec.Items.Clear();
            List<object> cache = new List<object>();
            int staticlength = 0x4000;
            if (Art.IsUOSA() || SecondArt.IsUOSA())
                staticlength = 0x8000;
            if (checkBox1.Checked)
            {
                for (int i = 0; i < staticlength; i++)
                {
                    if (!Compare(i))
                        cache.Add(i);
                }
            }
            else
            {
                for (int i = 0; i < staticlength; i++)
                {
                    cache.Add(i);
                }
            }
            listBoxOrg.Items.AddRange(cache.ToArray());
            listBoxSec.Items.AddRange(cache.ToArray());
            listBoxOrg.EndUpdate();
            listBoxSec.EndUpdate();
        }

        private void ExportAsBmp(object sender, EventArgs e)
        {
            if (listBoxSec.SelectedIndex == -1)
                return;
            int i = int.Parse(listBoxSec.Items[listBoxSec.SelectedIndex].ToString());
            if (!SecondArt.IsValidStatic(i))
                return;
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, String.Format("Item(Sec) 0x{0:X}.bmp", i));
            SecondArt.GetStatic(i).Save(FileName, ImageFormat.Bmp);
            MessageBox.Show(
                String.Format("Item saved to {0}", FileName),
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        private void ExportAsTiff(object sender, EventArgs e)
        {
            if (listBoxSec.SelectedIndex == -1)
                return;
            int i = int.Parse(listBoxSec.Items[listBoxSec.SelectedIndex].ToString());
            if (!SecondArt.IsValidStatic(i))
                return;
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, String.Format("Item(Sec) 0x{0:X}.tiff", i));
            SecondArt.GetStatic(i).Save(FileName, ImageFormat.Tiff);
            MessageBox.Show(
                String.Format("Item saved to {0}", FileName),
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        private void OnClickCopy(object sender, EventArgs e)
        {
            if (listBoxSec.SelectedIndex == -1)
                return;
            int i = int.Parse(listBoxSec.Items[listBoxSec.SelectedIndex].ToString());
            if (!SecondArt.IsValidStatic(i))
                return;
            int staticlength = 0x4000;
            if (Art.IsUOSA())
                staticlength = 0x8000;
            if (i >= staticlength)
                return;
            Bitmap copy = new Bitmap(SecondArt.GetStatic(i));
            Ultima.Art.ReplaceStatic(i, copy);
            FiddlerControls.Options.ChangedUltimaClass["Art"] = true;
            FiddlerControls.Events.FireItemChangeEvent(this, i);
            m_Compare[i] = true;
            listBoxOrg.BeginUpdate();
            bool done = false;

            for (int id = 0; id < staticlength; id++)
            {
                if (id > i)
                {
                    listBoxOrg.Items.Insert(id, i);
                    done = true;
                    break;
                }
                if (id == i)
                {
                    done = true;
                    break;
                }
            }
            if (!done)
                listBoxOrg.Items.Add(i);
            listBoxOrg.EndUpdate();
            listBoxOrg.Invalidate();
            listBoxSec.Invalidate();
            OnIndexChangedOrg(this, null);
        }

        private void textBoxQuality_TextChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void OnClickLoadSecond(object sender, EventArgs e)
        {

        }
        */

    }
}
