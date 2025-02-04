using System;
using System.IO;
using System.Text.Json;


public class Log
{
    public string name { get; set; }
    public DateTime datetime { get; set; }
    public string source { get; set; }
    public string destination { get; set; }
    public float size { get; set; }
    public float tt { get; set; }
    public string message {get; set; }

    public void write()
    {
        string logDirectory = $"logs/{name}";

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        string logfilename = $"{logDirectory}/{name}_{datetime:yyyyMMdd}_log.json";
        string jsonString = JsonSerializer.Serialize(this);


        File.AppendAllText(logfilename, jsonString + Environment.NewLine);

        Console.WriteLine($"Log ajouté : {logfilename}");
    }

    public void show()
    {
        string logDirectory = $"logs/{name}";
        Console.WriteLine(File.ReadAllText($"{logDirectory}/{name}_{datetime:yyyyMMdd}_log.json"));
    }
}
