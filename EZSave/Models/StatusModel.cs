namespace EZSave.Core.Models
{
    public class StatusModel
    {
        public string Name { get; set; } = "";
        public string State { get; set; } = "";
        public double Progress { get; set; }
        public double Size { get; set; }
        public double TotalTransferTime { get; set; }
        public int TotalFilesToCopy { get; set; }
    }
}