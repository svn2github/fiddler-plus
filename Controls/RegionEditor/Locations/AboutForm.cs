using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiddlerControls.RegionEditor.Locations
{
	/// <summary>
	/// Summary description for AboutForm.
	/// </summary>
	public class AboutForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label VersionNumber;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label LicenseText;
		private System.Windows.Forms.Button ButtonClose;

		private static string License =
			"This software is provided as is without warranties of any kind. In no event shall the author be liable for any direct, special, indirect or consequential damages whatsoever resulting from loss of use of data or profits, whether in an action of contract, negligence or other tortious conduct, arising out of or in connection with the use or performance of this software.";

		public AboutForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			VersionNumber.Text = string.Format( "Version: {0}", this.ProductVersion );
			LicenseText.Text = License;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AboutForm));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.VersionNumber = new System.Windows.Forms.Label();
			this.LicenseText = new System.Windows.Forms.Label();
			this.ButtonClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(152, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(208, 120);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
			// 
			// linkLabel1
			// 
			this.linkLabel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.linkLabel1.ForeColor = System.Drawing.Color.DeepSkyBlue;
			this.linkLabel1.LinkColor = System.Drawing.Color.SteelBlue;
			this.linkLabel1.Location = new System.Drawing.Point(168, 120);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(184, 32);
			this.linkLabel1.TabIndex = 1;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "http://arya.runuo.com/";
			this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.linkLabel1.VisitedLinkColor = System.Drawing.Color.SteelBlue;
			this.linkLabel1.Click += new System.EventHandler(this.pictureBox1_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.MediumVioletRed;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(144, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "LOCATION EDITOR";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VersionNumber
			// 
			this.VersionNumber.Location = new System.Drawing.Point(8, 32);
			this.VersionNumber.Name = "VersionNumber";
			this.VersionNumber.Size = new System.Drawing.Size(136, 23);
			this.VersionNumber.TabIndex = 3;
			// 
			// LicenseText
			// 
			this.LicenseText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.LicenseText.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.LicenseText.Location = new System.Drawing.Point(8, 56);
			this.LicenseText.Name = "LicenseText";
			this.LicenseText.Size = new System.Drawing.Size(136, 128);
			this.LicenseText.TabIndex = 4;
			// 
			// ButtonClose
			// 
			this.ButtonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.ButtonClose.Location = new System.Drawing.Point(280, 160);
			this.ButtonClose.Name = "ButtonClose";
			this.ButtonClose.TabIndex = 5;
			this.ButtonClose.Text = "Close";
			this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
			// 
			// AboutForm
			// 
			this.AcceptButton = this.ButtonClose;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(358, 190);
			this.ControlBox = false;
			this.Controls.Add(this.ButtonClose);
			this.Controls.Add(this.LicenseText);
			this.Controls.Add(this.VersionNumber);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.pictureBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.TopMost = true;
			this.ResumeLayout(false);

		}
		#endregion

		private void ButtonClose_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void pictureBox1_Click(object sender, System.EventArgs e)
		{
			System.Diagnostics.Process.Start("http://arya.runuo.com/");
		}
	}
}
