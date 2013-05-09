using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using ButtonBarsControl.Control;
using ButtonBarsControl.Design.Entity;
using ButtonBarsControl.Design.Generics;
using ButtonBarsControl.Design.Layout;
using ButtonBarsControl.Design.Utility;
using ButtonBarsControl.Properties;
using Appearance=ButtonBarsControl.Design.Layout.Appearance;

namespace ButtonBarsControl.Design.Editors
{
    internal class AppearanceEditor : UITypeEditor
    {
        /// <summary>
        /// Gets the editor style used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"/> method.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Drawing.Design.UITypeEditorEditStyle"/> value that indicates the style of editor used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"/> method. If the <see cref="T:System.Drawing.Design.UITypeEditor"/> does not support this method, then <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"/> will return <see cref="F:System.Drawing.Design.UITypeEditorEditStyle.None"/>.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that can be used to gain additional context information. </param>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// Edits the specified object's value using the editor style indicated by the <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"/> method.
        /// </summary>
        /// <returns>
        /// The new value of the object. If the value of the object has not changed, this should return the same object it was passed.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that can be used to gain additional context information. </param><param name="provider">An <see cref="T:System.IServiceProvider"/> that this editor can use to obtain services. </param><param name="value">The object to edit. </param>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null && context.Instance != null)
            {
                var editor = new AppearanceEditorUI((ButtonBar) (context.Instance));
                editor.ShowDialog();
            }
            return base.EditValue(context, provider, value);
        }

        #region Nested type: AppearanceEditorUI

        internal class AppearanceEditorUI : Form
        {
            private readonly AppearanceBar appBar;
            private readonly AppearanceItem appItem;
            private readonly ButtonBar original;
            private ButtonBar bBar;

            private Button btnCancel;
            private Button btnOK;
            private IContainer components;
            private LinkLabel lblApply;
            private Label lblAvailableTheme;
            private Label lblCurrentStyle;
            private LinkLabel lblLoad;
            private Label lblPreview;
            private LinkLabel lblReload;
            private LinkLabel lblReset;
            private LinkLabel lblSave;
            private ListBox lbxTemplate;
            private PropertyGrid pgrdBar;
            private PropertyGrid pgrdItem;
            private Panel pnlBottom;
            private Panel pnlLeft;
            private Panel pnlRight;
            private Panel pnlTop;
            private TabControl tabMain;
            private TabPage tpBar;
            private TabPage tpItems;

            public AppearanceEditorUI(ButtonBar original)
            {
                this.original = original;
                appBar = (AppearanceBar) original.Appearance.Bar.Clone();
                appItem = (AppearanceItem) original.Appearance.Item.Clone();
                InitializeComponent();
                pgrdBar.SelectedObject = original.Appearance.Bar;
                pgrdItem.SelectedObject = original.Appearance.Item;
                lbxTemplate.Items.AddRange(new object[]
                                               {
                                                   Resources.THEME_VS2005,
                                                   Resources.THEME_CLASSIC,
                                                   Resources.THEME_BLUE,
                                                   Resources.THEME_OLIVE,
                                                   Resources.THEME_ROYAL,
                                                   Resources.THEME_SILVER
                                               });
                lbxTemplate.SelectedIndex = 0;
                bBar.Appearance.Bar.Assign(appBar);
                bBar.Appearance.Item.Assign(appItem);
                bBar.ThemeProperty.UseTheme = false;
                bBar.SetThemeDefaults();
                bBar.RefreshControl();
                lblCurrentStyle.Text = Resources.LBL_CURRENT_STYLE;
                lblApply.Text = Resources.LNK_APPLYTHEME;
                lblAvailableTheme.Text = Resources.LBL_AVAILABLE_THEME;
                lblLoad.Text = Resources.LNK_LOAD;
                lblPreview.Text = Resources.LBL_PREVIEW;
                lblReload.Text = Resources.LNK_RELOAD;
                lblReset.Text = Resources.LNK_RESET;
                lblSave.Text = Resources.LNK_SAVETHEME;
                Text = Resources.FORM_TEXT;
            }

            public AppearanceBar AppearanceBar
            {
                get { return ((DialogResult == DialogResult.OK) ? (AppearanceBar) pgrdBar.SelectedObject : appBar); }
            }

            public AppearanceItem AppearanceItem
            {
                get { return ((DialogResult == DialogResult.OK) ? (AppearanceItem) pgrdItem.SelectedObject : appItem); }
            }

            private void OnApplyClick(object sender, LinkLabelLinkClickedEventArgs e)
            {
                var selection = lbxTemplate.SelectedItem.ToString();
                if (Resources.THEME_VS2005 == selection)
                    SetColors(ColorSchemeDefinition.VS2005);
                else if (Resources.THEME_CLASSIC == selection)
                    SetColors(ColorSchemeDefinition.Classic);
                else if (Resources.THEME_BLUE == selection)
                    SetColors(ColorSchemeDefinition.Blue);
                else if (Resources.THEME_OLIVE == selection)
                    SetColors(ColorSchemeDefinition.OliveGreen);
                else if (Resources.THEME_ROYAL == selection)
                    SetColors(ColorSchemeDefinition.Royale);
                else if (Resources.THEME_SILVER == selection)
                    SetColors(ColorSchemeDefinition.Silver);
                pgrdBar.Refresh();
                pgrdItem.Refresh();
                bBar.RefreshControl();
                original.RefreshControl();
            }

            private void SetColors(ColorSchemeDefinition def)
            {
                var currentBarStyle = (AppearanceBar) pgrdBar.SelectedObject;
                var currentItemStyle = (AppearanceItem) pgrdItem.SelectedObject;

                currentBarStyle.BackStyle.Assign(def.BarBackStyle);
                currentBarStyle.FocusedBorder = def.BarFocusedBorder;
                currentBarStyle.NormalBorder = def.BarNormalBorder;
                currentBarStyle.ResetAppearanceBorder();
                currentBarStyle.ResetCornerRadius();
                currentBarStyle.DisabledMask = def.DisabledMask;

                currentItemStyle.BackStyle.Assign(def.BackStyle);
                currentItemStyle.ClickStyle.Assign(def.ClickStyle);
                currentItemStyle.Gradient = def.GradientMode;
                currentItemStyle.HoverBorder = def.HoverBorder;
                currentItemStyle.HoverForeGround = def.HoverForeGround;
                currentItemStyle.HoverStyle.Assign(def.HoverStyle);
                currentItemStyle.NormalBorder = def.NormalBorder;
                currentItemStyle.NormalForeGround = def.NormalForeGround;
                currentItemStyle.SelectedBorder = def.SelectedBorder;
                currentItemStyle.SelectedForeGround = def.SelectedForeGround;
                currentItemStyle.SelectedHoverStyle.Assign(def.SelectedHoverStyle);
                currentItemStyle.SelectedStyle.Assign(def.SelectedStyle);
                currentItemStyle.DisabledStyle.Assign(def.DisabledStyle);
                currentItemStyle.DisabledBorder = def.DisabledBorder;
                currentItemStyle.DisabledForeGround = def.DisabledForeGround;
                bBar.Appearance.Bar.Assign(currentBarStyle);
                bBar.Appearance.Item.Assign(currentItemStyle);
                bBar.SetThemeDefaults();
                original.Appearance.Bar.Assign(currentBarStyle);
                original.Appearance.Item.Assign(currentItemStyle);
                original.SetThemeDefaults();
            }

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }

            private void OnSaveClick(object sender, LinkLabelLinkClickedEventArgs e)
            {
                try
                {
                    using (var dlg = new SaveFileDialog())
                    {
                        dlg.Filter = Resources.XML_FILE;
                        if (dlg.ShowDialog() != DialogResult.OK)
                            return;
                        using (XmlWriter writer = new XmlTextWriter(dlg.FileName, Encoding.UTF8))
                        {
                            var serializer = new XmlSerializer(typeof (Appearance));
                            var app = new Appearance();
                            app.Bar.Assign((AppearanceBar) pgrdBar.SelectedObject);
                            app.Item.Assign((AppearanceItem) pgrdItem.SelectedObject);
                            serializer.Serialize(writer, app);
                            writer.Flush();
                            writer.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            private void OnLoadClick(object sender, LinkLabelLinkClickedEventArgs e)
            {
                try
                {
                    using (var dlg = new OpenFileDialog())
                    {
                        dlg.Filter = Resources.XML_FILE;
                        if (dlg.ShowDialog() != DialogResult.OK)
                            return;
                        using (var fs = new FileStream(dlg.FileName, FileMode.Open))
                        {
                            var serializer = new XmlSerializer(typeof (Appearance));
                            var app = (Appearance) serializer.Deserialize(fs);
                            var o = (AppearanceBar) pgrdBar.SelectedObject;
                            var o2 = (AppearanceItem) pgrdItem.SelectedObject;
                            o.Assign(app.Bar);
                            o2.Assign(app.Item);
                            bBar.Appearance.Assign(app);
                            bBar.SetThemeDefaults();
                            original.Appearance.Assign(app);
                            original.SetThemeDefaults();
                            pgrdBar.Refresh();
                            pgrdItem.Refresh();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            private void OnResetClick(object sender, LinkLabelLinkClickedEventArgs e)
            {
                var o = (AppearanceBar) pgrdBar.SelectedObject;
                o.Reset();
                var o1 = (AppearanceItem) pgrdItem.SelectedObject;
                o1.Reset();

                pgrdBar.Refresh();
                pgrdItem.Refresh();

                bBar.Appearance.Bar.Reset();
                bBar.Appearance.Item.Reset();
                bBar.SetThemeDefaults();
                bBar.RefreshControl();

                original.Appearance.Bar.Reset();
                original.Appearance.Item.Reset();
                original.SetThemeDefaults();
                original.RefreshControl();
            }

            private void OnOKClick(object sender, EventArgs e)
            {
                var currentBarStyle = (AppearanceBar) pgrdBar.SelectedObject;
                var currentItemStyle = (AppearanceItem) pgrdItem.SelectedObject;
                original.Appearance.Bar.Assign(currentBarStyle);
                original.Appearance.Item.Assign(currentItemStyle);
                original.SetThemeDefaults();
            }

            private void OnCancelClick(object sender, EventArgs e)
            {
                original.Appearance.Bar.Assign(appBar);
                original.Appearance.Item.Assign(appItem);
                original.SetThemeDefaults();
                original.RefreshControl();
            }

            private void OnCustomDrawItems(object sender, DrawItemsEventArgs e)
            {
                var o = (AppearanceItem) pgrdItem.SelectedObject;
                var ts = o.AppearenceText.IsEmpty ? ButtonBar.DEFAULT.Item.AppearenceText : o.AppearenceText;
                switch (int.Parse(e.Item.Tag.ToString()))
                {
                    case 1:
                        PaintUtility.PaintGradientRectangle(e.Graphics, e.Bounds,
                                                            o.DisabledStyle.IsEmpty
                                                                ? ButtonBar.DEFAULT.Item.DisabledStyle
                                                                : o.DisabledStyle);
                        PaintUtility.PaintBorder(e.Graphics, e.Bounds,
                                                 o.DisabledBorder.IsEmpty
                                                     ? ButtonBar.DEFAULT.Item.DisabledBorder
                                                     : o.DisabledBorder);
                        PaintUtility.DrawString(e.Graphics, e.Bounds, e.Item.Caption, ts, false,
                                                o.DisabledForeGround.IsEmpty
                                                    ? ButtonBar.DEFAULT.Item.DisabledForeGround
                                                    : o.DisabledForeGround);
                        break;
                    case 2:
                        PaintUtility.PaintGradientRectangle(e.Graphics, e.Bounds,
                                                            o.SelectedStyle.IsEmpty
                                                                ? ButtonBar.DEFAULT.Item.SelectedStyle
                                                                : o.SelectedStyle);
                        PaintUtility.PaintBorder(e.Graphics, e.Bounds,
                                                 o.SelectedBorder.IsEmpty
                                                     ? ButtonBar.DEFAULT.Item.SelectedBorder
                                                     : o.SelectedBorder);
                        PaintUtility.DrawString(e.Graphics, e.Bounds, e.Item.Caption, ts, false,
                                                o.SelectedForeGround.IsEmpty
                                                    ? ButtonBar.DEFAULT.Item.SelectedForeGround
                                                    : o.SelectedForeGround);
                        break;
                    case 3:
                        PaintUtility.PaintGradientRectangle(e.Graphics, e.Bounds,
                                                            o.ClickStyle.IsEmpty
                                                                ? ButtonBar.DEFAULT.Item.ClickStyle
                                                                : o.ClickStyle);
                        PaintUtility.PaintBorder(e.Graphics, e.Bounds,
                                                 o.SelectedBorder.IsEmpty
                                                     ? ButtonBar.DEFAULT.Item.SelectedBorder
                                                     : o.SelectedBorder);
                        PaintUtility.DrawString(e.Graphics, e.Bounds, e.Item.Caption, ts, false,
                                                o.SelectedForeGround.IsEmpty
                                                    ? ButtonBar.DEFAULT.Item.SelectedForeGround
                                                    : o.SelectedForeGround);
                        break;
                    case 4:
                        PaintUtility.PaintGradientRectangle(e.Graphics, e.Bounds,
                                                            o.HoverStyle.IsEmpty
                                                                ? ButtonBar.DEFAULT.Item.HoverStyle
                                                                : o.HoverStyle);
                        PaintUtility.PaintBorder(e.Graphics, e.Bounds,
                                                 o.HoverBorder.IsEmpty
                                                     ? ButtonBar.DEFAULT.Item.HoverBorder
                                                     : o.HoverBorder);
                        PaintUtility.DrawString(e.Graphics, e.Bounds, e.Item.Caption, ts, false,
                                                o.HoverForeGround.IsEmpty
                                                    ? ButtonBar.DEFAULT.Item.HoverForeGround
                                                    : o.HoverForeGround);
                        break;
                    case 5:
                        PaintUtility.PaintGradientRectangle(e.Graphics, e.Bounds,
                                                            o.SelectedHoverStyle.IsEmpty
                                                                ? ButtonBar.DEFAULT.Item.SelectedHoverStyle
                                                                : o.SelectedHoverStyle);
                        PaintUtility.PaintBorder(e.Graphics, e.Bounds,
                                                 o.HoverBorder.IsEmpty
                                                     ? ButtonBar.DEFAULT.Item.HoverBorder
                                                     : o.HoverBorder);
                        PaintUtility.DrawString(e.Graphics, e.Bounds, e.Item.Caption, ts, false,
                                                o.HoverForeGround.IsEmpty
                                                    ? ButtonBar.DEFAULT.Item.HoverForeGround
                                                    : o.HoverForeGround);
                        break;
                    default:
                        PaintUtility.PaintGradientRectangle(e.Graphics, e.Bounds,
                                                            o.BackStyle.IsEmpty
                                                                ? ButtonBar.DEFAULT.Item.BackStyle
                                                                : o.BackStyle);
                        PaintUtility.PaintBorder(e.Graphics, e.Bounds,
                                                 o.NormalBorder.IsEmpty
                                                     ? ButtonBar.DEFAULT.Item.NormalBorder
                                                     : o.NormalBorder);
                        PaintUtility.DrawString(e.Graphics, e.Bounds, e.Item.Caption, ts, false,
                                                o.NormalForeGround.IsEmpty
                                                    ? ButtonBar.DEFAULT.Item.NormalForeGround
                                                    : o.NormalForeGround);
                        break;
                }
                e.Handeled = true;
            }

            private void OnSelectionChanging(int index, GenericChangeEventArgs<BarItem> e)
            {
                e.Cancel = true;
            }

            private void OnReloadClicked(object sender, LinkLabelLinkClickedEventArgs e)
            {
                bBar.Appearance.Bar.Assign(appBar);
                bBar.Appearance.Item.Assign(appItem);
                bBar.SetThemeDefaults();
                original.Appearance.Bar.Assign(appBar);
                original.Appearance.Item.Assign(appItem);
                original.SetThemeDefaults();
                pgrdBar.Refresh();
                pgrdItem.Refresh();
                bBar.RefreshControl();
                original.RefreshControl();
            }

            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
                this.components = new System.ComponentModel.Container();
                var barItem1 = new ButtonBarsControl.Control.BarItem();
                var barItem2 = new ButtonBarsControl.Control.BarItem();
                var barItem3 = new ButtonBarsControl.Control.BarItem();
                var barItem4 = new ButtonBarsControl.Control.BarItem();
                var barItem5 = new ButtonBarsControl.Control.BarItem();
                var barItem6 = new ButtonBarsControl.Control.BarItem();
                this.pnlTop = new System.Windows.Forms.Panel();
                this.pnlRight = new System.Windows.Forms.Panel();
                this.tabMain = new System.Windows.Forms.TabControl();
                this.tpBar = new System.Windows.Forms.TabPage();
                this.pgrdBar = new System.Windows.Forms.PropertyGrid();
                this.tpItems = new System.Windows.Forms.TabPage();
                this.pgrdItem = new System.Windows.Forms.PropertyGrid();
                this.lblCurrentStyle = new System.Windows.Forms.Label();
                this.pnlLeft = new System.Windows.Forms.Panel();
                this.lblReload = new System.Windows.Forms.LinkLabel();
                this.lblReset = new System.Windows.Forms.LinkLabel();
                this.lblSave = new System.Windows.Forms.LinkLabel();
                this.lblLoad = new System.Windows.Forms.LinkLabel();
                this.lblPreview = new System.Windows.Forms.Label();
                this.bBar = new ButtonBarsControl.Control.ButtonBar(this.components);
                this.lblAvailableTheme = new System.Windows.Forms.Label();
                this.lblApply = new System.Windows.Forms.LinkLabel();
                this.lbxTemplate = new System.Windows.Forms.ListBox();
                this.pnlBottom = new System.Windows.Forms.Panel();
                this.btnCancel = new System.Windows.Forms.Button();
                this.btnOK = new System.Windows.Forms.Button();
                this.pnlTop.SuspendLayout();
                this.pnlRight.SuspendLayout();
                this.tabMain.SuspendLayout();
                this.tpBar.SuspendLayout();
                this.tpItems.SuspendLayout();
                this.pnlLeft.SuspendLayout();
                this.pnlBottom.SuspendLayout();
                this.SuspendLayout();
                // 
                // pnlTop
                // 
                this.pnlTop.Controls.Add(this.pnlRight);
                this.pnlTop.Controls.Add(this.pnlLeft);
                this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
                this.pnlTop.Location = new System.Drawing.Point(0, 0);
                this.pnlTop.Name = "pnlTop";
                this.pnlTop.Size = new System.Drawing.Size(420, 441);
                this.pnlTop.TabIndex = 0;
                // 
                // pnlRight
                // 
                this.pnlRight.Controls.Add(this.tabMain);
                this.pnlRight.Controls.Add(this.lblCurrentStyle);
                this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
                this.pnlRight.Location = new System.Drawing.Point(179, 0);
                this.pnlRight.Name = "pnlRight";
                this.pnlRight.Size = new System.Drawing.Size(241, 441);
                this.pnlRight.TabIndex = 1;
                // 
                // tabMain
                // 
                this.tabMain.Anchor =
                    ((System.Windows.Forms.AnchorStyles)
                     ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                       | System.Windows.Forms.AnchorStyles.Right)));
                this.tabMain.Controls.Add(this.tpBar);
                this.tabMain.Controls.Add(this.tpItems);
                this.tabMain.Location = new System.Drawing.Point(3, 26);
                this.tabMain.Name = "tabMain";
                this.tabMain.SelectedIndex = 0;
                this.tabMain.Size = new System.Drawing.Size(235, 409);
                this.tabMain.TabIndex = 2;
                // 
                // tpBar
                // 
                this.tpBar.Controls.Add(this.pgrdBar);
                this.tpBar.Location = new System.Drawing.Point(4, 22);
                this.tpBar.Name = "tpBar";
                this.tpBar.Padding = new System.Windows.Forms.Padding(3);
                this.tpBar.Size = new System.Drawing.Size(227, 383);
                this.tpBar.TabIndex = 0;
                this.tpBar.Text = global::ButtonBarsControl.Properties.Resources.TAB_BAR;
                this.tpBar.UseVisualStyleBackColor = true;
                // 
                // pgrdBar
                // 
                this.pgrdBar.CommandsVisibleIfAvailable = false;
                this.pgrdBar.Dock = System.Windows.Forms.DockStyle.Fill;
                this.pgrdBar.HelpVisible = false;
                this.pgrdBar.Location = new System.Drawing.Point(3, 3);
                this.pgrdBar.Name = "pgrdBar";
                this.pgrdBar.PropertySort = System.Windows.Forms.PropertySort.NoSort;
                this.pgrdBar.Size = new System.Drawing.Size(221, 377);
                this.pgrdBar.TabIndex = 1;
                this.pgrdBar.ToolbarVisible = false;
                // 
                // tpItems
                // 
                this.tpItems.Controls.Add(this.pgrdItem);
                this.tpItems.Location = new System.Drawing.Point(4, 22);
                this.tpItems.Name = "tpItems";
                this.tpItems.Padding = new System.Windows.Forms.Padding(3);
                this.tpItems.Size = new System.Drawing.Size(227, 383);
                this.tpItems.TabIndex = 1;
                this.tpItems.Text = global::ButtonBarsControl.Properties.Resources.TAB_ITEMS;
                this.tpItems.UseVisualStyleBackColor = true;
                // 
                // pgrdItem
                // 
                this.pgrdItem.CommandsVisibleIfAvailable = false;
                this.pgrdItem.Dock = System.Windows.Forms.DockStyle.Fill;
                this.pgrdItem.HelpVisible = false;
                this.pgrdItem.Location = new System.Drawing.Point(3, 3);
                this.pgrdItem.Name = "pgrdItem";
                this.pgrdItem.PropertySort = System.Windows.Forms.PropertySort.NoSort;
                this.pgrdItem.Size = new System.Drawing.Size(221, 377);
                this.pgrdItem.TabIndex = 2;
                this.pgrdItem.ToolbarVisible = false;
                // 
                // lblCurrentStyle
                // 
                this.lblCurrentStyle.AutoSize = true;
                this.lblCurrentStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                                                                    ((System.Drawing.FontStyle)
                                                                     ((System.Drawing.FontStyle.Bold |
                                                                       System.Drawing.FontStyle.Underline))));
                this.lblCurrentStyle.Location = new System.Drawing.Point(6, 9);
                this.lblCurrentStyle.Name = "lblCurrentStyle";
                this.lblCurrentStyle.Size = new System.Drawing.Size(80, 13);
                this.lblCurrentStyle.TabIndex = 0;
                this.lblCurrentStyle.Text = "Current Style";
                // 
                // pnlLeft
                // 
                this.pnlLeft.Controls.Add(this.lblReload);
                this.pnlLeft.Controls.Add(this.lblReset);
                this.pnlLeft.Controls.Add(this.lblSave);
                this.pnlLeft.Controls.Add(this.lblLoad);
                this.pnlLeft.Controls.Add(this.lblPreview);
                this.pnlLeft.Controls.Add(this.bBar);
                this.pnlLeft.Controls.Add(this.lblAvailableTheme);
                this.pnlLeft.Controls.Add(this.lblApply);
                this.pnlLeft.Controls.Add(this.lbxTemplate);
                this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
                this.pnlLeft.Location = new System.Drawing.Point(0, 0);
                this.pnlLeft.Name = "pnlLeft";
                this.pnlLeft.Size = new System.Drawing.Size(179, 441);
                this.pnlLeft.TabIndex = 0;
                // 
                // lblReload
                // 
                this.lblReload.AutoSize = true;
                this.lblReload.Location = new System.Drawing.Point(6, 213);
                this.lblReload.Name = "lblReload";
                this.lblReload.Size = new System.Drawing.Size(41, 13);
                this.lblReload.TabIndex = 8;
                this.lblReload.TabStop = true;
                this.lblReload.Text = "Re&load";
                this.lblReload.LinkClicked +=
                    new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnReloadClicked);
                // 
                // lblReset
                // 
                this.lblReset.AutoSize = true;
                this.lblReset.Location = new System.Drawing.Point(6, 194);
                this.lblReset.Name = "lblReset";
                this.lblReset.Size = new System.Drawing.Size(35, 13);
                this.lblReset.TabIndex = 7;
                this.lblReset.TabStop = true;
                this.lblReset.Text = "&Reset";
                this.lblReset.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnResetClick);
                // 
                // lblSave
                // 
                this.lblSave.AutoSize = true;
                this.lblSave.Location = new System.Drawing.Point(6, 175);
                this.lblSave.Name = "lblSave";
                this.lblSave.Size = new System.Drawing.Size(68, 13);
                this.lblSave.TabIndex = 6;
                this.lblSave.TabStop = true;
                this.lblSave.Text = "&Save Theme";
                this.lblSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnSaveClick);
                // 
                // lblLoad
                // 
                this.lblLoad.AutoSize = true;
                this.lblLoad.Location = new System.Drawing.Point(6, 156);
                this.lblLoad.Name = "lblLoad";
                this.lblLoad.Size = new System.Drawing.Size(67, 13);
                this.lblLoad.TabIndex = 5;
                this.lblLoad.TabStop = true;
                this.lblLoad.Text = "&Load Theme";
                this.lblLoad.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLoadClick);
                // 
                // lblPreview
                // 
                this.lblPreview.AutoSize = true;
                this.lblPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                                                               ((System.Drawing.FontStyle)
                                                                ((System.Drawing.FontStyle.Bold |
                                                                  System.Drawing.FontStyle.Underline))));
                this.lblPreview.Location = new System.Drawing.Point(6, 234);
                this.lblPreview.Name = "lblPreview";
                this.lblPreview.Size = new System.Drawing.Size(52, 13);
                this.lblPreview.TabIndex = 3;
                this.lblPreview.Text = "Preview";
                // 
                // bBar
                // 
                this.bBar.AutoScroll = true;
                this.bBar.AutoScrollMinSize = new System.Drawing.Size(0, 165);
                this.bBar.ButtonWidth = 150;
                barItem1.Caption = global::ButtonBarsControl.Properties.Resources.NORMAL_TEXT;
                barItem1.Selected = true;
                barItem1.Tag = "0";
                barItem1.ToolTipText = global::ButtonBarsControl.Properties.Resources.NORMAL_TEXT;
                barItem2.Caption = global::ButtonBarsControl.Properties.Resources.DISABLED_TEXT;
                barItem2.Tag = "1";
                barItem2.ToolTipText = global::ButtonBarsControl.Properties.Resources.DISABLED_TEXT;
                barItem3.Caption = global::ButtonBarsControl.Properties.Resources.SELECTED_TEXT;
                barItem3.Tag = "2";
                barItem3.ToolTipText = global::ButtonBarsControl.Properties.Resources.SELECTED_TEXT;
                barItem4.Caption = global::ButtonBarsControl.Properties.Resources.CLICKED_TEXT;
                barItem4.Tag = "3";
                barItem4.ToolTipText = global::ButtonBarsControl.Properties.Resources.CLICKED_TEXT;
                barItem5.Caption = global::ButtonBarsControl.Properties.Resources.HOVERED_TEXT;
                barItem5.Tag = "4";
                barItem5.ToolTipText = global::ButtonBarsControl.Properties.Resources.HOVERED_TEXT;
                barItem6.Caption = global::ButtonBarsControl.Properties.Resources.SELECTED_HOVER_TEXT;
                barItem6.Tag = "5";
                barItem6.ToolTipText = global::ButtonBarsControl.Properties.Resources.SELECTED_HOVER_TEXT;
                this.bBar.Items.AddRange(new ButtonBarsControl.Control.BarItem[]
                                             {
                                                 barItem1,
                                                 barItem2,
                                                 barItem3,
                                                 barItem4,
                                                 barItem5,
                                                 barItem6
                                             });
                this.bBar.Location = new System.Drawing.Point(7, 250);
                this.bBar.Name = "bBar";
                this.bBar.Padding = new System.Windows.Forms.Padding(3);
                this.bBar.Size = new System.Drawing.Size(150, 189);
                this.bBar.TabIndex = 4;
                this.bBar.Text = "Button Bar";
                this.bBar.ThemeProperty.UseTheme = false;
                this.bBar.CustomDrawItems +=
                    new System.EventHandler<ButtonBarsControl.Design.Entity.DrawItemsEventArgs>(this.OnCustomDrawItems);
                this.bBar.SelectionChanging +=
                    new ButtonBarsControl.Control.ItemChangingHandler(this.OnSelectionChanging);
                // 
                // lblAvailableTheme
                // 
                this.lblAvailableTheme.AutoSize = true;
                this.lblAvailableTheme.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                                                                      ((System.Drawing.FontStyle)
                                                                       ((System.Drawing.FontStyle.Bold |
                                                                         System.Drawing.FontStyle.Underline))));
                this.lblAvailableTheme.Location = new System.Drawing.Point(6, 8);
                this.lblAvailableTheme.Name = "lblAvailableTheme";
                this.lblAvailableTheme.Size = new System.Drawing.Size(103, 13);
                this.lblAvailableTheme.TabIndex = 0;
                this.lblAvailableTheme.Text = "Available themes";
                // 
                // lblApply
                // 
                this.lblApply.AutoSize = true;
                this.lblApply.Location = new System.Drawing.Point(6, 137);
                this.lblApply.Name = "lblApply";
                this.lblApply.Size = new System.Drawing.Size(117, 13);
                this.lblApply.TabIndex = 2;
                this.lblApply.TabStop = true;
                this.lblApply.Text = "&Apply to current Theme";
                this.lblApply.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnApplyClick);
                // 
                // lbxTemplate
                // 
                this.lbxTemplate.FormattingEnabled = true;
                this.lbxTemplate.Location = new System.Drawing.Point(7, 26);
                this.lbxTemplate.Name = "lbxTemplate";
                this.lbxTemplate.Size = new System.Drawing.Size(165, 108);
                this.lbxTemplate.TabIndex = 1;
                // 
                // pnlBottom
                // 
                this.pnlBottom.Controls.Add(this.btnCancel);
                this.pnlBottom.Controls.Add(this.btnOK);
                this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
                this.pnlBottom.Location = new System.Drawing.Point(0, 441);
                this.pnlBottom.Name = "pnlBottom";
                this.pnlBottom.Size = new System.Drawing.Size(420, 33);
                this.pnlBottom.TabIndex = 1;
                // 
                // btnCancel
                // 
                this.btnCancel.Anchor =
                    ((System.Windows.Forms.AnchorStyles)
                     ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
                this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.btnCancel.Location = new System.Drawing.Point(333, 6);
                this.btnCancel.Name = "btnCancel";
                this.btnCancel.Size = new System.Drawing.Size(75, 23);
                this.btnCancel.TabIndex = 1;
                this.btnCancel.Text = global::ButtonBarsControl.Properties.Resources.BTN_CANCEL;
                this.btnCancel.UseVisualStyleBackColor = true;
                this.btnCancel.Click += new System.EventHandler(this.OnCancelClick);
                // 
                // btnOK
                // 
                this.btnOK.Anchor =
                    ((System.Windows.Forms.AnchorStyles)
                     ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
                this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.btnOK.Location = new System.Drawing.Point(252, 6);
                this.btnOK.Name = "btnOK";
                this.btnOK.Size = new System.Drawing.Size(75, 23);
                this.btnOK.TabIndex = 0;
                this.btnOK.Text = global::ButtonBarsControl.Properties.Resources.BTN_OK;
                this.btnOK.UseVisualStyleBackColor = true;
                this.btnOK.Click += new System.EventHandler(this.OnOKClick);
                // 
                // AppearanceEditorUI
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(420, 474);
                this.Controls.Add(this.pnlBottom);
                this.Controls.Add(this.pnlTop);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.Name = "AppearanceEditorUI";
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                this.Text = "Appearance Editor";
                this.pnlTop.ResumeLayout(false);
                this.pnlRight.ResumeLayout(false);
                this.pnlRight.PerformLayout();
                this.tabMain.ResumeLayout(false);
                this.tpBar.ResumeLayout(false);
                this.tpItems.ResumeLayout(false);
                this.pnlLeft.ResumeLayout(false);
                this.pnlLeft.PerformLayout();
                this.pnlBottom.ResumeLayout(false);
                this.ResumeLayout(false);
            }

            # endregion
        }

        #endregion
    }
}