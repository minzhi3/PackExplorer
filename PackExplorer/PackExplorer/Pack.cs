using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace PackExplorer
{
    public delegate bool CheckType(Element e);
    public delegate List<Element> AnalysePack(Element e);
    public class Pack : Element
    {

        AnalysePack pack_algo;
        CheckType ispack, iscompress;
        AnalyseCompress comp_algo;
        protected List<Element> elements;
        protected Int64 count;
        public Pack(Element e, AnalysePack ap,CheckType ispa,AnalyseCompress ac,CheckType iscp)
            :base(e.Data,e.Offset,e.Size,e.Name)
        {
            Initial(ap,ispa,ac,iscp);
        }
        //public Pack(Stream sm,long offset,long size,string name)
        //    : base(sm, offset, size, name)
        //{
        //    Initial(ap);
        //}
        private void Initial(AnalysePack ap, CheckType ispack, AnalyseCompress ac, CheckType iscp)
        {
            this.pack_algo = ap;
            this.ispack = ispack;
            this.iscompress = iscp;
            this.comp_algo = ac;
            elements=ap((Element)base.MemberwiseClone());

        }
        /// <summary>
        /// Extract the pack file
        /// </summary>
        /// <param name="path">path of destination</param>
        /// <param name="ispack">check function of subpack existed</param>
        public void Extract(string path,MainForm.StatusShow showstatus)
        {
            string dest_path=CreateDirectory(path);
            foreach (Element e in elements)
            {
                if (ispack(e))
                {
                    Pack p=new Pack(e,pack_algo,ispack,comp_algo,iscompress);
                    p.Extract(dest_path, showstatus);
                }
                else
                {
                    if (iscompress(e))
                    {
                        e.Output(dest_path, iscompress, comp_algo);
                    }
                    else
                    {
                        e.Output(dest_path);
                    }
                }
                showstatus(Path.Combine(dest_path, e.Name));
            }
        }

        //extract pack
        private string CreateDirectory(string path)
        {
            
            string dest=Path.Combine(path,base.Name);

            //auto rename existed directory
            string dest_origin = dest;
            int i = 1;
            while (Directory.Exists(dest))
            {
                dest = dest_origin + i.ToString();
                i++;
            } 

            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }

            return dest;
        }
    }
}
