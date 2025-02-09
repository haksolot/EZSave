namespace EZSave.Core.Models
{
    public class StatusModel
    {
        public string Name { get; set; } = "";
        public string State { get; set; } = "";
        public int Progress { get; set; }
        public int Size { get; set; }
        public int TotalTransferTime { get; set; }
        public int TotalFilesToCopy { get; set; }
    }
}