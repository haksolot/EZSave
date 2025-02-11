namespace EZSave.Core.Models
{
  public class ConfigFileModel
  {
    public string? ConfFileDestination { get; set; }
    public string? LogFileDestination { get; set; }
    public string? LogType { get; set; }
    public string? StatusFileDestination { get; set; }
    public Dictionary<string, JobModel>? Jobs { get; set; } = new();
  }
}
