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
            {
                string[] test = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                textBoxPath.Text = test[0].ToString();
                StatusMainForm.Text = "The file has been opened";
            }
        }

        private void ExportBtn_Click(object sender, EventArgs e)
        {
            FileStream fs = File.Open(textBoxPath.Text, FileMode.Open, FileAccess.Read);
            Pack root = new PackGPDA(fs);
            root.Analyse();
            fs.Close();
        }
    }
}
