# Overview
.NET library to extract LB4 archive

# Download Executable Utility
CUI : <https://github.com/redfoxyamato/PM2Extract/releases><br>
GUI : <https://github.com/redfoxyamato/PM2ExtractGui/releases>

# Sample code
```C#

LB4Extractor ext = new LB4Extractor(lb4Path); //make instance to extract archive.
foreach(LB4Entry entry in ext.getArchiveInfo().Entries) //get entry in archive file.
{
    File.WriteAllBytes(string.Format("{0}\\{1}", extractPath, entry.Name), entry.Buffer); //Write entry into file.
}
```
