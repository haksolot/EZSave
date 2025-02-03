using System;
using System.Text.Json;


public class Log
{
    public string name { get; set; }
    public DateTime datetime { get; set; }
    public string source { get; set; }
    public string destination { get; set; }
    public float size { get; set; }
    public float tt { get; set; }

    public void write()
    {
        var save = new Log
        {
            Jobname = name,
            Date = datetime,
            Source = source,
            Destination = destination,
            Size = size,
            TT = tt
        };
        string logfilename = name + "_" + datetime.ToString() + "_log";
        string jsonString = JsonSerializer.Serialize(save);
        File.WriteAllText(fileName, jsonString);

        Console.WriteLine(File.ReadAllText(fileName));
    }

    public void show()
    {

    }
}
