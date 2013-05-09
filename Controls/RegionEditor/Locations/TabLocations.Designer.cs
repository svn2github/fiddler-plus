namespace FiddlerControls.RegionEditor.Locations
{
    partial class TabLocations
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            FiddlerControls.RegionEditor.BoxCommon.MulManager mulManager1 = new FiddlerControls.RegionEditor.BoxCommon.MulManager();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.locationsTreeView = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mapComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.zNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.yNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.xNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.l_menuStrip = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.картаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.map0DungeonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.map1SosariaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.map2IlshinarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.map3MalasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.map4TokunoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.map5TerMurToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.colorButton = new System.Windows.Forms.Button();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.масштабToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scale0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scale1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scale2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scale3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scale4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scale01ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scale02ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scale03ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapViewer = new FiddlerControls.RegionEditor.MapViewer.MapViewer();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xNumericUpDown)).BeginInit();
            this.l_menuStrip.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.mapViewer);
            this.splitContainer1.Panel2.Controls.Add(this.l_menuStrip);
            this.splitContainer1.Size = new System.Drawing.Size(739, 408);
            this.splitContainer1.SplitterDistance = 214;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.locationsTreeView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel1);
            this.splitContainer2.Size = new System.Drawing.Size(214, 408);
            this.splitContainer2.SplitterDistance = 331;
            this.splitContainer2.TabIndex = 0;
            // 
            // locationsTreeView
            // 
            this.locationsTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.locationsTreeView.ContextMenuStrip = this.contextMenuStrip;
            this.locationsTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.locationsTreeView.FullRowSelect = true;
            this.locationsTreeView.HideSelection = false;
            this.locationsTreeView.Location = new System.Drawing.Point(0, 0);
            this.locationsTreeView.Name = "locationsTreeView";
            this.locationsTreeView.Size = new System.Drawing.Size(214, 331);
            this.locationsTreeView.Sorted = true;
            this.locationsTreeView.TabIndex = 2;
            this.locationsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.locationsTreeView_AfterSelect);
            this.locationsTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.locationsTreeView_NodeMouseDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.colorButton);
            this.panel1.Controls.Add(this.mapComboBox);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.zNumericUpDown);
            this.panel1.Controls.Add(this.yNumericUpDown);
            this.panel1.Controls.Add(this.xNumericUpDown);
            this.panel1.Controls.Add(this.nameTextBox);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(214, 73);
            this.panel1.TabIndex = 0;
            // 
            // mapComboBox
            // 
            this.mapComboBox.FormattingEnabled = true;
            this.mapComboBox.Items.AddRange(new object[] {
            "[0] Dungeon",
            "[1] Sosaria",
            "[2] Ilshinar",
            "[3] Malas",
            "[4] Tokuna",
            "[5] TerMur"});
            this.mapComboBox.Location = new System.Drawing.Point(59, 49);
            this.mapComboBox.Name = "mapComboBox";
            this.mapComboBox.Size = new System.Drawing.Size(85, 21);
            this.mapComboBox.TabIndex = 34;
            this.mapComboBox.SelectedIndexChanged += new System.EventHandler(this.l_mapComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(19, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 20);
            this.label4.TabIndex = 33;
            this.label4.Text = "Map";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // zNumericUpDown
            // 
            this.zNumericUpDown.Location = new System.Drawing.Point(166, 26);
            this.zNumericUpDown.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.zNumericUpDown.Minimum = new decimal(new int[] {
            127,
            0,
            0,
            -2147483648});
            this.zNumericUpDown.Name = "zNumericUpDown";
            this.zNumericUpDown.Size = new System.Drawing.Size(45, 20);
            this.zNumericUpDown.TabIndex = 32;
            this.zNumericUpDown.Value = new decimal(new int[] {
            127,
            0,
            0,
            -2147483648});
            this.zNumericUpDown.ValueChanged += new System.EventHandler(this.l_zNumericUpDown_ValueChanged);
            // 
            // yNumericUpDown
            // 
            this.yNumericUpDown.Location = new System.Drawing.Point(96, 26);
            this.yNumericUpDown.Maximum = new decimal(new int[] {
            12288,
            0,
            0,
            0});
            this.yNumericUpDown.Name = "yNumericUpDown";
            this.yNumericUpDown.Size = new System.Drawing.Size(48, 20);
            this.yNumericUpDown.TabIndex = 31;
            this.yNumericUpDown.Value = new decimal(new int[] {
            8192,
            0,
            0,
            0});
            this.yNumericUpDown.ValueChanged += new System.EventHandler(this.l_yNumericUpDown_ValueChanged);
            // 
            // xNumericUpDown
            // 
            this.xNumericUpDown.Location = new System.Drawing.Point(19, 26);
            this.xNumericUpDown.Maximum = new decimal(new int[] {
            12288,
            0,
            0,
            0});
            this.xNumericUpDown.Name = "xNumericUpDown";
            this.xNumericUpDown.Size = new System.Drawing.Size(55, 20);
            this.xNumericUpDown.TabIndex = 30;
            this.xNumericUpDown.Value = new decimal(new int[] {
            12288,
            0,
            0,
            0});
            this.xNumericUpDown.ValueChanged += new System.EventHandler(this.l_xNumericUpDown_ValueChanged);
            // 
            // nameTextBox
            // 
            this.nameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nameTextBox.Location = new System.Drawing.Point(3, 3);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(208, 20);
            this.nameTextBox.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(150, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(10, 23);
            this.label3.TabIndex = 26;
            this.label3.Text = "Z";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(80, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 23);
            this.label2.TabIndex = 25;
            this.label2.Text = "Y";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 23);
            this.label1.TabIndex = 24;
            this.label1.Text = "X";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // l_menuStrip
            // 
            this.l_menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.настройкиToolStripMenuItem});
            this.l_menuStrip.Location = new System.Drawing.Point(0, 0);
            this.l_menuStrip.Name = "l_menuStrip";
            this.l_menuStrip.Size = new System.Drawing.Size(521, 24);
            this.l_menuStrip.TabIndex = 0;
            this.l_menuStrip.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(48, 20);
            this.toolStripMenuItem1.Text = "Файл";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.newToolStripMenuItem.Text = "Новый";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.openToolStripMenuItem.Text = "Открыть";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.saveToolStripMenuItem.Text = "Сохранить";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.saveAsToolStripMenuItem.Text = "Сохранить как...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.картаToolStripMenuItem,
            this.масштабToolStripMenuItem});
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            // 
            // картаToolStripMenuItem
            // 
            this.картаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.map0DungeonToolStripMenuItem,
            this.map1SosariaToolStripMenuItem,
            this.map2IlshinarToolStripMenuItem,
            this.map3MalasToolStripMenuItem,
            this.map4TokunoToolStripMenuItem,
            this.map5TerMurToolStripMenuItem});
            this.картаToolStripMenuItem.Name = "картаToolStripMenuItem";
            this.картаToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.картаToolStripMenuItem.Text = "Карта";
            // 
            // map0DungeonToolStripMenuItem
            // 
            this.map0DungeonToolStripMenuItem.Name = "map0DungeonToolStripMenuItem";
            this.map0DungeonToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.map0DungeonToolStripMenuItem.Tag = "Dungeon";
            this.map0DungeonToolStripMenuItem.Text = "[map0] Подземелья";
            this.map0DungeonToolStripMenuItem.Click += new System.EventHandler(this.mapToolStripMenuItem_Click);
            // 
            // map1SosariaToolStripMenuItem
            // 
            this.map1SosariaToolStripMenuItem.Name = "map1SosariaToolStripMenuItem";
            this.map1SosariaToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.map1SosariaToolStripMenuItem.Tag = "Sosaria";
            this.map1SosariaToolStripMenuItem.Text = "[map1] Сосария";
            this.map1SosariaToolStripMenuItem.Click += new System.EventHandler(this.mapToolStripMenuItem_Click);
            // 
            // map2IlshinarToolStripMenuItem
            // 
            this.map2IlshinarToolStripMenuItem.Name = "map2IlshinarToolStripMenuItem";
            this.map2IlshinarToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.map2IlshinarToolStripMenuItem.Tag = "Ilshenar";
            this.map2IlshinarToolStripMenuItem.Text = "[map2] Илшинар";
            this.map2IlshinarToolStripMenuItem.Click += new System.EventHandler(this.mapToolStripMenuItem_Click);
            // 
            // map3MalasToolStripMenuItem
            // 
            this.map3MalasToolStripMenuItem.Name = "map3MalasToolStripMenuItem";
            this.map3MalasToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.map3MalasToolStripMenuItem.Tag = "Malas";
            this.map3MalasToolStripMenuItem.Text = "[map3] Малас";
            this.map3MalasToolStripMenuItem.Click += new System.EventHandler(this.mapToolStripMenuItem_Click);
            // 
            // map4TokunoToolStripMenuItem
            // 
            this.map4TokunoToolStripMenuItem.Name = "map4TokunoToolStripMenuItem";
            this.map4TokunoToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.map4TokunoToolStripMenuItem.Tag = "Tokuno";
            this.map4TokunoToolStripMenuItem.Text = "[map4] Токуно";
            this.map4TokunoToolStripMenuItem.Click += new System.EventHandler(this.mapToolStripMenuItem_Click);
            // 
            // map5TerMurToolStripMenuItem
            // 
            this.map5TerMurToolStripMenuItem.Name = "map5TerMurToolStripMenuItem";
            this.map5TerMurToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.map5TerMurToolStripMenuItem.Tag = "TerMur";
            this.map5TerMurToolStripMenuItem.Text = "[map5] ТерМур";
            this.map5TerMurToolStripMenuItem.Click += new System.EventHandler(this.mapToolStripMenuItem_Click);
            // 
            // colorDialog
            // 
            this.colorDialog.AnyColor = true;
            this.colorDialog.Color = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(8)))), ((int)(((byte)(8)))));
            this.colorDialog.FullOpen = true;
            this.colorDialog.SolidColorOnly = true;
            // 
            // colorButton
            // 
            this.colorButton.BackColor = System.Drawing.Color.Black;
            this.colorButton.Location = new System.Drawing.Point(166, 50);
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size(45, 20);
            this.colorButton.TabIndex = 35;
            this.colorButton.UseVisualStyleBackColor = false;
            this.colorButton.Click += new System.EventHandler(this.l_colorButton_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToolStripMenuItem,
            this.addGroupToolStripMenuItem,
            this.addNodeToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(167, 92);
            // 
            // addGroupToolStripMenuItem
            // 
            this.addGroupToolStripMenuItem.Name = "addGroupToolStripMenuItem";
            this.addGroupToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.addGroupToolStripMenuItem.Text = "Создать группу";
            this.addGroupToolStripMenuItem.Click += new System.EventHandler(this.addGroupToolStripMenuItem_Click);
            // 
            // addNodeToolStripMenuItem
            // 
            this.addNodeToolStripMenuItem.Name = "addNodeToolStripMenuItem";
            this.addNodeToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.addNodeToolStripMenuItem.Text = "Создать элемент";
            this.addNodeToolStripMenuItem.Click += new System.EventHandler(this.addNodeToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.deleteToolStripMenuItem.Text = "Удалить";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // goToolStripMenuItem
            // 
            this.goToolStripMenuItem.Name = "goToolStripMenuItem";
            this.goToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.goToolStripMenuItem.Text = "Переместиться";
            this.goToolStripMenuItem.Click += new System.EventHandler(this.goToolStripMenuItem_Click);
            // 
            // масштабToolStripMenuItem
            // 
            this.масштабToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scale03ToolStripMenuItem,
            this.scale02ToolStripMenuItem,
            this.scale01ToolStripMenuItem,
            this.scale0ToolStripMenuItem,
            this.scale1ToolStripMenuItem,
            this.scale2ToolStripMenuItem,
            this.scale3ToolStripMenuItem,
            this.scale4ToolStripMenuItem});
            this.масштабToolStripMenuItem.Name = "масштабToolStripMenuItem";
            this.масштабToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.масштабToolStripMenuItem.Text = "Масштаб";
            // 
            // scale0ToolStripMenuItem
            // 
            this.scale0ToolStripMenuItem.Checked = true;
            this.scale0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.scale0ToolStripMenuItem.Name = "scale0ToolStripMenuItem";
            this.scale0ToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.scale0ToolStripMenuItem.Tag = "0";
            this.scale0ToolStripMenuItem.Text = "  100 %";
            this.scale0ToolStripMenuItem.Click += new System.EventHandler(this.scaleToolStripMenuItem_Click);
            // 
            // scale1ToolStripMenuItem
            // 
            this.scale1ToolStripMenuItem.Name = "scale1ToolStripMenuItem";
            this.scale1ToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.scale1ToolStripMenuItem.Tag = "1";
            this.scale1ToolStripMenuItem.Text = "  200 %";
            this.scale1ToolStripMenuItem.Click += new System.EventHandler(this.scaleToolStripMenuItem_Click);
            // 
            // scale2ToolStripMenuItem
            // 
            this.scale2ToolStripMenuItem.Name = "scale2ToolStripMenuItem";
            this.scale2ToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.scale2ToolStripMenuItem.Tag = "2";
            this.scale2ToolStripMenuItem.Text = "  400%";
            this.scale2ToolStripMenuItem.Click += new System.EventHandler(this.scaleToolStripMenuItem_Click);
            // 
            // scale3ToolStripMenuItem
            // 
            this.scale3ToolStripMenuItem.Name = "scale3ToolStripMenuItem";
            this.scale3ToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.scale3ToolStripMenuItem.Tag = "3";
            this.scale3ToolStripMenuItem.Text = "  800 %";
            this.scale3ToolStripMenuItem.Click += new System.EventHandler(this.scaleToolStripMenuItem_Click);
            // 
            // scale4ToolStripMenuItem
            // 
            this.scale4ToolStripMenuItem.Name = "scale4ToolStripMenuItem";
            this.scale4ToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.scale4ToolStripMenuItem.Tag = "4";
            this.scale4ToolStripMenuItem.Text = "1600 %";
            this.scale4ToolStripMenuItem.Click += new System.EventHandler(this.scaleToolStripMenuItem_Click);
            // 
            // scale01ToolStripMenuItem
            // 
            this.scale01ToolStripMenuItem.Name = "scale01ToolStripMenuItem";
            this.scale01ToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.scale01ToolStripMenuItem.Tag = "-1";
            this.scale01ToolStripMenuItem.Text = "    50 %";
            this.scale01ToolStripMenuItem.Click += new System.EventHandler(this.scaleToolStripMenuItem_Click);
            // 
            // scale02ToolStripMenuItem
            // 
            this.scale02ToolStripMenuItem.Name = "scale02ToolStripMenuItem";
            this.scale02ToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.scale02ToolStripMenuItem.Tag = "-2";
            this.scale02ToolStripMenuItem.Text = "    25 %";
            this.scale02ToolStripMenuItem.Click += new System.EventHandler(this.scaleToolStripMenuItem_Click);
            // 
            // scale03ToolStripMenuItem
            // 
            this.scale03ToolStripMenuItem.Name = "scale03ToolStripMenuItem";
            this.scale03ToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.scale03ToolStripMenuItem.Tag = "-3";
            this.scale03ToolStripMenuItem.Text = " 12,5 %";
            this.scale03ToolStripMenuItem.Click += new System.EventHandler(this.scaleToolStripMenuItem_Click);
            // 
            // mapViewer
            // 
            this.mapViewer.Center = new System.Drawing.Point(7212, 3108);
            this.mapViewer.DisplayErrors = true;
            this.mapViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapViewer.DrawObjects = null;
            this.mapViewer.DrawStatics = false;
            this.mapViewer.Location = new System.Drawing.Point(0, 24);
            this.mapViewer.Map = FiddlerControls.RegionEditor.MapViewer.Maps.Sosaria;
            mulManager1.CustomFolder = null;
            mulManager1.Table = null;
            this.mapViewer.MulManager = mulManager1;
            this.mapViewer.Name = "mapViewer";
            this.mapViewer.Navigation = FiddlerControls.RegionEditor.MapViewer.MapNavigation.None;
            this.mapViewer.RotateView = false;
            this.mapViewer.ShowCross = false;
            this.mapViewer.Size = new System.Drawing.Size(521, 384);
            this.mapViewer.TabIndex = 1;
            this.mapViewer.Text = "mapViewer";
            this.mapViewer.WheelZoom = false;
            this.mapViewer.XRayView = false;
            this.mapViewer.ZoomLevel = 0;
            this.mapViewer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mapViewer_MouseDown);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Filter = "xml файл (*.xml)|*.xml";
            this.openFileDialog.Title = "Выберите *.xml файл локаций";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "xml файл (*.xml)|*.xml";
            this.saveFileDialog.Title = "Сохранение *.xml файла локаций";
            // 
            // TabLocations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.Name = "TabLocations";
            this.Size = new System.Drawing.Size(739, 408);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xNumericUpDown)).EndInit();
            this.l_menuStrip.ResumeLayout(false);
            this.l_menuStrip.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.MenuStrip l_menuStrip;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown zNumericUpDown;
        private System.Windows.Forms.NumericUpDown yNumericUpDown;
        private System.Windows.Forms.NumericUpDown xNumericUpDown;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView locationsTreeView;
        private FiddlerControls.RegionEditor.MapViewer.MapViewer mapViewer;
        private System.Windows.Forms.ComboBox mapComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem картаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem map0DungeonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem map1SosariaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem map2IlshinarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem map3MalasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem map4TokunoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem map5TerMurToolStripMenuItem;
        private System.Windows.Forms.Button colorButton;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem масштабToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scale0ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scale1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scale2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scale3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scale4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scale03ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scale02ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scale01ToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;

    }
}
