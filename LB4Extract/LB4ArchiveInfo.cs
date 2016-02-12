
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

namespace LB4Extract
{
    public class LB4ArchiveInfo
    {
        private string fileName;
        private ushort count;
        /// <summary>
        /// Name of archive file.
        /// </summary>
        string Name
        {
            get { return fileName; }
        }
        public ushort Count
        {
            get { return count; }
        }

        private List<LB4Entry> entries;
        public List<LB4Entry> Entries
        {
            get { return entries; }
        }

        private byte[] buffer;

        public LB4ArchiveInfo(string name)
        {
            fileName = name;
            entries = new List<LB4Entry>();
            LoadFile();
        }
        public void LoadFile()
        {
            buffer = File.ReadAllBytes(Name);

            byte[] fileUnknownBytes = Util.SubBytes(buffer, 0, 2);
            ushort unknown = (ushort)Util.getLEValue(fileUnknownBytes);

            byte[] fileCountBytes = Util.SubBytes(buffer,4,2);
            count = (ushort)Util.getLEValue(fileCountBytes);

            //Start loading file info
            uint index = 0x6;
            for(int i = 0;i < count;i++)
            {
                //1-bit File name's count
                byte name_count = buffer[index];
                index++;

                //Any-bit File name
                byte[] file_name = Util.SubBytes(buffer, index, name_count);
                string name_str = Encoding.ASCII.GetString(file_name);
                index += name_count;
                
                //2-bit File size
                byte[] file_size_bytes = Util.SubBytes(buffer, index, 2);
                ushort file_size = (ushort)Util.getLEValue(file_size_bytes);
                index += 2;

                entries.Add(new LB4Entry(file_size,name_str));

                Util.debug(string.Format("[File Info]New item found."));
                Util.debug(string.Format("Name:{0}",name_str));
                Util.debug(string.Format("Size:{0}",Util.formatInt(file_size)));
                Util.debug("");
            }

            //Start loading file contents
            for(int i = 0;i < count;i++)
            {
                LB4Entry entry = entries[i];

                Util.debug(string.Format("[File contents]New contents found."));
                Util.debug(string.Format("Start index:{0}",Util.formatInt(index)));

                uint first_index = index;

                byte[] contents = Util.SubBytes(buffer,index,entry.Size);
                index += entry.Size;

                Util.debug(string.Format("End index:{0}",Util.formatInt(index - 1)));
                Util.debug(string.Format("Diff:{0}",Util.formatInt(index - first_index)));
                Util.debug("");

                entries[i].SetContents(contents);
            }
            File.AppendAllText("C:/debug.txt", (buffer.Length / unknown) + "\r\n");
        }
    }
}
