namespace EZSave.Core.Models
{
    public class StatusModel
    {
        public string Name { get; set; } = "";
        public string SourceFilePath { get; set; } = "";
        public string TargetFilePath { get; set; } = "";
        public string State { get; set; } = "";
<<<<<<< HEAD
=======
        public int Progress { get; set; }
>>>>>>> a6b514af9972fb709312585652c568b5e7be7eec
        public int TotalFilesSize { get; set; }
        public int TotalFilesToCopy { get; set; }
        public int Progression { get; set; }
        public int FilesLeftToCopy { get; set; }
        public int FilesSizeLeftToCopy { get; set; }
    }
}