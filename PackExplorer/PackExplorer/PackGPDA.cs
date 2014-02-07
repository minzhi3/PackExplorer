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

        string dest_path;
        public PackGPDA(FileStream sm) : base(sm) 
        {
            FileInfo fi = new FileInfo(sm.Name);
            dest_path = fi.DirectoryName;
        }
        public PackGPDA(Stream sm, Element e, string dest) : base(sm, e.Offset, e.Size, e.Name) 
        {
            dest_path = dest;
        }

        public override void Analyse()
        {
            BinaryReader br = new BinaryReader(base.datastream); //file stream
            
            //check type
            if (!CheckType(base.datastream))
            {
                throw new Exception("Not a GPDA pack");
            }
            //File size
            if (base.Size != br.ReadInt64())
            {
                throw new Exception("Format not match (Length at 0x4)");
            }

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

            string file_dest = CreateDirectory(dest_path);
            foreach (Entry e in entries)
            {
                byte[] buf = new byte[e.Size];

                br.BaseStream.Seek(e.Offset, SeekOrigin.Begin);
                br.Read(buf, 0, (int)e.Size);

                MemoryStream ms = new MemoryStream(buf);
                if (CheckType(ms))
                {
                    PackGPDA pack = new PackGPDA(ms, e, file_dest);
                    pack.Analyse();
                    //pack.Output(dest_path);
                }
                else
                {
                    Element leaf = new Element(ms, e);
                    leaf.Output(file_dest);
                }
            }


        }

        public static bool CheckType(Stream packstream)
        {
            BinaryReader br = new BinaryReader(packstream); //file stream
            br.BaseStream.Seek(0, SeekOrigin.Begin);
            //Check IDString GPDA
            byte[] chk = br.ReadBytes(4);
            for (int i = 0; i < 3; i++)
            {
                if (chk[i] != Idstring[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
