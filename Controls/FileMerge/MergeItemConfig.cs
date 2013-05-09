using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FiddlerControls.FileMerge
{
    public partial class MergeItemConfig : Form
    {
        public MergeItemConfig()
        {
            InitializeComponent();
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Укажите папку содержащую файлы \"artidx.mul\" \"art.mul\"\n(также рекомендуется наличие файлов \"tiledata.mul\" \"radarcol.mul\")";
                dialog.ShowNewFolderButton = false;
                if (dialog.ShowDialog() == DialogResult.OK)
                    folderTextBox.Text = dialog.SelectedPath;
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            string path = folderTextBox.Text;
            if (String.IsNullOrEmpty(path))
            {
                MessageBox.Show(String.Format("Не указан путь к директории\nЗагрузка будет прервана.", AppDomain.CurrentDomain.SetupInformation.ApplicationBase), "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (!Directory.Exists(path))
            {
                MessageBox.Show(String.Format("Указанный путь не существует\nЗагрузка будет прервана.", AppDomain.CurrentDomain.SetupInformation.ApplicationBase), "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            string idxPath = Path.Combine(path, "artidx.mul");
            string mulPath = Path.Combine(path, "art.mul");
            if (!File.Exists(idxPath) || !File.Exists(mulPath))
            {
                MessageBox.Show(String.Format("В указанной папке не найдены необходимые файлы \"artidx.mul\" и \"art.mul\"\nЗагрузка будет прервана.", AppDomain.CurrentDomain.SetupInformation.ApplicationBase), "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            Ultima.Secondary.Art.Dispose();
            if (!Ultima.Secondary.Art.SetFileIndex(idxPath, mulPath))
            {
                MessageBox.Show(String.Format("Не удалось инициализировать \"artidx.mul\" и \"art.mul\"\nЗагрузка будет прервана.", AppDomain.CurrentDomain.SetupInformation.ApplicationBase), "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            Ultima.Secondary.TileData.Dispose();
            if (!Ultima.Secondary.TileData.SetFile(Path.Combine(path, "tiledata.mul")))
            {
                MessageBox.Show(String.Format("Не удалось инициализировать \"tiledata.mul\"", AppDomain.CurrentDomain.SetupInformation.ApplicationBase), "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            Ultima.Secondary.RadarCol.Dispose();
            if (!Ultima.Secondary.RadarCol.SetFile(Path.Combine(path, "radarcol.mul")))
            {
                MessageBox.Show(String.Format("Не удалось инициализировать \"radarcol.mul\"", AppDomain.CurrentDomain.SetupInformation.ApplicationBase), "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {

        }
    }
}
