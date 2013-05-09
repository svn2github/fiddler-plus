using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Collections;
using FiddlerControls.RegionEditor.MapViewer;

namespace FiddlerControls.RegionEditor.Locations
{
    public partial class TabLocations : UserControl
    {
        private LocationTree m_Locations;
        public class LocationTree
        {
            public string FilePath
            {
                get { return m_FilePath; }
                set { m_FilePath = value; }
            }
            private string m_FilePath;

            public LocationTree()
            {
                m_Root = new ParentNode(null, "Locations");
            }

            public LocationTree(string filePath)
            {
                m_FilePath = filePath;
                XmlTextReader xml = new XmlTextReader(new FileStream(m_FilePath, FileMode.Open));
                xml.WhitespaceHandling = WhitespaceHandling.None;

                m_Root = Parse(xml);

                xml.Close();
            }

            public ParentNode Root
            {
                get { return m_Root; }
            }
            private ParentNode m_Root;

            private ParentNode Parse(XmlTextReader xml)
            {
                xml.Read();
                xml.Read();
                //xml.Read();

                return new ParentNode(xml, null);
            }

            public void Save(string filePath = null)
            {
                if (!String.IsNullOrEmpty(filePath))
                    m_FilePath = filePath;
                XmlTextWriter xml = new XmlTextWriter(new FileStream(m_FilePath, FileMode.Create), Encoding.UTF8);
                xml.WriteStartDocument(true);
                m_Root.Save(xml);
                xml.Flush();
                xml.Close();
            }
        }

        public class ParentNode
        {
            public ParentNode(ParentNode parent, string name = "empty") : this(parent, name, Color.FromArgb(0x08, 0x08, 0x08))
            {
            }

            public ParentNode(ParentNode parent, string name, Color color)
            {
                m_Parent = parent;
                m_Children = new object[0];
                m_Name = name;
                m_Color = color;
            }

            public ParentNode(XmlTextReader xml, ParentNode parent)
            {
                m_Parent = parent;

                Parse(xml);
            }

            private void Parse(XmlTextReader xml)
            {
                if (xml.MoveToAttribute("color"))
                    m_Color = Color.FromArgb(Convert.ToInt32(xml.Value));
                else
                    m_Color = Color.FromArgb(0x08, 0x08, 0x08);

                if (xml.MoveToAttribute("name"))
                    m_Name = xml.Value;
                else
                    m_Name = "empty";

                if (xml.IsEmptyElement)
                {
                    m_Children = new object[0];
                }
                else
                {
                    ArrayList children = new ArrayList();

                    while (xml.Read() && xml.NodeType == XmlNodeType.Element)
                    {
                        if (xml.Name == "child")
                            children.Add(new ChildNode(xml, this));
                        else
                            children.Add(new ParentNode(xml, this));
                    }

                    m_Children = children.ToArray();
                }
            }

            public void Save(XmlTextWriter xml, int tab = 0)
            {
                string tabulation = String.Empty;
                for (int i = 0; i < tab; ++i)
                    tabulation += "  "; //"t"
                xml.WriteWhitespace(Environment.NewLine);
                if (!String.IsNullOrEmpty(tabulation))
                    xml.WriteWhitespace(tabulation);                    
                xml.WriteStartElement(m_Parent == null ? "places" : "parent");
                xml.WriteAttributeString("color", m_Color.ToArgb().ToString());
                xml.WriteAttributeString("name", m_Name);
                foreach (object сhildren in m_Children)
                    if (сhildren is ParentNode)
                        (сhildren as ParentNode).Save(xml, tab+1);
                    else if (сhildren is ChildNode)
                        (сhildren as ChildNode).Save(xml, tab+1);
                xml.WriteWhitespace(Environment.NewLine);
                if (!String.IsNullOrEmpty(tabulation))
                    xml.WriteWhitespace(tabulation);
                xml.WriteEndElement();
            }

            public ParentNode Parent
            {
                get { return m_Parent; }
                set { m_Parent = value; }
            }
            private ParentNode m_Parent;

            public object[] Children
            {
                get { return m_Children; }
                set { m_Children = value; }
            }
            private object[] m_Children;

            public Color Color
            {
                get { return m_Color; }
                set { m_Color = value; }
            }
            private Color m_Color;

            public string Name
            {
                get { return m_Name; }
                set { m_Name = value; }
            }
            private string m_Name;
        }

        public class ChildNode
        {
            public ChildNode(XmlTextReader xml, ParentNode parent)
            {
                m_Parent = parent;

                Parse(xml);
            }

            private void Parse(XmlTextReader xml)
            {
                if (xml.MoveToAttribute("map"))
                    m_Map = (Maps)Convert.ToInt32(xml.Value);

                m_X = m_Y = m_Z = 0;
                if (xml.MoveToAttribute("x"))
                    m_X = Convert.ToInt32(xml.Value);
                if (xml.MoveToAttribute("y"))
                    m_Y = Convert.ToInt32(xml.Value);
                if (xml.MoveToAttribute("z"))
                    m_Z = Convert.ToInt32(xml.Value);

                if (xml.MoveToAttribute("color"))
                    m_Color = Color.FromArgb(Convert.ToInt32(xml.Value));
                else
                    m_Color = Color.FromArgb(0x08, 0x08, 0x08);

                if (xml.MoveToAttribute("name"))
                    m_Name = xml.Value;
                else
                    m_Name = "empty";
            }

            public void Save(XmlTextWriter xml, int tab = 0)
            {
                string tabulation = String.Empty;
                for (int i = 0; i < tab; ++i)
                    tabulation += "  "; //"t"
                xml.WriteWhitespace(Environment.NewLine);
                if (!String.IsNullOrEmpty(tabulation))
                    xml.WriteWhitespace(tabulation);
                xml.WriteStartElement("child");
                xml.WriteAttributeString("map", ((int)Map).ToString());
                xml.WriteAttributeString("x", m_X.ToString());
                xml.WriteAttributeString("y", m_Y.ToString());
                xml.WriteAttributeString("z", m_Z.ToString());
                xml.WriteAttributeString("color", m_Color.ToArgb().ToString());
                xml.WriteAttributeString("name", m_Name);
                xml.WriteEndElement();
            }

            public ParentNode Parent
            {
                get { return m_Parent; }
                set { m_Parent = value; }
            }
            private ParentNode m_Parent;

            public Color Color
            {
                get { return m_Color; }
                set { m_Color = value; }
            }
            private Color m_Color;

            public string Name
            {
                get { return m_Name; }
                set { m_Name = value; }
            }
            private string m_Name;

            public Maps Map
            {
                get { return m_Map; }
                set { m_Map = value; }
            }
            private Maps m_Map;

            public int X
            {
                get { return m_X; }
                set { m_X = value; }
            }
            private int m_X;

            public int Y
            {
                get { return m_Y; }
                set { m_Y = value; }
            }
            private int m_Y;

            public int Z
            {
                get { return m_Z; }
                set { m_Z = value; }
            }
            private int m_Z;
        }

        

        public TabLocations()
        {
            InitializeComponent();
            
            mapViewer.DrawObjects = new List<IMapDrawable>();
            mapViewer.Map = Maps.Sosaria;
            mapViewer.DrawStatics = true;
            mapViewer.Center = new Point(150, 200);

            #if DEBUG
            // Предотвращаем падение конструктора
            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Data\Locations.xml"))) return;
            #endif

            locationsTreeView.BeginUpdate();
            locationsTreeView.Nodes.Clear();
            m_Locations = new LocationTree(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Data\Locations.xml"));
            RebuildTree(null);
            locationsTreeView.EndUpdate();
            if (locationsTreeView.Nodes != null && locationsTreeView.Nodes.Count > 0)
                locationsTreeView.SelectedNode = locationsTreeView.Nodes[0];

            openFileDialog.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Data");
            saveFileDialog.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Data"); 
        }

        private void RebuildTree(TreeNode parentNode)
        {
            object[] children = (parentNode == null) ? m_Locations.Root.Children
                : (parentNode.Tag is ParentNode) ? (parentNode.Tag as ParentNode).Children
                : null;

            if (children == null || children.Length < 1)
                return;

            for (int i = 0; i < children.Length; ++i)
            {
                // Create root node
                TreeNode treeNode = new TreeNode();

                if (children[i] is ParentNode)
                {
                    ParentNode node = children[i] as ParentNode;
                    treeNode.NodeFont = new Font(new FontFamily("Microsoft Sans Serif"), 9, FontStyle.Bold);
                    treeNode.ForeColor = node.Color;
                    treeNode.Text = node.Name;
                    treeNode.Tag = node;
                }
                else if (children[i] is ChildNode)
                {
                    ChildNode node = children[i] as ChildNode;
                    treeNode.NodeFont = new Font(new FontFamily("Microsoft Sans Serif"), 9, FontStyle.Regular);
                    treeNode.ForeColor = node.Color;
                    treeNode.Text = node.Name;
                    treeNode.Tag = node;
                }

                if (parentNode == null)
                    locationsTreeView.Nodes.Add(treeNode);
                else
                    parentNode.Nodes.Add(treeNode);
                RebuildTree(treeNode);
            }
        }

        private void locationsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            object tag = locationsTreeView.SelectedNode.Tag;
            if (tag == null)
            {
                nameTextBox.Enabled = false;
                colorButton.Enabled = false;
                mapComboBox.Enabled = false;
                xNumericUpDown.Enabled = false;
                yNumericUpDown.Enabled = false;
                zNumericUpDown.Enabled = false;

                return;
            }
            else if (tag is ParentNode)
            {
                ParentNode node = tag as ParentNode;
                colorButton.Enabled = true;
                colorButton.BackColor = node.Color;
                nameTextBox.Text = node.Name;

                mapComboBox.Enabled = false;
                mapComboBox.SelectedIndex = -1;
                xNumericUpDown.Enabled = false;
                yNumericUpDown.Enabled = false;
                zNumericUpDown.Enabled = false;      

            }
            else if (tag is ChildNode)
            {
                ChildNode node = tag as ChildNode;

                xNumericUpDown.Maximum = MapSizes.GetSize((int)node.Map).Width;
                yNumericUpDown.Maximum = MapSizes.GetSize((int)node.Map).Height;
                colorButton.Enabled = true;
                colorButton.BackColor = node.Color;
                nameTextBox.Text = node.Name;

                mapComboBox.Enabled = true; 
                mapComboBox.SelectedIndex = (int)node.Map;
                xNumericUpDown.Enabled = true;
                xNumericUpDown.Value = node.X;
                yNumericUpDown.Enabled = true;
                yNumericUpDown.Value = node.Y;
                zNumericUpDown.Enabled = true;  
                zNumericUpDown.Value = node.Z;
            }
        }

        private void locationsTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is ChildNode)
            {
                ChildNode node = e.Node.Tag as ChildNode;
                SetLocation(node.Map, node.X, node.Y, node.Z);
            }
        }

        public void SetLocation (Maps map, int x, int y, int z = 128)
        {
            if (mapViewer.Map != map)
            {
                mapViewer.Map = map;
                map0DungeonToolStripMenuItem.Checked  = false;
                map1SosariaToolStripMenuItem.Checked  = false;
                map2IlshinarToolStripMenuItem.Checked = false;
                map3MalasToolStripMenuItem.Checked    = false;
                map4TokunoToolStripMenuItem.Checked   = false;
                map5TerMurToolStripMenuItem.Checked   = false;
                switch(map)
                {
                    case Maps.Dungeon : map0DungeonToolStripMenuItem.Checked  = true;  break;
                    case Maps.Sosaria:  map1SosariaToolStripMenuItem.Checked  = true;  break;
                    case Maps.Ilshenar: map2IlshinarToolStripMenuItem.Checked = true;  break;
                    case Maps.Malas:    map3MalasToolStripMenuItem.Checked    = true;  break;
                    case Maps.Tokuno:   map4TokunoToolStripMenuItem.Checked   = true;  break;
                    case Maps.TerMur:   map5TerMurToolStripMenuItem.Checked   = true;  break;
                }
            }
            Point point = new Point(x, y);
            if (mapViewer.Center != point)
                mapViewer.Center = point;
        }
    
        public void SendClient(Maps map, int x, int y, int z = 128)
        {
            Ultima.Client.SendText(String.Format("[self set map {0} X {1} Y {2} Z {3}", map, x, y, z));
        }

        private void l_xNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (locationsTreeView.SelectedNode.Tag is ChildNode)
            {
                ChildNode node = locationsTreeView.SelectedNode.Tag as ChildNode;
                node.X = (int)xNumericUpDown.Value;
            }
        }

        private void l_yNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (locationsTreeView.SelectedNode.Tag is ChildNode)
            {
                ChildNode node = locationsTreeView.SelectedNode.Tag as ChildNode;
                node.Y = (int)yNumericUpDown.Value;
            }
        }

        private void l_zNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (locationsTreeView.SelectedNode.Tag is ChildNode)
            {
                ChildNode node = locationsTreeView.SelectedNode.Tag as ChildNode;
                node.Z = (int)zNumericUpDown.Value;
            }
        }

        private void l_mapComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (locationsTreeView.SelectedNode.Tag is ChildNode)
            {
                ChildNode node = locationsTreeView.SelectedNode.Tag as ChildNode;
                node.Map = (Maps)mapComboBox.SelectedIndex;
            }
        }

        private void l_colorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog(this) == DialogResult.OK)
            {
                colorButton.BackColor = colorDialog.Color;
                if (locationsTreeView.SelectedNode.Tag is ChildNode)
                    (locationsTreeView.SelectedNode.Tag as ChildNode).Color = colorDialog.Color;
                else if (locationsTreeView.SelectedNode.Tag is ParentNode)
                    (locationsTreeView.SelectedNode.Tag as ParentNode).Color = colorDialog.Color;
                locationsTreeView.SelectedNode.ForeColor = colorDialog.Color;
            }
        }

        private void scaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scale03ToolStripMenuItem.Checked = false;
            scale02ToolStripMenuItem.Checked = false;
            scale01ToolStripMenuItem.Checked = false;
            scale0ToolStripMenuItem.Checked  = false;
            scale1ToolStripMenuItem.Checked  = false;
            scale2ToolStripMenuItem.Checked  = false;
            scale3ToolStripMenuItem.Checked  = false;
            scale4ToolStripMenuItem.Checked  = false;
            if (sender is ToolStripMenuItem)
            {
                mapViewer.ZoomLevel = Convert.ToInt32((string)(sender as ToolStripMenuItem).Tag);
                (sender as ToolStripMenuItem).Checked = true;
            }   
        }

        private void mapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                Maps map = (Maps)Enum.Parse(typeof(Maps), (string)(sender as ToolStripMenuItem).Tag);
                SetLocation(map, MapSizes.GetSize(map).Width / 2, MapSizes.GetSize(map).Height / 2);
            }

        }

        private bool m_SettingPoint;

        private void mapViewer_MouseDown(object sender, MouseEventArgs e)
        {
            int x = mapViewer.ControlToMapX(e.X);
            int y = mapViewer.ControlToMapY(e.Y);

			if ( m_SettingPoint )
			{
                xNumericUpDown.Value = x;
                yNumericUpDown.Value = y;
                zNumericUpDown.Value = mapViewer.GetMapHeight(new Point(x, y));
                mapViewer.RemoveAllDrawObjects();

				MapCircle circle = new MapCircle( 3, new Point( x, y ), mapViewer.Map, Color.White );
				MapCross cross = new MapCross( 5, Color.White, new Point( x, y ), mapViewer.Map );

                mapViewer.AddDrawObject(circle);
                mapViewer.AddDrawObject(cross);

				// Make color of button normal
				//ButtonSet.BackColor = SystemColors.Control;

				// Make location defined
				//((Loc)TreeCat.SelectedNode.Tag).IsDefined = true;
				//((Loc)TreeCat.SelectedNode.Tag).X = x;
				//((Loc)TreeCat.SelectedNode.Tag).Y = y;
				//((Loc)TreeCat.SelectedNode.Tag).Z = Map.GetMapHeight( new Point( x,y ) );

				// End setting action
                m_SettingPoint = false;

				//IsModified = true;

				return;
			}

            mapViewer.Center = new Point(x, y);
        }

        private void goToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(locationsTreeView.SelectedNode.Tag is ChildNode))
                return;
            ChildNode node = locationsTreeView.SelectedNode.Tag as ChildNode;
            SendClient(node.Map, node.X, node.Y, node.Z);
        }

        private void addGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParentNode node = null;
            if (locationsTreeView.SelectedNode.Tag is ParentNode)
                node = (locationsTreeView.SelectedNode.Tag as ParentNode).Parent;
            if (locationsTreeView.SelectedNode.Tag is ChildNode)
                node = (locationsTreeView.SelectedNode.Tag as ChildNode).Parent;

            int index = locationsTreeView.SelectedNode.Index;
            
            object[] children = node.Children;
            Array.Resize<object>(ref children, node.Children.Length + 1);
            Array.Copy(node.Children, index, children, index + 1, node.Children.Length - index);
            node.Children = children;

            ParentNode newnode = new ParentNode(node);
            node.Children[index] = newnode;

            locationsTreeView.BeginUpdate();
            TreeNode treeNode = new TreeNode();

            treeNode.Text = newnode.Name;
            treeNode.ForeColor = newnode.Color;
            treeNode.Tag = newnode;

            locationsTreeView.SelectedNode.Parent.Nodes.Insert(index, treeNode);
            locationsTreeView.EndUpdate();
        }

        private void addNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #region File Operations 

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            locationsTreeView.BeginUpdate();
            locationsTreeView.Nodes.Clear();
            m_Locations = new LocationTree();
            RebuildTree(null);
            locationsTreeView.EndUpdate();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                locationsTreeView.BeginUpdate();
                locationsTreeView.Nodes.Clear();
                m_Locations = new LocationTree(openFileDialog.FileName);
                RebuildTree(null);
                locationsTreeView.EndUpdate();
                if (locationsTreeView.Nodes != null && locationsTreeView.Nodes.Count > 0)
                    locationsTreeView.SelectedNode = locationsTreeView.Nodes[0];
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(m_Locations.FilePath))
                m_Locations.Save();
            else
                saveAsToolStripMenuItem_Click(sender, e);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                m_Locations.Save(saveFileDialog.FileName);
        }

        #endregion


    }
}
