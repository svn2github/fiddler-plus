namespace FiddlerControls.RegionEditor
{
    partial class RegionEditor
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabLocations = new System.Windows.Forms.TabPage();
            this.locations = new FiddlerControls.RegionEditor.Locations.TabLocations();
            this.tabRegions = new System.Windows.Forms.TabPage();
            this.tabSpawner = new System.Windows.Forms.TabPage();
            this.tabEncounters = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabLocations.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl1.Controls.Add(this.tabLocations);
            this.tabControl1.Controls.Add(this.tabRegions);
            this.tabControl1.Controls.Add(this.tabSpawner);
            this.tabControl1.Controls.Add(this.tabEncounters);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(841, 618);
            this.tabControl1.TabIndex = 0;
            // 
            // tabLocations
            // 
            this.tabLocations.BackColor = System.Drawing.Color.Transparent;
            this.tabLocations.Controls.Add(this.locations);
            this.tabLocations.Location = new System.Drawing.Point(23, 4);
            this.tabLocations.Name = "tabLocations";
            this.tabLocations.Padding = new System.Windows.Forms.Padding(3);
            this.tabLocations.Size = new System.Drawing.Size(814, 610);
            this.tabLocations.TabIndex = 0;
            this.tabLocations.Text = "Локации";
            // 
            // locations
            // 
            this.locations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.locations.Location = new System.Drawing.Point(3, 3);
            this.locations.Name = "locations";
            this.locations.Size = new System.Drawing.Size(808, 604);
            this.locations.TabIndex = 0;
            // 
            // tabRegions
            // 
            this.tabRegions.Location = new System.Drawing.Point(23, 4);
            this.tabRegions.Name = "tabRegions";
            this.tabRegions.Padding = new System.Windows.Forms.Padding(3);
            this.tabRegions.Size = new System.Drawing.Size(814, 610);
            this.tabRegions.TabIndex = 1;
            this.tabRegions.Text = "Регионы";
            this.tabRegions.UseVisualStyleBackColor = true;
            // 
            // tabSpawner
            // 
            this.tabSpawner.Location = new System.Drawing.Point(23, 4);
            this.tabSpawner.Name = "tabSpawner";
            this.tabSpawner.Size = new System.Drawing.Size(814, 610);
            this.tabSpawner.TabIndex = 2;
            this.tabSpawner.Text = "Заселение";
            this.tabSpawner.UseVisualStyleBackColor = true;
            // 
            // tabEncounters
            // 
            this.tabEncounters.Location = new System.Drawing.Point(23, 4);
            this.tabEncounters.Name = "tabEncounters";
            this.tabEncounters.Size = new System.Drawing.Size(814, 610);
            this.tabEncounters.TabIndex = 3;
            this.tabEncounters.Text = "Стычки";
            this.tabEncounters.UseVisualStyleBackColor = true;
            // 
            // RegionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "RegionEditor";
            this.Size = new System.Drawing.Size(841, 618);
            this.tabControl1.ResumeLayout(false);
            this.tabLocations.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabLocations;
        private System.Windows.Forms.TabPage tabRegions;
        private System.Windows.Forms.TabPage tabSpawner;
        private System.Windows.Forms.TabPage tabEncounters;
        private Locations.TabLocations locations;
    }
}
