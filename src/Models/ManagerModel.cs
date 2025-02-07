namespace EZSave.Core.Models
{
  public class ManagerModel
  {
    private static ManagerModel _instance;
    private static readonly object _lockObj = new object();

    public List<JobModel> Jobs { get; set; } = new List<JobModel>();
    public int Limit { get; set; } = 5;

    private ManagerModel() { }

    public static ManagerModel Instance
    {
      get
      {
        lock (_lockObj)
        {
          if (_instance == null)
          {
            _instance = new ManagerModel();
          }
          return _instance;
        }
      }
    }
  }
}
