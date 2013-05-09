using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using FiddlerControls.RegionEditor;
using FiddlerControls.RegionEditor.BoxCommon;
using FiddlerControls.RegionEditor.MapViewer;
using System.Xml.Serialization;
using System.IO;

namespace FiddlerControls.RegionEditor.Locations
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class LocationEditor : System.Windows.Forms.Form
	{
		private const string TitleText = "Location Editor";

		private string m_GoCommand = "[Go";
		private string m_UOFolder = "";

		private bool SettingPoint = false;
		private string Facet = "";
		private bool IsModified = false;
		private string CurrentFile = "";

		private LocOptions Options;

		private System.Windows.Forms.TreeView TreeCat;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button ButtonSet;
		private System.Windows.Forms.Button ButtonZoomIn;
		private System.Windows.Forms.Button ButtonZoomOut;
		private System.Windows.Forms.Button ButtonDel;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem MenuFile;
		private System.Windows.Forms.MenuItem FileNew;
		private System.Windows.Forms.MenuItem FileOpen;
		private System.Windows.Forms.MenuItem FileSave;
		private System.Windows.Forms.MenuItem FileSaveAs;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem FileExit;
		private System.Windows.Forms.OpenFileDialog OpenFile;
		private System.Windows.Forms.SaveFileDialog SaveFile;
		private System.Windows.Forms.TextBox Input;
		private System.Windows.Forms.Button ButtonAdd;
		private System.Windows.Forms.TextBox InX;
		private System.Windows.Forms.TextBox InY;
		private System.Windows.Forms.TextBox InZ;
		private System.Windows.Forms.MenuItem MenuOptions;
		private System.Windows.Forms.MenuItem OptionsTopmost;
		private System.Windows.Forms.MenuItem OptionsMap;
		private System.Windows.Forms.MenuItem Map0;
		private System.Windows.Forms.MenuItem Map2;
		private System.Windows.Forms.MenuItem Map3;
		private System.Windows.Forms.Label Status;
		private System.Windows.Forms.MenuItem Map1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.FolderBrowserDialog FolderBrowse;
        private FiddlerControls.RegionEditor.MapViewer.MapViewer Map;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem HelpHelp;
		private System.Windows.Forms.MenuItem HelpSite;
		private System.Windows.Forms.MenuItem HelpAbout;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem OptionsDrawStatics;
		private System.Windows.Forms.MenuItem OptionsGoCommand;
		private System.Windows.Forms.Button ButtonGo;
		private System.Windows.Forms.MenuItem Map4;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public LocationEditor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			Text = TitleText;

			Options = new LocOptions();

			LoadOptions();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(LocationEditor));
			this.TreeCat = new System.Windows.Forms.TreeView();
			this.Input = new System.Windows.Forms.TextBox();
			this.ButtonAdd = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.InX = new System.Windows.Forms.TextBox();
			this.InY = new System.Windows.Forms.TextBox();
			this.InZ = new System.Windows.Forms.TextBox();
			this.ButtonSet = new System.Windows.Forms.Button();
			this.ButtonZoomIn = new System.Windows.Forms.Button();
			this.ButtonZoomOut = new System.Windows.Forms.Button();
			this.ButtonDel = new System.Windows.Forms.Button();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.MenuFile = new System.Windows.Forms.MenuItem();
			this.FileNew = new System.Windows.Forms.MenuItem();
			this.FileOpen = new System.Windows.Forms.MenuItem();
			this.FileSave = new System.Windows.Forms.MenuItem();
			this.FileSaveAs = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.FileExit = new System.Windows.Forms.MenuItem();
			this.MenuOptions = new System.Windows.Forms.MenuItem();
			this.OptionsTopmost = new System.Windows.Forms.MenuItem();
			this.OptionsMap = new System.Windows.Forms.MenuItem();
			this.Map0 = new System.Windows.Forms.MenuItem();
			this.Map1 = new System.Windows.Forms.MenuItem();
			this.Map2 = new System.Windows.Forms.MenuItem();
			this.Map3 = new System.Windows.Forms.MenuItem();
			this.OptionsDrawStatics = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.OptionsGoCommand = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.HelpHelp = new System.Windows.Forms.MenuItem();
			this.HelpSite = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.HelpAbout = new System.Windows.Forms.MenuItem();
			this.OpenFile = new System.Windows.Forms.OpenFileDialog();
			this.SaveFile = new System.Windows.Forms.SaveFileDialog();
			this.Status = new System.Windows.Forms.Label();
			this.FolderBrowse = new System.Windows.Forms.FolderBrowserDialog();
            this.Map = new FiddlerControls.RegionEditor.MapViewer.MapViewer();
			this.ButtonGo = new System.Windows.Forms.Button();
			this.Map4 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// TreeCat
			// 
			this.TreeCat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TreeCat.ImageIndex = -1;
			this.TreeCat.LabelEdit = true;
			this.TreeCat.Location = new System.Drawing.Point(8, 8);
			this.TreeCat.Name = "TreeCat";
			this.TreeCat.SelectedImageIndex = -1;
			this.TreeCat.Size = new System.Drawing.Size(184, 248);
			this.TreeCat.Sorted = true;
			this.TreeCat.TabIndex = 0;
			this.TreeCat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TreeCat_KeyDown);
			this.TreeCat.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeCat_AfterSelect);
			this.TreeCat.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeCat_BeforeCheck);
			this.TreeCat.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.TreeCat_AfterLabelEdit);
			// 
			// Input
			// 
			this.Input.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Input.Location = new System.Drawing.Point(200, 56);
			this.Input.Name = "Input";
			this.Input.Size = new System.Drawing.Size(152, 20);
			this.Input.TabIndex = 3;
			this.Input.Text = "";
			// 
			// ButtonAdd
			// 
			this.ButtonAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ButtonAdd.Location = new System.Drawing.Point(304, 80);
			this.ButtonAdd.Name = "ButtonAdd";
			this.ButtonAdd.Size = new System.Drawing.Size(48, 20);
			this.ButtonAdd.TabIndex = 4;
			this.ButtonAdd.Text = "Add";
			this.ButtonAdd.Click += new System.EventHandler(this.ButtonAdd_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(200, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(8, 23);
			this.label1.TabIndex = 7;
			this.label1.Text = "X";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(256, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(8, 23);
			this.label2.TabIndex = 8;
			this.label2.Text = "Y";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(312, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(8, 23);
			this.label3.TabIndex = 9;
			this.label3.Text = "Z";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// InX
			// 
			this.InX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.InX.Enabled = false;
			this.InX.Location = new System.Drawing.Point(216, 8);
			this.InX.Name = "InX";
			this.InX.Size = new System.Drawing.Size(32, 20);
			this.InX.TabIndex = 10;
			this.InX.Text = "";
			// 
			// InY
			// 
			this.InY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.InY.Enabled = false;
			this.InY.Location = new System.Drawing.Point(272, 8);
			this.InY.Name = "InY";
			this.InY.Size = new System.Drawing.Size(32, 20);
			this.InY.TabIndex = 11;
			this.InY.Text = "";
			// 
			// InZ
			// 
			this.InZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.InZ.Enabled = false;
			this.InZ.Location = new System.Drawing.Point(328, 8);
			this.InZ.Name = "InZ";
			this.InZ.Size = new System.Drawing.Size(24, 20);
			this.InZ.TabIndex = 12;
			this.InZ.Text = "";
			// 
			// ButtonSet
			// 
			this.ButtonSet.Enabled = false;
			this.ButtonSet.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ButtonSet.Location = new System.Drawing.Point(200, 32);
			this.ButtonSet.Name = "ButtonSet";
			this.ButtonSet.Size = new System.Drawing.Size(152, 20);
			this.ButtonSet.TabIndex = 13;
			this.ButtonSet.Text = "Set new point";
			this.ButtonSet.Click += new System.EventHandler(this.ButtonSet_Click);
			// 
			// ButtonZoomIn
			// 
			this.ButtonZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ButtonZoomIn.Location = new System.Drawing.Point(328, 104);
			this.ButtonZoomIn.Name = "ButtonZoomIn";
			this.ButtonZoomIn.Size = new System.Drawing.Size(24, 20);
			this.ButtonZoomIn.TabIndex = 15;
			this.ButtonZoomIn.Text = "+";
			this.ButtonZoomIn.Click += new System.EventHandler(this.ButtonZoomIn_Click);
			// 
			// ButtonZoomOut
			// 
			this.ButtonZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ButtonZoomOut.Location = new System.Drawing.Point(328, 128);
			this.ButtonZoomOut.Name = "ButtonZoomOut";
			this.ButtonZoomOut.Size = new System.Drawing.Size(24, 20);
			this.ButtonZoomOut.TabIndex = 16;
			this.ButtonZoomOut.Text = "-";
			this.ButtonZoomOut.Click += new System.EventHandler(this.ButtonZoomOut_Click);
			// 
			// ButtonDel
			// 
			this.ButtonDel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ButtonDel.Location = new System.Drawing.Point(200, 80);
			this.ButtonDel.Name = "ButtonDel";
			this.ButtonDel.Size = new System.Drawing.Size(48, 20);
			this.ButtonDel.TabIndex = 18;
			this.ButtonDel.Text = "Delete";
			this.ButtonDel.Click += new System.EventHandler(this.ButtonDel_Click);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.MenuFile,
																					  this.MenuOptions,
																					  this.menuItem2});
			// 
			// MenuFile
			// 
			this.MenuFile.Index = 0;
			this.MenuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.FileNew,
																					 this.FileOpen,
																					 this.FileSave,
																					 this.FileSaveAs,
																					 this.menuItem6,
																					 this.FileExit});
			this.MenuFile.Text = "File";
			// 
			// FileNew
			// 
			this.FileNew.Index = 0;
			this.FileNew.Text = "New";
			this.FileNew.Click += new System.EventHandler(this.FileNew_Click);
			// 
			// FileOpen
			// 
			this.FileOpen.Index = 1;
			this.FileOpen.Text = "Open";
			this.FileOpen.Click += new System.EventHandler(this.FileOpen_Click);
			// 
			// FileSave
			// 
			this.FileSave.Index = 2;
			this.FileSave.Text = "Save";
			this.FileSave.Click += new System.EventHandler(this.FileSave_Click);
			// 
			// FileSaveAs
			// 
			this.FileSaveAs.Index = 3;
			this.FileSaveAs.Text = "Save as...";
			this.FileSaveAs.Click += new System.EventHandler(this.FileSaveAs_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 4;
			this.menuItem6.Text = "-";
			// 
			// FileExit
			// 
			this.FileExit.Index = 5;
			this.FileExit.Text = "Exit";
			this.FileExit.Click += new System.EventHandler(this.FileExit_Click);
			// 
			// MenuOptions
			// 
			this.MenuOptions.Index = 1;
			this.MenuOptions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.OptionsTopmost,
																						this.OptionsMap,
																						this.OptionsDrawStatics,
																						this.menuItem1,
																						this.OptionsGoCommand});
			this.MenuOptions.Text = "Options";
			this.MenuOptions.Popup += new System.EventHandler(this.MenuOptions_Popup);
			// 
			// OptionsTopmost
			// 
			this.OptionsTopmost.Index = 0;
			this.OptionsTopmost.Text = "Always On Top";
			this.OptionsTopmost.Click += new System.EventHandler(this.OptionsTopmost_Click);
			// 
			// OptionsMap
			// 
			this.OptionsMap.Index = 1;
			this.OptionsMap.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.Map0,
																					   this.Map1,
																					   this.Map2,
																					   this.Map3,
																					   this.Map4});
			this.OptionsMap.Text = "Map";
			// 
			// Map0
			// 
			this.Map0.Index = 0;
			this.Map0.Text = "Map 0";
			this.Map0.Click += new System.EventHandler(this.Map0_Click);
			// 
			// Map1
			// 
			this.Map1.Index = 1;
			this.Map1.Text = "Map 1 (Trammel)";
			this.Map1.Click += new System.EventHandler(this.Map1_Click);
			// 
			// Map2
			// 
			this.Map2.Index = 2;
			this.Map2.Text = "Map 2";
			this.Map2.Click += new System.EventHandler(this.Map2_Click);
			// 
			// Map3
			// 
			this.Map3.Index = 3;
			this.Map3.Text = "Map 3";
			this.Map3.Click += new System.EventHandler(this.Map3_Click);
			// 
			// OptionsDrawStatics
			// 
			this.OptionsDrawStatics.Index = 2;
			this.OptionsDrawStatics.Text = "Draw statics";
			this.OptionsDrawStatics.Click += new System.EventHandler(this.OptionsDrawStatics_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 3;
			this.menuItem1.Text = "Map File Folder";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
			// 
			// OptionsGoCommand
			// 
			this.OptionsGoCommand.Index = 4;
			this.OptionsGoCommand.Text = "Change Go Command";
			this.OptionsGoCommand.Click += new System.EventHandler(this.OptionsGoCommand_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 2;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.HelpHelp,
																					  this.HelpSite,
																					  this.menuItem7,
																					  this.HelpAbout});
			this.menuItem2.Text = "Help";
			// 
			// HelpHelp
			// 
			this.HelpHelp.Index = 0;
			this.HelpHelp.Text = "Help";
			this.HelpHelp.Click += new System.EventHandler(this.HelpHelp_Click);
			// 
			// HelpSite
			// 
			this.HelpSite.Index = 1;
			this.HelpSite.Text = "Visit The Box";
			this.HelpSite.Click += new System.EventHandler(this.HelpSite_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 2;
			this.menuItem7.Text = "-";
			// 
			// HelpAbout
			// 
			this.HelpAbout.Index = 3;
			this.HelpAbout.Text = "About";
			this.HelpAbout.Click += new System.EventHandler(this.HelpAbout_Click);
			// 
			// OpenFile
			// 
			this.OpenFile.Filter = "Xml Files (*.xml)|*.xml";
			// 
			// SaveFile
			// 
			this.SaveFile.Filter = "Xml Files (*.xml)|*.xml";
			// 
			// Status
			// 
			this.Status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Status.Location = new System.Drawing.Point(200, 152);
			this.Status.Name = "Status";
			this.Status.Size = new System.Drawing.Size(152, 104);
			this.Status.TabIndex = 19;
			this.Status.DoubleClick += new System.EventHandler(this.Status_DoubleClick);
			// 
			// Map
			// 
			this.Map.Center = new System.Drawing.Point(0, 0);
			this.Map.DisplayErrors = true;
			this.Map.DrawStatics = true;
			this.Map.Location = new System.Drawing.Point(360, 8);
            this.Map.Map = FiddlerControls.RegionEditor.MapViewer.Maps.Sosaria;
			this.Map.Name = "Map";
            this.Map.Navigation = FiddlerControls.RegionEditor.MapViewer.MapNavigation.None;
			this.Map.RotateView = false;
			this.Map.ShowCross = false;
			this.Map.Size = new System.Drawing.Size(296, 248);
			this.Map.TabIndex = 20;
			this.Map.Text = "mapViewer1";
			this.Map.WheelZoom = false;
			this.Map.XRayView = false;
			this.Map.ZoomLevel = 0;
			this.Map.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Map_MouseDown);
			// 
			// ButtonGo
			// 
			this.ButtonGo.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ButtonGo.Location = new System.Drawing.Point(200, 120);
			this.ButtonGo.Name = "ButtonGo";
			this.ButtonGo.Size = new System.Drawing.Size(48, 24);
			this.ButtonGo.TabIndex = 21;
			this.ButtonGo.Text = "Go";
			this.ButtonGo.Click += new System.EventHandler(this.ButtonGo_Click);
			// 
			// Map4
			// 
			this.Map4.Index = 4;
			this.Map4.Text = "Map 4";
			this.Map4.Click += new System.EventHandler(this.Map4_Click);
			// 
			// LocationEditor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(664, 259);
			this.Controls.Add(this.ButtonGo);
			this.Controls.Add(this.Map);
			this.Controls.Add(this.Status);
			this.Controls.Add(this.ButtonDel);
			this.Controls.Add(this.ButtonZoomOut);
			this.Controls.Add(this.ButtonZoomIn);
			this.Controls.Add(this.ButtonSet);
			this.Controls.Add(this.InZ);
			this.Controls.Add(this.InY);
			this.Controls.Add(this.InX);
			this.Controls.Add(this.Input);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ButtonAdd);
			this.Controls.Add(this.TreeCat);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Menu = this.mainMenu1;
			this.Name = "LocationEditor";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.LocationEditor_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.EnableVisualStyles();
			Application.Run(new LocationEditor());
		}

		private void FileOpen_Click(object sender, System.EventArgs e)
		{
			if ( IsModified )
			{
				// Add code to deal with changes
			}

			// Load
			if ( OpenFile.ShowDialog() == DialogResult.OK )
			{
				CurrentFile = OpenFile.FileName;
				string[] temp = CurrentFile.Split( new char[]{ '\\' } );
				string file = temp[ temp.Length - 1 ];

				try
				{
					ReadFile();
					Status.Text = "The file " + file + " has been loaded correctly";
				}
				catch ( Exception )
				{
					// Trying to open the wrong file type
					Status.Text = "The file " + file + " hasn't been loaded because it isn't a valid Locs file for RunUO";
				}
			}
		
		}

		private void ReadFile()
		{
			XmlDocument dom = new XmlDocument();
			dom.Load( CurrentFile );
			IsModified = false;

			// Clear trees
			TreeCat.BeginUpdate();
			TreeCat.Nodes.Clear();

			// Get the places node
			XmlNode xPlaces = dom.ChildNodes[1];

			// Get the facet node
			XmlNode xFacet = xPlaces.ChildNodes[0];

			Facet = xFacet.Attributes["name"].Value;
			// Set the title bar
			this.Text = Facet + " - " + TitleText;

			// Create root node
			TreeNode FacetNode = new TreeNode( Facet );
			TreeCat.Nodes.Add( FacetNode );

			// Set the map is possible
			switch ( Facet.ToLower() )
			{
				//case "felucca":
                case "dungeon": Map.Map = Maps.Dungeon; break;
				//case "trammel":
                case "sosaria": Map.Map = Maps.Sosaria; break;
				case "ilshenar":Map.Map = Maps.Ilshenar;break;
				case "malas":   Map.Map = Maps.Malas;   break;
				case "tokuno":  Map.Map = Maps.Tokuno;  break;
                case "termur":  Map.Map = Maps.TerMur;  break;
				default:        Map.Map = Maps.Dungeon; break;
			}

			// Create the tree
			foreach ( XmlNode xNode in xFacet )
				FacetNode.Nodes.Add( MakeNode( xNode ) );

			TreeCat.EndUpdate();
		}

		private TreeNode MakeNode( XmlNode xNode )
		{
			TreeNode tNode = new TreeNode();

			if ( xNode.Name == "child" )
			{
				// This is a simple child node
				Loc loc = new Loc( xNode );
				tNode.Text = loc.Name;
				// Put location as tag
				tNode.Tag = loc;
			}
			else
			{
				// This is a parent node
				tNode.Text = xNode.Attributes["name"].Value;

				foreach ( XmlNode xChild in xNode.ChildNodes )
					tNode.Nodes.Add( MakeNode( xChild ) );
			}

			return tNode;
		}

		private void TreeCat_BeforeCheck(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			Map.RemoveAllDrawObjects();

			// Reset point setting
			ButtonSet.BackColor = SystemColors.Control;
			SettingPoint = false;

			try
			{
				TreeCat.SelectedNode.ForeColor = SystemColors.WindowText;
				TreeCat.SelectedNode.BackColor = SystemColors.Window;
				if ( TreeCat.SelectedNode.Tag is Loc )
					UpdateData();
			}
			catch ( Exception )
			{
			}
		}

		private void TreeCat_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			TreeCat.SelectedNode.ForeColor = SystemColors.HighlightText;
			TreeCat.SelectedNode.BackColor = SystemColors.Highlight;

			if ( TreeCat.SelectedNode.Parent == null )
			{
				// Facet node
				InX.Enabled = false;
				InY.Enabled = false;
				InZ.Enabled = false;
				ButtonSet.Enabled = false;

				return;
			}

			if (TreeCat.SelectedNode.Tag is Loc)
			{
				// This is a child node. Enable stuff
				InX.Enabled = true;
				InY.Enabled = true;
				InZ.Enabled = true;
				ButtonSet.Enabled = true;

				Loc loc = (Loc) TreeCat.SelectedNode.Tag;

				if ( ((Loc)TreeCat.SelectedNode.Tag).IsDefined )
				{

					// Set the right coordinates on the map	
					Map.Center = new Point( loc.X, loc.Y );

					// Add the marker on the map
                    MapCircle circle = new FiddlerControls.RegionEditor.MapViewer.MapCircle(3, new Point(loc.X, loc.Y), Map.Map, Color.White);
					MapCross cross = new MapCross( 4, Color.White, new Point( loc.X, loc.Y ), Map.Map );

					Map.AddDrawObject( circle );
					Map.AddDrawObject( cross );
				}

				// Display the coordinates in the edit boxed
				InX.Text = loc.X.ToString();
				InY.Text = loc.Y.ToString();
				InZ.Text = loc.Z.ToString();				
			}
			else
			{
				// This is a parent node, disable stuff
				InX.Enabled = false;
				InY.Enabled = false;
				InZ.Enabled = false;
				ButtonSet.Enabled = false;
			}
		}

		private void ButtonZoomIn_Click(object sender, System.EventArgs e)
		{
			Map.ZoomIn();
		}

		private void ButtonZoomOut_Click(object sender, System.EventArgs e)
		{
			Map.ZoomOut();
		}

		private bool IsDuplicate( TreeNode node, string text, bool NodeIsParent )
		{
			TreeNode pNode = null;

			if ( !NodeIsParent )
				pNode = node.Parent;
			else
				pNode = node;

			TreeNodeCollection nodes = null;

			if ( pNode == null )
				nodes = TreeCat.Nodes;
			else
				nodes = pNode.Nodes;

			foreach ( TreeNode cNode in nodes )
			{
				if ( !cNode.Equals( node ) )
				{
					// Compare vs other nodes
					if ( text.ToLower() == cNode.Text.ToLower() )
					{
						if ( NodeIsParent )
							// In this case no duplicates at all
							return true;

						// Check if they are the same type of node
						if ( node.Tag is Loc )
						{
							if ( cNode.Tag is Loc )
								return true;
						}
						else
						{
							if ( !( cNode.Tag is Loc ) )
								return true;
						}
					}
				}
			}
			return false;
		}

		private void TreeCat_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			if ( e.Label == null )
				return; // Not actually modified

			if ( e.Label.Length == 0 )
			{
				Status.Text = "Category and location names cannot be empty";
				e.CancelEdit = true;
				return;
			}

			// Make sure this isn't a duplicate
			if ( IsDuplicate( e.Node, e.Label, false ) )
			{
				Status.Text = "The name " + e.Label + " is a duplicate, therefore not acceptable";
				e.CancelEdit = true;
				return;
			}

			// Finally update the location
			if ( e.Node.Tag is Loc )
				((Loc)e.Node.Tag).Name = e.Label;

			IsModified = true;
		}

		private void Map_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			int x = Map.ControlToMapX( e.X );
			int y = Map.ControlToMapY( e.Y );

			if ( SettingPoint )
			{
				// Get a new point
				InX.Text = x.ToString();
				InY.Text = y.ToString();
				InZ.Text = Map.GetMapHeight( new Point( x, y ) ).ToString();
				Map.RemoveAllDrawObjects();

				MapCircle circle = new MapCircle( 3, new Point( x, y ), Map.Map, Color.White );
				MapCross cross = new MapCross( 5, Color.White, new Point( x, y ), Map.Map );

				Map.AddDrawObject( circle );
				Map.AddDrawObject( cross );

				// Make color of button normal
				ButtonSet.BackColor = SystemColors.Control;

				// Make location defined
				((Loc)TreeCat.SelectedNode.Tag).IsDefined = true;
				((Loc)TreeCat.SelectedNode.Tag).X = x;
				((Loc)TreeCat.SelectedNode.Tag).Y = y;
				((Loc)TreeCat.SelectedNode.Tag).Z = Map.GetMapHeight( new Point( x,y ) );

				// End setting action
				SettingPoint = false;

				IsModified = true;

				return;
			}
			
			Map.Center = new Point( x, y );
		}

		private short GetMaxSize( bool horizontal )
		{
			switch ( Map.Map )
			{
                case Maps.Dungeon: return (short)(horizontal ? 7168 : 4096);
                case Maps.Sosaria: return (short)(horizontal ? 12288: 8192);
                case Maps.Ilshenar:return (short)(horizontal ? 2304 : 1600);
                case Maps.Malas:   return (short)(horizontal ? 2560 : 2048);
                case Maps.Tokuno:  return (short)(horizontal ? 1448 : 1448);
                case Maps.TerMur:  return (short)(horizontal ? 1280 : 4096);
			}
			return 0;
		}

		private void UpdateData()
		{
			short x = -1;
			short y = -1;
			short z = -1;

			bool xOk = true;
			bool yOk = true;
			bool zOk = true;

			try
			{
				x = System.Convert.ToInt16( InX.Text );
				if ( x != ((Loc)TreeCat.SelectedNode.Tag).X )
				{
					if ( ( x >= 0 ) && ( x <= GetMaxSize(true) ) )
					{
						// Update
						((Loc)TreeCat.SelectedNode.Tag).X = x;
						IsModified = true;
					}
					else
						xOk = false;
				}
			}
			catch ( Exception )
			{
				xOk = false;
			}

			try
			{
				y = System.Convert.ToInt16( InY.Text );
				if ( y != ((Loc)TreeCat.SelectedNode.Tag).Y )
				{
					if ( ( y >= 0 ) && ( y <= GetMaxSize(false) ) )
					{
						// Update
						((Loc)TreeCat.SelectedNode.Tag).Y = y;
						IsModified = true;
					}
					else
						yOk = false;
				}
			}
			catch ( Exception )
			{
				yOk = false;
			}

			try
			{
				z = System.Convert.ToInt16( InZ.Text );
				if ( z != ((Loc)TreeCat.SelectedNode.Tag).Z )
				{
					if ( ( z > - 700 ) && ( z < 700 ) )
					{
						// Update
						((Loc)TreeCat.SelectedNode.Tag).Z = z;
						IsModified = true;
					}
					else
						zOk = false;
				}
			}
			catch ( Exception )
			{
				zOk = false;
			}

			if ( ! ( zOk && yOk && xOk ) )
			{
				if ( (InX.Text.Length == 0 ) && ( InY.Text.Length == 0 ) && ( InZ.Text.Length == 0 ) && !((Loc)TreeCat.SelectedNode.Tag).IsDefined )
				{
					// Don't display an error because this is undefined
					return;
				}
				// Errors
				Status.Text = "Some of the coordinates you have modified haven't been accepted as corrected. Double click this box to view the location you were editing.";
				Status.Tag = TreeCat.SelectedNode;
			}
			else
			{
				if ( ( x > 0 ) || ( y > 0 ) || ( z > 0 ) )
				{
					((Loc)TreeCat.SelectedNode.Tag).IsDefined = true;
					Status.Tag = null;
				}
			}
		}

		private void Status_DoubleClick(object sender, System.EventArgs e)
		{
			if ( Status.Tag is TreeNode )
			{
				try
				{
					TreeCat.SelectedNode = (TreeNode) Status.Tag;
					Status.Tag = null;
					Status.Text = "";
				}
				catch ( Exception )
				{
					Status.Text = "The node you were editing has been deleted";
					Status.Tag = null;
				}
			}
		}

		private void ButtonSet_Click(object sender, System.EventArgs e)
		{
			if ( !SettingPoint )
			{
				SettingPoint = true;
				ButtonSet.BackColor = SystemColors.ControlDark;
			}
			else
			{
				SettingPoint = false;
				ButtonSet.BackColor = SystemColors.Control;
			}
		}

		private void TreeCat_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if ( e.KeyCode == Keys.F2 )
			{
				// Edit the label
				TreeNode node = TreeCat.SelectedNode;

				if ( node != null )
					// Edit it
					node.BeginEdit();
			}
		}

		private void ButtonDel_Click(object sender, System.EventArgs e)
		{
			TreeNode node = TreeCat.SelectedNode;
 
			if ( node != null )
			{
				string message = "Are you sure you want to delete " + node.Text + "?";

				if ( node.Nodes.Count > 0 )
					message = "Are you sure you want to delete the category " + node.Text + " and all of its sub-items?";
					
				// There's a node
				if ( MessageBox.Show( this,
					message,
					"Deleting item",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question ) == DialogResult.Yes )
				{
					// Delete node
					Status.Text = node.Text + " deleted";
					TreeNode pNode = node.Parent;

					TreeCat.Nodes.Remove( node );

					IsModified = true;

					// Verify if parent has no more nodes
					if ( pNode.Nodes.Count == 0 )
					{
						if ( !( pNode.Tag is Loc ) && ( pNode.Parent != null ) )
						{
							pNode.Tag = new Loc( pNode.Text );

							// Enable stuff
							InX.Enabled = true;
							InY.Enabled = true;
							InZ.Enabled = true;
							ButtonSet.Enabled = true;
						}
					}
				}	
			}
		}

		private void ButtonAdd_Click(object sender, System.EventArgs e)
		{
			string name = Input.Text;

			if ( name.Length == 0 )
			{
				Status.Text = "The name for the new item cannot be empty";
				return;
			}

			TreeNode pNode = TreeCat.SelectedNode;

			if ( pNode == null )
			{
				if ( TreeCat.Nodes.Count == 0 )
				{
					// Empty tree, add the first item
					Loc nLoc = new Loc( name  );
					TreeNode nNode = new TreeNode( name );
					Facet = name;
					nNode.Tag = nLoc;

					TreeCat.Nodes.Add( nNode );
					TreeCat.SelectedNode = nNode;

					IsModified = true;
					return;
				}

				Status.Text = "Please select a category first";
				return;
			}

			if ( pNode.Tag is Loc )
			{
				if ( ((Loc)pNode.Tag).IsDefined )
				{
					Status.Text = "You can't add sub-items to a location. Please choose a category instead.";
					return;
				}
			}

			// Check for duplicates
			if ( IsDuplicate( pNode, name, true ) )
			{
				Status.Text = "An item named " + name + " already exists in the category " + pNode.Text;
				return;
			}

			// Ok we're adding a child. First set parent tag to null
			pNode.Tag = null;

			Loc NewLoc = new Loc( name  );
			TreeNode NewNode = new TreeNode( name );
			NewNode.Tag = NewLoc;

			pNode.Nodes.Add( NewNode );
			TreeCat.SelectedNode = NewNode;

			if ( ( pNode.Nodes.Count > 1 ) && ( pNode.Parent != null ) )
				TreeCat.SelectedNode = pNode;

			IsModified = true;
		}

		private void MenuOptions_Popup(object sender, System.EventArgs e)
		{
			// Topmost
			if ( this.TopMost )
				OptionsTopmost.Checked = true;
			else
				OptionsTopmost.Checked = false;

			// Draw statics
			OptionsDrawStatics.Checked = Map.DrawStatics;

			// Mapfile

			// First reset all
			Map0.Checked = false;
			Map1.Checked = false;
			Map2.Checked = false;
			Map3.Checked = false;

			switch ( Map.Map )
			{
				case Maps.Dungeon:  Map0.Checked = true; break;
				case Maps.Sosaria:  Map1.Checked = true; break;
				case Maps.Ilshenar: Map2.Checked = true; break;
				case Maps.Malas:    Map3.Checked = true; break;
			}
		}

		private void OptionsTopmost_Click(object sender, System.EventArgs e)
		{
			this.TopMost = !this.TopMost;
		}

		private void Map0_Click(object sender, System.EventArgs e)
		{
			Map.Map = Maps.Dungeon;
		}

		private void Map1_Click(object sender, System.EventArgs e)
		{
			Map.Map = Maps.Sosaria;
		}

		private void Map2_Click(object sender, System.EventArgs e)
		{
			Map.Map = Maps.Ilshenar;
		}

		private void Map3_Click(object sender, System.EventArgs e)
		{
			Map.Map = Maps.Malas;
		}

		private void ClearAll()
		{
			TreeCat.Nodes.Clear();

			InX.Text = "";
			InX.Enabled = false;

			InY.Text = "";
			InY.Enabled = false;

			InZ.Text = "";
			InZ.Enabled = false;

			ButtonSet.Enabled = false;

			IsModified = false;

			Map.RemoveAllDrawObjects();

			CurrentFile = "";

			Status.Text = "";
		}

		private DialogResult ModifiedConfirmation()
		{
			DialogResult dr =  MessageBox.Show( this,
				"The current document has been modified. Would you like to save the changes?", "Save changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );

			if ( dr == DialogResult.Yes )
			{
				// Save
				if ( SaveFile.ShowDialog() == DialogResult.OK )
				{
					CurrentFile = SaveFile.FileName;
					Save();
					return DialogResult.OK;
				}
				else
					return DialogResult.Cancel;
			}

			if ( dr == DialogResult.No )
				return DialogResult.OK;

			return dr;
		}

		private void Save()
		{
			XmlDocument dom = new XmlDocument();

			XmlNode Decl = dom.CreateNode( XmlNodeType.XmlDeclaration, "", "" );
			dom.AppendChild( Decl );

			XmlNode Root = dom.CreateNode( XmlNodeType.Element, "places", "" );
			dom.AppendChild( Root );

			XmlNode FacetNode = dom.CreateNode( XmlNodeType.Element, "parent", "" );
			XmlAttribute attr = dom.CreateAttribute( "name" );
			attr.Value = TreeCat.Nodes[0].Text;
			FacetNode.Attributes.Append( attr );
			
			foreach ( TreeNode tNode in TreeCat.Nodes[0].Nodes )
				FacetNode.AppendChild( MakeNode( tNode, dom ) );

			Root.AppendChild( FacetNode );

			dom.Save( CurrentFile );

			IsModified = false;
		}

		private XmlNode MakeNode( TreeNode tNode, XmlDocument dom )
		{
			if ( tNode.Nodes.Count == 0 )
			{
				// This is a data node
				Loc loc = (Loc) tNode.Tag;
				return loc.GetXmlNode( dom );
			}

			XmlNode xNode = dom.CreateNode( XmlNodeType.Element, "parent", "" );
			XmlAttribute xAttr = dom.CreateAttribute( "name" );
			xAttr.Value = tNode.Text;
			xNode.Attributes.Append( xAttr );

			foreach ( TreeNode node in tNode.Nodes )
				xNode.AppendChild( MakeNode( node, dom ) );

			return xNode;
		}

		private void FileNew_Click(object sender, System.EventArgs e)
		{
			if ( IsModified )
			{
				if ( ModifiedConfirmation() == DialogResult.Cancel )
					return;
			}

			ClearAll();

			Text = TitleText;
		}

		private void FileSave_Click(object sender, System.EventArgs e)
		{
			if ( CurrentFile.Length == 0 )
				FileSaveAs_Click( sender, e );
			else
				Save();
		}

		private void FileSaveAs_Click(object sender, System.EventArgs e)
		{
			if ( SaveFile.ShowDialog() == DialogResult.OK )
			{
				CurrentFile = SaveFile.FileName;
				Save();
			}
		}

		private void FileExit_Click(object sender, System.EventArgs e)
		{
			if ( !IsModified )
				Close();

			if ( this.ModifiedConfirmation() == DialogResult.OK )
				this.Close();
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			if ( FolderBrowse.ShowDialog() == DialogResult.OK )
			{
				// Change the folder
				Map.MulManager.CustomFolder = FolderBrowse.SelectedPath;
				m_UOFolder = FolderBrowse.SelectedPath;
   			}
		}

		private void HelpSite_Click(object sender, System.EventArgs e)
		{
			System.Diagnostics.Process.Start( "http://arya.distanthost.com/" );
		}

		private void HelpAbout_Click(object sender, System.EventArgs e)
		{
			AboutForm about = new AboutForm();
			about.ShowDialog();
		}

		private void OptionsDrawStatics_Click(object sender, System.EventArgs e)
		{
			Map.DrawStatics = ! Map.DrawStatics;
		}

		private void OptionsGoCommand_Click(object sender, System.EventArgs e)
		{
			InputBox iBox = new InputBox( "Please enter the command used to go to a location on your server. This is [Go for the default RunUO distribution.", m_GoCommand );

			if ( iBox.ShowDialog() == DialogResult.OK )
				m_GoCommand = iBox.Input;
		}

		private void ButtonGo_Click(object sender, System.EventArgs e)
		{
			TreeNode node = null;

			try
			{
				node = TreeCat.SelectedNode;
			}
			catch
			{
				Status.Text = "There's no location selected";
				return;
			}

			if ( node.Parent == null )
			{
				Status.Text = "Please select a location. You can't teleport to the facet";
				return;
			}

			if ( node.Nodes.Count > 0 )
			{
				Status.Text = "Please select a location, not a category";
				return;
			}

			if ( ! ( node.Tag is Loc ) )
			{
				Status.Text = "That isn't a valid location";
				return;
			}

			Loc loc = node.Tag as Loc;

			if ( !loc.IsDefined )
			{
				Status.Text = "Please set a destination point first";
				return;
			}

			string msg = string.Format( "{0} {1} {2} {3}\n", m_GoCommand, loc.X, loc.Y, loc.Z );

            Common.SendToUO(msg);
		}

		private string OptionsFile
		{
			get
			{
				string exeFile = this.GetType().Assembly.Location;
				string path = System.IO.Path.GetDirectoryName( exeFile );
				return System.IO.Path.Combine( path, "LocationEditorOptions.xml" );
			}
		}

		private void SaveOptions()
		{
			// Set the options
			Options.AlwaysOnTop = this.TopMost;
			Options.DrawStatics = Map.DrawStatics;
			Options.GoCommand = m_GoCommand;
			Options.UOFolder = m_UOFolder;
			Options.Map = Map.Map;
			Options.ZoomLevel = Map.ZoomLevel;
			Options.MapCenter = Map.Center;

			XmlSerializer serializer = new XmlSerializer(typeof( LocOptions ));
			StreamWriter writer = new StreamWriter( OptionsFile );
			serializer.Serialize(writer, ( LocOptions ) Options);
			writer.Close();
		}

		private void LoadOptions()
		{
			// Check if options file exists
			if ( System.IO.File.Exists( OptionsFile ) )
			{
				// File exists read it
				XmlSerializer serializer = new XmlSerializer(typeof(LocOptions));
				FileStream theStream = new FileStream( OptionsFile, FileMode.Open );
				Options = (LocOptions) serializer.Deserialize( theStream );
				theStream.Close();

				// Apply options
				Map.DrawStatics = Options.DrawStatics;
				Map.Map = Options.Map;
				Map.ZoomLevel = Options.ZoomLevel;
				Map.Center = Options.MapCenter;
				if ( Options.UOFolder != "" )
					Map.MulManager.CustomFolder = Options.UOFolder;
				this.TopMost = Options.AlwaysOnTop;
				m_GoCommand = Options.GoCommand;
			}
		}

		private void LocationEditor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			SaveOptions();
		}

		private void HelpHelp_Click(object sender, System.EventArgs e)
		{
			// Get file name
			string exeFile = this.GetType().Assembly.Location;
			string path = Path.GetDirectoryName( exeFile );
			string helpFile = Path.Combine( path, "help.htm" );

			if ( ! File.Exists( helpFile ) )
			{
				MessageBox.Show( "Couldn't locate the help file. Reinstalling this software will solve this issue." );
				return;
			}

			System.Diagnostics.Process.Start( helpFile );
		}

		private void Map4_Click(object sender, System.EventArgs e)
		{
			Map.Map = Maps.Tokuno;
		}
	}
}