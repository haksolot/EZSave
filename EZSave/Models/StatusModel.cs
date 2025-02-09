namespace EZSave.Core.Models
{
    public class StatusModel
    {
        public string Name { get; set; } = "";
        public string SourceFilePath { get; set; } = "";
        public string TargetFilePath { get; set; } = "";
        public string State { get; set; } = "";
        public long TotalFilesSize { get; set; }
        public int TotalFilesToCopy { get; set; }
        public int Progression { get; set; }
        public int FilesLeftToCopy { get; set; }
        public long FilesSizeLeftToCopy { get; set; }
    }
}