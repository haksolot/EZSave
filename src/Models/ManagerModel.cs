namespace EZSave.Core.Models
{
  public class ManagerModel
  {
        //public static readonly ManagerModel Manager = new ManagerModel();

    private static ManagerModel? _instance;
    private static readonly object _lockObj = new object();

        public List<JobModel> Jobs { get; set; } = [];
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
