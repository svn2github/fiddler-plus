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

namespace UoFiddler
{
    partial class UoFiddler
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.TabPanel = new System.Windows.Forms.TabControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.unDockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Start = new System.Windows.Forms.TabPage();
            this.Buildlabel = new System.Windows.Forms.Label();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.Versionlabel = new System.Windows.Forms.Label();
            this.Animation = new System.Windows.Forms.TabPage();
            this.controlAnimations = new FiddlerControls.Animationlist();
            this.AnimData = new System.Windows.Forms.TabPage();
            this.controlAnimdata = new FiddlerControls.AnimData();
            this.Items = new System.Windows.Forms.TabPage();
            this.controlItemShow = new FiddlerControls.ItemShow();
            this.TileDatas = new System.Windows.Forms.TabPage();
            this.controlTileData = new FiddlerControls.TileDatas();
            this.LandTiles = new System.Windows.Forms.TabPage();
            this.controlLandTiles = new FiddlerControls.LandTiles();
            this.Texture = new System.Windows.Forms.TabPage();
            this.controlTexture = new FiddlerControls.Texture();
            this.Multis = new System.Windows.Forms.TabPage();
            this.controlMulti = new FiddlerControls.Multis();
            this.Gumps = new System.Windows.Forms.TabPage();
            this.controlGumps = new FiddlerControls.Gump();
            this.Sounds = new System.Windows.Forms.TabPage();
            this.controlSound = new FiddlerControls.Sounds();
            this.Light = new System.Windows.Forms.TabPage();
            this.controlLight = new FiddlerControls.Light();
            this.Hue = new System.Windows.Forms.TabPage();
            this.controlHue = new FiddlerControls.Hues();
            this.fonts = new System.Windows.Forms.TabPage();
            this.controlfonts = new FiddlerControls.Fonts();
            this.Cliloc = new System.Windows.Forms.TabPage();
            this.controlCliloc = new FiddlerControls.Cliloc();
            this.speech = new System.Windows.Forms.TabPage();
            this.controlspeech = new FiddlerControls.Speech();
            this.Skills = new System.Windows.Forms.TabPage();
            this.controlSkills = new FiddlerControls.Skills();
            this.SkillGrp = new System.Windows.Forms.TabPage();
            this.controlSkillGrp = new FiddlerControls.SkillGrp();
            this.map = new System.Windows.Forms.TabPage();
            this.controlmap = new FiddlerControls.Map();
            this.RadarCol = new System.Windows.Forms.TabPage();
            this.controlRadarCol = new FiddlerControls.RadarColor();
            this.multimap = new System.Windows.Forms.TabPage();
            this.controlMultimap = new FiddlerControls.MultiMap();
            this.Dress = new System.Windows.Forms.TabPage();
            this.controldress = new FiddlerControls.Dress();
            this.cmd = new System.Windows.Forms.TabPage();
            this.serverInteractive1 = new global::UoFiddler.Telnet.ServerInteractive();
            this.CentrEd = new System.Windows.Forms.TabPage();
            this.controlCentrEd = new FiddlerControls.CentrEditor.CentrEd();
            this.WebBrowser = new System.Windows.Forms.TabPage();
            this.controlWebBrowser = new FiddlerControls.WebBrowser();
            this.Regions = new System.Windows.Forms.TabPage();
            this.regionEditor1 = new FiddlerControls.RegionEditor.RegionEditor();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.SettingsMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.AlwaysOnTopMenuitem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pathSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.restartNeededMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButtonView = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToggleViewStart = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewAnimations = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewAnimData = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewItems = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewTileData = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewLandTiles = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewTexture = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewMulti = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewGumps = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewSounds = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewLight = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewHue = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewFonts = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewCliloc = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewSpeech = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewSkills = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewSkillGrp = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewMap = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewRadarColor = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewMultiMap = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleViewDress = new System.Windows.Forms.ToolStripMenuItem();
            this.ExternToolsDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.manageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButtonPlugins = new System.Windows.Forms.ToolStripDropDownButton();
            this.manageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.TabPanel.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.Start.SuspendLayout();
            this.Animation.SuspendLayout();
            this.AnimData.SuspendLayout();
            this.Items.SuspendLayout();
            this.TileDatas.SuspendLayout();
            this.LandTiles.SuspendLayout();
            this.Texture.SuspendLayout();
            this.Multis.SuspendLayout();
            this.Gumps.SuspendLayout();
            this.Sounds.SuspendLayout();
            this.Light.SuspendLayout();
            this.Hue.SuspendLayout();
            this.fonts.SuspendLayout();
            this.Cliloc.SuspendLayout();
            this.speech.SuspendLayout();
            this.Skills.SuspendLayout();
            this.SkillGrp.SuspendLayout();
            this.map.SuspendLayout();
            this.RadarCol.SuspendLayout();
            this.multimap.SuspendLayout();
            this.Dress.SuspendLayout();
            this.cmd.SuspendLayout();
            this.CentrEd.SuspendLayout();
            this.WebBrowser.SuspendLayout();
            this.Regions.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabPanel
            // 
            this.TabPanel.Controls.Add(this.Start);
            this.TabPanel.Controls.Add(this.Animation);
            this.TabPanel.Controls.Add(this.AnimData);
            this.TabPanel.Controls.Add(this.Items);
            this.TabPanel.Controls.Add(this.TileDatas);
            this.TabPanel.Controls.Add(this.LandTiles);
            this.TabPanel.Controls.Add(this.Texture);
            this.TabPanel.Controls.Add(this.Multis);
            this.TabPanel.Controls.Add(this.Gumps);
            this.TabPanel.Controls.Add(this.Sounds);
            this.TabPanel.Controls.Add(this.Light);
            this.TabPanel.Controls.Add(this.Hue);
            this.TabPanel.Controls.Add(this.fonts);
            this.TabPanel.Controls.Add(this.Cliloc);
            this.TabPanel.Controls.Add(this.speech);
            this.TabPanel.Controls.Add(this.Skills);
            this.TabPanel.Controls.Add(this.SkillGrp);
            this.TabPanel.Controls.Add(this.map);
            this.TabPanel.Controls.Add(this.RadarCol);
            this.TabPanel.Controls.Add(this.multimap);
            this.TabPanel.Controls.Add(this.Dress);
            this.TabPanel.Controls.Add(this.cmd);
            this.TabPanel.Controls.Add(this.CentrEd);
            this.TabPanel.Controls.Add(this.WebBrowser);
            this.TabPanel.Controls.Add(this.Regions);
            this.TabPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabPanel.Location = new System.Drawing.Point(0, 25);
            this.TabPanel.Margin = new System.Windows.Forms.Padding(0);
            this.TabPanel.Name = "TabPanel";
            this.TabPanel.SelectedIndex = 0;
            this.TabPanel.Size = new System.Drawing.Size(909, 617);
            this.TabPanel.TabIndex = 1;
            this.TabPanel.Tag = "20";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.unDockToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(117, 26);
            // 
            // unDockToolStripMenuItem
            // 
            this.unDockToolStripMenuItem.Name = "unDockToolStripMenuItem";
            this.unDockToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.unDockToolStripMenuItem.Text = "UnDock";
            this.unDockToolStripMenuItem.Click += new System.EventHandler(this.OnClickUndock);
            // 
            // Start
            // 
            this.Start.BackColor = System.Drawing.Color.White;
            this.Start.BackgroundImage = global::UoFiddler.Properties.Resources.UOFiddler;
            this.Start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Start.Controls.Add(this.Buildlabel);
            this.Start.Controls.Add(this.linkLabel3);
            this.Start.Controls.Add(this.Versionlabel);
            this.Start.Location = new System.Drawing.Point(4, 22);
            this.Start.Margin = new System.Windows.Forms.Padding(0);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(901, 591);
            this.Start.TabIndex = 10;
            this.Start.Tag = 0;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = true;
            // 
            // Buildlabel
            // 
            this.Buildlabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Buildlabel.AutoSize = true;
            this.Buildlabel.Location = new System.Drawing.Point(854, 576);
            this.Buildlabel.Name = "Buildlabel";
            this.Buildlabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Buildlabel.Size = new System.Drawing.Size(44, 13);
            this.Buildlabel.TabIndex = 5;
            this.Buildlabel.Text = "Сборка";
            this.Buildlabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // linkLabel3
            // 
            this.linkLabel3.ActiveLinkColor = System.Drawing.Color.RosyBrown;
            this.linkLabel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel3.DisabledLinkColor = System.Drawing.Color.RosyBrown;
            this.linkLabel3.Font = new System.Drawing.Font("Impact", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.linkLabel3.LinkColor = System.Drawing.Color.RosyBrown;
            this.linkLabel3.Location = new System.Drawing.Point(328, 546);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(448, 34);
            this.linkLabel3.TabIndex = 4;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Модификация сервера Quintessence";
            this.linkLabel3.VisitedLinkColor = System.Drawing.Color.RosyBrown;
            // 
            // Versionlabel
            // 
            this.Versionlabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Versionlabel.AutoSize = true;
            this.Versionlabel.Location = new System.Drawing.Point(854, 563);
            this.Versionlabel.Name = "Versionlabel";
            this.Versionlabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Versionlabel.Size = new System.Drawing.Size(44, 13);
            this.Versionlabel.TabIndex = 1;
            this.Versionlabel.Text = "Версия";
            this.Versionlabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Animation
            // 
            this.Animation.Controls.Add(this.controlAnimations);
            this.Animation.Location = new System.Drawing.Point(4, 22);
            this.Animation.Margin = new System.Windows.Forms.Padding(0);
            this.Animation.Name = "Animation";
            this.Animation.Size = new System.Drawing.Size(901, 591);
            this.Animation.TabIndex = 0;
            this.Animation.Tag = 1;
            this.Animation.Text = "Animations";
            this.Animation.UseVisualStyleBackColor = true;
            // 
            // controlAnimations
            // 
            this.controlAnimations.BackColor = System.Drawing.SystemColors.Control;
            this.controlAnimations.Cursor = System.Windows.Forms.Cursors.Default;
            this.controlAnimations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlAnimations.Location = new System.Drawing.Point(0, 0);
            this.controlAnimations.Margin = new System.Windows.Forms.Padding(0);
            this.controlAnimations.Name = "controlAnimations";
            this.controlAnimations.Size = new System.Drawing.Size(901, 591);
            this.controlAnimations.TabIndex = 0;
            // 
            // AnimData
            // 
            this.AnimData.Controls.Add(this.controlAnimdata);
            this.AnimData.Location = new System.Drawing.Point(4, 22);
            this.AnimData.Margin = new System.Windows.Forms.Padding(0);
            this.AnimData.Name = "AnimData";
            this.AnimData.Size = new System.Drawing.Size(901, 591);
            this.AnimData.TabIndex = 18;
            this.AnimData.Tag = 2;
            this.AnimData.Text = "AnimData";
            this.AnimData.UseVisualStyleBackColor = true;
            // 
            // controlAnimdata
            // 
            this.controlAnimdata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlAnimdata.Location = new System.Drawing.Point(0, 0);
            this.controlAnimdata.Name = "controlAnimdata";
            this.controlAnimdata.Size = new System.Drawing.Size(901, 591);
            this.controlAnimdata.TabIndex = 0;
            // 
            // Items
            // 
            this.Items.Controls.Add(this.controlItemShow);
            this.Items.Location = new System.Drawing.Point(4, 22);
            this.Items.Margin = new System.Windows.Forms.Padding(0);
            this.Items.Name = "Items";
            this.Items.Size = new System.Drawing.Size(901, 591);
            this.Items.TabIndex = 2;
            this.Items.Tag = 3;
            this.Items.Text = "Items";
            this.Items.UseVisualStyleBackColor = true;
            // 
            // controlItemShow
            // 
            this.controlItemShow.Cursor = System.Windows.Forms.Cursors.Default;
            this.controlItemShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlItemShow.Location = new System.Drawing.Point(0, 0);
            this.controlItemShow.Name = "controlItemShow";
            this.controlItemShow.Size = new System.Drawing.Size(901, 591);
            this.controlItemShow.TabIndex = 0;
            // 
            // TileDatas
            // 
            this.TileDatas.Controls.Add(this.controlTileData);
            this.TileDatas.Location = new System.Drawing.Point(4, 22);
            this.TileDatas.Margin = new System.Windows.Forms.Padding(0);
            this.TileDatas.Name = "TileDatas";
            this.TileDatas.Size = new System.Drawing.Size(901, 591);
            this.TileDatas.TabIndex = 16;
            this.TileDatas.Tag = 4;
            this.TileDatas.Text = "TileData";
            this.TileDatas.UseVisualStyleBackColor = true;
            // 
            // controlTileData
            // 
            this.controlTileData.Cursor = System.Windows.Forms.Cursors.Default;
            this.controlTileData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlTileData.Location = new System.Drawing.Point(0, 0);
            this.controlTileData.MinimumSize = new System.Drawing.Size(620, 600);
            this.controlTileData.Name = "controlTileData";
            this.controlTileData.Size = new System.Drawing.Size(901, 600);
            this.controlTileData.TabIndex = 0;
            // 
            // LandTiles
            // 
            this.LandTiles.Controls.Add(this.controlLandTiles);
            this.LandTiles.Location = new System.Drawing.Point(4, 22);
            this.LandTiles.Margin = new System.Windows.Forms.Padding(0);
            this.LandTiles.Name = "LandTiles";
            this.LandTiles.Size = new System.Drawing.Size(901, 591);
            this.LandTiles.TabIndex = 3;
            this.LandTiles.Tag = 5;
            this.LandTiles.Text = "LandTiles";
            this.LandTiles.UseVisualStyleBackColor = true;
            // 
            // controlLandTiles
            // 
            this.controlLandTiles.Cursor = System.Windows.Forms.Cursors.Default;
            this.controlLandTiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlLandTiles.Location = new System.Drawing.Point(0, 0);
            this.controlLandTiles.Name = "controlLandTiles";
            this.controlLandTiles.Size = new System.Drawing.Size(901, 591);
            this.controlLandTiles.TabIndex = 0;
            // 
            // Texture
            // 
            this.Texture.Controls.Add(this.controlTexture);
            this.Texture.Location = new System.Drawing.Point(4, 22);
            this.Texture.Margin = new System.Windows.Forms.Padding(0);
            this.Texture.Name = "Texture";
            this.Texture.Size = new System.Drawing.Size(901, 591);
            this.Texture.TabIndex = 11;
            this.Texture.Tag = 6;
            this.Texture.Text = "Texture";
            this.Texture.UseVisualStyleBackColor = true;
            // 
            // controlTexture
            // 
            this.controlTexture.Cursor = System.Windows.Forms.Cursors.Default;
            this.controlTexture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlTexture.Location = new System.Drawing.Point(0, 0);
            this.controlTexture.Name = "controlTexture";
            this.controlTexture.Size = new System.Drawing.Size(901, 591);
            this.controlTexture.TabIndex = 0;
            // 
            // Multis
            // 
            this.Multis.Controls.Add(this.controlMulti);
            this.Multis.Location = new System.Drawing.Point(4, 22);
            this.Multis.Margin = new System.Windows.Forms.Padding(0);
            this.Multis.Name = "Multis";
            this.Multis.Size = new System.Drawing.Size(901, 591);
            this.Multis.TabIndex = 1;
            this.Multis.Tag = 7;
            this.Multis.Text = "Multis";
            this.Multis.UseVisualStyleBackColor = true;
            // 
            // controlMulti
            // 
            this.controlMulti.Cursor = System.Windows.Forms.Cursors.Default;
            this.controlMulti.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlMulti.Location = new System.Drawing.Point(0, 0);
            this.controlMulti.Name = "controlMulti";
            this.controlMulti.Size = new System.Drawing.Size(901, 591);
            this.controlMulti.TabIndex = 0;
            // 
            // Gumps
            // 
            this.Gumps.Controls.Add(this.controlGumps);
            this.Gumps.Location = new System.Drawing.Point(4, 22);
            this.Gumps.Margin = new System.Windows.Forms.Padding(0);
            this.Gumps.Name = "Gumps";
            this.Gumps.Size = new System.Drawing.Size(901, 591);
            this.Gumps.TabIndex = 4;
            this.Gumps.Tag = 8;
            this.Gumps.Text = "Gumps";
            this.Gumps.UseVisualStyleBackColor = true;
            // 
            // controlGumps
            // 
            this.controlGumps.Cursor = System.Windows.Forms.Cursors.Default;
            this.controlGumps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlGumps.Location = new System.Drawing.Point(0, 0);
            this.controlGumps.Name = "controlGumps";
            this.controlGumps.Size = new System.Drawing.Size(901, 591);
            this.controlGumps.TabIndex = 0;
            // 
            // Sounds
            // 
            this.Sounds.Controls.Add(this.controlSound);
            this.Sounds.Location = new System.Drawing.Point(4, 22);
            this.Sounds.Margin = new System.Windows.Forms.Padding(0);
            this.Sounds.Name = "Sounds";
            this.Sounds.Size = new System.Drawing.Size(901, 591);
            this.Sounds.TabIndex = 5;
            this.Sounds.Tag = 9;
            this.Sounds.Text = "Sounds";
            this.Sounds.UseVisualStyleBackColor = true;
            // 
            // controlSound
            // 
            this.controlSound.Cursor = System.Windows.Forms.Cursors.Default;
            this.controlSound.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlSound.Location = new System.Drawing.Point(0, 0);
            this.controlSound.Name = "controlSound";
            this.controlSound.Size = new System.Drawing.Size(901, 591);
            this.controlSound.TabIndex = 0;
            // 
            // Light
            // 
            this.Light.Controls.Add(this.controlLight);
            this.Light.Location = new System.Drawing.Point(4, 22);
            this.Light.Margin = new System.Windows.Forms.Padding(0);
            this.Light.Name = "Light";
            this.Light.Size = new System.Drawing.Size(901, 591);
            this.Light.TabIndex = 12;
            this.Light.Tag = 10;
            this.Light.Text = "Light";
            this.Light.UseVisualStyleBackColor = true;
            // 
            // controlLight
            // 
            this.controlLight.Cursor = System.Windows.Forms.Cursors.Default;
            this.controlLight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlLight.Location = new System.Drawing.Point(0, 0);
            this.controlLight.Name = "controlLight";
            this.controlLight.Size = new System.Drawing.Size(192, 74);
            this.controlLight.TabIndex = 0;
            // 
            // Hue
            // 
            this.Hue.Controls.Add(this.controlHue);
            this.Hue.Location = new System.Drawing.Point(4, 22);
            this.Hue.Margin = new System.Windows.Forms.Padding(0);
            this.Hue.Name = "Hue";
            this.Hue.Size = new System.Drawing.Size(901, 591);
            this.Hue.TabIndex = 6;
            this.Hue.Tag = 11;
            this.Hue.Text = "Hue";
            this.Hue.UseVisualStyleBackColor = true;
            // 
            // controlHue
            // 
            this.controlHue.Cursor = System.Windows.Forms.Cursors.Default;
            this.controlHue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlHue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.controlHue.Location = new System.Drawing.Point(0, 0);
            this.controlHue.Name = "controlHue";
            this.controlHue.Padding = new System.Windows.Forms.Padding(1);
            this.controlHue.Size = new System.Drawing.Size(901, 591);
            this.controlHue.TabIndex = 0;
            // 
            // fonts
            // 
            this.fonts.Controls.Add(this.controlfonts);
            this.fonts.Location = new System.Drawing.Point(4, 22);
            this.fonts.Margin = new System.Windows.Forms.Padding(0);
            this.fonts.Name = "fonts";
            this.fonts.Size = new System.Drawing.Size(901, 591);
            this.fonts.TabIndex = 7;
            this.fonts.Tag = 12;
            this.fonts.Text = "Fonts";
            this.fonts.UseVisualStyleBackColor = true;
            // 
            // controlfonts
            // 
            this.controlfonts.Cursor = System.Windows.Forms.Cursors.Default;
            this.controlfonts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlfonts.Location = new System.Drawing.Point(0, 0);
            this.controlfonts.Name = "controlfonts";
            this.controlfonts.Size = new System.Drawing.Size(901, 591);
            this.controlfonts.TabIndex = 0;
            // 
            // Cliloc
            // 
            this.Cliloc.Controls.Add(this.controlCliloc);
            this.Cliloc.Location = new System.Drawing.Point(4, 22);
            this.Cliloc.Margin = new System.Windows.Forms.Padding(0);
            this.Cliloc.Name = "Cliloc";
            this.Cliloc.Size = new System.Drawing.Size(901, 591);
            this.Cliloc.TabIndex = 8;
            this.Cliloc.Tag = 13;
            this.Cliloc.Text = "CliLoc";
            this.Cliloc.UseVisualStyleBackColor = true;
            // 
            // controlCliloc
            // 
            this.controlCliloc.Cursor = System.Windows.Forms.Cursors.Default;
            this.controlCliloc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlCliloc.Location = new System.Drawing.Point(0, 0);
            this.controlCliloc.Name = "controlCliloc";
            this.controlCliloc.Size = new System.Drawing.Size(901, 591);
            this.controlCliloc.TabIndex = 0;
            // 
            // speech
            // 
            this.speech.Controls.Add(this.controlspeech);
            this.speech.Location = new System.Drawing.Point(4, 22);
            this.speech.Margin = new System.Windows.Forms.Padding(0);
            this.speech.Name = "speech";
            this.speech.Size = new System.Drawing.Size(901, 591);
            this.speech.TabIndex = 17;
            this.speech.Tag = 14;
            this.speech.Text = "Speech";
            this.speech.UseVisualStyleBackColor = true;
            // 
            // controlspeech
            // 
            this.controlspeech.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlspeech.Location = new System.Drawing.Point(0, 0);
            this.controlspeech.Name = "controlspeech";
            this.controlspeech.Size = new System.Drawing.Size(192, 74);
            this.controlspeech.TabIndex = 0;
            // 
            // Skills
            // 
            this.Skills.Controls.Add(this.controlSkills);
            this.Skills.Location = new System.Drawing.Point(4, 22);
            this.Skills.Margin = new System.Windows.Forms.Padding(0);
            this.Skills.Name = "Skills";
            this.Skills.Size = new System.Drawing.Size(901, 591);
            this.Skills.TabIndex = 15;
            this.Skills.Tag = 15;
            this.Skills.Text = "Skills";
            this.Skills.UseVisualStyleBackColor = true;
            // 
            // controlSkills
            // 
            this.controlSkills.Cursor = System.Windows.Forms.Cursors.Default;
            this.controlSkills.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlSkills.Location = new System.Drawing.Point(0, 0);
            this.controlSkills.Name = "controlSkills";
            this.controlSkills.Size = new System.Drawing.Size(901, 591);
            this.controlSkills.TabIndex = 0;
            // 
            // SkillGrp
            // 
            this.SkillGrp.Controls.Add(this.controlSkillGrp);
            this.SkillGrp.Location = new System.Drawing.Point(4, 22);
            this.SkillGrp.Margin = new System.Windows.Forms.Padding(0);
            this.SkillGrp.Name = "SkillGrp";
            this.SkillGrp.Size = new System.Drawing.Size(901, 591);
            this.SkillGrp.TabIndex = 20;
            this.SkillGrp.Tag = 16;
            this.SkillGrp.Text = "SkillGrp";
            this.SkillGrp.UseVisualStyleBackColor = true;
            // 
            // controlSkillGrp
            // 
            this.controlSkillGrp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlSkillGrp.Location = new System.Drawing.Point(0, 0);
            this.controlSkillGrp.Name = "controlSkillGrp";
            this.controlSkillGrp.Size = new System.Drawing.Size(901, 591);
            this.controlSkillGrp.TabIndex = 0;
            // 
            // map
            // 
            this.map.Controls.Add(this.controlmap);
            this.map.Location = new System.Drawing.Point(4, 22);
            this.map.Margin = new System.Windows.Forms.Padding(0);
            this.map.Name = "map";
            this.map.Size = new System.Drawing.Size(901, 591);
            this.map.TabIndex = 9;
            this.map.Tag = 17;
            this.map.Text = "Map";
            this.map.UseVisualStyleBackColor = true;
            // 
            // controlmap
            // 
            this.controlmap.Cursor = System.Windows.Forms.Cursors.Default;
            this.controlmap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlmap.Location = new System.Drawing.Point(0, 0);
            this.controlmap.Margin = new System.Windows.Forms.Padding(0);
            this.controlmap.Name = "controlmap";
            this.controlmap.Size = new System.Drawing.Size(901, 591);
            this.controlmap.TabIndex = 0;
            // 
            // RadarCol
            // 
            this.RadarCol.Controls.Add(this.controlRadarCol);
            this.RadarCol.Location = new System.Drawing.Point(4, 22);
            this.RadarCol.Margin = new System.Windows.Forms.Padding(0);
            this.RadarCol.Name = "RadarCol";
            this.RadarCol.Size = new System.Drawing.Size(901, 591);
            this.RadarCol.TabIndex = 19;
            this.RadarCol.Tag = 18;
            this.RadarCol.Text = "RadarColor";
            this.RadarCol.UseVisualStyleBackColor = true;
            // 
            // controlRadarCol
            // 
            this.controlRadarCol.Cursor = System.Windows.Forms.Cursors.Default;
            this.controlRadarCol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlRadarCol.Location = new System.Drawing.Point(0, 0);
            this.controlRadarCol.Name = "controlRadarCol";
            this.controlRadarCol.Size = new System.Drawing.Size(192, 74);
            this.controlRadarCol.TabIndex = 0;
            // 
            // multimap
            // 
            this.multimap.Controls.Add(this.controlMultimap);
            this.multimap.Location = new System.Drawing.Point(4, 22);
            this.multimap.Margin = new System.Windows.Forms.Padding(0);
            this.multimap.Name = "multimap";
            this.multimap.Size = new System.Drawing.Size(901, 591);
            this.multimap.TabIndex = 14;
            this.multimap.Tag = 19;
            this.multimap.Text = "MultiMap";
            this.multimap.UseVisualStyleBackColor = true;
            // 
            // controlMultimap
            // 
            this.controlMultimap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlMultimap.Location = new System.Drawing.Point(0, 0);
            this.controlMultimap.Name = "controlMultimap";
            this.controlMultimap.Size = new System.Drawing.Size(901, 591);
            this.controlMultimap.TabIndex = 0;
            // 
            // Dress
            // 
            this.Dress.Controls.Add(this.controldress);
            this.Dress.Location = new System.Drawing.Point(4, 22);
            this.Dress.Margin = new System.Windows.Forms.Padding(0);
            this.Dress.Name = "Dress";
            this.Dress.Size = new System.Drawing.Size(901, 591);
            this.Dress.TabIndex = 13;
            this.Dress.Tag = 20;
            this.Dress.Text = "Dress";
            this.Dress.UseVisualStyleBackColor = true;
            // 
            // controldress
            // 
            this.controldress.Cursor = System.Windows.Forms.Cursors.Default;
            this.controldress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controldress.Location = new System.Drawing.Point(0, 0);
            this.controldress.Name = "controldress";
            this.controldress.Size = new System.Drawing.Size(901, 591);
            this.controldress.TabIndex = 0;
            // 
            // cmd
            // 
            this.cmd.Controls.Add(this.serverInteractive1);
            this.cmd.Location = new System.Drawing.Point(4, 22);
            this.cmd.Margin = new System.Windows.Forms.Padding(0);
            this.cmd.Name = "cmd";
            this.cmd.Size = new System.Drawing.Size(901, 591);
            this.cmd.TabIndex = 21;
            this.cmd.Tag = "21";
            this.cmd.Text = "Cmd";
            this.cmd.UseVisualStyleBackColor = true;
            // 
            // serverInteractive1
            // 
            this.serverInteractive1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverInteractive1.Location = new System.Drawing.Point(0, 0);
            this.serverInteractive1.Name = "serverInteractive1";
            this.serverInteractive1.Size = new System.Drawing.Size(901, 591);
            this.serverInteractive1.TabIndex = 0;
            // 
            // CentrEd
            // 
            this.CentrEd.Controls.Add(this.controlCentrEd);
            this.CentrEd.Location = new System.Drawing.Point(4, 22);
            this.CentrEd.Margin = new System.Windows.Forms.Padding(0);
            this.CentrEd.Name = "CentrEd";
            this.CentrEd.Size = new System.Drawing.Size(901, 591);
            this.CentrEd.TabIndex = 22;
            this.CentrEd.Tag = "22";
            this.CentrEd.Text = "CentrEd+";
            this.CentrEd.UseVisualStyleBackColor = true;
            // 
            // controlCentrEd
            // 
            this.controlCentrEd.BackColor = System.Drawing.SystemColors.Control;
            this.controlCentrEd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlCentrEd.Location = new System.Drawing.Point(0, 0);
            this.controlCentrEd.Name = "controlCentrEd";
            this.controlCentrEd.Size = new System.Drawing.Size(192, 74);
            this.controlCentrEd.TabIndex = 0;
            // 
            // WebBrowser
            // 
            this.WebBrowser.Controls.Add(this.controlWebBrowser);
            this.WebBrowser.Location = new System.Drawing.Point(4, 22);
            this.WebBrowser.Margin = new System.Windows.Forms.Padding(0);
            this.WebBrowser.Name = "WebBrowser";
            this.WebBrowser.Size = new System.Drawing.Size(901, 591);
            this.WebBrowser.TabIndex = 23;
            this.WebBrowser.Tag = "23";
            this.WebBrowser.Text = "Browser";
            this.WebBrowser.UseVisualStyleBackColor = true;
            // 
            // controlWebBrowser
            // 
            this.controlWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.controlWebBrowser.Name = "controlWebBrowser";
            this.controlWebBrowser.Size = new System.Drawing.Size(192, 74);
            this.controlWebBrowser.TabIndex = 0;
            // 
            // Regions
            // 
            this.Regions.Controls.Add(this.regionEditor1);
            this.Regions.Location = new System.Drawing.Point(4, 22);
            this.Regions.Margin = new System.Windows.Forms.Padding(0);
            this.Regions.Name = "Regions";
            this.Regions.Size = new System.Drawing.Size(901, 591);
            this.Regions.TabIndex = 24;
            this.Regions.Tag = "24";
            this.Regions.Text = "Regions";
            this.Regions.UseVisualStyleBackColor = true;
            // 
            // regionEditor1
            // 
            this.regionEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.regionEditor1.Location = new System.Drawing.Point(0, 0);
            this.regionEditor1.Name = "regionEditor1";
            this.regionEditor1.Size = new System.Drawing.Size(901, 591);
            this.regionEditor1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SettingsMenu,
            this.toolStripButton1,
            this.toolStripDropDownButtonView,
            this.ExternToolsDropDown,
            this.toolStripDropDownButtonPlugins,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(909, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // SettingsMenu
            // 
            this.SettingsMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SettingsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AlwaysOnTopMenuitem,
            this.optionsToolStripMenuItem,
            this.pathSettingsMenuItem,
            this.toolStripSeparator2,
            this.restartNeededMenuItem});
            this.SettingsMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SettingsMenu.Name = "SettingsMenu";
            this.SettingsMenu.Size = new System.Drawing.Size(79, 22);
            this.SettingsMenu.Text = "Настройка";
            // 
            // AlwaysOnTopMenuitem
            // 
            this.AlwaysOnTopMenuitem.CheckOnClick = true;
            this.AlwaysOnTopMenuitem.Name = "AlwaysOnTopMenuitem";
            this.AlwaysOnTopMenuitem.Size = new System.Drawing.Size(195, 22);
            this.AlwaysOnTopMenuitem.Text = "Поверх окон";
            this.AlwaysOnTopMenuitem.Click += new System.EventHandler(this.onClickAlwaysTop);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.optionsToolStripMenuItem.Text = "Конфигурация..";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.OnClickOptions);
            // 
            // pathSettingsMenuItem
            // 
            this.pathSettingsMenuItem.Name = "pathSettingsMenuItem";
            this.pathSettingsMenuItem.Size = new System.Drawing.Size(195, 22);
            this.pathSettingsMenuItem.Text = "Настройка путей..";
            this.pathSettingsMenuItem.Click += new System.EventHandler(this.click_path);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(192, 6);
            // 
            // restartNeededMenuItem
            // 
            this.restartNeededMenuItem.ForeColor = System.Drawing.Color.DarkRed;
            this.restartNeededMenuItem.Name = "restartNeededMenuItem";
            this.restartNeededMenuItem.Size = new System.Drawing.Size(195, 22);
            this.restartNeededMenuItem.Text = "Перезагрузить файлы";
            this.restartNeededMenuItem.Click += new System.EventHandler(this.Restart);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(92, 22);
            this.toolStripButton1.Text = "О программе..";
            this.toolStripButton1.Click += new System.EventHandler(this.OnClickAbout);
            // 
            // toolStripDropDownButtonView
            // 
            this.toolStripDropDownButtonView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButtonView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToggleViewStart,
            this.ToggleViewAnimations,
            this.ToggleViewAnimData,
            this.ToggleViewItems,
            this.ToggleViewTileData,
            this.ToggleViewLandTiles,
            this.ToggleViewTexture,
            this.ToggleViewMulti,
            this.ToggleViewGumps,
            this.ToggleViewSounds,
            this.ToggleViewLight,
            this.ToggleViewHue,
            this.ToggleViewFonts,
            this.ToggleViewCliloc,
            this.ToggleViewSpeech,
            this.ToggleViewSkills,
            this.ToggleViewSkillGrp,
            this.ToggleViewMap,
            this.ToggleViewRadarColor,
            this.ToggleViewMultiMap,
            this.ToggleViewDress});
            this.toolStripDropDownButtonView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonView.Name = "toolStripDropDownButtonView";
            this.toolStripDropDownButtonView.Size = new System.Drawing.Size(65, 22);
            this.toolStripDropDownButtonView.Text = "Вкладки";
            // 
            // ToggleViewStart
            // 
            this.ToggleViewStart.Checked = true;
            this.ToggleViewStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewStart.Name = "ToggleViewStart";
            this.ToggleViewStart.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewStart.Tag = 0;
            this.ToggleViewStart.Text = "Start";
            this.ToggleViewStart.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewAnimations
            // 
            this.ToggleViewAnimations.Checked = true;
            this.ToggleViewAnimations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewAnimations.Name = "ToggleViewAnimations";
            this.ToggleViewAnimations.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewAnimations.Tag = 1;
            this.ToggleViewAnimations.Text = "Animations";
            this.ToggleViewAnimations.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewAnimData
            // 
            this.ToggleViewAnimData.Checked = true;
            this.ToggleViewAnimData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewAnimData.Name = "ToggleViewAnimData";
            this.ToggleViewAnimData.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewAnimData.Tag = 2;
            this.ToggleViewAnimData.Text = "AnimData";
            this.ToggleViewAnimData.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewItems
            // 
            this.ToggleViewItems.Checked = true;
            this.ToggleViewItems.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewItems.Name = "ToggleViewItems";
            this.ToggleViewItems.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewItems.Tag = 3;
            this.ToggleViewItems.Text = "Items";
            this.ToggleViewItems.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewTileData
            // 
            this.ToggleViewTileData.Checked = true;
            this.ToggleViewTileData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewTileData.Name = "ToggleViewTileData";
            this.ToggleViewTileData.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewTileData.Tag = 4;
            this.ToggleViewTileData.Text = "TileData";
            this.ToggleViewTileData.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewLandTiles
            // 
            this.ToggleViewLandTiles.Checked = true;
            this.ToggleViewLandTiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewLandTiles.Name = "ToggleViewLandTiles";
            this.ToggleViewLandTiles.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewLandTiles.Tag = 5;
            this.ToggleViewLandTiles.Text = "LandTiles";
            this.ToggleViewLandTiles.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewTexture
            // 
            this.ToggleViewTexture.Checked = true;
            this.ToggleViewTexture.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewTexture.Name = "ToggleViewTexture";
            this.ToggleViewTexture.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewTexture.Tag = 6;
            this.ToggleViewTexture.Text = "Texture";
            this.ToggleViewTexture.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewMulti
            // 
            this.ToggleViewMulti.Checked = true;
            this.ToggleViewMulti.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewMulti.Name = "ToggleViewMulti";
            this.ToggleViewMulti.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewMulti.Tag = 7;
            this.ToggleViewMulti.Text = "Multi";
            this.ToggleViewMulti.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewGumps
            // 
            this.ToggleViewGumps.Checked = true;
            this.ToggleViewGumps.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewGumps.Name = "ToggleViewGumps";
            this.ToggleViewGumps.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewGumps.Tag = 8;
            this.ToggleViewGumps.Text = "Gumps";
            this.ToggleViewGumps.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewSounds
            // 
            this.ToggleViewSounds.Checked = true;
            this.ToggleViewSounds.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewSounds.Name = "ToggleViewSounds";
            this.ToggleViewSounds.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewSounds.Tag = 9;
            this.ToggleViewSounds.Text = "Sounds";
            this.ToggleViewSounds.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewLight
            // 
            this.ToggleViewLight.Checked = true;
            this.ToggleViewLight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewLight.Name = "ToggleViewLight";
            this.ToggleViewLight.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewLight.Tag = 10;
            this.ToggleViewLight.Text = "Light";
            this.ToggleViewLight.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewHue
            // 
            this.ToggleViewHue.Checked = true;
            this.ToggleViewHue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewHue.Name = "ToggleViewHue";
            this.ToggleViewHue.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewHue.Tag = 11;
            this.ToggleViewHue.Text = "Hue";
            this.ToggleViewHue.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewFonts
            // 
            this.ToggleViewFonts.Checked = true;
            this.ToggleViewFonts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewFonts.Name = "ToggleViewFonts";
            this.ToggleViewFonts.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewFonts.Tag = 12;
            this.ToggleViewFonts.Text = "Fonts";
            this.ToggleViewFonts.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewCliloc
            // 
            this.ToggleViewCliloc.Checked = true;
            this.ToggleViewCliloc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewCliloc.Name = "ToggleViewCliloc";
            this.ToggleViewCliloc.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewCliloc.Tag = 13;
            this.ToggleViewCliloc.Text = "Cliloc";
            this.ToggleViewCliloc.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewSpeech
            // 
            this.ToggleViewSpeech.Checked = true;
            this.ToggleViewSpeech.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewSpeech.Name = "ToggleViewSpeech";
            this.ToggleViewSpeech.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewSpeech.Tag = 14;
            this.ToggleViewSpeech.Text = "Speech";
            this.ToggleViewSpeech.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewSkills
            // 
            this.ToggleViewSkills.Checked = true;
            this.ToggleViewSkills.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewSkills.Name = "ToggleViewSkills";
            this.ToggleViewSkills.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewSkills.Tag = 15;
            this.ToggleViewSkills.Text = "Skills";
            this.ToggleViewSkills.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewSkillGrp
            // 
            this.ToggleViewSkillGrp.Checked = true;
            this.ToggleViewSkillGrp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewSkillGrp.Name = "ToggleViewSkillGrp";
            this.ToggleViewSkillGrp.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewSkillGrp.Tag = 16;
            this.ToggleViewSkillGrp.Text = "SkillGrp";
            this.ToggleViewSkillGrp.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewMap
            // 
            this.ToggleViewMap.Checked = true;
            this.ToggleViewMap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewMap.Name = "ToggleViewMap";
            this.ToggleViewMap.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewMap.Tag = 17;
            this.ToggleViewMap.Text = "Map";
            this.ToggleViewMap.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewRadarColor
            // 
            this.ToggleViewRadarColor.Checked = true;
            this.ToggleViewRadarColor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewRadarColor.Name = "ToggleViewRadarColor";
            this.ToggleViewRadarColor.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewRadarColor.Tag = 18;
            this.ToggleViewRadarColor.Text = "RadarColor";
            this.ToggleViewRadarColor.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewMultiMap
            // 
            this.ToggleViewMultiMap.Checked = true;
            this.ToggleViewMultiMap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewMultiMap.Name = "ToggleViewMultiMap";
            this.ToggleViewMultiMap.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewMultiMap.Tag = 19;
            this.ToggleViewMultiMap.Text = "MultiMap";
            this.ToggleViewMultiMap.Click += new System.EventHandler(this.ToggleView);
            // 
            // ToggleViewDress
            // 
            this.ToggleViewDress.Checked = true;
            this.ToggleViewDress.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleViewDress.Name = "ToggleViewDress";
            this.ToggleViewDress.Size = new System.Drawing.Size(135, 22);
            this.ToggleViewDress.Tag = 20;
            this.ToggleViewDress.Text = "Dress";
            this.ToggleViewDress.Click += new System.EventHandler(this.ToggleView);
            // 
            // ExternToolsDropDown
            // 
            this.ExternToolsDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ExternToolsDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageToolStripMenuItem});
            this.ExternToolsDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExternToolsDropDown.Name = "ExternToolsDropDown";
            this.ExternToolsDropDown.Size = new System.Drawing.Size(148, 22);
            this.ExternToolsDropDown.Text = "Внешние инструменты";
            // 
            // manageToolStripMenuItem
            // 
            this.manageToolStripMenuItem.Name = "manageToolStripMenuItem";
            this.manageToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.manageToolStripMenuItem.Text = "Управление..";
            this.manageToolStripMenuItem.Click += new System.EventHandler(this.onClickToolManage);
            // 
            // toolStripDropDownButtonPlugins
            // 
            this.toolStripDropDownButtonPlugins.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButtonPlugins.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageToolStripMenuItem1,
            this.toolStripSeparator1});
            this.toolStripDropDownButtonPlugins.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonPlugins.Name = "toolStripDropDownButtonPlugins";
            this.toolStripDropDownButtonPlugins.Size = new System.Drawing.Size(86, 22);
            this.toolStripDropDownButtonPlugins.Text = "Надстройки";
            // 
            // manageToolStripMenuItem1
            // 
            this.manageToolStripMenuItem1.Name = "manageToolStripMenuItem1";
            this.manageToolStripMenuItem1.Size = new System.Drawing.Size(146, 22);
            this.manageToolStripMenuItem1.Text = "Управление..";
            this.manageToolStripMenuItem1.Click += new System.EventHandler(this.onClickManagePlugins);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(57, 22);
            this.toolStripButton2.Text = "Справка";
            this.toolStripButton2.Click += new System.EventHandler(this.OnClickHelp);
            // 
            // UoFiddler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 642);
            this.Controls.Add(this.TabPanel);
            this.Controls.Add(this.toolStrip1);
            this.Name = "UoFiddler";
            this.Text = "UOFiddler+";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.TabPanel.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.Start.ResumeLayout(false);
            this.Start.PerformLayout();
            this.Animation.ResumeLayout(false);
            this.AnimData.ResumeLayout(false);
            this.Items.ResumeLayout(false);
            this.TileDatas.ResumeLayout(false);
            this.LandTiles.ResumeLayout(false);
            this.Texture.ResumeLayout(false);
            this.Multis.ResumeLayout(false);
            this.Gumps.ResumeLayout(false);
            this.Sounds.ResumeLayout(false);
            this.Light.ResumeLayout(false);
            this.Hue.ResumeLayout(false);
            this.fonts.ResumeLayout(false);
            this.Cliloc.ResumeLayout(false);
            this.speech.ResumeLayout(false);
            this.Skills.ResumeLayout(false);
            this.SkillGrp.ResumeLayout(false);
            this.map.ResumeLayout(false);
            this.RadarCol.ResumeLayout(false);
            this.multimap.ResumeLayout(false);
            this.Dress.ResumeLayout(false);
            this.cmd.ResumeLayout(false);
            this.CentrEd.ResumeLayout(false);
            this.WebBrowser.ResumeLayout(false);
            this.Regions.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl TabPanel;
        private System.Windows.Forms.TabPage Animation;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TabPage Items;
        private FiddlerControls.ItemShow controlItemShow;
        private FiddlerControls.Animationlist controlAnimations;
        private System.Windows.Forms.TabPage LandTiles;
        private FiddlerControls.LandTiles controlLandTiles;
        private System.Windows.Forms.TabPage Gumps;
        private FiddlerControls.Gump controlGumps;
        private System.Windows.Forms.TabPage Sounds;
        private FiddlerControls.Sounds controlSound;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TabPage Multis;
        private FiddlerControls.Multis controlMulti;
        private System.Windows.Forms.TabPage Hue;
        private FiddlerControls.Hues controlHue;
        private System.Windows.Forms.TabPage fonts;
        private FiddlerControls.Fonts controlfonts;
        private System.Windows.Forms.TabPage Cliloc;
        private FiddlerControls.Cliloc controlCliloc;
        private System.Windows.Forms.TabPage map;
        private FiddlerControls.Map controlmap;
        private System.Windows.Forms.TabPage Start;
        private System.Windows.Forms.TabPage Texture;
        private FiddlerControls.Texture controlTexture;
        private System.Windows.Forms.TabPage Light;
        private FiddlerControls.Light controlLight;
        private System.Windows.Forms.Label Versionlabel;
        private System.Windows.Forms.TabPage Dress;
        private FiddlerControls.Dress controldress;
        private System.Windows.Forms.ToolStripDropDownButton SettingsMenu;
        private System.Windows.Forms.ToolStripMenuItem AlwaysOnTopMenuitem;
        private System.Windows.Forms.ToolStripMenuItem pathSettingsMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem restartNeededMenuItem;
        private System.Windows.Forms.TabPage multimap;
        private FiddlerControls.MultiMap controlMultimap;
        private System.Windows.Forms.TabPage Skills;
        private FiddlerControls.Skills controlSkills;
        private System.Windows.Forms.TabPage TileDatas;
        private FiddlerControls.TileDatas controlTileData;
        private System.Windows.Forms.TabPage speech;
        private FiddlerControls.Speech controlspeech;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripDropDownButton ExternToolsDropDown;
        private System.Windows.Forms.ToolStripMenuItem manageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem unDockToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonPlugins;
        private System.Windows.Forms.ToolStripMenuItem manageToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TabPage AnimData;
        private FiddlerControls.AnimData controlAnimdata;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.TabPage RadarCol;
        private FiddlerControls.RadarColor controlRadarCol;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonView;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewStart;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewMulti;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewAnimations;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewItems;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewLandTiles;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewTexture;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewGumps;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewSounds;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewHue;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewFonts;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewCliloc;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewMap;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewLight;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewSpeech;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewSkills;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewAnimData;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewMultiMap;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewDress;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewTileData;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewRadarColor;
        private System.Windows.Forms.TabPage SkillGrp;
        private FiddlerControls.SkillGrp controlSkillGrp;
        private System.Windows.Forms.ToolStripMenuItem ToggleViewSkillGrp;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.Label Buildlabel;
        private System.Windows.Forms.TabPage cmd;
        private global::UoFiddler.Telnet.ServerInteractive serverInteractive1;
        private System.Windows.Forms.TabPage WebBrowser;
        private FiddlerControls.WebBrowser controlWebBrowser;
        private System.Windows.Forms.TabPage Regions;
        private FiddlerControls.RegionEditor.RegionEditor regionEditor1;
        private System.Windows.Forms.TabPage CentrEd;
        private FiddlerControls.CentrEditor.CentrEd controlCentrEd;
    }
}

