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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Ultima;
using tevton.SyntaxHighlight;
using System.ComponentModel;

namespace FiddlerControls
{
    public partial class Cliloc : UserControl
    {
        public Cliloc()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            refmarker = this;
            source = new BindingSource();
            FindEntry.TextBox.PreviewKeyDown+=new PreviewKeyDownEventHandler(FindEntry_PreviewKeyDown);

            var _assembly = Assembly.Load("GuiControls");
            FindButton.DisplayStyle = ToolStripItemDisplayStyle.Image;          FindButton.Image = Image.FromStream(_assembly.GetManifestResourceStream("GuiControls.Icons.cliloc.control.btn.find.png"));
            GotoButton.DisplayStyle = ToolStripItemDisplayStyle.Image;          GotoButton.Image = Image.FromStream(_assembly.GetManifestResourceStream("GuiControls.Icons.cliloc.control.btn.goto.png"));
            ClilocTilesButton.DisplayStyle = ToolStripItemDisplayStyle.Image;   ClilocTilesButton.Image = Image.FromStream(_assembly.GetManifestResourceStream("GuiControls.Icons.cliloc.control.btn.tiles.png"));
            ClilocEmptyButton.DisplayStyle = ToolStripItemDisplayStyle.Image;   ClilocEmptyButton.Image = Image.FromStream(_assembly.GetManifestResourceStream("GuiControls.Icons.cliloc.control.btn.empty.png"));
            ClilocMenuButton.Image = Image.FromStream(_assembly.GetManifestResourceStream("GuiControls.Icons.cliloc.control.btn.menu.png"));
            LangComboBox.Image = Image.FromStream(_assembly.GetManifestResourceStream("GuiControls.Icons.cliloc.control.btn.lang.png"));
            ClilocSaveButton.Image = Image.FromStream(_assembly.GetManifestResourceStream("GuiControls.Icons.cliloc.control.btn.save.png"));
            ClilocRewriteButton.Image = Image.FromStream(_assembly.GetManifestResourceStream("GuiControls.Icons.cliloc.control.btn.save.png"));
            _flagico = new Image[5];
            _flagico[0] = Image.FromStream(_assembly.GetManifestResourceStream("GuiControls.Icons.cliloc.control.data.original.png"));
            _flagico[1] = Image.FromStream(_assembly.GetManifestResourceStream("GuiControls.Icons.cliloc.control.data.edited.png"));
            _flagico[2] = Image.FromStream(_assembly.GetManifestResourceStream("GuiControls.Icons.cliloc.control.data.added.png"));
            _flagico[3] = Image.FromStream(_assembly.GetManifestResourceStream("GuiControls.Icons.cliloc.control.data.delete.png"));
            _flagico[4] = Image.FromStream(_assembly.GetManifestResourceStream("GuiControls.Icons.cliloc.control.data.tilename.png"));

            highlighter.Dictionary = dic;
            highlighter.HookToRichTextBox(TextBox);

            dic.Font = TextBox.Font;
            dic.ForegroundColor = TextBox.ForeColor;
            dic.BackgroundColor = TextBox.BackColor;
            /*
            dic.Add(new SyntaxHighlightItem("keywords",
                new string[] {"\\bSELECT\\b", "\\bINSERT\\b", "\\bDELETE\\b", 
                    "\\bUPDATE\\b", "\\bFROM\\b", "\\bWHERE\\b"},
                FontStyle.Bold, RegexOptions.IgnoreCase));
            dic.Add(new SyntaxHighlightItem("strings",
                new string[] { "'[^'\r\n]*'" },
                FontStyle.Regular, Color.Blue, Color.Transparent));
            dic.Add(new SyntaxHighlightItem("comments",
            *///    new string[] { "--.*", "/\\*[\\d\\D]*?\\*/" },
            //   FontStyle.Italic, Color.Gray, Color.Transparent));
            
            dic.Add(new SyntaxHighlightItem("tags",
                new string[] { "<[\\d\\D]*?\\>" },
                FontStyle.Bold, Color.RoyalBlue, Color.Transparent));

            dic.Add(new SyntaxHighlightItem("vars",
                new string[] { "~[\\d][\\d]*?[_][\\D][\\D]*?~" },
                FontStyle.Bold, Color.DarkRed, Color.Transparent));
        }

        private static Image[] _flagico;
        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1) return;
            int number = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            if ((number >= 1020000 && number < 1020000 + 0x4000)            || 
                (number >= 1078872 + 0x4000 && number < 1078872 + 0x8000)   || 
                (number >= 1084024 + 0x8000 && number < 1084024 + 0xFFDC)   ) {
                e.CellStyle.BackColor = Color.Gainsboro;
            } 
            if (e.ColumnIndex != 2) return;

            using (
                Brush gridBrush = new SolidBrush(this.dataGridView1.GridColor),
                backColorBrush = new SolidBrush(e.CellStyle.BackColor)) {
                using (Pen gridLinePen = new Pen(gridBrush))
                {
                    e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom-1, e.CellBounds.Right-1, e.CellBounds.Bottom-1);
                    //e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right-1, e.CellBounds.Top, e.CellBounds.Right-1, e.CellBounds.Bottom);

                    if (e.Value != null) {
                        if (Convert.ToString(e.Value) == "Original") {
                            e.Graphics.DrawImage(_flagico[0], e.CellBounds.X+2, e.CellBounds.Y+2);
                        } else if (Convert.ToString(e.Value) == "Modified") {
                            e.Graphics.DrawImage(_flagico[1], e.CellBounds.X+2, e.CellBounds.Y+2);
                        } else if (Convert.ToString(e.Value) == "Custom") {
                            e.Graphics.DrawImage(_flagico[2], e.CellBounds.X+2, e.CellBounds.Y+2);
                        }
                    }
                    e.Handled = true;
                }
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            // highlighter.HighlightRichTextBox(TextBox);
        }

        private SyntaxHighlightDictionary dic = new SyntaxHighlightDictionary();
        private RichTextBoxHighlighter highlighter = new RichTextBoxHighlighter();

        #region Var's
        private static Cliloc refmarker;
        private static StringList cliloc;
        private static BindingSource source;
        private int lang;
        private SortOrder sortorder;
        private int sortcolumn;
        private bool Loaded = false;

        /// <summary>
        /// Sets Language and loads cliloc
        /// </summary>
        private int Lang
        {
            get { return lang; }
            set
            {
                lang = value;
                switch (value)
                {
                    case 0:
                        cliloc = new StringList("enu");
                        break;
                    case 1:
                        cliloc = new StringList("rus");
                        break;
                    case 2:
                        TestCustomLang("cliloc.custom1");
                        cliloc = new StringList("custom1");
                        break;
                    case 3:
                        TestCustomLang("cliloc.custom2");
                        cliloc = new StringList("custom2");
                        break;
                }
            }
        }
        #endregion

        /// <summary>
        /// Reload when loaded (file changed)
        /// </summary>
        private void Reload()
        {
            if (!Loaded)
                return;
            OnLoad(this, EventArgs.Empty);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            sortorder = SortOrder.Ascending;
            sortcolumn = 0;
            LangComboBox.SelectedIndex = 0;
            Lang = 0;
            cliloc.Entries.Sort(new StringList.NumberComparer(false));
            //source.DataSource = cliloc.Entries;
            //source.DataSource = new BindingList<StringEntry>(cliloc.Entries);
            source.DataSource = new StringEntryBindingList(cliloc);
            
            dataGridView1.DataSource = source;
            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                dataGridView1.Columns[0].Width = 60;
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridView1.Columns[0].Resizable = DataGridViewTriState.False;
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].HeaderCell.SortGlyphDirection = SortOrder.None;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].HeaderCell.SortGlyphDirection = SortOrder.None;
                dataGridView1.Columns[2].Width = 24;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridView1.Columns[2].Resizable = DataGridViewTriState.False;
                dataGridView1.Columns[2].ReadOnly = true;

                dataGridView1.Columns[0].HeaderText = "Номер";
                dataGridView1.Columns[1].HeaderText = "Текст";
                dataGridView1.Columns[2].HeaderText = "";//"Флаг";
                dataGridView1.Columns[2].DisplayIndex = 0;
                dataGridView1.Columns[0].DisplayIndex = 1;
                dataGridView1.Columns[1].DisplayIndex = 2;
            }
            dataGridView1.Invalidate();
            if (Files.GetFilePath("cliloc.custom1") != null)
                LangComboBox.Items[2] = String.Format("Custom 1 ({0})", Path.GetExtension(Files.GetFilePath("cliloc.custom1")));
            else
                LangComboBox.Items[2] = "Custom 1";
            if (Files.GetFilePath("cliloc.custom2") != null)
                LangComboBox.Items[3] = String.Format("Custom 2 ({0})", Path.GetExtension(Files.GetFilePath("cliloc.custom2")));
            else
                LangComboBox.Items[3] = "Custom 2";
            if (!Loaded)
                FiddlerControls.Events.FilePathChangeEvent += new FiddlerControls.Events.FilePathChangeHandler(OnFilePathChangeEvent);
            Loaded = true;

            countLabel.Text = String.Format("{0} / {0}", cliloc.Entries.Count);
            countLabel.Tag = cliloc.Entries.Count;
            Cursor.Current = Cursors.Default;
        }

        private void OnFilePathChangeEvent()
        {
            Reload();
        }

        private void TestCustomLang(string what)
        {
            if (Files.GetFilePath(what) == null)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Multiselect = false;
                    dialog.Title = "Choose Cliloc file to open";
                    dialog.CheckFileExists = true;
                    dialog.Filter = "cliloc files (cliloc.*)|cliloc.*";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        Files.SetMulPath(dialog.FileName, what);
                        LangComboBox.BeginUpdate();
                        if (what == "cliloc.custom1")
                            LangComboBox.Items[2] = String.Format("Custom 1 ({0})", Path.GetExtension(dialog.FileName));
                        else
                            LangComboBox.Items[3] = String.Format("Custom 2 ({0})", Path.GetExtension(dialog.FileName));
                        LangComboBox.EndUpdate();
                    }
                }
            }
        }

        private void onLangChange(object sender, EventArgs e)
        {
            if (LangComboBox.SelectedIndex != Lang)
            {
                Lang = LangComboBox.SelectedIndex;
                sortorder = SortOrder.Ascending;
                sortcolumn = 0;
                cliloc.Entries.Sort(new StringList.NumberComparer(false));
                source.DataSource = cliloc.Entries;
                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    dataGridView1.Columns[0].Width = 60;
                    dataGridView1.Columns[1].HeaderCell.SortGlyphDirection = SortOrder.None;
                    dataGridView1.Columns[2].HeaderCell.SortGlyphDirection = SortOrder.None;
                    dataGridView1.Columns[2].Width = 60;
                    dataGridView1.Columns[2].ReadOnly = true;
                }
                dataGridView1.Invalidate();
            }
        }

        private void GotoEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GotoNr(sender, e);
        }

        private void GotoNr(object sender, EventArgs e)
        {
            var ns = NumberStyles.Integer;
            var nt = GotoEntry.Text.ToString();
            int nr;
            var ni = false;

            if (GotoEntry.Text.StartsWith("I")) {
                if (String.Compare(nt.Substring(1, 2), "0x", true) == 0) {
                    nt = nt.Substring(3);
                    ns = NumberStyles.HexNumber;
                }   ni = true;
            } else
            if (GotoEntry.Text.StartsWith("<Item ID=\"0x")) {
                nt = nt.Substring(12, 4);
                ns = NumberStyles.HexNumber; 
                ni = true;
            }
                

            if (Int32.TryParse(nt, ns, null, out nr)) {
                if (ni) {
                    if (nr < 0) return;
                    else if (nr < 0x4000) nr += 1020000;
                    else if (nr < 0x8000) nr += 1078872;
                    else if (nr < 0xFFDC) nr += 1084024;
                    else return;
                }
                for (int i = 0; i < dataGridView1.Rows.Count; ++i) {
                    if ((int)dataGridView1.Rows[i].Cells[0].Value == nr) {
                        if (dataGridView1.Rows[i].Visible) {
                            dataGridView1.Rows[i].Selected = true;
                            dataGridView1.FirstDisplayedScrollingRowIndex = i;
                        }
                        return;
                    }
                }
            }
            //MessageBox.Show("Индификатор не найден.", "Перейти", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        private void FindEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                FindEntryClick(sender, e);
        }

        private void FindEntryClick(object sender, EventArgs e)
        {
            Regex regex = new Regex(@FindEntry.Text.ToString(), RegexOptions.IgnoreCase);
            for (int i = (dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected) + 1); i < dataGridView1.Rows.Count; ++i)
            {
                if (regex.IsMatch(dataGridView1.Rows[i].Cells[1].Value.ToString()) && dataGridView1.Rows[i].Visible)
                {
                    dataGridView1.Rows[i].Selected = true;
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    return;
                }
            }
            MessageBox.Show("Искомый текст не найден.", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        private void ClilocTilesButton_Click(object sender, EventArgs e)
        {
            dataGridView1.UseWaitCursor = true;
            ((StringEntryBindingList)source.DataSource).ShowItems = ClilocTilesButton.Checked;
            ((StringEntryBindingList)source.DataSource).ApplyFilter();
            dataGridView1.Invalidate();
            countLabel.Tag = ((StringEntryBindingList)source.DataSource).VisibleRows;
            countLabel.Text = String.Format("{0} / {1}", (int)countLabel.Tag, cliloc.Entries.Count);
            dataGridView1.UseWaitCursor = false;
        }

        private void ClilocEmptyButton_Click(object sender, EventArgs e)
        {
            dataGridView1.UseWaitCursor = true;
            ((StringEntryBindingList)source.DataSource).ShowEmpty = ClilocTilesButton.Checked;
            ((StringEntryBindingList)source.DataSource).ApplyFilter();
            dataGridView1.Invalidate();
            countLabel.Tag = ((StringEntryBindingList)source.DataSource).VisibleRows;
            countLabel.Text = String.Format("{0} / {1}", (int)countLabel.Tag, cliloc.Entries.Count);
            dataGridView1.UseWaitCursor = false;
        }

        private void OnClickSave(object sender, EventArgs e)
        {
            if (NumberLabel.Tag != null)
                SaveEntry((int)NumberLabel.Tag, TextBox.Text);

            dataGridView1.CancelEdit();
            
            string path = (sender == ClilocRewriteButton)
                        ? Path.GetDirectoryName(Files.GetFilePath(String.Format("cliloc.{0}", cliloc.Language)))
                        : Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Mul Files");
            string FileName;
            if (cliloc.Language == "custom1")
                FileName = Path.Combine(path, String.Format("Cliloc{0}", Path.GetExtension(Files.GetFilePath("cliloc.custom1"))));
            else if (cliloc.Language == "custom2")
                FileName = Path.Combine(path, String.Format("Cliloc{0}", Path.GetExtension(Files.GetFilePath("cliloc.custom2"))));
            else
                FileName = Path.Combine(path, String.Format("Cliloc.{0}", cliloc.Language));
            cliloc.SaveStringList(FileName);
            dataGridView1.Columns[sortcolumn].HeaderCell.SortGlyphDirection = SortOrder.None;
            dataGridView1.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            sortcolumn = 0;
            sortorder = SortOrder.Ascending;
            dataGridView1.Invalidate();
            MessageBox.Show(
                String.Format("CliLoc был успешно сохранен в {0}", FileName),
                "Сохранение",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
            Options.ChangedUltimaClass["CliLoc"] = false;
        }

        private void onCell_Selected(object sender, EventArgs e)
        {
            if (NumberLabel.Tag != null)
                SaveEntry((int)NumberLabel.Tag, TextBox.Text);

            if (dataGridView1.SelectedRows.Count > 0) {
                NumberLabel.Tag = dataGridView1.SelectedRows[0].Cells[0].Value;
                NumberLabel.Text = String.Format("# {0}", (int)NumberLabel.Tag);

                //TextBox.Clear();
                TextBox.Text = (string)dataGridView1.SelectedRows[0].Cells[1].Value;
                //TextBox.AppendText((string)dataGridView1.SelectedRows[0].Cells[1].Value);
            } else {
                NumberLabel.Tag = null;
                NumberLabel.Text = String.Format("# {0}", String.Empty);
                TextBox.Clear();
            }
            highlighter.HighlightRichTextBox(TextBox);
        }

        private void insertNBefor_Click(object sender, EventArgs e)
        {
            insertEntries(-1, -1, 1);
        }

        private void insertNAfter_Click(object sender, EventArgs e)
        {
            insertEntries(-1, +1, 1);
        }

        private void insertNBeforTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            int number;
            if (e.KeyCode != Keys.Enter) return;
            if (int.TryParse(insertNBeforTextBox.Text, NumberStyles.Integer, null, out number)) {
                insertEntries(-1, -1, number);
            } else
                MessageBox.Show("Не удалось разобрать число.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        private void insertNAfterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            int number;
            if (e.KeyCode != Keys.Enter) return;
            if (int.TryParse(insertNAfterTextBox.Text, NumberStyles.Integer, null, out number)) {
                insertEntries(-1, +1, number);
            } else
                MessageBox.Show("Не удалось разобрать число.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        private void insertEntries(int entryId, int direction, int count)
        {
            if (direction == 0) return;

            if (entryId < 0) {
                if (dataGridView1.SelectedRows.Count <= 0) return;
                entryId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
            }

            while (--count >= 0)
                if (IsNumberFree(direction > 0 ? ++entryId : --entryId))
                    AddEntry(entryId);
        }

        private void AddEntryTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            var text = addEntryTextBox.Text;
            var style = NumberStyles.Integer;
            var item = false;
            int number;
            if (text.StartsWith("I")) {
                if (String.Compare(text.Substring(1, 2), "0x", true) == 0) {
                    text = text.Substring(3);
                    style = NumberStyles.HexNumber;
                }   item = true;
            } else
            if (text.StartsWith("<Item ID=\"0x")) {
                text = text.Substring(12, 4);
                style = NumberStyles.HexNumber; 
                item = true;
            }
                
            if (int.TryParse(text, style, null, out number)) {
                if (item) {
                    if (number < 0) return;
                    else if (number < 0x4000) number += 1020000;
                    else if (number < 0x8000) number += 1078872;
                    else if (number < 0xFFDC) number += 1084024;
                    else return;
                }
                if (IsNumberFree(number)) {
                    AddEntry(number);
                } else
                    MessageBox.Show("Строка уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            } else
                MessageBox.Show("Не удалось разобрать число.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        private void OnClick_DeleteEntry(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                //cliloc.Entries.RemoveAt(dataGridView1.SelectedCells[0].OwningRow.Index);
                ((BindingList<StringEntry>)source.DataSource).RemoveAt(dataGridView1.SelectedCells[0].OwningRow.Index);
                dataGridView1.Invalidate();
                Options.ChangedUltimaClass["CliLoc"] = true;

                refmarker.countLabel.Tag = (int)refmarker.countLabel.Tag - 1;
                refmarker.countLabel.Text = String.Format("{0} / {1}", (int)refmarker.countLabel.Tag, cliloc.Entries.Count);
            }
        }

        private void OnHeaderClicked(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView1.UseWaitCursor = true;
            if (sortcolumn == e.ColumnIndex)
            {
                if (sortorder == SortOrder.Ascending)
                    sortorder = SortOrder.Descending;
                else
                    sortorder = SortOrder.Ascending;
            }
            else
            {
                sortorder = SortOrder.Ascending;
                dataGridView1.Columns[sortcolumn].HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortorder;
            sortcolumn = e.ColumnIndex;

             //((StringEntryBindingList)source.DataSource).ApplySort();

            if (e.ColumnIndex == 0)
                cliloc.Entries.Sort(new StringList.NumberComparer(sortorder == SortOrder.Descending));
            else if (e.ColumnIndex == 1)
                cliloc.Entries.Sort(new StringList.TextComparer(sortorder == SortOrder.Descending));
            else
                cliloc.Entries.Sort(new StringList.FlagComparer(sortorder == SortOrder.Descending));

            //NOTE: Не кошерно, но работает вполне сносно...
            source.DataSource = new StringEntryBindingList(cliloc);

            if (!ClilocTilesButton.Checked || !ClilocTilesButton.Checked) {
                ((StringEntryBindingList)source.DataSource).ShowItems = ClilocTilesButton.Checked;
                ((StringEntryBindingList)source.DataSource).ShowEmpty = ClilocTilesButton.Checked;
                ((StringEntryBindingList)source.DataSource).ApplyFilter();    
            }

            dataGridView1.Invalidate();
            countLabel.Tag = ((StringEntryBindingList)source.DataSource).VisibleRows;
            countLabel.Text = String.Format("{0} / {1}", (int)countLabel.Tag, cliloc.Entries.Count);
            dataGridView1.UseWaitCursor = false;
        }

        private void OnCLick_CopyClilocNumber(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
                Clipboard.SetDataObject(
                    ((int)dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value).ToString(), true);
        }

        private void OnCLick_CopyClilocText(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
                Clipboard.SetDataObject(
                    (string)dataGridView1.SelectedCells[0].OwningRow.Cells[1].Value, true);
        }

        #region Public Interface for ClilocAdd

        public static void SaveEntry(int number, string text)
        {
            if (number < 0) return;
            for (int i = 0; i < cliloc.Entries.Count; ++i)
            {
                if (((StringEntry)cliloc.Entries[i]).Number == number)
                {
                    if (String.Compare(cliloc.Entries[i].Text, text) == 0)
                        return;

                    ((StringEntry)cliloc.Entries[i]).Text = text;
                    ((StringEntry)cliloc.Entries[i]).Flag = StringEntry.CliLocFlag.Modified;
                    refmarker.dataGridView1.Invalidate();
                    refmarker.dataGridView1.Rows[i].Selected = true;
                    //refmarker.dataGridView1.FirstDisplayedScrollingRowIndex = i;

                    Options.ChangedUltimaClass["CliLoc"] = true;
                    return;
                }
            }
        }

        public static bool IsNumberFree(int number)
        {
            foreach (StringEntry entry in cliloc.Entries)
            {
                if (entry.Number == number)
                    return false;
            }
            return true;
        }


        public static void AddEntry(int number)
        {
            int index = 0;
            foreach (StringEntry entry in cliloc.Entries)
            {
                if (entry.Number > number || index + 1 == cliloc.Entries.Count)
                {
                    if (index + 1 == cliloc.Entries.Count) index = ((BindingList<StringEntry>)source.DataSource).Count;
                    ((BindingList<StringEntry>)source.DataSource).Insert(index, new StringEntry(number, "", StringEntry.CliLocFlag.Custom));
                    refmarker.dataGridView1.Invalidate();
                    refmarker.dataGridView1.Rows[index].Selected = true;
                    //refmarker.dataGridView1.FirstDisplayedScrollingRowIndex = index;
                    Options.ChangedUltimaClass["CliLoc"] = true;

                    refmarker.countLabel.Tag = (int)refmarker.countLabel.Tag + 1;
                    refmarker.countLabel.Text = String.Format("{0} / {1}", (int)refmarker.countLabel.Tag, cliloc.Entries.Count);
                    return;
                }
                ++index;
            }
        }
        #endregion

        private void FindEntry_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //if (e.KeyData == Keys.Control) || (e.Ke Keys.Alt | Keys.Tab | Keys.a))
                e.IsInputKey = true;
        }

        private void OnClickExportCSV(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, "CliLoc.csv");
            using (StreamWriter Tex = new StreamWriter(new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite)))
            {
                Tex.WriteLine("Number;Text;Flag");
                foreach (StringEntry entry in cliloc.Entries)
                {
                    Tex.WriteLine(String.Format("{0};{1};{2}", entry.Number,entry.Text,entry.Flag));
                }
            }
            MessageBox.Show(String.Format("CliLoc экспортирован в {0}", FileName), "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void OnClickImportCSV(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "Choose csv file to import";
            dialog.CheckFileExists = true;
            dialog.Filter = "csv files (*.csv)|*.csv";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(dialog.FileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if ((line = line.Trim()).Length == 0 || line.StartsWith("#"))
                            continue;
                        if (line.StartsWith("Number;"))
                            continue;
                        try
                        {
                            string[] split = line.Split(';');
                            if (split.Length < 3)
                                continue;

                            int id = int.Parse(split[0].Trim());
                            string text = split[1].Trim();

                            int index = 0;
                            foreach (StringEntry entry in cliloc.Entries)
                            {
                                if (entry.Number == id)
                                {
                                    entry.Text = text;
                                    entry.Flag = StringEntry.CliLocFlag.Modified;
                                    Options.ChangedUltimaClass["CliLoc"] = true;
                                    break;
                                }
                                else if (entry.Number > id)
                                {
                                    cliloc.Entries.Insert(index, new StringEntry(id, text, StringEntry.CliLocFlag.Custom));
                                    Options.ChangedUltimaClass["CliLoc"] = true;
                                    break;
                                }
                                ++index;
                            }
                            dataGridView1.Invalidate();
                        }
                        catch { }
                    }
                }
            }
            dialog.Dispose();
        }

        private void ClilocExportTiledataButton_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (var entry in cliloc.Entries) {
                int index = entry.Number;
                     if (entry.Number >= 1020000 && entry.Number < 1020000 + 0x4000)
                    index -= 1020000;
                else if (entry.Number >= 1078872 + 0x4000 && entry.Number < 1078872 + 0x8000)
                    index -= 1078872;
                else if (entry.Number >= 1084024 + 0x8000 && entry.Number < 1084024 + 0xFFDC)
                    index -= 1084024;
                else continue;
                
                if (TileData.ItemTable[index].Name != entry.Text) {
                    TileData.ItemTable[index].Name = entry.Text;
                    Options.ChangedUltimaClass["CliLoc"] = true;
                    ++count;
                }
            }

            MessageBox.Show(String.Format("{0} Строк было экспортированно в tiledata.mul", count), "Импорт", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void ClilocImportTiledataButton_Click(object sender, EventArgs e)
        {
            int count = 0;
            for (int i = 0; i < TileData.ItemTable.Length; ++i) {
                int number = i;
                     if (number < 0x4000) number += 1020000;
                else if (number < 0x8000) number += 1078872;
                else if (number < 0xFFDC) number += 1084024;
                else continue;

                if (IsNumberFree(number) && !String.IsNullOrEmpty(TileData.ItemTable[i].Name.Trim()))
                    AddEntry(number);
            }

            foreach (var entry in cliloc.Entries) {
                int index = entry.Number;
                     if (entry.Number >= 1020000 && entry.Number < 1020000 + 0x4000)
                    index -= 1020000;
                else if (entry.Number >= 1078872 + 0x4000 && entry.Number < 1078872 + 0x8000)
                    index -= 1078872;
                else if (entry.Number >= 1084024 + 0x8000 && entry.Number < 1084024 + 0xFFDC)
                    index -= 1084024;
                else continue;
                
                if (entry.Text != TileData.ItemTable[index].Name) {
                    entry.Text = TileData.ItemTable[index].Name;
                    Options.ChangedUltimaClass["CliLoc"] = true;
                    ++count;
                }
            }
  
            MessageBox.Show(String.Format("{0} Строк было импортированно из tiledata.mul", count), "Импорт", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        #region BindingList Implementation

        public class StringEntryBindingList : FilterBindingListAdapter<StringEntry>
        {

            public StringEntryBindingList(StringList stringlist) : base(new BindingList<StringEntry>(stringlist.Entries))
            {
                ShowItems = refmarker.ClilocTilesButton.Checked;
                ShowEmpty = refmarker.ClilocEmptyButton.Checked;
            }

            public bool ShowItems { get; set; }
            public bool ShowEmpty { get; set; }
            public int VisibleRows { get; private set; }

            public void ApplyFilter()
            {
                VisibleRows = 0;
                base.Filter = "yes";
                //base.DoFilter();
            }

            protected override bool ISVisible(StringEntry entry)
            {
                if (!ShowItems && ((entry.Number >= 1020000 && entry.Number < 1020000 + 0x4000)
                                || (entry.Number >= 1078872 + 0x4000 && entry.Number < 1078872 + 0x8000)
                                || (entry.Number >= 1084024 + 0x8000 && entry.Number < 1084024 + 0xFFDC) ))
                    return false;
                if (!ShowEmpty && String.IsNullOrEmpty(entry.Text.Trim()))
                    return false;
                ++VisibleRows;
                return true;
            }
        }
         
        public class FilterBindingListAdapter<T> : BindingList<T>, IBindingListView
        {
            protected string filter = String.Empty;
            protected IBindingList bindingList;
            private bool filtering = false;

            public FilterBindingListAdapter(IBindingList bindingList)
            {
                this.bindingList = bindingList;
                DoFilter();
            }


            protected override void OnListChanged(ListChangedEventArgs e)
            {
                if (!filtering)
                {
                    switch (e.ListChangedType)
                    {
                        case ListChangedType.ItemAdded:
                            bindingList.Insert(e.NewIndex, this[e.NewIndex]);
                            break;
                        case ListChangedType.ItemDeleted:
                            //bindingList.RemoveAt(e.NewIndex);
                            break;
                    }
                }

                base.OnListChanged(e);
            }

            protected override void RemoveItem(int index)
            {
                if (!filtering)
                {
                    bindingList.RemoveAt(index);
                }

                base.RemoveItem(index);
            }

            protected virtual void DoFilter()
            {
                filtering = true;
                this.Clear();

                foreach (T e in bindingList)
                {
                    if (filter.Length == 0 || this.ISVisible(e))
                    {
                        this.Add((T)e);
                    }
                }
                filtering = false;
            }

            protected virtual bool ISVisible(T element)
            {
                return true;
            }


            #region IBindingListView Members

            public void ApplySort(ListSortDescriptionCollection sorts)
            {
                throw new NotImplementedException();
            }

            public string Filter
            {
                get
                {
                    return filter;
                }
                set
                {
                    filter = value;
                    DoFilter();
                }
            }

            public void RemoveFilter()
            {
                Filter = String.Empty;
            }

            public ListSortDescriptionCollection SortDescriptions
            {
                get { throw new NotImplementedException(); }
            }

            public bool SupportsAdvancedSorting
            {
                get { return false; }
            }

            public bool SupportsFiltering
            {
                get { return true; }
            }

            #endregion
        }
        #endregion

        
    }
}
