using System;
using EZSave.Core.Models;
using System.Text.Json;

namespace EZSave.Core.Services
{
  public class StatusService
  {
    public void SaveStatus(StatusModel statusmodel, ConfigFileModel configmodel)
    {
      var liststatus = new Dictionary<string, StatusModel>();

      string statusDirectory = configmodel.StatusFileDestination;
      string statusFilePath = Path.Combine(statusDirectory, "_status.json");

      if (!Directory.Exists(statusDirectory))
      {
        Directory.CreateDirectory(statusDirectory);
      }

      if (File.Exists(statusFilePath))
      {
        string json = File.ReadAllText(statusFilePath);
        if (!string.IsNullOrWhiteSpace(json))
        {
          liststatus = JsonSerializer.Deserialize<Dictionary<string, StatusModel>>(json) ?? new Dictionary<string, StatusModel>();
        }
      }
      if (liststatus.ContainsKey(statusmodel.Name))
      {
        liststatus[statusmodel.Name].SourceFilePath = statusmodel.SourceFilePath;
        liststatus[statusmodel.Name].TargetFilePath = statusmodel.TargetFilePath;
        liststatus[statusmodel.Name].State = statusmodel.State;
        liststatus[statusmodel.Name].TotalFilesSize = statusmodel.TotalFilesSize;

        liststatus[statusmodel.Name].TotalFilesToCopy = statusmodel.TotalFilesToCopy;
        liststatus[statusmodel.Name].FilesLeftToCopy = statusmodel.FilesLeftToCopy;
        liststatus[statusmodel.Name].FilesSizeLeftToCopy = statusmodel.FilesSizeLeftToCopy;
        decimal progress = (decimal)(statusmodel.TotalFilesToCopy - statusmodel.FilesLeftToCopy) / statusmodel.TotalFilesToCopy * 100;
        liststatus[statusmodel.Name].Progression = (int)Math.Floor(progress);

      }
      else
      {
        decimal progress = (decimal)(statusmodel.TotalFilesToCopy - statusmodel.FilesSizeLeftToCopy) / statusmodel.TotalFilesToCopy * 100;
        statusmodel.Progression = (int)Math.Floor(progress);

        liststatus.Add(statusmodel.Name, statusmodel);
      }

      string jsonString = JsonSerializer.Serialize(liststatus, new JsonSerializerOptions { WriteIndented = true });
      File.WriteAllText(statusFilePath, jsonString);
    }
  }
}
