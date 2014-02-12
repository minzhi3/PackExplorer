using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace PackExplorer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] test = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            textBoxPath.Text = test[0].ToString();
            StatusMainForm.Text = "The file has been opened";
            FileInfo fi = new FileInfo(textBoxPath.Text);
            textBoxFavorFiles.Text =Path.Combine(fi.DirectoryName,"favor");
            Element.path_of_favor_files = textBoxFavorFiles.Text;
        }

        private void ExportBtn_Click(object sender, EventArgs e)
        {
            SetExportButtonEnable(false);
            Thread thread=new Thread(new ThreadStart(ExtractPack));
            thread.IsBackground=true;
            thread.Start();
        }
        private void ExtractPack()
        {
            FileInfo fi = new FileInfo(textBoxPath.Text);
            PackFile pf = new PackFile(fi);
            pf.Extract(AnalyseManager.AnalysePackGPDA, AnalyseManager.CheckPackGPDA, new StatusShow(ShowStatus));
            this.ShowStatus("Complete");
            SetExportButtonEnable(true);
            Thread.CurrentThread.Abort();
        }
        //--------------Status of mainform-----------
        /// <summary>
        /// delegate class about text in status bar
        /// </summary>
        /// <param name="status">Text of status</param>
        public delegate void StatusShow(string status);

        /// <summary>
        /// delegate class about export button
        /// </summary>
        /// <param name="enable">ability of button</param>
        private delegate void ExportEnable(bool enable);
        /// <summary>
        /// Show text on status trip
        /// </summary>
        /// <param name="status"></param>
        private void ShowStatus(string status)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new StatusShow(ShowStatus),status);
            }
            else
            {
                this.StatusMainForm.Text=status;
            }
        }
        private void SetExportButtonEnable(bool enable)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ExportEnable(SetExportButtonEnable), enable);
            }
            else
            {
                this.ExportBtn.Enabled = enable;
                this.textBoxFavorFiles.Enabled = enable;
                this.textBoxPath.Enabled = enable;
            }
        }
    }
}
