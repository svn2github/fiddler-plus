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
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Ultima;

namespace FiddlerControls
{
    public partial class TileDatas : UserControl
    {
        

        public TileDatas()
        {
            ChangesInTiledataOrRadarCol = new bool[ItemMaxIndex + LandMaxIndex];
            Array.Clear(ChangesInTiledataOrRadarCol, 0, ChangesInTiledataOrRadarCol.Length);

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            InitializeComponent();
            checkedListBox1.BeginUpdate();
            checkedListBox1.Items.Clear();
            EnumNames = System.Enum.GetNames(typeof(TileFlag));
            for (int i = 1; i < EnumNames.Length; ++i)
            {
                if (EnumNames[i].StartsWith("UnUsed"))
                    continue;
                checkedListBox1.Items.Add(EnumNames[i], false);
            }
            checkedListBox1.EndUpdate();
            checkedListBox2.BeginUpdate();
            checkedListBox2.Items.Clear();
            checkedListBox2.Items.Add(System.Enum.GetName(typeof(TileFlag), TileFlag.Damaging), false);
            checkedListBox2.Items.Add(System.Enum.GetName(typeof(TileFlag), TileFlag.Wet), false);
            checkedListBox2.Items.Add(System.Enum.GetName(typeof(TileFlag), TileFlag.Impassable), false);
            checkedListBox2.Items.Add(System.Enum.GetName(typeof(TileFlag), TileFlag.Wall), false);
            checkedListBox2.Items.Add(System.Enum.GetName(typeof(TileFlag), TileFlag.Unknown3), false);
            checkedListBox2.EndUpdate();
            refMarker = this;

            ReloadCliloc();
        }

        private static string[] EnumNames = null;

        private static TileDatas refMarker = null;
        private bool ChangingIndex = false;


        public static void Select(int graphic, bool land)
        {
            SearchGraphic(graphic, land);
        }
        public static bool SearchGraphic(int graphic, bool land)
        {
            int index = 0;
            if (land)
            {
                for (int i = index; i < refMarker.treeViewLand.Nodes.Count; ++i)
                {
                    TreeNode node = refMarker.treeViewLand.Nodes[i];
                    if ((int)node.Tag == graphic)
                    {
                        refMarker.tabcontrol.SelectTab(1);
                        refMarker.treeViewLand.SelectedNode = node;
                        node.EnsureVisible();
                        return true;
                    }
                }
            }
            else
            {
                for (int i = index; i < refMarker.treeViewItem.Nodes.Count; ++i)
                {
                    TreeNode node = refMarker.treeViewItem.Nodes[i];
                    if ((int)node.Tag == graphic)
                    {
                        refMarker.tabcontrol.SelectTab(0);
                        refMarker.treeViewItem.SelectedNode = node;
                        node.EnsureVisible();
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool SearchName(string name, bool next, bool land)
        {
            int index = 0;
            Regex regex = new Regex(@name, RegexOptions.IgnoreCase);
            if (land)
            {
                if (next)
                {
                    if (refMarker.treeViewLand.SelectedNode.Index >= 0)
                        index = refMarker.treeViewLand.SelectedNode.Index + 1;
                    if (index >= refMarker.treeViewLand.Nodes.Count)
                        index = 0;
                }
                for (int i = index; i < refMarker.treeViewLand.Nodes.Count; ++i)
                {
                    TreeNode node = refMarker.treeViewLand.Nodes[i];
                    if (regex.IsMatch(TileData.LandTable[(int)node.Tag].Name))
                    {
                        refMarker.tabcontrol.SelectTab(1);
                        refMarker.treeViewLand.SelectedNode = node;
                        node.EnsureVisible();
                        return true;
                    }
                }
            }
            else
            {
                if (next)
                {
                    if (refMarker.treeViewItem.SelectedNode.Index >= 0)
                        index = refMarker.treeViewItem.SelectedNode.Index + 1;
                    if (index >= refMarker.treeViewItem.Nodes.Count)
                        index = 0;
                }
                for (int i = index; i < refMarker.treeViewItem.Nodes.Count; ++i)
                {
                    TreeNode node = refMarker.treeViewItem.Nodes[i];
                    if (regex.IsMatch(TileData.ItemTable[(int)node.Tag].Name))
                    {
                        refMarker.tabcontrol.SelectTab(0);
                        refMarker.treeViewItem.SelectedNode = node;
                        node.EnsureVisible();
                        return true;
                    }
                }
            }
            return false;
        }

        public static void ApplyFilterItem(ItemData item)
        {
            refMarker.treeViewItem.BeginUpdate();
            refMarker.treeViewItem.Nodes.Clear();
            List<TreeNode> nodes = new List<TreeNode>();
            for (int i = 0; i < TileData.ItemTable.Length; ++i)
            {
                if (!String.IsNullOrEmpty(item.Name))
                {
                    if (!TileData.ItemTable[i].Name.ToLower().Contains(item.Name.ToLower()))
                        continue;
                }
                if (item.Animation != 0)
                {
                    if (TileData.ItemTable[i].Animation != item.Animation)
                        continue;
                }
                if (item.Weight != 0)
                {
                    if (TileData.ItemTable[i].Weight != item.Weight)
                        continue;
                }
                if (item.Quality != 0)
                {
                    if (TileData.ItemTable[i].Quality != item.Quality)
                        continue;
                }
                if (item.Quantity != 0)
                {
                    if (TileData.ItemTable[i].Quantity != item.Quantity)
                        continue;
                }
                if (item.Hue != 0)
                {
                    if (TileData.ItemTable[i].Hue != item.Hue)
                        continue;
                }
                if (item.StackingOffset != 0)
                {
                    if (TileData.ItemTable[i].StackingOffset != item.StackingOffset)
                        continue;
                }
                if (item.Value != 0)
                {
                    if (TileData.ItemTable[i].Value != item.Value)
                        continue;
                }
                if (item.Height != 0)
                {
                    if (TileData.ItemTable[i].Height != item.Height)
                        continue;
                }
                if (item.MiscData != 0)
                {
                    if (TileData.ItemTable[i].MiscData != item.MiscData)
                        continue;
                }
                if (item.Unk2 != 0)
                {
                    if (TileData.ItemTable[i].Unk2 != item.Unk2)
                        continue;
                }
                if (item.Unk3 != 0)
                {
                    if (TileData.ItemTable[i].Unk3 != item.Unk3)
                        continue;
                }
                if (item.Flags != 0)
                {
                    if ((TileData.ItemTable[i].Flags & item.Flags) == 0)
                        continue;
                }
                TreeNode node = new TreeNode(String.Format("0x{0:X4} ({0}) {1}", i, TileData.ItemTable[i].Name));
                node.Tag = i;
                nodes.Add(node);
            }
            refMarker.treeViewItem.Nodes.AddRange(nodes.ToArray());
            refMarker.treeViewItem.EndUpdate();
            if (refMarker.treeViewItem.Nodes.Count > 0)
                refMarker.treeViewItem.SelectedNode = refMarker.treeViewItem.Nodes[0];
        }

        public static void ApplyFilterLand(LandData land)
        {
            refMarker.treeViewLand.BeginUpdate();
            refMarker.treeViewLand.Nodes.Clear();
            List<TreeNode> nodes = new List<TreeNode>();
            for (int i = 0; i < TileData.LandTable.Length; ++i)
            {
                if (!String.IsNullOrEmpty(land.Name))
                {
                    if (!TileData.ItemTable[i].Name.ToLower().Contains(land.Name.ToLower()))
                        continue;
                }
                if (land.TextureID != 0)
                {
                    if (TileData.LandTable[i].TextureID != land.TextureID)
                        continue;
                }
                if (land.Flags != 0)
                {
                    if ((TileData.LandTable[i].Flags & land.Flags) == 0)
                        continue;
                }
                TreeNode node = new TreeNode(String.Format("0x{0:X4} ({0}) {1}", i, TileData.LandTable[i].Name));
                node.Tag = i;
                nodes.Add(node);
            }
            refMarker.treeViewLand.Nodes.AddRange(nodes.ToArray());
            refMarker.treeViewLand.EndUpdate();
            if (refMarker.treeViewLand.Nodes.Count > 0)
                refMarker.treeViewLand.SelectedNode = refMarker.treeViewLand.Nodes[0];
        }

        private bool Loaded = false;
        private void Reload()
        {
            if (Loaded)
                OnLoad(this, EventArgs.Empty);
        }
        private void OnLoad(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
        
            tabcontrol.SelectedTab = null;
            //tabPageEmpty.Parent = null;

            Loaded = true;
            Cursor.Current = Cursors.Default;
        }


        private void onChangedSelectedTab(object sender, EventArgs e)
        {
            TabPage tab = tabcontrol.SelectedTab;

            if (tab == tabPageAnim)
                OnEnterAnimTab();
            else
                OnLeaveAnimTab();

            if (tab == tabPageLand)
                OnEnterLandTab();
            else
                OnLeaveLandTab();

            if (tab == tabPageItems)
                OnEnterItemTab();
            else
                OnLeaveItemTab();

            return;
        }

        private void OnFilePathChangeEvent()
        {
            Reload();
        }

        private void OnTileDataChangeEvent(object sender, int index)
        {
            if (!Loaded)
                return;
            if (sender.Equals(this))
                return;
            if (index > 0x3FFF) //items
            {
                if (treeViewItem.SelectedNode == null)
                    return;
                if ((int)treeViewItem.SelectedNode.Tag == index)
                {
                    treeViewItem.SelectedNode.ForeColor = Color.Red;
                    AfterSelectTreeViewItem(this, new TreeViewEventArgs(treeViewItem.SelectedNode));
                }
                else
                {
                    foreach (TreeNode node in treeViewItem.Nodes)
                    {
                        if ((int)node.Tag == index)
                        {
                            node.ForeColor = Color.Red;
                            break;
                        }
                    }
                }
            }
            else
            {
                if (treeViewLand.SelectedNode == null)
                    return;
                if ((int)treeViewLand.SelectedNode.Tag == index)
                {
                    treeViewLand.SelectedNode.ForeColor = Color.Red;
                    AfterSelectTreeViewLand(this, new TreeViewEventArgs(treeViewLand.SelectedNode));
                }
                else
                {
                    foreach (TreeNode node in treeViewLand.Nodes)
                    {
                        if ((int)node.Tag == index)
                        {
                            node.ForeColor = Color.Red;
                            break;
                        }
                    }
                }
            }
        }

        

        private void AfterSelectTreeViewLand(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null)
                return;
            int index = (int)e.Node.Tag;
            try
            {
                Bitmap bit = Ultima.Art.GetLand(index);
                Bitmap newbit = new Bitmap(pictureBoxLand.Size.Width, pictureBoxLand.Size.Height);
                Graphics newgraph = Graphics.FromImage(newbit);
                newgraph.Clear(Color.FromArgb(-1));
                newgraph.DrawImage(bit, (pictureBoxLand.Size.Width - bit.Width) / 2, (pictureBoxLand.Size.Height - bit.Height) / 2);
                pictureBoxLand.Image = newbit;
            }
            catch
            {
                pictureBoxLand.Image = new Bitmap(pictureBoxLand.Width, pictureBoxLand.Height);
            }
            LandData data = TileData.LandTable[index];
            ChangingIndex = true;
            textBoxNameLand.Text = data.Name;
            textBoxTexID.Text = data.TextureID.ToString();
            if ((data.Flags & TileFlag.Damaging) != 0)
                checkedListBox2.SetItemChecked(0, true);
            else
                checkedListBox2.SetItemChecked(0, false);
            if ((data.Flags & TileFlag.Wet) != 0)
                checkedListBox2.SetItemChecked(1, true);
            else
                checkedListBox2.SetItemChecked(1, false);
            if ((data.Flags & TileFlag.Impassable) != 0)
                checkedListBox2.SetItemChecked(2, true);
            else
                checkedListBox2.SetItemChecked(2, false);
            if ((data.Flags & TileFlag.Wall) != 0)
                checkedListBox2.SetItemChecked(3, true);
            else
                checkedListBox2.SetItemChecked(3, false);
            if ((data.Flags & TileFlag.Unknown3) != 0)
                checkedListBox2.SetItemChecked(4, true);
            else
                checkedListBox2.SetItemChecked(4, false);
            ChangingIndex = false;
        }

        private void OnClickEditTiledata(object sender, EventArgs e)
        {
            textBoxName.Enabled = textBoxAnim.Enabled = textBoxWeight.Enabled = textBoxQuality.Enabled = textBoxQuantity.Enabled
                                = textBoxHue.Enabled = textBoxStackOff.Enabled = textBoxValue.Enabled = textBoxHeigth.Enabled
                                = textBoxUnk1.Enabled = textBoxUnk2.Enabled = textBoxUnk3.Enabled = checkedListBox1.Enabled
                                = textBoxNameLand.Enabled = textBoxTexID.Enabled = checkedListBox2.Enabled
                                = EditTileDataToolStripMenuItem.Checked;
        }

        

        private void SaveChangesInTiledataItems(int index = -1)
        {
            /*
            if(index < 0)
            {
                if (tabcontrol.SelectedIndex != 0) //items
                    return;
                else if (listViewItem.SelectedItems != null)
                    for (int i = 0; i < listViewItem.SelectedItems.Count; ++i)
                        SaveChangesInTiledataItems((int)listViewItem.SelectedItems[0].Tag);
                else if (treeViewItem.SelectedNode != null)
                    SaveChangesInTiledataItems((int)treeViewItem.SelectedNode.Tag);
                return;
            }
            */
            bool changed = false;

            ItemData item = TileData.ItemTable[index];
            string name = textBoxName.Text;
            if (name.Length > 20)
                name = name.Substring(0, 20);
            if (!String.IsNullOrEmpty(name) && String.Compare(item.Name, name, false) != 0)
            {
                item.Name = name;
                changed = true;
            }

            //treeViewItem.SelectedNode.Text = String.Format("0x{0:X4} ({0}) {1}", index, name);
            
            byte byteres;
            short shortres;
            if (short.TryParse(textBoxAnim.Text, out shortres) && item.Animation != shortres)
            {
                item.Animation = shortres;
                changed = true;
            }
            if (byte.TryParse(textBoxWeight.Text, out byteres) && item.Weight != byteres)
            {
                item.Weight = byteres;
                changed = true;
            }
            if (byte.TryParse(textBoxQuality.Text, out byteres) && item.Quality != byteres)
            {
                item.Quality = byteres;
                changed = true;
            }
            if (byte.TryParse(textBoxQuantity.Text, out byteres) && item.Quantity != byteres)
            {
                item.Quantity = byteres;
                changed = true;
            }
            if (byte.TryParse(textBoxHue.Text, out byteres) && item.Hue != byteres)
            {
                item.Hue = byteres;
                changed = true;
            }
            if (byte.TryParse(textBoxStackOff.Text, out byteres) && item.StackingOffset != byteres)
            {
                item.StackingOffset = byteres;
                changed = true;
            }
            if (byte.TryParse(textBoxValue.Text, out byteres) && item.Value != byteres)
            {
                item.Value = byteres;
                changed = true;
            }
            if (byte.TryParse(textBoxHeigth.Text, out byteres) && item.Height != byteres)
            {
                item.Height = byteres;
                changed = true;
            }
            if (short.TryParse(textBoxUnk1.Text, out shortres) && item.MiscData != shortres)
            {
                item.MiscData = shortres;
                changed = true;
            }
            if (byte.TryParse(textBoxUnk2.Text, out byteres) && item.Unk2 != byteres)
            {
                item.Unk2 = byteres;
                changed = true;
            }
            if (byte.TryParse(textBoxUnk3.Text, out byteres) && item.Unk3 != byteres)
            {
                item.Unk3 = byteres;
                changed = true;
            }

            int lbid = -1;
            TileFlag flags = item.Flags;
            Array EnumValues = System.Enum.GetValues(typeof(TileFlag));
            for (int e = 1; e < EnumValues.Length; ++e)
            {
                if (EnumNames[e].StartsWith("UnUsed"))
                    continue;
                CheckState state = checkedListBox1.GetItemCheckState(++lbid);
                if (state == CheckState.Checked)
                    flags |= (TileFlag)EnumValues.GetValue(e);
                else if (state == CheckState.Unchecked)
                    flags &= ~(TileFlag)EnumValues.GetValue(e);
            }
            if (item.Flags != flags)
            {
                item.Flags = flags;
                changed = true;
            }
            TileData.ItemTable[index] = item;

            //listViewItem.SelectedItems[0].ForeColor = Color.BlueViolet; 
            //treeViewItem.SelectedNode.ForeColor = Color.Red;

            if (changed)
            {
                ChangesInTiledataOrRadarCol[LandMaxIndex + index] = true;

                Options.ChangedUltimaClass["TileData"] = true;
                FiddlerControls.Events.FireTileDataChangeEvent(this, index + 0x4000);
            }
        }

        private void SaveChangesInTiledataLands(int index = -1)
        {
            bool changed = false;

            LandData land = TileData.LandTable[index];
            string name = textBoxNameLand.Text;
            if (name.Length > 20)
                name = name.Substring(0, 20);
            if (!String.IsNullOrEmpty(name) && String.Compare(land.Name, name, false) != 0)
            {
                land.Name = name;
                changed = true;
            }

            short shortres;
            if (short.TryParse(textBoxTexID.Text, out shortres) && land.TextureID != shortres)
            {
                land.TextureID = shortres;
                changed = true;
            }

            TileFlag flags = land.Flags;
            CheckState state;

            state = checkedListBox2.GetItemCheckState(0);
            if (state == CheckState.Checked)
                flags |= TileFlag.Damaging;
            else if (state == CheckState.Unchecked)
                flags &= ~TileFlag.Damaging;
            state = checkedListBox2.GetItemCheckState(1);
            if (state == CheckState.Checked)
                flags |= TileFlag.Wet;
            else if (state == CheckState.Unchecked)
                flags &= ~TileFlag.Wet;
            state = checkedListBox2.GetItemCheckState(2);
            if (state == CheckState.Checked)
                flags |= TileFlag.Impassable;
            else if (state == CheckState.Unchecked)
                flags &= ~TileFlag.Impassable;
            state = checkedListBox2.GetItemCheckState(3);
            if (state == CheckState.Checked)
                flags |= TileFlag.Wall;
            else if (state == CheckState.Unchecked)
                flags &= ~TileFlag.Wall;
            state = checkedListBox2.GetItemCheckState(4);
            if (state == CheckState.Checked)
                flags |= TileFlag.Unknown3;
            else if (state == CheckState.Unchecked)
                flags &= ~TileFlag.Unknown3;

            if (land.Flags != flags)
            {
                land.Flags = flags;
                changed = true;
            }
            TileData.LandTable[index] = land;

            //listViewItem.SelectedItems[0].ForeColor = Color.BlueViolet; 
            //treeViewItem.SelectedNode.ForeColor = Color.Red;

            if (changed)
            {
                ChangesInTiledataOrRadarCol[index] = true;

                Options.ChangedUltimaClass["TileData"] = true;
                FiddlerControls.Events.FireTileDataChangeEvent(this, index);
            }
        }


        private void OnClickSaveTiledata(object sender, EventArgs e)
        {
            if (EditTileDataToolStripMenuItem.Checked)
                foreach (int land in listViewLandSelected)
                    SaveChangesInTiledataLands(land);   // Сохранение тайлдаты для выделенного элемента

            if (EditTileDataToolStripMenuItem.Checked)
                foreach (int item in listViewItemSelected)
                    SaveChangesInTiledataItems(item);   // Сохранение тайлдаты для выделенного элемента

            string path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Mul Files");
            string FileName = Path.Combine(path, "tiledata.mul");
            Ultima.TileData.SaveTileData(FileName);
            MessageBox.Show(
                String.Format("TileData сохраненна в {0}", FileName),
                "Сохранено", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
            Options.ChangedUltimaClass["TileData"] = false;
        }

        private void OnClickSaveChanges(object sender, EventArgs e)
        {
            /*
            UOUpdater.DllExport patch = new UOUpdater.DllExport(
                                    @"C:\UltimaOnline\source\_build\uoFiddler\patch\new", 
                                    @"C:\UltimaOnline\source\_build\uoFiddler\patch\old", 
                                    @"C:\UltimaOnline\source\_build\uoFiddler\patch\out");
            List<UOUpdater.SectionType> sections = new List<UOUpdater.SectionType>(2);
            sections.Add(UOUpdater.SectionType.Map0);
            sections.Add(UOUpdater.SectionType.Statics0);
            //string file = patch.MakePatch(sections);
            string file = patch.ApplyPatch();
            System.Windows.Forms.MessageBox.Show(file);
            */

            if (tabcontrol.SelectedIndex == 0) //items
            {
                if (treeViewItem.SelectedNode == null)
                    return;
                int index = (int)treeViewItem.SelectedNode.Tag;
                ItemData item = TileData.ItemTable[index];
                string name = textBoxName.Text;
                if (name.Length > 20)
                    name = name.Substring(0, 20);
                item.Name = name;
                treeViewItem.SelectedNode.Text = String.Format("0x{0:X4} ({0}) {1}", index, name);
                byte byteres;
                short shortres;
                if (short.TryParse(textBoxAnim.Text, out shortres))
                    item.Animation = shortres;
                if (byte.TryParse(textBoxWeight.Text, out byteres))
                    item.Weight = byteres;
                if (byte.TryParse(textBoxQuality.Text, out byteres))
                    item.Quality = byteres;
                if (byte.TryParse(textBoxQuantity.Text, out byteres))
                    item.Quantity = byteres;
                if (byte.TryParse(textBoxHue.Text, out byteres))
                    item.Hue = byteres;
                if (byte.TryParse(textBoxStackOff.Text, out byteres))
                    item.StackingOffset = byteres;
                if (byte.TryParse(textBoxValue.Text, out byteres))
                    item.Value = byteres;
                if (byte.TryParse(textBoxHeigth.Text, out byteres))
                    item.Height = byteres;
                if (short.TryParse(textBoxUnk1.Text, out shortres))
                    item.MiscData = shortres;
                if (byte.TryParse(textBoxUnk2.Text, out byteres))
                    item.Unk2 = byteres;
                if (byte.TryParse(textBoxUnk3.Text, out byteres))
                    item.Unk3 = byteres;
                int lbid = -1;
                item.Flags = TileFlag.None;
                Array EnumValues = System.Enum.GetValues(typeof(TileFlag));
                for (int i = 1; i < EnumValues.Length; ++i)
                {
                    if (EnumNames[i].StartsWith("UnUsed"))
                        continue;
                    if (checkedListBox1.GetItemChecked(++lbid))
                        item.Flags |= (TileFlag)EnumValues.GetValue(i);
                }
                TileData.ItemTable[index] = item;
                treeViewItem.SelectedNode.ForeColor = Color.Red;
                Options.ChangedUltimaClass["TileData"] = true;
                FiddlerControls.Events.FireTileDataChangeEvent(this, index + 0x4000);
                if (memorySaveWarningToolStripMenuItem.Checked)
                    MessageBox.Show(
                        String.Format("Edits of 0x{0:X4} ({0}) saved to memory. Click 'Save Tiledata' to write to file.", index), "Saved",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
            }
            else //land
            {
                if (treeViewLand.SelectedNode == null)
                    return;
                int index = (int)treeViewLand.SelectedNode.Tag;
                LandData land = TileData.LandTable[index];
                string name = textBoxNameLand.Text;
                if (name.Length > 20)
                    name = name.Substring(0, 20);
                land.Name = name;
                treeViewLand.SelectedNode.Text = String.Format("0x{0:X4} {1}", index, name);
                short shortres;
                if (short.TryParse(textBoxTexID.Text, out shortres))
                    land.TextureID = shortres;
                land.Flags = TileFlag.None;
                if (checkedListBox2.GetItemChecked(0))
                    land.Flags |= TileFlag.Damaging;
                if (checkedListBox2.GetItemChecked(1))
                    land.Flags |= TileFlag.Wet;
                if (checkedListBox2.GetItemChecked(2))
                    land.Flags |= TileFlag.Impassable;
                if (checkedListBox2.GetItemChecked(3))
                    land.Flags |= TileFlag.Wall;
                if (checkedListBox2.GetItemChecked(4))
                    land.Flags |= TileFlag.Unknown3;

                TileData.LandTable[index] = land;
                Options.ChangedUltimaClass["TileData"] = true;
                FiddlerControls.Events.FireTileDataChangeEvent(this, index);
                treeViewLand.SelectedNode.ForeColor = Color.Red;
                if (memorySaveWarningToolStripMenuItem.Checked)
                    MessageBox.Show(
                        String.Format("Edits of 0x{0:X4} ({0}) saved to memory. Click 'Save Tiledata' to write to file.", index), "Saved",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
            }
        }

        #region SaveDirectEvents
        private void OnFlagItemCheckItems(object sender, ItemCheckEventArgs e)
        {
            if (saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                if (ChangingIndex)
                    return;
                if (e.CurrentValue != e.NewValue)
                {
                    if (treeViewItem.SelectedNode == null)
                        return;
                    int index = (int)treeViewItem.SelectedNode.Tag;
                    ItemData item = TileData.ItemTable[index];
                    Array EnumValues = System.Enum.GetValues(typeof(TileFlag));
                    TileFlag changeflag = (TileFlag)EnumValues.GetValue(e.Index + 1);
                    if ((item.Flags & changeflag) != 0) //better doublecheck
                    {
                        if (e.NewValue == CheckState.Unchecked)
                        {
                            item.Flags ^= changeflag;
                            TileData.ItemTable[index] = item;
                            treeViewItem.SelectedNode.ForeColor = Color.Red;
                            Options.ChangedUltimaClass["TileData"] = true;
                            FiddlerControls.Events.FireTileDataChangeEvent(this, index + 0x4000);
                        }
                    }
                    else if ((item.Flags & changeflag) == 0)
                    {
                        if (e.NewValue == CheckState.Checked)
                        {
                            item.Flags |= changeflag;
                            TileData.ItemTable[index] = item;
                            treeViewItem.SelectedNode.ForeColor = Color.Red;
                            Options.ChangedUltimaClass["TileData"] = true;
                            FiddlerControls.Events.FireTileDataChangeEvent(this, index + 0x4000);
                        }
                    }

                }
            }
        }

        private void OnTextChangedItemAnim(object sender, EventArgs e)
        {
            if (saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                if (ChangingIndex)
                    return;
                if (treeViewItem.SelectedNode == null)
                    return;
                int index = (int)treeViewItem.SelectedNode.Tag;
                ItemData item = TileData.ItemTable[index];
                short shortres;
                if (short.TryParse(textBoxAnim.Text, out shortres))
                {
                    item.Animation = shortres;
                    TileData.ItemTable[index] = item;
                    treeViewItem.SelectedNode.ForeColor = Color.Red;
                    Options.ChangedUltimaClass["TileData"] = true;
                    FiddlerControls.Events.FireTileDataChangeEvent(this, index + 0x4000);
                }
            }
        }

        private void OnTextChangedItemName(object sender, EventArgs e)
        {
            if (saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                if (ChangingIndex)
                    return;
                if (treeViewItem.SelectedNode == null)
                    return;
                int index = (int)treeViewItem.SelectedNode.Tag;
                ItemData item = TileData.ItemTable[index];
                string name = textBoxName.Text;
                if (name.Length == 0)
                    return;
                if (name.Length > 20)
                    name = name.Substring(0, 20);
                item.Name = name;
                treeViewItem.SelectedNode.Text = String.Format("0x{0:X4} ({0}) {1}", index, name);
                TileData.ItemTable[index] = item;
                treeViewItem.SelectedNode.ForeColor = Color.Red;
                Options.ChangedUltimaClass["TileData"] = true;
                FiddlerControls.Events.FireTileDataChangeEvent(this, index + 0x4000);
            }
        }

        private void OnTextChangedItemWeight(object sender, EventArgs e)
        {
            if (saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                if (ChangingIndex)
                    return;
                if (treeViewItem.SelectedNode == null)
                    return;
                int index = (int)treeViewItem.SelectedNode.Tag;
                ItemData item = TileData.ItemTable[index];
                byte byteres;
                if (byte.TryParse(textBoxWeight.Text, out byteres))
                {
                    item.Weight = byteres;
                    TileData.ItemTable[index] = item;
                    treeViewItem.SelectedNode.ForeColor = Color.Red;
                    Options.ChangedUltimaClass["TileData"] = true;
                    FiddlerControls.Events.FireTileDataChangeEvent(this, index + 0x4000);
                }
            }
        }

        private void OnTextChangedItemQuality(object sender, EventArgs e)
        {
            if (saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                if (ChangingIndex)
                    return;
                if (treeViewItem.SelectedNode == null)
                    return;
                int index = (int)treeViewItem.SelectedNode.Tag;
                ItemData item = TileData.ItemTable[index];
                byte byteres;
                if (byte.TryParse(textBoxQuality.Text, out byteres))
                {
                    item.Quality = byteres;
                    TileData.ItemTable[index] = item;
                    treeViewItem.SelectedNode.ForeColor = Color.Red;
                    Options.ChangedUltimaClass["TileData"] = true;
                    FiddlerControls.Events.FireTileDataChangeEvent(this, index + 0x4000);
                }
            }
        }

        private void OnTextChangedItemQuantity(object sender, EventArgs e)
        {
            if (saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                if (ChangingIndex)
                    return;
                if (treeViewItem.SelectedNode == null)
                    return;
                int index = (int)treeViewItem.SelectedNode.Tag;
                ItemData item = TileData.ItemTable[index];
                byte byteres;
                if (byte.TryParse(textBoxQuantity.Text, out byteres))
                {
                    item.Quantity = byteres;
                    TileData.ItemTable[index] = item;
                    treeViewItem.SelectedNode.ForeColor = Color.Red;
                    Options.ChangedUltimaClass["TileData"] = true;
                    FiddlerControls.Events.FireTileDataChangeEvent(this, index + 0x4000);
                }
            }
        }

        private void OnTextChangedItemHue(object sender, EventArgs e)
        {
            if (saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                if (ChangingIndex)
                    return;
                if (treeViewItem.SelectedNode == null)
                    return;
                int index = (int)treeViewItem.SelectedNode.Tag;
                ItemData item = TileData.ItemTable[index];
                byte byteres;
                if (byte.TryParse(textBoxHue.Text, out byteres))
                {
                    item.Hue = byteres;
                    TileData.ItemTable[index] = item;
                    treeViewItem.SelectedNode.ForeColor = Color.Red;
                    Options.ChangedUltimaClass["TileData"] = true;
                    FiddlerControls.Events.FireTileDataChangeEvent(this, index + 0x4000);
                }
            }
        }

        private void OnTextChangedItemStackOff(object sender, EventArgs e)
        {
            if (saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                if (ChangingIndex)
                    return;
                if (treeViewItem.SelectedNode == null)
                    return;
                int index = (int)treeViewItem.SelectedNode.Tag;
                ItemData item = TileData.ItemTable[index];
                byte byteres;
                if (byte.TryParse(textBoxStackOff.Text, out byteres))
                {
                    item.StackingOffset = byteres;
                    TileData.ItemTable[index] = item;
                    treeViewItem.SelectedNode.ForeColor = Color.Red;
                    Options.ChangedUltimaClass["TileData"] = true;
                    FiddlerControls.Events.FireTileDataChangeEvent(this, index + 0x4000);
                }
            }
        }

        private void OnTextChangedItemValue(object sender, EventArgs e)
        {
            if (saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                if (ChangingIndex)
                    return;
                if (treeViewItem.SelectedNode == null)
                    return;
                int index = (int)treeViewItem.SelectedNode.Tag;
                ItemData item = TileData.ItemTable[index];
                byte byteres;
                if (byte.TryParse(textBoxValue.Text, out byteres))
                {
                    item.Value = byteres;
                    TileData.ItemTable[index] = item;
                    treeViewItem.SelectedNode.ForeColor = Color.Red;
                    Options.ChangedUltimaClass["TileData"] = true;
                    FiddlerControls.Events.FireTileDataChangeEvent(this, index + 0x4000);
                }
            }
        }

        private void OnTextChangedItemHeight(object sender, EventArgs e)
        {
            if (saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                if (ChangingIndex)
                    return;
                if (treeViewItem.SelectedNode == null)
                    return;
                int index = (int)treeViewItem.SelectedNode.Tag;
                ItemData item = TileData.ItemTable[index];
                byte byteres;
                if (byte.TryParse(textBoxHeigth.Text, out byteres))
                {
                    item.Height = byteres;
                    TileData.ItemTable[index] = item;
                    treeViewItem.SelectedNode.ForeColor = Color.Red;
                    Options.ChangedUltimaClass["TileData"] = true;
                    FiddlerControls.Events.FireTileDataChangeEvent(this, index + 0x4000);
                }
            }
        }

        private void OnTextChangedItemMiscData(object sender, EventArgs e)
        {
            if (saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                if (ChangingIndex)
                    return;
                if (treeViewItem.SelectedNode == null)
                    return;
                int index = (int)treeViewItem.SelectedNode.Tag;
                ItemData item = TileData.ItemTable[index];
                short shortres;
                if (short.TryParse(textBoxUnk1.Text, out shortres))
                {
                    item.MiscData = shortres;
                    TileData.ItemTable[index] = item;
                    treeViewItem.SelectedNode.ForeColor = Color.Red;
                    Options.ChangedUltimaClass["TileData"] = true;
                    FiddlerControls.Events.FireTileDataChangeEvent(this, index + 0x4000);
                }
            }
        }

        private void OnTextChangedItemUnk2(object sender, EventArgs e)
        {
            if (saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                if (ChangingIndex)
                    return;
                if (treeViewItem.SelectedNode == null)
                    return;
                int index = (int)treeViewItem.SelectedNode.Tag;
                ItemData item = TileData.ItemTable[index];
                byte byteres;
                if (byte.TryParse(textBoxUnk2.Text, out byteres))
                {
                    item.Unk2 = byteres;
                    TileData.ItemTable[index] = item;
                    treeViewItem.SelectedNode.ForeColor = Color.Red;
                    Options.ChangedUltimaClass["TileData"] = true;
                    FiddlerControls.Events.FireTileDataChangeEvent(this, index + 0x4000);
                }
            }
        }

        private void OnTextChangedItemUnk3(object sender, EventArgs e)
        {
            if (saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                if (ChangingIndex)
                    return;
                if (treeViewItem.SelectedNode == null)
                    return;
                int index = (int)treeViewItem.SelectedNode.Tag;
                ItemData item = TileData.ItemTable[index];
                byte byteres;
                if (byte.TryParse(textBoxUnk3.Text, out byteres))
                {
                    item.Unk3 = byteres;
                    TileData.ItemTable[index] = item;
                    treeViewItem.SelectedNode.ForeColor = Color.Red;
                    Options.ChangedUltimaClass["TileData"] = true;
                    FiddlerControls.Events.FireTileDataChangeEvent(this, index + 0x4000);
                }
            }
        }

        private void OnTextChangedLandName(object sender, EventArgs e)
        {
            if (saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                if (ChangingIndex)
                    return;
                if (treeViewLand.SelectedNode == null)
                    return;
                int index = (int)treeViewLand.SelectedNode.Tag;
                LandData land = TileData.LandTable[index];
                string name = textBoxNameLand.Text;
                if (name.Length == 0)
                    return;
                if (name.Length > 20)
                    name = name.Substring(0, 20);
                land.Name = name;
                treeViewLand.SelectedNode.Text = String.Format("0x{0:X4} ({0}) {1}", index, name);
                TileData.LandTable[index] = land;
                treeViewLand.SelectedNode.ForeColor = Color.Red;
                Options.ChangedUltimaClass["TileData"] = true;
                FiddlerControls.Events.FireTileDataChangeEvent(this, index);
            }
        }

        private void OnTextChangedLandTexID(object sender, EventArgs e)
        {
            if (saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                if (ChangingIndex)
                    return;
                if (treeViewLand.SelectedNode == null)
                    return;
                int index = (int)treeViewLand.SelectedNode.Tag;
                LandData land = TileData.LandTable[index];
                short shortres;
                if (short.TryParse(textBoxTexID.Text, out shortres))
                {
                    land.TextureID = shortres;
                    TileData.LandTable[index] = land;
                    treeViewLand.SelectedNode.ForeColor = Color.Red;
                    Options.ChangedUltimaClass["TileData"] = true;
                    FiddlerControls.Events.FireTileDataChangeEvent(this, index);
                }
            }
        }

        private void OnFlagItemCheckLandtiles(object sender, ItemCheckEventArgs e)
        {
            if (saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                if (ChangingIndex)
                    return;
                if (e.CurrentValue != e.NewValue)
                {
                    if (treeViewLand.SelectedNode == null)
                        return;
                    int index = (int)treeViewLand.SelectedNode.Tag;
                    LandData land = TileData.LandTable[index];
                    TileFlag changeflag;
                    switch (e.Index)
                    {
                        case 0: changeflag = TileFlag.Damaging;
                            break;
                        case 1: changeflag = TileFlag.Wet;
                            break;
                        case 2: changeflag = TileFlag.Impassable;
                            break;
                        case 3: changeflag = TileFlag.Wall;
                            break;
                        case 4: changeflag = TileFlag.Unknown3;
                            break;
                        default: changeflag = TileFlag.None;
                            break;
                    }

                    if ((land.Flags & changeflag) != 0)
                    {
                        if (e.NewValue == CheckState.Unchecked)
                        {
                            land.Flags ^= changeflag;
                            TileData.LandTable[index] = land;
                            treeViewLand.SelectedNode.ForeColor = Color.Red;
                            Options.ChangedUltimaClass["TileData"] = true;
                            FiddlerControls.Events.FireTileDataChangeEvent(this, index);
                        }
                    }
                    else if ((land.Flags & changeflag) == 0)
                    {
                        if (e.NewValue == CheckState.Checked)
                        {
                            land.Flags |= changeflag;
                            TileData.LandTable[index] = land;
                            treeViewLand.SelectedNode.ForeColor = Color.Red;
                            Options.ChangedUltimaClass["TileData"] = true;
                            FiddlerControls.Events.FireTileDataChangeEvent(this, index);
                        }
                    }
                }
            }
        }
        #endregion


        private void OnClickExport(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            if (tabcontrol.SelectedIndex == 0) //items
            {
                string FileName = Path.Combine(path, "ItemData.csv");
                Ultima.TileData.ExportItemDataToCSV(FileName);
                MessageBox.Show(String.Format("ItemData saved to {0}", FileName), "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                string FileName = Path.Combine(path, "LandData.csv");
                Ultima.TileData.ExportLandDataToCSV(FileName);
                MessageBox.Show(String.Format("LandData saved to {0}", FileName), "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        private TileDatasSearch showform1 = null;
        private TileDatasSearch showform2 = null;
        private void OnClickSearch(object sender, EventArgs e)
        {
            if (tabcontrol.SelectedIndex == 0) //items
            {
                if ((showform1 == null) || (showform1.IsDisposed))
                {
                    showform1 = new TileDatasSearch(false);
                    showform1.TopMost = true;
                    showform1.Show();
                }
            }
            else //landtiles
            {
                if ((showform2 == null) || (showform2.IsDisposed))
                {
                    showform2 = new TileDatasSearch(true);
                    showform2.TopMost = true;
                    showform2.Show();
                }
            }
        }

        private void OnClickSelectItem(object sender, EventArgs e)
        {
            if (treeViewItem.SelectedNode == null)
                return;
            int index = (int)treeViewItem.SelectedNode.Tag;
            if (Options.DesignAlternative)
                FiddlerControls.ItemShowAlternative.SearchGraphic(index);
            else
                FiddlerControls.ItemShow.SearchGraphic(index);
        }

        private void OnClickSelectInLandtiles(object sender, EventArgs e)
        {
            if (treeViewLand.SelectedNode == null)
                return;
            int index = (int)treeViewLand.SelectedNode.Tag;
            if (Options.DesignAlternative)
                FiddlerControls.LandTilesAlternative.SearchGraphic(index);
            else
                FiddlerControls.LandTiles.SearchGraphic(index);
        }

        private void OnClickSelectRadarItem(object sender, EventArgs e)
        {
            if (treeViewItem.SelectedNode == null)
                return;
            int index = (int)treeViewItem.SelectedNode.Tag;
            FiddlerControls.RadarColor.Select(index, false);
        }

        private void OnClickSelectRadarLand(object sender, EventArgs e)
        {
            if (treeViewLand.SelectedNode == null)
                return;
            int index = (int)treeViewLand.SelectedNode.Tag;
            FiddlerControls.RadarColor.Select(index, true);
        }

        private void OnClickImport(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "Choose csv file to import";
            dialog.CheckFileExists = true;
            dialog.Filter = "csv files (*.csv)|*.csv";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Options.ChangedUltimaClass["TileData"] = true;
                if (tabcontrol.SelectedIndex == 0)//items
                {
                    Ultima.TileData.ImportItemDataFromCSV(dialog.FileName);
                    AfterSelectTreeViewItem(this, new TreeViewEventArgs(treeViewItem.SelectedNode));
                }
                else
                {
                    Ultima.TileData.ImportLandDataFromCSV(dialog.FileName);
                    AfterSelectTreeViewLand(this, new TreeViewEventArgs(treeViewLand.SelectedNode));
                }
            }
            dialog.Dispose();
        }

        private TileDataFilter filterform = null;
        private void OnClickSetFilter(object sender, EventArgs e)
        {
            if ((filterform == null) || (filterform.IsDisposed))
            {
                filterform = new TileDataFilter();
                filterform.TopMost = true;
                filterform.Show();
            }
        }

        private bool[] ChangesInTiledataOrRadarCol;

        #region Cliloc

            private static string[] ClilocNames;

            private void ReloadCliloc()
            {
                ClilocNames = new string[ItemMaxIndex];
                Ultima.StringTable cliloc = new StringTable();

                for (int i = 1; i < 0x4000; ++i)
                    ClilocNames[i] = cliloc.GetText(1020000 + i);
                if (ItemMaxIndex >= 0x7FFF)
                    for (int i = 0x4000; i < 0x8000; ++i)
                        ClilocNames[i] = cliloc.GetText(1078872 + i);
                if (ItemMaxIndex >= 0xFFDC)
                    for (int i = 0x8000; i < 0xFFDC; ++i)
                        ClilocNames[i] = cliloc.GetText(1084024 + i);
            }

        #endregion

        #region RadarColor

            private bool Updating = false;

            [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public short ItemCurrColor
            {
                get { return m_ItemCurrCol; }
                set
                {
                    if (m_ItemCurrCol != value || pictureBoxItemColor.Image == null)
                    {
                        m_ItemCurrCol = value;
                        Updating = true;
                       
                        pictureBoxItemColor.Image = null;
                        Color col = Ultima.Hues.HueToColor(value);
                        pictureBoxItemColor.BackColor = col;
                        numericUpDownItemR.Value = col.R;
                        numericUpDownItemG.Value = col.G;
                        numericUpDownItemB.Value = col.B;

                        textBoxItemHexCol.Text = String.Format("{0:X2}{1:X2}{2:X2}", col.R, col.G, col.B);
                        Updating = false;
                    }
                }
            }
            private short m_ItemCurrCol = -1;

            [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public short LandCurrColor
            {
                get { return m_LandCurrCol; }
                set
                {
                    if (m_LandCurrCol != value || pictureBoxLandColor.Image == null)
                    {
                        m_LandCurrCol = value;
                        Updating = true;

                        pictureBoxLandColor.Image = null;
                        Color col = Ultima.Hues.HueToColor(value);
                        pictureBoxLandColor.BackColor = col;
                        numericUpDownLandR.Value = col.R;
                        numericUpDownLandG.Value = col.G;
                        numericUpDownLandB.Value = col.B;

                        textBoxLandHexCol.Text = String.Format("{0:X2}{1:X2}{2:X2}", col.R, col.G, col.B);
                        Updating = false;
                    }
                }
            }
            private short m_LandCurrCol = -1;

            private void ShowItemRadarCol(List<int> SelectedIndex)
            {
                short color = Ultima.RadarCol.GetItemColor(SelectedIndex[0]);
                for (int i = 1; i < SelectedIndex.Count; ++i)
                    if(color != Ultima.RadarCol.GetItemColor(SelectedIndex[i]))
                    {
                        Updating = true;
                        string text = "Разные цвета";
                        Bitmap bit = new Bitmap(pictureBoxItemColor.Size.Width, pictureBoxItemColor.Size.Height);
                        Graphics graph = Graphics.FromImage(bit);
                        graph.Clear(Color.FromArgb(-1));
                        graph.DrawString(text, Fonts.DefaultFont, Brushes.DarkGray, 30, (bit.Height - 12) / 2);
                        //graph.Flush();
                        //graph.Save();
                        pictureBoxItemColor.Image = bit;
                        textBoxItemHexCol.Text = String.Empty;
                        numericUpDownItemR.Value = 0;
                        numericUpDownItemG.Value = 0;
                        numericUpDownItemB.Value = 0;
                        Updating = false;
                        return;
                    }
                ItemCurrColor = color;
            }

            private void ShowLandRadarCol(List<int> SelectedIndex)
            {
                short color = Ultima.RadarCol.GetLandColor(SelectedIndex[0]);
                for (int i = 1; i < SelectedIndex.Count; ++i)
                    if (color != Ultima.RadarCol.GetLandColor(SelectedIndex[i]))
                    {
                        Updating = true;
                        string text = "Разные цвета";
                        Bitmap bit = new Bitmap(pictureBoxLandColor.Size.Width, pictureBoxItemColor.Size.Height);
                        Graphics graph = Graphics.FromImage(bit);
                        graph.Clear(Color.FromArgb(-1));
                        graph.DrawString(text, Fonts.DefaultFont, Brushes.DarkGray, 30, (bit.Height - 12) / 2);
                        //graph.Flush();
                        //graph.Save();
                        pictureBoxLandColor.Image = bit;
                        textBoxLandHexCol.Text = String.Empty;
                        numericUpDownLandR.Value = 0;
                        numericUpDownLandG.Value = 0;
                        numericUpDownLandB.Value = 0;
                        Updating = false;
                        return;
                    }
                LandCurrColor = color;
            }

            private void SaveChangesInRadarcolItems(int index)
            {
                if (pictureBoxItemColor.Image != null || ItemCurrColor < 0)
                    return;

                if (RadarCol.GetItemColor(index) != ItemCurrColor) {
                    RadarCol.SetItemColor(index, ItemCurrColor);
                    ChangesInTiledataOrRadarCol[LandMaxIndex + index] = true;

                    Options.ChangedUltimaClass["RadarCol"] = true;
                }
            }

            private void SaveChangesInRadarcolLands(int index)
            {
                if (pictureBoxLandColor.Image != null || LandCurrColor < 0)
                    return;

                if (RadarCol.GetLandColor(index) != LandCurrColor) {
                    RadarCol.SetLandColor(index, LandCurrColor);
                    ChangesInTiledataOrRadarCol[index] = true;

                    Options.ChangedUltimaClass["RadarCol"] = true;
                }
            }

            private void onChangeItemR(object sender, EventArgs e)
            {
                if (!Updating)
                {
                    Color col = Color.FromArgb((int)numericUpDownItemR.Value, (int)numericUpDownItemG.Value, (int)numericUpDownItemB.Value);
                    ItemCurrColor = Ultima.Hues.ColorToHue(col);
                }
            }

            private void onChangeLandR(object sender, EventArgs e)
            {
                if (!Updating)
                {
                    Color col = Color.FromArgb((int)numericUpDownLandR.Value, (int)numericUpDownLandG.Value, (int)numericUpDownLandB.Value);
                    LandCurrColor = Ultima.Hues.ColorToHue(col);
                }
            }

            private void OnChangeItemG(object sender, EventArgs e)
            {
                if (!Updating)
                {
                    Color col = Color.FromArgb((int)numericUpDownItemR.Value, (int)numericUpDownItemG.Value, (int)numericUpDownItemB.Value);
                    ItemCurrColor = Ultima.Hues.ColorToHue(col);
                }
            }

            private void onChangeLandG(object sender, EventArgs e)
            {
                if (!Updating)
                {
                    Color col = Color.FromArgb((int)numericUpDownLandR.Value, (int)numericUpDownLandG.Value, (int)numericUpDownLandB.Value);
                    LandCurrColor = Ultima.Hues.ColorToHue(col);
                }
            }

            private void OnChangeItemB(object sender, EventArgs e)
            {
                if (!Updating)
                {
                    Color col = Color.FromArgb((int)numericUpDownItemR.Value, (int)numericUpDownItemG.Value, (int)numericUpDownItemB.Value);
                    ItemCurrColor = Ultima.Hues.ColorToHue(col);
                    
                }
            }

            private void onChangeLandB(object sender, EventArgs e)
            {
                if (!Updating)
                {
                    Color col = Color.FromArgb((int)numericUpDownLandR.Value, (int)numericUpDownLandG.Value, (int)numericUpDownLandB.Value);
                    LandCurrColor = Ultima.Hues.ColorToHue(col);
                }
            }

            private void OnChangeItemHexText(object sender, EventArgs e)
            {
                if (!Updating)
                {
                    Updating = true;
                    try
                    {
                        if (textBoxItemHexCol.Text.Length != 6)
                            throw new Exception();
                        int col = Convert.ToInt32(textBoxItemHexCol.Text, 16);
                        ItemCurrColor = Ultima.Hues.ColorToHue(Color.FromArgb(col));
                    }
                    catch (Exception ex)
                    {
                        Color col = Ultima.Hues.HueToColor(ItemCurrColor);
                        textBoxItemHexCol.Text = String.Format("{0:X2}{1:X2}{2:X2}", col.R, col.G, col.B);
                    }
                    Updating = false;
                }
            }

            private void OnChangeLandHexText(object sender, EventArgs e)
            {
                if (!Updating)
                {
                    Updating = true;
                    try
                    {
                        if (textBoxLandHexCol.Text.Length != 6)
                            throw new Exception();
                        int col = Convert.ToInt32(textBoxLandHexCol.Text, 16);
                        LandCurrColor = Ultima.Hues.ColorToHue(Color.FromArgb(col));
                    }
                    catch (Exception ex)
                    {
                        Color col = Ultima.Hues.HueToColor(LandCurrColor);
                        textBoxLandHexCol.Text = String.Format("{0:X2}{1:X2}{2:X2}", col.R, col.G, col.B);
                    }
                    Updating = false;
                }
            }

            private void GenereteColorButton_Click(object sender, EventArgs e)
            {
                if (listViewItemSelected.Count == 1) {
                    int item = listViewItemSelected[0];
                    ItemCurrColor = Utils.AverageCol(Ultima.Art.GetStatic(item));
                } else {
                    foreach (int item in listViewItemSelected) {
                        short color = Utils.AverageCol(Ultima.Art.GetStatic(item));
                        if (RadarCol.GetItemColor(item) != color) {
                            RadarCol.SetItemColor(item, color);
                            ChangesInTiledataOrRadarCol[LandMaxIndex + item] = true;
                            Options.ChangedUltimaClass["RadarCol"] = true;
                        }
                    }
                    // ItemCurrColor = -1;
                    if (listViewItemSelected.Count != 0)
                        ShowLandRadarCol(listViewLandSelected); // Обновление радаркол для выбранных элементов
                }
            }

            private void ChangedColorButton_Click(object sender, EventArgs e)
            {
                colorDialog.FullOpen = true;
                colorDialog.AnyColor = true;
                colorDialog.SolidColorOnly = true;
                colorDialog.Color = pictureBoxItemColor.BackColor;
                if (colorDialog.ShowDialog(this) == DialogResult.OK) {
                    ItemCurrColor = Ultima.Hues.ColorToHue(colorDialog.Color);
                }
            }

            private void GenereteColorButton2_Click(object sender, EventArgs e)
            {
                if (listViewLandSelected.Count == 1) {
                    int land = listViewLandSelected[0];
                    LandCurrColor = Utils.AverageCol(Ultima.Art.GetLand(land));
                } else {
                    foreach (int land in listViewLandSelected) {
                        short color = Utils.AverageCol(Ultima.Art.GetLand(land));
                        if (RadarCol.GetLandColor(land) != color) {
                            RadarCol.SetLandColor(land, color);
                            ChangesInTiledataOrRadarCol[land] = true;
                            Options.ChangedUltimaClass["RadarCol"] = true;
                        }
                    }
                    // LandCurrColor = -1;
                    if (listViewLandSelected.Count != 0)
                        ShowLandRadarCol(listViewLandSelected); // Обновление радаркол для выбранных элементов
                }
            }

            private void ChangedColorButton2_Click(object sender, EventArgs e)
            {
                colorDialog.FullOpen = true;
                colorDialog.AnyColor = true;
                colorDialog.SolidColorOnly = true;
                colorDialog.Color = pictureBoxLandColor.BackColor;
                if (colorDialog.ShowDialog(this) == DialogResult.OK) {
                    LandCurrColor = Ultima.Hues.ColorToHue(colorDialog.Color);
                }
            }

            private void OnClickEditRadarcol(object sender, EventArgs e)
            { 
                numericUpDownItemR.Enabled = numericUpDownItemG.Enabled = numericUpDownItemB.Enabled =
                    textBoxItemHexCol.Enabled = ChangedColorButton.Enabled = 
                    numericUpDownLandR.Enabled = numericUpDownLandG.Enabled = numericUpDownLandB.Enabled =
                    textBoxLandHexCol.Enabled = ChangedColorButton2.Enabled = 
                    GenereteColorButton.Enabled = GenereteColorButton2.Enabled =
                    EditRadarColToolStripMenuItem.Checked;
            }

            private void OnClickSaveRadarcol(object sender, EventArgs e)
            {
                if (EditRadarColToolStripMenuItem.Checked && pictureBoxLandColor.Image == null)
                    foreach (int land in listViewLandSelected)
                        SaveChangesInRadarcolLands(land);   // Сохранение радаркола для выделенного элемента

                if (EditRadarColToolStripMenuItem.Checked && pictureBoxItemColor.Image == null)
                    foreach (int item in listViewItemSelected)
                        SaveChangesInRadarcolItems(item);   // Сохранение радаркола для выделенного элемента

                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string FileName = Path.Combine(path, "Mul Files", "radarcol.mul");
                Ultima.RadarCol.Save(FileName);
                MessageBox.Show(
                    String.Format("RadarCol сохраненна в {0}", FileName),
                    "Сохраненно", MessageBoxButtons.OK, MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                Options.ChangedUltimaClass["RadarCol"] = false;
            }
            
        #endregion

        #region ItemList

            private void AfterSelectTreeViewItem(object sender, TreeViewEventArgs e)
            {
                if (e.Node == null)
                    return;
                int index = (int)e.Node.Tag;

                SelectTreeViewItem(index);
            }

        #endregion

        #region ItemShow
            
            private static bool Loaded_ItemList = false;

            private void ReloadItems()
            {
                Cursor.Current = Cursors.WaitCursor;
                Options.LoadedUltimaClass["TileData"] = true;
                Options.LoadedUltimaClass["Art"] = true;
                Options.LoadedUltimaClass["RadarColor"] = true;

                if (!Loaded_ItemList)
                    ItemHideLoad();

                if (Options.UseArtListInTileShow)
                {
                    splitContainer4.Panel1Collapsed = true;
                    ////////////////////////////////////////////
                    /// Инициализация listViewItem
                    ////////////////////////////////////////////
                    listViewItem.BeginUpdate();
                    listViewItem.Clear();
                    List<ListViewItem> itemcache = new List<ListViewItem>();
                    if (((Files.UseHashFile) && (Files.CompareHashFile("Art"))) && (!Ultima.Art.Modified))
                    {
                        string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                        string FileName = Path.Combine(path, "UOFiddlerArt.hash");
                        if (File.Exists(FileName))
                        {
                            using (FileStream bin = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                unsafe
                                {
                                    byte[] buffer = new byte[bin.Length];
                                    bin.Read(buffer, 0, (int)bin.Length);
                                    fixed (byte* bf = buffer)
                                    {
                                        int* poffset = (int*)bf;
                                        int offset = *poffset + 4;
                                        int* dat = (int*)(bf + offset);
                                        int i = offset;
                                        while (i < buffer.Length)
                                        {
                                            int j = *dat++;
                                            if (!IsItemHide(j))
                                            {
                                                ListViewItem item = new ListViewItem(j.ToString(), 0);
                                                item.Name = j.ToString();
                                                item.Tag = j;
                                                itemcache.Add(item);
                                            }
                                            i += 4;
                                        }
                                    }
                                }
                                //int length = bin.ReadInt32();
                                //bin.BaseStream.Seek(length, SeekOrigin.Current);
                                //while (bin.BaseStream.Length != bin.BaseStream.Position)
                                //{
                                //    int i = bin.ReadInt32();
                                //    ListViewItem item = new ListViewItem(i.ToString(), 0);
                                //    item.Tag = i;
                                //    itemcache.Add(item);
                                //}
                            }
                            listViewItem.Items.AddRange(itemcache.ToArray());
                        }
                    }
                    else
                    {
                        int staticlength = ItemMaxIndex;
                        for (int i = 0; i < staticlength; ++i)
                        {
                            if (!IsItemHide(i) && Art.IsValidStatic(i))
                            {
                                ListViewItem item = new ListViewItem(i.ToString(), 0);
                                item.Name = i.ToString();
                                item.Tag = i;
                                itemcache.Add(item);
                            }
                        }
                        listViewItem.Items.AddRange(itemcache.ToArray());

                        if (Files.UseHashFile)
                            MakeHashFile();
                    }

                    listViewItem.TileSize = new Size(Options.ArtItemSizeWidth, Options.ArtItemSizeHeight);
                    listViewItem.EndUpdate();
                }
                else
                {
                    splitContainer4.Panel2Collapsed = true;
                    ////////////////////////////////////////////
                    /// Инициализация treeViewItem
                    ////////////////////////////////////////////
                    treeViewItem.BeginUpdate();
                    treeViewItem.Nodes.Clear();
                    if (TileData.ItemTable != null)
                    {
                        TreeNode[] nodes = new TreeNode[TileData.ItemTable.Length];
                        for (int i = 0; i < TileData.ItemTable.Length; ++i)
                        {
                            TreeNode node = new TreeNode(String.Format("0x{0:X4} ({0:D5})\t {1}", i, TileData.ItemTable[i].Name));
                            node.Tag = i;
                            nodes[i] = node;
                        }
                        treeViewItem.Nodes.AddRange(nodes);
                    }
                    treeViewItem.EndUpdate();
                }

                /*
                treeViewLand.BeginUpdate();
                treeViewLand.Nodes.Clear();
                if (TileData.LandTable != null)
                {
                    TreeNode[] nodes = new TreeNode[TileData.LandTable.Length];
                    for (int i = 0; i < TileData.LandTable.Length; ++i)
                    {
                        TreeNode node = new TreeNode(String.Format("0x{0:X4} ({0}) {1}", i, TileData.LandTable[i].Name));
                        node.Tag = i;
                        nodes[i] = node;
                    }
                    treeViewLand.Nodes.AddRange(nodes);
                }
                treeViewLand.EndUpdate();
                if (!Loaded)
                {
                    FiddlerControls.Events.FilePathChangeEvent += new FiddlerControls.Events.FilePathChangeHandler(OnFilePathChangeEvent);
                    FiddlerControls.Events.TileDataChangeEvent += new FiddlerControls.Events.TileDataChangeHandler(OnTileDataChangeEvent);
                }
                */
                //ReloadLands();
                //ReloadAnims();

                Loaded_ItemList = true;
                Cursor.Current = Cursors.Default;
            }

            private void OnEnterItemTab()
            {
                if (!Loaded_ItemList)
                    ReloadItems();
            }

            private void OnLeaveItemTab()
            {
            }

            #region SendItem

            private void senditemcommand(string cmd, int itemid = -1, string args = "")
            {
                int index = itemid;
                if (index == -1)
                {
                    if(listViewItemSelected.Count != 1)
                        return;
                    index = listViewItemSelected[0];
                }
                if (Client.Running)
                        Ultima.Client.SendText(String.Format("{0} 0x{1:X4} {2}", cmd, index, args));
                    else
                        MessageBox.Show("Клиент не запущен или не отвечает.", "SendItem", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            
            private void addStaticSingleToolStripMenuItem_Click(object sender, EventArgs e)
            {
                senditemcommand("[add static");
            }

            private void addStaticMultiToolStripMenuItem_Click(object sender, EventArgs e)
            {
                senditemcommand("[m add static");
            }

            private void addStaticAreaToolStripMenuItem_Click(object sender, EventArgs e)
            {
                senditemcommand("[tile static");
            }
            
            private void listViewItem_DoubleClick(object sender, EventArgs e)
            {
                senditemcommand("[add static");
            }

            #endregion

            private bool[] m_ItemHide = null;
            private void ItemHideLoad()
            {
                if (m_ItemHide == null || m_ItemHide.Length != ItemMaxIndex)
                    m_ItemHide = new bool[ItemMaxIndex];
                Array.Clear(m_ItemHide, 0, m_ItemHide.Length);
                XML_ItemShowInitialize();
            }

            private bool IsItemHide(int i)
            {
                if (!ItemsIgnorListStripMenuItem.Checked)
                    return false;
                if (i >= m_ItemHide.Length)
                    return false;
                return m_ItemHide[i];
            }

            private void XML_ItemShowInitialize()
            {
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string FileName = Path.Combine(path, @"ignoritemlist.xml");
                if (!File.Exists(FileName))
                    return;

                XmlDocument dom = new XmlDocument();
                dom.Load(FileName);
                XmlElement xTiles = dom["TileGroups"];

                foreach (XmlElement xGroup in xTiles)
                    foreach (XmlElement elem in xGroup.ChildNodes)
                    {
                        int i = Int32.Parse(elem.GetAttribute("index"));
                        if (i < m_ItemHide.Length)
                            m_ItemHide[i] = true;
                    }
            }
            
            private void ItemsIgnorListStripMenuItem_CheckedChanged(object sender, EventArgs e)
            {
                /*
                FileStream fs = File.Create(@"C:\UltimaOnline\source\_build\uoFiddler\unused.txt");
                StreamWriter sw = new StreamWriter(fs);
                int first = -1;
                int last = -1;
                for (int i = 0; i < m_ItemHide.Length; ++i)
                {
                    if (!m_ItemHide[i] && i != m_ItemHide.Length - 1)
                        continue;
                    if(last < 0)
                    {
                        sw.Write(String.Format("S${0:X}", i));
                        first = last = i;
                    }
                    else if (last + 1 == i)
                    {
                        ++last;
                    }
                    else
                    {
                        if (first != last)
                            sw.Write(String.Format("-${0:X}", last));
                        sw.WriteLine();
                        sw.Write(String.Format("S${0:X}", i));
                        first = last = i;

                        //first = last = -1;
                    }
                }
                sw.Flush();
                sw.Close();
                return;
                */
                /*
                //Ultima.MapWorker mapInf = new Ultima.MapWorker(@"C:\UltimaOnline\client", 0, 7168, 4096);
                //Ultima.MapWorker mapWriter = new Ultima.MapWorker(@"C:\UltimaOnline\source\_build\uoFiddler", 0, 7168, 4096);
                Ultima.MapWorker mapInf = new Ultima.MapWorker(@"C:\UltimaOnline\client", 1, 12288, 8192);
                Ultima.MapWorker mapWriter = new Ultima.MapWorker(@"C:\UltimaOnline\source\_build\uoFiddler", 1, 12288, 8192);
                mapWriter.Create();
                mapInf.Open();

                //Ultima.MapWorker.Data[] data = new Ultima.MapWorker.Data[mapInf.Width >> 3];

                for (int f = 0; f < (mapInf.Width >> 3); ++f)
                {
                    Ultima.MapWorker.Block[] m_Bloak = mapInf.ReadLine1();
                    mapWriter.Write1(m_Bloak);
                }

                mapWriter.Close();
                mapInf.Close();
                return;
                */

                //Reload(); 
                listViewItem.BeginUpdate();
                if (ItemsIgnorListStripMenuItem.Checked)
                {
                    for (int j = listViewItem.Items.Count - 1; j >= 0; --j)
                    {
                        int tag = (int)listViewItem.Items[j].Tag;
                        if (m_ItemHide[tag])
                            listViewItem.Items.RemoveAt(j);
                    }
                }
                else
                {
                    ListViewItem item;
                    int itemindex = 0;
                    for (int i = 0; i < m_ItemHide.Length; ++i)
                        if(m_ItemHide[i])
                        {
                            for( ; itemindex < listViewItem.Items.Count; ++itemindex)
                                if((int)listViewItem.Items[itemindex].Tag > i)
                                {
                                    item = new ListViewItem(i.ToString(), 0);
                                    item.Name = i.ToString();
                                    item.Tag = i;
                                    listViewItem.Items.Insert(itemindex, item);
                                    break;
                                }
                            if (itemindex >= listViewItem.Items.Count)
                            {
                                item = new ListViewItem(i.ToString(), 0);
                                item.Name = i.ToString();
                                item.Tag = i;
                                listViewItem.Items.Insert(itemindex, item);
                                ++itemindex;
                            }
                        }
                }
                listViewItem.EndUpdate();
                listViewItem.View = View.Details; // that works faszinating
                listViewItem.View = View.Tile;
            }

            private void ItemsViewEmptyStripMenuItem_CheckedChanged(object sender, EventArgs e)
            {
                listViewItem.BeginUpdate();
                if (ItemsViewEmptyStripMenuItem.Checked)
                {
                    //Reload(); 
                    for (int j = listViewItem.Items.Count - 1; j >= 0; --j)
                    {
                        int tag = (int)listViewItem.Items[j].Tag;
                        if (!Ultima.Art.IsValidStatic(tag))
                            listViewItem.Items.RemoveAt(j);
                    }
                }
                else
                {
                    ListViewItem item;
                    for (int j = 0, i = 0; j < ItemMaxIndex; ++j, ++i)     
                    {
                        if (IsItemHide(j))
                        {
                            --i;
                            continue;
                        }

                        if (listViewItem.Items.Count > i)
                        {
                            if ((int)listViewItem.Items[i].Tag != j)
                            {
                                item = new ListViewItem(j.ToString(), 0);
                                item.Tag = j;
                                listViewItem.Items.Insert(i, item);
                            }
                        }
                        else
                        {
                            item = new ListViewItem(j.ToString(), 0);
                            item.Tag = j;
                            listViewItem.Items.Insert(i, item);
                        }
                    }
                }
                listViewItem.EndUpdate();
                listViewItem.View = View.Details; // that works faszinating
                listViewItem.View = View.Tile;
            }

            private void MakeHashFile()
            {
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string FileName = Path.Combine(path, "UOFiddlerArt.hash");
                using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    using (BinaryWriter bin = new BinaryWriter(fs))
                    {
                        byte[] md5 = Files.GetMD5(Files.GetFilePath("Art.mul"));
                        if (md5 == null)
                            return;
                        int length = md5.Length;
                        bin.Write(length);
                        bin.Write(md5);
                        foreach (ListViewItem item in listViewItem.Items)
                        {
                            bin.Write((int)item.Tag);
                        }
                    }
                }
            }

            // не юзается
            private void ItemListUpdate()
            {
                listViewItem.BeginUpdate();
                listViewItem.Clear();
                List<ListViewItem> itemcache = new List<ListViewItem>();
                if (((Files.UseHashFile) && (Files.CompareHashFile("Art"))) && (!Ultima.Art.Modified))
                {
                    string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                    string FileName = Path.Combine(path, "UOFiddlerArt.hash");
                    if (File.Exists(FileName))
                    {
                        using (FileStream bin = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            unsafe
                            {
                                byte[] buffer = new byte[bin.Length];
                                bin.Read(buffer, 0, (int)bin.Length);
                                fixed (byte* bf = buffer)
                                {
                                    int* poffset = (int*)bf;
                                    int offset = *poffset + 4;
                                    int* dat = (int*)(bf + offset);
                                    int i = offset;
                                    while (i < buffer.Length)
                                    {
                                        int j = *dat++;
                                        if(!IsItemHide(i))
                                        {
                                            ListViewItem item = new ListViewItem(j.ToString(), 0);
                                            item.Name = i.ToString();
                                            item.Tag = j;
                                            itemcache.Add(item);
                                        }
                                        i += 4;
                                    }
                                }
                            }
                            //int length = bin.ReadInt32();
                            //bin.BaseStream.Seek(length, SeekOrigin.Current);
                            //while (bin.BaseStream.Length != bin.BaseStream.Position)
                            //{
                            //    int i = bin.ReadInt32();
                            //    ListViewItem item = new ListViewItem(i.ToString(), 0);
                            //    item.Tag = i;
                            //    itemcache.Add(item);
                            //}
                        }
                        listViewItem.Items.AddRange(itemcache.ToArray());
                    }
                }
                else
                {
                    int staticlength = ItemMaxIndex;
                    for (int i = 0; i < staticlength; ++i)
                    {
                        if (!IsItemHide(i) && Art.IsValidStatic(i))
                        {
                            ListViewItem item = new ListViewItem(i.ToString(), 0);
                            item.Name = i.ToString();
                            item.Tag = i;
                            itemcache.Add(item);
                        }
                    }
                    listViewItem.Items.AddRange(itemcache.ToArray());

                    if (Files.UseHashFile)
                        MakeHashFile();
                }

                listViewItem.TileSize = new Size(Options.ArtItemSizeWidth, Options.ArtItemSizeHeight);
                listViewItem.EndUpdate();
            }

            static Brush BrushBackground = Brushes.Black;
            static Brush BrushSelectBack = Brushes.LightBlue;
            static Brush BrushChangeBack = Brushes.BlueViolet;
            static Brush BrushPatchBack = Brushes.LightGreen;
            static Brush BrushEmptyBack = Brushes.Red;
            static Pen PenCommonBorder = new Pen(Color.FromArgb(32, 32, 32));
            static Pen PenSelectBorder = Pens.White;//Pens.Black;

            private void drawitem(object sender, DrawListViewItemEventArgs e)
            {
                int i = (int)e.Item.Tag;
                bool patched;
                Bitmap bmp = Art.GetStatic(i, out patched);

                if (bmp == null)
                {
                    if (e.Item.Selected)//((e.State & ListViewItemStates.Focused) != 0)
                        e.Graphics.FillRectangle(BrushSelectBack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    else
                        e.Graphics.DrawRectangle(PenCommonBorder, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    e.Graphics.FillRectangle(BrushEmptyBack, e.Bounds.X + 5, e.Bounds.Y + 5, e.Bounds.Width - 10, e.Bounds.Height - 10);
                    return;
                }
                else
                {
                    if (e.Item.Selected)//((e.State & ListViewItemStates.Focused) != 0)
                        e.Graphics.FillRectangle(BrushSelectBack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    else if (ChangesInTiledataOrRadarCol[LandMaxIndex + i])
                        e.Graphics.FillRectangle(BrushChangeBack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    else if (patched)
                        e.Graphics.FillRectangle(BrushPatchBack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    else
                        e.Graphics.FillRectangle(BrushBackground, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

                    if (Options.ArtItemClip)
                    {
                        e.Graphics.DrawImage(bmp, e.Bounds.X + 1, e.Bounds.Y + 1,
                                    new Rectangle((bmp.Width-e.Bounds.Width)/2, 0, e.Bounds.Width-1, e.Bounds.Height-1),
                                    GraphicsUnit.Pixel);
                        //e.Graphics.DrawImage(bmp, e.Bounds.X + 1, e.Bounds.Y + 1,
                        //                     new Rectangle(0, 0, e.Bounds.Width - 1, e.Bounds.Height - 1),
                        //                     GraphicsUnit.Pixel);
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
                        e.Graphics.DrawRectangle(PenCommonBorder, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    else
                        e.Graphics.DrawRectangle(PenSelectBorder, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
                }
            }

            private void OnItemListResize(object sender, EventArgs e)
            {
                if (listViewItem.SelectedItems.Count > 0)
                {
                    listViewItem.SelectedItems[0].EnsureVisible();
                    int i = (int)listViewItem.SelectedItems[0].Tag;
                }
            }

            private void ShowItemInfo(List<int> index)
            {
                if (index.Count == 0) {
                    graphiclabelItem.Text = "ID Тайла:";
                    namelabel.Text = "Название:";
                    return;
                }
                else if (index.Count == 1) {
                    graphiclabelItem.Text = String.Format("ID Тайла: 0x{0:X4} ({0:D5})", index[0]);
                    namelabel.Text = String.Format("Название: {0}", ClilocNames[index[0]] ?? "null");
                } else {
                    graphiclabelItem.Text = String.Format("Выбранно: {0} тайлов", index.Count);
                    namelabel.Text = String.Empty;
                }
                
                try
                {
                    if (index.Count == 1) {
                        Bitmap bit = Ultima.Art.GetStatic(index[0]);
                        Bitmap newbit = new Bitmap(pictureBoxItem.Size.Width, pictureBoxItem.Size.Height);
                        Graphics newgraph = Graphics.FromImage(newbit);
                        newgraph.Clear(Color.FromArgb(-1));
                        newgraph.DrawImage(bit, (pictureBoxItem.Size.Width - bit.Width) / 2, (pictureBoxItem.Size.Height - bit.Height) / 2);
                        pictureBoxItem.Image = newbit;
                    } else {
                        string text = " Предпросмотр невозможен\n(выбранно несколько тайлов)";
                        Bitmap bit = new Bitmap(pictureBoxItem.Size.Width, pictureBoxItem.Size.Height);
                        Graphics graph = Graphics.FromImage(bit);
                        graph.Clear(Color.FromArgb(-1));
                        graph.DrawString(text, Fonts.DefaultFont, Brushes.DarkGray, 95, (bit.Height - 24) / 2);
                        //graph.Flush();
                        //graph.Save();
                        pictureBoxItem.Image = bit;
                    }  
                } catch {
                    pictureBoxItem.Image = new Bitmap(pictureBoxItem.Width, pictureBoxItem.Height);
                }

                ChangingIndex = true;
                try
                {
                ItemData data = TileData.ItemTable[index[0]];
                string name = data.Name;
                int anim = (int)data.Animation;
                int weigh = (int)data.Weight;
                int quality = (int)data.Quality;
                int quantity = (int)data.Quantity;
                int hue = (int)data.Hue;
                int stackOff = (int)data.StackingOffset;
                int value = (int)data.Value;
                int heigth = (int)data.Height;
                int unk1 = (int)data.MiscData;
                int unk2 = (int)data.Unk2;
                int unk3 = (int)data.Unk3;
                TileFlag dif = TileFlag.None;
                TileFlag flags = data.Flags;

                for (int i = 1; i < index.Count; ++i)
                {
                    data = TileData.ItemTable[index[i]];
                    if (name != null && String.Compare(name, data.Name, false) != 0)
                      name = null;
                    if (anim >= 0 && anim != (int)data.Animation)
                      anim = -1;
                    if (weigh >= 0 && weigh != (int)data.Weight)
                      weigh = -1;
                    if (quality >= 0 && quality != (int)data.Quality)
                      quality = -1;
                    if (quantity >= 0 && quantity != (int)data.Quantity)
                      quantity = -1;
                    if (hue >= 0 && hue != (int)data.Hue)
                      hue = -1;
                    if (stackOff >= 0 && stackOff != (int)data.StackingOffset)
                      stackOff = -1;
                    if (value >= 0 && value != (int)data.Value)
                      value = -1;
                    if (heigth >= 0 && heigth != (int)data.Height)
                      heigth = -1;
                    if (unk1 >= 0 && unk1 != (int)data.MiscData)
                      unk1 = -1;
                    if (unk2 >= 0 && unk2 != (int)data.Unk2)
                      unk2 = -1;
                    if (unk3 >= 0 && unk3 != (int)data.Unk3)
                      unk3 = -1;
                    dif |= (flags ^ data.Flags);
                }
                
                textBoxName.Text = name ?? String.Empty;
                textBoxAnim.Text = anim >= 0 ? anim.ToString() : String.Empty;
                textBoxWeight.Text = weigh >= 0 ? weigh.ToString() : String.Empty;
                textBoxQuality.Text = quality >= 0 ? quality.ToString() : String.Empty;
                textBoxQuantity.Text = quantity >= 0 ? quantity.ToString() : String.Empty;
                textBoxHue.Text = hue >= 0 ? hue.ToString() : String.Empty;
                textBoxStackOff.Text = stackOff >= 0 ? stackOff.ToString() : String.Empty;
                textBoxValue.Text = value >= 0 ? value.ToString() : String.Empty;
                textBoxHeigth.Text = heigth >= 0 ? heigth.ToString() : String.Empty;
                textBoxUnk1.Text = unk1 >= 0 ? unk1.ToString() : String.Empty;
                textBoxUnk2.Text = unk2 >= 0 ? unk2.ToString() : String.Empty;
                textBoxUnk3.Text = unk3 >= 0 ? unk3.ToString() : String.Empty;
                Array EnumValues = System.Enum.GetValues(typeof(TileFlag));
                int lbid = 0;
                for (int i = 1; i < EnumValues.Length; ++i)
                {
                    if (EnumNames[i].StartsWith("UnUsed"))
                        continue;
                    if ((dif & (TileFlag)EnumValues.GetValue(i)) != 0)
                        checkedListBox1.SetItemCheckState(lbid, CheckState.Indeterminate);
                    else if ((flags & (TileFlag)EnumValues.GetValue(i)) != 0)
                        checkedListBox1.SetItemCheckState(lbid, CheckState.Checked);
                    else
                        checkedListBox1.SetItemCheckState(lbid, CheckState.Unchecked);
                    ++lbid;
                }
                } catch {;}
                ChangingIndex = false;
            }

            private List<int> listViewItemSelected = new List<int>();

            private void listViewItem_SelectedIndexChanged(object sender, EventArgs e)
            { 
                if (EditTileDataToolStripMenuItem.Checked)
                    foreach (int item in listViewItemSelected)
                        SaveChangesInTiledataItems(item);   // Сохранение тайлдаты для выделенного элемента

                if (EditRadarColToolStripMenuItem.Checked && pictureBoxItemColor.Image == null)
                    foreach (int item in listViewItemSelected)
                        SaveChangesInRadarcolItems(item);   // Сохранение радаркола для выделенного элемента

                int index = -1;
                if (listViewItem.SelectedItems.Count > listViewItemSelected.Count)
                {
                    for(int i = listViewItem.SelectedItems.Count - 1; i >= 0; --i)
                    {
                        int tag = (int)listViewItem.SelectedItems[i].Tag;
                        if (!listViewItemSelected.Contains(tag))
                        {
                            index = tag;
                            listViewItemSelected.Add(index);
                            break;
                        }
                    }
                }
                else if (listViewItem.SelectedItems.Count < listViewItemSelected.Count)
                {
                    List<int> itemSelected = new List<int>(listViewItemSelected);
                    for (int i = listViewItem.SelectedItems.Count - 1; i >= 0; --i)
                    {
                        int tag = (int)listViewItem.SelectedItems[i].Tag;
                        if (itemSelected.Contains(tag))
                            itemSelected.Remove(tag);
                    }
                    if (itemSelected.Count > 0)
                    {
                        index = itemSelected[0];
                        listViewItemSelected.Remove(index);
                    }
                }

                if (listViewItemSelected.Count != 0)
                {
                    ShowItemInfo(listViewItemSelected);     // Обновление тайлдаты для выбранных элементов
                    ShowItemRadarCol(listViewItemSelected); // Обновление радаркол для выбранных элементов
                }

                //string str = String.Empty;
                //for(int i = 0; i < listViewItem.SelectedItems.Count; ++i)
                //    str += String.Format("{0}, ", (int)listViewItem.SelectedItems[i].Tag);
                //str += "\n";
                //for (int i = 0; i < listViewItemSelected.Count; ++i)
                //    str += String.Format("{0}, ", listViewItemSelected[i]);
                //MessageBox.Show(str);
            }

        #endregion

        #region LandShow

            private static bool Loaded_LandList = false;
            
            private void ReloadLands()
            {
                splitContainer6.Panel1Collapsed = true;

                Cursor.Current = Cursors.WaitCursor;
                Options.LoadedUltimaClass["TileData"] = true;
                Options.LoadedUltimaClass["Art"] = true;
                Options.LoadedUltimaClass["Texture"] = true;

                List<ListViewItem> itemcache = new List<ListViewItem>();

                listViewLand.BeginUpdate();
                listViewLand.Clear();
                
                for (int i = 0; i < LandMaxIndex; ++i)
                {
                    if (Art.IsValidLand(i))
                    {
                        ListViewItem item = new ListViewItem(i.ToString(), 0);
                        item.Name = i.ToString();
                        item.Tag = i;
                        itemcache.Add(item);
                    }
                }

                listViewLand.Items.AddRange(itemcache.ToArray());

                //if (Files.UseHashFile)
                //    MakeHashFile();

                listViewLand.EndUpdate();

                //Loaded = true;

                Loaded_LandList = true;
                Cursor.Current = Cursors.Default;
            }

            private void OnEnterLandTab()
            {
                if (!Loaded_LandList)
                    ReloadLands();
            }

            private void OnLeaveLandTab()
            {
            }


            private void drawland(object sender, DrawListViewItemEventArgs e)
            {
                int i = (int)e.Item.Tag;
                bool patched;
                Bitmap bmp = Art.GetLand(i, out patched);

                if (bmp == null)
                {
                    if (e.Item.Selected)//((e.State & ListViewItemStates.Focused) != 0)
                        e.Graphics.FillRectangle(BrushSelectBack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    else
                        e.Graphics.DrawRectangle(PenCommonBorder, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    e.Graphics.FillRectangle(BrushEmptyBack, e.Bounds.X + 5, e.Bounds.Y + 5, e.Bounds.Width - 10, e.Bounds.Height - 10);
                    return;
                }
                else
                {
                    if (e.Item.Selected)//((e.State & ListViewItemStates.Focused) != 0)
                        e.Graphics.FillRectangle(BrushSelectBack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    else if (ChangesInTiledataOrRadarCol[i])
                        e.Graphics.FillRectangle(BrushChangeBack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    else if (patched)
                        e.Graphics.FillRectangle(BrushPatchBack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    else
                        e.Graphics.FillRectangle(BrushBackground, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

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
                        e.Graphics.DrawRectangle(PenCommonBorder, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    else
                        e.Graphics.DrawRectangle(PenSelectBorder, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
                }
            }

            private void OnLandListResize(object sender, EventArgs e)
            {
                if (listViewLand.SelectedItems.Count > 0)
                {
                    listViewLand.SelectedItems[0].EnsureVisible();
                    int i = (int)listViewLand.SelectedItems[0].Tag;
                }
            }

            private void ShowLandInfo(List<int> index)
            {
                ChangingIndex = true;
                LandData data = TileData.LandTable[index[0]];

                string name = data.Name;
                int texture = (int)data.TextureID;
                TileFlag flags = data.Flags;
                TileFlag dif = TileFlag.None;

                try
                {
                    if (index.Count == 1)
                    {
                        Bitmap bit = Ultima.Art.GetLand(index[0]);
                        Bitmap newbit = new Bitmap(pictureBoxLand.Size.Width, pictureBoxLand.Size.Height);
                        Graphics newgraph = Graphics.FromImage(newbit);
                        newgraph.Clear(Color.FromArgb(-1));
                        newgraph.DrawImage(bit, (pictureBoxLand.Size.Width - bit.Width) / 2, (pictureBoxLand.Size.Height - bit.Height) / 2);
                        pictureBoxLand.Image = newbit;
                    }
                    else
                    {
                        string text = " Предпросмотр\n  невозможен";
                        Bitmap bit = new Bitmap(pictureBoxLand.Size.Width, pictureBoxLand.Size.Height);
                        Graphics graph = Graphics.FromImage(bit);
                        graph.Clear(Color.FromArgb(-1));
                        graph.DrawString(text, Fonts.DefaultFont, Brushes.DarkGray, 15, (bit.Height - 24) / 2);
                        pictureBoxLand.Image = bit;
                    }
                }
                catch
                {
                    pictureBoxLand.Image = new Bitmap(pictureBoxLand.Width, pictureBoxLand.Height);
                }

                try
                {
                    if (index.Count == 1)
                    {
						Bitmap bit = Art.IsPreAlpha ? (texture > 0 ? Art.GetStatic(texture) : new Bitmap(0,0)) 
													: Ultima.Textures.GetTexture(texture);
                        Bitmap newbit = new Bitmap(pictureBoxTexture.Size.Width, pictureBoxTexture.Size.Height);
                        Graphics newgraph = Graphics.FromImage(newbit);
                        newgraph.Clear(Color.FromArgb(-1));
                        newgraph.DrawImage(bit, (pictureBoxTexture.Size.Width - bit.Width) / 2, (pictureBoxTexture.Size.Height - bit.Height) / 2);
                        pictureBoxTexture.Image = newbit;
                    }
                    else
                    {
                        string text = " Предпросмотр невозможен\n(выбранно несколько тайлов)";
                        Bitmap bit = new Bitmap(pictureBoxTexture.Size.Width, pictureBoxTexture.Size.Height);
                        Graphics graph = Graphics.FromImage(bit);
                        graph.Clear(Color.FromArgb(-1));
                        graph.DrawString(text, Fonts.DefaultFont, Brushes.DarkGray, 15, (bit.Height - 24) / 2);
                        pictureBoxTexture.Image = bit;
                    }
                }
                catch
                {
                    pictureBoxTexture.Image = new Bitmap(pictureBoxTexture.Width, pictureBoxTexture.Height);
                }

                for (int i = 1; i < index.Count; ++i)
                {
                    data = TileData.LandTable[index[i]];
                    if (name != null && String.Compare(name, data.Name, false) != 0)
                        name = null;
                    if (texture >= 0 && texture != (int)data.TextureID)
                        texture = -1;
                    dif |= (flags ^ data.Flags);
                }

                textBoxNameLand.Text = name ?? String.Empty;
                textBoxTexID.Text = texture >= 0 ? texture.ToString() : String.Empty;
                if ((dif & TileFlag.Damaging) != 0)
                    checkedListBox2.SetItemCheckState(0, CheckState.Indeterminate);
                else if ((flags & TileFlag.Damaging) != 0)
                    checkedListBox2.SetItemCheckState(0, CheckState.Checked);
                else
                    checkedListBox2.SetItemCheckState(0, CheckState.Unchecked);
                if ((dif & TileFlag.Wet) != 0)
                    checkedListBox2.SetItemCheckState(1, CheckState.Indeterminate);
                else if ((flags & TileFlag.Wet) != 0)
                    checkedListBox2.SetItemCheckState(1, CheckState.Checked);
                else
                    checkedListBox2.SetItemCheckState(1, CheckState.Unchecked);
                if ((dif & TileFlag.Impassable) != 0)
                    checkedListBox2.SetItemCheckState(2, CheckState.Indeterminate);
                else if ((flags & TileFlag.Impassable) != 0)
                    checkedListBox2.SetItemCheckState(2, CheckState.Checked);
                else
                    checkedListBox2.SetItemCheckState(2, CheckState.Unchecked);
                if ((dif & TileFlag.Wall) != 0)
                    checkedListBox2.SetItemCheckState(3, CheckState.Indeterminate);
                else if ((flags & TileFlag.Wall) != 0)
                    checkedListBox2.SetItemCheckState(3, CheckState.Checked);
                else
                    checkedListBox2.SetItemCheckState(3, CheckState.Unchecked);
                if ((dif & TileFlag.Unknown3) != 0)
                    checkedListBox2.SetItemCheckState(4, CheckState.Indeterminate);
                else if ((flags & TileFlag.Unknown3) != 0)
                    checkedListBox2.SetItemCheckState(4, CheckState.Checked);
                else
                    checkedListBox2.SetItemCheckState(4, CheckState.Unchecked);

                ChangingIndex = false;

                if (index.Count == 0)
                {
                    graphiclabelLand.Text = "ID Тайла:";
                    namelabelLand.Text = "Название:";
                }
                else if (index.Count == 1)
                {
                    graphiclabelLand.Text = String.Format("ID Тайла: 0x{0:X4} ({0:D5})", index[0]);
                    namelabelLand.Text = String.Format("Название: {0}", "null");
                }
                else
                {
                    graphiclabelLand.Text = String.Format("Выбранно: {0} тайлов", index.Count);
                    namelabelLand.Text = String.Empty;
                }
            }

            private List<int> listViewLandSelected = new List<int>();

            private void listViewLand_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (EditTileDataToolStripMenuItem.Checked)
                    foreach (int land in listViewLandSelected)
                        SaveChangesInTiledataLands(land);   // Сохранение тайлдаты для выделенного элемента

                if (EditRadarColToolStripMenuItem.Checked && pictureBoxLandColor.Image == null)
                    foreach (int land in listViewLandSelected)
                        SaveChangesInRadarcolLands(land);   // Сохранение радаркола для выделенного элемента

                int index = -1;
                if (listViewLand.SelectedItems.Count > listViewLandSelected.Count)
                {
                    for(int i = listViewLand.SelectedItems.Count - 1; i >= 0; --i)
                    {
                        int tag = (int)listViewLand.SelectedItems[i].Tag;
                        if (!listViewLandSelected.Contains(tag))
                        {
                            index = tag;
                            listViewLandSelected.Add(index);
                            break;
                        }
                    }
                }
                else if (listViewLand.SelectedItems.Count < listViewLandSelected.Count)
                {
                    List<int> landSelected = new List<int>(listViewLandSelected);
                    for (int i = listViewLand.SelectedItems.Count - 1; i >= 0; --i)
                    {
                        int tag = (int)listViewLand.SelectedItems[i].Tag;
                        if (landSelected.Contains(tag))
                            landSelected.Remove(tag);
                    }
                    if (landSelected.Count > 0)
                    {
                        index = landSelected[0];
                        listViewLandSelected.Remove(index);
                    }
                }

                if (listViewLandSelected.Count != 0)
                {
                    ShowLandInfo(listViewLandSelected);     // Обновление тайлдаты для выбранных элементов
                    ShowLandRadarCol(listViewLandSelected); // Обновление радаркол для выбранных элементов
                }

                //string str = String.Empty;
                //for(int i = 0; i < listViewItem.SelectedItems.Count; ++i)
                //    str += String.Format("{0}, ", (int)listViewItem.SelectedItems[i].Tag);
                //str += "\n";
                //for (int i = 0; i < listViewItemSelected.Count; ++i)
                //    str += String.Format("{0}, ", listViewItemSelected[i]);
                //MessageBox.Show(str);
            }



        #endregion

        #region AnimData

            [Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public int CurrAnim
            {
                get { return m_currAnim; }
                set
                {
                    //if (!Loaded_AnimData)
                    //    return;
                    selData = (Animdata.Data)Animdata.AnimData[value];
                    if (m_currAnim != value)
                    {                       
                        listViewAnimFrames.BeginUpdate();
                        listViewAnimFrames.Clear();
                        if (selData != null)
                            for (int i = 0; i < selData.FrameCount; ++i)
                            {
                                int frame = value + selData.FrameData[i];

                                ListViewItem item = new ListViewItem(String.Format("{0} (0x{1:X4})", i, frame), 0);
                                item.Tag = frame;
                                listViewAnimFrames.Items.Add(item);
                            }
                        listViewAnimFrames.EndUpdate();   
                    }
                    m_currAnim = value;

                    if (selData != null)
                    {
                        if (m_animTimer == null)
                        {
                            int graphic = value;
                            if (curframe > -1)
                                graphic += selData.FrameData[curframe];
                            pictureBoxAnimation.Image = Art.GetStatic(graphic);
                        }
                        numericUpDownFrameDelay.Value = (decimal)((float)selData.FrameInterval / 10f);
                        numericUpDownStartDelay.Value = (decimal)((float)selData.FrameStart / 10f);
                    }
                }
            }
            private static bool Loaded_AnimData = false;
            private Animdata.Data selData;
            private int m_currAnim;
            private int curframe;
            private Timer m_animTimer;
            private int Timer_frame;

            private void OnEnterAnimTab()
            {
                if (!Loaded_AnimData)
                    ReloadAnims();
                if (m_animTimer != null)
                    m_animTimer.Start();
            }

            private void OnLeaveAnimTab()
            {
                if (m_animTimer != null)
                    m_animTimer.Stop();
            }

            private void ReloadAnims()
            {
                Cursor.Current = Cursors.WaitCursor;
                Options.LoadedUltimaClass["Animdata"] = true;
                Options.LoadedUltimaClass["TileData"] = true;
                Options.LoadedUltimaClass["Art"] = true;
                frames_TileSizeWidth  = listViewAnimFrames.TileSize.Width;
                frames_TileSizeHeight = listViewAnimFrames.TileSize.Height;
                splitContainer13.Panel2Collapsed = true;
                onEditAnimData(null, null);

                checkedListBox3.BeginUpdate();
                checkedListBox3.Items.Clear();
                EnumNames = System.Enum.GetNames(typeof(TileFlag));
                for (int i = 1; i < EnumNames.Length; ++i)
                {
                    if (EnumNames[i].StartsWith("UnUsed"))
                        continue;
                    checkedListBox3.Items.Add(EnumNames[i], false);
                }
                checkedListBox3.EndUpdate();

                treeViewAnim.BeginUpdate();
                treeViewAnim.Nodes.Clear();

                treeViewAnim.TreeViewNodeSorter = new AnimdataSorter();

                foreach (int id in Animdata.AnimData.Keys)
                {
                    Animdata.Data data = (Animdata.Data)Animdata.AnimData[id];
                    TreeNode node = new TreeNode();
                    node.Tag = id;
                    node.Name = id.ToString();
                    node.Text = String.Format("0x{0:X4} {1}", id, TileData.ItemTable[id].Name);

                    bool validanimation = false;
                    treeViewAnim.Nodes.Add(node);
                    for (int i = 0; i < data.FrameCount; ++i)
                    {
                        int frame = id + data.FrameData[i];
                        if (Art.IsValidStatic(frame))
                        {
                            TreeNode subnode = new TreeNode();
                            subnode.Text = String.Format("0x{0:X4} {1}", frame, TileData.ItemTable[frame].Name);
                            node.Nodes.Add(subnode);

                            if ((TileData.ItemTable[frame].Flags & TileFlag.Animation) == 0)
                                validanimation = true;
                        }
                        else
                            break;
                    }
                    if (!Art.IsValidStatic(id))
                        node.ForeColor = Color.Red;
                    else if ((TileData.ItemTable[id].Flags & TileFlag.Animation) == 0)
                        node.ForeColor = Color.Blue;
                    else if (validanimation)
                        node.ForeColor = Color.Green;
                    else
                        node.ForeColor = Color.Black;
                }

                treeViewAnim.EndUpdate();
                if (treeViewAnim.Nodes.Count > 0)
                    treeViewAnim.SelectedNode = treeViewAnim.Nodes[0];
                onAfterAnimNodeSelect(null, null);
                if ((StartStopAnimation.Checked && m_animTimer == null) || (!StartStopAnimation.Checked && m_animTimer != null))
                    onClickStartStopAnimation(null, null);
                    //StartStopAnimation.Checked = false;

                if (!Loaded_AnimData)
                {
                    FiddlerControls.Events.FilePathChangeEvent += new FiddlerControls.Events.FilePathChangeHandler(OnFilePathChangeEvent);
                    listViewAnimFrames.MouseWheel += new MouseEventHandler(onFrameListMouseWheel);
                }
                Loaded_AnimData = true;
                Cursor.Current = Cursors.Default;
            }

            private void OnClickSaveAnimData(object sender, EventArgs e)
            {
                Cursor.Current = Cursors.WaitCursor;
                Animdata.Save(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Mul Files"));
                Cursor.Current = Cursors.Default;

                MessageBox.Show(String.Format("AnimData сохраненна в {0}", Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Mul Files")),
                    "Сохраненно", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                Options.ChangedUltimaClass["Animdata"] = false;
            }

            private void onEditAnimData(object sender, EventArgs e)
            {
                bool enabled = EditAnimDataToolStripMenuItem.Checked;
                //contextMenuStrip3.Enabled = enabled;
                addAnimToolStripMenuItem.Enabled = enabled;
                fixAnimToolStripMenuItem.Enabled = (enabled && treeViewAnim.SelectedNode.Parent == null && treeViewAnim.SelectedNode.ForeColor != Color.Black); 
                removeAnimToolStripMenuItem.Enabled = enabled; 
                //contextMenuStrip4.Enabled = enabled;
                moveFrameToolStripMenuItem.Enabled = enabled;
                addFrameToolStripMenuItem.Enabled = enabled;
                removeFramesToolStripMenuItem.Enabled = enabled;
                removeAllFramesToolStripMenuItem.Enabled = enabled;
            }

            // Добавление, удаление, исправление анимации
        
            private void AddAnim(int index)
            {
                if (Animdata.GetAnimData(index) == null)
                {
                    Animdata.AnimData[index] = new Animdata.Data(new sbyte[64], 0, 1, 0, 0);
                    TreeNode node = new TreeNode();
                    node.Tag = index;
                    node.Text = String.Format("0x{0:X4} {1}", index, TileData.ItemTable[index].Name);
                    if ((TileData.ItemTable[index].Flags & TileFlag.Animation) == 0)
                        node.ForeColor = Color.Blue;
                    treeViewAnim.Nodes.Add(node);
                    TreeNode subnode = new TreeNode();
                    subnode.Text = String.Format("0x{0:X4} {1}", index, TileData.ItemTable[index].Name);
                    node.Nodes.Add(subnode);
                    node.EnsureVisible();
                    treeViewAnim.SelectedNode = node;
                    Options.ChangedUltimaClass["Animdata"] = true;

                    tabcontrol.SelectedTab = tabPageAnim;
                }
            }

            private void onTextChangeAddAnim(object sender, EventArgs e)
            {
                int index;
                if (Utils.ConvertStringToInt(AddTextBox.Text, out index, 0, ItemMaxIndex))
                {
                    if (Animdata.GetAnimData(index) != null || !Art.IsValidStatic(index))
                        AddTextBox.ForeColor = Color.Red;
                    else
                        AddTextBox.ForeColor = Color.Black;
                }
                else
                    AddTextBox.ForeColor = Color.Red;
            }

            private void onKeyDownAddAnim(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    int index;
                    if (Utils.ConvertStringToInt(AddTextBox.Text, out index, 0, ItemMaxIndex))
                        AddAnim(index);
                }
            }

            private void onClickRemoveAnim(object sender, EventArgs e)
            {
                if (treeViewAnim.SelectedNode == null)
                    return;

                TileData.ItemTable[CurrAnim].Flags ^= TileFlag.Animation;
                Options.ChangedUltimaClass["TileData"] = true;

                Animdata.AnimData.Remove(CurrAnim);
                Options.ChangedUltimaClass["Animdata"] = true;
                treeViewAnim.SelectedNode.Remove();
            }

            private void FixAnim(TreeNode node)
            {
                int itemId = (int)node.Tag;
                Animdata.Data data = (Animdata.Data)Animdata.AnimData[itemId];
                if (data != null)
                {
                    bool nodefixed = false;
                    for (int i = 0; i < data.FrameCount; ++i)
                    {
                        int itemid = itemId + selData.FrameData[i];
                        if ((TileData.ItemTable[itemid].Flags & TileFlag.Animation) == 0)
                        {
                            TileData.ItemTable[itemid].Flags |= TileFlag.Animation;
                            nodefixed = true;
                        }
                    }
                    if (nodefixed)
                    {
                        Options.ChangedUltimaClass["TileData"] = true;
                        node.ForeColor = Color.Black;
                    }
                }
                if (!Art.IsValidStatic(itemId))
                {
                    TileData.ItemTable[itemId].Flags ^= TileFlag.Animation;
                    Options.ChangedUltimaClass["TileData"] = true;

                    Animdata.AnimData.Remove(itemId);
                    Options.ChangedUltimaClass["Animdata"] = true;
                    node.Remove();
                }
            }

            private void onClickFixAnim(object sender, EventArgs e)
            {
                if (treeViewAnim.SelectedNode == null)
                    return;

                FixAnim(treeViewAnim.SelectedNode);
            }

            private void onClickfixAllAnim(object sender, EventArgs e)
            {
                for(int i = treeViewAnim.Nodes.Count - 1; i >= 0; --i)
                    FixAnim(treeViewAnim.Nodes[i]);
            }

            // Навигация по списку анимации

            private void GoToAnimNode(int itemId)
            {
                int index = treeViewAnim.Nodes.IndexOfKey(itemId.ToString());
                if (index > -1)
                {
                    tabcontrol.SelectedTab = tabPageAnim;
                    treeViewAnim.SelectedNode = treeViewAnim.Nodes[index];
                    treeViewAnim.SelectedNode.EnsureVisible();
                }
                else
                {
                    MessageBox.Show(String.Format("В списке AnimData, нету анимации с ID = 0x{0:X4}", itemId), "Анимация", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            private void onGoToItem(object sender, EventArgs e)
            {
                if (treeViewAnim.SelectedNode != null)
                    GoToItemNode((int)treeViewAnim.SelectedNode.Tag);
            }

            private void onClickExpandAllAnim(object sender, EventArgs e)
            {
                treeViewAnim.ExpandAll();
            }

            private void onClickCollapseAllAnim(object sender, EventArgs e)
            {
                treeViewAnim.CollapseAll();
            }

            // Перемещение кадров

            private void MoveFrame(int src_FrameId, int trg_FrameId)
            {
                if (selData != null)
                {
                    trg_FrameId = Math.Max(0, Math.Min(trg_FrameId, selData.FrameCount - 1));
                    if (src_FrameId < 0 || src_FrameId >= selData.FrameCount)
                        throw new ArgumentOutOfRangeException();
                    if (src_FrameId == trg_FrameId)
                        return;

                    sbyte[] FrameData = new sbyte[64];
                    Array.Copy(selData.FrameData, FrameData, 64);

                    if(trg_FrameId > src_FrameId)
                        Array.Copy(FrameData, src_FrameId + 1, selData.FrameData, src_FrameId, trg_FrameId - src_FrameId);
                    else
                        Array.Copy(FrameData, trg_FrameId, selData.FrameData, trg_FrameId + 1, src_FrameId - trg_FrameId);
                    selData.FrameData[trg_FrameId] = FrameData[src_FrameId];

                    UpdateFrames();
                    listViewAnimFrames.Items[trg_FrameId].Selected = true;

                    Options.ChangedUltimaClass["Animdata"] = true;
                }
            }

            private void onMoveDownFrame(object sender, EventArgs e)
            {
                if (listViewAnimFrames.SelectedItems != null && listViewAnimFrames.SelectedItems.Count > 0)
                    MoveFrame(listViewAnimFrames.SelectedItems[0].Index, listViewAnimFrames.SelectedItems[0].Index - 1);
            }

            private void onMoveUpFrame(object sender, EventArgs e)
            {
                if (listViewAnimFrames.SelectedItems != null && listViewAnimFrames.SelectedItems.Count > 0)
                    MoveFrame(listViewAnimFrames.SelectedItems[0].Index, listViewAnimFrames.SelectedItems[0].Index + 1);
            }

            private void onMoveBeginFrame(object sender, EventArgs e)
            {
                if (listViewAnimFrames.SelectedItems != null && listViewAnimFrames.SelectedItems.Count > 0)
                    MoveFrame(listViewAnimFrames.SelectedItems[0].Index, 0);
            }

            private void onMoveEndFrame(object sender, EventArgs e)
            {
                if (listViewAnimFrames.SelectedItems != null && listViewAnimFrames.SelectedItems.Count > 0)
                    MoveFrame(listViewAnimFrames.SelectedItems[0].Index, 63);
            }

            private void onTextChangeMoveFrame(object sender, EventArgs e)
            {
                int index;
                if (listViewAnimFrames.SelectedItems != null && listViewAnimFrames.SelectedItems.Count > 0)
                    if (Utils.ConvertStringToInt(toolStripTextBoxMoveFrame.Text, out index, 0, 63))
                        toolStripTextBoxMoveFrame.ForeColor = Color.Black;
                    else
                        toolStripTextBoxMoveFrame.ForeColor = Color.Red;
                else
                    toolStripTextBoxMoveFrame.Text = String.Empty;
            }

            private void onKeyDownMoveFrame(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter && listViewAnimFrames.SelectedItems != null && listViewAnimFrames.SelectedItems.Count > 0)
                {
                    int index;
                    if (Utils.ConvertStringToInt(toolStripTextBoxMoveFrame.Text, out index, 0, 63))
                        MoveFrame(listViewAnimFrames.SelectedItems[0].Index, index);
                }
            }

            private void onFrameListMouseWheel(object sender, MouseEventArgs e)
            {
                if (EditAnimDataToolStripMenuItem.Checked && e.Delta != 0 && listViewAnimFrames.SelectedItems != null && listViewAnimFrames.SelectedItems.Count > 0)
                {
                    int delta = e.Delta / 120;
                    int index = listViewAnimFrames.SelectedItems[0].Index;
                    MoveFrame(index, index + delta);
                }
            }

            // Добавление кадров

            private void AddFrame(int itemId, int frameId = 63, bool update = true)
            {
                if (selData != null)
                {
                    int index = itemId - CurrAnim;
                    if ((itemId > ItemMaxIndex) || (itemId < 0) || (!Art.IsValidStatic(itemId)) || (index < sbyte.MinValue) || (index > sbyte.MaxValue))
                        throw new ArgumentOutOfRangeException();
                    frameId = Math.Max(0, Math.Min(frameId, selData.FrameCount));
                    if (frameId > 63 || selData.FrameCount > 63)
                        return;

                    sbyte[] FrameData = new sbyte[64];
                    Array.Copy(selData.FrameData, FrameData, 64);
                    Array.Copy(FrameData, frameId, selData.FrameData, frameId + 1, 63 - frameId);
                    selData.FrameData[frameId] = (sbyte)index;
                    selData.FrameCount++;

                    /*
                    ListViewItem item = new ListViewItem(String.Format("{0} (0x{1:X4})", frameId, itemId), 0);
                    item.Tag = itemId;
                    listViewAnimFrames.Items.Insert(frameId, item);

                    TreeNode subnode = new TreeNode();
                    subnode.Tag = selData.FrameCount - 1;
                    subnode.Text = String.Format("0x{0:X4} {1}", itemId, TileData.ItemTable[itemId].Name);
                    if (treeViewAnim.SelectedNode.Parent == null)
                        treeViewAnim.SelectedNode.Nodes.Insert(frameId, subnode);
                    else
                        treeViewAnim.SelectedNode.Parent.Nodes.Insert(frameId, subnode);
                    */

                    if (update)
                    {
                        UpdateFrames();
                        listViewAnimFrames.Items[frameId].Selected = true;
                    }
                    Options.ChangedUltimaClass["Animdata"] = true;
                }
            }

            private void onTextChangeAddFrame(object sender, EventArgs e)
            {
                int index;
                if (Utils.ConvertStringToInt(toolStripTextBoxAddFrame.Text, out index, 0, ItemMaxIndex))
                    if (index < CurrAnim + sbyte.MinValue || index > CurrAnim + sbyte.MaxValue || !Art.IsValidStatic(index))
                        toolStripTextBoxAddFrame.ForeColor = Color.Red;
                    else
                        toolStripTextBoxAddFrame.ForeColor = Color.Black;
                    
                else
                    toolStripTextBoxAddFrame.ForeColor = Color.Red;
            }

            private void onKeyDownAddFrame(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    int index;
                    if (Utils.ConvertStringToInt(toolStripTextBoxAddFrame.Text, out index, 0, ItemMaxIndex))
                    {
                        if (index < CurrAnim + sbyte.MinValue || index > CurrAnim + sbyte.MaxValue || !Art.IsValidStatic(index))
                            return;

                        AddFrame(index, (listViewAnimFrames.SelectedItems != null && listViewAnimFrames.SelectedItems.Count > 0)
                                        ? listViewAnimFrames.SelectedItems[0].Index : 63);
                    }                    
                }
            }

            private int addframes_selected, frames_TileSizeWidth, frames_TileSizeHeight;
            private List<Control> frames_ParentControls = null;
            private Control frames_ParentControl = null;

            private void onAddFrame(object sender, EventArgs e)
            {
                Cursor.Current = Cursors.WaitCursor;
                
                listViewAnimFrames.BeginUpdate();
                listViewAnimFrames.Clear();
                listViewAnimFrames.TileSize = new Size(Options.ArtItemSizeWidth, Options.ArtItemSizeHeight + 14);
                listViewAnimFrames.MultiSelect = true;

                splitContainer11.Panel1Collapsed = true;
                splitContainer12.Panel2Collapsed = true;
                splitContainer13.Panel2Collapsed = false;

                if (listViewAnimFrames.SelectedItems != null && listViewAnimFrames.SelectedItems.Count > 0)
                    addframes_selected = listViewAnimFrames.SelectedItems[0].Index - 1;
                else
                    addframes_selected = 63;
  
                frames_ParentControls = new List<Control>(this.ParentForm.Controls.Count);
                foreach (Control control in this.ParentForm.Controls)
                    if (control.Visible)
                    {
                        frames_ParentControls.Add(control);
                        control.Visible = false;
                    }
                frames_ParentControl = splitContainer13.Parent;
                splitContainer13.Parent = this.ParentForm;
                listViewAnimFrames.ContextMenuStrip = null;

                for (int itemId = Math.Max(0, CurrAnim + sbyte.MinValue); itemId < Math.Min(CurrAnim + sbyte.MaxValue, ItemMaxIndex); ++itemId)
                {
                    if (!Art.IsValidStatic(itemId))
                        continue;

                    ListViewItem item = new ListViewItem(String.Format("0x{0:X4}", itemId), 0);
                    item.Tag = itemId;
                    listViewAnimFrames.Items.Add(item);
                }

                listViewAnimFrames.EndUpdate();
                Cursor.Current = Cursors.Default;
            }

            private void onConfirmAddFrames(object sender, EventArgs e)
            {
                if (listViewAnimFrames.SelectedItems != null)
                    for (int i = 0; i < listViewAnimFrames.SelectedItems.Count; ++i)
                    {
                        int itemId = (int)listViewAnimFrames.SelectedItems[i].Tag;
                        AddFrame(itemId, ++addframes_selected, false);
                    }
                onCancelAddFrames(sender, e);
                listViewAnimFrames.Items[Math.Min(addframes_selected, listViewAnimFrames.Items.Count - 1)].Selected = true;
            }

            private void onCancelAddFrames(object sender, EventArgs e)
            {
                splitContainer13.Panel2Collapsed = true;
                splitContainer11.Panel1Collapsed = false;
                splitContainer12.Panel2Collapsed = false;
                listViewAnimFrames.TileSize = new Size(frames_TileSizeWidth, frames_TileSizeHeight);
                listViewAnimFrames.MultiSelect = true;

                foreach (Control control in frames_ParentControls)
                    control.Visible = true;
                frames_ParentControls = null;

                splitContainer13.Parent = frames_ParentControl;
                frames_ParentControl = null;
                listViewAnimFrames.ContextMenuStrip = contextMenuStrip4;
                
                UpdateFrames();
            }

            // Удаление кадров

            private void RemoveFrame(int frameId)
            {
                if (selData != null)
                {
                    if (frameId < 0 || frameId >= selData.FrameCount)
                        throw new ArgumentOutOfRangeException();

                    sbyte[] FrameData = new sbyte[64];
                    Array.Copy(selData.FrameData, FrameData, 64);
                    Array.Copy(FrameData, frameId + 1, selData.FrameData, frameId, 63 - frameId);
                    selData.FrameData[63] = 0;
                    selData.FrameCount--;
                    /*
                    listViewAnimFrames.Items[frameId].Remove();
                    treeViewAnim.SelectedNode.Nodes[frameId].Remove();
                    */
                    UpdateFrames();

                    Options.ChangedUltimaClass["Animdata"] = true;
                }
            }

            private void onRemoveFrame(object sender, EventArgs e)
            {
                if (listViewAnimFrames.SelectedItems != null && listViewAnimFrames.SelectedItems.Count > 0)
                    RemoveFrame(listViewAnimFrames.SelectedItems[0].Index);
            }

            private void onRemoveAllFrames(object sender, EventArgs e)
            {
                if (selData != null)
                    for (int i = selData.FrameCount - 1; i >= 0; --i)
                        RemoveFrame(i);
            }

            // Прорисовка и обновление элементов

            private void onClickAnimNode(object sender, TreeNodeMouseClickEventArgs e)
            {
                if (e.Button == MouseButtons.Right)
                    treeViewAnim.SelectedNode = e.Node;
            }

            private void onAfterAnimNodeSelect(object sender, TreeViewEventArgs e)
            {
                if (treeViewAnim.SelectedNode != null)
                {
                    if (treeViewAnim.SelectedNode.Parent == null)
                    {
                        curframe = -1;
                        CurrAnim = (int)treeViewAnim.SelectedNode.Tag;
                        groupBox8.Visible = false;
                    }
                    else
                    {
                        curframe = treeViewAnim.SelectedNode.Index;
                        CurrAnim = (int)treeViewAnim.SelectedNode.Parent.Tag;
                        if (!listViewAnimFrames.Items[curframe].Selected)
                            listViewAnimFrames.Items[curframe].Selected = true;
                    }
                    fixAnimToolStripMenuItem.Enabled = (EditAnimDataToolStripMenuItem.Checked && treeViewAnim.SelectedNode.Parent == null && treeViewAnim.SelectedNode.ForeColor != Color.Black);
                }
            }

            private void onFramesSelectedIndexChanged(object sender, EventArgs e)
            {
                if (listViewAnimFrames.SelectedItems == null || listViewAnimFrames.SelectedItems.Count <= 0)
                {
                    if (treeViewAnim.SelectedNode.Parent != null)
                        treeViewAnim.SelectedNode = treeViewAnim.SelectedNode.Parent;

                    groupBox8.Visible = false;
                }
                else
                {
                    if (treeViewAnim.SelectedNode.Parent != null && treeViewAnim.SelectedNode.Parent.IsExpanded)
                        treeViewAnim.SelectedNode = treeViewAnim.SelectedNode.Parent.Nodes[listViewAnimFrames.SelectedItems[0].Index];
                    else if (treeViewAnim.SelectedNode.Parent == null && treeViewAnim.SelectedNode.IsExpanded)
                        treeViewAnim.SelectedNode = treeViewAnim.SelectedNode.Nodes[listViewAnimFrames.SelectedItems[0].Index];

                    groupBox8.Visible = true;
                    TileFlag flags = TileData.ItemTable[(int)listViewAnimFrames.SelectedItems[0].Tag].Flags;
                    Array EnumValues = System.Enum.GetValues(typeof(TileFlag));
                    int lbid = 0;
                    for (int i = 1; i < EnumValues.Length; ++i)
                    {
                        if (EnumNames[i].StartsWith("UnUsed"))
                            continue;
                        if ((flags & (TileFlag)EnumValues.GetValue(i)) != 0)
                            checkedListBox3.SetItemCheckState(lbid, CheckState.Checked);
                        else
                            checkedListBox3.SetItemCheckState(lbid, CheckState.Unchecked);
                        ++lbid;
                    }
                }    
            }

            private void UpdateFrames()
            {
                treeViewAnim.BeginUpdate();
                listViewAnimFrames.BeginUpdate();

                TreeNode node = treeViewAnim.SelectedNode;
                node.Text = String.Format("0x{0:X4} {1}", CurrAnim, TileData.ItemTable[CurrAnim].Name);
                node.Nodes.Clear();
                listViewAnimFrames.Clear();

                for (int i = 0; i < selData.FrameCount; ++i)
                {
                    int itemId = CurrAnim + selData.FrameData[i];

                    TreeNode subnode = new TreeNode();
                    subnode.Text = String.Format("0x{0:X4} {1}", itemId, TileData.ItemTable[itemId].Name);
                    node.Nodes.Add(subnode);

                    ListViewItem item = new ListViewItem(String.Format("{0} (0x{1:X4})", i, itemId), 0);
                    item.Tag = itemId;
                    listViewAnimFrames.Items.Add(item);
                }

                listViewAnimFrames.EndUpdate();
                treeViewAnim.EndUpdate();
            }

            private void drawframe(object sender, DrawListViewItemEventArgs e)
            {
                int i = (int)e.Item.Tag;
                bool patched;
                Bitmap bmp = Art.GetStatic(i, out patched);
                
                if (bmp == null)
                {
                    if (e.Item.Selected)//((e.State & ListViewItemStates.Focused) != 0)
                        e.Graphics.FillRectangle(BrushSelectBack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height - 14);
                    else
                        e.Graphics.DrawRectangle(PenCommonBorder, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height - 13);
                    e.Graphics.FillRectangle(BrushEmptyBack, e.Bounds.X + 5, e.Bounds.Y + 5, e.Bounds.Width - 10, e.Bounds.Height - 10);
                    return;
                }
                else
                {
                    if (e.Item.Selected)//((e.State & ListViewItemStates.Focused) != 0)
                        e.Graphics.FillRectangle(BrushSelectBack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height - 13);
                    else if (ChangesInTiledataOrRadarCol[LandMaxIndex + i])
                        e.Graphics.FillRectangle(BrushChangeBack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height - 13);
                    else if (patched)
                        e.Graphics.FillRectangle(BrushPatchBack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height - 13);
                    else
                        e.Graphics.FillRectangle(BrushBackground, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height - 13);

                    if (frames_ParentControl == null ? frameClipToolStripMenuItem.Checked : Options.ArtItemClip)
                    {
                        e.Graphics.DrawImage(bmp, e.Bounds.X + 1, e.Bounds.Y + 1,
                                             new Rectangle(0, 0, e.Bounds.Width - 1, e.Bounds.Height - 14),
                                             GraphicsUnit.Pixel);
                    }
                    else
                    {
                        int width  = bmp.Width;
                        int height = bmp.Height;
                        int boundWidth  = e.Bounds.Width;
                        int boundHeight = e.Bounds.Height - 13;
                        if (width > boundWidth)
                        {
                            width  = boundWidth;
                            height = boundHeight * bmp.Height / bmp.Width;
                        }
                        if (height > boundHeight)
                        {
                            height = boundHeight;
                            width  = boundWidth * bmp.Width / bmp.Height;
                        }
                        e.Graphics.DrawImage(bmp,
                                             new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, width, height));
                    }

                    //e.Graphics.DrawImage(bmp, e.Bounds.X, e.Bounds.Y, width, height);
                    e.DrawText(TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter);
                    //e.Graphics.DrawRectangle(new Pen(Color.Gray), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

                    if (!e.Item.Selected)//((e.State & ListViewItemStates.Focused) == 0)
                        e.Graphics.DrawRectangle(PenCommonBorder, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height - 13);
                    else
                        e.Graphics.DrawRectangle(PenSelectBorder, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 14);
                }
            }

            // Превью анимации и настройка анимации

            private void m_animTimer_Tick(object sender, EventArgs e)
            {
                if (selData == null || selData.FrameCount == 0)
                {
                    pictureBoxAnimation.Image = null;
                    labelAnimation.Text = "Кадр: --/--\nВремя:   0.0\nПериод:   0.0";
                    return;
                }

                ++Timer_frame;
                if (Timer_frame >= selData.FrameCount)
                    Timer_frame = 0;
                pictureBoxAnimation.Image = Art.GetStatic(CurrAnim + selData.FrameData[Timer_frame]);

                int time = 100 * selData.FrameInterval + 1;
                time *= Timer_frame;
                time += 100 * selData.FrameStart + 1;
                string strtime = String.Format("{0:D3}.{1:D1}", time / 1000, (time % 1000) / 100);
                if (strtime[1] == '0') strtime = String.Format("  {0}", strtime.Substring(2));
                else if (strtime[0] == '0') strtime = String.Format(" {0}", strtime.Substring(1));

                time = 100 * selData.FrameInterval + 1;
                time *= selData.FrameCount - 1;
                time += 100 * selData.FrameStart + 1;
                string strdurt = String.Format("{0:D3}.{1:D1}", time / 1000, (time % 1000) / 100);
                if (strdurt[1] == '0') strdurt = String.Format("  {0}", strdurt.Substring(2));
                else if (strdurt[0] == '0') strdurt = String.Format(" {0}", strdurt.Substring(1));

                labelAnimation.Text = String.Format("Кадр: {0:D2}/{1:D2}\nВремя: {2}\nПериод: {3}", Timer_frame, selData.FrameCount - 1, strtime, strdurt);

                if (Timer_frame == selData.FrameCount - 1)
                    m_animTimer.Interval = 100 * selData.FrameStart + 1;
                else if (Timer_frame == 0)
                    m_animTimer.Interval = 100 * selData.FrameInterval + 1;
            }

            private void onValueChangedStartDelay(object sender, EventArgs e)
            {
                if (selData != null)
                {
                    selData.FrameStart = (byte)(10f * (float)numericUpDownStartDelay.Value);
                    Options.ChangedUltimaClass["Animdata"] = true;
                }
            }

            private void onValueChangedFrameDelay(object sender, EventArgs e)
            {
                if (selData != null)
                {
                    selData.FrameInterval = (byte)(10f * (float)numericUpDownFrameDelay.Value);
                    if (m_animTimer != null)
                        m_animTimer.Interval = 100 * selData.FrameInterval + 1;
                    Options.ChangedUltimaClass["Animdata"] = true;
                }
            }

            private void onClickStartStopAnimation(object sender, EventArgs e)
            {
                if (m_animTimer != null)
                {
                    if (m_animTimer.Enabled)
                        m_animTimer.Stop();

                    m_animTimer.Dispose();
                    m_animTimer = null;
                    Timer_frame = 0;
                }
                else
                {
                    m_animTimer = new Timer();
                    m_animTimer.Interval = (selData != null) ? 100 * selData.FrameInterval + 1 : 500;
                    m_animTimer.Tick += new EventHandler(m_animTimer_Tick);
                    Timer_frame = 0;
                    m_animTimer.Start();
                }
            }
           

        #endregion

        private void SelectTreeViewItem(int index)
        {
            try
            {
                Bitmap bit = Ultima.Art.GetStatic(index);
                Bitmap newbit = new Bitmap(pictureBoxItem.Size.Width, pictureBoxItem.Size.Height);
                Graphics newgraph = Graphics.FromImage(newbit);
                newgraph.Clear(Color.FromArgb(-1));
                newgraph.DrawImage(bit, (pictureBoxItem.Size.Width - bit.Width) / 2, (pictureBoxItem.Size.Height - bit.Height) / 2);
                pictureBoxItem.Image = newbit;
            }
            catch
            {
                pictureBoxItem.Image = new Bitmap(pictureBoxItem.Width, pictureBoxItem.Height);
            }
            ItemData data = TileData.ItemTable[index];
            ChangingIndex = true;
            textBoxName.Text = data.Name;
            textBoxAnim.Text = data.Animation.ToString();
            textBoxWeight.Text = data.Weight.ToString();
            textBoxQuality.Text = data.Quality.ToString();
            textBoxQuantity.Text = data.Quantity.ToString();
            textBoxHue.Text = data.Hue.ToString();
            textBoxStackOff.Text = data.StackingOffset.ToString();
            textBoxValue.Text = data.Value.ToString();
            textBoxHeigth.Text = data.Height.ToString();
            textBoxUnk1.Text = data.MiscData.ToString();
            textBoxUnk2.Text = data.Unk2.ToString();
            textBoxUnk3.Text = data.Unk3.ToString();
            Array EnumValues = System.Enum.GetValues(typeof(TileFlag));
            int lbid = 0;
            for (int i = 1; i < EnumValues.Length; ++i)
            {
                if (EnumNames[i].StartsWith("UnUsed"))
                    continue;
                if ((data.Flags & (TileFlag)EnumValues.GetValue(i)) != 0)
                    checkedListBox1.SetItemChecked(lbid, true);
                else
                    checkedListBox1.SetItemChecked(lbid, false);
                ++lbid;
            }
            ChangingIndex = false;

            graphiclabelItem.Text = String.Format("ArtID: 0x{0:X4} ({0:D5})", index);
        }

        private void graphiclabelItem_Click(object sender, EventArgs e)
        {
            if (listViewItemSelected.Count != 1)
                return;
            string[] substr = graphiclabelItem.Text.Split(' ');
            if (substr.Length < 3)
                return;
           
            Clipboard.SetDataObject(substr[2]);
        }

        private void graphiclabelItem_DoubleClick(object sender, EventArgs e)
        {
            if (listViewItemSelected.Count != 1)
                return;
            string[] substr = graphiclabelItem.Text.Split(' ');
            if (substr.Length < 3)
                return;

            Client.SendString(substr[2]);
        }

        private void onCopyItems(object sender, EventArgs e)
        {
            string text = String.Empty;
            foreach (int item in listViewItemSelected)
                text = String.Format("{0}\n<Item ID=\"0x{1:X4}\" />", text, item);
            Clipboard.SetDataObject(text);
        }

        private void listViewItem_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                onCopyItems(sender, e);
        }

        private void graphiclabelLand_Click(object sender, EventArgs e)
        {
            if (listViewLandSelected.Count != 1)
                return;
            string[] substr = graphiclabelLand.Text.Split(' ');
            if (substr.Length < 3)
                return;

            Clipboard.SetDataObject(substr[2]);
        }

        private void graphiclabelLand_DoubleClick(object sender, EventArgs e)
        {
            if (listViewLandSelected.Count != 1)
                return;
            string[] substr = graphiclabelLand.Text.Split(' ');
            if (substr.Length < 3)
                return;

            Client.SendString(substr[2]);
        }

        private void onCopyLands(object sender, EventArgs e)
        {
            string text = String.Empty;
            foreach (int land in listViewLandSelected)
                text = String.Format("{0}\n<Land ID=\"0x{1:X4}\" />", text, land);
            Clipboard.SetDataObject(text);
        }

        private void listViewLand_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                onCopyLands(sender, e);
        }

        private int ItemMaxIndex { get { return Ultima.Art.StaticLength; } }

        private int LandMaxIndex { get { return Ultima.Art.LandLength; } }

        #region Экспорт и импорт

        private void ItemsAsImage(ImageFormat format, List<int> items, bool hexnames = true)
        {
            string ext = String.Empty;
            if (format == ImageFormat.Bmp) ext = "bmp"; 
            else if (format == ImageFormat.Png) ext = "png";
            else if (format == ImageFormat.Gif) ext = "gif";
            else if (format == ImageFormat.Tiff) ext = "tiff";
            else if (format == ImageFormat.Jpeg) ext = "jpg";  

            if (items == null)          // Извлекаем все тайлы
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    dialog.Description = "Выберите папку";
                    dialog.ShowNewFolderButton = true;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        TextWriter cfg = null;
                        if (ItemsCreateXmlToolStripMenuItem.Checked)
                        {
                            cfg = new StreamWriter(new FileStream(Path.Combine(dialog.SelectedPath, "Items.xml"), FileMode.Create, FileAccess.Write, FileShare.None));
                            cfg.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<MassImport>");
                            cfg.Flush();
                        }

                        for (int index = 0; index < ItemMaxIndex; ++index)
                        {
                            string FileName = Path.Combine(dialog.SelectedPath, String.Format(hexnames ? "I0x{0:X4}.{1}" : "I{0:D5}.{1}", index, ext));
                            if (Art.IsValidStatic(index))
                            {
                                Bitmap bit = new Bitmap(Ultima.Art.GetStatic(index));
                                bit.Save(FileName, format);
                                bit.Dispose();
                            }
                            if (cfg != null)
                            {
                                cfg.WriteLine("<item index=\"{0}\" file=\"{1}\" remove=\"{2}\" />", index, Path.GetFileName(FileName), Art.IsValidStatic(index) ? "False" : "True");
                                cfg.Flush();
                            }
                        }

                        if (ItemsCreateXmlToolStripMenuItem.Checked)
                        {
                            cfg.WriteLine("</MassImport>");
                            cfg.Flush();
                            cfg.Dispose();
                        }
                        MessageBox.Show(String.Format("Все предметы были сохранены в {0}", dialog.SelectedPath), "Сохраненно", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                }
            }
            else if(items.Count > 0)    // Извлекаем выбранные тайлы
            {
                string folder = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Extracted\Items");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                foreach (int index in items)
                {
                    string FileName = Path.Combine(folder, String.Format(hexnames ? "I0x{0:X4}.{1}" : "I{0:D5}.{1}", index, ext));
                    if (Art.IsValidStatic(index))
                    {
                        Bitmap bit = new Bitmap(Ultima.Art.GetStatic(index));
                        bit.Save(FileName, format);
                        bit.Dispose();
                    }
                }

                MessageBox.Show(String.Format("Все выбранные предметы были сохранены.\nВсего было сохранено: {0} предмет(а/ов) в {1}", items.Count, folder), "Сохраненно", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            
        }

        private void LandsAsImage(ImageFormat format, List<int> lands, bool hexnames = true)
        {
            string ext = String.Empty;

            if (format == ImageFormat.Bmp) ext = "bmp"; 
            else if (format == ImageFormat.Png) ext = "png";
            else if (format == ImageFormat.Gif) ext = "gif";
            else if (format == ImageFormat.Tiff) ext = "tiff";
            else if (format == ImageFormat.Jpeg) ext = "jpg";  

            if (lands == null)          // Извлекаем все тайлы
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    dialog.Description = "Выберите папку";
                    dialog.ShowNewFolderButton = true;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        for (int index = 0; index < LandMaxIndex; ++index)
                        {
                            string FileName = Path.Combine(dialog.SelectedPath, String.Format(hexnames ? "L0x{0:X4}.{1}" : "L{0:D5}.{1}", index, ext));
                            if (Art.IsValidLand(index))
                            {
                                Bitmap bit = new Bitmap(Ultima.Art.GetLand(index));
                                bit.Save(FileName, format);
                                bit.Dispose();
                            }
                            FileName = Path.Combine(dialog.SelectedPath, String.Format(hexnames ? "T0x{0:X4}.{1}" : "T{0:D5}.{1}", index, ext));
                            if (Textures.TestTexture(index))
                            {
                                Bitmap bit = new Bitmap(Ultima.Textures.GetTexture(index));
                                bit.Save(FileName, format);
                                bit.Dispose();
                            }
                        }

                        MessageBox.Show(String.Format("Все тайлы и текстуры были сохранены в {0}", dialog.SelectedPath), "Сохраненно", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                }
            }
            else if(lands.Count > 0)    // Извлекаем выбранные тайлы
            {
                string folder1 = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Extracted\Lands");
                string folder2 = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Extracted\Textures");
                if (!Directory.Exists(folder1))
                    Directory.CreateDirectory(folder1);
                if (!Directory.Exists(folder2))
                    Directory.CreateDirectory(folder2);
                foreach (int index in lands)
                {
                    string FileName = Path.Combine(folder1, String.Format(hexnames ? "L0x{0:X4}.{1}" : "L{0:D5}.{1}", index, ext));
                    if (Art.IsValidLand(index)) {
                        Bitmap bit = new Bitmap(Ultima.Art.GetLand(index));
                        bit.Save(FileName, format);
                        bit.Dispose();
                    }
                    int texid = TileData.LandTable[index].TextureID;
                    FileName = Path.Combine(folder2, String.Format(hexnames ? "T0x{0:X4}.{1}" : "T{0:D5}.{1}", index, ext));
                    if (Textures.TestTexture(texid)) {
                        Bitmap bit = new Bitmap(Ultima.Textures.GetTexture(texid));
                        bit.Save(FileName, format);
                        bit.Dispose();
                    }
                }
                
                MessageBox.Show(String.Format("Все тайлы были сохранены в {0}\nВсе текстуры были сохранены в {1}\n", folder1, folder2), "Сохраненно", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            
        }

        private void ItemsCreateXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemsCreateXmlToolStripMenuItem.Checked = !ItemsCreateXmlToolStripMenuItem.Checked;
        }

        private void ItemsAsBmpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemsAsImage(ImageFormat.Bmp, null);
        }

        private void ItemsAsPngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemsAsImage(ImageFormat.Png, null);
        }

        private void ItemsAsTifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemsAsImage(ImageFormat.Tiff, null);
        }

        private void ItemsAsGifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemsAsImage(ImageFormat.Gif, null);
        }

        private void ItemsAsJpgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemsAsImage(ImageFormat.Jpeg, null);
        }

        private void SelectedItemsAsBmpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemsAsImage(ImageFormat.Bmp, listViewItemSelected);
        }

        private void SelectedItemsAsPngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemsAsImage(ImageFormat.Png, listViewItemSelected);
        }

        private void SelectedItemsAsTifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemsAsImage(ImageFormat.Tiff, listViewItemSelected);
        }

        private void SelectedItemsAsGifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemsAsImage(ImageFormat.Gif, listViewItemSelected);
        }

        private void SelectedItemsAsJpgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemsAsImage(ImageFormat.Jpeg, listViewItemSelected);
        }
     
        private void LandsAsBmpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LandsAsImage(ImageFormat.Bmp, null);
        }

        private void LandsAsPngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LandsAsImage(ImageFormat.Png, null);
        }

        private void LandsAsTifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LandsAsImage(ImageFormat.Tiff, null);
        }

        private void LandsAsGifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LandsAsImage(ImageFormat.Gif, null);
        }

        private void LandsAsJpgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LandsAsImage(ImageFormat.Jpeg, null);
        }

        private void SelectedLandsAsBmpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LandsAsImage(ImageFormat.Bmp, listViewLandSelected);
        }

        private void SelectedLandsAsPngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LandsAsImage(ImageFormat.Png, listViewLandSelected);
        }

        private void SelectedLandsAsTifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LandsAsImage(ImageFormat.Tiff, listViewLandSelected);
        }

        private void SelectedLandsAsGifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LandsAsImage(ImageFormat.Gif, listViewLandSelected);
        }

        private void SelectedLandsAsJpgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LandsAsImage(ImageFormat.Jpeg, listViewLandSelected);
        }

        private void ItemsFromBmpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = "I:\\@@@@";
                dialog.Description = "Выберите папку c файлами в формате \"I0x????.bmp\" или \"I?????.bmp\".";
                dialog.ShowNewFolderButton = false;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string[] hexFiles = System.IO.Directory.GetFiles(dialog.SelectedPath, "I0x????.bmp", SearchOption.TopDirectoryOnly);
                    string[] decFiles = System.IO.Directory.GetFiles(dialog.SelectedPath, "I?????.bmp", SearchOption.TopDirectoryOnly);
                    MessageBox.Show(String.Format("Найдено Hex: {0}  Dec: {1}", hexFiles.Length, decFiles.Length), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                    Dictionary<ushort, string> files = new Dictionary<ushort, string>();//(ItemMaxIndex);

                    foreach (string file in decFiles)
                    {
                        ushort index;
                        if (!ushort.TryParse(Path.GetFileName(file).Substring(1, 5), out index))
                            continue;
                        files.Add(index, file);
                    }

                    foreach (string file in hexFiles)
                    {
                        ushort index;
                        if (!ushort.TryParse(Path.GetFileName(file).Substring(3, 4), System.Globalization.NumberStyles.HexNumber, null, out index))
                        {
                            //MessageBox.Show(String.Format("Неспарсировано!!\n{0}\n{1}", file, file.Substring(3, 4)), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            continue;
                        }
                        if (files.ContainsKey(index))
                            files.Remove(index);
                        files.Add(index, file);

                        //MessageBox.Show(String.Format("Добавлено: {0}  : {1}", index, file), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                    }

                    MessageBox.Show(String.Format("Начинаем конвертирование"), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                    for (ushort index = 0; index < ItemMaxIndex; ++index)
                    {
                        if (!files.ContainsKey(index))
                            continue;

                        string file;
                        if (!files.TryGetValue(index, out file))
                            continue;

                        Bitmap import = new Bitmap(file);
                        //Clipboard.SetDataObject(import, true);
                        //MessageBox.Show(String.Format("Айди {0} Файло {1}", index, file), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);



                        if (ItemsUseCkeyToolStripMenuItem.Checked)
                            import = FiddlerControls.Utils.CKeyFilter(import);
                        if (ItemsUseBcolToolStripMenuItem.Checked)
                            import = FiddlerControls.Utils.BColFilter(import, false);
                        if (ItemsUseTFixToolStripMenuItem.Checked)
                            import = FiddlerControls.Utils.CuttingRawBmpForTranslucent(import, false);

                        import = FiddlerControls.Utils.ConvertBmp(import);
                        //Clipboard.SetDataObject(import, true);
                        //MessageBox.Show(String.Format("Файло сконвертировано"), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                        Ultima.Art.ReplaceStatic(index, import);
                        //MessageBox.Show(String.Format("Файло заменено"), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        //import.Dispose();

                        //MessageBox.Show(String.Format("Выход"), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        //break;
                        //if (!direct)
                        {
                            FiddlerControls.Events.FireItemChangeEvent(this, index);
                            FiddlerControls.Options.ChangedUltimaClass["Art"] = true;
                        }
                    }

                    if (Ultima.Art.Modified)
                    {
                        //MessageBox.Show(String.Format("Сохраняемсо"), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        Ultima.Art.Save(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Mul Files"));
                        MessageBox.Show(String.Format("Предметы были импортированы и сохранены в {0}", Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Mul Files")), "Сохраннено", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    MessageBox.Show(String.Format("Событие выполнено"), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }

     
        }

        private void LandsFromBmpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = "I:\\@@@@";
                dialog.Description = "Выберите папку c файлами в формате \"T0x????.bmp\" и \"L0x????.bmp\" или \"T?????.bmp\" и \"L?????.bmp\".";
                dialog.ShowNewFolderButton = false;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string[] hexTextFiles = System.IO.Directory.GetFiles(dialog.SelectedPath, "T0x????.bmp", SearchOption.TopDirectoryOnly);
                    string[] decTextFiles = System.IO.Directory.GetFiles(dialog.SelectedPath, "T?????.bmp", SearchOption.TopDirectoryOnly);
                    string[] hexLandFiles = System.IO.Directory.GetFiles(dialog.SelectedPath, "L0x????.bmp", SearchOption.TopDirectoryOnly);
                    string[] decLandFiles = System.IO.Directory.GetFiles(dialog.SelectedPath, "L?????.bmp", SearchOption.TopDirectoryOnly);

                    MessageBox.Show(String.Format("Найдено Hex: {0}  Dec: {1}", hexTextFiles.Length + hexLandFiles.Length, decTextFiles.Length + decLandFiles.Length), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                    Dictionary<ushort, string> textFiles = new Dictionary<ushort, string>(); //(ItemMaxIndex);
                    Dictionary<ushort, string> landFiles = new Dictionary<ushort, string>();

                    foreach (string file in decTextFiles)
                    {
                        ushort index;
                        if (!ushort.TryParse(Path.GetFileName(file).Substring(1, 5), out index))
                            continue;
                        textFiles.Add(index, file);
                    }

                    foreach (string file in hexTextFiles)
                    {
                        ushort index;
                        if (!ushort.TryParse(Path.GetFileName(file).Substring(3, 4), System.Globalization.NumberStyles.HexNumber, null, out index))
                        {
                            MessageBox.Show(String.Format("Hex Err in: {0}", file), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            continue;
                        }

                        if (textFiles.ContainsKey(index))
                            textFiles.Remove(index);
                        textFiles.Add(index, file);
                    }

                    foreach (string file in decLandFiles)
                    {
                        ushort index;
                        if (!ushort.TryParse(Path.GetFileName(file).Substring(1, 5), out index))
                            continue;
                        landFiles.Add(index, file);
                    }

                    foreach (string file in hexLandFiles)
                    {
                        ushort index;
                        if (!ushort.TryParse(Path.GetFileName(file).Substring(3, 4), System.Globalization.NumberStyles.HexNumber, null, out index))
                        {
                            MessageBox.Show(String.Format("Hex Err in: {0}", file), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            continue;
                        }

                        if (landFiles.ContainsKey(index))
                            landFiles.Remove(index);
                        landFiles.Add(index, file);
                    }

                    MessageBox.Show(String.Format("Начинаем конвертирование"), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                    for (ushort index = 0; index < LandMaxIndex; ++index)
                    {
                        if (!textFiles.ContainsKey(index))
                            continue;

                        string file;
                        if (!textFiles.TryGetValue(index, out file))
                            continue;

                        Bitmap import = new Bitmap(file);

                        if (ItemsUseCkeyToolStripMenuItem.Checked)
                            import = FiddlerControls.Utils.CKeyFilter(import);

                        if (ItemsUseBcolToolStripMenuItem.Checked)
                            import = FiddlerControls.Utils.BColFilter(import, true);

                        import = FiddlerControls.Utils.ConvertBmp(import);

                        Ultima.Textures.Replace(index, import);

                        //MessageBox.Show(String.Format("Файло заменено"), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        //import.Dispose();

                        //MessageBox.Show(String.Format("Выход"), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        //break;
                        //if (!direct)
                        {
                            FiddlerControls.Events.FireTextureChangeEvent(this, index);
                            FiddlerControls.Options.ChangedUltimaClass["Texture"] = true;
                        }
                    }

                    for (ushort index = 0; index < LandMaxIndex; ++index)
                    {
                        if (!landFiles.ContainsKey(index))
                            continue;

                        string file;
                        if (!landFiles.TryGetValue(index, out file))
                            continue;

                        Bitmap import = new Bitmap(file);

                        if (ItemsUseCkeyToolStripMenuItem.Checked)
                            import = FiddlerControls.Utils.CKeyFilter(import);

                        if (ItemsUseBcolToolStripMenuItem.Checked)
                            import = FiddlerControls.Utils.BColFilter(import, true);

                        import = FiddlerControls.Utils.ConvertBmp(import);

                        Ultima.Art.ReplaceLand(index, import);

                        //MessageBox.Show(String.Format("Файло заменено"), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        //import.Dispose();

                        //MessageBox.Show(String.Format("Выход"), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        //break;
                        //if (!direct)
                        {
                            FiddlerControls.Events.FireLandTileChangeEvent(this, index);
                            FiddlerControls.Options.ChangedUltimaClass["Art"] = true;
                        }
                    }

                    if (Ultima.Art.Modified)
                    {
                        //MessageBox.Show(String.Format("Сохраняемсо"), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        Ultima.Art.Save(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Mul Files"));
                        Ultima.Textures.Save(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Mul Files"));
                        MessageBox.Show(String.Format("Предметы были импортированы и сохранены в {0}", Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Mul Files")), "Сохраннено", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }

                    MessageBox.Show(String.Format("Событие выполнено"), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
        }

        #endregion


        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }




        private void FixEmptyTileDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < LandMaxIndex; ++i)
                if (Ultima.Art.IsValidLand(i))
                {
                    if(!String.IsNullOrEmpty(Ultima.TileData.LandTable[i].Name) || Ultima.TileData.LandTable[i].Flags != Ultima.TileFlag.None)
                        continue;

                    Ultima.TileData.LandTable[i].Name = "NoName";
                    if(Ultima.Textures.TestTexture(i))
                        Ultima.TileData.LandTable[i].TextureID = (short)i;
                }

            if (!Loaded_ItemList)
                ItemHideLoad();
            for(int i = 0; i < ItemMaxIndex; ++i)
                if (!m_ItemHide[i] && Ultima.Art.IsValidStatic(i))
                {
                    if(!String.IsNullOrEmpty(Ultima.TileData.ItemTable[i].Name) || Ultima.TileData.ItemTable[i].Flags != Ultima.TileFlag.None )
                        continue;

                    Ultima.TileData.ItemTable[i].Name = "NoName";
                    Ultima.TileData.ItemTable[i].Weight = 255;
                }

            Ultima.TileData.SaveTileData(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Mul Files", "tiledata.mul"));
            MessageBox.Show(String.Format("tiledata.mul была сохранена в {0}", Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Mul Files")), "Сохраннено", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void FixEmptyRadarColToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listViewLand.SelectedItems.Clear();
            listViewItem.SelectedItems.Clear();
            
            //string str = "";
            for(int i = 0; i < LandMaxIndex; ++i)
                if (Ultima.Art.IsValidLand(i)) {
                    // Игнорируем ниже перечисленые тайлы, т.к. на оси они имеют цвет R8G8B8(16,16,16)
                    if (i == 0x0000 || i == 0x0001 || i == 0x0002 || i == 0x01AE || i == 0x01AF || i == 0x01B0 || i == 0x01B1 || i == 0x01B2 || i == 0x01B3 || i == 0x01B4 || i == 0x01B5
                                    || i == 0x0244 || i == 0x02BC || i == 0x02BD || i == 0x02BE || i == 0x02BF || i == 0x02C0 || i == 0x02C1 || i == 0x02C2 || i == 0x02C3 || i == 0x2F55
                                    || i == 0x3F90 || i == 0x1226 || (i >= 0x2710 && i <= 0x27CE) || (i >= 0x39E8 && i <= 0x3A08) )
                        continue;
                    short color = Ultima.RadarCol.GetLandColor(i);
                    if (color != 0x0842) //a0r2g2b2 // 0x0421) //a0r1g1b1 
                        continue;
                    //if (i < 0x992C) str += String.Format("i == 0x{0:X4} || ", i);
                    color = Utils.AverageCol(Ultima.Art.GetLand(i));
                    Ultima.RadarCol.SetLandColor(i, color);
                    ChangesInTiledataOrRadarCol[i] = true;
                } else Ultima.RadarCol.SetLandColor(i, 0x0842);
            
            if (!Loaded_ItemList)
                ItemHideLoad();
            for(int i = 0; i < ItemMaxIndex; ++i)
                if (!m_ItemHide[i] && Ultima.Art.IsValidStatic(i)) {
                    // Игнорируем ниже перечисленые тайлы, т.к. на оси они имеют цвет R8G8B8(8,8,8)
                    if (i == 0x0001 || i == 0x1B70 || i == 0x1ED9 || i == 0x1EE2 || i == 0x1EEB || i == 0x1EF4 || i == 0x2198
                                    || i == 0x2199 || i == 0x219A || i == 0x219B || i == 0x219C || i == 0x219D || i == 0x219E
                                    || i == 0x219F || i == 0x21A0 || i == 0x21A1 || i == 0x21A2 || i == 0x21A3 || i == 0x21A4
                                    || i == 0x2E44 || i == 0x30F4 || i == 0x30F5 || i == 0x37C4 || i == 0x3D84 || i == 0x4260
                                    || i == 0x4276 || i == 0x42AC || i == 0x4540 || i == 0x4542 || i == 0x46B6 || i == 0x46B7
                                    || i == 0x46B8 || i == 0x46B9 || i == 0x46BA || i == 0x46BB || i == 0x46BC || i == 0x46BD )
                        continue;
                    short color = Ultima.RadarCol.GetItemColor(i);
                    if (color != 0x0421) //a0r1g1b1 
                        continue;
                    //if(i < 0x992C ) str += String.Format("i == 0x{0:X4} || ", i);
                    color = Utils.AverageCol(Ultima.Art.GetStatic(i));
                    Ultima.RadarCol.SetItemColor(i, color);
                    ChangesInTiledataOrRadarCol[LandMaxIndex + i] = true;
                } else Ultima.RadarCol.SetItemColor(i, 0x0421);

            Options.ChangedUltimaClass["RadarCol"] = true;            
            Ultima.RadarCol.Save(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Mul Files", "radarcol.mul"));
            MessageBox.Show(String.Format("radarcol.mul был сохранен в {0}", Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Mul Files")), "Сохраннено", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void GenerateRadarColToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Внимание!! Данная операция полностью пересоздат файл radarcol.mul, \n удалив из него все данные. Вы уверены что хотите продолжить?\n\n"
                                + "Если вы хотите добавить цвета для новых тайлов настоятельно рекомендуется \nпользоваться функцией \"Автогенерация отсутсвующих цветов\".\n"
                                + "Вы все еще хотите продолжить?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            for(int i = 0; i < LandMaxIndex; ++i)                               //a0r2g2b2
                Ultima.RadarCol.SetLandColor(i, !Ultima.Art.IsValidLand(i)   ? (short)0x0842 : Utils.AverageCol(Ultima.Art.GetLand(i)));
            for(int i = 0; i < ItemMaxIndex; ++i)                               //a0r1g1b1 
                Ultima.RadarCol.SetItemColor(i, !Ultima.Art.IsValidStatic(i) ? (short)0x0421 : Utils.AverageCol(Ultima.Art.GetStatic(i)));
            Ultima.RadarCol.Save(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Mul Files", "radarcol.mul"));
            MessageBox.Show(String.Format("radarcol.mul был создан и сохранен в {0}", Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Mul Files")), "Сохраннено", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

        }

        private void SaveItemsDescriptionStripMenuItem_Click(object sender, EventArgs e)
        {
            TextWriter cfg = null;
            cfg = new StreamWriter(new FileStream(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Generated", "artdib.cfg"), FileMode.Create, FileAccess.Write, FileShare.None), Encoding.GetEncoding(1251));
            //xml.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Artdib>");
            cfg.WriteLine("# Файл в кодировке win-1251, содержит описание графики тайлов статики.");
            cfg.WriteLine("# Описание состоит из dx, dy - смещение от центра изображения до ЛВ угла,");
            cfg.WriteLine("# width, height - реальные размеры образа изображения (т.е. без учета отступов).");
            cfg.WriteLine("# Формат: ItemId  dx  dy  width  height");
            cfg.Flush();

            for (int index = 0; index < ItemMaxIndex; ++index) {
                int dx, dy, width, height;
                if (!Art.IsValidStatic(index)) {
                    dx = dy = 0; width = height = 0;
                } else {
                    var bit = new Bitmap(Ultima.Art.GetStatic(index));
                    dx = dy = 0xFFFF; width = height = 0;
                    unsafe {
                        var bd = bit.LockBits(new Rectangle(0, 0, bit.Width, bit.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                        var line = (uint*)bd.Scan0;
                        var delta = bd.Stride >> 2;
                        for (int Y = 0; Y < bit.Height; ++Y, line += delta) {
                            uint* cur = line;
                            for (int X = 0; X < bit.Width; ++X) {
                                byte a = (byte)((cur[X] & 0xFF000000) >> 24);
                                byte r = (byte)((cur[X] & 0x00FF0000) >> 16);
                                byte g = (byte)((cur[X] & 0x0000FF00) >> 8);
                                byte b = (byte)((cur[X] & 0x000000FF) >> 0);

                                if (r >= 8 || g >= 8 || b >= 8) {
                                    dx = Math.Min(dx, X); 
                                    dy = Math.Min(dy, Y);
                                    width  = Math.Max(width,  X);
                                    height = Math.Max(height, Y);
                                }
                            }
                        }
                        bit.UnlockBits(bd);
                    }
                    if (dx != 0xFFFF && dy != 0xFFFF) {
                        width -= dx; height -= dy;
                        dx += width/2;  dx *= -1;
                        dy += height/2; dy *= -1;
                    } else {
                        dx = dy = 0; width = height = 0;
                    }
                    bit.Dispose();
                }

                cfg.WriteLine("0x{0:X4}  {1,4}  {2,4}  {3,3}  {4,3}", index, dx, dy, width, height);
                //xml.WriteLine("\t<Item ID=\"0x{0:X4}\" dx=\"{1}\" dy=\"{2}\" width=\"{3}\" height=\"{4}\" />", index, dx, dy, width, height);
            }

            //xml.WriteLine("</Artdib>");
            cfg.Flush();
            cfg.Dispose();
            MessageBox.Show(String.Format("Все предметы были сохранены в {0}Generated", AppDomain.CurrentDomain.SetupInformation.ApplicationBase), "Сохраненно", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void MergeItemsStripMenuItem_Click(object sender, EventArgs e)
        {
            FileMerge.MergeItem merger = new FileMerge.MergeItem(this.ParentForm);
        }

        private void TextureCrossGenStripMenuItem_Click(object sender, EventArgs e)
        {
            new TextureCrossGen(this.ParentForm);
        }


        private void GoToItemNode(int itemId)
        {
            int index = listViewItem.Items.IndexOfKey(itemId.ToString());
            if (index > -1 && itemId >= 0 && itemId < ItemMaxIndex && Art.IsValidStatic(itemId))
            {
                tabcontrol.SelectedTab = tabPageItems;
                listViewItem.SelectedItems.Clear();
                listViewItem.Items[index].Selected = true;
                listViewItem.SelectedItems[0].EnsureVisible();
            }
            else
            {
                MessageBox.Show(String.Format("В списке Art.mul, нету тайла предмета с ID = 0x{0:X4}", itemId), "Предметы", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void onAddForAnim(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems != null && listViewItem.SelectedItems.Count == 1)
                AddAnim((int)listViewItem.SelectedItems[0].Tag);
        }

        private void onGoToAnim(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems != null && listViewItem.SelectedItems.Count == 1)
                GoToAnimNode((int)listViewItem.SelectedItems[0].Tag);
        }

        

    }
}
