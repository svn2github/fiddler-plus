namespace FiddlerControls
{
    partial class TextureCrossGen
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
            this.listViewMask = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBoxTexture1 = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBoxTexture2 = new System.Windows.Forms.PictureBox();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.maskedTextBoxID = new System.Windows.Forms.MaskedTextBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.comboBoxOptions = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTexture1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTexture2)).BeginInit();
            this.SuspendLayout();
            // 
            // listViewMask
            // 
            this.listViewMask.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewMask.BackColor = System.Drawing.Color.LightGray;
            this.listViewMask.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewMask.HideSelection = false;
            this.listViewMask.Location = new System.Drawing.Point(150, 1);
            this.listViewMask.Name = "listViewMask";
            this.listViewMask.OwnerDraw = true;
            this.listViewMask.ShowGroups = false;
            this.listViewMask.Size = new System.Drawing.Size(389, 435);
            this.listViewMask.TabIndex = 2;
            this.listViewMask.TileSize = new System.Drawing.Size(64, 64);
            this.listViewMask.UseCompatibleStateImageBehavior = false;
            this.listViewMask.View = System.Windows.Forms.View.Tile;
            this.listViewMask.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.DrawMaskItem);
            this.listViewMask.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnAction_DragDrop);
            this.listViewMask.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnAction_DragEnter);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBoxTexture1);
            this.groupBox1.Location = new System.Drawing.Point(4, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(140, 147);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Текстура #1 (Черный)";
            // 
            // pictureBoxTexture1
            // 
            this.pictureBoxTexture1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBoxTexture1.Location = new System.Drawing.Point(6, 13);
            this.pictureBoxTexture1.Name = "pictureBoxTexture1";
            this.pictureBoxTexture1.Size = new System.Drawing.Size(128, 128);
            this.pictureBoxTexture1.TabIndex = 1;
            this.pictureBoxTexture1.TabStop = false;
            this.pictureBoxTexture1.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnAction_DragDrop);
            this.pictureBoxTexture1.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnAction_DragEnter);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBoxTexture2);
            this.groupBox2.Location = new System.Drawing.Point(4, 161);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(140, 147);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Текстура #2 (Белый)";
            // 
            // pictureBoxTexture2
            // 
            this.pictureBoxTexture2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBoxTexture2.Location = new System.Drawing.Point(6, 13);
            this.pictureBoxTexture2.Name = "pictureBoxTexture2";
            this.pictureBoxTexture2.Size = new System.Drawing.Size(128, 128);
            this.pictureBoxTexture2.TabIndex = 1;
            this.pictureBoxTexture2.TabStop = false;
            this.pictureBoxTexture2.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnAction_DragDrop);
            this.pictureBoxTexture2.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnAction_DragEnter);
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonGenerate.Enabled = false;
            this.buttonGenerate.Location = new System.Drawing.Point(10, 370);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(128, 30);
            this.buttonGenerate.TabIndex = 6;
            this.buttonGenerate.Text = "Сгенерировать";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonClose.Location = new System.Drawing.Point(10, 406);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(61, 22);
            this.buttonClose.TabIndex = 7;
            this.buttonClose.Text = "Закрыть";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDelete.Location = new System.Drawing.Point(77, 406);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(61, 22);
            this.buttonDelete.TabIndex = 8;
            this.buttonDelete.Text = "Удалить";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 311);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 26);
            this.label1.TabIndex = 10;
            this.label1.Text = "Нумеровать\r\nначиная с ID:";
            // 
            // maskedTextBoxID
            // 
            this.maskedTextBoxID.Location = new System.Drawing.Point(78, 315);
            this.maskedTextBoxID.Mask = "\\0x>&&&&";
            this.maskedTextBoxID.Name = "maskedTextBoxID";
            this.maskedTextBoxID.Size = new System.Drawing.Size(60, 20);
            this.maskedTextBoxID.TabIndex = 11;
            this.maskedTextBoxID.TextChanged += new System.EventHandler(this.maskedTextBoxID_TextChanged);
            this.maskedTextBoxID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.maskedTextBoxID_KeyPress);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Выбирите папку в которую хотите сохранить текстуры и тайлы.";
            // 
            // comboBoxOptions
            // 
            this.comboBoxOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOptions.Items.AddRange(new object[] {
            "Наименьшее",
            "Наибольшее",
            "Как у текстуры #1",
            "Как у текстуры #2",
            "Текстуры 64x64",
            "Текстуры 128x128",
            "Текстуры 256x256"});
            this.comboBoxOptions.Location = new System.Drawing.Point(10, 342);
            this.comboBoxOptions.Name = "comboBoxOptions";
            this.comboBoxOptions.Size = new System.Drawing.Size(127, 21);
            this.comboBoxOptions.TabIndex = 12;
            this.comboBoxOptions.Tag = "";
            // 
            // TextureCrossGen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxOptions);
            this.Controls.Add(this.maskedTextBoxID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonGenerate);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listViewMask);
            this.MinimumSize = new System.Drawing.Size(540, 437);
            this.Name = "TextureCrossGen";
            this.Size = new System.Drawing.Size(540, 437);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTexture1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTexture2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewMask;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBoxTexture1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictureBoxTexture2;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxID;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ComboBox comboBoxOptions;

    }
}