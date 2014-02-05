using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackExplorer
{
    class Entry
    {
        long offset;
        long size;
        string name;

        //构造函数
        public Entry(Int64 offset, Int64 size, string name)
        {
            this.offset = offset;
            this.name = name;
            this.size = size;
        }

        //属性
        public long Offset
        {
            get { return offset; }
        }
        public long Size
        {
            get { return size; }
        }
        public string Name
        {
            get { return name; }
        }
    }
}
