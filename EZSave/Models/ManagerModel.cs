namespace EZSave.Core.Models
{
    public class ManagerModel
    {
        public List<JobModel> Jobs { get; set; } = [];
        public int Limit { get; set; } = 5;

    }
}
