using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace FiddlerControls
{
    public partial class TextureCrossGen : UserControl
    {
        public TextureCrossGen(Control parent) : this()
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
            mMinimumSize = parent.MinimumSize;
            parent.MinimumSize = new Size(MinimumSize.Width + parent.Size.Width - parent.ClientSize.Width, MinimumSize.Height + parent.Size.Height - parent.ClientSize.Height);
            Cursor.Current = Cursors.Default;     
        }

        private Size mMinimumSize;
        private List<Control> mParentControls;

        public void Close()
        {
            Cursor.Current = Cursors.WaitCursor;
            //if (backgroundWorker.IsBusy)
            //    backgroundWorker.CancelAsync();
            //Ultima.Secondary.Art.Dispose();
            //Ultima.Secondary.TileData.Dispose();
            //Ultima.Secondary.RadarCol.Dispose();

            //this.Visible = false;
            this.DestroyHandle();
            foreach (Control control in mParentControls)
                control.Visible = true;

            this.Parent.MinimumSize = mMinimumSize;
            Cursor.Current = Cursors.Default;
        }

        ~TextureCrossGen()
        {
            Close();
        }

        public TextureCrossGen()
        {
            InitializeComponent();
            this.buttonGenerate.Tag = false;
            this.pictureBoxTexture1.AllowDrop = true;
            this.pictureBoxTexture2.AllowDrop = true;
            this.listViewMask.AllowDrop = true;
            this.comboBoxOptions.SelectedIndex = 4;
            folderBrowserDialog.SelectedPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Generated\LandTextures");
            if (!Directory.Exists(folderBrowserDialog.SelectedPath))
                Directory.CreateDirectory(folderBrowserDialog.SelectedPath);
            //folderBrowserDialog.SelectedPath = @"C:\UltimaOnline\source\_build\uoFiddler\Extracted\Textures\out";
            //OnAction_DragDrop(pictureBoxTexture1, new DragEventArgs(new DataObject(DataFormats.FileDrop, new string[]{@"C:\UltimaOnline\source\_build\uoFiddler\Extracted\Textures\T0x2E0E.png"}), 0, 0, 0, DragDropEffects.Copy, DragDropEffects.Copy));
            //OnAction_DragDrop(pictureBoxTexture2, new DragEventArgs(new DataObject(DataFormats.FileDrop, new string[]{@"C:\UltimaOnline\source\_build\uoFiddler\Extracted\Textures\T0x012B.png"}), 0, 0, 0, DragDropEffects.Copy, DragDropEffects.Copy));
            //OnAction_DragDrop(listViewMask,       new DragEventArgs(new DataObject(DataFormats.FileDrop, new string[]{@"C:\UltimaOnline\source\_build\uoFiddler\Extracted\Textures\T0x3760.bmp"}), 0, 0, 0, DragDropEffects.Copy, DragDropEffects.Copy));
            maskedTextBoxID.Text = String.Format("0x{0:X4}", 0x0123);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            var selected = new List<int>(64);
            foreach (ListViewItem item in listViewMask.SelectedItems)
                selected.Add(listViewMask.Items.IndexOf(item));
            selected.Sort(); selected.Reverse();
            foreach (var index in selected)
                RemoveMask(index);

            maskedTextBoxID_TextChanged(sender, EventArgs.Empty);
        }

        private void InsertMask(int index, Bitmap bitmap) 
        {
            if (index < 0) 
                index = listViewMask.Items.Count;

            listViewMask.BeginUpdate();
            var item = new ListViewItem(index.ToString(), 0);
            item.Tag = bitmap;
            listViewMask.Items.Insert(index, item);
            listViewMask.EndUpdate();
        }

        private void RemoveMask(int index) 
        {
            if (index < 0 || index >= listViewMask.Items.Count) 
                return;

            listViewMask.BeginUpdate();
            (listViewMask.Items[index].Tag as Bitmap).Dispose();
            listViewMask.Items.RemoveAt(index);
            listViewMask.EndUpdate();
        }

        private void DrawMaskItem(object sender, DrawListViewItemEventArgs e)
        {
            Bitmap bmp = e.Item.Tag as Bitmap;          
            if (bmp != null) {
                e.Graphics.FillRectangle(listViewMask.SelectedItems.Contains(e.Item) ? Brushes.LightBlue : Brushes.LightGray, e.Bounds);
                e.Graphics.DrawImage(bmp, new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width - 4, e.Bounds.Height - 4));
            }
        }

        private void OnAction_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.Copy;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }  

        private void OnAction_DragDrop(object sender, DragEventArgs e)
        {
            int i = 0;
            var files = new List<string>((string[])e.Data.GetData(DataFormats.FileDrop, false));
            while (i < files.Count) {
                if (String.IsNullOrEmpty(System.IO.Path.GetExtension(files[i])))
                    files.AddRange(System.IO.Directory.GetFiles(files[i], "*.*", System.IO.SearchOption.AllDirectories));
                ++i;
            }
            for (i = files.Count-1; i >= 0; --i) {
                var fext = System.IO.Path.GetExtension(files[i]);
                if ((String.Compare(fext, ".bmp", true) != 0) && (String.Compare(fext, ".png", true) != 0))
                    files.RemoveAt(i);
            }

            if (sender is PictureBox) {
                if (files.Count != 1)
                    return;
                var texture = new Bitmap(files[0]);
                if ((texture.Width != texture.Height) || (texture.Width != 64 && texture.Width != 128 && texture.Width != 256)) 
                    return;
                (sender as PictureBox).BackgroundImage = texture;
                (sender as PictureBox).Update();
            } else if (sender is ListView) {
                Cursor.Current = Cursors.WaitCursor;
                foreach (var file in files) {
                    var mask = new Bitmap(file);
                    if ((mask.Width != mask.Height) || (mask.Width != 64 && mask.Width != 128 && mask.Width != 256)) 
                        return;
                    InsertMask(-1, mask);
                }
                (sender as ListView).Update();
                Cursor.Current = Cursors.Default;
            }

            maskedTextBoxID_TextChanged(sender, EventArgs.Empty);
        }

        private void maskedTextBoxID_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')) {
                e.Handled = true;
            } else {
                e.KeyChar = '\t';
            }
        }

        private void maskedTextBoxID_TextChanged(object sender, EventArgs e)
        {
            int value;
            if ((Boolean)buttonGenerate.Tag)
                return;
            if (listViewMask.Items.Count > 0 && pictureBoxTexture1.BackgroundImage != null && pictureBoxTexture2.BackgroundImage != null)
                buttonGenerate.Enabled = Int32.TryParse(maskedTextBoxID.Text.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out value);
            else buttonGenerate.Enabled = false;
        }

        [Obsolete("Не коректное преобразование")]
        private Bitmap RotateBitmap(Bitmap b, float angle)
        {
            //create a new empty bitmap to hold rotated image
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(returnBitmap);
            //move rotation point to center of image
            g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
            //rotate
            g.RotateTransform(angle);
            //move image back
            g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
            //draw passed in image onto graphics object
            g.DrawImage(b, new Point(0, 0));
            return returnBitmap;
        }

        public static Bitmap RotateBitmap(Bitmap bmp, float angle, Color bkColor)
        {
            int w = bmp.Width;
            int h = bmp.Height;
            PixelFormat pf = default(PixelFormat);
            if (bkColor == Color.Transparent) {
                pf = PixelFormat.Format32bppArgb;
            } else {
                pf = bmp.PixelFormat;
            }

            Bitmap tempImg = new Bitmap(w, h, pf);
            Graphics g = Graphics.FromImage(tempImg);
            g.Clear(bkColor);
            g.DrawImageUnscaled(bmp, 1, 1); // 1, 1
            g.Dispose();

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            //Using System.Drawing.Drawing2D.Matrix class 
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);
            Bitmap newImg = new Bitmap(Convert.ToInt32(rct.Width-1), Convert.ToInt32(rct.Height-1), pf); // Convert.ToInt32(rct.Width), Convert.ToInt32(rct.Height), pf);
            g = Graphics.FromImage(newImg);
            g.Clear(bkColor);
            g.TranslateTransform(-rct.X-1, -rct.Y-1); //(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.CompositingQuality = CompositingQuality.AssumeLinear;
            g.InterpolationMode  = InterpolationMode.NearestNeighbor;
            g.SmoothingMode      = SmoothingMode.None;
            g.DrawImageUnscaled(tempImg, 0, 0);
            g.Dispose();
            tempImg.Dispose();
            return newImg;
        }

        private Bitmap ResizeBitmap(Bitmap b, int nWidth, int nHeight, bool smoothing)
        {
            return ResizeBitmap(b, new Rectangle(0, 0, b.Width, b.Height), nWidth, nHeight, smoothing);
        }

        private Bitmap ResizeBitmap(Bitmap b, Rectangle rect, int nWidth, int nHeight, bool smoothing)
        {
            Bitmap result = new Bitmap(nWidth, nHeight);
            using (Graphics g = Graphics.FromImage((Image)result)) {
                //set the resize quality modes to high quality
                g.CompositingQuality = smoothing? CompositingQuality.HighQuality       : CompositingQuality.AssumeLinear;
                g.InterpolationMode  = smoothing? InterpolationMode.HighQualityBilinear: InterpolationMode.NearestNeighbor;
                g.SmoothingMode = smoothing ? SmoothingMode.HighQuality                : SmoothingMode.None;
                //draw the image into the target bitmap 
                g.DrawImage(b, new Rectangle(0, 0, nWidth, nHeight), rect, GraphicsUnit.Pixel);
            }
            return result;
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            int index = Int32.Parse(maskedTextBoxID.Text.Substring(2), System.Globalization.NumberStyles.HexNumber);
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                return;
            string path = folderBrowserDialog.SelectedPath;

            buttonGenerate.Tag = true;
            buttonGenerate.Enabled = false;
            var image1 = pictureBoxTexture1.BackgroundImage as Bitmap;
            var image2 = pictureBoxTexture2.BackgroundImage as Bitmap;
            foreach (ListViewItem item in listViewMask.Items) {
                var mask_b = item.Tag as Bitmap;

                int resl;
                switch (comboBoxOptions.SelectedIndex) {
                    default:
                    case 0: resl = Math.Min(image1.Width, image2.Width); break;
                    case 1: resl = Math.Max(image1.Width, image2.Width); break;
                    case 2: resl = image1.Width; break;
                    case 3: resl = image2.Width; break;
                    case 4: resl =  64; break;
                    case 5: resl = 128; break;
                    case 6: resl = 256; break;
                }

                var dest_b = new Bitmap(resl, resl, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                var srsA_b = image1.Width == resl ? image1 : ResizeBitmap(image1, resl, resl, true);
                var srsB_b = image2.Width == resl ? image2 : ResizeBitmap(image2, resl, resl, true); 
                    mask_b = mask_b.Width == resl ? mask_b : ResizeBitmap(mask_b, resl, resl, true);
                
                for (int x = 0; x < resl; ++x)
                    for (int y = 0; y < resl; ++y) {
                        Color srsA_c = srsA_b.GetPixel(x, y);
                        Color srsB_c = srsB_b.GetPixel(x, y);
                        Color mask_c = mask_b.GetPixel(x, y);
                        float mask_a = 1f - (float)Math.Max(Math.Max(mask_c.R, mask_c.G), mask_c.B) / 255f;
                        Color dest_c = Color.FromArgb(
                            (int)Math.Min(srsB_c.R + mask_a * (srsA_c.R - srsB_c.R), 255f),
                            (int)Math.Min(srsB_c.G + mask_a * (srsA_c.G - srsB_c.G), 255f),
                            (int)Math.Min(srsB_c.B + mask_a * (srsA_c.B - srsB_c.B), 255f)
                        );
                        dest_b.SetPixel(x, y, dest_c);
                    }
               
                var dest_l = (resl == 64) ? dest_b : ResizeBitmap(dest_b, 64, 64, false);
                var dest_r = RotateBitmap(dest_l, 45f, Color.Black);
                if (resl != 64) dest_l.Dispose();
                    dest_l = ResizeBitmap(dest_r, new Rectangle(1, 1, dest_r.Width-2, dest_r.Height-2), 44, 44, false);             

                dest_b.Save(System.IO.Path.Combine(path, String.Format("T0x{0:X4}.bmp", index)), System.Drawing.Imaging.ImageFormat.Bmp);
                dest_l.Save(System.IO.Path.Combine(path, String.Format("L0x{0:X4}.bmp", index)), System.Drawing.Imaging.ImageFormat.Bmp);
                dest_b.Dispose();
                dest_r.Dispose();
                dest_l.Dispose();
                maskedTextBoxID.Text = String.Format("0x{0:X4}", ++index);
            }
            buttonGenerate.Tag = false;
            buttonGenerate.Enabled = true;
        }    
    }
}
