using System;
using System.IO;
using System.Text.Json;
using EZSave.Core.Models;


public class LogService
{


    public void Write(LogModel logModel,ConfigFileModel configModel)
    {
        string logDirectory = configModel.LogFileDestination;
        string logFilePath = Path.Combine(logDirectory, "_log.json");

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }


        string jsonString = JsonSerializer.Serialize(logModel);
        if (!File.Exists(logFilePath))
        {
            File.WriteAllText(logFilePath, "[" + jsonString);
        }
        else
        {
            File.AppendAllText(logFilePath, "," + jsonString);
        }

        File.AppendAllText(logFilePath, "]");

        Console.WriteLine($"Log ajouté : {Path.GetFullPath(logDirectory)}");
    }

}
