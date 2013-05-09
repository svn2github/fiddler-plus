using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MassImport
{
    public partial class GumpCombine : Form
    {
        public GumpCombine()
        {
            InitializeComponent();
        }

        private void lvFiles_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            Bitmap bmp = e.Item.Tag as Bitmap;
            if (bmp != null) {
                if (keydown > 0)
                    e.Graphics.FillRectangle(e.ItemIndex == selected ? Brushes.LightBlue : Brushes.LightGray, e.Bounds);
                else e.Graphics.FillRectangle(lvFiles.SelectedItems.Contains(e.Item) ? Brushes.LightBlue : Brushes.LightGray, e.Bounds);
                //if (bmp.Width < 60 && bmp.Height < 60)
                //    e.Graphics.DrawImageUnscaled(bmp, new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width - 4, e.Bounds.Height - 4));
                //else
                    e.Graphics.DrawImage(bmp, new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width - 4, e.Bounds.Height - 4));
            }
        }

        private void InsertMask(int index, Bitmap bitmap)
        {
            if (index < 0)
                index = lvFiles.Items.Count;

            lvFiles.BeginUpdate();
            var item = new ListViewItem(index.ToString(), 0);
            item.Tag = BColFilter(bitmap, false); // bitmap;
            lvFiles.Items.Insert(index, item);
            lvFiles.EndUpdate();
        }

        private void RemoveMask(int index)
        {
            if (index < 0 || index >= lvFiles.Items.Count)
                return;

            lvFiles.BeginUpdate();
            (lvFiles.Items[index].Tag as Bitmap).Dispose();
            lvFiles.Items.RemoveAt(index);
            lvFiles.EndUpdate();
        }

        private void tbFolder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                for (int i = lvFiles.Items.Count - 1; i >= 0; --i)
                    RemoveMask(i);

                foreach (var file in Directory.GetFiles(tbFolder.Text)) {
                    var ext = Path.GetExtension(file).ToLower();
                    if (ext != ".bmp" && ext != ".png") continue;
                    Bitmap bmp = (Bitmap) Bitmap.FromFile(file);
                    if (bmp.Width > 60 || bmp.Height > 60)
                        InsertMask(lvFiles.Items.Count, bmp);
                }
                btnDraw_Click(null, EventArgs.Empty);
            }           
        }

        private int keydown = 0;
        private int selected = 0;
        private int lasttick = Environment.TickCount;
        private void lvFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (keydown < 0) keydown = 0;
            if (Environment.TickCount - lasttick > 500) {
                ++keydown;
                lasttick = Environment.TickCount;
            }
            
            if (e.Modifiers == Keys.Control &&  e.KeyCode == Keys.A) {
                foreach (var item in lvFiles.Items) {
                    (item as ListViewItem).Selected = true;
                }
            } else if (lvFiles.SelectedItems.Count == 1) {
                int index = lvFiles.SelectedItems[0].Index;
                if (index != 0 && (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up)) {
                    selected = index-1;
                    lvFiles.BeginUpdate();
                    lvFiles.Items[index].Selected = false;
                    Bitmap bmp = lvFiles.Items[index-1].Tag as Bitmap;
                    lvFiles.Items[index-1].Tag = lvFiles.Items[index].Tag as Bitmap;
                    lvFiles.Items[index].Tag = bmp;
                    lvFiles.Items[index-1].Selected = true;
                    lvFiles.EndUpdate();
                    lvFiles.Invalidate();
                    lvFiles.Update();
                    btnDraw_Click(null, EventArgs.Empty); 
                    lvFiles.Items[index-1].Selected = true;
                } else 
                if ((index != (lvFiles.Items.Count-1)) && (e.KeyCode == Keys.Right || e.KeyCode == Keys.Down)) {
                    selected = index+1;
                    lvFiles.BeginUpdate();
                    lvFiles.Items[index].Selected = false;
                    Bitmap bmp = lvFiles.Items[index+1].Tag as Bitmap;
                    lvFiles.Items[index+1].Tag = lvFiles.Items[index].Tag as Bitmap;
                    lvFiles.Items[index].Tag = bmp;
                    lvFiles.Items[index+1].Selected = true;
                    lvFiles.EndUpdate();
                    lvFiles.Invalidate();
                    lvFiles.Update();
                    btnDraw_Click(null, EventArgs.Empty);
                    lvFiles.Items[index+1].Selected = true;
                } else
                if (e.KeyCode == Keys.Delete) {
                    selected = index;
                    RemoveMask(index);
                    btnDraw_Click(null, EventArgs.Empty);
                }
                //keydown = false;
            }
        }

        private void lvFiles_KeyUp(object sender, KeyEventArgs e)
        {
            if (keydown < 0) keydown = 0;
            --keydown;
            if (lvFiles.SelectedItems.Count > 0) {
                lvFiles.SelectedItems[0].Selected = false;
                lvFiles.Items[selected].Selected = true;
            }
            
            //keydown = false;
        }

        private bool indexchanged = false;
        private void lvFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (!keydown) {
            //    selected = lvFiles.SelectedItems[0].Index;
           // }
            /*
            if (keydown) {
                keydown = false;
                lvFiles.SelectedItems[0].Selected = false;
                lvFiles.Items[selected].Selected = true;
                //keydown = false;
            } else if (!indexchanged) {
                indexchanged = true;
                lvFiles.SelectedItems[0].Selected = false;
                lvFiles.Items[selected].Selected = true;
                indexchanged = false;
            }*/
        }

        private void lvFiles_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (keydown != 0 && e.ItemIndex != selected && !e.Item.Selected) {
                e.Item.Selected = false;
                lvFiles.Items[selected].Selected = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            pbPaperdrol.Image.Save(Path.Combine(tbFolder.Text, "__view__.png"));
        }

        public static unsafe Bitmap BColFilter(Bitmap bmp, bool forceCCol2BCol)
        {
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
            uint* line = (uint*)bd.Scan0;
            int delta = bd.Stride >> 2;

            Bitmap bmpnew = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);
            BitmapData bdnew = bmpnew.LockBits(new Rectangle(0, 0, bmpnew.Width, bmpnew.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            uint* linenew = (uint*)bdnew.Scan0;
            int deltanew = bdnew.Stride >> 2;

            for (int Y = 0; Y < bmp.Height; ++Y, line += delta, linenew += deltanew)
            {
                uint* cur = line;
                uint* curnew = linenew;
                for (int X = 0; X < bmp.Width; ++X)
                {
                    byte a = (byte)((cur[X] & 0xFF000000) >> 24);
                    byte r = (byte)((cur[X] & 0x00FF0000) >> 16);
                    byte g = (byte)((cur[X] & 0x0000FF00) >> 8);
                    byte b = (byte)((cur[X] & 0x000000FF) >> 0);

                    if (r < 8 && g < 8 && b < 8)
                    {
                        byte max = Math.Max(r, Math.Max(g, b));
                        if (!forceCCol2BCol)
                        {
                            if (r != 0 && r == max)
                                curnew[X] |= 0xFF080000;
                            if (g != 0 && g == max)
                                curnew[X] |= 0xFF000800;
                            if (b != 0 && b == max)
                                curnew[X] |= 0xFF000008;
                        }
                        else
                        {
                            if (r == max)
                                curnew[X] |= 0xFF080000;
                            if (g == max)
                                curnew[X] |= 0xFF000800;
                            if (b == max)
                                curnew[X] |= 0xFF000008;
                        }
                        if ((curnew[X] & 0x00FFFFFF) == 0)
                            curnew[X] = 0;// 0xFF000000;
                    }
                    else
                        curnew[X] = cur[X];
                }
            }
            bmp.UnlockBits(bd);
            bmpnew.UnlockBits(bdnew);
            return bmpnew;
        }

        private void btnDraw_Click(object sender, EventArgs e)
        {
            if (pbPaperdrol.Image == null){
                pbPaperdrol.Image = new Bitmap(260, 247, PixelFormat.Format32bppArgb);
            }
            using (Graphics graphpic = Graphics.FromImage(pbPaperdrol.Image)) {
                graphpic.Clear(Color.Black);
                if (sender == null)
                    foreach (var item in lvFiles.Items) {
                        Bitmap bmp = (Bitmap)(item as ListViewItem).Tag;
                        graphpic.DrawImage(bmp, 0, 0);
                    }
                else
                    foreach (var item in lvFiles.SelectedItems) {
                        Bitmap bmp = (Bitmap)(item as ListViewItem).Tag;
                        graphpic.DrawImage(bmp, 0, 0);
                    }
                
            }
            pbPaperdrol.Invalidate();
            
        }

        private void lvFiles_MouseClick(object sender, MouseEventArgs e)
        {
            selected = -1;
            keydown = -100;
        }

       

       

        


        

      
    }
}
