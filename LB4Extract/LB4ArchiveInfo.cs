
using System.Collections.Generic;
using System.Text;
using System.IO;

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
            }

            //Start loading file contents
            for(int i = 0;i < count;i++)
            {
                LB4Entry entry = entries[i];

                byte[] contents = Util.SubBytes(buffer,index,entry.Size);
                index += entry.Size;

                entries[i].SetContents(contents);
            }
            File.AppendAllText("C:/debug.txt", (buffer.Length / unknown) + "\r\n");
        }
    }
}
