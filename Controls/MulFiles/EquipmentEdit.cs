using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ultima;

namespace FiddlerControls.MulFiles
{
    public partial class EquipmentEdit : UserControl
    {
        public struct Race
        {
            public readonly string Name;
            public readonly int M_Body;
            public readonly int F_Body;
            public readonly int M_Gump;
            public readonly int F_Gump;
            public readonly int ColHue;
            private Race(string name, int m_body, int f_body, int hue, int m_gump, int f_gump) {
                Name = name;
                ColHue = hue;
                M_Body = Animations.IsAnimDefinied(m_body) ? m_body : 0;
                F_Body = Animations.IsAnimDefinied(m_body) ? f_body : 0;
                BodyConverter.Convert(ref m_body);
                BodyConverter.Convert(ref f_body); 
                M_Gump = Gumps.IsValidIndex(m_gump) ? m_gump : -1;
                F_Gump = Gumps.IsValidIndex(f_gump) ? f_gump : -1;
            }

            public static int Length { get { return instances.Length; } }
            private static readonly Race[] instances = new [] {
                new Race("Пустота",  0, 0, 0, -1, -1),
                new Race("Человек",  400,  401, 1052, 0x000C, 0x000D),
                new Race("Эльф",     605,  606, 1030, 0x000E, 0x000F), // 0x0766, 0x0765 ?
                new Race("Дите",    1008, 1009, 2426,     -1,     -1), // 0x0766, 0x0765 ?
                new Race("Гаргулья", 666,  667,    0, 0x029A, 0x0299),
            };

            public static Race GetInstance(int id) {
                return (id >= 0 && id < instances.Length) ? instances[id] : instances[0];
            }
        }

        private static EquipEntry[] enties;
        public class EquipEntry
        {
            public int Number       { get; private set; }
            public int Animation    { get; private set; } // Real Body value
            public int HueColor     { get; private set; } // Body hue value
            public int GumpMale     { get; private set; } //+50000
            public int GumpFemale   { get; private set; } //+60000

            public CheckState ItemWeapon      { get; private set; }
            public CheckState ItemArmor       { get; private set; }
            public CheckState ItemPartialHue  { get; private set; }
            public CheckState ItemLightSource { get; private set; }
            public string ItemName  { get; private set; }
            public int[] ArtTiles   { get; private set; }

            public EquipEntry(int id, int[] tiles)
            {
                Number    = id;
                Animation = Animations.Translate(id);
                HueColor  = BodyTable.m_Entries.ContainsKey(id) ? (BodyTable.m_Entries[id] as BodyTableEntry).NewHue : 0;
                GumpMale   = 50000 + id;
                GumpFemale = 60000 + id;
                if (id == 0 || !Gumps.IsValidIndex(GumpMale))   GumpMale   = -1;
                if (id == 0 || !Gumps.IsValidIndex(GumpFemale)) GumpFemale = -1;

                ArtTiles = tiles;
                ReadTileData();
            }

            private void ReadTileData()
            {
                bool invalid = Number == 0 || ArtTiles == null || ArtTiles.Length == 0;
                ItemName        = invalid ? null : TileData.ItemTable[ArtTiles[0]].Name;
                ItemWeapon      = invalid ? CheckState.Indeterminate : TileData.ItemTable[ArtTiles[0]].Flags.HasFlag(TileFlag.Weapon)      ? CheckState.Checked : CheckState.Unchecked;
                ItemArmor       = invalid ? CheckState.Indeterminate : TileData.ItemTable[ArtTiles[0]].Flags.HasFlag(TileFlag.Armor)       ? CheckState.Checked : CheckState.Unchecked;
                ItemPartialHue  = invalid ? CheckState.Indeterminate : TileData.ItemTable[ArtTiles[0]].Flags.HasFlag(TileFlag.PartialHue)  ? CheckState.Checked : CheckState.Unchecked;
                ItemLightSource = invalid ? CheckState.Indeterminate : TileData.ItemTable[ArtTiles[0]].Flags.HasFlag(TileFlag.LightSource) ? CheckState.Checked : CheckState.Unchecked;
                if (ArtTiles == null) return;
                for (var i = 1; i < ArtTiles.Length; ++i) {
                    if (ItemName != TileData.ItemTable[ArtTiles[i]].Name) ItemName = null;
                    if (ItemWeapon      != (TileData.ItemTable[ArtTiles[i]].Flags.HasFlag(TileFlag.Weapon)      ? CheckState.Checked : CheckState.Unchecked)) ItemWeapon      = CheckState.Indeterminate;
                    if (ItemArmor       != (TileData.ItemTable[ArtTiles[i]].Flags.HasFlag(TileFlag.Armor)       ? CheckState.Checked : CheckState.Unchecked)) ItemArmor       = CheckState.Indeterminate;
                    if (ItemPartialHue  != (TileData.ItemTable[ArtTiles[i]].Flags.HasFlag(TileFlag.PartialHue)  ? CheckState.Checked : CheckState.Unchecked)) ItemPartialHue  = CheckState.Indeterminate;
                    if (ItemLightSource != (TileData.ItemTable[ArtTiles[i]].Flags.HasFlag(TileFlag.LightSource) ? CheckState.Checked : CheckState.Unchecked)) ItemLightSource = CheckState.Indeterminate;
                }

            }

            public bool IsValid()
            {
                bool vald = Number > 0 && Animation > 0 && (GumpMale >= 0 || GumpFemale >= 0) && ArtTiles != null && ArtTiles.Length > 0;
                if (vald) {
                    var body = Animation;
                    var file = BodyConverter.Convert(ref body);
                    if (!Animations.IsAnimDefinied(Animation) || Animations.GetAnimLength(body, file) != 35)
                        vald = false;
                }
                return vald;
            }

            public void SetHueColor(int hue)
            {
                if (hue < 0 || hue == HueColor) return;
                HueColor = hue;
                if (Animation != Number || HueColor != 0)
                    BodyTable.m_Entries[Number] = new BodyTableEntry(Animation, Number, HueColor);
                else if (BodyTable.m_Entries.ContainsKey(Number)) BodyTable.m_Entries.Remove(Number);
            }

            public void SetAnimation(int anim)
            {
                if (anim < 0 || anim == Animation) return;
                Animation = anim;
                if (Animation != Number || HueColor != 0)
                    BodyTable.m_Entries[Number] = new BodyTableEntry(Animation, Number, HueColor);
                else if (BodyTable.m_Entries.ContainsKey(Number)) BodyTable.m_Entries.Remove(Number);

                Instance.treeViewBody_UpdateNode(Number);
            }

            public void AddItem(int tileId)
            {
                List<int> list;
                var anim = TileData.ItemTable[tileId].Animation;
                if (anim >= 0 && anim < enties.Length) {
                    list = new List<int>(enties[anim].ArtTiles);
                    if (list.Remove(tileId)) {
                        enties[anim].ArtTiles = list.ToArray();
                        enties[anim].ReadTileData();
                    }
                        
                }

                TileData.ItemTable[tileId].Animation = (short)this.Number;
                list = new List<int>(this.ArtTiles);
                list.Add(tileId);
                this.ArtTiles = list.ToArray();

                if (ItemName != null)
                    Ultima.TileData.ItemTable[tileId].Name = ItemName;
                if (ItemWeapon == CheckState.Unchecked)
                    Ultima.TileData.ItemTable[tileId].Flags &= ~TileFlag.Weapon;
                else if (ItemWeapon == CheckState.Checked)
                    Ultima.TileData.ItemTable[tileId].Flags |= TileFlag.Weapon;
                if (ItemArmor == CheckState.Unchecked)
                    Ultima.TileData.ItemTable[tileId].Flags &= ~TileFlag.Armor;
                else if (ItemArmor == CheckState.Checked)
                    Ultima.TileData.ItemTable[tileId].Flags |= TileFlag.Armor;
                if (ItemPartialHue == CheckState.Unchecked)
                    Ultima.TileData.ItemTable[tileId].Flags &= ~TileFlag.PartialHue;
                else if (ItemPartialHue == CheckState.Checked)
                    Ultima.TileData.ItemTable[tileId].Flags |= TileFlag.PartialHue;
                if (ItemLightSource == CheckState.Unchecked)
                    Ultima.TileData.ItemTable[tileId].Flags &= ~TileFlag.LightSource;
                else if (ItemLightSource == CheckState.Checked)
                    Ultima.TileData.ItemTable[tileId].Flags |= TileFlag.LightSource;
                this.ReadTileData();
                for (int i = Instance.listViewItem.Items.Count-1; i >= 0; --i)
                    if (tileId == (int)Instance.listViewItem.Items[i].Tag) {
                        var node = Instance.listViewItem.Items[i];
                        node.ToolTipText = String.Format("ID: 0x{0:X4} ({0,5}){2}{1}", tileId, Ultima.TileData.ItemTable[tileId].Name ?? String.Empty, Environment.NewLine);
                        break;
                    }

                Instance.treeViewBody_UpdateNode(Number);
            }

            public void RemoveItem(int tileId)
            {
                enties[0].AddItem(tileId);
            }

            public void ReplaceItem(int index, Bitmap tileBmp)
            {
                if (tileBmp == null) {
                    throw new ArgumentNullException();
                } else if (index < ArtTiles.Length) {
                    var bitmap = tileBmp.PixelFormat == PixelFormat.Format16bppArgb1555 ? tileBmp
                               : FiddlerControls.Utils.ConvertBmp(new Bitmap(tileBmp));
                    Ultima.Art.ReplaceStatic(ArtTiles[index], bitmap);
                    Ultima.RadarCol.SetItemColor(ArtTiles[index], Utils.AverageCol(bitmap));
                }
            }

            public void ReplaceItem(int index, int tileId)
            {
                ReplaceItem(index, tileId < 0 ? null : new Bitmap(Ultima.Art.GetStatic(tileId)));
            }
        
            private void SetGump(int id, Bitmap gump)
            {
                if (gump == null) {
                    for (int i = 0; i < Instance.listViewGump.Items.Count; ++i)
                        if (id == (int)Instance.listViewGump.Items[i].Tag) {
                            Ultima.Gumps.RemoveGump(id); 
                            Instance.listViewGump.BeginUpdate();
                            Instance.listViewGump.Items.RemoveAt(i);
                            Instance.listViewGump.EndUpdate();
                            if (id == GumpMale)   GumpMale   = -1;
                            if (id == GumpFemale) GumpFemale = -1;
                            Instance.treeViewBody_UpdateNode(Number);
                            return;
                        }
                } else {
                    int i, _id = id >= 60000 ? id - 10000 : id; 
                    for (i = Instance.listViewGump.Items.Count-1; i >= 0; --i) {
                        int _tag = (int)Instance.listViewGump.Items[i].Tag;
                        if (_tag >= 60000) _tag -= 10000;
                        if (_id >= _tag) {
                            if (_id > _tag) {
                                ++i;
                            } else if (id < 60000) {
                                if  (i > 0 && id == (int)Instance.listViewGump.Items[i-1].Tag)
                                    --i;
                            } else {
                                if (i < Instance.listViewGump.Items.Count && id != (int)Instance.listViewGump.Items[i].Tag)
                                    ++i;
                            }
                            break;
                        }
                    }
                    i = Math.Max(0, Math.Min(i, Instance.listViewGump.Items.Count));
                    var bitmap = gump.PixelFormat == PixelFormat.Format16bppArgb1555 ? gump
                                    : FiddlerControls.Utils.ConvertBmp(new Bitmap(gump));
                    Ultima.Gumps.ReplaceGump(id, bitmap);

                    if (i >= Instance.listViewGump.Items.Count || id != (int)Instance.listViewGump.Items[i].Tag) {
                        Instance.listViewGump.BeginUpdate();
                        var node = new ListViewItem();
                        node.Tag = id;
                        node.ToolTipText = String.Format("ID: 0x{0:X4} ({0,5}) [{3}]{2}{1}", id, ItemName ?? String.Empty, Environment.NewLine, id >= 60000 ? "жен" : "муж");
                        Instance.listViewGump.Items.Insert(i, node);
                        Instance.listViewGump.EndUpdate();

                        // Заставляем контрол прорисовать вставленный элемент не в конце списка
                        Instance.listViewGump.Alignment = ListViewAlignment.Default;
                        Instance.listViewGump.Alignment = ListViewAlignment.Top;
                        Instance.listViewGump.Update();
                        Instance.listViewGump.Invalidate();
                    }

                    if (id >= 60000) GumpFemale = id;
                    else GumpMale = id;
                    Instance.treeViewBody_UpdateNode(Number);
                    return;
                } 
            }

            private void SetGump(int id, int gump)
            {
                SetGump(id, gump < 0 ? null : Ultima.Gumps.GetGump(gump));
            }

            public void SetMaleGump(int gumpId)
            {
                SetGump(50000 + Number, gumpId);
            }

            public void SetMaleGump(Bitmap gumpBmp)
            {
                SetGump(50000 + Number, gumpBmp);
            }

            public void SetFemaleGump(int gumpId)
            {
                SetGump(60000 + Number, gumpId);
            }

            public void SetFemaleGump(Bitmap gumpBmp)
            {
                SetGump(60000 + Number, gumpBmp);
            }

            public void SetItemName(string value)
            {
                if (ArtTiles == null || value == null || ItemName == value) return;
                for (int i = 0; i < ArtTiles.Length; ++i) {
                    Ultima.TileData.ItemTable[ArtTiles[i]].Name = value ?? String.Empty;
                    for (int k = Instance.listViewItem.Items.Count-1; k >= 0; --k)
                        if (ArtTiles[i] == (int)Instance.listViewItem.Items[k].Tag) {
                            var node = Instance.listViewItem.Items[k];
                            node.ToolTipText = String.Format("ID: 0x{0:X4} ({0,5}){2}{1}", ArtTiles[i], ItemName ?? String.Empty, Environment.NewLine);
                            break;
                        }
                }
                ReadTileData();
                Instance.treeViewBody_UpdateNode(Number);
            }

            public void SetItemWeapon(bool value)
            {
                if (value ? ItemWeapon == CheckState.Checked : ItemWeapon == CheckState.Unchecked) return;
                SetFlag(TileFlag.Weapon, value);
            }

            public void SetItemArmor(bool value)
            {
                if (value ? ItemArmor == CheckState.Checked : ItemArmor == CheckState.Unchecked) return;
                SetFlag(TileFlag.Armor, value);
            }

            public void SetItemPartialHue(bool value)
            {
                if (value ? ItemPartialHue == CheckState.Checked : ItemPartialHue == CheckState.Unchecked) return;
                SetFlag(TileFlag.PartialHue, value);
            }

            public void SetItemLightSource(bool value)
            {
                if (value ? ItemLightSource == CheckState.Checked : ItemLightSource == CheckState.Unchecked) return;
                SetFlag(TileFlag.LightSource, value);
            }

            private void SetFlag(TileFlag flag, bool value)
            {
                if (ArtTiles == null) return;
                for (int i = 0; i < ArtTiles.Length; ++i) {
                    if (value)
                        Ultima.TileData.ItemTable[ArtTiles[i]].Flags |= flag;
                    else
                        Ultima.TileData.ItemTable[ArtTiles[i]].Flags &= ~flag;
                }
                ReadTileData();
            }

            public static EquipEntry FromTile(int tileId)
            {
                return enties[Ultima.TileData.ItemTable[tileId].Animation];
            }

            public static EquipEntry FromGump(int gumpId)
            {
                var body = gumpId - 50000;
                if (body >= 10000) body -= 10000;
                return enties[(body < 0 || body >= enties.Length) ? 0 : body] ?? enties[0];
            }
        }
        private EquipEntry SelectedEntry { get; set; }

        private static EquipmentEdit Instance;

        public EquipmentEdit()
        {
            Instance = this;
            InitializeComponent();
        }    
        private void onLoad(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            tbSelectedPath.Text = filesList.SelectedPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            cbBitmapType.SelectedIndex = 0;
            pbMGump.Image = new Bitmap(pbMGump.Width, pbMGump.Height);//new Bitmap(260, 237);
            pbFGump.Image = new Bitmap(pbMGump.Width, pbMGump.Height);//new Bitmap(260, 237);

            // Поиск тайлов предметов и создание списка
            listViewItem.BeginUpdate();
            listViewItem.Clear();
            var tiles = new List<int>(2048);
            for (int i = 0; i < TileData.ItemTable.Length; ++i)
                if (TileData.ItemTable[i].Wearable && Art.IsValidStatic(i)) {
                    tiles.Add(i);
                    var node = new ListViewItem();
                    node.Tag = i;
                    node.ToolTipText = String.Format("ID: 0x{0:X4} ({0,5}){2}{1}", i, Ultima.TileData.ItemTable[i].Name, Environment.NewLine);
                    listViewItem.Items.Add(node);
                }
            listViewItem.EndUpdate();
            listViewItem.Tag = new object[1];
            (listViewItem.Tag as object[])[0] = false;
            
            // Сортировка тайлов по анимациям
            int anim_min = 0, anim_max = 0;
            var anims = new Dictionary<int, List<int>>(1024);  
            foreach (var tile in tiles) {
                var animId = Math.Max(0, (int)TileData.ItemTable[tile].Animation);
                if (!anims.ContainsKey(animId))
                    anims.Add(animId, new List<int>(2));
                anims[animId].Add(tile);
                anim_max = Math.Max(anim_max, animId);
                anim_min = Math.Min(anim_min, animId);
            }

            // Построение списка экипировки
            treeViewBody.BeginUpdate();
            treeViewBody.Nodes.Clear();
            enties = new EquipEntry[Math.Max(2048, ++anim_max)];
            Array.Clear(enties, 0, enties.Length);
            //var pairs = from pair in anims orderby pair.Key ascending select pair;
            for (int i = 0; i < enties.Length; ++i) {
                enties[i] = anims.ContainsKey(i) ? new EquipEntry(i, anims[i].ToArray()) : i < 1000 ? null : new EquipEntry(i, new int[0]); 
                var node = new TreeNode(String.Format("{0,4}", i));
                node.Tag = i;
                treeViewBody.Nodes.Add(node);
                treeViewBody_UpdateNode(i, false);
            }
            treeViewBody.EndUpdate();
            treeViewBody.Update();
            treeViewBody.Refresh();
            treeViewBody.Nodes[1000].EnsureVisible();
            numNumber.Maximum = enties.Length;
            SelectedEntry = enties[0];

            // Построение списка гампов
            listViewGump.BeginUpdate();
            listViewGump.Clear();
            for (int id = 50000; id < 60000; ++id) {
                if (!Ultima.Gumps.IsValidIndex(id)) continue;
                var node = new ListViewItem();
                node.Tag = id;
                node.ToolTipText = String.Format("ID: 0x{0:X4} ({0,5}) [муж]{2}{1}", id, EquipEntry.FromGump(id).ItemName ?? String.Empty, Environment.NewLine);
                listViewGump.Items.Add(node);

                if (!Ultima.Gumps.IsValidIndex(id+10000)) continue;
                node = new ListViewItem();
                node.Tag = id + 10000;
                node.ToolTipText = String.Format("ID: 0x{0:X4} ({0,5}) [жен]{2}{1}", id+10000, EquipEntry.FromGump(id+10000).ItemName ?? String.Empty, Environment.NewLine);
                listViewGump.Items.Add(node);
            }
            listViewGump.EndUpdate();
            listViewGump.Tag = new object[1];
            (listViewGump.Tag as object[])[0] = false;

            // Построение списка тел
            listViewBody.BeginUpdate();
            listViewBody.Clear();
            for (int id = 400; id < 941; ++id) {
                var body = Ultima.AnimationEdit.GetAnimation(3, id, 4, 1);
                if (body == null || body.Frames == null || body.Frames.Count < 1) continue;
                var node = new ListViewItem();
                node.Name = String.Format("{0,4}", 600+id);
                node.Tag = new[] { 4, 1, 0, 600+id };
                listViewBody.Items.Add(node);
            }
            listViewBody.EndUpdate();

            // Построение списка Цветов
            listViewBodyHue.BeginUpdate();
            listViewBodyHue.Clear();
            for (int id = 0; id < Ultima.Hues.List.Length; ++id) {
                var node = new ListViewItem();
                node.Name = String.Format("{0,4}", id);
                node.Tag = new[] { 4, 1, 0, id };
                listViewBodyHue.Items.Add(node);
            }
            listViewBodyHue.EndUpdate();

            // Инициалзация списка расс
            cbBody.BeginUpdate();
            cbBody.Items.Clear();
            for (int i = 0; i < Race.Length; ++i)
                cbBody.Items.Add(Race.GetInstance(i).Name);
            cbBody.EndUpdate();

            lvAnim.BeginUpdate(); 
            lvAnim.Clear();
            string[] directs = new string[]{"4 - Down", "5 - South", "6 - Left", "7 - West", "0 - Up"};
            string[] actions = new string[]{"Walk_01","WalkStaff_01","Run_01","RunStaff_01","Idle_01","Idle_01",
                                            "Fidget_Yawn_Stretch_01","CombatIdle1H_01","CombatIdle1H_01","AttackSlash1H_01",
                                            "AttackPierce1H_01","AttackBash1H_01","AttackBash2H_01","AttackSlash2H_01",
                                            "AttackPierce2H_01","CombatAdvance_1H_01","Spell1","Spell2","AttackBow_01",
                                            "AttackCrossbow_01","GetHit_Fr_Hi_01","Die_Hard_Fwd_01","Die_Hard_Back_01",
                                            "Horse_Walk_01","Horse_Run_01","Horse_Idle_01","Horse_Attack1H_SlashRight_01",
                                            "Horse_AttackBow_01","Horse_AttackCrossbow_01","Horse_Attack2H_SlashRight_01",
                                            "Block_Shield_Hard_01","Punch_Punch_Jab_01","Bow_Lesser_01","Salute_Armed1h_01",
                                            "Ingest_Eat_01"}; //human
            for (int a = 0; a < 35; ++a)
                for (int d = 0; d < 5; ++d) {
                var node = new ListViewItem();
                node.Tag = new []{a, d, 0};
                node.ToolTipText = String.Format("{4}{0}Action: {1,2}{0}Direction: {2}{0}Durarion: {3}", Environment.NewLine, a, directs[d], "-", actions[a]);
                lvAnim.Items.Add(node);
            }
            lvAnim.EndUpdate();

            cbBody.SelectedIndex = Race.Length > 1 ? 1 : 0;
        }

        private void tsFile_Save_Click(object sender, EventArgs e)
        {
            var folder = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Mul Files");

            #region Конвертирование старых мулов
            /*
            for(int i = 50000; i < 60000; ++i) {
                if (!Gumps.IsValidIndex(i)) continue;
                var name = TileData.ItemTable[EquipEntry.FromGump(i).Number].Name;
                Gumps.GetGump(i).Save(String.Format(@"{0}\gumps\G{1:D4} [муж] - {2}.bmp", folder, i-50000, name));
                Gumps.RemoveGump(i);

                if (!Gumps.IsValidIndex(i+10000)) continue;
                Gumps.GetGump(i+10000).Save(String.Format(@"{0}\gumps\G{1:D4} [жен] - {2}.bmp", folder, i-50000, name));
                Gumps.RemoveGump(i+10000);
            }
            for(int i = 60000; i < 70000; ++i) {
                if (!Gumps.IsValidIndex(i)) continue;
                Gumps.GetGump(i).Save(String.Format(@"{0}\gumps\G{1:D4} [жен] - {2}.bmp", folder, i-60000, String.Empty));
                Gumps.RemoveGump(i);
            }
            
            for(int i = 0; i < listViewItem.Items.Count; ++i) {
                var id = (int)listViewItem.Items[i].Tag;
                if ((id >= 0x0A0F && id <= 0x0A25) || (id >= 0x40FE && id <= 0x4101)) continue;
                Art.GetStatic(id).Save(String.Format(@"{0}\items\I{1:D4} [0x{2:X4}] - {3}.bmp", folder, TileData.ItemTable[id].Animation, id, TileData.ItemTable[id].Name));
                if (id < 0xD000) Art.ReplaceStatic(id, Utils.ConvertBmp(Bitmap.FromFile(@"C:\UltimaOnline\tools\Fiddler+\Output\_new (Новая версия)\upd\deleted.bmp") as Bitmap));
                else Art.RemoveStatic(id);
                TileData.ItemTable[id].Animation = TileData.ItemTable[id].Weight = TileData.ItemTable[id].Quality = TileData.ItemTable[id].Quantity = TileData.ItemTable[id].Hue =
                                                   TileData.ItemTable[id].StackingOffset = TileData.ItemTable[id].Value = 0;
                TileData.ItemTable[id].Height = 0; TileData.ItemTable[id].MiscData = TileData.ItemTable[id].Unk2 = TileData.ItemTable[id].Unk3 = 0;
                TileData.ItemTable[id].Name = String.Empty;
                TileData.ItemTable[id].Flags = TileFlag.None; 
                RadarCol.SetItemColor(id, 0);
            } 

            //for (int i = 0x6000; i < 0x67D0; ++i) {
            for (int i = 0x67D0; i < 0x7000; ++i) {
                Art.ReplaceStatic(i, Utils.ConvertBmp(Bitmap.FromFile(@"C:\UltimaOnline\tools\Fiddler+\Output\_new (Новая версия)\upd\empty.bmp") as Bitmap));
                TileData.ItemTable[i].Name = String.Empty;
                TileData.ItemTable[i].Flags = TileFlag.Wearable;
                TileData.ItemTable[i].Height = 1;
            }     

            Art.Save(String.Format(@"{0}\data", folder));
            //Gumps.Save(String.Format(@"{0}\data", folder));
            RadarCol.Save(String.Format(@"{0}\data\radarcol.mul", folder));
            TileData.SaveTileData(String.Format(@"{0}\data\tiledata.mul", folder));

            MessageBox.Show(
                String.Format("Следующие файлы: \"gumpidx.mul\", \"gumpart.mul\", \"artidx.mul\", \"art.mul\", \"radarcol.mul\", \"tiledata.mul\", \"body.def\", \"mobtypes.txt\" были сохраненны в \"{0}\"", folder),
                "Сохранено", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
            return;
            */
            #endregion
            //======================

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            Ultima.TileData.SaveTileData(Path.Combine(folder, "tiledata.mul"));
            Ultima.RadarCol.Save(Path.Combine(folder, "radarcol.mul"));
            Ultima.Files.UseHashFile = Ultima.Files.CacheData = false;
            Ultima.Gumps.Save(folder);
            Ultima.Art.Save(folder);
            Ultima.BodyTable.Save(Path.Combine(folder, "body.def"));

            var table = new Hashtable();
            var lines = File.ReadAllLines(Files.GetFilePath("mobtypes.txt"));
            foreach (var line in lines) {
                var tockens = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (tockens.Length < 3 || tockens[0].StartsWith("#"))
                    continue;
                try {
                    var anim = int.Parse(tockens[0]);
                    var type = tockens[1];
                    var flag = int.Parse(tockens[2]);
                    var text = line.IndexOf('#') > 0 ? line.Substring(line.IndexOf('#')) : "#";
                    table[anim] = new object[]{type, flag, text, line};
                } catch {;}
            }
            var cont = String.Empty;
            var invalid = new[] {400,401,402,403, 605,606,607,608, 666,674,694,695};
            for (int i = 0; i < 10000; ++i) {
                if (i < enties.Length && enties[i] != null && !invalid.Contains(i)) {
                    if (!enties[i].IsValid()) continue;
                    var flag = table.ContainsKey(i) ? (int)(table[i] as object[])[1] : 0;
                    var text = (enties[i].ItemName != null && enties[i].ItemName != String.Empty)
                        ? String.Format("# {0}", enties[i].ItemName) : table.ContainsKey(i) ? (table[i] as object[])[2] : String.Empty;
                    cont += String.Format("{0}{1}\tEQUIPMENT\t{3}{2}\t{4}{5}", i, i < 1000 ? "\t" : "", flag < 1000 ? "\t" : "", flag, text, Environment.NewLine);
                } else if (table.ContainsKey(i))
                    cont += (string)(table[i] as object[])[3] + Environment.NewLine;
            }
            File.WriteAllText(Path.Combine(folder, "mobtypes.txt"), cont);

            MessageBox.Show(
                String.Format("Следующие файлы: \"gumpidx.mul\", \"gumpart.mul\", \"artidx.mul\", \"art.mul\", \"radarcol.mul\", \"tiledata.mul\", \"body.def\", \"mobtypes.txt\" были сохраненны в \"{0}\"", folder),
                "Сохранено", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
            Options.ChangedUltimaClass["TileData"] = false;
        }

        private void tsFile_Export_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Данная операция потребует перезапуска приложения Fiddler+, что приведет к потери не сохраненных данных. Вы уверены что хотите продолжить?",
                    "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1) == DialogResult.No) return;

            string folder_in, folder_out = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Extracted", "Equip"); 
            using (var dialog = new FolderBrowserDialog()) {
                dialog.Description = "Выберите папку содержащую следующие файлы: \"gumpidx.mul\", \"gumpart.mul\", \"artidx.mul\", \"art.mul\", \"tiledata.mul\"";
                dialog.ShowNewFolderButton = false;
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                folder_in = dialog.SelectedPath;
            }

            this.Enabled = false;
            this.FindForm().Enabled = false;
            var back = new string[5];
            back[0] = Ultima.Files.MulPath["tiledata.mul"]; 
            back[1] = Ultima.Files.MulPath["artidx.mul"];   
            back[2] = Ultima.Files.MulPath["art.mul"];      
            back[3] = Ultima.Files.MulPath["gumpidx.mul"];  
            back[4] = Ultima.Files.MulPath["gumpart.mul"];  

            try {
                Ultima.Files.MulPath["tiledata.mul"] = Path.Combine(folder_in, "tiledata.mul");
                Ultima.Files.MulPath["artidx.mul"]   = Path.Combine(folder_in, "artidx.mul");
                Ultima.Files.MulPath["art.mul"]      = Path.Combine(folder_in, "art.mul");
                Ultima.Files.MulPath["gumpidx.mul"]  = Path.Combine(folder_in, "gumpidx.mul");
                Ultima.Files.MulPath["gumpart.mul"]  = Path.Combine(folder_in, "gumpart.mul");
                Ultima.Art.Reload();
                Ultima.TileData.Initialize();
                Ultima.Gumps.Reload();

                if (!Directory.Exists(folder_out)) Directory.CreateDirectory(folder_out);
                if (!Directory.Exists(String.Format(@"{0}\data", folder_out))) Directory.CreateDirectory(String.Format(@"{0}\data", folder_out));

                var error = new List<int>();
                var names = new string[10000]; 
                for(int i = 0; i < Math.Min(TileData.ItemTable.Length, Art.StaticLength); ++i) {
                    if (TileData.ItemTable[i].Animation <= 0 || TileData.ItemTable[i].Animation >= 10000 || !Art.IsValidStatic(i)) continue;
                    names[TileData.ItemTable[i].Animation] = TileData.ItemTable[i].Name ?? String.Empty;
                    names[TileData.ItemTable[i].Animation] = names[TileData.ItemTable[i].Animation].Replace('/', '$').Replace('\\', '$').Replace(':', '$').Replace('*', '$').Replace('?', '$').Replace('"', '$').Replace('<', '$').Replace('>', '$').Replace('|', '$');
                    var path = String.Format(@"{0}\I{1:D4} [0x{2:X4}] - {3}.bmp", folder_out, TileData.ItemTable[i].Animation, i, names[TileData.ItemTable[i].Animation]);
                    try {
                        Art.GetStatic(i).Save(path, ImageFormat.Bmp);
                        Art.ReplaceStatic(i, Utils.ConvertBmp(Bitmap.FromFile(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "Bitmaps", "tile_deleted.bmp")) as Bitmap));
                        TileData.ItemTable[i].Animation = TileData.ItemTable[i].Weight = TileData.ItemTable[i].Quality = TileData.ItemTable[i].Quantity = TileData.ItemTable[i].Hue =
                                                          TileData.ItemTable[i].StackingOffset = TileData.ItemTable[i].Value = 0;
                        TileData.ItemTable[i].Height = 0; TileData.ItemTable[i].MiscData = TileData.ItemTable[i].Unk2 = TileData.ItemTable[i].Unk3 = 0;
                        TileData.ItemTable[i].Name = String.Empty;
                        TileData.ItemTable[i].Flags = TileFlag.None;  
                    } catch { error.Add(i); }
                }

                for(int i = 50000; i < 60000; ++i) {
                    if (!Gumps.IsValidIndex(i)) continue;
                    var name = names[i-50000] ?? "NoName";
                    try {
                        Gumps.GetGump(i).Save(String.Format(@"{0}\G{1:D4} [муж] - {2}.bmp", folder_out, i-50000, name), ImageFormat.Bmp);
                        Gumps.RemoveGump(i);
                    } catch { error.Add(-i); }

                    if (!Gumps.IsValidIndex(i+10000)) continue;
                    try {
                        Gumps.GetGump(i+10000).Save(String.Format(@"{0}\G{1:D4} [жен] - {2}.bmp", folder_out, i-50000, name), ImageFormat.Bmp);
                        Gumps.RemoveGump(i+10000);
                    } catch { error.Add(-i-10000); }
                }
                for(int i = 60000; i < 70000; ++i) {
                    if (!Gumps.IsValidIndex(i)) continue;
                    try {
                        Gumps.GetGump(i).Save(String.Format(@"{0}\G{1:D4} [жен] - {2}.bmp", folder_out, i-60000, String.Empty), ImageFormat.Bmp);
                        Gumps.RemoveGump(i);
                    } catch { if (!error.Contains(-i)) error.Add(-i); }
                }

                var text = String.Empty;
                foreach (var i in error) {
                    if (i >= 0) text = String.Format("{1}{0}0x{2:X4}    ( {2:D5} )  -  {3}", Environment.NewLine, text, i, TileData.ItemTable[i].Name ?? String.Empty);
                } File.WriteAllText(String.Format(@"{0}\data\invalid_tiles.log", folder_out), text);
                text = String.Empty;
                foreach (var i in error) {
                    if (i < 0) text = String.Format("{1}{0}0x{2:X4}    ( {2:D5} )  -  {3}", Environment.NewLine, text, -i, names[(-i-((i<=-60000)?60000:50000))] ?? String.Empty);
                } File.WriteAllText(String.Format(@"{0}\data\invalid_gumps.log", folder_out), text);

                Art.Save(String.Format(@"{0}\data", folder_out));
                Gumps.Save(String.Format(@"{0}\data", folder_out));
                TileData.SaveTileData(String.Format(@"{0}\data\tiledata.mul", folder_out));

                MessageBox.Show(
                    String.Format("Следующие файлы: \"gumpidx.mul\", \"gumpart.mul\", \"artidx.mul\", \"art.mul\", \"tiledata.mul\" были " +
                    "сохраненны в \"{0}\". А изображения тайлов и гампов экоспортированы в \"{1}\"{2}{2}Всего ошибок: {3}. Список индексов " +
                    "тайлов и гампов что не удалось извлечь сохраненны в списки \"{4}\" и \"{5}\"",
                    String.Format(@"{0}\data", folder_out), folder_out, Environment.NewLine, error.Count, 
                    String.Format(@"{0}\data\invalid_tiles.log", folder_out), String.Format(@"{0}\data\invalid_gumps.log", folder_out)),
                    "Сохранено", MessageBoxButtons.OK, MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);

            } catch (Exception exception) {
                MessageBox.Show(String.Format("Операция прервана из-за ошибки: \"{1}\"{0}{0}\"{2}\"", Environment.NewLine, exception.Message, exception.StackTrace),
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            } finally {             
                Ultima.Files.MulPath["tiledata.mul"] = back[0];
                Ultima.Files.MulPath["artidx.mul"]   = back[1];
                Ultima.Files.MulPath["art.mul"]      = back[2];
                Ultima.Files.MulPath["gumpidx.mul"]  = back[3];
                Ultima.Files.MulPath["gumpart.mul"]  = back[4];
                Application.Restart();
                //Ultima.Art.Reload();
                //Ultima.TileData.Initialize();
                //Ultima.Gumps.Reload();
            }
        }

        // ==========================================================================================================

        private void listViewItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                ((sender as ListView).Tag as object[])[0] = true; // Lock
        }

        private void listViewItem_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                ((sender as ListView).Tag as object[])[0] = false; // Unlock
        }

        private void listViewGump_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((bool)((listViewGump.Tag as object[])[0])) return;
            if (listViewGump.SelectedItems.Count <= 0) return;
            SelectEntry(EquipEntry.FromGump((int)listViewGump.SelectedItems[0].Tag));
        }

        private void listViewItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((bool)((listViewItem.Tag as object[])[0])) return;
            if (listViewItem.SelectedItems.Count <= 0) return;
            SelectEntry(EquipEntry.FromTile((int)listViewItem.SelectedItems[0].Tag)); 
        }
       
        private void cbBody_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectEntry(SelectedEntry);
        }


        private void SelectEntry(EquipEntry entry)
        {
            SelectedEntry = entry;
            if (entry == null || entry.Number == 0) {
                tbNumber.Text = tbName.Text = tbRealBody.Text = String.Empty;
                cbWeapon.CheckState = cbArmor.CheckState = cbPartialHue.CheckState = cbLightSource.CheckState = CheckState.Indeterminate;

                DrawGump(enties[0]);

                lvItems.Update();
                lvItems.Clear();
                lvItems.EndUpdate();

                lvAnim.Update();
                return;
            }

            treeViewBody.Nodes[entry.Number].EnsureVisible();

            listViewBodyHue.Items[entry.HueColor].Selected = true;
            listViewBodyHue.Items[entry.HueColor].EnsureVisible();
            for (int i = 0; i < listViewBody.Items.Count; ++i)
                if ((listViewBody.Items[i].Tag as int[])[3] == entry.Animation) {
                    listViewBody.Items[i].Selected = true;
                    listViewBody.Items[i].EnsureVisible();
                    break;
                }

            // Обновление инфы
            tbNumber.Text = String.Format("{0,4}", numNumber.Value = entry.Number);
            tbName.Text = entry.ItemName ?? String.Empty;
            tbRealBody.Text = String.Format("{0,4}", numNumber.Value = entry.Animation);
            tbRealBody.ForeColor = Animations.IsAnimDefinied(entry.Animation) ? Color.Blue : Color.DarkRed;
            cbWeapon.CheckState = entry.ItemWeapon;
            cbArmor.CheckState = entry.ItemArmor;
            cbPartialHue.CheckState = entry.ItemPartialHue;
            cbLightSource.CheckState = entry.ItemLightSource;

            // Обновление гампов
            DrawGump(entry);            

            // Обновление тайлов
            lvItems.Update();
            lvItems.Clear();
            for (int i = 0; i < entry.ArtTiles.Length; ++i) {
                var node = new ListViewItem();
                node.Name = String.Format("{0}. 0x{1:X4}", i+1, entry.ArtTiles[i]);
                node.Tag = entry.ArtTiles[i];
                lvItems.Items.Add(node);
            }      
            lvItems.EndUpdate();
            contextMenuItem_CopyTo_SetLength(entry.ArtTiles.Length);

            // Обновление анимации
            lvAnim.Update();
        }

        private void EquipmentEdit_Enter(object sender, EventArgs e)
        {
            tAnim.Enabled = true;
        }

        private void EquipmentEdit_Leave(object sender, EventArgs e)
        {
            tAnim.Enabled = false;
        }

        private void tAnim_Tick(object sender, EventArgs e)
        {
            if (!menuPlayAnimation.Checked) return;
            for (int i = 0; i < lvAnim.Items.Count; ++i) {
                var bact = (lvAnim.Items[i].Tag as int[])[0];
                var bdir = (lvAnim.Items[i].Tag as int[])[1];
                var bfrm = (lvAnim.Items[i].Tag as int[])[2];
                int bhue = 0;
                var aaa = Animations.GetAnimation(1000, bact, bdir, ref bhue, false, false);
                if (++bfrm >= aaa.Length)
                    bfrm = 0;
                (lvAnim.Items[i].Tag as int[])[2] = bfrm;
            } lvAnim.Invalidate();
        }

               
        #region Рендинг

        static Pen PenBlack = Pens.Black;   

        static Pen   PenAnimBack = Pens.Black;
        static Brush BrushItemBack = Brushes.Black;
        static Brush BrushItemBackInvlid = new SolidBrush(Color.FromArgb(36, 0, 0));
        static Brush BrushItemBackPatchd = Brushes.CornflowerBlue;
        static Brush BrushItemBackSelect = new SolidBrush(Color.FromArgb(32, 32, 32));
        static Pen   PenAnimBorder = Pens.Gold;// new Pen(Color.FromArgb(32, 32, 32));
        static Pen   PenItemBorder = new Pen(Color.FromArgb(32, 32, 32));
        static Font  FontItemName = new Font("Consolas", 9, FontStyle.Regular);

        private void DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            bool patched;
            int i = (int)e.Item.Tag;
            Bitmap bmp = Art.GetStatic(i, out patched);

            if (bmp == null) {
                if (e.Item.Selected)//((e.State & ListViewItemStates.Focused) != 0)
                    e.Graphics.FillRectangle(BrushItemBackSelect, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                else
                    e.Graphics.DrawRectangle(PenItemBorder, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                e.Graphics.FillRectangle(BrushItemBackInvlid, e.Bounds.X + 5, e.Bounds.Y + 5, e.Bounds.Width - 10, e.Bounds.Height - 10);
                return;
            } else {
                if (e.Item.Selected)//((e.State & ListViewItemStates.Focused) != 0)
                    e.Graphics.FillRectangle(BrushItemBackSelect, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                else if (patched)
                    e.Graphics.FillRectangle(BrushItemBackPatchd, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                else if (!EquipEntry.FromTile(i).IsValid())
                    e.Graphics.FillRectangle(BrushItemBackInvlid, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                else
                    e.Graphics.FillRectangle(BrushItemBack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);


                e.Graphics.DrawImage(bmp, e.Bounds.X + 1, e.Bounds.Y + 1,
                                    new Rectangle((bmp.Width-e.Bounds.Width)/2, (bmp.Height-e.Bounds.Height)/2, e.Bounds.Width-1, e.Bounds.Height-1),
                                    GraphicsUnit.Pixel);

                if (!e.Item.Selected)//((e.State & ListViewItemStates.Focused) == 0)
                    e.Graphics.DrawRectangle(PenItemBorder, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                else
                    e.Graphics.DrawRectangle(PenBlack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);

                if (!String.IsNullOrEmpty(e.Item.Name)) {
                    var trect = new Rectangle(e.Bounds.X, e.Bounds.Y+e.Bounds.Height-FontItemName.Height, e.Bounds.Width, FontItemName.Height);
                    var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;
                    TextRenderer.DrawText(e.Graphics, e.Item.Name, FontItemName, trect, Color.LightSlateGray, flags);
                }
            }
        }

        private void DrawAnim(object sender, DrawListViewItemEventArgs e)
        {
            var race = Race.GetInstance(cbBody.SelectedIndex);
            var body = new int[3];
            var bhue = new int[body.Length];
            var bact = (e.Item.Tag as int[])[0];
            var bdir = (e.Item.Tag as int[])[1];
            var bfrm = (e.Item.Tag as int[])[2];

            if (sender == listViewBody) {
                body[1] = (e.Item.Tag as int[])[3];
                e.Graphics.DrawRectangle(e.Item.Selected ? PenAnimBorder : PenAnimBack, e.Bounds.X-0, e.Bounds.Y-0, e.Bounds.Width+0, e.Bounds.Height+0);
                e.Graphics.DrawRectangle(e.Item.Selected ? PenAnimBorder : PenAnimBack, e.Bounds.X-1, e.Bounds.Y-1, e.Bounds.Width+2, e.Bounds.Height+2);
                e.Graphics.DrawRectangle(e.Item.Selected ? PenAnimBorder : PenAnimBack, e.Bounds.X-2, e.Bounds.Y-2, e.Bounds.Width+4, e.Bounds.Height+4);
            } else if (sender == listViewBodyHue) {
                body[2] = SelectedEntry.Animation;
                bhue[2] = (e.Item.Tag as int[])[3];
                if (SelectedEntry.ItemPartialHue == CheckState.Checked)
                    bhue[2] |= 0x8000;
                e.Graphics.DrawRectangle(e.Item.Selected ? PenAnimBorder : PenAnimBack, e.Bounds.X-0, e.Bounds.Y-0, e.Bounds.Width+0, e.Bounds.Height+0);
                e.Graphics.DrawRectangle(e.Item.Selected ? PenAnimBorder : PenAnimBack, e.Bounds.X-1, e.Bounds.Y-1, e.Bounds.Width+2, e.Bounds.Height+2);
                e.Graphics.DrawRectangle(e.Item.Selected ? PenAnimBorder : PenAnimBack, e.Bounds.X-2, e.Bounds.Y-2, e.Bounds.Width+4, e.Bounds.Height+4);
            } else {
                body[1] = menuUseFemaleBody.Checked ? race.F_Body : race.M_Body;
                bhue[1] = race.ColHue;
                if (body[1] > 0 && bact >= 23 && bact <= 29) {
                    body[0] = 1052;
                    bhue[0] = 0;
                }
                if (SelectedEntry != null && SelectedEntry.Number > 0) {
                    body[2] = SelectedEntry.Animation;
                    bhue[2] = SelectedEntry.HueColor;
                    if (SelectedEntry.ItemPartialHue == CheckState.Checked)
                        bhue[2] |= 0x8000;
                }
            }


            var fram = new Frame[body.Length];
            var posx = new int[body.Length];
            var posy = new int[body.Length];
            int minx = int.MaxValue, miny = int.MaxValue, maxx = 0, maxy = 0;

            for (int i = 0; i < body.Length; ++i) {
                if (body[i] <= 0) continue;
                var anim = Animations.GetAnimation(body[i], bact, bdir, ref bhue[i], true, false);
                if (anim == null || anim.Length <= bfrm) continue;
                var prev = anim[bfrm-1 >= 0 ? bfrm-1 : anim.Length-1]; 
                fram[i] = anim[bfrm];
                if (prev == null || fram[i] == null || prev.Bitmap == null || fram[i].Bitmap == null) continue;

                posx[i] = e.Bounds.X-2 + (e.Bounds.Width+4)/2    - fram[i].Center.X;
                posy[i] = e.Bounds.Y-2 + (e.Bounds.Height+4)*4/5 - fram[i].Center.Y - fram[i].Bitmap.Height;                
                var prx = e.Bounds.X-2 + (e.Bounds.Width+4)/2    - prev.Center.X;
                var pry = e.Bounds.Y-2 + (e.Bounds.Height+4)*4/5 - prev.Center.Y - prev.Bitmap.Height;
                minx = Math.Min(minx, Math.Min(posx[i], prx));
                miny = Math.Min(miny, Math.Min(posy[i], pry));
                maxx = Math.Max(maxx, Math.Max(posx[i]+fram[i].Bitmap.Width,  prx+prev.Bitmap.Width));
                maxy = Math.Max(maxy, Math.Max(posy[i]+fram[i].Bitmap.Height, pry+prev.Bitmap.Height));
            }

            e.Graphics.FillRectangle(BrushItemBack, minx, miny, maxx-minx, maxy-miny);
            for (int i = 0; i < fram.Length; ++i) {
                if (fram[i] == null || fram[i].Bitmap == null) continue;
                e.Graphics.DrawImage(fram[i].Bitmap, posx[i], posy[i], new Rectangle(0, 0, fram[i].Bitmap.Width, fram[i].Bitmap.Height), GraphicsUnit.Pixel);
            }
            if (!String.IsNullOrEmpty(e.Item.Name)) {
                var trect = new Rectangle(e.Bounds.X, e.Bounds.Y + e.Bounds.Height - FontItemName.Height, e.Bounds.Width, FontItemName.Height);
                var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;
                TextRenderer.DrawText(e.Graphics, e.Item.Name, FontItemName, trect, Color.LightSlateGray, flags);
            } 
        }

        private void DrawGump(object sender, DrawListViewItemEventArgs e)
        {
            bool patched;
            int i = (int)e.Item.Tag;
            var bmp = Ultima.Gumps.GetGump((int)e.Item.Tag, out patched); 
            if (bmp == null) return;

            if (e.Item.Selected)//((e.State & ListViewItemStates.Focused) != 0)
                e.Graphics.FillRectangle(BrushItemBackSelect, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            else if (patched)
                e.Graphics.FillRectangle(BrushItemBackPatchd, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            else if (!EquipEntry.FromGump(i).IsValid())
                e.Graphics.FillRectangle(BrushItemBackInvlid, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            else
                e.Graphics.FillRectangle(BrushItemBack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

            if (!e.Item.Selected)//((e.State & ListViewItemStates.Focused) == 0)
                e.Graphics.DrawRectangle(PenItemBorder, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            else
                e.Graphics.DrawRectangle(PenBlack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);

            e.Graphics.DrawImage(bmp, new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 2), 17f, 30f, 156f, 194f, GraphicsUnit.Pixel);

            if (!e.Item.Selected)//((e.State & ListViewItemStates.Focused) == 0)
                e.Graphics.DrawRectangle(PenItemBorder, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            else
                e.Graphics.DrawRectangle(PenBlack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
        }

        private void DrawGump(EquipEntry entry)
        {
            bool patched;
            var race = Race.GetInstance(cbBody.SelectedIndex);
            using (var graphpic = Graphics.FromImage(pbMGump.Image)) {
                graphpic.Clear(Color.Black);
                if (race.M_Gump > 0)
                    graphpic.DrawImage(new Bitmap(Gumps.GetGump(race.M_Gump, Ultima.Hues.GetHue(race.ColHue), true, out patched)), 0, 0);
                if (Gumps.IsValidIndex(entry.GumpMale) && entry.Number > 0) {
                    /*if (SelectedEntry.HueColor > 0)
                        graphpic.DrawImage(new Bitmap(Gumps.GetGump(entry.GumpMale, Ultima.Hues.GetHue(SelectedEntry.HueColor), SelectedEntry.ItemPartialHue == CheckState.Checked, out patched)), 0, 0);
                    else*/ graphpic.DrawImage(new Bitmap(Gumps.GetGump(entry.GumpMale)), 0, 0);

                    var trect = new Rectangle(0, 237, pbMGump.Width, pbMGump.Image.Height - 237);
                    var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;
                    TextRenderer.DrawText(graphpic, String.Format("{0,5} (0x{0:X4})", entry.GumpMale), FontItemName, trect, Color.LightSlateGray, flags);
                }
            } pbMGump.Invalidate();
            using (var graphpic = Graphics.FromImage(pbFGump.Image)) {
                graphpic.Clear(Color.Black);
                if (race.F_Gump > 0)
                    graphpic.DrawImage(new Bitmap(Gumps.GetGump(race.F_Gump, Ultima.Hues.GetHue(race.ColHue), true, out patched)), 0,0);
                var gumpId = Gumps.IsValidIndex(entry.GumpFemale) ? entry.GumpFemale : entry.GumpMale;
                if (Gumps.IsValidIndex(gumpId) && entry.Number > 0) {
                    /*if (SelectedEntry.HueColor > 0)
                        graphpic.DrawImage(new Bitmap(Gumps.GetGump(gumpId, Ultima.Hues.GetHue(SelectedEntry.HueColor), SelectedEntry.ItemPartialHue == CheckState.Checked, out patched)), 0, 0);
                    else*/ graphpic.DrawImage(new Bitmap(Gumps.GetGump(gumpId)), 0, 0);
                    
                    if (gumpId == entry.GumpFemale) {
                        var trect = new Rectangle(0, 237, pbFGump.Width, pbFGump.Image.Height - 237);
                        var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;
                        TextRenderer.DrawText(graphpic, String.Format("{0,5} (0x{0:X4})", gumpId), FontItemName, trect, Color.LightSlateGray, flags);
                    }
                    
                }
            } pbFGump.Invalidate();
        }

        private void DrawBitm(object sender, DrawListViewItemEventArgs e)
        {
            var bmp = (Bitmap)e.Item.Tag;
            if (bmp == null) return;

            if (!e.Item.Selected)//((e.State & ListViewItemStates.Focused) == 0)
                e.Graphics.DrawRectangle(PenItemBorder, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            else
                e.Graphics.DrawRectangle(PenBlack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);

            if (cbBitmapType.SelectedIndex == 1) // Тайлы
                e.Graphics.DrawImage(bmp, e.Bounds.X + 1, e.Bounds.Y + 1,
                                        new Rectangle((bmp.Width-e.Bounds.Width)/2, (bmp.Height-e.Bounds.Height)/2, e.Bounds.Width-1, e.Bounds.Height-1),
                                        GraphicsUnit.Pixel);
            else if (cbBitmapType.SelectedIndex == 0) // Гампы
                e.Graphics.DrawImage(bmp, new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width-2, e.Bounds.Height-2), 17f, 30f, 156f, 194f, GraphicsUnit.Pixel);

            if (!e.Item.Selected)//((e.State & ListViewItemStates.Focused) == 0)
                e.Graphics.DrawRectangle(PenItemBorder, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            else
                e.Graphics.DrawRectangle(PenBlack, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
        }

        #endregion

        #region Левая панель

        private void treeViewBody_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectEntry(enties[treeViewBody.SelectedNode != null ? (int)treeViewBody.SelectedNode.Tag : 0]);
        }

        private void contextMenuBody_PrevValid_Click(object sender, EventArgs e)
        {
            var i = (int)treeViewBody.SelectedNode.Tag;
            while (--i >= 0 && (enties[i] == null || !enties[i].IsValid())) ;
            if (i >= 0)
                treeViewBody.SelectedNode = treeViewBody.Nodes[i];
        }

        private void contextMenuBody_NextValid_Click(object sender, EventArgs e)
        {
            var i = (int)treeViewBody.SelectedNode.Tag;
            while (++i < enties.Length && (enties[i] == null || !enties[i].IsValid())) ;
            if (i < enties.Length)
                treeViewBody.SelectedNode = treeViewBody.Nodes[i];
        }

        private void contextMenuBody_PrevInvalid_Click(object sender, EventArgs e)
        {
            var i = (int)treeViewBody.SelectedNode.Tag;
            while (--i >= 0 && (enties[i] == null || enties[i].IsValid())) ;
            if (i >= 0)
                treeViewBody.SelectedNode = treeViewBody.Nodes[i];
        }

        private void contextMenuBody_NextInvalid_Click(object sender, EventArgs e)
        {
            var i = (int)treeViewBody.SelectedNode.Tag;
            while (++i < enties.Length && (enties[i] == null || enties[i].IsValid())) ;
            if (i < enties.Length)
                treeViewBody.SelectedNode = treeViewBody.Nodes[i];
        }

        private void contextMenuBody_PrevFree_Click(object sender, EventArgs e)
        {
            var i = (int)treeViewBody.SelectedNode.Tag;
            while (--i >= 0 && (enties[i] != null && Animations.IsAnimDefinied(enties[i].Animation))) ;
            if (i >= 0)
                treeViewBody.SelectedNode = treeViewBody.Nodes[i];
        }

        private void contextMenuBody_NextFree_Click(object sender, EventArgs e)
        {
            var i = (int)treeViewBody.SelectedNode.Tag;
            while (++i < enties.Length && (enties[i] != null && Animations.IsAnimDefinied(enties[i].Animation))) ;
            if (i < enties.Length)
                treeViewBody.SelectedNode = treeViewBody.Nodes[i];
        }

        private void contextMenuBody_GoTo1000_Click(object sender, EventArgs e)
        {
            treeViewBody.Nodes[1000].EnsureVisible();
        }

        private void contextMenuBody_GoTo1600_Click(object sender, EventArgs e)
        {
            treeViewBody.Nodes[1600].EnsureVisible();
        }

        private void treeViewBody_UpdateNode(int animId, bool update = true)
        {
            if (update) {
                treeViewBody.BeginUpdate();
            }

            var enty = enties[animId];
            var node = treeViewBody.Nodes[animId];
            var used = Animations.IsAnimDefinied(animId);
            var real = Animations.Translate(animId) == animId;
            var vald = enty == null ? false : enty.IsValid();

            //treeViewBody.LabelEdit = true;
            //node.BeginEdit();
            node.Text = String.Format("{0,4}", animId);
            if (enty != null) {
                node.Text += String.Format(" - {0}", used ? !String.IsNullOrEmpty(enty.ItemName) ? enty.ItemName : "NoName" : "Empty");
            } else {
                node.Text += animId < 1000 ? used ? " - Creature" : " - Invalid" : "";
            }

            node.ForeColor = animId < 1000 ? (used ? real ? Color.DarkGreen : Color.MediumSeaGreen : Color.LightGray)
                                           : (used ? vald ? real ? Color.DarkBlue : Color.Black : Color.DarkRed : Color.DarkGray);
            //node.EndEdit(false);
            //treeViewBody.LabelEdit = false; 
            if (update) {
                treeViewBody.EndUpdate();
                treeViewBody.Update();
                treeViewBody.Refresh();
            }
            //treeViewBody.Invalidate();
        }

        #endregion

        #region Редактирование Тайлов

        private void contextMenuItem_CopyTo_SetLength(int count, bool visible = true)
        {
            if (visible)
                contextMenuItem_CopyTo_SetLength(-1, false);
            switch (count) {
                default :                                            
                case 12: contextMenuItem_CopyTo12.Visible = cmBrowserItems_CopyTo12.Visible = visible; goto case 11;
                case 11: contextMenuItem_CopyTo11.Visible = cmBrowserItems_CopyTo11.Visible = visible; goto case 10;
                case 10: contextMenuItem_CopyTo10.Visible = cmBrowserItems_CopyTo10.Visible = visible; goto case  9;
                case  9: contextMenuItem_CopyTo09.Visible = cmBrowserItems_CopyTo09.Visible = visible; goto case  8;
                case  8: contextMenuItem_CopyTo08.Visible = cmBrowserItems_CopyTo08.Visible = visible; goto case  7;
                case  7: contextMenuItem_CopyTo07.Visible = cmBrowserItems_CopyTo07.Visible = visible; goto case  6;
                case  6: contextMenuItem_CopyTo06.Visible = cmBrowserItems_CopyTo06.Visible = visible; goto case  5;
                case  5: contextMenuItem_CopyTo05.Visible = cmBrowserItems_CopyTo05.Visible = visible; goto case  4;
                case  4: contextMenuItem_CopyTo04.Visible = cmBrowserItems_CopyTo04.Visible = visible; goto case  3;
                case  3: contextMenuItem_CopyTo03.Visible = cmBrowserItems_CopyTo03.Visible = visible; goto case  2;
                case  2: contextMenuItem_CopyTo02.Visible = cmBrowserItems_CopyTo02.Visible = visible; goto case  1;
                case  1: contextMenuItem_CopyTo01.Visible = cmBrowserItems_CopyTo01.Visible = visible; goto case  0;
                case  0: break;
            }
        }

        private void contextMenuItem_CopyTo_Click(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems.Count == 0 || SelectedEntry == null) return;
            SelectedEntry.ReplaceItem(int.Parse((string)(sender as ToolStripMenuItem).Tag), (int)listViewItem.SelectedItems[0].Tag);
            SelectEntry(SelectedEntry);
            listViewItem.Invalidate();
        }

        private void cmBrowserItems_CopyTo_Click(object sender, EventArgs e)
        {
            if (listViewBitmap.SelectedItems.Count == 0 || SelectedEntry == null) return;
            SelectedEntry.ReplaceItem(int.Parse((string)(sender as ToolStripMenuItem).Tag), (Bitmap)listViewBitmap.SelectedItems[0].Tag);
            SelectEntry(SelectedEntry);
            listViewItem.Invalidate();
        }

        private void cmBrowserItems_CopyAll_Click(object sender, EventArgs e)
        {
            cmBrowserGumps_CopyAll_Click(sender, e);
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            if (SelectedEntry != null)
                SelectedEntry.SetItemName(tbName.Text);
        }

        private void cbWeapon_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectedEntry != null)
                SelectedEntry.SetItemWeapon(cbWeapon.Checked);
        }

        private void cbArmor_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectedEntry != null)
                SelectedEntry.SetItemArmor(cbArmor.Checked);
        }

        private void cbPartialHue_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectedEntry != null)
                SelectedEntry.SetItemPartialHue(cbPartialHue.Checked);
            listViewBodyHue.Invalidate();
        }

        private void cbLightSource_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectedEntry != null)
                SelectedEntry.SetItemLightSource(cbLightSource.Checked);
        }

        #endregion

        #region Редактирование Гампов

        private void pbGump_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                cmGumps.Tag = sender;
        }

        private void cmGumps_Remove_Click(object sender, EventArgs e)
        {
            if (SelectedEntry == null) return;
            if (cmGumps.Tag == pbMGump)
                SelectedEntry.SetMaleGump(null);
            else if (cmGumps.Tag == pbFGump)
                SelectedEntry.SetFemaleGump(null);
            DrawGump(SelectedEntry);
            listViewGump.Invalidate();
        }

        private void cmGumps_FlipType_Click(object sender, EventArgs e)
        {
            if (SelectedEntry == null) return;
            var temp1 = SelectedEntry.GumpMale   < 0 ? null : Ultima.Gumps.GetGump(SelectedEntry.GumpMale).Clone()   as Bitmap;
            var temp2 = SelectedEntry.GumpFemale < 0 ? null : Ultima.Gumps.GetGump(SelectedEntry.GumpFemale).Clone() as Bitmap;
            SelectedEntry.SetMaleGump(temp2);
            SelectedEntry.SetFemaleGump(temp1);
            DrawGump(SelectedEntry);
            listViewGump.Invalidate();
        }

        private void contextMenuGump_AddMale_Click(object sender, EventArgs e)
        {
            if (SelectedEntry == null) return;
            if (listViewGump.SelectedItems.Count == 0) return;
            SelectedEntry.SetMaleGump((int)listViewGump.SelectedItems[0].Tag);
            DrawGump(SelectedEntry);
            listViewGump.Invalidate();
        }

        private void contextMenuGump_AddFemale_Click(object sender, EventArgs e)
        {
            if (SelectedEntry == null) return;
            if (listViewGump.SelectedItems.Count == 0) return;
            SelectedEntry.SetFemaleGump((int)listViewGump.SelectedItems[0].Tag);
            DrawGump(SelectedEntry);
            listViewGump.Invalidate();
        }

        private void cmBrowserGumps_AddMale_Click(object sender, EventArgs e)
        {
            if (SelectedEntry == null) return;
            if (listViewBitmap.SelectedItems.Count == 0) return;
            SelectedEntry.SetMaleGump((Bitmap)listViewBitmap.SelectedItems[0].Tag);
            DrawGump(SelectedEntry);
            listViewGump.Invalidate();
        }

        private void cmBrowserGumps_AddFemale_Click(object sender, EventArgs e)
        {
            if (SelectedEntry == null) return;
            if (listViewBitmap.SelectedItems.Count == 0) return;
            SelectedEntry.SetFemaleGump((Bitmap)listViewBitmap.SelectedItems[0].Tag);
            DrawGump(SelectedEntry);
            listViewGump.Invalidate();
        }

        private void cmBrowserGumps_CopyAll_Click(object sender, EventArgs e)
        {
            int anim; string folder = filesList.SelectedDirectory;
            if (SelectedEntry == null || String.IsNullOrEmpty(folder) || !Directory.Exists(folder) || listViewBitmap.SelectedItems.Count == 0 ||
                !Int32.TryParse(listViewBitmap.SelectedItems[0].ToolTipText.Substring(1, 4), out anim))
                return;

            var files = new List<string>(16);
            string gump_m = null, gump_f = null, tile_1 = null, tile_2 = null;
            try {
                files.Clear();
                files.AddRange(Directory.GetFiles(folder, String.Format("G{0:D4}*[муж]*.bmp", anim), SearchOption.TopDirectoryOnly));
                files.AddRange(Directory.GetFiles(folder, String.Format("G{0:D4}*[муж]*.png", anim), SearchOption.TopDirectoryOnly));
                gump_m = (files.Count > 0) ? files[0] : null;

                files.Clear();
                files.AddRange(Directory.GetFiles(folder, String.Format("G{0:D4}*[жен]*.bmp", anim), SearchOption.TopDirectoryOnly));
                files.AddRange(Directory.GetFiles(folder, String.Format("G{0:D4}*[жен]*.png", anim), SearchOption.TopDirectoryOnly));
                gump_f = (files.Count > 0) ? files[0] : null;

                files.Clear();
                files.AddRange(Directory.GetFiles(folder, String.Format("I{0:D4}*.bmp", anim), SearchOption.TopDirectoryOnly));
                files.AddRange(Directory.GetFiles(folder, String.Format("I{0:D4}*.png", anim), SearchOption.TopDirectoryOnly));
                tile_1 = (files.Count > 0) ? files[0] : null;
                tile_2 = (files.Count > 0) ? files[1] : null;
            } catch {;}
            Bitmap bitmap;
            if (!String.IsNullOrEmpty(gump_m)) {
                bitmap = new Bitmap(gump_m);
                if (bitmap.Width == 260 && bitmap.Height == 237)
                    SelectedEntry.SetMaleGump(bitmap);
            }
            if (!String.IsNullOrEmpty(gump_f)) {
                bitmap = new Bitmap(gump_f);
                if (bitmap.Width == 260 && bitmap.Height == 237)
                    SelectedEntry.SetFemaleGump(bitmap);
            }
            if (!String.IsNullOrEmpty(tile_1)) {
                bitmap = new Bitmap(tile_1);
                if (bitmap.Width < 128 && bitmap.Height < 128)
                    SelectedEntry.ReplaceItem(0, bitmap);
            }
            if (!String.IsNullOrEmpty(tile_2)) {
                bitmap = new Bitmap(tile_2);
                if (bitmap.Width < 128 && bitmap.Height < 128)
                    SelectedEntry.ReplaceItem(1, bitmap);
            }

            SelectEntry(SelectedEntry);
            listViewItem.Invalidate();
            listViewGump.Invalidate();
        }

        #endregion

        #region Редактирование Анимации

        private void listViewBody_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewBody.SelectedItems.Count == 0) return;
            if (SelectedEntry != null) {
                SelectedEntry.SetAnimation((listViewBody.SelectedItems[0].Tag as int[])[3]);
                tbRealBody.Text = String.Format("{0,4}", numNumber.Value = SelectedEntry.Animation);
                tbRealBody.ForeColor = Animations.IsAnimDefinied(SelectedEntry.Animation) ? Color.Blue : Color.DarkRed;
            }
            if (!menuPlayAnimation.Checked)
                lvAnim.Invalidate();

            listViewBody.Invalidate();
        }

        private void listViewBodyHue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewBodyHue.SelectedItems.Count == 0) return;
            if (SelectedEntry != null)
                SelectedEntry.SetHueColor((listViewBodyHue.SelectedItems[0].Tag as int[])[3]);
            if (!menuPlayAnimation.Checked)
                lvAnim.Invalidate();

            listViewBodyHue.Invalidate();
            DrawGump(SelectedEntry);
        }

        #endregion

        #region Редактирование Тайлов

        private void cmItems_Remove_Click(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count <= 0 || SelectedEntry == null) return;
            SelectedEntry.RemoveItem((int)lvItems.SelectedItems[0].Tag);
            SelectEntry(SelectedEntry);
            listViewItem.Invalidate();
        }

        private void contextMenuItem_Add_Click(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems.Count <= 0 || SelectedEntry == null) return;
            SelectedEntry.AddItem((int)listViewItem.SelectedItems[0].Tag);
            SelectEntry(SelectedEntry);
            listViewItem.Invalidate();
        }

        private void cmItems_AddItemsFor_Click(object sender, EventArgs e)
        {
            if (SelectedEntry == null) return;
            int start_id, end_id, dir = 0, id;
            switch ((string)(sender as ToolStripMenuItem).Tag) {
                case "cloth":           start_id = 0x6362; end_id = 0x6B62; dir = -1; break;
                case "armor":           start_id = 0x6362; end_id = 0x6902; dir = +1; break;
                case "shield":          start_id = 0x629A; end_id = 0x6361; dir = +1; break;

                case "weapon-bladed":   start_id = 0x6F17; end_id = 0x6FFF; dir = +1; break;
                case "weapon-axe":      start_id = 0x6E2A; end_id = 0x6F16; dir = +1; break;
                case "weapon-blunt":    start_id = 0x6D3D; end_id = 0x6E29; dir = +1; break;
                case "weapon-polearm":  start_id = 0x6C50; end_id = 0x6D3C; dir = +1; break;
                case "weapon-range":    start_id = 0x6B63; end_id = 0x6C4F; dir = +1; break;

                case "hair":            start_id = 0x6005; end_id = 0x6099; dir = +1; break;
                case "bard":            start_id = 0x6005; end_id = 0x6099; dir = -1; break;
                case "misc":            start_id = 0x609A; end_id = 0x6299; dir = +1; break;
                default: return;
            } if (dir == 0) return;
            id = dir > 0 ? start_id : end_id;

            while (EquipEntry.FromTile(id).Number != 0) {
                if (id < start_id || id > end_id) return;
                id += dir;
            }
            if (EquipEntry.FromTile(id+dir).Number != 0) return;

            SelectedEntry.AddItem(Math.Min(id, id+dir));
            SelectedEntry.AddItem(Math.Max(id, id+dir));

            SelectEntry(SelectedEntry);
            listViewItem.Invalidate();
        }

        private void contextMenuItem_VFlip_Click(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems.Count <= 0) return;
            RotateFlip(RotateFlipType.RotateNoneFlipY, (int)listViewItem.SelectedItems[0].Tag);
        }

        private void contextMenuItem_HFlip_Click(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems.Count <= 0) return;
            RotateFlip(RotateFlipType.RotateNoneFlipX, (int)listViewItem.SelectedItems[0].Tag);
        }

        private void contextMenuItem_RotateL90_Click(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems.Count <= 0) return;
            RotateFlip(RotateFlipType.Rotate90FlipXY, (int)listViewItem.SelectedItems[0].Tag);
        }

        private void contextMenuItem_RotateR90_Click(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems.Count <= 0) return;
            RotateFlip(RotateFlipType.Rotate90FlipNone, (int)listViewItem.SelectedItems[0].Tag);
        }

        private void cmItems_HFlip_Click(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems.Count <= 0) return;
            RotateFlip(RotateFlipType.RotateNoneFlipY, (int)lvItems.SelectedItems[0].Tag);
        }

        private void cmItems_VFlip_Click(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems.Count <= 0) return;
            RotateFlip(RotateFlipType.RotateNoneFlipX, (int)lvItems.SelectedItems[0].Tag);
        }

        private void cmItems_RotateL90_Click(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems.Count <= 0) return;
            RotateFlip(RotateFlipType.RotateNoneFlipX, (int)lvItems.SelectedItems[0].Tag);
        }

        private void cmItems_RotateR90_Click(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems.Count <= 0) return;
            RotateFlip(RotateFlipType.Rotate90FlipNone, (int)lvItems.SelectedItems[0].Tag);
        }

        private void RotateFlip(RotateFlipType type, int tileId)
        {
            var bmp = Art.GetStatic(tileId);
            bmp.RotateFlip(type);
            Art.ReplaceStatic(tileId, bmp);
            SelectEntry(SelectedEntry);
            listViewItem.Invalidate();
        }

        #endregion

        #region Вкладка - Проводник

        private void filesList_KeyDown(object sender, KeyEventArgs e)
        {
            var dir = filesList.SelectedDirectory;
            switch (e.KeyCode) {
                case Keys.Enter: filesList.SelectedPath = filesList.SelectedDirectory ?? Path.GetDirectoryName(filesList.SelectedPath); break;
                case Keys.Back : filesList.SelectedPath = Path.GetDirectoryName(filesList.SelectedPath); break;
                default: return;
            }
        }

        private void filesList_SelectedPathChanged(object sender, FilesBrowser.SelectedPathChangedEventArgs fse)
        {
            tbSelectedPath.Text = fse.NewSelectedPath;
        }

        private void tbSelectedPath_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode) {
                case Keys.Enter: if (!String.IsNullOrEmpty(tbSelectedPath.Text) && Directory.Exists(tbSelectedPath.Text)) filesList.SelectedPath = tbSelectedPath.Text; 
                                 else tbSelectedPath.Text = filesList.SelectedPath; break;
                default: return;
            }
        }

        

        private void filesList_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadThumbnail(filesList.SelectedDirectory ?? filesList.SelectedPath);
        }

        private void cbBitmapType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbBitmapType.SelectedIndex) {
                case 0: listViewBitmap.TileSize = listViewGump.TileSize; listViewBitmap.ContextMenuStrip = cmBrowserGumps; break;// Гампы
                case 1: listViewBitmap.TileSize = listViewItem.TileSize; listViewBitmap.ContextMenuStrip = cmBrowserItems; break;// Тайлы
                default: return;
            }
            LoadThumbnail(filesList.SelectedDirectory ?? filesList.SelectedPath);
        }

        private void LoadThumbnail(string folder)
        {
            listViewBitmap.BeginUpdate();
            listViewBitmap.Items.Clear();

            if (!String.IsNullOrEmpty(folder) && Directory.Exists(folder)) {
                var files = new List<string>(512);
                try {
                    files.AddRange(Directory.GetFiles(folder, "*.bmp", SearchOption.TopDirectoryOnly));
                    files.AddRange(Directory.GetFiles(folder, "*.png", SearchOption.TopDirectoryOnly));
                } catch {;}

                foreach (var file in files) {
                    var bmp = new Bitmap(file);
                    switch (cbBitmapType.SelectedIndex) {
                        case 0 : if (bmp.Width != 260 || bmp.Height != 237) continue; break;// Гампы
                        case 1 : if (bmp.Width >= 128 || bmp.Height >= 128) continue; break;// Тайлы
                        default: return;
                    }

                    var node = new ListViewItem();
                    node.Tag = bmp;
                    node.ToolTipText = String.Format("{1}{0}{0}Формат: *.{2} ({3}){0}Размеры: {4}x{5}{0}", Environment.NewLine,
                                       Path.GetFileName(file), Path.GetExtension(file), bmp.PixelFormat, bmp.Width, bmp.Height);
                    listViewBitmap.Items.Add(node);
                }
            }

            listViewBitmap.EndUpdate();
            listViewBitmap.Invalidate();
        }

        #endregion

        

        
               
    }

    
}
