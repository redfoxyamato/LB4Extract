
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace LB4Extract
{
    public class LB4ArchiveInfo
    {
        private ushort count;
        
        /// <summary>
        /// Name of archive file.
        /// </summary>

        private readonly string fileName;
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

        private bool outputDebug;
        public bool DebugMode
        {
            get { return outputDebug; }
        }

        private byte[] buffer;

        public LB4ArchiveInfo(string name)
        {
            fileName = name;
            entries = new List<LB4Entry>();
            outputDebug = true;
            if (File.Exists(name))
            {
                LoadFile();
            }
        }
        public void DisableDebug()
        {
            outputDebug = false;
        }
        public void LoadFile()
        {
            buffer = File.ReadAllBytes(Name);

            byte[] fileUnknownBytes = Util.SubBytes(buffer, 0, 2);
            ushort unknown = (ushort)Util.getLEValue(fileUnknownBytes);

            byte[] fileCountBytes = Util.SubBytes(buffer,4,2);
            count = (ushort)Util.getLEValue(fileCountBytes);

            Util.debug(this, "[Archive Info]");
            Util.debug(this, string.Format("Entry Count:{0}",Util.formatInt(count)));
            Util.debug(this, "");

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

                Util.debug(this,string.Format("[File Info]New file found."));
                Util.debug(this,string.Format("Name:{0}",name_str));
                Util.debug(this,string.Format("Size:{0}",Util.formatInt(file_size)));
            }

            //Start loading file contents
            for(int i = 0;i < count;i++)
            {
                LB4Entry entry = entries[i];

                Util.debug(this, string.Format("[File contents]New contents found."));
                Util.debug(this, string.Format("Name:{0}",entry.Name));
                Util.debug(this, string.Format("Start index:{0}",Util.formatInt(index)));

                uint first_index = index;

                byte[] contents = Util.SubBytes(buffer,index,entry.Size);
                index += entry.Size;

                Util.debug(this,string.Format("End index:{0}",Util.formatInt(index - 1)));
                Util.debug(this,string.Format("Diff:{0}",Util.formatInt(index - first_index)));
                Util.debug(this,"");

                entries[i].SetContents(contents);
            }
        }
        public List<LB4Entry> GetMatchedEntries(string filter)
        {
            string regex = convertWildcardToRegex(filter);
            List<LB4Entry> list = new List<LB4Entry>();
            foreach(LB4Entry entry in entries)
            {
                if(Regex.IsMatch(entry.Name,regex))
                {
                    list.Add(entry);
                }
            }
            return list;
        }
        public string convertWildcardToRegex(string wildcard)
        {
            return Regex.Replace(wildcard,
                ".",
                m =>
                {
                    string s = m.Value;
                    if (s.Equals("?"))
                    {
                        //?は任意の1文字を示す正規表現(.)に変換
                        return ".";
                    }
                    else if (s.Equals("*"))
                    {
                        //*は0文字以上の任意の文字列を示す正規表現(.*)に変換
                        return ".*";
                    }
                    else
                    {
                        //上記以外はエスケープする
                        return Regex.Escape(s);
                    }
                });
        }
    }
}
