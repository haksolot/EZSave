namespace EZSave.Core.Models
{
  public class JobModel
  {
    public string Name { get; set; } = "";
    public string Source { get; set; } = "";
    public string Destination { get; set; } = "";
    public string Type { get; set; } = "";

    public JobModel(string name, string source, string destination, string type)
    {
      Name = name;
      Source = source;
      Destination = destination;
      Type = type;
    }
  }
}
