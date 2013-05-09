using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GuiControls;

namespace FiddlerControls
{
    public partial class WebBrowser : UserControl
    {
        public WebBrowser()
        {
            InitializeComponent();
            toolStripButtonPrev.Image   = Image.FromStream(Resources.GetStream(@"Icons.browser", "back",    "png"));
            toolStripButtonNext.Image   = Image.FromStream(Resources.GetStream(@"Icons.browser", "next",    "png"));
            toolStripButtonUpdate.Image = Image.FromStream(Resources.GetStream(@"Icons.browser", "refresh", "png"));
            toolStripDropDownButtonLinks.Image = Image.FromStream(Resources.GetStream(@"Icons.browser", "favorits", "png"));

            webSiteToolStripButton.Image = Image.FromStream(Resources.GetStream(@"Icons.browser", "website", "png"));
            emailToolStripButton.Image = Image.FromStream(Resources.GetStream(@"Icons.browser", "mail_new", "png"));

            Utils.FileRename(@"C:\UltimaOnline\source\_build\uoFiddler\Extracted\new\l", 0x1260);
            Utils.FileRename(@"C:\UltimaOnline\source\_build\uoFiddler\Extracted\new\t", 0x1260);
        }

        private void Browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            string uri = e.Url.ToString();
            if (uri.StartsWith(@"http://") || uri.StartsWith(@"ftp://") || uri.StartsWith(@"https://"))
                toolStripTextBoxUrl.Text = uri;
        }

        private void toolStripButtonUpdate_Click(object sender, EventArgs e)
        {
            Browser.Refresh();
        }

        private void toolStripTextBoxUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string uri = toolStripTextBoxUrl.Text;
                if (!uri.StartsWith(@"http://") && !uri.StartsWith(@"ftp://") && !uri.StartsWith(@"https://"))
                    uri = String.Format("http://{0}", uri);
                Browser.Url = new System.Uri(uri);
            }
        }




        #region Веб сайт

        private void websiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Browser.Url = new System.Uri(@"http://uoquint.ru/#body");
        }

        private void forumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Browser.Url = new System.Uri(@"http://uoquint.ru/forum/#body");
        }

        #endregion

        #region Сервисы

        private void mailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It's private secure link, you will be redirected to project site instead.");
            Browser.Url = new System.Uri(@"http://dev.uoquint.ru/projects");
        }

        private void docsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It's private secure link, you will be redirected to project site instead.");
            Browser.Url = new System.Uri(@"http://dev.uoquint.ru/projects");
        }

        #endregion

        #region Сервер

        private void fTPServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It's private secure link, you will be redirected to project site instead.");
            Browser.Url = new System.Uri(@"http://dev.uoquint.ru/projects");
        }

        private void downloadMap0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It's private secure link, you will be redirected to project site instead.");
            Browser.Url = new System.Uri(@"http://dev.uoquint.ru/projects");
        }

        private void downloadMap1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It's private secure link, you will be redirected to project site instead.");
            Browser.Url = new System.Uri(@"http://dev.uoquint.ru/projects");
        }

        private void downloadSta0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It's private secure link, you will be redirected to project site instead.");
            Browser.Url = new System.Uri(@"http://dev.uoquint.ru/projects");
        }

        private void downloadSta1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It's private secure link, you will be redirected to project site instead.");
            Browser.Url = new System.Uri(@"http://dev.uoquint.ru/projects");
        }

        #endregion

        #region Служебные

        private void serverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It's private secure link, you will be redirected to project site instead.");
            Browser.Url = new System.Uri(@"http://dev.uoquint.ru/projects");
        }

        private void commandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It's private secure link, you will be redirected to project site instead.");
            Browser.Url = new System.Uri(@"http://dev.uoquint.ru/projects");
        }

        private void itemsAndMobilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It's private secure link, you will be redirected to project site instead.");
            Browser.Url = new System.Uri(@"http://dev.uoquint.ru/projects");
        }

        private void keyWordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It's private secure link, you will be redirected to project site instead.");
            Browser.Url = new System.Uri(@"http://dev.uoquint.ru/projects");
        }

        private void bodyAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It's private secure link, you will be redirected to project site instead.");
            Browser.Url = new System.Uri(@"http://dev.uoquint.ru/projects");
        }

        private void classReviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It's private secure link, you will be redirected to project site instead.");
            Browser.Url = new System.Uri(@"http://dev.uoquint.ru/projects");
        }

        private void protocolGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It's private secure link, you will be redirected to project site instead.");
            Browser.Url = new System.Uri(@"http://dev.uoquint.ru/projects");
        }

        #endregion

    }
}
