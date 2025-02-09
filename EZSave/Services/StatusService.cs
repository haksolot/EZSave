using System;
using EZSave.Core.Models;
using System.Text.Json;

namespace EZSave.Core.Services
{
    public class StatusService
    {
        public void SaveStatus(StatusModel statusmodel, ConfigFileModel configmodel)
        {
            var liststatus = new Dictionary<string,StatusModel>();

            string statusDirectory = configmodel.StatusFileDestination;
            string statusFilePath = Path.Combine(statusDirectory, "_status.json");
            
            //Verif dossier existe
            if (!Directory.Exists(statusDirectory))
            {
                Directory.CreateDirectory(statusDirectory);
            }

            // Changer seulement les status existants  
            if (File.Exists(statusFilePath))
            {
                string json = File.ReadAllText(statusFilePath);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    liststatus = JsonSerializer.Deserialize<Dictionary<string, StatusModel>>(json) ?? new Dictionary<string, StatusModel>();
                }
            }
            //else
            //{
            //    using (File.Create(statusFilePath)) { }
            //}
            // Verif si le statuc existe dans le fichier 
            if (liststatus.ContainsKey(statusmodel.Name))
            {
                // upadte les valeurs du status
                liststatus[statusmodel.Name].SourceFilePath = statusmodel.SourceFilePath;
                liststatus[statusmodel.Name].TargetFilePath = statusmodel.TargetFilePath;
                liststatus[statusmodel.Name].State = statusmodel.State;
                liststatus[statusmodel.Name].TotalFilesSize = statusmodel.TotalFilesSize;
                liststatus[statusmodel.Name].TotalFilesToCopy = statusmodel.TotalFilesToCopy;
                liststatus[statusmodel.Name].FilesLeftToCopy = statusmodel.FilesSizeLeftToCopy;
                liststatus[statusmodel.Name].FilesSizeLeftToCopy = statusmodel.FilesSizeLeftToCopy;
                decimal progress = (decimal)(statusmodel.TotalFilesToCopy - statusmodel.FilesLeftToCopy) / statusmodel.TotalFilesSize * 100;
                liststatus[statusmodel.Name].Progression = (int)Math.Round(progress, MidpointRounding.AwayFromZero);
            }
            else
            {
                decimal progress = (decimal)(statusmodel.TotalFilesSize - statusmodel.FilesSizeLeftToCopy) / statusmodel.TotalFilesSize * 100;
                statusmodel.Progression = (int)Math.Round(progress, MidpointRounding.AwayFromZero);

                // sinon ajoute le statut
                liststatus.Add(statusmodel.Name, statusmodel);
            }

            string jsonString = JsonSerializer.Serialize(liststatus, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(statusFilePath, jsonString);
        }
    }
}