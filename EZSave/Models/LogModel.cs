namespace EZSave.Core.Models
{
    public class LogModel
    {
        public string Name { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public string FileSource { get; set; } = "";
        public string FileDestination { get; set; } = "";
        public float FileSize { get; set; }
        public float FileTransferTime { get; set; }
    }
}
