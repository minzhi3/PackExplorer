using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PackExplorer
{
    public class Element
    {
        protected Stream datastream;

        long offset;
        long size;
        string name;

        //construct------------------------------
        public Element(Stream sm, Int64 offset, Int64 size, string name)
        {
            Initial(sm, offset, size, name);
        }
        public Element(Stream sm)
        {
            Initial(sm, 0, sm.Length, "ROOT");
        }
        private void Initial(Stream sm, Int64 offset, Int64 size, string name)
        {
            datastream = sm;
            this.offset = offset;
            this.name = name;
            this.size = size;
        }

        //properties-------------------------------
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
        public Stream Data
        {
            get { return datastream; }
        }

        public void Output(string path)
        {
            string dest=Path.Combine(path, name);

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
        /// <summary>
        /// Get 4 bytes in head of stream
        /// </summary>
        /// <returns></returns>
        public byte[] Get4Bytes()  
        {
            BinaryReader br = new BinaryReader(datastream); //file stream
            br.BaseStream.Seek(0, SeekOrigin.Begin);
            //Check IDString GPDA
            byte[] head = br.ReadBytes(4);
            return head;
        }
    }
}
