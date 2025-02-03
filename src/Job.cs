using System;
using System.IO;

public class Job {
    public string Name { get; }
    public string Source { get; }
    public string Destination { get; }
    public string Type { get; }

    public Job(string name, string source, string destination, string type)
    {
        Name = name;
        Source = source;
        Destination = destination;

        if (type.ToLower() == "full" || type.ToLower() == "differential")
        {
            Type = type.ToLower();
        }
        else
        {
            throw new ArgumentException("Le type de sauvegarde doit être 'full' ou 'differential'.");
        }
    }

    public void start() {
        if (Type == "full") {
                PerformFullBackup();
            }
        else {
                PerformDifferentialBackup();
            }
    }
    
    private void PerformFullBackup() {
        foreach (string file in Directory.GetFiles(Source, "*", SearchOption.AllDirectories)) {
            string relativePath = file.Substring(Source.Length).TrimStart(Path.DirectorySeparatorChar);
            string destinationFile = Path.Combine(Destination, relativePath);
            
            Directory.CreateDirectory(Path.GetDirectoryName(destinationFile)); 
            File.Copy(file, destinationFile, true); 
        }
    }
    
    private void PerformDifferentialBackup() {
        foreach (string file in Directory.GetFiles(Source, "*", SearchOption.AllDirectories)) {
            string relativePath = file.Substring(Source.Length).TrimStart(Path.DirectorySeparatorChar);
            string destinationFile = Path.Combine(Destination, relativePath);
            if (!File.Exists(destinationFile) || File.GetLastWriteTime(file) > File.GetLastWriteTime(destinationFile)) {
                Directory.CreateDirectory(Path.GetDirectoryName(destinationFile)); 
                File.Copy(file, destinationFile, true); 
            }
        }
    }
}
