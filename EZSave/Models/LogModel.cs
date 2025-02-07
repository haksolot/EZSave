using System;

public class LogModel
{
    public string Name { get; set; } = "";
    public DateTime Timestamp { get; set; }
    public string Source { get; set; } = "";
    public string Destination { get; set; } = "";
    public float Size { get; set; }
    public float TT { get; set; }
    public string Message { get; set; } = "";
}
