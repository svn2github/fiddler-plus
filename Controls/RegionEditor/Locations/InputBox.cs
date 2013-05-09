using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiddlerControls.RegionEditor.Locations
{
	/// <summary>
	/// A general purpose input box
	/// </summary>
	public class InputBox : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.TextBox InputText;
		private System.Windows.Forms.Label MessageText;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Creates a general purpose input box
		/// </summary>
		/// <param name="message">The message displayed by the input box. This is the question asked, or the description of the input required</param>
		/// <param name="initialValue">The initial value of the input. It can be null</param>
		public InputBox( string message, string initialValue)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			MessageText.Text = message;
			if ( initialValue != null )
				InputText.Text = initialValue;
		}

		/// <summary>
		/// Gets the user input
		/// </summary>
		public string Input
		{
			get
			{
				return InputText.Text;
			}
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
			this.button1 = new System.Windows.Forms.Button();
			this.Cancel = new System.Windows.Forms.Button();
			this.InputText = new System.Windows.Forms.TextBox();
			this.MessageText = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Location = new System.Drawing.Point(216, 64);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 20);
			this.button1.TabIndex = 0;
			this.button1.Text = "OK";
			// 
			// Cancel
			// 
			this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.Cancel.Location = new System.Drawing.Point(136, 64);
			this.Cancel.Name = "Cancel";
			this.Cancel.Size = new System.Drawing.Size(75, 20);
			this.Cancel.TabIndex = 1;
			this.Cancel.Text = "Cancel";
			// 
			// InputText
			// 
			this.InputText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.InputText.Location = new System.Drawing.Point(8, 64);
			this.InputText.Name = "InputText";
			this.InputText.Size = new System.Drawing.Size(120, 20);
			this.InputText.TabIndex = 2;
			this.InputText.Text = "textBox1";
			// 
			// MessageText
			// 
			this.MessageText.Location = new System.Drawing.Point(8, 8);
			this.MessageText.Name = "MessageText";
			this.MessageText.Size = new System.Drawing.Size(280, 48);
			this.MessageText.TabIndex = 3;
			this.MessageText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// InputBox
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 88);
			this.Controls.Add(this.MessageText);
			this.Controls.Add(this.InputText);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "InputBox";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);

		}
		#endregion
	}
}
