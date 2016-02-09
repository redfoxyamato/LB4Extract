
namespace LB4Extract
{
    public class LB4Entry
    {

        private bool initialized;
        public bool Initialized
        {
            get { return initialized; }
        }

        private ushort content_size;
        public ushort Size
        {
            get { return content_size; }
        }

        private string content_name;
        public string Name
        {
            get { return content_name; }
        }

        private byte[] buffer;
        public byte[] Buffer
        {
            get { return buffer; }
        }

        public LB4Entry(ushort size,string name)
        {
            content_size = size;
            content_name = name;
        }

        public bool SetContents(byte[] contents)
        {
            if(initialized)
            {
                return false;
            }
            buffer = contents;
            initialized = true;
            return true;
        }
    }
}
