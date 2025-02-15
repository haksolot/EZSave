namespace EZSave.Core.Models
{
  public class ConfigFileModel
  {
    public string? ConfFileDestination { get; set; }
    public string? LogFileDestination { get; set; } = "Log";
    public string? LogType { get; set; } = "xml";
    public string? StatusFileDestination { get; set; } = "Status";
    public Dictionary<string, JobModel>? Jobs { get; set; } = new();
  }
}
