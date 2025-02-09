namespace EZSave.Core.Models
{
    public class StatusModel
    {
        public string Name { get; set; } = "";
        public string SourceFilePath { get; set; } = "";
        public string TargetFilePath { get; set; } = "";
        public string State { get; set; } = "";
        public int TotalFilesSize { get; set; }
        public int TotalFilesToCopy { get; set; }
        public int Progression { get; set; }
        public int FilesLeftToCopy { get; set; }
        public int FilesSizeLeftToCopy { get; set; }
    }
}