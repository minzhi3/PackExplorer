using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace PackExplorer
{
    abstract class Pack
    {
        protected Stream packstream;
        protected List<Entry> entries;
        protected Int64 size;
        protected Int64 count;
        public Pack(Stream sm)
        {
            packstream = sm;
            Analyse();
        }
        public abstract void Analyse();
    }
}
