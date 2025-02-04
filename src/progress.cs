using System;

public class Progress
{
    public int FilesLeft { get; private set; }
    public float TotalFileSize { get; private set; } 
    public string Source { get; private set; }
    public string Destination { get; private set; }

    public Progress(int totalFiles, float totalSize, string source, string destination)
    {
        FilesLeft = totalFiles;
        TotalFileSize = totalSize;
        Source = source;
        Destination = destination;
    }

    public void UpdateProgress(int filesTransferred)
    {
        FilesLeft -= filesTransferred;
        if (FilesLeft < 0) FilesLeft = 0;
    }

    public bool IsComplete()
    {
        return FilesLeft == 0;
    }

    public void ShowProgress()
    {
        Console.WriteLine($"Progress: {FilesLeft} files left to transfer from {Source} to {Destination}.");
    }
}
