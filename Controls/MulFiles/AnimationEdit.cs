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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GuiControls;
using Ultima;

namespace FiddlerControls
{
    public partial class AnimationEdit : UserControl
    {
        public AnimationEdit()
        {
            InitializeComponent();
            //this.Icon = FiddlerControls.Options.GetFiddlerIcon();
            FileType = 0;
            CurrDir = 0;
            toolStripComboBox1.SelectedIndex = 0;
            FramePoint = new Point(pictureBox1.Width / 2, pictureBox1.Height * 3 / 4);
            ShowOnlyValid = false;
            Loaded = false;
            listView1.MultiSelect = true;

            toolStripButton1.Image = Image.FromStream(Resources.GetStream(@"Icons.animedit", "move", "png"));
            toolStripButton12.Image = Image.FromStream(Resources.GetStream(@"Icons.animedit", "mouse", "png"));
            toolStripButton10.Image = Image.FromStream(Resources.GetStream(@"Icons.animedit", "border", "png"));
            toolStripButton7.Image = Image.FromStream(Resources.GetStream(@"Icons.animedit", "background", "png"));
            checkBox2.Image = Image.FromStream(Resources.GetStream(@"Icons.animedit", "lock", "png"));
            toolStripButton6.Image = Image.FromStream(Resources.GetStream(@"Icons.animedit", "lock", "png"));
            toolStripButton11.Image = Image.FromStream(Resources.GetStream(@"Icons.animedit", "playbtn", "png"));
            tsbDirection1.Image = Image.FromStream(Resources.GetStream(@"Icons.animedit", "arrow1", "png"));
            tsbDirection2.Image = Image.FromStream(Resources.GetStream(@"Icons.animedit", "arrow2", "png"));
            tsbDirection3.Image = Image.FromStream(Resources.GetStream(@"Icons.animedit", "arrow3", "png"));
            tsbDirection4.Image = Image.FromStream(Resources.GetStream(@"Icons.animedit", "arrow4", "png"));
            tsbDirection5.Image = Image.FromStream(Resources.GetStream(@"Icons.animedit", "arrow5", "png"));
        }
        /*
        public AnimationEdit(Control parent) : this()
        {
            Cursor.Current = Cursors.WaitCursor;
            mParentControls = new List<Control>(parent.Controls.Count);
            foreach (Control control in parent.Controls)
                if (control.Visible) {
                    mParentControls.Add(control);
                    control.Visible = false;
                }

            this.Parent = parent;
            this.Dock = DockStyle.Fill;
            mMinimumSize = ParentForm.MinimumSize;
            ParentForm.MinimumSize = new Size(MinimumSize.Width + ParentForm.Size.Width - Parent.ClientSize.Width, MinimumSize.Height + ParentForm.Size.Height - Parent.ClientSize.Height);
            parent.Refresh();
            Cursor.Current = Cursors.Default;
        }
        
        private Size mMinimumSize;
        private List<Control> mParentControls;
     
        private void onClose(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            AnimationEdit_FormClosing(sender, new FormClosingEventArgs(CloseReason.UserClosing, false));

            this.Visible = false;
            this.DestroyHandle();
            foreach (Control control in mParentControls)
                control.Visible = true;

            this.ParentForm.MinimumSize = mMinimumSize;
            this.Dispose();
            Cursor.Current = Cursors.Default;
        }
        */

        static int[] AnimCx = new int[5];
        static int[] AnimCy = new int[5];
        private bool Loaded;
        private int FileType;
        int CurrAction;
        int CurrBody;
        private int CurrDir;
        private Point FramePoint;
        private bool ShowOnlyValid;
        static bool DrawEmpty = false;
        static bool DrawFull = false;
        static Pen BlackUndraw = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
        static SolidBrush WhiteUndraw = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
        static SolidBrush WhiteTrasparent = new SolidBrush(Color.FromArgb(160, 255, 255, 255));
        static readonly Color WhiteConvert = Color.FromArgb(255, 255, 255, 255);
        static readonly Color GreyConvert = Color.FromArgb(255, 170, 170, 170);

        #region AnimNames
        private string[][] AnimNames = {
            //new string[]{"Walk","Run","Idle","Idle","Fidget","Attack1","Attack2","GetHit","Die1"},//sea
            new string[]{"Walk","Run","Idle","Eat","Alert","Attack1","Attack2","GetHit","Die1","Idle","Fidget",
                         "LieDown","Die2"},//animal
            new string[]{"Walk","Idle","Die1","Die2","Attack1","Attack2","Attack3","AttackBow","AttackCrossBow",
                         "AttackThrow","GetHit","Pillage","Stomp","Cast2","Cast3","BlockRight","BlockLeft","Idle",
                         "Fidget","Fly","TakeOff","GetHitInAir"}, //Monster
            new string[]{"Walk_01","WalkStaff_01","Run_01","RunStaff_01","Idle_01","Idle_01",
                         "Fidget_Yawn_Stretch_01","CombatIdle1H_01","CombatIdle1H_01","AttackSlash1H_01",
                         "AttackPierce1H_01","AttackBash1H_01","AttackBash2H_01","AttackSlash2H_01",
                         "AttackPierce2H_01","CombatAdvance_1H_01","Spell1","Spell2","AttackBow_01",
                         "AttackCrossbow_01","GetHit_Fr_Hi_01","Die_Hard_Fwd_01","Die_Hard_Back_01",
                         "Horse_Walk_01","Horse_Run_01","Horse_Idle_01","Horse_Attack1H_SlashRight_01",
                         "Horse_AttackBow_01","Horse_AttackCrossbow_01","Horse_Attack2H_SlashRight_01",
                         "Block_Shield_Hard_01","Punch_Punch_Jab_01","Bow_Lesser_01","Salute_Armed1h_01",
                         "Ingest_Eat_01"}//human
        };
        public string[][] GetAnimNames { get { return AnimNames; } }
        #endregion

        private bool useCKeyFilter { get { return useCKeyFilterToolStripMenuItem.Checked; } }
        private bool useBColFilter { get { return useBColFilterToolStripMenuItem.Checked; } }

        private void onLoad(object sender, EventArgs e)
        {
            Options.LoadedUltimaClass["AnimationEdit"] = true;
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            if (FileType != 0)
            {
                int count = Animations.GetAnimCount(FileType);
                List<TreeNode> nodes = new List<TreeNode>();
                for (int i = 0; i < count; ++i)
                {
                    int animlength = Animations.GetAnimLength(i, FileType);
                    string type = animlength == 22 ? "H" : animlength == 13 ? "L" : "P";
                    TreeNode node = new TreeNode();
                    node.Tag = i;
                    node.Text = String.Format("{0}: {1} ({2})", type, i, BodyConverter.GetTrueBody(FileType, i));
                    bool valid = false;
                    for (int j = 0; j < animlength; ++j)
                    {
                        TreeNode subnode = new TreeNode();
                        subnode.Tag = j;
                        subnode.Text = String.Format("{0:D2} {1}", j, AnimNames[animlength == 22 ? 1 : animlength == 13 ? 0 : 2][j]);
                        if (Ultima.AnimationEdit.IsActionDefinied(FileType, i, j))
                            valid = true;
                        else
                            subnode.ForeColor = Color.Red;
                        node.Nodes.Add(subnode);
                    }
                    if (!valid)
                    {
                        if (ShowOnlyValid)
                            continue;
                        node.ForeColor = Color.Red;
                    }
                    nodes.Add(node);

                    var anim_items = toolStripComboBox1.Tag as int[];
                    while (anim_items[0] <= 0) {
                        toolStripComboBox1.Items.RemoveAt(0);
                        Array.Reverse(anim_items);
                        Array.Resize(ref anim_items, anim_items.Length-1);
                        Array.Reverse(anim_items);
                        toolStripComboBox1.Tag = anim_items;
                    } 
                }
                treeView1.Nodes.AddRange(nodes.ToArray());
            } 
            treeView1.EndUpdate();
            if (treeView1.Nodes.Count > 0)
                treeView1.SelectedNode = treeView1.Nodes[0];
            if (!Loaded) {
                FiddlerControls.Events.FilePathChangeEvent += new FiddlerControls.Events.FilePathChangeHandler(OnFilePathChangeEvent);

                var list = new List<int>(16); list.Add(0);
                for (int i = 1; i < toolStripComboBox1.Items.Count; ++i) {
                    var item = toolStripComboBox1.Items[i].ToString();
                    if (!File.Exists(Ultima.Files.GetFilePath(item + ".idx")) || !File.Exists(Ultima.Files.GetFilePath(item + ".mul")))
                        toolStripComboBox1.Items[i] = String.Empty;
                    else list.Add(i);
                }
                for (int i = toolStripComboBox1.Items.Count-1; i > 0; --i)
                    if (toolStripComboBox1.Items[i].ToString() == String.Empty) toolStripComboBox1.Items.RemoveAt(i);
                toolStripComboBox1.Tag = list.ToArray();
            }
            Loaded = true;
        }

        private void OnFilePathChangeEvent()
        {
            if (!Loaded)
                return;
            FileType = 0;
            CurrDir = 0;
            CurrAction = 0;
            CurrBody = 0;
            toolStripComboBox1.SelectedIndex = 0;
            FramePoint = new Point(pictureBox1.Width / 2, pictureBox1.Height * 3 / 4);
            ShowOnlyValid = false;
            showOnlyValidToolStripMenuItem.Checked = false;
            OnLoad(null);
        }

        private TreeNode GetNode(int tag)
        {
            if (ShowOnlyValid)
            {
                foreach (TreeNode node in treeView1.Nodes)
                {
                    if ((int)node.Tag == tag)
                        return node;
                }
                return null;
            }
            else
                return treeView1.Nodes[tag];
        }

        private unsafe void SetPaletteBox()
        {
            if (FileType != 0)
            {
                AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                Bitmap bmp = new Bitmap(0x100, pictureBoxPalette.Height, PixelFormat.Format16bppArgb1555);
                if (edit != null)
                {
                    BitmapData bd = bmp.LockBits(new Rectangle(0, 0, 0x100, pictureBoxPalette.Height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
                    ushort* line = (ushort*)bd.Scan0;
                    int delta = bd.Stride >> 1;
                    for (int y = 0; y < bd.Height; ++y, line += delta)
                    {
                        ushort* cur = line;
                        for (int i = 0; i < 0x100; ++i)
                        {
                            *cur++ = edit.Palette[i];
                        }
                    }
                    bmp.UnlockBits(bd);
                }
                pictureBoxPalette.Image = bmp;
            }
        }

        private void AfterSelectTreeView(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Parent == null)
                {
                    if (treeView1.SelectedNode.Tag != null)
                        CurrBody = (int)treeView1.SelectedNode.Tag;
                    CurrAction = 0;
                }
                else
                {
                    if (treeView1.SelectedNode.Parent.Tag != null)
                        CurrBody = (int)treeView1.SelectedNode.Parent.Tag;
                    CurrAction = (int)treeView1.SelectedNode.Tag;
                }
                listView1.BeginUpdate();
                listView1.Clear();
                AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                if (edit != null)
                {
                    int width = 80;
                    int height = 110;
                    Bitmap[] currbits = edit.GetFrames();
                    if (currbits != null)
                    {
                        for (int i = 0; i < currbits.Length; ++i)
                        {
                            if (currbits[i] == null)
                                continue;
                            ListViewItem item;
                            item = new ListViewItem(i.ToString(), 0);
                            item.Tag = i;
                            listView1.Items.Add(item);
                            if (currbits[i].Width > width)
                                width = currbits[i].Width;
                            if (currbits[i].Height > height)
                                height = currbits[i].Height;
                        }
                        listView1.TileSize = new Size(width + 5, height + 5);
                        trackBar2.Maximum = currbits.Length - 1;
                        trackBar2.Value = 0;
                        trackBar2.Invalidate();

                        numericUpDownCx.Value = edit.Frames[trackBar2.Value].Center.X;
                        numericUpDownCy.Value = edit.Frames[trackBar2.Value].Center.Y;
                    }
                    //Soulblighter Modification
                    else
                    {
                        trackBar2.Maximum = 0;
                        trackBar2.Value = 0;
                        trackBar2.Invalidate();
                    }
                    //End of Soulblighter Modification
                }
                listView1.EndUpdate();
                if (listView1.Items.Count > 0)
                    onFrameCountBarChanged(null, null);
                pictureBox1.Invalidate();
                SetPaletteBox();
            }
        }

        private void DrawFrameItem(object sender, DrawListViewItemEventArgs e)
        {
            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
            Bitmap[] currbits = edit.GetFrames();
            Bitmap bmp = currbits[(int)e.Item.Tag];
            int width = bmp.Width;
            int height = bmp.Height;

           
            if (listView1.SelectedItems.Contains(e.Item)) {
                if ((int)e.Item.Tag == trackBar2.Value)
                    e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                else
                    e.Graphics.FillRectangle(Brushes.LightCyan, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                e.Graphics.DrawRectangle(new Pen(Color.Red), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            } else {
                e.Graphics.FillRectangle(Brushes.White, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                e.Graphics.DrawRectangle(new Pen(Color.Gray), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            }
            e.Graphics.DrawImage(bmp, e.Bounds.X, e.Bounds.Y, width, height);
            e.DrawText(TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter);
        }

        private void onAnimChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox1.Tag == null) return;
            var selected = (toolStripComboBox1.Tag as int[])[toolStripComboBox1.SelectedIndex];
            if (selected != FileType) {
                FileType = selected;
                onLoad(this, EventArgs.Empty);
            }
        }

        private void OnDirectionChanged(object sender, EventArgs e)
        {
            if (sender == trackBar1) {   CurrDir = trackBar1.Value;
            } else if (sender == tsbDirection1) {  trackBar1.Value = CurrDir = 0;
            } else if (sender == tsbDirection2) {  trackBar1.Value = CurrDir = 1;
            } else if (sender == tsbDirection3) {  trackBar1.Value = CurrDir = 2;
            } else if (sender == tsbDirection4) {  trackBar1.Value = CurrDir = 3;
            } else if (sender == tsbDirection5) {  trackBar1.Value = CurrDir = 4;
            }

            tsbDirection1.Checked = tsbDirection2.Checked = tsbDirection3.Checked
                    = tsbDirection4.Checked = tsbDirection5.Checked = false;
            switch (trackBar1.Value) {
                case 0: tsbDirection1.Checked = true; break;
                case 1: tsbDirection2.Checked = true; break;
                case 2: tsbDirection3.Checked = true; break;
                case 3: tsbDirection4.Checked = true; break;
                case 4: tsbDirection5.Checked = true; break;
            }

            AfterSelectTreeView(null, null);
        }

        private void OnSizeChangedPictureBox(object sender, EventArgs e)
        {
            FramePoint = new Point(pictureBox1.Width / 2, pictureBox1.Height * 3 / 4);
            pictureBox1.Invalidate();
        }
        //Soulblighter Modification

        private void onPaintFrame(object sender, PaintEventArgs e)
        {

            AnimIdx edit = null;
            try {
                edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
            } catch {;}

            if (edit != null)
            {
                Bitmap[] currbits = edit.GetFrames();
                int varW = 0;
                int varH = 0;
                int varFW = 0;
                int varFH = 0;
                e.Graphics.Clear(Color.LightGray);
                e.Graphics.DrawLine(Pens.Black, new Point(FramePoint.X, 0), new Point(FramePoint.X, pictureBox1.Height));
                e.Graphics.DrawLine(Pens.Black, new Point(0, FramePoint.Y), new Point(pictureBox1.Width, FramePoint.Y));
                if ((currbits != null) && (currbits.Length > 0))
                {
                    if (currbits[trackBar2.Value] != null)
                    {
                        if (!DrawEmpty)
                        {
                            varW = 0;
                            varH = 0;
                        }
                        else
                        {
                            varW = currbits[trackBar2.Value].Width;
                            varH = currbits[trackBar2.Value].Height;
                        }
                        if (!DrawFull)
                        {
                            varFW = 0;
                            varFH = 0;
                        }
                        else
                        {
                            varFW = currbits[trackBar2.Value].Width;
                            varFH = currbits[trackBar2.Value].Height;
                        }
                        int x = FramePoint.X - edit.Frames[trackBar2.Value].Center.X;
                        int y = FramePoint.Y - edit.Frames[trackBar2.Value].Center.Y - currbits[trackBar2.Value].Height;
                        e.Graphics.FillRectangle(WhiteTrasparent, new Rectangle(x, y, varFW, varFH));
                        e.Graphics.DrawRectangle(Pens.Red, new Rectangle(x, y, varW, varH));
                        e.Graphics.DrawImage(currbits[trackBar2.Value], x, y);
                    }
                    //e.Graphics.DrawLine(Pens.Red, new Point(0, 335-(int)numericUpDown1.Value), new Point(pictureBox1.Width, 335-(int)numericUpDown1.Value));
                }
                //Draw Referencial Point Arrow
                Point point1 = new Point(418 - (int)numericUpDown2.Value, 335 - (int)numericUpDown1.Value);
                Point point2 = new Point(418 - (int)numericUpDown2.Value, 352 - (int)numericUpDown1.Value);
                Point point3 = new Point(422 - (int)numericUpDown2.Value, 348 - (int)numericUpDown1.Value);
                Point point4 = new Point(425 - (int)numericUpDown2.Value, 353 - (int)numericUpDown1.Value);
                Point point5 = new Point(427 - (int)numericUpDown2.Value, 352 - (int)numericUpDown1.Value);
                Point point6 = new Point(425 - (int)numericUpDown2.Value, 347 - (int)numericUpDown1.Value);
                Point point7 = new Point(430 - (int)numericUpDown2.Value, 347 - (int)numericUpDown1.Value);
                Point[] arrayPoints = { point1, point2, point3, point4, point5, point6, point7 };
                e.Graphics.FillPolygon(WhiteUndraw, arrayPoints);
                e.Graphics.DrawPolygon(BlackUndraw, arrayPoints);
            }
        }
        //End of Soulblighter Modification
        //Soulblighter Modification
        private void onFrameCountBarChanged(object sender, EventArgs e)
        {
            if (FileType != 0)
            {
                AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                if (edit != null)
                {
                    if (edit.Frames.Count >= trackBar2.Value)
                    {
                        if (sender == listView1) {
                            if (listView1.SelectedItems.Count == 1)
                                trackBar2.Value = (int)listView1.SelectedItems[0].Tag;
                            return;
                        }

                        numericUpDownCx.Value = edit.Frames[trackBar2.Value].Center.X;
                        numericUpDownCy.Value = edit.Frames[trackBar2.Value].Center.Y;

                        listView1.SelectedItems.Clear();
                        if ((int)listView1.Items[trackBar2.Value].Tag == trackBar2.Value) {
                            listView1.Items[trackBar2.Value].Selected = true;
                        } else foreach (ListViewItem item in listView1.Items) {
                            if ((int)item.Tag == trackBar2.Value) {
                                item.Selected = true; break;
                            }
                        }
                    }
                }
                pictureBox1.Invalidate();
            }
        }
        //End of Soulblighter Modification

        private void OnCenterXValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (FileType != 0)
                {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                    if (edit != null)
                    {
                        if (edit.Frames.Count >= trackBar2.Value)
                        {
                            FrameEdit frame = edit.Frames[trackBar2.Value];
                            if (numericUpDownCx.Value != frame.Center.X)
                            {
                                frame.ChangeCenter((int)numericUpDownCx.Value, frame.Center.Y);
                                Options.ChangedUltimaClass["Animations"] = true;
                                pictureBox1.Invalidate();
                            }
                        }
                    }
                }
            }
            catch (System.NullReferenceException) { }
        }

        private void OnCenterYValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (FileType != 0)
                {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                    if (edit != null)
                    {
                        if (edit.Frames.Count >= trackBar2.Value)
                        {
                            FrameEdit frame = edit.Frames[trackBar2.Value];
                            if (numericUpDownCy.Value != frame.Center.Y)
                            {
                                frame.ChangeCenter(frame.Center.X, (int)numericUpDownCy.Value);
                                Options.ChangedUltimaClass["Animations"] = true;
                                pictureBox1.Invalidate();
                            }
                        }
                    }
                }
            }
            catch (System.NullReferenceException) { }
        }

        private void onClickExtractImages(object sender, EventArgs e)
        {
            if (FileType == 0) return;

            ToolStripMenuItem menu = (ToolStripMenuItem)sender;
            ImageFormat format = ImageFormat.Bmp;
            if (((string)menu.Tag) == ".png")
                format = ImageFormat.Png;
            else if (((string)menu.Tag) == ".tiff")
                format = ImageFormat.Tiff;

            int body, action;
            if (treeView1.SelectedNode.Parent == null) {
                body = (int)treeView1.SelectedNode.Tag;
                action = -1;
            } else { 
                body = (int)treeView1.SelectedNode.Parent.Tag;
                action = (int)treeView1.SelectedNode.Tag;
            }
            var animlength = Animations.GetAnimLength(body, FileType);

            var path = Path.Combine(Options.ExtractedPath, "Animation", String.Format("anim{1}{0}-{2:0000}",
                                    animlength == 22 ? "H" : animlength == 13 ? "L" : "P", FileType, body));
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            if (action == -1) action = 0;
            else animlength = action + 1;

            for (int a = action; a < animlength; ++a) {
                for (int i = 0; i < 5; ++i) {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, body, a, i);
                    if (edit != null) {
                        Bitmap[] bits = edit.GetFrames(false);
                        if (bits != null) {
                            for (int j = 0; j < bits.Length; ++j) {
                                string filename = String.Format("anim{5}-{0:0000}{6}-{1}-{2}_{3}{4}", body, a, i, j, menu.Tag, FileType,
                                                                                animlength == 22 ? "H" : animlength == 13 ? "L" : "P");
                                string file = Path.Combine(path, filename);
                                Bitmap bit = new Bitmap(bits[j]);
                                if (bit != null)
                                    bit.Save(file, format);
                                bit.Dispose();
                            }
                        }
                    }
                }
            }

            MessageBox.Show(String.Format("Кадры анимации сохранены в папку \"{0}\"", path),
                   "Сохранение кадров", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void OnClickRemoveAction(object sender, EventArgs e)
        {
            if (FileType != 0)
            {
                if (treeView1.SelectedNode.Parent == null)
                {
                    DialogResult result =
                           MessageBox.Show(String.Format("Are you sure to remove animation {0}", CurrBody),
                           "Remove",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Question,
                           MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Yes)
                    {
                        treeView1.SelectedNode.ForeColor = Color.Red;
                        for (int i = 0; i < treeView1.SelectedNode.Nodes.Count; ++i)
                        {
                            treeView1.SelectedNode.Nodes[i].ForeColor = Color.Red;
                            for (int d = 0; d < 5; ++d)
                            {
                                AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, i, d);
                                if (edit != null)
                                    edit.ClearFrames();
                            }
                        }
                        if (ShowOnlyValid)
                            treeView1.SelectedNode.Remove();
                        Options.ChangedUltimaClass["Animations"] = true;
                        AfterSelectTreeView(this, null);
                    }
                }
                else
                {
                    DialogResult result =
                           MessageBox.Show(String.Format("Are you sure to remove action {0}", CurrAction),
                           "Remove",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Question,
                           MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Yes)
                    {
                        for (int i = 0; i < 5; ++i)
                        {
                            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, i);
                            if (edit != null)
                                edit.ClearFrames();
                        }
                        treeView1.SelectedNode.Parent.Nodes[CurrAction].ForeColor = Color.Red;
                        bool valid = false;
                        foreach (TreeNode node in treeView1.SelectedNode.Parent.Nodes)
                        {
                            if (node.ForeColor != Color.Red)
                            {
                                valid = true;
                                break;
                            }
                        }
                        if (!valid)
                        {
                            if (ShowOnlyValid)
                                treeView1.SelectedNode.Parent.Remove();
                            else
                                treeView1.SelectedNode.Parent.ForeColor = Color.Red;
                        }
                        Options.ChangedUltimaClass["Animations"] = true;
                        AfterSelectTreeView(this, null);
                    }
                }
            }
        }

        private void OnClickSave(object sender, EventArgs e)
        {
            if (FileType != 0)
            {
                Ultima.AnimationEdit.Save(FileType, FiddlerControls.Options.OutputPath);
                MessageBox.Show(
                        String.Format("AnimationFile saved to {0}", FiddlerControls.Options.OutputPath),
                        "Saved",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
                Options.ChangedUltimaClass["Animations"] = false;
            }
        }
        //My Soulblighter Modification
        private void OnClickRemoveFrame(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int Corrector = 0;
                int[] frameindex = new int[listView1.SelectedItems.Count];
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    frameindex[i] = (int)listView1.SelectedIndices[i] - Corrector;
                    Corrector++;
                }
                for (int i = 0; i < frameindex.Length; i++)
                {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                    if (edit != null)
                    {
                        edit.RemoveFrame(frameindex[i]);
                        listView1.Items.RemoveAt(listView1.Items.Count - 1);
                        if (edit.Frames.Count != 0)
                            trackBar2.Maximum = edit.Frames.Count - 1;
                        else
                        {
                            TreeNode node = GetNode(CurrBody);
                            trackBar2.Maximum = 0;
                        }
                        listView1.Invalidate();
                        Options.ChangedUltimaClass["Animations"] = true;
                    }
                }
            }
        }
        //End of Soulblighter Modification
        private void OnClickReplace(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    int frameindex = (int)listView1.SelectedItems[0].Tag;
                    dialog.Multiselect = false;
                    dialog.Title = String.Format("Choose image file to replace at {0}", frameindex);
                    dialog.CheckFileExists = true;
                    dialog.Filter = "image files (*.tiff;*.bmp)|*.tiff;*.bmp";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        Bitmap bmp = new Bitmap(dialog.FileName);
                        if (dialog.FileName.Contains(".bmp"))
                            bmp = Utils.ConvertBmpAnim(bmp, (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                        AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                        if (edit != null)
                        {
                            edit.ReplaceFrame(bmp, frameindex);
                            listView1.Invalidate();
                            Options.ChangedUltimaClass["Animations"] = true;
                        }
                    }
                }
            }
        }

        private void OnClickAdd(object sender, EventArgs e)
        {
            if (FileType != 0)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Multiselect = true;
                    dialog.Title = "Выбирите файлы изображений для добавления";
                    dialog.CheckFileExists = true;
                    dialog.Filter = "Gif files (*.gif;)|*.gif; |Bitmap files (*.bmp;)|*.bmp; |Tiff files (*.tiff;)|*tiff; |Png files (*.png;)|*.png; |Jpeg files (*.jpeg;*.jpg;)|*.jpeg;*.jpg;";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        listView1.BeginUpdate();
                        //My Soulblighter Modifications
                        for (int w = 0; w < dialog.FileNames.Length; w++)
                        {
                            Bitmap bmp = new Bitmap(dialog.FileNames[w]); // dialog.Filename replaced by dialog.FileNames[w]
                            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                            if (dialog.FileName.Contains(".bmp") | dialog.FileName.Contains(".tiff") | dialog.FileName.Contains(".png") | dialog.FileName.Contains(".jpeg") | dialog.FileName.Contains(".jpg"))
                            {
                                if (useCKeyFilter)
                                    bmp = Utils.CKeyFilter(bmp);
                                if (useBColFilter)
                                    bmp = Utils.BColFilter(bmp, false);

                                bmp = Utils.ConvertBmpAnim(bmp, (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                                //edit.GetImagePalette(bmp);
                            }

                            if (edit != null)
                            {
                                //Gif Especial Properties
                                if (dialog.FileName.Contains(".gif"))
                                {
                                    FrameDimension dimension = new FrameDimension(bmp.FrameDimensionsList[0]);
                                    // Number of frames 
                                    int frameCount = bmp.GetFrameCount(dimension);
                                    Bitmap[] bitbmp = new Bitmap[frameCount];
                                    bmp.SelectActiveFrame(dimension, 0);
                                    edit.GetGifPalette(bmp);
                                    progressBar1.Maximum = frameCount;
                                    // Return an Image at a certain index 
                                    for (int index = 0; index < frameCount; index++)
                                    {
                                        bitbmp[index] = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format16bppArgb1555);
                                        bmp.SelectActiveFrame(dimension, index);
                                        bitbmp[index] = (Bitmap)bmp;
                                        bitbmp[index] = Utils.ConvertBmpAnim(bitbmp[index], (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                                        edit.AddFrame(bitbmp[index]);
                                        TreeNode node = GetNode(CurrBody);
                                        if (node != null)
                                        {
                                            node.ForeColor = Color.Black;
                                            node.Nodes[CurrAction].ForeColor = Color.Black;
                                        }
                                        ListViewItem item;
                                        int i = edit.Frames.Count - 1;
                                        item = new ListViewItem(i.ToString(), 0);
                                        item.Tag = i;
                                        listView1.Items.Add(item);
                                        int width = listView1.TileSize.Width - 5;
                                        if (bmp.Width > listView1.TileSize.Width)
                                            width = bmp.Width;
                                        int height = listView1.TileSize.Height - 5;
                                        if (bmp.Height > listView1.TileSize.Height)
                                            height = bmp.Height;

                                        listView1.TileSize = new Size(width + 5, height + 5);
                                        trackBar2.Maximum = i;
                                        Options.ChangedUltimaClass["Animations"] = true;
                                        if (progressBar1.Value < progressBar1.Maximum)
                                        {
                                            progressBar1.Value++;
                                            progressBar1.Invalidate();
                                        }
                                    }
                                    progressBar1.Value = 0;
                                    progressBar1.Invalidate();
                                    SetPaletteBox();
                                    numericUpDownCx.Value = edit.Frames[trackBar2.Value].Center.X;
                                    numericUpDownCy.Value = edit.Frames[trackBar2.Value].Center.Y;
                                    Options.ChangedUltimaClass["Animations"] = true;
                                }
                                //End of Soulblighter Modifications
                                else
                                {
                                    edit.AddFrame(bmp);
                                    TreeNode node = GetNode(CurrBody);
                                    if (node != null)
                                    {
                                        node.ForeColor = Color.Black;
                                        node.Nodes[CurrAction].ForeColor = Color.Black;
                                    }
                                    ListViewItem item;
                                    int i = edit.Frames.Count - 1;
                                    item = new ListViewItem(i.ToString(), 0);
                                    item.Tag = i;
                                    listView1.Items.Add(item);
                                    int width = listView1.TileSize.Width - 5;
                                    if (bmp.Width > listView1.TileSize.Width)
                                        width = bmp.Width;
                                    int height = listView1.TileSize.Height - 5;
                                    if (bmp.Height > listView1.TileSize.Height)
                                        height = bmp.Height;

                                    listView1.TileSize = new Size(width + 5, height + 5);
                                    trackBar2.Maximum = i;
                                    numericUpDownCx.Value = edit.Frames[trackBar2.Value].Center.X;
                                    numericUpDownCy.Value = edit.Frames[trackBar2.Value].Center.Y;
                                    Options.ChangedUltimaClass["Animations"] = true;
                                }
                            }
                        }
                        listView1.EndUpdate();
                        listView1.Invalidate();
                    }
                }
            }
            //Refresh List
            CurrDir = trackBar1.Value;
            AfterSelectTreeView(null, null);
        }

        private void onClickExtractPalette(object sender, EventArgs e)
        {
            if (FileType != 0)
            {
                ToolStripMenuItem menu = (ToolStripMenuItem)sender;
                AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                if (edit != null)
                {
                    string name = String.Format("palette_anim{0}_{1}_{2}_{3}", FileType, CurrBody, CurrAction, CurrDir);
                    var extractFolder = Path.Combine(Options.ExtractedPath, "AnimPalette");
                    if (!Directory.Exists(extractFolder)) Directory.CreateDirectory(extractFolder);
                    if (((string)menu.Tag) == "pal") {
                        edit.SavePalPalette(Path.Combine(extractFolder, name+".pal"));
                    } else if (((string)menu.Tag) == "txt") {
                        edit.ExportPalette(Path.Combine(extractFolder, name + ".txt"), 0);
                    } else {
                        string path = Path.Combine(extractFolder, name + "." + (string)menu.Tag);
                        edit.ExportPalette(path, (((string)menu.Tag) == "bmp") ? 1 : 2);
                    }
                    MessageBox.Show(String.Format("Палитра сохранена в директорию \"{0}\"", extractFolder),
                        "Сохранение палитры", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void onClickLoadPalette(object sender, EventArgs e)
        {
            if (FileType == 0) return;
            using (OpenFileDialog dialog = new OpenFileDialog()) {
                dialog.Multiselect = false;
                dialog.Title = "Выбирите файл палитры";
                dialog.CheckFileExists = true;
                dialog.Filter = "палитра (*.pal)|*.pal";
                if (dialog.ShowDialog() == DialogResult.OK) {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                    if (edit != null) {
                        edit.GetPalPalette(dialog.FileName);
                        SetPaletteBox();
                        listView1.Invalidate();
                        Options.ChangedUltimaClass["Animations"] = true;
                    }
                }
            }
        }

        private void onClickImportPalette(object sender, EventArgs e)
        {
            if (FileType != 0)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Multiselect = false;
                    dialog.Title = "Choose palette file";
                    dialog.CheckFileExists = true;
                    dialog.Filter = "txt files (*.txt)|*.txt";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                        if (edit != null)
                        {
                            using (StreamReader sr = new StreamReader(dialog.FileName))
                            {
                                string line;
                                ushort[] Palette = new ushort[0x100];
                                int i = 0;
                                while ((line = sr.ReadLine()) != null)
                                {
                                    if ((line = line.Trim()).Length == 0 || line.StartsWith("#"))
                                        continue;
                                    Palette[i++] = ushort.Parse(line);
                                    //My Soulblighter Modification
                                    if (Palette[i++] == 32768)
                                        Palette[i++] = 32769;
                                    //End of Soulblighter Modification
                                    if (i >= 0x100)
                                        break;
                                }
                                edit.ReplacePalette(Palette);
                            }
                            SetPaletteBox();
                            listView1.Invalidate();
                            Options.ChangedUltimaClass["Animations"] = true;
                        }
                    }
                }
            }
        }

        private void OnClickImportFromVD(object sender, EventArgs e)
        {
            if (FileType != 0)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Multiselect = false;
                    dialog.Title = "Choose palette file";
                    dialog.CheckFileExists = true;
                    dialog.Filter = "vd files (*.vd)|*.vd";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        int animlength = Animations.GetAnimLength(CurrBody, FileType);
                        int currtype = animlength == 22 ? 0 : animlength == 13 ? 1 : 2;
                        using (FileStream fs = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            using (BinaryReader bin = new BinaryReader(fs))
                            {
                                int filetype = bin.ReadInt16();
                                int animtype = bin.ReadInt16();
                                if (filetype != 6)
                                {
                                    MessageBox.Show(
                                        "Not an Anim File.",
                                        "Import",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information,
                                        MessageBoxDefaultButton.Button1);
                                    return;
                                }

                                if (animtype != currtype)
                                {
                                    MessageBox.Show(
                                        "Wrong Anim Id ( Type )",
                                        "Import",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information,
                                        MessageBoxDefaultButton.Button1);
                                    return;
                                }
                                Ultima.AnimationEdit.LoadFromVD(FileType, CurrBody, bin);
                            }
                        }

                        bool valid = false;
                        TreeNode node = GetNode(CurrBody);
                        if (node != null)
                        {
                            for (int j = 0; j < animlength; ++j)
                            {
                                if (Ultima.AnimationEdit.IsActionDefinied(FileType, CurrBody, j))
                                {
                                    node.Nodes[j].ForeColor = Color.Black;
                                    valid = true;
                                }
                                else
                                    node.Nodes[j].ForeColor = Color.Red;
                            }
                            if (valid)
                                node.ForeColor = Color.Black;
                            else
                                node.ForeColor = Color.Red;
                        }

                        Options.ChangedUltimaClass["Animations"] = true;
                        AfterSelectTreeView(this, null);
                        MessageBox.Show(
                                        "Finished",
                                        "Import",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information,
                                        MessageBoxDefaultButton.Button1);
                    }
                }
            }
        }

        private void OnClickExportToVD(object sender, EventArgs e)
        {
            if (FileType == 0) return;

            string path = FiddlerControls.Options.OutputPath;
            string FileName = Path.Combine(path, String.Format("anim{0}_0x{1:X}.vd", FileType, CurrBody));
            Ultima.AnimationEdit.ExportToVD(FileType, CurrBody, FileName);
            MessageBox.Show(String.Format("Анимация сохранена как \"{0}\"", FiddlerControls.Options.OutputPath),
                    "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private static string dialogSelected_ImportFromAVInGIF = Path.Combine(Options.ExtractedPath, "Animation", "656", "54477");
        private void OnClickImportFromAVInGIF(object sender, EventArgs e)
        {
            if (FileType == 0) return;
            var ext = (string)(sender as ToolStripMenuItem).Tag;

            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.ShowNewFolderButton = false;
                while (!Directory.Exists(dialogSelected_ImportFromAVInGIF) && dialogSelected_ImportFromAVInGIF != Directory.GetDirectoryRoot(dialogSelected_ImportFromAVInGIF))
                    dialogSelected_ImportFromAVInGIF = Path.GetDirectoryName(dialogSelected_ImportFromAVInGIF);
                dialog.SelectedPath = dialogSelected_ImportFromAVInGIF;
                dialog.Description = "Выбирите папку содержащую видео файлы анимации в формате *.avi. По возможности будет использованна палитра из одноименных файлов в формате *.pal";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    int body = (int)(treeView1.SelectedNode.Parent ?? treeView1.SelectedNode).Tag;
                    int animlength = Animations.GetAnimLength(body, FileType);
                    var folder = Path.GetFileName(dialogSelected_ImportFromAVInGIF = dialog.SelectedPath);
                    int curanim = folder.Length >= 6 ? folder.ToUpper()[5] == 'H' ? 22 : folder.ToUpper()[5] == 'L' ? 13 : folder.ToUpper()[5] == 'P' ? 34 : -1 : -1;
                    if (curanim != animlength)
                        MessageBox.Show("Не подходящий тип анимации", "Импорт", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                    for (int a = 0; a < animlength; ++a)
                    {
                        for (int i = 0; i < 5; ++i)
                        {
                            var files = Directory.GetFiles(dialog.SelectedPath, String.Format("{0:00}-{1}*.{2}", a, i, ext));
                            if (files != null && files.Length == 1) {
                                Ultima.AnimationEdit.GetAnimation(FileType, body, a, i).ImportFromFile(files[0]);
                            } else {
                                Ultima.AnimationEdit.GetAnimation(FileType, body, a, i).ClearFrames();
                            }
                        }
                    }

                    // ------
                    bool valid = false;
                    TreeNode node = GetNode(CurrBody);
                    if (node != null) {
                        for (int j = 0; j < animlength; ++j) {
                            if (Ultima.AnimationEdit.IsActionDefinied(FileType, CurrBody, j))  {
                                node.Nodes[j].ForeColor = Color.Black;
                                valid = true;
                            } else
                                node.Nodes[j].ForeColor = Color.Red;
                        }
                        if (valid)
                            node.ForeColor = Color.Black;
                        else
                            node.ForeColor = Color.Red;
                    }

                    Options.ChangedUltimaClass["Animations"] = true;
                    AfterSelectTreeView(this, null);
                    MessageBox.Show("Finished", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void OnClickExportToAVInGIF(object sender, EventArgs e)
        {
            if (FileType == 0) return;
            var ext = (string)(sender as ToolStripMenuItem).Tag;

            int body, action;
            if (treeView1.SelectedNode.Parent == null) {
                body = (int)treeView1.SelectedNode.Tag;
                action = -1;
            } else { 
                body = (int)treeView1.SelectedNode.Parent.Tag;
                action = (int)treeView1.SelectedNode.Tag;
            }
            var animlength = Animations.GetAnimLength(body, FileType);

            var path = Path.Combine(Options.ExtractedPath, "Animation", String.Format("anim{1}{0}-{2:0000}",
                                    animlength == 22 ? "H" : animlength == 13 ? "L" : "P", FileType, body));
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            if (action == -1) action = 0;
            else animlength = action + 1;   
            for (int a = action; a < animlength; ++a) {
                for (int i = 0; i < 5; ++i) {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, body, a, i);
                    if (edit != null) {
                        var file = Path.Combine(path, String.Format("{0:00}-{1}.{2}", a, i, ext));
                        switch (ext) {
                            case "avi" : edit.ExportToAVI(file, true); break;
                            case "gif" : edit.ExportToGIF(file); break;
                            default: throw new ArgumentException();
                        }
                    }
                }
            }

            if (sender != exportAllToAVIToolStripMenuItem)
                MessageBox.Show(String.Format("Анимация сохранена как \"{0}\"", path),
                        "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void OnClickShowOnlyValid(object sender, EventArgs e)
        {
            ShowOnlyValid = !ShowOnlyValid;
            if (ShowOnlyValid)
            {
                treeView1.BeginUpdate();
                for (int i = treeView1.Nodes.Count - 1; i >= 0; --i)
                {
                    if (treeView1.Nodes[i].ForeColor == Color.Red)
                        treeView1.Nodes[i].Remove();
                }
                treeView1.EndUpdate();
            }
            else
                OnLoad(null);
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {

        }
        //My Soulblighter Modification
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileType != 0)
                {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                    if (edit != null)
                    {
                        if (edit.Frames.Count >= trackBar2.Value)
                        {
                            FrameEdit[] frame = new FrameEdit[edit.Frames.Count];
                            for (int Index = 0; Index < edit.Frames.Count; Index++)
                            {
                                frame[Index] = edit.Frames[Index];
                                frame[Index].ChangeCenter((int)numericUpDownCx.Value, (int)numericUpDownCy.Value);
                                Options.ChangedUltimaClass["Animations"] = true;
                                pictureBox1.Invalidate();
                            }
                        }
                    }
                }
            }
            catch (System.NullReferenceException) { }
        }
        //End of Soulblighter Modification

        //My Soulblighter Modification
        private void fromGifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileType != 0)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Multiselect = false;
                    dialog.Title = "Choose palette file";
                    dialog.CheckFileExists = true;
                    dialog.Filter = "Gif files (*.gif)|*.gif";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        Bitmap bit = new Bitmap(dialog.FileName);
                        AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                        if (edit != null)
                        {
                            FrameDimension dimension = new FrameDimension(bit.FrameDimensionsList[0]);
                            // Number of frames 
                            int frameCount = bit.GetFrameCount(dimension);
                            bit.SelectActiveFrame(dimension, 0);
                            edit.GetGifPalette(bit);
                            SetPaletteBox();
                            listView1.Invalidate();
                            Options.ChangedUltimaClass["Animations"] = true;
                        }
                    }
                }
            }
        }

        private void ReferencialPointX(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }
        private void ReferencialPointY(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        static bool lockbutton = false; //Lock button variable
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!lockbutton && toolStripButton6.Enabled)
            {
                numericUpDown2.Value = 418 - e.X;
                numericUpDown1.Value = 335 - e.Y;
                pictureBox1.Invalidate();
            }
        }
        //Change center of frame on key press
        private void txtSendData_KeyDown(object sender, KeyEventArgs e)
        {
            if (timer1.Enabled == false)
            {
                if (e.KeyCode == Keys.Right)
                {
                    numericUpDownCx.Value--;
                    numericUpDownCx.Invalidate();
                }
                else if (e.KeyCode == Keys.Left)
                {
                    numericUpDownCx.Value++;
                    numericUpDownCx.Invalidate();
                }
                else if (e.KeyCode == Keys.Up)
                {
                    numericUpDownCy.Value++;
                    numericUpDownCy.Invalidate();
                }
                else if (e.KeyCode == Keys.Down)
                {
                    numericUpDownCy.Value--;
                    numericUpDownCy.Invalidate();
                }
                pictureBox1.Invalidate();
            }
        }
        //Change center of Referencial Point on key press
        private void txtSendData_KeyDown2(object sender, KeyEventArgs e)
        {
            if (lockbutton == false && toolStripButton6.Enabled == true)
            {
                if (e.KeyCode == Keys.Right)
                {
                    numericUpDown2.Value--;
                    numericUpDown2.Invalidate();
                }
                else if (e.KeyCode == Keys.Left)
                {
                    numericUpDown2.Value++;
                    numericUpDown2.Invalidate();
                }
                else if (e.KeyCode == Keys.Up)
                {
                    numericUpDown1.Value++;
                    numericUpDown1.Invalidate();
                }
                else if (e.KeyCode == Keys.Down)
                {
                    numericUpDown1.Value--;
                    numericUpDown1.Invalidate();
                }
                pictureBox1.Invalidate();
            }
        }
        //Lock Button
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            lockbutton = !lockbutton;
            numericUpDown2.Enabled = !lockbutton;
            numericUpDown1.Enabled = !lockbutton;
        }
        //Add in all Directions
        private void allDirectionsAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileType != 0)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Multiselect = true;
                    dialog.Title = "Choose 5 Gifs to add";
                    dialog.CheckFileExists = true;
                    dialog.Filter = "Gif files (*.gif;)|*.gif;";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        trackBar1.Enabled = false;
                        if (dialog.FileNames.Length == 5)
                        {
                            trackBar1.Value = 0;
                            for (int w = 0; w < dialog.FileNames.Length; w++)
                            {
                                if (w < 5)
                                {
                                    Bitmap bmp = new Bitmap(dialog.FileNames[w]); // dialog.Filename replaced by dialog.FileNames[w]
                                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                                    if (edit != null)
                                    {
                                        //Gif Especial Properties
                                        if (dialog.FileName.Contains(".gif"))
                                        {
                                            FrameDimension dimension = new FrameDimension(bmp.FrameDimensionsList[0]);
                                            // Number of frames 
                                            int frameCount = bmp.GetFrameCount(dimension);
                                            progressBar1.Maximum = frameCount;
                                            bmp.SelectActiveFrame(dimension, 0);
                                            edit.GetGifPalette(bmp);
                                            Bitmap[] bitbmp = new Bitmap[frameCount];
                                            // Return an Image at a certain index 
                                            for (int index = 0; index < frameCount; index++)
                                            {
                                                bitbmp[index] = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format16bppArgb1555);
                                                bmp.SelectActiveFrame(dimension, index);
                                                bitbmp[index] = (Bitmap)bmp;
                                                bitbmp[index] = Utils.ConvertBmpAnim(bitbmp[index], (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                                                edit.AddFrame(bitbmp[index]);
                                                TreeNode node = GetNode(CurrBody);
                                                if (node != null)
                                                {
                                                    node.ForeColor = Color.Black;
                                                    node.Nodes[CurrAction].ForeColor = Color.Black;
                                                }
                                                ListViewItem item;
                                                int i = edit.Frames.Count - 1;
                                                item = new ListViewItem(i.ToString(), 0);
                                                item.Tag = i;
                                                listView1.Items.Add(item);
                                                int width = listView1.TileSize.Width - 5;
                                                if (bmp.Width > listView1.TileSize.Width)
                                                    width = bmp.Width;
                                                int height = listView1.TileSize.Height - 5;
                                                if (bmp.Height > listView1.TileSize.Height)
                                                    height = bmp.Height;

                                                listView1.TileSize = new Size(width + 5, height + 5);
                                                trackBar2.Maximum = i;
                                                Options.ChangedUltimaClass["Animations"] = true;
                                                if (progressBar1.Value < progressBar1.Maximum)
                                                {
                                                    progressBar1.Value++;
                                                    progressBar1.Invalidate();
                                                }
                                            }
                                            progressBar1.Value = 0;
                                            progressBar1.Invalidate();
                                            SetPaletteBox();
                                            listView1.Invalidate();
                                            numericUpDownCx.Value = edit.Frames[trackBar2.Value].Center.X;
                                            numericUpDownCy.Value = edit.Frames[trackBar2.Value].Center.Y;
                                            Options.ChangedUltimaClass["Animations"] = true;
                                        }
                                    }
                                    if ((w < 4) & (w < dialog.FileNames.Length - 1))
                                        trackBar1.Value++;
                                }
                            }
                        }
                        //Looping if dialog.FileNames.Length != 5
                        while (dialog.FileNames.Length != 5)
                        {
                            if (dialog.ShowDialog() == DialogResult.Cancel)
                                break;
                            if (dialog.FileNames.Length != 5)
                                dialog.ShowDialog();
                            if (dialog.FileNames.Length == 5)
                            {
                                trackBar1.Value = 0;
                                for (int w = 0; w < dialog.FileNames.Length; w++)
                                {
                                    if (w < 5)
                                    {
                                        Bitmap bmp = new Bitmap(dialog.FileNames[w]); // dialog.Filename replaced by dialog.FileNames[w]
                                        AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                                        if (edit != null)
                                        {
                                            //Gif Especial Properties
                                            if (dialog.FileName.Contains(".gif"))
                                            {
                                                FrameDimension dimension = new FrameDimension(bmp.FrameDimensionsList[0]);
                                                // Number of frames 
                                                int frameCount = bmp.GetFrameCount(dimension);
                                                progressBar1.Maximum = frameCount;
                                                bmp.SelectActiveFrame(dimension, 0);
                                                edit.GetGifPalette(bmp);
                                                Bitmap[] bitbmp = new Bitmap[frameCount];
                                                // Return an Image at a certain index 
                                                for (int index = 0; index < frameCount; index++)
                                                {
                                                    bitbmp[index] = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format16bppArgb1555);
                                                    bmp.SelectActiveFrame(dimension, index);
                                                    bitbmp[index] = (Bitmap)bmp;
                                                    bitbmp[index] = Utils.ConvertBmpAnim(bitbmp[index], (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                                                    edit.AddFrame(bitbmp[index]);
                                                    TreeNode node = GetNode(CurrBody);
                                                    if (node != null)
                                                    {
                                                        node.ForeColor = Color.Black;
                                                        node.Nodes[CurrAction].ForeColor = Color.Black;
                                                    }
                                                    ListViewItem item;
                                                    int i = edit.Frames.Count - 1;
                                                    item = new ListViewItem(i.ToString(), 0);
                                                    item.Tag = i;
                                                    listView1.Items.Add(item);
                                                    int width = listView1.TileSize.Width - 5;
                                                    if (bmp.Width > listView1.TileSize.Width)
                                                        width = bmp.Width;
                                                    int height = listView1.TileSize.Height - 5;
                                                    if (bmp.Height > listView1.TileSize.Height)
                                                        height = bmp.Height;

                                                    listView1.TileSize = new Size(width + 5, height + 5);
                                                    trackBar2.Maximum = i;
                                                    Options.ChangedUltimaClass["Animations"] = true;
                                                    if (progressBar1.Value < progressBar1.Maximum)
                                                    {
                                                        progressBar1.Value++;
                                                        progressBar1.Invalidate();
                                                    }
                                                }
                                                progressBar1.Value = 0;
                                                progressBar1.Invalidate();
                                                SetPaletteBox();
                                                listView1.Invalidate();
                                                numericUpDownCx.Value = edit.Frames[trackBar2.Value].Center.X;
                                                numericUpDownCy.Value = edit.Frames[trackBar2.Value].Center.Y;
                                                Options.ChangedUltimaClass["Animations"] = true;
                                            }
                                        }
                                        if ((w < 4) & (w < dialog.FileNames.Length - 1))
                                            trackBar1.Value++;
                                    }
                                }
                            }
                        }
                        trackBar1.Enabled = true;
                    }
                }
            }
            //Refresh List
            CurrDir = trackBar1.Value;
            AfterSelectTreeView(null, null);
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            DrawEmpty = !DrawEmpty;
            pictureBox1.Invalidate();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            DrawFull = !DrawFull;
            pictureBox1.Invalidate();
        }
        //Closing window
        private void AnimationEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            DrawFull = false;
            DrawEmpty = false;
            lockbutton = false;
            timer1.Enabled = false;
            BlackUndraw = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
            WhiteUndraw = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
            Loaded = false;
            FiddlerControls.Events.FilePathChangeEvent -= new FiddlerControls.Events.FilePathChangeHandler(OnFilePathChangeEvent);
        }
        //Play Button Timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (trackBar2.Value < trackBar2.Maximum)
                trackBar2.Value++;
            else
                trackBar2.Value = 0;
            pictureBox1.Invalidate();
        }
        //Play Button
        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
                numericUpDownCx.Enabled = true;
                numericUpDownCy.Enabled = true;
                trackBar2.Enabled = true;
                button2.Enabled = true; //Same Center button
                toolStripButton12.Enabled = true; //Undraw Referencial Point button
                if (toolStripButton12.Checked)
                {
                    toolStripButton6.Enabled = false; //Lock button
                    BlackUndraw = new Pen(Color.FromArgb(0, 0, 0, 0), 1);
                    WhiteUndraw = new SolidBrush(Color.FromArgb(0, 255, 255, 255));
                }
                else
                {
                    toolStripButton6.Enabled = true;
                    BlackUndraw = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
                    WhiteUndraw = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
                }
                if (toolStripButton6.Checked || toolStripButton12.Checked)
                {
                    numericUpDown2.Enabled = false;
                    numericUpDown1.Enabled = false;
                }
                else
                {
                    numericUpDown2.Enabled = true;
                    numericUpDown1.Enabled = true;
                }

            }
            else
            {
                timer1.Enabled = true;
                numericUpDownCx.Enabled = false;
                numericUpDownCy.Enabled = false;
                numericUpDown2.Enabled = false;
                numericUpDown1.Enabled = false;
                trackBar2.Enabled = false;
                button2.Enabled = false; //Same Center button
                toolStripButton12.Enabled = false; //Undraw Referencial Point button
                toolStripButton6.Enabled = false; //Lock button
                BlackUndraw = new Pen(Color.FromArgb(0, 0, 0, 0), 1);
                WhiteUndraw = new SolidBrush(Color.FromArgb(0, 255, 255, 255));
            }
            pictureBox1.Invalidate();
        }
        //Animation Speed
        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = 50 + (trackBar3.Value * 30);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!toolStripButton1.Checked || e.Button != MouseButtons.Left) return;
            numericUpDownCx.Value += m_OldLocation.X - e.Location.X;
            numericUpDownCy.Value += m_OldLocation.Y - e.Location.Y;
            m_OldLocation = e.Location;
        }

        private Point m_OldLocation;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!toolStripButton1.Checked) return;
            pictureBox1.Cursor = Cursors.SizeAll;
            m_OldLocation = e.Location;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!toolStripButton1.Checked) return;
            pictureBox1.Cursor = Cursors.Default;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            if (!toolStripButton1.Checked) return;
            pictureBox1.Cursor = Cursors.Default;
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (toolStripButton1.Checked) {
                toolStripButton12.Checked = true;
                toolStripButton12_Click(sender, e);
            }
            if (!toolStripButton1.Checked)
                pictureBox1.Cursor = Cursors.Default;
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            if (!toolStripButton12.Checked) {
                toolStripButton1.Checked = false;
                toolStripButton1_Click(sender, e);
            }
            if (!toolStripButton12.Checked)
            {
                BlackUndraw = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
                WhiteUndraw = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
                toolStripButton6.Enabled = true;
                if (toolStripButton6.Checked)
                {
                    numericUpDown2.Enabled = false;
                    numericUpDown1.Enabled = false;
                }
                else
                {
                    numericUpDown2.Enabled = true;
                    numericUpDown1.Enabled = true;
                }
            }
            else
            {
                BlackUndraw = new Pen(Color.FromArgb(0, 0, 0, 0), 1);
                WhiteUndraw = new SolidBrush(Color.FromArgb(0, 255, 255, 255));
                toolStripButton6.Enabled = false;
                numericUpDown2.Enabled = false;
                numericUpDown1.Enabled = false;
            }
            pictureBox1.Invalidate();
        }
        //All Directions with Canvas
        private void allDirectionsAddWithCanvasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileType != 0)
                {
                    using (OpenFileDialog dialog = new OpenFileDialog())
                    {
                        dialog.Multiselect = true;
                        dialog.Title = "Choose 5 Gifs to add";
                        dialog.CheckFileExists = true;
                        dialog.Filter = "Gif files (*.gif;)|*.gif;";
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            Color CustomConvert = Color.FromArgb(255, (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                            trackBar1.Enabled = false;
                            if (dialog.FileNames.Length == 5)
                            {
                                trackBar1.Value = 0;
                                for (int w = 0; w < dialog.FileNames.Length; w++)
                                {
                                    if (w < 5)
                                    {
                                        Bitmap bmp = new Bitmap(dialog.FileNames[w]); // dialog.Filename replaced by dialog.FileNames[w]
                                        AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                                        if (edit != null)
                                        {
                                            //Gif Especial Properties
                                            if (dialog.FileName.Contains(".gif"))
                                            {
                                                FrameDimension dimension = new FrameDimension(bmp.FrameDimensionsList[0]);
                                                // Number of frames 
                                                int frameCount = bmp.GetFrameCount(dimension);
                                                progressBar1.Maximum = frameCount;
                                                bmp.SelectActiveFrame(dimension, 0);
                                                edit.GetGifPalette(bmp);
                                                Bitmap[] bitbmp = new Bitmap[frameCount];
                                                // Return an Image at a certain index 
                                                for (int index = 0; index < frameCount; index++)
                                                {
                                                    bitbmp[index] = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format16bppArgb1555);
                                                    bmp.SelectActiveFrame(dimension, index);
                                                    bitbmp[index] = (Bitmap)bmp;
                                                }
                                                //Canvas algorithm
                                                int top = 0;
                                                int bot = 0;
                                                int left = 0;
                                                int right = 0;
                                                int RegressT = -1;
                                                int RegressB = -1;
                                                int RegressL = -1;
                                                int RegressR = -1;
                                                bool var = true;
                                                bool breakOK = false;
                                                //Top
                                                for (int Yf = 0; Yf < bitbmp[0].Height; Yf++)
                                                {
                                                    for (int fram = 0; fram < frameCount; fram++)
                                                    {
                                                        bitbmp[fram].SelectActiveFrame(dimension, fram);
                                                        for (int Xf = 0; Xf < bitbmp[0].Width; Xf++)
                                                        {
                                                            Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                            if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                                var = true;
                                                            if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                            {
                                                                var = false;
                                                                if (Yf != 0)
                                                                {
                                                                    RegressT++;
                                                                    Yf -= 1;
                                                                    Xf = -1;
                                                                    fram = 0;
                                                                }
                                                                else
                                                                {
                                                                    breakOK = true;
                                                                    break;
                                                                }
                                                            }
                                                            if (var == true && Xf == bitbmp[fram].Width - 1 && fram == frameCount - 1 && RegressT == -1 && Yf < bitbmp[0].Height - 9)
                                                                top += 10;
                                                            if (var == true && Xf == bitbmp[fram].Width - 1 && fram == frameCount - 1 && RegressT == -1 && Yf >= bitbmp[0].Height - 9)
                                                                top++;
                                                            if (var == true && Xf == bitbmp[fram].Width - 1 && fram == frameCount - 1 && RegressT != -1)
                                                            {
                                                                top = top - RegressT;
                                                                breakOK = true;
                                                                break;
                                                            }
                                                        }
                                                        if (breakOK == true)
                                                            break;
                                                    }
                                                    if (Yf < bitbmp[0].Height - 9)
                                                        Yf += 9; // 1 of for + 9
                                                    if (breakOK == true)
                                                    {
                                                        breakOK = false;
                                                        break;
                                                    }
                                                }
                                                //Bot
                                                for (int Yf = bitbmp[0].Height - 1; Yf > 0; Yf--)
                                                {
                                                    for (int fram = 0; fram < frameCount; fram++)
                                                    {
                                                        bitbmp[fram].SelectActiveFrame(dimension, fram);
                                                        for (int Xf = 0; Xf < bitbmp[0].Width; Xf++)
                                                        {
                                                            Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                            if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                                var = true;
                                                            if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                            {
                                                                var = false;
                                                                if (Yf != bitbmp[0].Height - 1)
                                                                {
                                                                    RegressB++;
                                                                    Yf += 1;
                                                                    Xf = -1;
                                                                    fram = 0;
                                                                }
                                                                else
                                                                {
                                                                    breakOK = true;
                                                                    break;
                                                                }
                                                            }
                                                            if (var == true && Xf == bitbmp[fram].Width - 1 && fram == frameCount - 1 && RegressB == -1 && Yf > 9)
                                                                bot += 10;
                                                            if (var == true && Xf == bitbmp[fram].Width - 1 && fram == frameCount - 1 && RegressB == -1 && Yf <= 9)
                                                                bot++;
                                                            if (var == true && Xf == bitbmp[fram].Width - 1 && fram == frameCount - 1 && RegressB != -1)
                                                            {
                                                                bot = bot - RegressB;
                                                                breakOK = true;
                                                                break;
                                                            }
                                                        }
                                                        if (breakOK == true)
                                                            break;
                                                    }
                                                    if (Yf > 9)
                                                        Yf -= 9; // 1 of for + 9
                                                    if (breakOK == true)
                                                    {
                                                        breakOK = false;
                                                        break;
                                                    }
                                                }
                                                //Left

                                                for (int Xf = 0; Xf < bitbmp[0].Width; Xf++)
                                                {
                                                    for (int fram = 0; fram < frameCount; fram++)
                                                    {
                                                        bitbmp[fram].SelectActiveFrame(dimension, fram);
                                                        for (int Yf = 0; Yf < bitbmp[0].Height; Yf++)
                                                        {
                                                            Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                            if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                                var = true;
                                                            if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                            {
                                                                var = false;
                                                                if (Xf != 0)
                                                                {
                                                                    RegressL++;
                                                                    Xf -= 1;
                                                                    Yf = -1;
                                                                    fram = 0;
                                                                }
                                                                else
                                                                {
                                                                    breakOK = true;
                                                                    break;
                                                                }
                                                            }
                                                            if (var == true && Yf == bitbmp[fram].Height - 1 && fram == frameCount - 1 && RegressL == -1 && Xf < bitbmp[0].Width - 9)
                                                                left += 10;
                                                            if (var == true && Yf == bitbmp[fram].Height - 1 && fram == frameCount - 1 && RegressL == -1 && Xf >= bitbmp[0].Width - 9)
                                                                left++;
                                                            if (var == true && Yf == bitbmp[fram].Height - 1 && fram == frameCount - 1 && RegressL != -1)
                                                            {
                                                                left = left - RegressL;
                                                                breakOK = true;
                                                                break;
                                                            }
                                                        }
                                                        if (breakOK == true)
                                                            break;
                                                    }
                                                    if (Xf < bitbmp[0].Width - 9)
                                                        Xf += 9; // 1 of for + 9
                                                    if (breakOK == true)
                                                    {
                                                        breakOK = false;
                                                        break;
                                                    }
                                                }
                                                //Right
                                                for (int Xf = bitbmp[0].Width - 1; Xf > 0; Xf--)
                                                {
                                                    for (int fram = 0; fram < frameCount; fram++)
                                                    {
                                                        bitbmp[fram].SelectActiveFrame(dimension, fram);
                                                        for (int Yf = 0; Yf < bitbmp[0].Height; Yf++)
                                                        {
                                                            Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                            if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                                var = true;
                                                            if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                            {
                                                                var = false;
                                                                if (Xf != bitbmp[0].Width - 1)
                                                                {
                                                                    RegressR++;
                                                                    Xf += 1;
                                                                    Yf = -1;
                                                                    fram = 0;
                                                                }
                                                                else
                                                                {
                                                                    breakOK = true;
                                                                    break;
                                                                }
                                                            }
                                                            if (var == true && Yf == bitbmp[fram].Height - 1 && fram == frameCount - 1 && RegressR == -1 && Xf > 9)
                                                                right += 10;
                                                            if (var == true && Yf == bitbmp[fram].Height - 1 && fram == frameCount - 1 && RegressR == -1 && Xf <= 9)
                                                                right++;
                                                            if (var == true && Yf == bitbmp[fram].Height - 1 && fram == frameCount - 1 && RegressR != -1)
                                                            {
                                                                right = right - RegressR;
                                                                breakOK = true;
                                                                break;
                                                            }
                                                        }
                                                        if (breakOK == true)
                                                            break;
                                                    }
                                                    if (Xf > 9)
                                                        Xf -= 9; // 1 of for + 9
                                                    if (breakOK == true)
                                                    {
                                                        breakOK = false;
                                                        break;
                                                    }
                                                }

                                                for (int index = 0; index < frameCount; index++)
                                                {
                                                    Rectangle rect = new Rectangle(left, top, (bitbmp[index].Width - left - right), (bitbmp[index].Height - top - bot));
                                                    bitbmp[index].SelectActiveFrame(dimension, index);
                                                    Bitmap myImage2 = bitbmp[index].Clone(rect, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);
                                                    bitbmp[index] = myImage2;
                                                }

                                                //End of Canvas
                                                for (int index = 0; index < frameCount; index++)
                                                {
                                                    bitbmp[index].SelectActiveFrame(dimension, index);
                                                    bitbmp[index] = Utils.ConvertBmpAnim(bitbmp[index], (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                                                    edit.AddFrame(bitbmp[index]);
                                                    TreeNode node = GetNode(CurrBody);
                                                    if (node != null)
                                                    {
                                                        node.ForeColor = Color.Black;
                                                        node.Nodes[CurrAction].ForeColor = Color.Black;
                                                    }
                                                    ListViewItem item;
                                                    int i = edit.Frames.Count - 1;
                                                    item = new ListViewItem(i.ToString(), 0);
                                                    item.Tag = i;
                                                    listView1.Items.Add(item);
                                                    int width = listView1.TileSize.Width - 5;
                                                    if (bmp.Width > listView1.TileSize.Width)
                                                        width = bmp.Width;
                                                    int height = listView1.TileSize.Height - 5;
                                                    if (bmp.Height > listView1.TileSize.Height)
                                                        height = bmp.Height;

                                                    listView1.TileSize = new Size(width + 5, height + 5);
                                                    trackBar2.Maximum = i;
                                                    Options.ChangedUltimaClass["Animations"] = true;
                                                    if (progressBar1.Value < progressBar1.Maximum)
                                                    {
                                                        progressBar1.Value++;
                                                        progressBar1.Invalidate();
                                                    }
                                                }
                                                progressBar1.Value = 0;
                                                progressBar1.Invalidate();
                                                SetPaletteBox();
                                                listView1.Invalidate();
                                                numericUpDownCx.Value = edit.Frames[trackBar2.Value].Center.X;
                                                numericUpDownCy.Value = edit.Frames[trackBar2.Value].Center.Y;
                                                Options.ChangedUltimaClass["Animations"] = true;
                                            }
                                        }
                                        if ((w < 4) & (w < dialog.FileNames.Length - 1))
                                            trackBar1.Value++;
                                    }
                                }
                            }
                            //Looping if dialog.FileNames.Length != 5
                            while (dialog.FileNames.Length != 5)
                            {
                                if (dialog.ShowDialog() == DialogResult.Cancel)
                                    break;
                                if (dialog.FileNames.Length != 5)
                                    dialog.ShowDialog();
                                if (dialog.FileNames.Length == 5)
                                {
                                    trackBar1.Value = 0;
                                    for (int w = 0; w < dialog.FileNames.Length; w++)
                                    {
                                        if (w < 5)
                                        {
                                            Bitmap bmp = new Bitmap(dialog.FileNames[w]); // dialog.Filename replaced by dialog.FileNames[w]
                                            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                                            if (edit != null)
                                            {
                                                //Gif Especial Properties
                                                if (dialog.FileName.Contains(".gif"))
                                                {
                                                    FrameDimension dimension = new FrameDimension(bmp.FrameDimensionsList[0]);
                                                    // Number of frames 
                                                    int frameCount = bmp.GetFrameCount(dimension);
                                                    bmp.SelectActiveFrame(dimension, 0);
                                                    edit.GetGifPalette(bmp);
                                                    Bitmap[] bitbmp = new Bitmap[frameCount];
                                                    progressBar1.Maximum = frameCount;
                                                    // Return an Image at a certain index 
                                                    for (int index = 0; index < frameCount; index++)
                                                    {
                                                        bitbmp[index] = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format16bppArgb1555);
                                                        bmp.SelectActiveFrame(dimension, index);
                                                        bitbmp[index] = (Bitmap)bmp;
                                                    }
                                                    //Canvas algorithm
                                                    int top = 0;
                                                    int bot = 0;
                                                    int left = 0;
                                                    int right = 0;
                                                    int RegressT = -1;
                                                    int RegressB = -1;
                                                    int RegressL = -1;
                                                    int RegressR = -1;
                                                    bool var = true;
                                                    bool breakOK = false;
                                                    //Top
                                                    for (int Yf = 0; Yf < bitbmp[0].Height; Yf++)
                                                    {
                                                        for (int fram = 0; fram < frameCount; fram++)
                                                        {
                                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                                            for (int Xf = 0; Xf < bitbmp[0].Width; Xf++)
                                                            {
                                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                                    var = true;
                                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                                {
                                                                    var = false;
                                                                    if (Yf != 0)
                                                                    {
                                                                        RegressT++;
                                                                        Yf -= 1;
                                                                        Xf = -1;
                                                                        fram = 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        breakOK = true;
                                                                        break;
                                                                    }
                                                                }
                                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == frameCount - 1 && RegressT == -1 && Yf < bitbmp[0].Height - 9)
                                                                    top += 10;
                                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == frameCount - 1 && RegressT == -1 && Yf >= bitbmp[0].Height - 9)
                                                                    top++;
                                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == frameCount - 1 && RegressT != -1)
                                                                {
                                                                    top = top - RegressT;
                                                                    breakOK = true;
                                                                    break;
                                                                }
                                                            }
                                                            if (breakOK == true)
                                                                break;
                                                        }
                                                        if (Yf < bitbmp[0].Height - 9)
                                                            Yf += 9; // 1 of for + 9
                                                        if (breakOK == true)
                                                        {
                                                            breakOK = false;
                                                            break;
                                                        }
                                                    }
                                                    //Bot
                                                    for (int Yf = bitbmp[0].Height - 1; Yf > 0; Yf--)
                                                    {
                                                        for (int fram = 0; fram < frameCount; fram++)
                                                        {
                                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                                            for (int Xf = 0; Xf < bitbmp[0].Width; Xf++)
                                                            {
                                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                                    var = true;
                                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                                {
                                                                    var = false;
                                                                    if (Yf != bitbmp[0].Height - 1)
                                                                    {
                                                                        RegressB++;
                                                                        Yf += 1;
                                                                        Xf = -1;
                                                                        fram = 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        breakOK = true;
                                                                        break;
                                                                    }
                                                                }
                                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == frameCount - 1 && RegressB == -1 && Yf > 9)
                                                                    bot += 10;
                                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == frameCount - 1 && RegressB == -1 && Yf <= 9)
                                                                    bot++;
                                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == frameCount - 1 && RegressB != -1)
                                                                {
                                                                    bot = bot - RegressB;
                                                                    breakOK = true;
                                                                    break;
                                                                }
                                                            }
                                                            if (breakOK == true)
                                                                break;
                                                        }
                                                        if (Yf > 9)
                                                            Yf -= 9; // 1 of for + 9
                                                        if (breakOK == true)
                                                        {
                                                            breakOK = false;
                                                            break;
                                                        }
                                                    }
                                                    //Left

                                                    for (int Xf = 0; Xf < bitbmp[0].Width; Xf++)
                                                    {
                                                        for (int fram = 0; fram < frameCount; fram++)
                                                        {
                                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                                            for (int Yf = 0; Yf < bitbmp[0].Height; Yf++)
                                                            {
                                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                                    var = true;
                                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                                {
                                                                    var = false;
                                                                    if (Xf != 0)
                                                                    {
                                                                        RegressL++;
                                                                        Xf -= 1;
                                                                        Yf = -1;
                                                                        fram = 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        breakOK = true;
                                                                        break;
                                                                    }
                                                                }
                                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == frameCount - 1 && RegressL == -1 && Xf < bitbmp[0].Width - 9)
                                                                    left += 10;
                                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == frameCount - 1 && RegressL == -1 && Xf >= bitbmp[0].Width - 9)
                                                                    left++;
                                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == frameCount - 1 && RegressL != -1)
                                                                {
                                                                    left = left - RegressL;
                                                                    breakOK = true;
                                                                    break;
                                                                }
                                                            }
                                                            if (breakOK == true)
                                                                break;
                                                        }
                                                        if (Xf < bitbmp[0].Width - 9)
                                                            Xf += 9; // 1 of for + 9
                                                        if (breakOK == true)
                                                        {
                                                            breakOK = false;
                                                            break;
                                                        }
                                                    }
                                                    //Right
                                                    for (int Xf = bitbmp[0].Width - 1; Xf > 0; Xf--)
                                                    {
                                                        for (int fram = 0; fram < frameCount; fram++)
                                                        {
                                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                                            for (int Yf = 0; Yf < bitbmp[0].Height; Yf++)
                                                            {
                                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                                    var = true;
                                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                                {
                                                                    var = false;
                                                                    if (Xf != bitbmp[0].Width - 1)
                                                                    {
                                                                        RegressR++;
                                                                        Xf += 1;
                                                                        Yf = -1;
                                                                        fram = 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        breakOK = true;
                                                                        break;
                                                                    }
                                                                }
                                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == frameCount - 1 && RegressR == -1 && Xf > 9)
                                                                    right += 10;
                                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == frameCount - 1 && RegressR == -1 && Xf <= 9)
                                                                    right++;
                                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == frameCount - 1 && RegressR != -1)
                                                                {
                                                                    right = right - RegressR;
                                                                    breakOK = true;
                                                                    break;
                                                                }
                                                            }
                                                            if (breakOK == true)
                                                                break;
                                                        }
                                                        if (Xf > 9)
                                                            Xf -= 9; // 1 of for + 9
                                                        if (breakOK == true)
                                                        {
                                                            breakOK = false;
                                                            break;
                                                        }
                                                    }

                                                    for (int index = 0; index < frameCount; index++)
                                                    {
                                                        Rectangle rect = new Rectangle(left, top, (bitbmp[index].Width - left - right), (bitbmp[index].Height - top - bot));
                                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                                        Bitmap myImage2 = bitbmp[index].Clone(rect, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);
                                                        bitbmp[index] = myImage2;
                                                    }

                                                    //End of Canvas
                                                    for (int index = 0; index < frameCount; index++)
                                                    {
                                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                                        bitbmp[index] = Utils.ConvertBmpAnim(bitbmp[index], (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                                                        edit.AddFrame(bitbmp[index]);
                                                        TreeNode node = GetNode(CurrBody);
                                                        if (node != null)
                                                        {
                                                            node.ForeColor = Color.Black;
                                                            node.Nodes[CurrAction].ForeColor = Color.Black;
                                                        }
                                                        ListViewItem item;
                                                        int i = edit.Frames.Count - 1;
                                                        item = new ListViewItem(i.ToString(), 0);
                                                        item.Tag = i;
                                                        listView1.Items.Add(item);
                                                        int width = listView1.TileSize.Width - 5;
                                                        if (bmp.Width > listView1.TileSize.Width)
                                                            width = bmp.Width;
                                                        int height = listView1.TileSize.Height - 5;
                                                        if (bmp.Height > listView1.TileSize.Height)
                                                            height = bmp.Height;

                                                        listView1.TileSize = new Size(width + 5, height + 5);
                                                        trackBar2.Maximum = i;
                                                        Options.ChangedUltimaClass["Animations"] = true;
                                                        if (progressBar1.Value < progressBar1.Maximum)
                                                        {
                                                            progressBar1.Value++;
                                                            progressBar1.Invalidate();
                                                        }
                                                    }
                                                    progressBar1.Value = 0;
                                                    progressBar1.Invalidate();
                                                    SetPaletteBox();
                                                    listView1.Invalidate();
                                                    numericUpDownCx.Value = edit.Frames[trackBar2.Value].Center.X;
                                                    numericUpDownCy.Value = edit.Frames[trackBar2.Value].Center.Y;
                                                    Options.ChangedUltimaClass["Animations"] = true;
                                                }
                                            }
                                            if ((w < 4) & (w < dialog.FileNames.Length - 1))
                                                trackBar1.Value++;
                                        }
                                    }
                                }
                            }
                            trackBar1.Enabled = true;
                        }
                    }
                }
                //Refresh List after Canvas reduction
                CurrDir = trackBar1.Value;
                AfterSelectTreeView(null, null);
            }
            catch (System.OutOfMemoryException) { }
        }
        //Add with Canvas
        private void addWithCanvasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileType != 0)
                {
                    using (OpenFileDialog dialog = new OpenFileDialog())
                    {
                        dialog.Multiselect = false;
                        dialog.Title = "Choose image file to add";
                        dialog.CheckFileExists = true;
                        dialog.Filter = "Gif files (*.gif;)|*.gif;";
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            Color CustomConvert = Color.FromArgb(255, (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                            //My Soulblighter Modifications
                            for (int w = 0; w < dialog.FileNames.Length; w++)
                            {
                                Bitmap bmp = new Bitmap(dialog.FileNames[w]); // dialog.Filename replaced by dialog.FileNames[w]
                                AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                                if (edit != null)
                                {
                                    //Gif Especial Properties
                                    if (dialog.FileName.Contains(".gif"))
                                    {
                                        FrameDimension dimension = new FrameDimension(bmp.FrameDimensionsList[0]);
                                        // Number of frames 
                                        int frameCount = bmp.GetFrameCount(dimension);
                                        bmp.SelectActiveFrame(dimension, 0);
                                        edit.GetGifPalette(bmp);
                                        Bitmap[] bitbmp = new Bitmap[frameCount];
                                        progressBar1.Maximum = frameCount;
                                        // Return an Image at a certain index 
                                        for (int index = 0; index < frameCount; index++)
                                        {
                                            bitbmp[index] = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format16bppArgb1555);
                                            bmp.SelectActiveFrame(dimension, index);
                                            bitbmp[index] = (Bitmap)bmp;
                                        }
                                        //Canvas algorithm
                                        int top = 0;
                                        int bot = 0;
                                        int left = 0;
                                        int right = 0;
                                        int RegressT = -1;
                                        int RegressB = -1;
                                        int RegressL = -1;
                                        int RegressR = -1;
                                        bool var = true;
                                        bool breakOK = false;
                                        //Top
                                        for (int Yf = 0; Yf < bitbmp[0].Height; Yf++)
                                        {
                                            for (int fram = 0; fram < frameCount; fram++)
                                            {
                                                bitbmp[fram].SelectActiveFrame(dimension, fram);
                                                for (int Xf = 0; Xf < bitbmp[0].Width; Xf++)
                                                {
                                                    Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                    if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                        var = true;
                                                    if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                    {
                                                        var = false;
                                                        if (Yf != 0)
                                                        {
                                                            RegressT++;
                                                            Yf -= 1;
                                                            Xf = -1;
                                                            fram = 0;
                                                        }
                                                        else
                                                        {
                                                            breakOK = true;
                                                            break;
                                                        }
                                                    }
                                                    if (var == true && Xf == bitbmp[fram].Width - 1 && fram == frameCount - 1 && RegressT == -1 && Yf < bitbmp[0].Height - 9)
                                                        top += 10;
                                                    if (var == true && Xf == bitbmp[fram].Width - 1 && fram == frameCount - 1 && RegressT == -1 && Yf >= bitbmp[0].Height - 9)
                                                        top++;
                                                    if (var == true && Xf == bitbmp[fram].Width - 1 && fram == frameCount - 1 && RegressT != -1)
                                                    {
                                                        top = top - RegressT;
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (breakOK == true)
                                                    break;
                                            }
                                            if (Yf < bitbmp[0].Height - 9)
                                                Yf += 9; // 1 of for + 9
                                            if (breakOK == true)
                                            {
                                                breakOK = false;
                                                break;
                                            }
                                        }
                                        //Bot
                                        for (int Yf = bitbmp[0].Height - 1; Yf > 0; Yf--)
                                        {
                                            for (int fram = 0; fram < frameCount; fram++)
                                            {
                                                bitbmp[fram].SelectActiveFrame(dimension, fram);
                                                for (int Xf = 0; Xf < bitbmp[0].Width; Xf++)
                                                {
                                                    Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                    if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                        var = true;
                                                    if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                    {
                                                        var = false;
                                                        if (Yf != bitbmp[0].Height - 1)
                                                        {
                                                            RegressB++;
                                                            Yf += 1;
                                                            Xf = -1;
                                                            fram = 0;
                                                        }
                                                        else
                                                        {
                                                            breakOK = true;
                                                            break;
                                                        }
                                                    }
                                                    if (var == true && Xf == bitbmp[fram].Width - 1 && fram == frameCount - 1 && RegressB == -1 && Yf > 9)
                                                        bot += 10;
                                                    if (var == true && Xf == bitbmp[fram].Width - 1 && fram == frameCount - 1 && RegressB == -1 && Yf <= 9)
                                                        bot++;
                                                    if (var == true && Xf == bitbmp[fram].Width - 1 && fram == frameCount - 1 && RegressB != -1)
                                                    {
                                                        bot = bot - RegressB;
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (breakOK == true)
                                                    break;
                                            }
                                            if (Yf > 9)
                                                Yf -= 9; // 1 of for + 9
                                            if (breakOK == true)
                                            {
                                                breakOK = false;
                                                break;
                                            }
                                        }
                                        //Left

                                        for (int Xf = 0; Xf < bitbmp[0].Width; Xf++)
                                        {
                                            for (int fram = 0; fram < frameCount; fram++)
                                            {
                                                bitbmp[fram].SelectActiveFrame(dimension, fram);
                                                for (int Yf = 0; Yf < bitbmp[0].Height; Yf++)
                                                {
                                                    Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                    if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                        var = true;
                                                    if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                    {
                                                        var = false;
                                                        if (Xf != 0)
                                                        {
                                                            RegressL++;
                                                            Xf -= 1;
                                                            Yf = -1;
                                                            fram = 0;
                                                        }
                                                        else
                                                        {
                                                            breakOK = true;
                                                            break;
                                                        }
                                                    }
                                                    if (var == true && Yf == bitbmp[fram].Height - 1 && fram == frameCount - 1 && RegressL == -1 && Xf < bitbmp[0].Width - 9)
                                                        left += 10;
                                                    if (var == true && Yf == bitbmp[fram].Height - 1 && fram == frameCount - 1 && RegressL == -1 && Xf >= bitbmp[0].Width - 9)
                                                        left++;
                                                    if (var == true && Yf == bitbmp[fram].Height - 1 && fram == frameCount - 1 && RegressL != -1)
                                                    {
                                                        left = left - RegressL;
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (breakOK == true)
                                                    break;
                                            }
                                            if (Xf < bitbmp[0].Width - 9)
                                                Xf += 9; // 1 of for + 9
                                            if (breakOK == true)
                                            {
                                                breakOK = false;
                                                break;
                                            }
                                        }
                                        //Right
                                        for (int Xf = bitbmp[0].Width - 1; Xf > 0; Xf--)
                                        {
                                            for (int fram = 0; fram < frameCount; fram++)
                                            {
                                                bitbmp[fram].SelectActiveFrame(dimension, fram);
                                                for (int Yf = 0; Yf < bitbmp[0].Height; Yf++)
                                                {
                                                    Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                    if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                        var = true;
                                                    if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                    {
                                                        var = false;
                                                        if (Xf != bitbmp[0].Width - 1)
                                                        {
                                                            RegressR++;
                                                            Xf += 1;
                                                            Yf = -1;
                                                            fram = 0;
                                                        }
                                                        else
                                                        {
                                                            breakOK = true;
                                                            break;
                                                        }
                                                    }
                                                    if (var == true && Yf == bitbmp[fram].Height - 1 && fram == frameCount - 1 && RegressR == -1 && Xf > 9)
                                                        right += 10;
                                                    if (var == true && Yf == bitbmp[fram].Height - 1 && fram == frameCount - 1 && RegressR == -1 && Xf <= 9)
                                                        right++;
                                                    if (var == true && Yf == bitbmp[fram].Height - 1 && fram == frameCount - 1 && RegressR != -1)
                                                    {
                                                        right = right - RegressR;
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (breakOK == true)
                                                    break;
                                            }
                                            if (Xf > 9)
                                                Xf -= 9; // 1 of for + 9
                                            if (breakOK == true)
                                            {
                                                breakOK = false;
                                                break;
                                            }
                                        }

                                        for (int index = 0; index < frameCount; index++)
                                        {
                                            Rectangle rect = new Rectangle(left, top, (bitbmp[index].Width - left - right), (bitbmp[index].Height - top - bot));
                                            bitbmp[index].SelectActiveFrame(dimension, index);
                                            Bitmap myImage2 = bitbmp[index].Clone(rect, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);
                                            bitbmp[index] = myImage2;
                                        }

                                        //End of Canvas
                                        for (int index = 0; index < frameCount; index++)
                                        {
                                            bitbmp[index].SelectActiveFrame(dimension, index);
                                            bitbmp[index] = Utils.ConvertBmpAnim(bitbmp[index], (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                                            edit.AddFrame(bitbmp[index]);
                                            TreeNode node = GetNode(CurrBody);
                                            if (node != null)
                                            {
                                                node.ForeColor = Color.Black;
                                                node.Nodes[CurrAction].ForeColor = Color.Black;
                                            }
                                            ListViewItem item;
                                            int i = edit.Frames.Count - 1;
                                            item = new ListViewItem(i.ToString(), 0);
                                            item.Tag = i;
                                            listView1.Items.Add(item);
                                            int width = listView1.TileSize.Width - 5;
                                            if (bmp.Width > listView1.TileSize.Width)
                                                width = bmp.Width;
                                            int height = listView1.TileSize.Height - 5;
                                            if (bmp.Height > listView1.TileSize.Height)
                                                height = bmp.Height;

                                            listView1.TileSize = new Size(width + 5, height + 5);
                                            trackBar2.Maximum = i;
                                            Options.ChangedUltimaClass["Animations"] = true;
                                            if (progressBar1.Value < progressBar1.Maximum)
                                            {
                                                progressBar1.Value++;
                                                progressBar1.Invalidate();
                                            }
                                        }
                                        progressBar1.Value = 0;
                                        progressBar1.Invalidate();
                                        SetPaletteBox();
                                        listView1.Invalidate();
                                        numericUpDownCx.Value = edit.Frames[trackBar2.Value].Center.X;
                                        numericUpDownCy.Value = edit.Frames[trackBar2.Value].Center.Y;
                                        Options.ChangedUltimaClass["Animations"] = true;
                                    }
                                    //End of Soulblighter Modifications
                                }
                            }
                        }
                    }
                }
                //Refresh List after Canvas reduction
                CurrDir = trackBar1.Value;
                AfterSelectTreeView(null, null);
            }
            catch (System.OutOfMemoryException) { }
        }

        private unsafe void OnClickAddGen(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = true;
                dialog.Title = "Выбирите файлы изображений для добавления. Напоминание: для выбранных изображений будет сгенерирована палитра, которая заменит старую.";
                dialog.CheckFileExists = true;
                dialog.Filter = "файлы изображений (*.tiff;*.bmp)|*.tiff;*.bmp";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // Получаем палитру
                    ushort[] Palette = new ushort[0x100];
                    int count = 0;
                    foreach (string filename in dialog.FileNames)
                    {
                        Bitmap bmp = new Bitmap(filename);
                        if (dialog.FileName.Contains(".bmp"))
                        {
                            if (useCKeyFilter)
                                bmp = Utils.CKeyFilter(bmp);
                            if (useBColFilter)
                                bmp = Utils.BColFilter(bmp, false);

                            bmp = Utils.ConvertBmp(bmp);
                        }

                        BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
                        ushort* line = (ushort*)bd.Scan0;
                        int delta = bd.Stride >> 1;
                        ushort* cur = line;
                        for (int y = 0; y < bmp.Height; ++y, line += delta)
                        {
                            cur = line;
                            for (int x = 0; x < bmp.Width; ++x)
                            {
                                ushort c = cur[x];
                                if (c != 0)
                                {
                                    bool found = false;
                                    for (int i = 0; i < Palette.Length; ++i)
                                    {
                                        if (Palette[i] == c)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                    if (!found)
                                        Palette[count++] = c;
                                    if (count >= 0x100)
                                    {
                                        MessageBox.Show(
                                            "Используется больше чем 0x100 (256) цветов!",
                                            "Генерация палитры",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error,
                                            MessageBoxDefaultButton.Button1);
                                        return;
                                    }
                                }
                            }
                        }
                    }

                    // Устанавливаем палитру
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                    if (edit != null)
                    {
                        edit.ReplacePalette(Palette);
                        SetPaletteBox();
                        listView1.Invalidate();
                        Options.ChangedUltimaClass["Animations"] = true;
                    }
                    else
                    {
                        MessageBox.Show(
                            String.Format("Палитра не может быть применена в данный момент, из-за выполняемых действий."),
                            "Генерация палитры",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    // Добавляем кадры
                    listView1.BeginUpdate();
                    foreach (string filename in dialog.FileNames)
                    {
                        Bitmap bmp = new Bitmap(filename);
                        if (filename.Contains(".bmp"))
                        {
                            if (useCKeyFilter)
                                bmp = Utils.CKeyFilter(bmp);
                            if (useBColFilter)
                                bmp = Utils.BColFilter(bmp, false);

                            bmp = Utils.ConvertBmp(bmp);
                        }
                        AnimIdx edit2 = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                        if (edit2 != null)
                        {
                            edit2.AddFrame(bmp);
                            TreeNode node = GetNode(CurrBody);
                            if (node != null)
                            {
                                node.ForeColor = Color.Black;
                                node.Nodes[CurrAction].ForeColor = Color.Black;
                            }
                            ListViewItem item;
                            int i = edit2.Frames.Count - 1;
                            item = new ListViewItem(i.ToString(), 0);
                            item.Tag = i;
                            listView1.Items.Add(item);
                            int width = listView1.TileSize.Width - 5;
                            if (bmp.Width > listView1.TileSize.Width)
                                width = bmp.Width;
                            int height = listView1.TileSize.Height - 5;
                            if (bmp.Height > listView1.TileSize.Height)
                                height = bmp.Height;

                            listView1.TileSize = new Size(width + 5, height + 5);
                            trackBar2.Maximum = i;
                            Options.ChangedUltimaClass["Animations"] = true;
                        }
                    }
                    listView1.EndUpdate();
                    listView1.Invalidate();

                }
            }
        }

        private unsafe void OnClickGeneratePalette(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = true;
                dialog.Title = "Выбирите изображения для которых хотите сгенерировать палитру";
                dialog.CheckFileExists = true;
                dialog.Filter = "файлы изображений (*.tiff;*.bmp;*.png;*.jpg;*.jpeg)|*.tiff;*.bmp;*.png;*.jpg;*.jpeg";
                if (dialog.ShowDialog() == DialogResult.OK)
                {

                    foreach (string filename in dialog.FileNames)
                    {
                        Bitmap bit = new Bitmap(filename);
                        AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                        if (edit != null)
                        {
                            bit = Utils.ConvertBmpAnim(bit, (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                            edit.GetImagePalette(bit);
                        }
                        SetPaletteBox();
                        listView1.Invalidate();
                        Options.ChangedUltimaClass["Animations"] = true;
                        SetPaletteBox();
                    }
                }
            }
        }
        //End of Soulblighter Modification

        private void OnClickExportAllToVD(object sender, EventArgs e)
        {
            if (FileType != 0)
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    dialog.Description = "Select directory";
                    dialog.ShowNewFolderButton = true;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        for (int i = 0; i < treeView1.Nodes.Count; ++i)
                        {
                            int index = (int)treeView1.Nodes[i].Tag;
                            if (index >= 0 && treeView1.Nodes[i].Parent == null && treeView1.Nodes[i].ForeColor != Color.Red)
                            {
                                string FileName = Path.Combine(dialog.SelectedPath, String.Format("anim{0}_0x{1:X}.vd", FileType, index));
                                Ultima.AnimationEdit.ExportToVD(FileType, index, FileName);
                            }
                        }
                        MessageBox.Show(String.Format("All Animations saved to {0}", dialog.SelectedPath.ToString()),
                                "Export", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                }
            }

        }

        private void OnClickExportAllToAVInGIF(object sender, EventArgs e)
        {
            if (FileType == 0) return;
            treeView1.Enabled = false;
            
            for (int i = 0; i < treeView1.Nodes.Count; ++i) {
                int index = (int)treeView1.Nodes[i].Tag;
                if (index >= 0 && treeView1.Nodes[i].Parent == null && treeView1.Nodes[i].ForeColor != Color.Red) {
                    treeView1.SelectedNode = treeView1.Nodes[i];
                    try { OnClickExportToAVInGIF(sender, e); } catch {;}
                }
            }
            
            treeView1.Enabled = true;
            MessageBox.Show(String.Format("Вся анимация была экспортирована в папку \"{0}\"", Path.Combine(Options.ExtractedPath, "Animation")),
                    "Экспорт анимации", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        //Get position of all animations in array
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                trackBar1.Enabled = false;
                trackBar2.Value = 0;
                button1.Enabled = true;
                for (int count = 0; count < 5; )
                {
                    if (trackBar1.Value < 4)
                    {
                        AnimCx[trackBar1.Value] = (int)numericUpDownCx.Value;
                        AnimCy[trackBar1.Value] = (int)numericUpDownCy.Value;
                        trackBar1.Value++;
                        count++;
                    }
                    else
                    {
                        AnimCx[trackBar1.Value] = (int)numericUpDownCx.Value;
                        AnimCy[trackBar1.Value] = (int)numericUpDownCy.Value;
                        trackBar1.Value = 0;
                        count++;
                    }

                }
                toolStripLabel8.Text = "1: " + AnimCx[0] + "/" + AnimCy[0];
                toolStripLabel9.Text = "2: " + AnimCx[1] + "/" + AnimCy[1];
                toolStripLabel10.Text = "3: " + AnimCx[2] + "/" + AnimCy[2];
                toolStripLabel11.Text = "4: " + AnimCx[3] + "/" + AnimCy[3];
                toolStripLabel12.Text = "5: " + AnimCx[4] + "/" + AnimCy[4];
                trackBar1.Enabled = true;
            }
            else
            {
                toolStripLabel8.Text = "1:    /     ";
                toolStripLabel9.Text = "2:    /     ";
                toolStripLabel10.Text = "3:    /     ";
                toolStripLabel11.Text = "4:    /     ";
                toolStripLabel12.Text = "5:    /     ";
                button1.Enabled = false;
            }
        }
        //Set Button
        private void button1_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 0;
            trackBar1.Enabled = false;
            for (int i = 0; i <= trackBar1.Maximum; i++)
            {
                try
                {
                    if (FileType != 0)
                    {
                        AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                        if (edit != null)
                        {
                            if (edit.Frames.Count >= trackBar2.Value)
                            {
                                for (int Index = 0; Index < edit.Frames.Count; Index++)
                                {
                                    edit.Frames[Index].ChangeCenter(AnimCx[i], AnimCy[i]);
                                    Options.ChangedUltimaClass["Animations"] = true;
                                    pictureBox1.Invalidate();
                                }
                            }
                        }
                    }
                }
                catch (System.NullReferenceException) { }
                if (trackBar1.Value < trackBar1.Maximum)
                {
                    trackBar1.Value++;
                }
                else
                    trackBar1.Value = 0;
            }
            trackBar1.Enabled = true;
        }
        //Add Directions with Canvas ( CV5 style GIF )
        private void addDirectionsAddWithCanvasUniqueImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileType != 0)
                {
                    using (OpenFileDialog dialog = new OpenFileDialog())
                    {
                        dialog.Multiselect = false;
                        dialog.Title = "Choose 1 Gif ( with all directions in CV5 Style ) to add";
                        dialog.CheckFileExists = true;
                        dialog.Filter = "Gif files (*.gif;)|*.gif;";
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            Color CustomConvert = Color.FromArgb(255, (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                            trackBar1.Enabled = false;
                            trackBar1.Value = 0;
                            Bitmap bmp = new Bitmap(dialog.FileName);
                            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                            if (edit != null)
                            {
                                //Gif Especial Properties
                                if (dialog.FileName.Contains(".gif"))
                                {
                                    FrameDimension dimension = new FrameDimension(bmp.FrameDimensionsList[0]);
                                    // Number of frames 
                                    int frameCount = bmp.GetFrameCount(dimension);
                                    progressBar1.Maximum = frameCount;
                                    bmp.SelectActiveFrame(dimension, 0);
                                    edit.GetGifPalette(bmp);
                                    Bitmap[] bitbmp = new Bitmap[frameCount];
                                    // Return an Image at a certain index 
                                    for (int index = 0; index < frameCount; index++)
                                    {
                                        bitbmp[index] = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format16bppArgb1555);
                                        bmp.SelectActiveFrame(dimension, index);
                                        bitbmp[index] = (Bitmap)bmp;
                                    }
                                    //Canvas algorithm
                                    int top = 0;
                                    int bot = 0;
                                    int left = 0;
                                    int right = 0;
                                    int RegressT = -1;
                                    int RegressB = -1;
                                    int RegressL = -1;
                                    int RegressR = -1;
                                    bool var = true;
                                    bool breakOK = false;
                                    // position 0
                                    //Top
                                    for (int Yf = 0; Yf < bitbmp[(frameCount / 8) * 4].Height; Yf++)
                                    {
                                        for (int fram = (frameCount / 8) * 4; fram < (frameCount / 8) * 5; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[(frameCount / 8) * 4].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != 0)
                                                    {
                                                        RegressT++;
                                                        Yf -= 1;
                                                        Xf = -1;
                                                        fram = (frameCount / 8) * 4;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 5) - 1) && RegressT == -1 && Yf < bitbmp[0].Height - 9)
                                                    top += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 5) - 1) && RegressT == -1 && Yf >= bitbmp[0].Height - 9)
                                                    top++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 5) - 1) && RegressT != -1)
                                                {
                                                    top = top - RegressT;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf < bitbmp[(frameCount / 8) * 4].Height - 9)
                                            Yf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Bot
                                    for (int Yf = bitbmp[(frameCount / 8) * 4].Height - 1; Yf > 0; Yf--)
                                    {
                                        for (int fram = (frameCount / 8) * 4; fram < (frameCount / 8) * 5; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[(frameCount / 8) * 4].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != bitbmp[(frameCount / 8) * 4].Height - 1)
                                                    {
                                                        RegressB++;
                                                        Yf += 1;
                                                        Xf = -1;
                                                        fram = (frameCount / 8) * 4;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 5) - 1) && RegressB == -1 && Yf > 9)
                                                    bot += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 5) - 1) && RegressB == -1 && Yf <= 9)
                                                    bot++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 5) - 1) && RegressB != -1)
                                                {
                                                    bot = bot - RegressB;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf > 9)
                                            Yf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Left

                                    for (int Xf = 0; Xf < bitbmp[(frameCount / 8) * 4].Width; Xf++)
                                    {
                                        for (int fram = (frameCount / 8) * 4; fram < (frameCount / 8) * 5; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[(frameCount / 8) * 4].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != 0)
                                                    {
                                                        RegressL++;
                                                        Xf -= 1;
                                                        Yf = -1;
                                                        fram = (frameCount / 8) * 4;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 5) - 1) && RegressL == -1 && Xf < bitbmp[0].Width - 9)
                                                    left += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 5) - 1) && RegressL == -1 && Xf >= bitbmp[0].Width - 9)
                                                    left++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 5) - 1) && RegressL != -1)
                                                {
                                                    left = left - RegressL;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf < bitbmp[(frameCount / 8) * 4].Width - 9)
                                            Xf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Right
                                    for (int Xf = bitbmp[(frameCount / 8) * 4].Width - 1; Xf > 0; Xf--)
                                    {
                                        for (int fram = (frameCount / 8) * 4; fram < (frameCount / 8) * 5; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[(frameCount / 8) * 4].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != bitbmp[(frameCount / 8) * 4].Width - 1)
                                                    {
                                                        RegressR++;
                                                        Xf += 1;
                                                        Yf = -1;
                                                        fram = (frameCount / 8) * 4;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 5) - 1) && RegressR == -1 && Xf > 9)
                                                    right += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 5) - 1) && RegressR == -1 && Xf <= 9)
                                                    right++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 5) - 1) && RegressR != -1)
                                                {
                                                    right = right - RegressR;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf > 9)
                                            Xf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    for (int index = (frameCount / 8) * 4; index < (frameCount / 8) * 5; index++)
                                    {
                                        Rectangle rect = new Rectangle(left, top, (bitbmp[index].Width - left - right), (bitbmp[index].Height - top - bot));
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        Bitmap myImage2 = bitbmp[index].Clone(rect, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);
                                        bitbmp[index] = myImage2;
                                    }

                                    //Reseta cordenadas
                                    top = 0;
                                    bot = 0;
                                    left = 0;
                                    right = 0;
                                    RegressT = -1;
                                    RegressB = -1;
                                    RegressL = -1;
                                    RegressR = -1;
                                    var = true;
                                    breakOK = false;
                                    // position 1
                                    //Top
                                    for (int Yf = 0; Yf < bitbmp[0].Height; Yf++)
                                    {
                                        for (int fram = 0; fram < frameCount / 8; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[0].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != 0)
                                                    {
                                                        RegressT++;
                                                        Yf -= 1;
                                                        Xf = -1;
                                                        fram = 0;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8)) - 1) && RegressT == -1 && Yf < bitbmp[0].Height - 9)
                                                    top += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8)) - 1) && RegressT == -1 && Yf >= bitbmp[0].Height - 9)
                                                    top++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8)) - 1) && RegressT != -1)
                                                {
                                                    top = top - RegressT;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf < bitbmp[0].Height - 9)
                                            Yf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Bot
                                    for (int Yf = bitbmp[0].Height - 1; Yf > 0; Yf--)
                                    {
                                        for (int fram = 0; fram < frameCount / 8; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[0].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != bitbmp[0].Height - 1)
                                                    {
                                                        RegressB++;
                                                        Yf += 1;
                                                        Xf = -1;
                                                        fram = 0;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8)) - 1) && RegressB == -1 && Yf > 9)
                                                    bot += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8)) - 1) && RegressB == -1 && Yf <= 9)
                                                    bot++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8)) - 1) && RegressB != -1)
                                                {
                                                    bot = bot - RegressB;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf > 9)
                                            Yf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Left

                                    for (int Xf = 0; Xf < bitbmp[0].Width; Xf++)
                                    {
                                        for (int fram = 0; fram < frameCount / 8; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[0].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != 0)
                                                    {
                                                        RegressL++;
                                                        Xf -= 1;
                                                        Yf = -1;
                                                        fram = 0;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8)) - 1) && RegressL == -1 && Xf < bitbmp[0].Width - 9)
                                                    left += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8)) - 1) && RegressL == -1 && Xf >= bitbmp[0].Width - 9)
                                                    left++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8)) - 1) && RegressL != -1)
                                                {
                                                    left = left - RegressL;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf < bitbmp[0].Width - 9)
                                            Xf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Right
                                    for (int Xf = bitbmp[0].Width - 1; Xf > 0; Xf--)
                                    {
                                        for (int fram = 0; fram < frameCount / 8; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[0].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != bitbmp[0].Width - 1)
                                                    {
                                                        RegressR++;
                                                        Xf += 1;
                                                        Yf = -1;
                                                        fram = 0;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8)) - 1) && RegressR == -1 && Xf > 9)
                                                    right += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8)) - 1) && RegressR == -1 && Xf <= 9)
                                                    right++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8)) - 1) && RegressR != -1)
                                                {
                                                    right = right - RegressR;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf > 9)
                                            Xf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    for (int index = 0; index < frameCount / 8; index++)
                                    {
                                        Rectangle rect = new Rectangle(left, top, (bitbmp[index].Width - left - right), (bitbmp[index].Height - top - bot));
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        Bitmap myImage2 = bitbmp[index].Clone(rect, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);
                                        bitbmp[index] = myImage2;
                                    }

                                    //Reseta cordenadas
                                    top = 0;
                                    bot = 0;
                                    left = 0;
                                    right = 0;
                                    RegressT = -1;
                                    RegressB = -1;
                                    RegressL = -1;
                                    RegressR = -1;
                                    var = true;
                                    breakOK = false;
                                    // position 2
                                    //Top
                                    for (int Yf = 0; Yf < bitbmp[((frameCount / 8) * 5)].Height; Yf++)
                                    {
                                        for (int fram = ((frameCount / 8) * 5); fram < (frameCount / 8) * 6; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[((frameCount / 8) * 5)].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != 0)
                                                    {
                                                        RegressT++;
                                                        Yf -= 1;
                                                        Xf = -1;
                                                        fram = ((frameCount / 8) * 5);
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 6) - 1) && RegressT == -1 && Yf < bitbmp[0].Height - 9)
                                                    top += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 6) - 1) && RegressT == -1 && Yf >= bitbmp[0].Height - 9)
                                                    top++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 6) - 1) && RegressT != -1)
                                                {
                                                    top = top - RegressT;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf < bitbmp[((frameCount / 8) * 5)].Height - 9)
                                            Yf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Bot
                                    for (int Yf = bitbmp[((frameCount / 8) * 5)].Height - 1; Yf > 0; Yf--)
                                    {
                                        for (int fram = ((frameCount / 8) * 5); fram < (frameCount / 8) * 6; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[((frameCount / 8) * 5)].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != bitbmp[((frameCount / 8) * 5)].Height - 1)
                                                    {
                                                        RegressB++;
                                                        Yf += 1;
                                                        Xf = -1;
                                                        fram = ((frameCount / 8) * 5);
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 6) - 1) && RegressB == -1 && Yf > 9)
                                                    bot += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 6) - 1) && RegressB == -1 && Yf <= 9)
                                                    bot++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 6) - 1) && RegressB != -1)
                                                {
                                                    bot = bot - RegressB;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf > 9)
                                            Yf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Left

                                    for (int Xf = 0; Xf < bitbmp[((frameCount / 8) * 5)].Width; Xf++)
                                    {
                                        for (int fram = ((frameCount / 8) * 5); fram < (frameCount / 8) * 6; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[((frameCount / 8) * 5)].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != 0)
                                                    {
                                                        RegressL++;
                                                        Xf -= 1;
                                                        Yf = -1;
                                                        fram = ((frameCount / 8) * 5);
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 6) - 1) && RegressL == -1 && Xf < bitbmp[0].Width - 9)
                                                    left += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 6) - 1) && RegressL == -1 && Xf >= bitbmp[0].Width - 9)
                                                    left++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 6) - 1) && RegressL != -1)
                                                {
                                                    left = left - RegressL;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf < bitbmp[((frameCount / 8) * 5)].Width - 9)
                                            Xf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Right
                                    for (int Xf = bitbmp[((frameCount / 8) * 5)].Width - 1; Xf > 0; Xf--)
                                    {
                                        for (int fram = ((frameCount / 8) * 5); fram < (frameCount / 8) * 6; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[((frameCount / 8) * 5)].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != bitbmp[((frameCount / 8) * 5)].Width - 1)
                                                    {
                                                        RegressR++;
                                                        Xf += 1;
                                                        Yf = -1;
                                                        fram = ((frameCount / 8) * 5);
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 6) - 1) && RegressR == -1 && Xf > 9)
                                                    right += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 6) - 1) && RegressR == -1 && Xf <= 9)
                                                    right++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 6) - 1) && RegressR != -1)
                                                {
                                                    right = right - RegressR;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf > 9)
                                            Xf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    for (int index = ((frameCount / 8) * 5); index < (frameCount / 8) * 6; index++)
                                    {
                                        Rectangle rect = new Rectangle(left, top, (bitbmp[index].Width - left - right), (bitbmp[index].Height - top - bot));
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        Bitmap myImage2 = bitbmp[index].Clone(rect, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);
                                        bitbmp[index] = myImage2;
                                    }

                                    //Reseta cordenadas
                                    top = 0;
                                    bot = 0;
                                    left = 0;
                                    right = 0;
                                    RegressT = -1;
                                    RegressB = -1;
                                    RegressL = -1;
                                    RegressR = -1;
                                    var = true;
                                    breakOK = false;
                                    // position 3
                                    //Top
                                    for (int Yf = 0; Yf < bitbmp[((frameCount / 8) * 1)].Height; Yf++)
                                    {
                                        for (int fram = ((frameCount / 8) * 1); fram < (frameCount / 8) * 2; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[((frameCount / 8) * 1)].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != 0)
                                                    {
                                                        RegressT++;
                                                        Yf -= 1;
                                                        Xf = -1;
                                                        fram = ((frameCount / 8) * 1);
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 2) - 1) && RegressT == -1 && Yf < bitbmp[0].Height - 9)
                                                    top += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 2) - 1) && RegressT == -1 && Yf >= bitbmp[0].Height - 9)
                                                    top++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 2) - 1) && RegressT != -1)
                                                {
                                                    top = top - RegressT;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf < bitbmp[((frameCount / 8) * 1)].Height - 9)
                                            Yf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Bot
                                    for (int Yf = bitbmp[((frameCount / 8) * 1)].Height - 1; Yf > 0; Yf--)
                                    {
                                        for (int fram = ((frameCount / 8) * 1); fram < (frameCount / 8) * 2; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[((frameCount / 8) * 1)].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != bitbmp[((frameCount / 8) * 1)].Height - 1)
                                                    {
                                                        RegressB++;
                                                        Yf += 1;
                                                        Xf = -1;
                                                        fram = ((frameCount / 8) * 1);
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 2) - 1) && RegressB == -1 && Yf > 9)
                                                    bot += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 2) - 1) && RegressB == -1 && Yf <= 9)
                                                    bot++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 2) - 1) && RegressB != -1)
                                                {
                                                    bot = bot - RegressB;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf > 9)
                                            Yf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Left

                                    for (int Xf = 0; Xf < bitbmp[((frameCount / 8) * 1)].Width; Xf++)
                                    {
                                        for (int fram = ((frameCount / 8) * 1); fram < (frameCount / 8) * 2; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[((frameCount / 8) * 1)].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != 0)
                                                    {
                                                        RegressL++;
                                                        Xf -= 1;
                                                        Yf = -1;
                                                        fram = ((frameCount / 8) * 1);
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 2) - 1) && RegressL == -1 && Xf < bitbmp[0].Width - 9)
                                                    left += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 2) - 1) && RegressL == -1 && Xf >= bitbmp[0].Width - 9)
                                                    left++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 2) - 1) && RegressL != -1)
                                                {
                                                    left = left - RegressL;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf < bitbmp[((frameCount / 8) * 1)].Width - 9)
                                            Xf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Right
                                    for (int Xf = bitbmp[((frameCount / 8) * 1)].Width - 1; Xf > 0; Xf--)
                                    {
                                        for (int fram = ((frameCount / 8) * 1); fram < (frameCount / 8) * 2; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[((frameCount / 8) * 1)].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != bitbmp[((frameCount / 8) * 1)].Width - 1)
                                                    {
                                                        RegressR++;
                                                        Xf += 1;
                                                        Yf = -1;
                                                        fram = ((frameCount / 8) * 1);
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 2) - 1) && RegressR == -1 && Xf > 9)
                                                    right += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 2) - 1) && RegressR == -1 && Xf <= 9)
                                                    right++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 2) - 1) && RegressR != -1)
                                                {
                                                    right = right - RegressR;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf > 9)
                                            Xf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    for (int index = ((frameCount / 8) * 1); index < (frameCount / 8) * 2; index++)
                                    {
                                        Rectangle rect = new Rectangle(left, top, (bitbmp[index].Width - left - right), (bitbmp[index].Height - top - bot));
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        Bitmap myImage2 = bitbmp[index].Clone(rect, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);
                                        bitbmp[index] = myImage2;
                                    }

                                    //Reseta cordenadas
                                    top = 0;
                                    bot = 0;
                                    left = 0;
                                    right = 0;
                                    RegressT = -1;
                                    RegressB = -1;
                                    RegressL = -1;
                                    RegressR = -1;
                                    var = true;
                                    breakOK = false;
                                    // position 4
                                    //Top
                                    for (int Yf = 0; Yf < bitbmp[((frameCount / 8) * 6)].Height; Yf++)
                                    {
                                        for (int fram = ((frameCount / 8) * 6); fram < (frameCount / 8) * 7; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[((frameCount / 8) * 6)].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != 0)
                                                    {
                                                        RegressT++;
                                                        Yf -= 1;
                                                        Xf = -1;
                                                        fram = ((frameCount / 8) * 6);
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 7) - 1) && RegressT == -1 && Yf < bitbmp[0].Height - 9)
                                                    top += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 7) - 1) && RegressT == -1 && Yf >= bitbmp[0].Height - 9)
                                                    top++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 7) - 1) && RegressT != -1)
                                                {
                                                    top = top - RegressT;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf < bitbmp[((frameCount / 8) * 6)].Height - 9)
                                            Yf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Bot
                                    for (int Yf = bitbmp[((frameCount / 8) * 6)].Height - 1; Yf > 0; Yf--)
                                    {
                                        for (int fram = ((frameCount / 8) * 6); fram < (frameCount / 8) * 7; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[((frameCount / 8) * 6)].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != bitbmp[((frameCount / 8) * 6)].Height - 1)
                                                    {
                                                        RegressB++;
                                                        Yf += 1;
                                                        Xf = -1;
                                                        fram = ((frameCount / 8) * 6);
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 7) - 1) && RegressB == -1 && Yf > 9)
                                                    bot += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 7) - 1) && RegressB == -1 && Yf <= 9)
                                                    bot++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 8) * 7) - 1) && RegressB != -1)
                                                {
                                                    bot = bot - RegressB;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf > 9)
                                            Yf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Left

                                    for (int Xf = 0; Xf < bitbmp[((frameCount / 8) * 6)].Width; Xf++)
                                    {
                                        for (int fram = ((frameCount / 8) * 6); fram < (frameCount / 8) * 7; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[((frameCount / 8) * 6)].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != 0)
                                                    {
                                                        RegressL++;
                                                        Xf -= 1;
                                                        Yf = -1;
                                                        fram = ((frameCount / 8) * 6);
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 7) - 1) && RegressL == -1 && Xf < bitbmp[0].Width - 9)
                                                    left += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 7) - 1) && RegressL == -1 && Xf >= bitbmp[0].Width - 9)
                                                    left++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 7) - 1) && RegressL != -1)
                                                {
                                                    left = left - RegressL;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf < bitbmp[((frameCount / 8) * 6)].Width - 9)
                                            Xf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Right
                                    for (int Xf = bitbmp[((frameCount / 8) * 6)].Width - 1; Xf > 0; Xf--)
                                    {
                                        for (int fram = ((frameCount / 8) * 6); fram < (frameCount / 8) * 7; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[((frameCount / 8) * 6)].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == GreyConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != GreyConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != bitbmp[((frameCount / 8) * 6)].Width - 1)
                                                    {
                                                        RegressR++;
                                                        Xf += 1;
                                                        Yf = -1;
                                                        fram = ((frameCount / 8) * 6);
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 7) - 1) && RegressR == -1 && Xf > 9)
                                                    right += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 7) - 1) && RegressR == -1 && Xf <= 9)
                                                    right++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 8) * 7) - 1) && RegressR != -1)
                                                {
                                                    right = right - RegressR;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf > 9)
                                            Xf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    for (int index = ((frameCount / 8) * 6); index < (frameCount / 8) * 7; index++)
                                    {
                                        Rectangle rect = new Rectangle(left, top, (bitbmp[index].Width - left - right), (bitbmp[index].Height - top - bot));
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        Bitmap myImage2 = bitbmp[index].Clone(rect, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);
                                        bitbmp[index] = myImage2;
                                    }

                                    //End of Canvas
                                    //posicao 0
                                    for (int index = ((frameCount / 8) * 4); index < (frameCount / 8) * 5; index++)
                                    {
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        bitbmp[index] = Utils.ConvertBmpAnimCV5(bitbmp[index], (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                                        edit.AddFrame(bitbmp[index]);
                                        TreeNode node = GetNode(CurrBody);
                                        if (node != null)
                                        {
                                            node.ForeColor = Color.Black;
                                            node.Nodes[CurrAction].ForeColor = Color.Black;
                                        }
                                        ListViewItem item;
                                        int i = edit.Frames.Count - 1;
                                        item = new ListViewItem(i.ToString(), 0);
                                        item.Tag = i;
                                        listView1.Items.Add(item);
                                        int width = listView1.TileSize.Width - 5;
                                        if (bmp.Width > listView1.TileSize.Width)
                                            width = bmp.Width;
                                        int height = listView1.TileSize.Height - 5;
                                        if (bmp.Height > listView1.TileSize.Height)
                                            height = bmp.Height;

                                        listView1.TileSize = new Size(width + 5, height + 5);
                                        trackBar2.Maximum = i;
                                        Options.ChangedUltimaClass["Animations"] = true;
                                        if (progressBar1.Value < progressBar1.Maximum)
                                        {
                                            progressBar1.Value++;
                                            progressBar1.Invalidate();
                                        }
                                        if (index == ((frameCount / 8) * 5) - 1)
                                            trackBar1.Value++;
                                    }
                                    //posicao 1
                                    edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                                    bmp.SelectActiveFrame(dimension, 0);
                                    edit.GetGifPalette(bmp);
                                    for (int index = 0; index < (frameCount / 8); index++)
                                    {
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        bitbmp[index] = Utils.ConvertBmpAnimCV5(bitbmp[index], (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                                        edit.AddFrame(bitbmp[index]);
                                        TreeNode node = GetNode(CurrBody);
                                        if (node != null)
                                        {
                                            node.ForeColor = Color.Black;
                                            node.Nodes[CurrAction].ForeColor = Color.Black;
                                        }
                                        ListViewItem item;
                                        int i = edit.Frames.Count - 1;
                                        item = new ListViewItem(i.ToString(), 0);
                                        item.Tag = i;
                                        listView1.Items.Add(item);
                                        int width = listView1.TileSize.Width - 5;
                                        if (bmp.Width > listView1.TileSize.Width)
                                            width = bmp.Width;
                                        int height = listView1.TileSize.Height - 5;
                                        if (bmp.Height > listView1.TileSize.Height)
                                            height = bmp.Height;

                                        listView1.TileSize = new Size(width + 5, height + 5);
                                        trackBar2.Maximum = i;
                                        Options.ChangedUltimaClass["Animations"] = true;
                                        if (progressBar1.Value < progressBar1.Maximum)
                                        {
                                            progressBar1.Value++;
                                            progressBar1.Invalidate();
                                        }
                                        if (index == (frameCount / 8) - 1)
                                            trackBar1.Value++;
                                    }
                                    //posicao 2
                                    edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                                    bmp.SelectActiveFrame(dimension, 0);
                                    edit.GetGifPalette(bmp);
                                    for (int index = ((frameCount / 8) * 5); index < (frameCount / 8) * 6; index++)
                                    {
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        bitbmp[index] = Utils.ConvertBmpAnimCV5(bitbmp[index], (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                                        edit.AddFrame(bitbmp[index]);
                                        TreeNode node = GetNode(CurrBody);
                                        if (node != null)
                                        {
                                            node.ForeColor = Color.Black;
                                            node.Nodes[CurrAction].ForeColor = Color.Black;
                                        }
                                        ListViewItem item;
                                        int i = edit.Frames.Count - 1;
                                        item = new ListViewItem(i.ToString(), 0);
                                        item.Tag = i;
                                        listView1.Items.Add(item);
                                        int width = listView1.TileSize.Width - 5;
                                        if (bmp.Width > listView1.TileSize.Width)
                                            width = bmp.Width;
                                        int height = listView1.TileSize.Height - 5;
                                        if (bmp.Height > listView1.TileSize.Height)
                                            height = bmp.Height;

                                        listView1.TileSize = new Size(width + 5, height + 5);
                                        trackBar2.Maximum = i;
                                        Options.ChangedUltimaClass["Animations"] = true;
                                        if (progressBar1.Value < progressBar1.Maximum)
                                        {
                                            progressBar1.Value++;
                                            progressBar1.Invalidate();
                                        }
                                        if (index == ((frameCount / 8) * 6) - 1)
                                            trackBar1.Value++;
                                    }
                                    //posicao 3
                                    edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                                    bmp.SelectActiveFrame(dimension, 0);
                                    edit.GetGifPalette(bmp);
                                    for (int index = ((frameCount / 8) * 1); index < (frameCount / 8) * 2; index++)
                                    {
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        bitbmp[index] = Utils.ConvertBmpAnimCV5(bitbmp[index], (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                                        edit.AddFrame(bitbmp[index]);
                                        TreeNode node = GetNode(CurrBody);
                                        if (node != null)
                                        {
                                            node.ForeColor = Color.Black;
                                            node.Nodes[CurrAction].ForeColor = Color.Black;
                                        }
                                        ListViewItem item;
                                        int i = edit.Frames.Count - 1;
                                        item = new ListViewItem(i.ToString(), 0);
                                        item.Tag = i;
                                        listView1.Items.Add(item);
                                        int width = listView1.TileSize.Width - 5;
                                        if (bmp.Width > listView1.TileSize.Width)
                                            width = bmp.Width;
                                        int height = listView1.TileSize.Height - 5;
                                        if (bmp.Height > listView1.TileSize.Height)
                                            height = bmp.Height;

                                        listView1.TileSize = new Size(width + 5, height + 5);
                                        trackBar2.Maximum = i;
                                        Options.ChangedUltimaClass["Animations"] = true;
                                        if (progressBar1.Value < progressBar1.Maximum)
                                        {
                                            progressBar1.Value++;
                                            progressBar1.Invalidate();
                                        }
                                        if (index == ((frameCount / 8) * 2) - 1)
                                            trackBar1.Value++;
                                    }
                                    //posicao 4
                                    edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                                    bmp.SelectActiveFrame(dimension, 0);
                                    edit.GetGifPalette(bmp);
                                    for (int index = ((frameCount / 8) * 6); index < (frameCount / 8) * 7; index++)
                                    {
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        bitbmp[index] = Utils.ConvertBmpAnimCV5(bitbmp[index], (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                                        edit.AddFrame(bitbmp[index]);
                                        TreeNode node = GetNode(CurrBody);
                                        if (node != null)
                                        {
                                            node.ForeColor = Color.Black;
                                            node.Nodes[CurrAction].ForeColor = Color.Black;
                                        }
                                        ListViewItem item;
                                        int i = edit.Frames.Count - 1;
                                        item = new ListViewItem(i.ToString(), 0);
                                        item.Tag = i;
                                        listView1.Items.Add(item);
                                        int width = listView1.TileSize.Width - 5;
                                        if (bmp.Width > listView1.TileSize.Width)
                                            width = bmp.Width;
                                        int height = listView1.TileSize.Height - 5;
                                        if (bmp.Height > listView1.TileSize.Height)
                                            height = bmp.Height;

                                        listView1.TileSize = new Size(width + 5, height + 5);
                                        trackBar2.Maximum = i;
                                        Options.ChangedUltimaClass["Animations"] = true;
                                        if (progressBar1.Value < progressBar1.Maximum)
                                        {
                                            progressBar1.Value++;
                                            progressBar1.Invalidate();
                                        }
                                    }
                                    progressBar1.Value = 0;
                                    progressBar1.Invalidate();
                                    SetPaletteBox();
                                    listView1.Invalidate();
                                    numericUpDownCx.Value = edit.Frames[trackBar2.Value].Center.X;
                                    numericUpDownCy.Value = edit.Frames[trackBar2.Value].Center.Y;
                                    Options.ChangedUltimaClass["Animations"] = true;
                                }
                            }
                            trackBar1.Enabled = true;
                        }
                    }
                }
                //Refresh List after Canvas reduction
                CurrDir = trackBar1.Value;
                AfterSelectTreeView(null, null);
            }
            catch (System.NullReferenceException) { trackBar1.Enabled = true; }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox2.Checked)
            {
                numericUpDown3.Enabled = true;
                numericUpDown4.Enabled = true;
                numericUpDown5.Enabled = true;
            }
            else
            {
                numericUpDown3.Enabled = false;
                numericUpDown4.Enabled = false;
                numericUpDown5.Enabled = false;
                numericUpDown3.Value = 255;
                numericUpDown4.Value = 255;
                numericUpDown5.Value = 255;
            }
        }
        //All directions Add KRframeViewer 
        private void allDirectionsAddWithCanvasKRframeEditorColorCorrectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileType != 0)
                {
                    using (OpenFileDialog dialog = new OpenFileDialog())
                    {
                        dialog.Multiselect = false;
                        dialog.Title = "Choose 1 Gif ( with all directions in KRframeViewer Style ) to add";
                        dialog.CheckFileExists = true;
                        dialog.Filter = "Gif files (*.gif;)|*.gif;";
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            Color CustomConvert = Color.FromArgb(255, (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                            trackBar1.Enabled = false;
                            trackBar1.Value = 0;
                            Bitmap bmp = new Bitmap(dialog.FileName);
                            AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                            if (edit != null)
                            {
                                //Gif Especial Properties
                                if (dialog.FileName.Contains(".gif"))
                                {
                                    FrameDimension dimension = new FrameDimension(bmp.FrameDimensionsList[0]);
                                    // Number of frames 
                                    int frameCount = bmp.GetFrameCount(dimension);
                                    progressBar1.Maximum = frameCount;
                                    bmp.SelectActiveFrame(dimension, 0);
                                    edit.GetGifPalette(bmp);
                                    Bitmap[] bitbmp = new Bitmap[frameCount];
                                    // Return an Image at a certain index 
                                    for (int index = 0; index < frameCount; index++)
                                    {
                                        bitbmp[index] = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format16bppArgb1555);
                                        bmp.SelectActiveFrame(dimension, index);
                                        bitbmp[index] = (Bitmap)bmp;
                                    }
                                    //Canvas algorithm
                                    int top = 0;
                                    int bot = 0;
                                    int left = 0;
                                    int right = 0;
                                    int RegressT = -1;
                                    int RegressB = -1;
                                    int RegressL = -1;
                                    int RegressR = -1;
                                    bool var = true;
                                    bool breakOK = false;
                                    // position 0
                                    //Top
                                    for (int Yf = 0; Yf < bitbmp[(frameCount / 5) * 0].Height; Yf++)
                                    {
                                        for (int fram = (frameCount / 5) * 0; fram < (frameCount / 5) * 1; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[(frameCount / 5) * 0].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != 0)
                                                    {
                                                        RegressT++;
                                                        Yf -= 1;
                                                        Xf = -1;
                                                        fram = (frameCount / 5) * 0;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 1) - 1) && RegressT == -1 && Yf < bitbmp[0].Height - 9)
                                                    top += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 1) - 1) && RegressT == -1 && Yf >= bitbmp[0].Height - 9)
                                                    top++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 1) - 1) && RegressT != -1)
                                                {
                                                    top = top - RegressT;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf < bitbmp[(frameCount / 5) * 0].Height - 9)
                                            Yf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Bot
                                    for (int Yf = bitbmp[(frameCount / 5) * 0].Height - 1; Yf > 0; Yf--)
                                    {
                                        for (int fram = (frameCount / 5) * 0; fram < (frameCount / 5) * 1; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[(frameCount / 5) * 0].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != bitbmp[(frameCount / 5) * 0].Height - 1)
                                                    {
                                                        RegressB++;
                                                        Yf += 1;
                                                        Xf = -1;
                                                        fram = (frameCount / 5) * 0;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 1) - 1) && RegressB == -1 && Yf > 9)
                                                    bot += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 1) - 1) && RegressB == -1 && Yf <= 9)
                                                    bot++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 1) - 1) && RegressB != -1)
                                                {
                                                    bot = bot - RegressB;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf > 9)
                                            Yf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Left

                                    for (int Xf = 0; Xf < bitbmp[(frameCount / 5) * 0].Width; Xf++)
                                    {
                                        for (int fram = (frameCount / 5) * 0; fram < (frameCount / 5) * 1; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[(frameCount / 5) * 0].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != 0)
                                                    {
                                                        RegressL++;
                                                        Xf -= 1;
                                                        Yf = -1;
                                                        fram = (frameCount / 5) * 0;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 1) - 1) && RegressL == -1 && Xf < bitbmp[0].Width - 9)
                                                    left += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 1) - 1) && RegressL == -1 && Xf >= bitbmp[0].Width - 9)
                                                    left++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 1) - 1) && RegressL != -1)
                                                {
                                                    left = left - RegressL;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf < bitbmp[(frameCount / 5) * 0].Width - 9)
                                            Xf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Right
                                    for (int Xf = bitbmp[(frameCount / 5) * 0].Width - 1; Xf > 0; Xf--)
                                    {
                                        for (int fram = (frameCount / 5) * 0; fram < (frameCount / 5) * 1; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[(frameCount / 5) * 0].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != bitbmp[(frameCount / 5) * 0].Width - 1)
                                                    {
                                                        RegressR++;
                                                        Xf += 1;
                                                        Yf = -1;
                                                        fram = (frameCount / 5) * 0;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 1) - 1) && RegressR == -1 && Xf > 9)
                                                    right += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 1) - 1) && RegressR == -1 && Xf <= 9)
                                                    right++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 1) - 1) && RegressR != -1)
                                                {
                                                    right = right - RegressR;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf > 9)
                                            Xf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    for (int index = (frameCount / 5) * 0; index < (frameCount / 5) * 1; index++)
                                    {
                                        Rectangle rect = new Rectangle(left, top, (bitbmp[index].Width - left - right), (bitbmp[index].Height - top - bot));
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        Bitmap myImage2 = bitbmp[index].Clone(rect, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);
                                        bitbmp[index] = myImage2;
                                    }

                                    //Reseta cordenadas
                                    top = 0;
                                    bot = 0;
                                    left = 0;
                                    right = 0;
                                    RegressT = -1;
                                    RegressB = -1;
                                    RegressL = -1;
                                    RegressR = -1;
                                    var = true;
                                    breakOK = false;
                                    // position 1
                                    //Top
                                    for (int Yf = 0; Yf < bitbmp[(frameCount / 5) * 1].Height; Yf++)
                                    {
                                        for (int fram = (frameCount / 5) * 1; fram < (frameCount / 5) * 2; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[(frameCount / 5) * 1].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != 0)
                                                    {
                                                        RegressT++;
                                                        Yf -= 1;
                                                        Xf = -1;
                                                        fram = (frameCount / 5) * 1;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 2) - 1) && RegressT == -1 && Yf < bitbmp[0].Height - 9)
                                                    top += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 2) - 1) && RegressT == -1 && Yf >= bitbmp[0].Height - 9)
                                                    top++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 2) - 1) && RegressT != -1)
                                                {
                                                    top = top - RegressT;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf < bitbmp[(frameCount / 5) * 1].Height - 9)
                                            Yf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Bot
                                    for (int Yf = bitbmp[(frameCount / 5) * 1].Height - 1; Yf > 0; Yf--)
                                    {
                                        for (int fram = (frameCount / 5) * 1; fram < (frameCount / 5) * 2; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[(frameCount / 5) * 1].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != bitbmp[(frameCount / 5) * 1].Height - 1)
                                                    {
                                                        RegressB++;
                                                        Yf += 1;
                                                        Xf = -1;
                                                        fram = (frameCount / 5) * 1;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 2) - 1) && RegressB == -1 && Yf > 9)
                                                    bot += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 2) - 1) && RegressB == -1 && Yf <= 9)
                                                    bot++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 2) - 1) && RegressB != -1)
                                                {
                                                    bot = bot - RegressB;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf > 9)
                                            Yf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Left

                                    for (int Xf = 0; Xf < bitbmp[(frameCount / 5) * 1].Width; Xf++)
                                    {
                                        for (int fram = (frameCount / 5) * 1; fram < (frameCount / 5) * 2; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[(frameCount / 5) * 1].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != 0)
                                                    {
                                                        RegressL++;
                                                        Xf -= 1;
                                                        Yf = -1;
                                                        fram = (frameCount / 5) * 1;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 2) - 1) && RegressL == -1 && Xf < bitbmp[0].Width - 9)
                                                    left += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 2) - 1) && RegressL == -1 && Xf >= bitbmp[0].Width - 9)
                                                    left++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 2) - 1) && RegressL != -1)
                                                {
                                                    left = left - RegressL;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf < bitbmp[(frameCount / 5) * 1].Width - 9)
                                            Xf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Right
                                    for (int Xf = bitbmp[(frameCount / 5) * 1].Width - 1; Xf > 0; Xf--)
                                    {
                                        for (int fram = (frameCount / 5) * 1; fram < (frameCount / 5) * 2; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[(frameCount / 5) * 1].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != bitbmp[(frameCount / 5) * 1].Width - 1)
                                                    {
                                                        RegressR++;
                                                        Xf += 1;
                                                        Yf = -1;
                                                        fram = (frameCount / 5) * 1;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 2) - 1) && RegressR == -1 && Xf > 9)
                                                    right += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 2) - 1) && RegressR == -1 && Xf <= 9)
                                                    right++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 2) - 1) && RegressR != -1)
                                                {
                                                    right = right - RegressR;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf > 9)
                                            Xf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    for (int index = (frameCount / 5) * 1; index < (frameCount / 5) * 2; index++)
                                    {
                                        Rectangle rect = new Rectangle(left, top, (bitbmp[index].Width - left - right), (bitbmp[index].Height - top - bot));
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        Bitmap myImage2 = bitbmp[index].Clone(rect, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);
                                        bitbmp[index] = myImage2;
                                    }

                                    //Reseta cordenadas
                                    top = 0;
                                    bot = 0;
                                    left = 0;
                                    right = 0;
                                    RegressT = -1;
                                    RegressB = -1;
                                    RegressL = -1;
                                    RegressR = -1;
                                    var = true;
                                    breakOK = false;
                                    // position 2
                                    //Top
                                    for (int Yf = 0; Yf < bitbmp[(frameCount / 5) * 2].Height; Yf++)
                                    {
                                        for (int fram = (frameCount / 5) * 2; fram < (frameCount / 5) * 3; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[(frameCount / 5) * 2].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != 0)
                                                    {
                                                        RegressT++;
                                                        Yf -= 1;
                                                        Xf = -1;
                                                        fram = (frameCount / 5) * 2;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 3) - 1) && RegressT == -1 && Yf < bitbmp[0].Height - 9)
                                                    top += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 3) - 1) && RegressT == -1 && Yf >= bitbmp[0].Height - 9)
                                                    top++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 3) - 1) && RegressT != -1)
                                                {
                                                    top = top - RegressT;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf < bitbmp[(frameCount / 5) * 2].Height - 9)
                                            Yf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Bot
                                    for (int Yf = bitbmp[(frameCount / 5) * 2].Height - 1; Yf > 0; Yf--)
                                    {
                                        for (int fram = (frameCount / 5) * 2; fram < (frameCount / 5) * 3; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[(frameCount / 5) * 2].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != bitbmp[(frameCount / 5) * 2].Height - 1)
                                                    {
                                                        RegressB++;
                                                        Yf += 1;
                                                        Xf = -1;
                                                        fram = (frameCount / 5) * 2;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 3) - 1) && RegressB == -1 && Yf > 9)
                                                    bot += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 3) - 1) && RegressB == -1 && Yf <= 9)
                                                    bot++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 3) - 1) && RegressB != -1)
                                                {
                                                    bot = bot - RegressB;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf > 9)
                                            Yf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Left

                                    for (int Xf = 0; Xf < bitbmp[(frameCount / 5) * 2].Width; Xf++)
                                    {
                                        for (int fram = (frameCount / 5) * 2; fram < (frameCount / 5) * 3; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[(frameCount / 5) * 2].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != 0)
                                                    {
                                                        RegressL++;
                                                        Xf -= 1;
                                                        Yf = -1;
                                                        fram = (frameCount / 5) * 2;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 3) - 1) && RegressL == -1 && Xf < bitbmp[0].Width - 9)
                                                    left += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 3) - 1) && RegressL == -1 && Xf >= bitbmp[0].Width - 9)
                                                    left++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 3) - 1) && RegressL != -1)
                                                {
                                                    left = left - RegressL;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf < bitbmp[(frameCount / 5) * 2].Width - 9)
                                            Xf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Right
                                    for (int Xf = bitbmp[(frameCount / 5) * 2].Width - 1; Xf > 0; Xf--)
                                    {
                                        for (int fram = (frameCount / 5) * 2; fram < (frameCount / 5) * 3; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[(frameCount / 5) * 2].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != bitbmp[(frameCount / 5) * 2].Width - 1)
                                                    {
                                                        RegressR++;
                                                        Xf += 1;
                                                        Yf = -1;
                                                        fram = (frameCount / 5) * 2;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 3) - 1) && RegressR == -1 && Xf > 9)
                                                    right += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 3) - 1) && RegressR == -1 && Xf <= 9)
                                                    right++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 3) - 1) && RegressR != -1)
                                                {
                                                    right = right - RegressR;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf > 9)
                                            Xf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    for (int index = (frameCount / 5) * 2; index < (frameCount / 5) * 3; index++)
                                    {
                                        Rectangle rect = new Rectangle(left, top, (bitbmp[index].Width - left - right), (bitbmp[index].Height - top - bot));
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        Bitmap myImage2 = bitbmp[index].Clone(rect, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);
                                        bitbmp[index] = myImage2;
                                    }

                                    //Reseta cordenadas
                                    top = 0;
                                    bot = 0;
                                    left = 0;
                                    right = 0;
                                    RegressT = -1;
                                    RegressB = -1;
                                    RegressL = -1;
                                    RegressR = -1;
                                    var = true;
                                    breakOK = false;
                                    // position 3
                                    //Top
                                    for (int Yf = 0; Yf < bitbmp[(frameCount / 5) * 3].Height; Yf++)
                                    {
                                        for (int fram = (frameCount / 5) * 3; fram < (frameCount / 5) * 4; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[(frameCount / 5) * 3].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != 0)
                                                    {
                                                        RegressT++;
                                                        Yf -= 1;
                                                        Xf = -1;
                                                        fram = (frameCount / 5) * 3;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 4) - 1) && RegressT == -1 && Yf < bitbmp[0].Height - 9)
                                                    top += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 4) - 1) && RegressT == -1 && Yf >= bitbmp[0].Height - 9)
                                                    top++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 4) - 1) && RegressT != -1)
                                                {
                                                    top = top - RegressT;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf < bitbmp[(frameCount / 5) * 3].Height - 9)
                                            Yf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Bot
                                    for (int Yf = bitbmp[(frameCount / 5) * 3].Height - 1; Yf > 0; Yf--)
                                    {
                                        for (int fram = (frameCount / 5) * 3; fram < (frameCount / 5) * 4; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[(frameCount / 5) * 3].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != bitbmp[(frameCount / 5) * 3].Height - 1)
                                                    {
                                                        RegressB++;
                                                        Yf += 1;
                                                        Xf = -1;
                                                        fram = (frameCount / 5) * 3;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 4) - 1) && RegressB == -1 && Yf > 9)
                                                    bot += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 4) - 1) && RegressB == -1 && Yf <= 9)
                                                    bot++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 4) - 1) && RegressB != -1)
                                                {
                                                    bot = bot - RegressB;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf > 9)
                                            Yf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Left

                                    for (int Xf = 0; Xf < bitbmp[(frameCount / 5) * 3].Width; Xf++)
                                    {
                                        for (int fram = (frameCount / 5) * 3; fram < (frameCount / 5) * 4; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[(frameCount / 5) * 3].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != 0)
                                                    {
                                                        RegressL++;
                                                        Xf -= 1;
                                                        Yf = -1;
                                                        fram = (frameCount / 5) * 3;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 4) - 1) && RegressL == -1 && Xf < bitbmp[0].Width - 9)
                                                    left += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 4) - 1) && RegressL == -1 && Xf >= bitbmp[0].Width - 9)
                                                    left++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 4) - 1) && RegressL != -1)
                                                {
                                                    left = left - RegressL;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf < bitbmp[(frameCount / 5) * 3].Width - 9)
                                            Xf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Right
                                    for (int Xf = bitbmp[(frameCount / 5) * 3].Width - 1; Xf > 0; Xf--)
                                    {
                                        for (int fram = (frameCount / 5) * 3; fram < (frameCount / 5) * 4; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[(frameCount / 5) * 3].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != bitbmp[(frameCount / 5) * 3].Width - 1)
                                                    {
                                                        RegressR++;
                                                        Xf += 1;
                                                        Yf = -1;
                                                        fram = (frameCount / 5) * 3;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 4) - 1) && RegressR == -1 && Xf > 9)
                                                    right += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 4) - 1) && RegressR == -1 && Xf <= 9)
                                                    right++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 4) - 1) && RegressR != -1)
                                                {
                                                    right = right - RegressR;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf > 9)
                                            Xf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    for (int index = (frameCount / 5) * 3; index < (frameCount / 5) * 4; index++)
                                    {
                                        Rectangle rect = new Rectangle(left, top, (bitbmp[index].Width - left - right), (bitbmp[index].Height - top - bot));
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        Bitmap myImage2 = bitbmp[index].Clone(rect, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);
                                        bitbmp[index] = myImage2;
                                    }

                                    //Reseta cordenadas
                                    top = 0;
                                    bot = 0;
                                    left = 0;
                                    right = 0;
                                    RegressT = -1;
                                    RegressB = -1;
                                    RegressL = -1;
                                    RegressR = -1;
                                    var = true;
                                    breakOK = false;
                                    // position 4
                                    //Top
                                    for (int Yf = 0; Yf < bitbmp[(frameCount / 5) * 4].Height; Yf++)
                                    {
                                        for (int fram = (frameCount / 5) * 4; fram < (frameCount / 5) * 5; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[(frameCount / 5) * 4].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != 0)
                                                    {
                                                        RegressT++;
                                                        Yf -= 1;
                                                        Xf = -1;
                                                        fram = (frameCount / 5) * 4;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 5) - 1) && RegressT == -1 && Yf < bitbmp[0].Height - 9)
                                                    top += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 5) - 1) && RegressT == -1 && Yf >= bitbmp[0].Height - 9)
                                                    top++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 5) - 1) && RegressT != -1)
                                                {
                                                    top = top - RegressT;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf < bitbmp[(frameCount / 5) * 4].Height - 9)
                                            Yf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Bot
                                    for (int Yf = bitbmp[(frameCount / 5) * 4].Height - 1; Yf > 0; Yf--)
                                    {
                                        for (int fram = (frameCount / 5) * 4; fram < (frameCount / 5) * 5; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Xf = 0; Xf < bitbmp[(frameCount / 5) * 4].Width; Xf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Yf != bitbmp[(frameCount / 5) * 4].Height - 1)
                                                    {
                                                        RegressB++;
                                                        Yf += 1;
                                                        Xf = -1;
                                                        fram = (frameCount / 5) * 4;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 5) - 1) && RegressB == -1 && Yf > 9)
                                                    bot += 10;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 5) - 1) && RegressB == -1 && Yf <= 9)
                                                    bot++;
                                                if (var == true && Xf == bitbmp[fram].Width - 1 && fram == (((frameCount / 5) * 5) - 1) && RegressB != -1)
                                                {
                                                    bot = bot - RegressB;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Yf > 9)
                                            Yf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Left

                                    for (int Xf = 0; Xf < bitbmp[(frameCount / 5) * 4].Width; Xf++)
                                    {
                                        for (int fram = (frameCount / 5) * 4; fram < (frameCount / 5) * 5; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[(frameCount / 5) * 4].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != 0)
                                                    {
                                                        RegressL++;
                                                        Xf -= 1;
                                                        Yf = -1;
                                                        fram = (frameCount / 5) * 4;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 5) - 1) && RegressL == -1 && Xf < bitbmp[0].Width - 9)
                                                    left += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 5) - 1) && RegressL == -1 && Xf >= bitbmp[0].Width - 9)
                                                    left++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 5) - 1) && RegressL != -1)
                                                {
                                                    left = left - RegressL;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf < bitbmp[(frameCount / 5) * 4].Width - 9)
                                            Xf += 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    //Right
                                    for (int Xf = bitbmp[(frameCount / 5) * 4].Width - 1; Xf > 0; Xf--)
                                    {
                                        for (int fram = (frameCount / 5) * 4; fram < (frameCount / 5) * 5; fram++)
                                        {
                                            bitbmp[fram].SelectActiveFrame(dimension, fram);
                                            for (int Yf = 0; Yf < bitbmp[(frameCount / 5) * 4].Height; Yf++)
                                            {
                                                Color pixel = bitbmp[fram].GetPixel(Xf, Yf);
                                                if (pixel == WhiteConvert | pixel == CustomConvert | pixel.A == 0)
                                                    var = true;
                                                if (pixel != WhiteConvert & pixel != CustomConvert & pixel.A != 0)
                                                {
                                                    var = false;
                                                    if (Xf != bitbmp[(frameCount / 5) * 4].Width - 1)
                                                    {
                                                        RegressR++;
                                                        Xf += 1;
                                                        Yf = -1;
                                                        fram = (frameCount / 5) * 4;
                                                    }
                                                    else
                                                    {
                                                        breakOK = true;
                                                        break;
                                                    }
                                                }
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 5) - 1) && RegressR == -1 && Xf > 9)
                                                    right += 10;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 5) - 1) && RegressR == -1 && Xf <= 9)
                                                    right++;
                                                if (var == true && Yf == bitbmp[fram].Height - 1 && fram == (((frameCount / 5) * 5) - 1) && RegressR != -1)
                                                {
                                                    right = right - RegressR;
                                                    breakOK = true;
                                                    break;
                                                }
                                            }
                                            if (breakOK == true)
                                                break;
                                        }
                                        if (Xf > 9)
                                            Xf -= 9; // 1 of for + 9
                                        if (breakOK == true)
                                        {
                                            breakOK = false;
                                            break;
                                        }
                                    }
                                    for (int index = (frameCount / 5) * 4; index < (frameCount / 5) * 5; index++)
                                    {
                                        Rectangle rect = new Rectangle(left, top, (bitbmp[index].Width - left - right), (bitbmp[index].Height - top - bot));
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        Bitmap myImage2 = bitbmp[index].Clone(rect, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);
                                        bitbmp[index] = myImage2;
                                    }

                                    //End of Canvas
                                    //posicao 0
                                    for (int index = ((frameCount / 5) * 0); index < (frameCount / 5) * 1; index++)
                                    {
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        bitbmp[index] = Utils.ConvertBmpAnimKR(bitbmp[index], (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                                        edit.AddFrame(bitbmp[index]);
                                        TreeNode node = GetNode(CurrBody);
                                        if (node != null)
                                        {
                                            node.ForeColor = Color.Black;
                                            node.Nodes[CurrAction].ForeColor = Color.Black;
                                        }
                                        ListViewItem item;
                                        int i = edit.Frames.Count - 1;
                                        item = new ListViewItem(i.ToString(), 0);
                                        item.Tag = i;
                                        listView1.Items.Add(item);
                                        int width = listView1.TileSize.Width - 5;
                                        if (bmp.Width > listView1.TileSize.Width)
                                            width = bmp.Width;
                                        int height = listView1.TileSize.Height - 5;
                                        if (bmp.Height > listView1.TileSize.Height)
                                            height = bmp.Height;

                                        listView1.TileSize = new Size(width + 5, height + 5);
                                        trackBar2.Maximum = i;
                                        Options.ChangedUltimaClass["Animations"] = true;
                                        if (progressBar1.Value < progressBar1.Maximum)
                                        {
                                            progressBar1.Value++;
                                            progressBar1.Invalidate();
                                        }
                                        if (index == ((frameCount / 5) * 1) - 1)
                                            trackBar1.Value++;
                                    }
                                    //posicao 1
                                    edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                                    bmp.SelectActiveFrame(dimension, 0);
                                    edit.GetGifPalette(bmp);
                                    for (int index = ((frameCount / 5) * 1); index < ((frameCount / 5) * 2); index++)
                                    {
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        bitbmp[index] = Utils.ConvertBmpAnimKR(bitbmp[index], (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                                        edit.AddFrame(bitbmp[index]);
                                        TreeNode node = GetNode(CurrBody);
                                        if (node != null)
                                        {
                                            node.ForeColor = Color.Black;
                                            node.Nodes[CurrAction].ForeColor = Color.Black;
                                        }
                                        ListViewItem item;
                                        int i = edit.Frames.Count - 1;
                                        item = new ListViewItem(i.ToString(), 0);
                                        item.Tag = i;
                                        listView1.Items.Add(item);
                                        int width = listView1.TileSize.Width - 5;
                                        if (bmp.Width > listView1.TileSize.Width)
                                            width = bmp.Width;
                                        int height = listView1.TileSize.Height - 5;
                                        if (bmp.Height > listView1.TileSize.Height)
                                            height = bmp.Height;

                                        listView1.TileSize = new Size(width + 5, height + 5);
                                        trackBar2.Maximum = i;
                                        Options.ChangedUltimaClass["Animations"] = true;
                                        if (progressBar1.Value < progressBar1.Maximum)
                                        {
                                            progressBar1.Value++;
                                            progressBar1.Invalidate();
                                        }
                                        if (index == ((frameCount / 5) * 2) - 1)
                                            trackBar1.Value++;
                                    }
                                    //posicao 2
                                    edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                                    bmp.SelectActiveFrame(dimension, 0);
                                    edit.GetGifPalette(bmp);
                                    for (int index = ((frameCount / 5) * 2); index < (frameCount / 5) * 3; index++)
                                    {
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        bitbmp[index] = Utils.ConvertBmpAnimKR(bitbmp[index], (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                                        edit.AddFrame(bitbmp[index]);
                                        TreeNode node = GetNode(CurrBody);
                                        if (node != null)
                                        {
                                            node.ForeColor = Color.Black;
                                            node.Nodes[CurrAction].ForeColor = Color.Black;
                                        }
                                        ListViewItem item;
                                        int i = edit.Frames.Count - 1;
                                        item = new ListViewItem(i.ToString(), 0);
                                        item.Tag = i;
                                        listView1.Items.Add(item);
                                        int width = listView1.TileSize.Width - 5;
                                        if (bmp.Width > listView1.TileSize.Width)
                                            width = bmp.Width;
                                        int height = listView1.TileSize.Height - 5;
                                        if (bmp.Height > listView1.TileSize.Height)
                                            height = bmp.Height;

                                        listView1.TileSize = new Size(width + 5, height + 5);
                                        trackBar2.Maximum = i;
                                        Options.ChangedUltimaClass["Animations"] = true;
                                        if (progressBar1.Value < progressBar1.Maximum)
                                        {
                                            progressBar1.Value++;
                                            progressBar1.Invalidate();
                                        }
                                        if (index == ((frameCount / 5) * 3) - 1)
                                            trackBar1.Value++;
                                    }
                                    //posicao 3
                                    edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                                    bmp.SelectActiveFrame(dimension, 0);
                                    edit.GetGifPalette(bmp);
                                    for (int index = ((frameCount / 5) * 3); index < (frameCount / 5) * 4; index++)
                                    {
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        bitbmp[index] = Utils.ConvertBmpAnimKR(bitbmp[index], (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                                        edit.AddFrame(bitbmp[index]);
                                        TreeNode node = GetNode(CurrBody);
                                        if (node != null)
                                        {
                                            node.ForeColor = Color.Black;
                                            node.Nodes[CurrAction].ForeColor = Color.Black;
                                        }
                                        ListViewItem item;
                                        int i = edit.Frames.Count - 1;
                                        item = new ListViewItem(i.ToString(), 0);
                                        item.Tag = i;
                                        listView1.Items.Add(item);
                                        int width = listView1.TileSize.Width - 5;
                                        if (bmp.Width > listView1.TileSize.Width)
                                            width = bmp.Width;
                                        int height = listView1.TileSize.Height - 5;
                                        if (bmp.Height > listView1.TileSize.Height)
                                            height = bmp.Height;

                                        listView1.TileSize = new Size(width + 5, height + 5);
                                        trackBar2.Maximum = i;
                                        Options.ChangedUltimaClass["Animations"] = true;
                                        if (progressBar1.Value < progressBar1.Maximum)
                                        {
                                            progressBar1.Value++;
                                            progressBar1.Invalidate();
                                        }
                                        if (index == ((frameCount / 5) * 4) - 1)
                                            trackBar1.Value++;
                                    }
                                    //posicao 4
                                    edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                                    bmp.SelectActiveFrame(dimension, 0);
                                    edit.GetGifPalette(bmp);
                                    for (int index = ((frameCount / 5) * 4); index < (frameCount / 5) * 5; index++)
                                    {
                                        bitbmp[index].SelectActiveFrame(dimension, index);
                                        bitbmp[index] = Utils.ConvertBmpAnimKR(bitbmp[index], (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
                                        edit.AddFrame(bitbmp[index]);
                                        TreeNode node = GetNode(CurrBody);
                                        if (node != null)
                                        {
                                            node.ForeColor = Color.Black;
                                            node.Nodes[CurrAction].ForeColor = Color.Black;
                                        }
                                        ListViewItem item;
                                        int i = edit.Frames.Count - 1;
                                        item = new ListViewItem(i.ToString(), 0);
                                        item.Tag = i;
                                        listView1.Items.Add(item);
                                        int width = listView1.TileSize.Width - 5;
                                        if (bmp.Width > listView1.TileSize.Width)
                                            width = bmp.Width;
                                        int height = listView1.TileSize.Height - 5;
                                        if (bmp.Height > listView1.TileSize.Height)
                                            height = bmp.Height;

                                        listView1.TileSize = new Size(width + 5, height + 5);
                                        trackBar2.Maximum = i;
                                        Options.ChangedUltimaClass["Animations"] = true;
                                        if (progressBar1.Value < progressBar1.Maximum)
                                        {
                                            progressBar1.Value++;
                                            progressBar1.Invalidate();
                                        }
                                    }
                                    progressBar1.Value = 0;
                                    progressBar1.Invalidate();
                                    SetPaletteBox();
                                    listView1.Invalidate();
                                    numericUpDownCx.Value = edit.Frames[trackBar2.Value].Center.X;
                                    numericUpDownCy.Value = edit.Frames[trackBar2.Value].Center.Y;
                                    Options.ChangedUltimaClass["Animations"] = true;
                                }
                            }
                            trackBar1.Enabled = true;
                        }
                    }
                }
                //Refresh List after Canvas reduction
                CurrDir = trackBar1.Value;
                AfterSelectTreeView(null, null);
            }
            catch (System.NullReferenceException) { trackBar1.Enabled = true; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < 5; x++)
            {
                //RGB
                if (radioButton1.Checked == true)
                {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                    if (edit != null)
                    {
                        edit.PaletteConversor(1);
                    }
                }
                //RBG
                if (radioButton2.Checked == true)
                {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                    if (edit != null)
                    {
                        edit.PaletteConversor(2);
                    }
                }
                //GRB
                if (radioButton3.Checked == true)
                {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                    if (edit != null)
                    {
                        edit.PaletteConversor(3);
                    }
                }
                //GBR
                if (radioButton4.Checked == true)
                {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                    if (edit != null)
                    {
                        edit.PaletteConversor(4);
                    }
                }
                //BGR
                if (radioButton5.Checked == true)
                {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                    if (edit != null)
                    {
                        edit.PaletteConversor(5);
                    }
                }
                //BRG
                if (radioButton6.Checked == true)
                {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                    if (edit != null)
                    {
                        edit.PaletteConversor(6);
                    }
                }
                SetPaletteBox();
                listView1.Invalidate();
                Options.ChangedUltimaClass["Animations"] = true;
                SetPaletteBox();
                if (trackBar1.Value != trackBar1.Maximum)
                    trackBar1.Value++;
                else
                    trackBar1.Value = 0;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            for (int x = 0; x < 5; x++)
            {
                AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                if (edit != null)
                {
                    edit.PaletteConversor(2);
                }
                SetPaletteBox();
                listView1.Invalidate();
                Options.ChangedUltimaClass["Animations"] = true;
                SetPaletteBox();
                if (trackBar1.Value != trackBar1.Maximum)
                    trackBar1.Value++;
                else
                    trackBar1.Value = 0;
            }

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            for (int x = 0; x < 5; x++)
            {
                AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                if (edit != null)
                {
                    edit.PaletteConversor(3);
                }
                SetPaletteBox();
                listView1.Invalidate();
                Options.ChangedUltimaClass["Animations"] = true;
                SetPaletteBox();
                if (trackBar1.Value != trackBar1.Maximum)
                    trackBar1.Value++;
                else
                    trackBar1.Value = 0;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked == false)
            {
                for (int x = 0; x < 5; x++)
                {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                    if (edit != null)
                    {
                        edit.PaletteConversor(4);
                        edit.PaletteConversor(4);
                    }
                    SetPaletteBox();
                    listView1.Invalidate();
                    Options.ChangedUltimaClass["Animations"] = true;
                    SetPaletteBox();
                    if (trackBar1.Value != trackBar1.Maximum)
                        trackBar1.Value++;
                    else
                        trackBar1.Value = 0;
                }
            }
            else
            {
                for (int x = 0; x < 5; x++)
                {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                    if (edit != null)
                    {
                        edit.PaletteConversor(4);
                    }
                    SetPaletteBox();
                    listView1.Invalidate();
                    Options.ChangedUltimaClass["Animations"] = true;
                    SetPaletteBox();
                    if (trackBar1.Value != trackBar1.Maximum)
                        trackBar1.Value++;
                    else
                        trackBar1.Value = 0;
                }
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            for (int x = 0; x < 5; x++)
            {
                AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                if (edit != null)
                {
                    edit.PaletteConversor(5);
                }
                SetPaletteBox();
                listView1.Invalidate();
                Options.ChangedUltimaClass["Animations"] = true;
                SetPaletteBox();
                if (trackBar1.Value != trackBar1.Maximum)
                    trackBar1.Value++;
                else
                    trackBar1.Value = 0;
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked == false)
            {
                for (int x = 0; x < 5; x++)
                {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                    if (edit != null)
                    {
                        edit.PaletteConversor(6);
                        edit.PaletteConversor(6);
                    }
                    SetPaletteBox();
                    listView1.Invalidate();
                    Options.ChangedUltimaClass["Animations"] = true;
                    SetPaletteBox();
                    if (trackBar1.Value != trackBar1.Maximum)
                        trackBar1.Value++;
                    else
                        trackBar1.Value = 0;
                }
            }
            else
            {
                for (int x = 0; x < 5; x++)
                {
                    AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                    if (edit != null)
                    {
                        edit.PaletteConversor(6);
                    }
                    SetPaletteBox();
                    listView1.Invalidate();
                    Options.ChangedUltimaClass["Animations"] = true;
                    SetPaletteBox();
                    if (trackBar1.Value != trackBar1.Maximum)
                        trackBar1.Value++;
                    else
                        trackBar1.Value = 0;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < 5; x++)
            {
                AnimIdx edit = Ultima.AnimationEdit.GetAnimation(FileType, CurrBody, CurrAction, CurrDir);
                if (edit != null)
                {
                    edit.PaletteReductor((int)numericUpDown6.Value, (int)numericUpDown7.Value, (int)numericUpDown8.Value);
                }
                SetPaletteBox();
                listView1.Invalidate();
                Options.ChangedUltimaClass["Animations"] = true;
                SetPaletteBox();
                if (trackBar1.Value != trackBar1.Maximum)
                    trackBar1.Value++;
                else
                    trackBar1.Value = 0;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void repackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Анализ списка анимации
            int[] L = new int[6], H = new int[6], P = new int[6];
            int[][][] body = new int[6][][]; body[0] = null;
            for (int f = 1; f <= 5; ++f) {
                L[f] = H[f] = P[f] = 0; body[f] = new int[3][];
                List<int> Ll = new List<int>(), Hl = new List<int>(), Pl = new List<int>();
                for (int b = 0; b < Animations.GetAnimCount(f); ++b) {
                    var valid = false;
                    var actions = Animations.GetAnimLength(b, f);
                    for (int a = 0; a < actions; ++a)
                        for (int d = 0; d < 5; ++d)
                            if (Animations.IsAnimDefinied(b, a, d, f)) {
                                valid = true;
                            }                  
                    if (valid) switch (actions) {
                        case 13: ++L[f]; Ll.Add(b); break;
                        case 22: ++H[f]; Hl.Add(b); break;
                        case 35: ++P[f]; Pl.Add(b); break;
                    }  
                }
                body[f][0] = Ll.ToArray();
                body[f][1] = Hl.ToArray();
                body[f][2] = Pl.ToArray();
            }

            #region Чистка 
            /*
            for (int f = 1; f <= 5; ++f)
                for (int t = 0; t <= 2; ++t)
                    for (int b = 0; b < body[f][t].Length; ++b)
                        for (int a = 0; a < Animations.GetAnimLength(body[f][t][b], f); ++a)
                            for (int d = 0; d < 5; ++d) {
                                var edit = Ultima.AnimationEdit.GetAnimation(f, body[f][t][b], a, d);
                                if (edit != null) edit.ClearFrames();
                            }
            for (int f = 1; f < 6; ++f)
                Ultima.AnimationEdit.Save(f, FiddlerControls.Options.OutputPath);
            MessageBox.Show(String.Format("AnimationFiles saved to {0}", FiddlerControls.Options.OutputPath),
                            "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            return;
            */
            #endregion
            #region Перенос 
            /*
            int f_from = 4, f_targ = 2;
            for (int t = 0; t <= 2; ++t)
                for (int b = 0; b < body[f_from][t].Length; ++b) {
                    var stream = new MemoryStream();
                    Ultima.AnimationEdit.ExportToVD(f_from, body[f_from][t][b], new BinaryWriter(stream));
                    stream.Position = 0;
                    Ultima.AnimationEdit.LoadFromVD(f_targ, body[f_from][t][b], new BinaryReader(stream));
                }
            Ultima.AnimationEdit.Save(f_targ, FiddlerControls.Options.OutputPath);
            MessageBox.Show(String.Format("AnimationFiles saved to {0}", FiddlerControls.Options.OutputPath),
                            "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            return;
            */
            #endregion
            #region Переупаковка 
            /*
            int f_targ = 4;
            var b_strm = new MemoryStream[400];
            for (int i_h=-1, i_l=199, f = 1; f <= 5; ++f) {
                for (int b = 0; b < body[f][1].Length; ++b)
                    Ultima.AnimationEdit.ExportToVD(f, body[f][1][b], new BinaryWriter(b_strm[++i_h] = new MemoryStream()));
                for (int b = 0; b < body[f][0].Length; ++b)
                    Ultima.AnimationEdit.ExportToVD(f, body[f][0][b], new BinaryWriter(b_strm[++i_l] = new MemoryStream()));
            }
            for (int t = 0; t <= 2; ++t)
                for (int b = 0; b < body[f_targ][t].Length; ++b)
                    for (int a = 0; a < Animations.GetAnimLength(body[f_targ][t][b], f_targ); ++a)
                        for (int d = 0; d < 5; ++d) {
                            var edit = Ultima.AnimationEdit.GetAnimation(f_targ, body[f_targ][t][b], a, d);
                            if (edit != null) edit.ClearFrames();
                        }
            for (int b_targ = -1, b = 0; b < 200; ++b) {
                if (b >= b_strm.Length || b_strm[b] == null) continue;
                b_strm[b].Position = 0;
                while (b_targ+1 ==  5 || b_targ+1 ==  6 || b_targ+1 ==  7 || b_targ+1 ==  8
                    || b_targ+1 == 66 || b_targ+1 == 67 || b_targ+1 == 94 || b_targ+1 == 95)
                    ++b_targ;
                Ultima.AnimationEdit.LoadFromVD(f_targ, ++b_targ, new BinaryReader(b_strm[b]));
            }
            for (int b_targ = 199, b = 200; b < 399; ++b) {
                if (b >= b_strm.Length || b_strm[b] == null) continue;
                b_strm[b].Position = 0;
                Ultima.AnimationEdit.LoadFromVD(f_targ, ++b_targ, new BinaryReader(b_strm[b]));
            }
            Ultima.AnimationEdit.Save(f_targ, FiddlerControls.Options.OutputPath);
            MessageBox.Show(String.Format("AnimationFiles saved to {0}", FiddlerControls.Options.OutputPath),
                            "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            //return;
            */
            #endregion
            #region Генерация Bodyconv.def 
            /*
            var str = String.Empty;
            for (int id = 1; id <= 199; ++id)
                str += String.Format("{0}\t\t{1}\t-1\t-1\t-1{2}", id, id, Environment.NewLine);
            str += Environment.NewLine;
            for (int id = 200; id <= 399; ++id)
                str += String.Format("{0}\t\t-1\t-1\t-1\t{1}{2}", id, id-200, Environment.NewLine);
            str += Environment.NewLine;
            for (int id = 400; id <= 499; ++id)
                str += String.Format("{0}\t\t-1\t{1}\t-1\t-1{2}", id, id-100, Environment.NewLine);
            str += Environment.NewLine;
            for (int id = 600; id <= 999; ++id)
                str += String.Format("{3}{0}\t\t-1\t-1\t{1}\t-1{2}", id, id-600, Environment.NewLine, (id == 605 || id == 606 || id == 607 || id == 608 || id == 666 || id == 667 || id == 694 || id == 695) ? "#" : "");
            str += Environment.NewLine;
            for (int id = 1000; id <= 1541; ++id)
                str += String.Format("{0}\t-1\t{1}\t-1\t-1{2}", id, id-600, Environment.NewLine);
            File.WriteAllText(Path.Combine(FiddlerControls.Options.OutputPath, "bodyconv.def"), str);

            str = String.Empty;
            for (int id = 1; id <= 33; ++id)
                str += String.Format("{0}\t\tMONSTER\t{1}\t\t#{2}", id, 0, Environment.NewLine);
            for (int id = 34; id <= 99; ++id)
                str += String.Format("#{0}\t\tMONSTER\t{1}\t\t#{2}", id, 0, Environment.NewLine);
            for (int id = 100; id <= 599; ++id)
                str += String.Format("#{0}\tMONSTER\t{1}\t\t#{2}", id, 0, Environment.NewLine);
            for (int id = 600; id <= 791; ++id)
                str += String.Format("{3}{0}\t\tMONSTER\t{1}\t\t#{2}", id, 0, Environment.NewLine, (id == 605 || id == 606 || id == 607 || id == 608 || id == 666 || id == 667 || id == 694 || id == 695) ? "#" : "");
            for (int id = 792; id <= 799; ++id)
                str += String.Format("#{0}\t\tMONSTER\t{1}\t\t#{2}", id, 0, Environment.NewLine);
            for (int id = 800; id <= 856; ++id)
                str += String.Format("{0}\t\tANIMAL\t\t{1}\t\t#{2}", id, 0, Environment.NewLine);
            for (int id = 857; id <= 999; ++id)
                str += String.Format("#{0}\tANIMAL\t\t{1}\t\t#{2}", id, 0, Environment.NewLine);
            for (int id = 1000; id <= 2048; ++id)
                str += String.Format("#{0}\tEQUIPMENT\t{1}\t\t#{2}", id, 0, Environment.NewLine);
            File.WriteAllText(Path.Combine(FiddlerControls.Options.OutputPath, "mobtypes.txt"), str);
            return;
            */
            #endregion



            var newbody = new int[1050,2];
            int h = 1, l = 399, p = 400;
            int[] body_exc = new int[] {400,401, 605,606,607,608, 666,667,694,695,  970};
            for (int f = 1; f <= 5; ++f)
                for (int b = 0; b < body[f][1].Length; ++b) {
                    newbody[h, 0] = f;
                    newbody[h, 1] = body[f][1][b];
                    ++h;
                } 
            for (int f = 5; f >= 1; --f)
                for (int b = body[f][0].Length-1; b >= 0; --b) {
                    newbody[l, 0] = f;
                    newbody[l, 1] = body[f][0][b];
                    --l;
                }


            #region Переупаковка
            /*
            int[,] body_eqip = new int[,] {
                // Тела, госты и наряды
                {1,400},{1,401},{1,970},{0,-1}, {5,522},{5,523},{0,-1},{0,-1}, {0,-1},{0,-1},{0,-1},{0,-1}, {0,-1},{0,-1},{0,-1},{0,-1},  {1,994},{1,990},{1,991},{1,987},
                // Части тел
                {0,-1},  
                {1,800}, {1,801}, {1,802}, {1,803}, {1,904}, {1,905}, {1,906}, // Борода
                {1,700}, {1,701}, {1,702}, {1,703}, {1,900}, {1,901}, {1,902}, {1,903}, {1,710}, {1,712},                            // Волосы
                {5,646}, {5,647}, {5,648}, {5,649}, {5,650}, {5,651}, {5,652}, {5,636}, {5,637}, {5,638}, {5,639}, // Эльфийские Волосы

                // Лошади
                {1,820}, {1,824}, {1,846}, {1,848},     {1,828}, {1,827}, {1,826}, {1,825},

                // Шапки
                {0,-1},  {1,404}, {1,405}, {1,406}, {1,407}, {1,408}, {1,409}, {1,410}, {1,411}, {1,412}, {1,413}, {1,414}, {1,415}, {1,416}, {1,417}, {1,418}, {1,419},
                {1,973}, {5,615}, {5,616}, {5,617}, {4,672}, {4,673}, 
                {4,414}, {4,415}, {4,416}, {4,417}, {4,418}, {4,419}, {4,420}, {4,421}, {4,427}, {4,428}, {4,439}, {4,441}, {4,477}, {4,488},  
                {4,432}, {4,433}, // воротники

                // Одежда
                {1,430}, {1,431}, {1,434}, {1,435}, {1,449}, {1,447}, {1,448}, {1,455}, {1,466}, {1,465}, 
                {1,468}, {1,469},  
                {1,476}, {1,477}, {1,479}, {1,480}, {1,490}, {1,492}, {1,912}, {1,913}, 
                {1,909}, {1,910}, {1,911}, {1,971}, {1,986}, {1,988},
                {4,660}, 
                {4,426}, {4,437}, {4,438}, {4,440}, {4,442}, {4,443}, {4,456}, {4,457}, {4,458}, {4,459}, {4,460}, {4,461}, {4,462}, {4,463}, {4,464}, {4,465}, {4,474}, 
                {4,478}, {4,480}, {4,481}, {4,482}, {4,483}, {4,484}, {4,485}, {4,486}, {4,487}, {4,489}, {4,491}, 

                {5,624}, {5,625}, {5,626}, {5,627}, {5,628}, {5,629}, {5,632}, {5,633},  

                {5,493}, {1,926}, {1,927},  

                // Колчаны
                {5,630}, {4,669},

                // Шлемы
                {1,560}, {1,561}, {1,562}, {1,563}, {1,564}, {1,565}, {1,566},

                {1,921}, {1,922}, {1,923}, {1,924}, {1,925},  // бабская бронь

                // Броня
                //торс    ноги    наручи    перчи    ворот   шлемы
                {1,542}, {1,543}, {1,544}, {1,545}, {1,546},            // Легкая кожанная
                {1,521},          {1,523},          {1,525},            // Клепаная кожанная
                {1,538}, {1,540},                                       // Хауберг 
                {1,548}, {1,549}, {1,550},                              // Кольчуга
                {1,527}, {1,529}, {1,528}, {1,530},          {1,928}, {1,929},           // Латы
                {1,554}, {1,555}, {1,556}, {1,557},                     // Кости

                {4,655}, {4,658}, {4,656}, {4,657}, {4,654}, {4,653},   // Латы добродетели
                         {4,659},
                {4,671},                                                // Латы добродетели (жен)
                {4,675}, {4,678}, {4,674}, {4,676},          {4,677},   // Драконья чешуя (муж)
                {4,680}, {4,683}, {4,679}, {4,681},          {4,682},   // Драконья чешуя (жен)

                {4,411}, {4,412},          {4,413},          {4,410},  // Наряд нинзи

                {5,640}, {5,643}, {5,642}, {5,641},                     // Легкая кожанная (муж)
                {5,645}, {5,644},                                       // Легкая кожанная (жен)
                {5,618}, {5,622}, {5,621}, {5,619}, {5,620},            // Клепаная кожанная (муж)
                {5,623},                                                // Клепаная кожанная (жен)
                {5,492}, {5,496}, {5,497}, {5,495}, {5,494},            // Деревяная бронь (муж)
                {5,498},                                                // Деревяная бронь (жен)
                
                {4,422}, {4,516}, {4,434}, // Желто-коричневая самурайская доспеха
                {4,471}, {4,520},
                {4,423}, {4,518}, {4,435}, // Бюрюзово-синяя самурайская доспеха
                {4,472}, 
                {4,424}, {4,519}, {4,436}, // Красноватая самурайская доспеха
                {4,473}, 

                // Щиты
                {1,579}, {1,578}, {1,577}, {1,576}, {1,993},                                            // Круглые
                {1,580}, {1,581}, {1,992}, {4,670}, {0,-1},  {0,-1},  {0,-1},  {0,-1}, {0,-1},          // Вытянутые
                {1,582}, {0,-1},                                                                        // Прямоугольные

                {1,649}, {3,938}, {4,466}, {5,600}, {5,601}, // луки
                {1,651}, {1,616}, {3,939}, // арбалтеы

                {1,611}, {1,612}, {1,613}, {1,614}, {1,615}, {1,624}, {1,634}, {1,644}, {1,653}, {5,610},// топоры
                {1,635}, {1,620}, {1,625}, {1,626}, {1,630}, {1,631}, {1,633}, {1,640}, {1,642}, {1,646}, {4,467}, {5,606},// булавы
                {4,453}, {1,980}, {1,981}, {1,982}, {1,983}, {3,932},// жезлы
                {1,617}, {1,621}, {1,628}, {1,636}, {1,639}, {1,641}, {1,645}, {1,648}, {1,972}, {3,936}, {3,933}, {3,934}, {5,607}, {4,447}, // посохи
                {4,446}, {3,935}, {3,937},    {3,930}, {3,931}, // серы косы
                {4,444}, {5,609}, {1,618}, {1,619}, {1,622}, {1,623}, {1,627}, {1,629}, {1,637}, {1,638}, {1,643}, {3,940}, {5,611}, {5,605}, {5,604}, {5,603}, {5,602}, {4,430}, {4,429}, // мечи

                {4,431}, {4,452}, {5,608}, {4,450}, {4,448}, {4,445},  // двойное

                {5,635}, {5,631}, {4,451}, {4,449}, {4,454}, // хз 

                // Книги
                {1,985}, {1,984}, {3,941}, {4,661}, {4,662}, {4,663}, {4,664}, {4,665}, {4,666}, {4,667}, {4,668}, {5,-1}, 

                // светильники
                {1,500}, {1,501}, {1,502}, {1,503}, {1,504}, {1,505},

                // {1,573}, {1,574},
            };
            int id_targ = 400, f_targ = 3;
            for (int i = 0; i < id_targ; ++i) {
                if (Ultima.Animations.IsAnimDefinied(i, f_targ))
                    for (int a = 0; a < Animations.GetAnimLength(i, f_targ); ++a)
                        for (int d = 0; d < 5; ++d) {
                            var edit = Ultima.AnimationEdit.GetAnimation(f_targ, i, a, d);
                            if (edit != null) edit.ClearFrames();
                        }
            }
            for (int i = 0; i < body_eqip.GetLength(0); ++i) {
                if (body_eqip[i,0] != 0 && body_eqip[i,1] >= 0) {
                    var stream = new MemoryStream();
                    Ultima.AnimationEdit.ExportToVD(body_eqip[i,0], body_eqip[i,1], new BinaryWriter(stream));
                    stream.Position = 0;
                    if (Ultima.Animations.IsAnimDefinied(id_targ, f_targ))
                        for (int a = 0; a < Animations.GetAnimLength(id_targ, f_targ); ++a)
                            for (int d = 0; d < 5; ++d) {
                                var edit = Ultima.AnimationEdit.GetAnimation(f_targ, id_targ, a, d);
                                if (edit != null) edit.ClearFrames();
                            }
                    Ultima.AnimationEdit.LoadFromVD(f_targ, id_targ, new BinaryReader(stream));
                } else if (Ultima.Animations.IsAnimDefinied(id_targ, f_targ))
                    for (int a = 0; a < Animations.GetAnimLength(id_targ, f_targ); ++a)
                        for (int d = 0; d < 5; ++d) {
                            var edit = Ultima.AnimationEdit.GetAnimation(f_targ, id_targ, a, d);
                            if (edit != null) edit.ClearFrames();
                        }
                ++id_targ;
            }
            for (int i = id_targ; i < Ultima.Animations.GetAnimCount(f_targ); ++i) {
                if (Ultima.Animations.IsAnimDefinied(i, f_targ))
                    for (int a = 0; a < Animations.GetAnimLength(i, f_targ); ++a)
                        for (int d = 0; d < 5; ++d) {
                            var edit = Ultima.AnimationEdit.GetAnimation(i, id_targ, a, d);
                            if (edit != null) edit.ClearFrames();
                        }
            }
            Ultima.AnimationEdit.Save(f_targ, FiddlerControls.Options.OutputPath);
            MessageBox.Show(String.Format("AnimationFiles saved to {0}", FiddlerControls.Options.OutputPath),
                            "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            return;
            */
            #endregion

            #region Пропущеная книга
            /*
            int id_targ = 764, f_targ = 3;
            int f_id = 303, f_a = 1;
            
            for (int t_a = 0; t_a < 35; ++t_a) {
                for (int d = 0; d < 5; ++d) {
                    var edit1 = Ultima.AnimationEdit.GetAnimation(5, f_id, f_a, d);
                    var edit2 = Ultima.AnimationEdit.GetAnimation(f_targ, id_targ, t_a, d);
                    

                    edit2.ReplacePalette(edit1.Palette);
                    //edit2.Palette = edit1.Palette;
                    for (int f = 0; f < edit1.Frames.Count; ++f) {
                        edit2.AddFrame(new Bitmap(2, 2));
                        edit2.Frames[f] = edit1.Frames[f];
                    }
                }
                if (++f_a > 12) {
                    ++f_id;
                    f_a = 0;
                }
            }
            Ultima.AnimationEdit.Save(f_targ, FiddlerControls.Options.OutputPath);
            MessageBox.Show(String.Format("AnimationFiles saved to {0}", FiddlerControls.Options.OutputPath),
                            "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            */
            #endregion

            /////////////////////////////////////
            var text = File.ReadAllLines(Ultima.Files.GetFilePath("Bodyconv.def"));
            foreach (var line in text) {
                if (String.IsNullOrEmpty(line)) continue;
                var tocken = (line.IndexOf('#')<0?line:line.Remove(line.IndexOf('#'))).TrimStart(new[] {' ', '\t'}).Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries);
                if (tocken.Length < 5 || tocken[0][0] == '#') continue;
                var id = new int[6]; int f = -1;
                for (var i = 0; i < 5; ++i) {
                    if (!Int32.TryParse(tocken[i], out id[i+1]))
                        throw new Exception("Syntax error in Bodyconv.def");
                    if (id[i+1] >= 0)
                        f = i+1;
                }

                try {
                if (id[f] < Animations.GetAnimCount(f) && Animations.GetAnimLength(id[f], f) == Animations.GetAnimLength(id[1], 1)) {
                    
                    //id[1] < Animations.GetAnimCount(1) && 
                    bool valid1 = true, validf = false;
                    for (int j = 0; j < Animations.GetAnimLength(id[1], 1); ++j) {
                        if (Ultima.AnimationEdit.IsActionDefinied(1, id[1], j))
                            valid1 = false;
                        if (Ultima.AnimationEdit.IsActionDefinied(f, id[f], j))
                            validf = true;
                    } if (!valid1 || !validf) continue;

                    var stream = new MemoryStream();
                    Ultima.AnimationEdit.ExportToVD(f, id[f], new BinaryWriter(stream));
                    stream.Position = 0;
                    Ultima.AnimationEdit.LoadFromVD(1, id[1], new BinaryReader(stream));

                    for (int a = 0; a < Animations.GetAnimLength(id[f], f); ++a) {
                        for (int d = 0; d < 5; ++d) {
                            var edit = Ultima.AnimationEdit.GetAnimation(f, id[f], a, d);
                            if (edit != null)
                                edit.ClearFrames();
                        }
                    }

                }
                } catch (Exception ex) {
                    throw new Exception(String.Format("{0:d} - {1:d} - {2:d}", f, id[f], 0));
                }
            }

            Options.ChangedUltimaClass["Animations"] = true;
            AfterSelectTreeView(this, null);

            for (int f = 1; f < 6; ++f)
                Ultima.AnimationEdit.Save(f, FiddlerControls.Options.OutputPath);
            MessageBox.Show(String.Format("AnimationFiles saved to {0}", FiddlerControls.Options.OutputPath),
                            "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            Options.ChangedUltimaClass["Animations"] = false;
        }

        

       

        

        

        

        
    }
}
