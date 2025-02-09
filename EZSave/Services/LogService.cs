using System;
using System.IO;
using System.Text.Json;
using EZSave.Core.Models;
using System.Collections.Generic;

namespace EZSave.Core.Services
{
    public class LogService
    {
        public void Write(LogModel logModel, ConfigFileModel configModel)
        {

            string logDirectory = configModel.LogFileDestination;
            if (string.IsNullOrWhiteSpace(logDirectory))
            {
                throw new ArgumentNullException("Le chemin du répertoire de logs est vide ou non défini dans configFileModel.");
            }
            string logFilePath = Path.Combine(logDirectory, DateTime.Now.ToString("yyyyMMdd")+"_log.json");

            // Verif si dossier existe
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            if (!File.Exists(logFilePath))
            {
                using (File.Create(logFilePath)) { } // Ferme immédiatement le fichier
            }
            string existingJson = File.ReadAllText(logFilePath).Trim();
            List<LogModel> logModels = new List<LogModel>();
            if (!string.IsNullOrWhiteSpace(existingJson))
            {
                logModels = JsonSerializer.Deserialize<List<LogModel>>(existingJson) ?? new List<LogModel>();
            }

            logModels.Add(logModel);

            string jsonString = JsonSerializer.Serialize(logModels, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(logFilePath, jsonString);
                 
        }
    }
}