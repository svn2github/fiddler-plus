using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ButtonBarsControl.Control;
using ButtonBarsControl.Design.Enums;
using ButtonBarsControl.Design.Generics;
using XPPanelControl;

namespace GuiControls
{
    public delegate void ItemClickEventHandler(object sender, ItemClickEventArgs e);
    public class ItemClickEventArgs
    {
        public ControlPanelItem Item { get; private set; } // readonly
        public MouseButtons Button { get; private set; } // readonly 
        public String Text { get; private set; } // readonly

        public ItemClickEventArgs(ControlPanelItem item, MouseButtons button, string s)
        {
            this.Item = item;
            this.Button = button;
            this.Text = s;
        }
    }

    public class ControlPanelItem
    {
        public BarItem BarItem { get { return m_BarItem; } }
        private BarItem m_BarItem;

        public ControlPanelItem(ControlPanelGroup group, int icon, string caption, string toolTip)
        {
            m_BarItem = CreateBarItem(group.ButtonBar, caption, icon, caption, toolTip);
        }

        public ControlPanelItem(ControlPanelGroup group, string tag, int icon, string caption, string toolTip)
        {
            m_BarItem = CreateBarItem(group.ButtonBar, tag, icon, caption, toolTip);
        }

        private BarItem CreateBarItem(ButtonBar buttonBar, object tag, int icon, string caption, string toolTip)
        {
            BarItem barItem = new BarItem();

            barItem.Caption = caption;
            barItem.ToolTipText = toolTip;
            barItem.ImageIndex = icon;
            barItem.Tag = tag;
            // 24 для 9 шрифта и 21 для 8го
            buttonBar.Height += 23;
            buttonBar.Parent.Height += 23;
            buttonBar.Items.Add(barItem);

            barItem.Selected = false;

            return barItem;
        }


        //public delegate void ItemClickEventHandler(object sender, ItemClickEventArgs e);
        public event ItemClickEventHandler ItemClickEvent;

        public void ItemClickInvoke(object sender, ItemClickEventArgs e)
        {
            ItemClickEvent(sender, e);
        }
    }

    public class ControlPanelGroup
    {
        public XPPanel XPPanel { get { return m_XPPanel; } }
        private XPPanel m_XPPanel;

        public ButtonBar ButtonBar { get { return m_ButtonBar; } }
        private ButtonBar m_ButtonBar;

        public ImageList ImageList { get; set; }

        public ControlPanel Parent { get; private set; }

        public ControlPanelGroup(ControlPanel parent, string caption, int image)
        {
            Parent = parent;
            m_XPPanel = CreateXpPanel(parent.XPPanelGroup, parent.ImageSet, caption, image);
            m_ButtonBar = CreateButtonBar(m_XPPanel, ImageList ?? parent.ImageList);
            m_ButtonBar.ItemClick += new GenericClickEventHandler<BarItem>(ButtonBar_ItemClick);
        }

        private XPPanel CreateXpPanel(XPPanelGroup XPPanelGroup, ImageSet xpImageSet, string caption, int image)
        {
            XPPanel xpPanel = new XPPanel();

            xpPanel.Name = "xpPanel";
            xpPanel.Anchor = (AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            xpPanel.ForeColor = SystemColors.WindowText;
            xpPanel.BackColor = Color.Transparent;
            //xpPanel.Location = new Point(8, 8);
            xpPanel.Size = new Size(141, 100);
            xpPanel.TabIndex = 0;

            // Заголовок
            xpPanel.Caption = caption;
            xpPanel.Font = new Font("Courier New", 10F, FontStyle.Bold);
            //xpPanel.Font                            = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold);
            xpPanel.TextColors.Foreground = Color.FromArgb(38, 115, 192);
            xpPanel.TextHighlightColors.Foreground = Color.FromArgb(38, 115, 192);
            xpPanel.VertAlignment = StringAlignment.Center;
            xpPanel.HorzAlignment = StringAlignment.Near;
            xpPanel.CaptionCornerType = XPPanelControl.CornerType.TopLeft | XPPanelControl.CornerType.TopRight;
            xpPanel.CurveRadius = 10;
            xpPanel.CaptionGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            xpPanel.CaptionGradient.Start = Color.FromArgb(254, 254, 254);
            xpPanel.CaptionGradient.End = Color.FromArgb(231, 236, 242);
            xpPanel.CollapsedGlyphs.ImageSet = xpImageSet;
            xpPanel.CollapsedGlyphs.Normal = 3;
            xpPanel.CollapsedGlyphs.Pressed = 2;
            xpPanel.CollapsedGlyphs.Highlight = 2;
            xpPanel.ExpandedGlyphs.ImageSet = xpImageSet;
            xpPanel.ExpandedGlyphs.Normal = 1;
            xpPanel.ExpandedGlyphs.Pressed = 0;
            xpPanel.ExpandedGlyphs.Highlight = 0;

            // Бордюр и фон
            xpPanel.CaptionUnderline = Color.FromArgb(254, 254, 254);
            xpPanel.OutlineColor = Color.FromArgb(254, 254, 254);
            xpPanel.PanelGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            xpPanel.PanelGradient.Start = Color.FromArgb(243, 245, 248);
            xpPanel.PanelGradient.End = Color.FromArgb(243, 245, 248);

            ButtonBar btnBar = new ButtonBar();

            // Фон
            btnBar.Dock = DockStyle.Fill;
            btnBar.ThemeProperty.ColorScheme = ColorScheme.Default;
            btnBar.ThemeProperty.UseTheme = false;
            btnBar.Appearance.Bar.BackStyle.BackColor1 = Color.FromArgb(243, 245, 248);
            btnBar.Appearance.Bar.BackStyle.BackColor2 = Color.FromArgb(243, 245, 248);
            btnBar.Appearance.Bar.AppearanceBorder.BorderVisibility = ToolStripStatusLabelBorderSides.None;
            btnBar.Appearance.Bar.CornerRadius = 0;

            // Отступы
            btnBar.Spacing = -1;
            btnBar.Padding = new Padding(10, 8, 10, 8);

            XPPanelGroup.Controls.Add(xpPanel);

            return xpPanel;
        }

        private ButtonBar CreateButtonBar(XPPanel xpPanel, ImageList imageList)
        {
            ButtonBar buttonBar = new ButtonBar();
            buttonBar.ImageList = imageList;
            buttonBar.ThemeProperty.UseTheme = false;
            buttonBar.HorizontalScroll.Visible = false;
            buttonBar.VerticalScroll.Visible = false;
            buttonBar.ShowBorders = true;
            // фон Bar'а
            buttonBar.Appearance.Bar.BackStyle.BackColor1 = Color.FromArgb(243, 245, 248);
            buttonBar.Appearance.Bar.BackStyle.BackColor2 = Color.FromArgb(243, 245, 248);
            // граница Bar'а
            buttonBar.Appearance.Bar.FocusedBorder = Color.FromArgb(243, 245, 248);
            buttonBar.Appearance.Bar.NormalBorder = Color.FromArgb(243, 245, 248);
            // фон Item'а
            buttonBar.Appearance.Item.BackStyle.BackColor1 = Color.FromArgb(243, 245, 248);
            buttonBar.Appearance.Item.BackStyle.BackColor2 = Color.FromArgb(243, 245, 248);
            buttonBar.Appearance.Item.HoverStyle.BackColor1 = Color.FromArgb(228, 238, 248);
            buttonBar.Appearance.Item.HoverStyle.BackColor2 = Color.FromArgb(228, 238, 248);
            buttonBar.Appearance.Item.ClickStyle.BackColor1 = Color.FromArgb(153, 204, 255);
            buttonBar.Appearance.Item.ClickStyle.BackColor2 = Color.FromArgb(153, 204, 255);
            buttonBar.Appearance.Item.SelectedStyle.BackColor1 = Color.FromArgb(192, 221, 252);
            buttonBar.Appearance.Item.SelectedStyle.BackColor2 = Color.FromArgb(192, 221, 252);
            buttonBar.Appearance.Item.SelectedHoverStyle.BackColor1 = Color.FromArgb(153, 204, 255);
            buttonBar.Appearance.Item.SelectedHoverStyle.BackColor2 = Color.FromArgb(153, 204, 255);
            // граница Item'а
            buttonBar.Appearance.Item.NormalBorder = Color.FromArgb(243, 245, 248);
            buttonBar.Appearance.Item.HoverBorder = Color.FromArgb(51, 153, 255);
            buttonBar.Appearance.Item.SelectedBorder = Color.FromArgb(51, 153, 255);

            buttonBar.Appearance.Item.AppearenceText.Font = new Font("Courier New", 9F, FontStyle.Regular);
            buttonBar.Appearance.Item.AppearenceText.Alignment = StringAlignment.Near;
            buttonBar.ImageAlignment = ImageAlignment.Left;

            buttonBar.Padding = new Padding(10, 8, 10, 8);
            buttonBar.Spacing = 0;

            xpPanel.Controls.Add(buttonBar);
            buttonBar.Location = new Point(1, 31);
            //buttonBar.Width  = 174;
            buttonBar.Height = 30;
            xpPanel.Height = 20 + 28;

            return buttonBar;
        }

        public void SelectItem(ControlPanelItem item)
        {
            foreach (ControlPanelItem c_item in m_ItemsDictionary.Values)
            {
                if (item == null || c_item != item)
                    c_item.BarItem.Selected = false;
            }
            if (item != null)
                item.BarItem.Selected = true;
        }

        private Dictionary<String, ControlPanelItem> m_ItemsDictionary = new Dictionary<String, ControlPanelItem>();

        public ControlPanelItem AddItem(int icon, string caption, string toolTip)
        {
            ControlPanelItem item = new ControlPanelItem(this, icon, caption, toolTip);

            m_ItemsDictionary.Add(caption, item);

            return item;
        }

        public ControlPanelItem AddItem(int icon, string caption, string toolTip, ItemClickEventHandler onClickEvent)
        {
            ControlPanelItem item = new ControlPanelItem(this, icon, caption, toolTip);

            m_ItemsDictionary.Add(caption, item);

            item.ItemClickEvent += onClickEvent;

            return item;
        }

        public ControlPanelItem AddItem(int icon, string caption, string toolTip, string tag, ItemClickEventHandler onClickEvent)
        {
            ControlPanelItem item = new ControlPanelItem(this, tag, icon, caption, toolTip);

            m_ItemsDictionary.Add(tag, item);

            item.ItemClickEvent += onClickEvent;

            return item;
        }

        public ControlPanelItem GetItemByName(string caption)
        {
            ControlPanelItem item;
            bool result = m_ItemsDictionary.TryGetValue(caption, out item);
            return result ? item : null;
        }

        private void ButtonBar_ItemClick(object sender, GenericClickEventArgs<BarItem> e)
        {
            string caption = (string)m_ButtonBar.SelectedItem.Tag;
            ControlPanelItem item = m_ItemsDictionary[caption];

            item.ItemClickInvoke(sender, new ItemClickEventArgs(item, e.Button, ""));

            foreach (ControlPanelGroup group in Parent.GetGroups())
            {
                if (group != null && group != this && group.ButtonBar.SelectedItem != null)
                    group.ButtonBar.SelectedItem.Selected = false;
            }
        }
    }

    public class ControlPanel
    {
        public Form Parent { get { return m_Parent; } }
        private Form m_Parent;

        public XPPanelGroup XPPanelGroup { get { return m_XPPanelGroup; } }
        private XPPanelGroup m_XPPanelGroup;

        public ImageSet ImageSet { get; set; }

        public ImageList ImageList { get; set; }

        public ControlPanel(Form parent)
        {
            m_Parent = parent;
            m_XPPanelGroup = CreateXpPanelGroup();
            m_XPPanelGroup.SizeChanged += new EventHandler(xpPanelGroup_SizeChanged);
        }

        private XPPanelGroup CreateXpPanelGroup()
        {
            XPPanelGroup xpPanelGroup = new XPPanelGroup();
            xpPanelGroup.PanelGradient.Start = Color.FromArgb(222, 229, 236);
            xpPanelGroup.PanelGradient.End = Color.FromArgb(178, 198, 220);

            xpPanelGroup.Padding = new Padding(0);
            xpPanelGroup.BorderMargin = new Size(12, 12);
            xpPanelGroup.PanelSpacing = 12;

            xpPanelGroup.Width = 142;
            xpPanelGroup.Dock = DockStyle.Left;

            Parent.Controls.Add(xpPanelGroup);
            return xpPanelGroup;
        }


        private Dictionary<String, ControlPanelGroup> m_GroupsDictionary = new Dictionary<String, ControlPanelGroup>();

        public ControlPanelGroup AddGroup(string caption, int image)
        {
            ControlPanelGroup group = new ControlPanelGroup(this, caption, image);

            m_GroupsDictionary.Add(caption, group);

            return group;
        }

        public ControlPanelGroup GetGroupByName(string caption)
        {
            ControlPanelGroup group;
            bool result = m_GroupsDictionary.TryGetValue(caption, out group);

            return result ? group : null;
        }

        public ControlPanelGroup[] GetGroups()
        {
            ControlPanelGroup[] groups = new ControlPanelGroup[m_GroupsDictionary.Values.Count + 1];
            m_GroupsDictionary.Values.CopyTo(groups, 0);
            return groups;
        }

        private void xpPanelGroup_SizeChanged(object sender, EventArgs e)
        {
            ControlPanelGroup[] array = new ControlPanelGroup[m_GroupsDictionary.Values.Count];
            m_GroupsDictionary.Values.CopyTo(array, 0);
            ButtonBar xpBar = array[0].ButtonBar;

            int width = m_XPPanelGroup.Size.Width - 2 * m_XPPanelGroup.BorderMargin.Width - xpBar.Padding.Left - xpBar.Padding.Right;
            if (!m_XPPanelGroup.VerticalScroll.Visible)
                width += SystemInformation.VerticalScrollBarWidth;
            if (xpBar.ButtonWidth != width)
                foreach (ControlPanelGroup group in m_GroupsDictionary.Values)
                {
                    group.ButtonBar.ButtonWidth = width;
                }
        }
    }

    public class Resources
    {
        public static Assembly Assembly = Assembly.GetExecutingAssembly();

        public static Encoding Encoding { get { return m_Encoding; } }
        private static Encoding m_Encoding = Encoding.GetEncoding(1251);


        private static string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }


        public static bool ExistsRes(string name)
        {
            string[] resNames = Assembly.GetManifestResourceNames();
            foreach (string resName in resNames)
            {
                if (resName.Equals(name))
                    return true;
            }
            return false;
        }

        public static Stream GetStream(string directory, string file)
        {
            return GetStream(directory, file, "");
        }

        public static Stream GetStream(string directory, string filename, string extension)
        {
            if ((!String.IsNullOrEmpty(directory) && directory[0] == '.') || (!String.IsNullOrEmpty(filename) && filename[0] == '.') || (!String.IsNullOrEmpty(extension) && extension[0] == '.'))
#if DEBUG
                System.Diagnostics.Debug.WriteLine(String.Format("Не верный формат параметров Resources.GetStream(): \"{0}\", \"{1}\", \"{2}\"", directory, filename, extension), DateTime.Now.ToString("HH':'mm':'ss") + "-> [GuiControls]");
#else
                throw new Exception(String.Format("Не верный формат параметров Resources.GetStream(): \"{0}\", \"{1}\", \"{2}\"", directory, filename, extension));   
#endif

            return GetStreamFromRes("GuiControls." + directory + "." + filename + (String.IsNullOrEmpty(extension) ? "" : ("." + extension)));
        }

        public static Stream GetStreamFromRes(string name)
        {
            Stream stream = Assembly.GetManifestResourceStream(name);

            try
            {
                stream = Assembly.GetManifestResourceStream(name);
                if (stream == null)
                    throw new NullReferenceException();
            }
            catch (Exception e)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(String.Format("Не удалось получить поток данных из сборки: \"{0}\".", name), DateTime.Now.ToString("HH':'mm':'ss") + "-> [GuiControls]");
#else
                throw new Exception(String.Format("[DataResources]  При попытке получить поток данных из сборки: \"{0}\" возникло исключение типа: {1}", name, e));
#endif
                stream = null;
            }

            return stream;
        }


        public static ImageSet BuildImageSet(string directory, String[] files)
        {
            ImageSet imageSet = new ImageSet();

            foreach (string file in files)
            {
                string[] temp = file.Split('.');
                string extension = temp[temp.Length - 1];

                Stream stream = GetStream(directory, file);
                if (extension == "ico")
                {
                    Icon icon = new Icon(stream);
                    imageSet.Images.Add(icon);
                }
                else
                {
                    Image image = Image.FromStream(stream);
                    imageSet.Images.Add(image);
                }
            }

            return imageSet;
        }

        public static ImageList BuildImageList(string directory, String[] files)
        {
            ImageList imageList = new ImageList();

            foreach (string file in files)
            {
                string[] temp = file.Split('.');
                string extension = temp[temp.Length - 1];

                Stream stream = GetStream(directory, file);
                if (extension == "ico")
                {
                    Icon icon = new Icon(stream);
                    imageList.Images.Add(icon);
                }
                else
                {
                    Image image = Image.FromStream(stream);
                    imageList.Images.Add(image);
                }
            }

            return imageList;
        }
    }
}