using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AdvancedDataGridView;

namespace UoFiddler
{
    public class OptionsTab : TabPage
    {
        public AdvancedDataGridView.TreeGridView m_TreeGridView;
        private AdvancedDataGridView.TreeGridColumn Node = new AdvancedDataGridView.TreeGridColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn Value = new System.Windows.Forms.DataGridViewTextBoxColumn();

        private AdvancedDataGridView.TreeGridColumn TreeGrid = new TreeGridColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn NameGrid = new DataGridViewTextBoxColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn ValueGrid = new DataGridViewTextBoxColumn();
        private AdvancedDataGridView.TreeGridColumn TreeGridColumn = new TreeGridColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn DataGridColumn = new DataGridViewTextBoxColumn();

        private DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();

        

        public OptionsTab(TabControl tabControl) : base()
        {
            Name = "Options";
            Text = "_Options";
            Tag  = "Options";

            m_TreeGridView = new TreeGridView();
            m_TreeGridView.Dock = DockStyle.Fill;
            //m_TreeGridView.Columns.Add("1", "2");
            //m_TreeGridView.Columns.Add("3", "4");


            
            Node.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            Node.DefaultNodeImage = null;
            Node.HeaderText = "Имя";
            Node.Name = "Node";
            Node.ReadOnly = true;
            Node.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            
            Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            Value.HeaderText = "Значение";
            Value.MinimumWidth = 50;
            Value.Name = "Value";
            Value.ReadOnly = false;
            Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            Value.Width = 50;
            
            /*
            this.TreeGridColumn.DefaultNodeImage = null;
            this.TreeGridColumn.HeaderText = "TreeGridColumn";
            this.TreeGridColumn.Name = "TreeGridColumn";
            this.TreeGridColumn.ReadOnly = true;
            this.TreeGridColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            this.DataGridColumn.HeaderText = "DataGridColumn";
            this.DataGridColumn.Name = "DataGridColumn";
            this.DataGridColumn.ReadOnly = true;
            this.DataGridColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            this.TreeGrid.DefaultNodeImage = null;
            this.TreeGrid.HeaderText = "TreeGrid";
            this.TreeGrid.Name = "TreeGrid";
            this.TreeGrid.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            this.NameGrid.HeaderText = "NameGrid";
            this.NameGrid.Name = "NameGrid";
            this.NameGrid.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            this.ValueGrid.HeaderText = "ValueGrid";
            this.ValueGrid.Name = "ValueGrid";
            this.ValueGrid.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            */

            m_TreeGridView.AllowUserToAddRows = false;
            m_TreeGridView.AllowUserToDeleteRows = false;
            m_TreeGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            m_TreeGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            m_TreeGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            m_TreeGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            m_TreeGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            m_TreeGridView.ColumnHeadersVisible = true;
            m_TreeGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Node,
            this.Value});
            dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(245)))), ((int)(((byte)(248)))));
            dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            m_TreeGridView.DefaultCellStyle = dataGridViewCellStyle;
            m_TreeGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            m_TreeGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            //m_TreeGridView.ImageList = this.imageList;
            m_TreeGridView.Location = new System.Drawing.Point(3, 16);
            m_TreeGridView.Name = "m_TreeGridView";
            m_TreeGridView.ReadOnly = false;
            m_TreeGridView.RowHeadersVisible = false;
            m_TreeGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            m_TreeGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            m_TreeGridView.ShowLines = false;
            m_TreeGridView.Size = new System.Drawing.Size(826, 399);
            m_TreeGridView.TabIndex = 0;
            //m_TreeGridView.NodeExpanding += new AdvancedDataGridView.ExpandingEventHandler(this.treeGridView1_NodeExpanding);
            //m_TreeGridView.NodeCollapsing += new AdvancedDataGridView.CollapsingEventHandler(this.treeGridView1_NodeCollapsing); 

            this.GotFocus += new EventHandler(OptionsTab_GotFocus);
            m_TreeGridView.CellClick += new DataGridViewCellEventHandler(m_TreeGridView_CellClick);
            this.Controls.Add(m_TreeGridView);
            tabControl.TabPages.Add(this);

            InitNodeList();
        }

        private static ColorDialog dialogColor = null;

        void m_TreeGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                MessageBox.Show(e.RowIndex.ToString() + " | " + e.ColumnIndex.ToString());

                TreeGridNode node = m_TreeGridView.Nodes[e.RowIndex];
                DataGridViewCell cell = m_TreeGridView.Nodes[e.RowIndex].Cells[1];
                
                object oldvalue = cell.Value;
                if(cell.ValueType == typeof(Color))
                {
                    if (dialogColor == null)
                    {
                        dialogColor = new ColorDialog();
                        dialogColor.AnyColor = true;
                        dialogColor.FullOpen = true;
                        dialogColor.SolidColorOnly = true;
                    }

                    dialogColor.Color = (Color)cell.Value;
                    if (dialogColor.ShowDialog() == DialogResult.OK)
                        if(oldvalue != (object)dialogColor.Color)
                        {
                            cell.Value = dialogColor.Color;
                            // действие
                        }

                }
                else
                    MessageBox.Show(e.RowIndex.ToString() + " | " + e.ColumnIndex.ToString());

            }
        }

        void OptionsTab_GotFocus(object sender, EventArgs e)
        {
            InitNodeList();
        }


        Dictionary<String, TreeGridNode> nodeDict = new Dictionary<String, TreeGridNode>();

        private void InitNodeList()
        {
            TreeGridNode rootNode, groupNode, valueNode;
            Font boldFont = new Font(m_TreeGridView.DefaultCellStyle.Font, FontStyle.Bold);

            // Настройки консоли и телнета
            rootNode = m_TreeGridView.Nodes.Add("telnet (интерфейса сетевого виртуального терминала)");
            rootNode.DefaultCellStyle.Font = boldFont;
            rootNode.ImageIndex = -1;
            rootNode.Cells[1].ValueType = typeof (Color);
            rootNode.Cells[1].Value = Color.Blue;

            groupNode = rootNode.Nodes.Add("подключение");
            groupNode.DefaultCellStyle.Font = boldFont;
            groupNode.ImageIndex = -1;

            valueNode = groupNode.Nodes.Add("сервер", "login.uoquint.ru");
            //nodeDict.Add("сервер", valueNode);
            valueNode.ImageIndex = -1;

            //valueNode = groupNode.Nodes.Add("порт", "2592");
            //nodeDict.Add("сервер", valueNode);
            //valueNode.ImageIndex = -1;

            valueNode = groupNode.Nodes.Add("аккаунт", "StaticGM");
            //nodeDict.Add("сервер", valueNode);
            valueNode.ImageIndex = -1;

            valueNode = groupNode.Nodes.Add("пароль", "please type password heare");
            //nodeDict.Add("сервер", valueNode);
            valueNode.ImageIndex = -1;


            groupNode = rootNode.Nodes.Add("внешний вид");
            groupNode.DefaultCellStyle.Font = boldFont;
            groupNode.ImageIndex = -1;

            valueNode = groupNode.Nodes.Add("шрифт", "ЕФМОА");
            //nodeDict.Add("сервер", valueNode);
            valueNode.ImageIndex = -1;

            valueNode = groupNode.Nodes.Add("цвет фона", "ХХХХХХ");
            //nodeDict.Add("сервер", valueNode);
            valueNode.ImageIndex = -1;


            valueNode = rootNode.Nodes.Add("подключаться при запуске", "true");
            //nodeDict.Add(NodeName.ProcessCpuUsage, valueNode);
            valueNode.ImageIndex = -1;

            valueNode = rootNode.Nodes.Add("число получаемых сообщений при подключении", "100");
            //nodeDict.Add(NodeName.ProcessCpuUsage, valueNode);
            valueNode.ImageIndex = -1;

            valueNode = rootNode.Nodes.Add("рарешить вывод консольных сообщений после подключения", "true");
            //nodeDict.Add(NodeName.ProcessCpuUsage, valueNode);
            valueNode.ImageIndex = -1;
        }

    }
}
