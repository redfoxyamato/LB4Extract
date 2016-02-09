
namespace LB4Extract
{
    public class LB4Extractor
    {
        string file;

        LB4ArchiveInfo info;

        public LB4Extractor(string fileName)
        {
            file = fileName;
            info = new LB4ArchiveInfo(file);
        }
        public string getName()
        {
            return file;
        }
        public LB4ArchiveInfo getArchiveInfo()
        {
            return info;
        }
    }
}
