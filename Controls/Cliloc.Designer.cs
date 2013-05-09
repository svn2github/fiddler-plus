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

namespace FiddlerControls
{
    partial class Cliloc
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Cliloc));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyCliLocNumberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyCliLocTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.перейтиКСледПустСтрокеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.insertNBefor = new System.Windows.Forms.ToolStripMenuItem();
            this.insertNBeforTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.insertNAfter = new System.Windows.Forms.ToolStripMenuItem();
            this.insertNAfterTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.clilocAddEntry = new System.Windows.Forms.ToolStripMenuItem();
            this.addEntryTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.deleteEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ClilocMenuButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.LangComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.ClilocSaveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ClilocRewriteButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ClilocExportTiledataButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ClilocImportTiledataButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.ClilocExportButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ClilocImportButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.GotoEntry = new System.Windows.Forms.ToolStripTextBox();
            this.GotoButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.FindEntry = new System.Windows.Forms.ToolStripTextBox();
            this.FindButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ClilocTilesButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.TextBox = new System.Windows.Forms.RichTextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.NumberLabel = new System.Windows.Forms.ToolStripLabel();
            this.SaveButton = new System.Windows.Forms.ToolStripButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.countLabel = new System.Windows.Forms.ToolStripLabel();
            this.ClilocEmptyButton = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyCliLocNumberToolStripMenuItem,
            this.copyCliLocTextToolStripMenuItem,
            this.toolStripSeparator3,
            this.toolStripMenuItem1,
            this.перейтиКСледПустСтрокеToolStripMenuItem,
            this.toolStripSeparator7,
            this.insertNBefor,
            this.insertNAfter,
            this.clilocAddEntry,
            this.deleteEntryToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(251, 192);
            // 
            // copyCliLocNumberToolStripMenuItem
            // 
            this.copyCliLocNumberToolStripMenuItem.Name = "copyCliLocNumberToolStripMenuItem";
            this.copyCliLocNumberToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.copyCliLocNumberToolStripMenuItem.Text = "Копировать номер ";
            this.copyCliLocNumberToolStripMenuItem.Click += new System.EventHandler(this.OnCLick_CopyClilocNumber);
            // 
            // copyCliLocTextToolStripMenuItem
            // 
            this.copyCliLocTextToolStripMenuItem.Name = "copyCliLocTextToolStripMenuItem";
            this.copyCliLocTextToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.copyCliLocTextToolStripMenuItem.Text = "Копировать текст";
            this.copyCliLocTextToolStripMenuItem.Click += new System.EventHandler(this.OnCLick_CopyClilocText);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(247, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Enabled = false;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(250, 22);
            this.toolStripMenuItem1.Text = "Перейти к предю пустой строке";
            // 
            // перейтиКСледПустСтрокеToolStripMenuItem
            // 
            this.перейтиКСледПустСтрокеToolStripMenuItem.Enabled = false;
            this.перейтиКСледПустСтрокеToolStripMenuItem.Name = "перейтиКСледПустСтрокеToolStripMenuItem";
            this.перейтиКСледПустСтрокеToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.перейтиКСледПустСтрокеToolStripMenuItem.Text = "Перейти к след. пустой строке";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(247, 6);
            // 
            // insertNBefor
            // 
            this.insertNBefor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertNBeforTextBox});
            this.insertNBefor.Name = "insertNBefor";
            this.insertNBefor.Size = new System.Drawing.Size(250, 22);
            this.insertNBefor.Text = "Вставить N строк подряд перед";
            this.insertNBefor.Click += new System.EventHandler(this.insertNBefor_Click);
            // 
            // insertNBeforTextBox
            // 
            this.insertNBeforTextBox.Name = "insertNBeforTextBox";
            this.insertNBeforTextBox.Size = new System.Drawing.Size(100, 23);
            this.insertNBeforTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.insertNBeforTextBox_KeyDown);
            // 
            // insertNAfter
            // 
            this.insertNAfter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertNAfterTextBox});
            this.insertNAfter.Name = "insertNAfter";
            this.insertNAfter.Size = new System.Drawing.Size(250, 22);
            this.insertNAfter.Text = "Вставить N строк подряд после";
            this.insertNAfter.Click += new System.EventHandler(this.insertNAfter_Click);
            // 
            // insertNAfterTextBox
            // 
            this.insertNAfterTextBox.Name = "insertNAfterTextBox";
            this.insertNAfterTextBox.Size = new System.Drawing.Size(100, 23);
            this.insertNAfterTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.insertNAfterTextBox_KeyDown);
            // 
            // clilocAddEntry
            // 
            this.clilocAddEntry.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addEntryTextBox});
            this.clilocAddEntry.Name = "clilocAddEntry";
            this.clilocAddEntry.Size = new System.Drawing.Size(250, 22);
            this.clilocAddEntry.Text = "Добавить новую строку";
            // 
            // addEntryTextBox
            // 
            this.addEntryTextBox.Name = "addEntryTextBox";
            this.addEntryTextBox.Size = new System.Drawing.Size(100, 23);
            this.addEntryTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddEntryTextBox_KeyDown);
            // 
            // deleteEntryToolStripMenuItem
            // 
            this.deleteEntryToolStripMenuItem.Name = "deleteEntryToolStripMenuItem";
            this.deleteEntryToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.deleteEntryToolStripMenuItem.Text = "Удалить выделенную строку";
            this.deleteEntryToolStripMenuItem.Click += new System.EventHandler(this.OnClick_DeleteEntry);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ClilocMenuButton,
            this.toolStripSeparator6,
            this.GotoEntry,
            this.GotoButton,
            this.toolStripSeparator1,
            this.FindEntry,
            this.FindButton,
            this.toolStripSeparator2,
            this.ClilocTilesButton,
            this.ClilocEmptyButton,
            this.toolStripSeparator5,
            this.countLabel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(928, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ClilocMenuButton
            // 
            this.ClilocMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LangComboBox,
            this.ClilocSaveButton,
            this.ClilocRewriteButton,
            this.toolStripSeparator4,
            this.ClilocExportTiledataButton,
            this.ClilocImportTiledataButton,
            this.toolStripSeparator8,
            this.ClilocExportButton,
            this.ClilocImportButton});
            this.ClilocMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("ClilocMenuButton.Image")));
            this.ClilocMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ClilocMenuButton.Name = "ClilocMenuButton";
            this.ClilocMenuButton.Size = new System.Drawing.Size(70, 22);
            this.ClilocMenuButton.Text = "Меню";
            // 
            // LangComboBox
            // 
            this.LangComboBox.Items.AddRange(new object[] {
            "English",
            "Russian",
            "Custom 1",
            "Custom 2"});
            this.LangComboBox.Name = "LangComboBox";
            this.LangComboBox.Size = new System.Drawing.Size(161, 23);
            this.LangComboBox.SelectedIndexChanged += new System.EventHandler(this.onLangChange);
            // 
            // ClilocSaveButton
            // 
            this.ClilocSaveButton.Name = "ClilocSaveButton";
            this.ClilocSaveButton.Size = new System.Drawing.Size(243, 22);
            this.ClilocSaveButton.Text = "Сохранить cliloc";
            this.ClilocSaveButton.Click += new System.EventHandler(this.OnClickSave);
            // 
            // ClilocRewriteButton
            // 
            this.ClilocRewriteButton.Name = "ClilocRewriteButton";
            this.ClilocRewriteButton.Size = new System.Drawing.Size(243, 22);
            this.ClilocRewriteButton.Text = "Перезаписать cliloc";
            this.ClilocRewriteButton.Click += new System.EventHandler(this.OnClickSave);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(240, 6);
            // 
            // ClilocExportTiledataButton
            // 
            this.ClilocExportTiledataButton.Name = "ClilocExportTiledataButton";
            this.ClilocExportTiledataButton.Size = new System.Drawing.Size(243, 22);
            this.ClilocExportTiledataButton.Text = "Экспортировать в tiledata.mul";
            this.ClilocExportTiledataButton.Click += new System.EventHandler(this.ClilocExportTiledataButton_Click);
            // 
            // ClilocImportTiledataButton
            // 
            this.ClilocImportTiledataButton.Name = "ClilocImportTiledataButton";
            this.ClilocImportTiledataButton.Size = new System.Drawing.Size(243, 22);
            this.ClilocImportTiledataButton.Text = "Импортировать из tiledata.mul";
            this.ClilocImportTiledataButton.Click += new System.EventHandler(this.ClilocImportTiledataButton_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(240, 6);
            // 
            // ClilocExportButton
            // 
            this.ClilocExportButton.Name = "ClilocExportButton";
            this.ClilocExportButton.Size = new System.Drawing.Size(243, 22);
            this.ClilocExportButton.Text = "Экспортировать в  *.csv файл";
            this.ClilocExportButton.Click += new System.EventHandler(this.OnClickExportCSV);
            // 
            // ClilocImportButton
            // 
            this.ClilocImportButton.Name = "ClilocImportButton";
            this.ClilocImportButton.Size = new System.Drawing.Size(243, 22);
            this.ClilocImportButton.Text = "Импортировать из *.csv файл";
            this.ClilocImportButton.Click += new System.EventHandler(this.OnClickImportCSV);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // GotoEntry
            // 
            this.GotoEntry.MaxLength = 17;
            this.GotoEntry.Name = "GotoEntry";
            this.GotoEntry.Size = new System.Drawing.Size(108, 25);
            this.GotoEntry.ToolTipText = "Введите номер строки или номер тайла статики ввиде \r\nI0x???? или <Item ID=\"0x????" +
    "\" /> или <Item ID=\"?????\" /> \r\nдля быстрой навигации.";
            this.GotoEntry.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GotoEntry_KeyDown);
            // 
            // GotoButton
            // 
            this.GotoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.GotoButton.Name = "GotoButton";
            this.GotoButton.Size = new System.Drawing.Size(58, 22);
            this.GotoButton.Text = "Перейти";
            this.GotoButton.ToolTipText = "Перейти к строке";
            this.GotoButton.Click += new System.EventHandler(this.GotoNr);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // FindEntry
            // 
            this.FindEntry.AcceptsTab = true;
            this.FindEntry.Name = "FindEntry";
            this.FindEntry.ShortcutsEnabled = false;
            this.FindEntry.Size = new System.Drawing.Size(200, 25);
            this.FindEntry.ToolTipText = "Часть текста для поиска во всех строках";
            this.FindEntry.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindEntry_KeyDown);
            // 
            // FindButton
            // 
            this.FindButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FindButton.Name = "FindButton";
            this.FindButton.Size = new System.Drawing.Size(46, 22);
            this.FindButton.Text = "Поиск";
            this.FindButton.ToolTipText = "Поиск строки";
            this.FindButton.Click += new System.EventHandler(this.FindEntryClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // ClilocTilesButton
            // 
            this.ClilocTilesButton.Checked = true;
            this.ClilocTilesButton.CheckOnClick = true;
            this.ClilocTilesButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ClilocTilesButton.Image = ((System.Drawing.Image)(resources.GetObject("ClilocTilesButton.Image")));
            this.ClilocTilesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ClilocTilesButton.Name = "ClilocTilesButton";
            this.ClilocTilesButton.Size = new System.Drawing.Size(105, 22);
            this.ClilocTilesButton.Text = "Имена тайлов";
            this.ClilocTilesButton.ToolTipText = "Отобразить или скрыть строки \r\nявляющиеся именами тайлов.";
            this.ClilocTilesButton.Click += new System.EventHandler(this.ClilocTilesButton_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.TextBox);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip2);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainer1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainer1.Size = new System.Drawing.Size(928, 330);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 3;
            // 
            // TextBox
            // 
            this.TextBox.DetectUrls = false;
            this.TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TextBox.Location = new System.Drawing.Point(0, 25);
            this.TextBox.Name = "TextBox";
            this.TextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.TextBox.Size = new System.Drawing.Size(300, 305);
            this.TextBox.TabIndex = 2;
            this.TextBox.Text = "";
            this.TextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NumberLabel,
            this.SaveButton});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStrip2.Size = new System.Drawing.Size(300, 25);
            this.toolStrip2.TabIndex = 3;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // NumberLabel
            // 
            this.NumberLabel.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.NumberLabel.Name = "NumberLabel";
            this.NumberLabel.Size = new System.Drawing.Size(26, 22);
            this.NumberLabel.Text = "# ";
            this.NumberLabel.ToolTipText = "Номер редактируемой строки";
            // 
            // SaveButton
            // 
            this.SaveButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.SaveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SaveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(69, 22);
            this.SaveButton.Text = "Сохранить";
            this.SaveButton.Visible = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 30;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(624, 330);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridView1_CellPainting);
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.OnHeaderClicked);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.onCell_Selected);
            // 
            // countLabel
            // 
            this.countLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(36, 22);
            this.countLabel.Text = "... / ...";
            this.countLabel.ToolTipText = "Отображается строк / Всего строк";
            // 
            // ClilocEmptyButton
            // 
            this.ClilocEmptyButton.Checked = true;
            this.ClilocEmptyButton.CheckOnClick = true;
            this.ClilocEmptyButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ClilocEmptyButton.Image = ((System.Drawing.Image)(resources.GetObject("ClilocEmptyButton.Image")));
            this.ClilocEmptyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ClilocEmptyButton.Name = "ClilocEmptyButton";
            this.ClilocEmptyButton.Size = new System.Drawing.Size(109, 22);
            this.ClilocEmptyButton.Text = "Пустые строки";
            this.ClilocEmptyButton.ToolTipText = "Отобразить или скрыть пустые строки.\r\n";
            this.ClilocEmptyButton.Click += new System.EventHandler(this.ClilocEmptyButton_Click);
            // 
            // Cliloc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Name = "Cliloc";
            this.Size = new System.Drawing.Size(928, 355);
            this.Load += new System.EventHandler(this.OnLoad);
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox GotoEntry;
        private System.Windows.Forms.ToolStripButton GotoButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox FindEntry;
        private System.Windows.Forms.ToolStripButton FindButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clilocAddEntry;
        private System.Windows.Forms.ToolStripMenuItem deleteEntryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyCliLocNumberToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyCliLocTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripDropDownButton ClilocMenuButton;
        private System.Windows.Forms.ToolStripComboBox LangComboBox;
        private System.Windows.Forms.ToolStripMenuItem ClilocSaveButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem ClilocExportButton;
        private System.Windows.Forms.ToolStripMenuItem ClilocImportButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton ClilocTilesButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.RichTextBox TextBox;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel NumberLabel;
        private System.Windows.Forms.ToolStripButton SaveButton;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem перейтиКСледПустСтрокеToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem insertNBefor;
        private System.Windows.Forms.ToolStripTextBox insertNBeforTextBox;
        private System.Windows.Forms.ToolStripMenuItem insertNAfter;
        private System.Windows.Forms.ToolStripTextBox insertNAfterTextBox;
        private System.Windows.Forms.ToolStripTextBox addEntryTextBox;
        private System.Windows.Forms.ToolStripMenuItem ClilocExportTiledataButton;
        private System.Windows.Forms.ToolStripMenuItem ClilocImportTiledataButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem ClilocRewriteButton;
        private System.Windows.Forms.ToolStripLabel countLabel;
        private System.Windows.Forms.ToolStripButton ClilocEmptyButton;
    }
}
