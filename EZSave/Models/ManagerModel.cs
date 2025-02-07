namespace EZSave.Core.Models
{
    public class ManagerModel
    {
        //public static readonly ManagerModel Manager = new ManagerModel();

        public List<JobModel> Jobs { get; set; } = [];
        public int Limit { get; set; } = 5;
        
    }
}
