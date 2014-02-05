using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace PackExplorer
{
    class PackGPDA:Pack
    {
        static byte[] Idstring = Encoding.ASCII.GetBytes("GPDA");
        public PackGPDA(Stream sm) : base(sm) { }
        public override void Analyse()
        {
            BinaryReader br = new BinaryReader(packstream);
            br.BaseStream.Seek(0, SeekOrigin.Begin);
            //Check IDString GPDA
            byte[] chk=br.ReadBytes(4);
            for (int i=0;i<3;i++)
            {
                if (chk[i]!=Idstring[i])
                {
                    throw new Exception("Not a GPDA pack");
                }
            }

            //File size
            size = br.ReadInt64();

            //Entries count
            count = br.ReadInt32();
            entries = new List<Entry>();

            //Read entry
            for (int i=0;i<count;i++)
            {
                long e_offset = br.ReadInt64();
                int e_size = br.ReadInt32();
                int e_name_offset = br.ReadInt32();
                
                //read name
                long pos = br.BaseStream.Position;
                br.BaseStream.Seek(e_name_offset, SeekOrigin.Begin);
                int name_length = br.ReadInt32();
                string e_name = new string(br.ReadChars(name_length));
                entries.Add(new Entry(e_offset, e_size, e_name));
                br.BaseStream.Seek(pos,SeekOrigin.Begin);
                //Debug.WriteLine(entries[i].Name);
            }



        }
    }
}
