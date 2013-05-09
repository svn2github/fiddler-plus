using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GuiControls;
using System.IO;

namespace FiddlerControls.CentrEditor
{
    public partial class CentrEd : UserControl
    {
        public class CentrEdProfile
        {
            public bool   Publ { get; set; }

            public string Name { get; set; }
            public string Host { get; set; }
            public int    Port { get; set; }
            public string User { get; set; }
            public string Pasw { get; set; }

            public string Data { get; set; }

            public string Conf { get; set; }
            public uint Flag { get; set; }
        }

        public CentrEd()
        {
            InitializeComponent();
            pbServer.BackgroundImage = pbClient.BackgroundImage = pbData.BackgroundImage = Image.FromStream(Resources.GetStream(@"Icons.centred", "wrong", "png"));
            pbServer.Tag = pbClient.Tag = pbData.Tag = true;

            var profdata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Path.Combine("CentrED-plus", "Profiles"));
            var profiles = Directory.GetDirectories(profdata, "*", SearchOption.TopDirectoryOnly);
            foreach (var profile in profiles) {
                var config = Path.Combine(profile, "login.ini");
                if (!File.Exists(config)) continue;
                var inifile = new IniFile(config);
                var entry = new CentrEdProfile();

                entry.Publ = true;
                entry.Name = Path.GetFileName(profile);
                entry.Host = inifile.ReadString("Connection", "Host", "localhost", false);
                entry.Port = inifile.ReadInt("Connection", "Port", 0, false);
                entry.User = inifile.ReadString("Connection", "Username", String.Empty, false);

                entry.Data = inifile.ReadString("Data", "Path", String.Empty, false);
            }

            var confdata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Path.Combine("CentrED-plus", "Configs"));
            if (!Directory.Exists(confdata)) Directory.CreateDirectory(confdata);
            var configes = Directory.GetDirectories(confdata, "*", SearchOption.TopDirectoryOnly);


            //var app = ;
            var ini = new IniFile(@"D:\AppData\Local\CentrED-plus\Profiles\map0\login.ini");
            var port = ini.ReadInt("Connection", "Port", 2597, false);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }




        private void OpenBrowserFolder(object sender, EventArgs e)
        {
            TextBox textbox;
            if (sender == btnData)          {   textbox = tbData;
                var dialog = new FolderBrowserDialog();
                dialog.ShowNewFolderButton = false;
                dialog.SelectedPath = textbox.Text;
                if (dialog.ShowDialog() == DialogResult.OK) {
                    textbox.Text = dialog.SelectedPath;
                }
            } else if (sender == btnClient) {   textbox = tbClient;
                var dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.Filter = "CentrED-plus.exe|CentrED-plus.exe";
                if (File.Exists(textbox.Text))
                    dialog.InitialDirectory = Path.GetDirectoryName(textbox.Text);
                if (dialog.ShowDialog() == DialogResult.OK) {
                    textbox.Text = dialog.FileName;
                }
            } else if (sender == btnServer) {   textbox = tbServer;
                var dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.Filter = "cedserver.exe|cedserver.exe";
                if (File.Exists(textbox.Text))
                    dialog.InitialDirectory = Path.GetDirectoryName(textbox.Text);
                if (dialog.ShowDialog() == DialogResult.OK) {
                    textbox.Text = dialog.FileName;
                }
            }
        }

        private void BrowserFolderChanged(object sender, EventArgs e)
        {
            var valid = true;
            var spath = (sender as TextBox).Text;
            PictureBox picturebox = null;
            
            if (sender == tbData)          {    picturebox = pbData;
                var files = new string[] { "tiledata.mul", "animdata.mul", "hues.mul", "artidx.mul", "art.mul", "texidx.mul", "texmaps.mul", "lightidx.mul", "light.mul" };
                if (!Directory.Exists(spath))
                    valid = false;
                foreach (var file in files) {
                    if (!valid) break;
                    if (!File.Exists(Path.Combine(spath, file)))
                        valid = false;
                }
            } else if (sender == tbClient) {    picturebox = pbClient;
                if (!File.Exists(spath) || !String.Equals(Path.GetFileName(spath), "CentrED-plus.exe", StringComparison.InvariantCultureIgnoreCase))
                    valid = false;
            } else if (sender == tbServer) {    picturebox = pbServer;
                if (!File.Exists(spath) || !String.Equals(Path.GetFileName(spath), "cedserver.exe", StringComparison.InvariantCultureIgnoreCase))
                    valid = false;
            }
            picturebox.BackgroundImage = Image.FromStream(Resources.GetStream(@"Icons.centred", valid ? "valid" : "wrong", "png"));
            picturebox.Tag = valid;
            ServerTypeChanged(sender, e);
        }

        private void ServerTypeChanged(object sender, EventArgs e)
        {
            bool server = false, client = false;
            if ((Boolean)pbClient.Tag && (Boolean)pbData.Tag) {
                server = (Boolean)pbServer.Tag && rbServer.Checked;
                client = rbLocal.Checked;
            }

            tbURI.Enabled = numPort.Enabled = tbAccount.Enabled = client;
            cbMap.Enabled = numWidth.Enabled = numHeight.Enabled = cbVersion.Enabled = clbFlags.Enabled = server;
            rbLocal.Enabled = rbServer.Enabled = client && server;
        }
    }
}
