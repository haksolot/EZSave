namespace EZSave.Core.Models
{
    public class StatusModel
    {
        public string Name { get; set; } = "";
        public string State { get; set; } = "";
        public int Progress { get; set; }
        public int TotalFilesSize { get; set; }
        public int TotalFilesToCopy { get; set; }
    }
}