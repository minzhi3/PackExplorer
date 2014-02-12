using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PackExplorer
{
    /// <summary>
    /// The file of pack
    /// </summary>
    class PackFile
    {
        FileInfo packfile;
        Pack pack;
        bool open;
        public FileInfo PackFileInfo
        {
            get {return packfile;}
        }
        public PackFile(FileInfo fi)
        {
            open = false;
            Initial(fi);
        }
        private void Initial(FileInfo fi)
        {
            packfile = fi;
        }
        public void Extract(Pack.AnalysePack analyse_algo, Pack.CheckPack check_algo,MainForm.StatusShow showstatus)
        {
            //Extracting start
            FileStream fs = File.Open(packfile.FullName, FileMode.Open, FileAccess.Read);
            open = true;
            Element e = new Element(fs);
            //check pack
            if (!check_algo(e))
            {
                showstatus("Not an avaialbe pack");
                System.Windows.Forms.MessageBox.Show("Not an avaiable pack. Please check the file format.");
            }
            else
            {
                pack = new Pack(e, analyse_algo, check_algo);
                pack.Extract(packfile.DirectoryName, showstatus);
            }

            //Extracting Complete
            pack = null;
            fs.Close();
            open = false;
        }

    }
}
