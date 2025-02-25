using System.Collections.ObjectModel;

namespace EZSave.Core.Models
{
    public class ConfigFileModel
    {
        public string? ConfFileDestination { get; set; }
        public string? LogFileDestination { get; set; } = "Log";
        public string? LogType { get; set; } = "xml";
        public string? StatusFileDestination { get; set; } = "Status";
        public Dictionary<string, JobModel>? Jobs { get; set; } = new();

        public long FileSizeThreshold { get; set; } = 1024 * 1024; 
    }
}