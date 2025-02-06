namespace EZSave.Core.Models
{
    public class ManagerModel
    {
        private static ManagerModel _instance;
        private static readonly object _lockObj = new object();

        public List<Job> jobs { get; set; } = new List<Job>();
        public int limit { get; set; } = 5;

        private ManagerModel() { }

        public static ManagerModel Instance
        {
            get
            {
                lock (_lockObj)
                {
                    return _instance ??= new ManagerModel();
                }
            }
        }
    }
}
