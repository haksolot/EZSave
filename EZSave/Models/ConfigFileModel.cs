namespace EZSave.Core.Models
{
    public class ConfigFileModel
    {
        public string ConfFileDestination { get; set; } = string.Empty;
        public string LogFileDestination { get; set; } = string.Empty;
        public string LogType { get; set; } = "xml";
        public string StatusFileDestination { get; set; } = string.Empty;
        public Dictionary<string, JobModel> Jobs { get; set; } = new();
    }
}
