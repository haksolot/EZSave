﻿using EZSave.Core.Models;
using System.Text.Json;
using System.Xml.Serialization;

namespace EZSave.Core.Services
{
    public class LogService
    {
        private static readonly object obj = new object();

        public void WriteJSON(LogModel logModel, ConfigFileModel configModel, string logFilePath)
        {
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

        public void WriteXML(LogModel logModel, ConfigFileModel configModel, string logFilePath)
        {
            List<LogModel> logModels = new List<LogModel>();

            lock (obj)
            {
                if (File.Exists(logFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<LogModel>));

                    using (FileStream fileStream = new FileStream(logFilePath, FileMode.Open))
                    {
                        if (fileStream.Length > 0)
                        {
                            logModels = (List<LogModel>)serializer.Deserialize(fileStream) ?? new List<LogModel>();
                        }
                    }
                }

                logModels.Add(logModel);

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<LogModel>));
                using (StreamWriter writer = new StreamWriter(logFilePath))
                {
                    xmlSerializer.Serialize(writer, logModels);
                }
            }
        }

        public void Write(LogModel logModel, ConfigFileModel configModel)
        {
            string? logDirectory = configModel.LogFileDestination;
            string logFilePath = Path.Combine(logDirectory, DateTime.Now.ToString("yyyyMMdd") + "_log." + configModel.LogType);

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            if (!File.Exists(logFilePath))
            {
                using (File.Create(logFilePath)) { } // Ferme immédiatement le fichier
            }
            if (configModel.LogType.ToLower() == "json")
            {
                WriteJSON(logModel, configModel, logFilePath);
            }
            else if (configModel.LogType.ToLower() == "xml")
            {
                WriteXML(logModel, configModel, logFilePath);
            }
            else
            {
                throw new ArgumentException("Type de log non pris en charge : " + configModel.LogType);
            }
        }
    }
}