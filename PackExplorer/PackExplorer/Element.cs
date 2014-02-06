using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PackExplorer
{
    class Element : Entry
    {
        protected Stream datastream;

        public Element(Stream sm)
            : base(0, sm.Length, "ROOT")
        {
            datastream = sm;
        }

        public Element(Stream sm, Entry e)
            : base(e.Offset, e.Size, e.Name)
        {
            datastream = sm;
        }
        public Element(Stream sm, long offset,long size,string name)
            :base(offset,size,name)
        {
            datastream = sm;
        }

        public void Output(string path)
        {
            string dest=Path.Combine(path, base.Name);

            //auto rename existed file
            string dest_origin = dest;
            int i=1;
            while (File.Exists(dest))
            {
                dest = dest_origin + i.ToString();
                i++;
            }
            FileStream fso = File.Create(dest);
            datastream.CopyTo(fso);
            fso.Close();
        }
    }
}
