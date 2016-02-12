# Overview
.NET library to extract LB4 archive

#Sample code
```

LB4Extractor ext = new LB4Extractor(lb4Path); //make instance to extract archive.
foreach(LB4Entry entry in ext.getArchiveInfo().Entries) //get entry in archive file.
{
    File.WriteAllBytes(string.Format("{0}\\{1}", extractPath, entry.Name), entry.Buffer); //Write entry into file.
}
```
