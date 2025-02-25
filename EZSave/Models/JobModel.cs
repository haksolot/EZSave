namespace EZSave.Core.Models
{
    public class JobModel
    {
        public string Name { get; set; } = "";
        public string Source { get; set; } = "";
        public string Destination { get; set; } = "";
        public string Type { get; set; } = "";
        public static SemaphoreSlim LockLargeFile = new SemaphoreSlim(1,1);
        public static long FileSizeThreshold { get; set; } = 1024 * 1024; 

    }
}
