using System;
using System.IO;
using System.Text.Json;
using EZSave.Core.Models;


public class LogService
{


    public void Write(LogModel model)
    {
        string logDirectory = $"logs/{model.Name}";

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        string logfilename = $"{logDirectory}/{model.Name}_{model.Timestamp:yyyyMMdd}_log.json";
        string jsonString = JsonSerializer.Serialize(model);


        File.AppendAllText(logfilename, jsonString + Environment.NewLine);

        Console.WriteLine($"Log ajouté : {logfilename}");
    }

    public void Show(LogModel model)
    {
        string logDirectory = $"logs/{model.Name}";
        Console.WriteLine(File.ReadAllText($"{logDirectory}/{model.Name}_{model.Timestamp:yyyyMMdd}_log.json"));
    }
}
