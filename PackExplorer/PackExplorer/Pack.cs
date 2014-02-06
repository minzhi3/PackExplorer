using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace PackExplorer
{
    abstract class Pack : Element
    {
        protected List<Entry> entries;
        protected Int64 count;
        public Pack(Stream sm)
            :base(sm)
        {
        }
        public Pack(Stream sm,long offset,long size,string name)
            : base(sm, offset, size, name)
        {
        }
        public abstract void Analyse();

        //extract pack
        public string CreateDirectory(string path)
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
