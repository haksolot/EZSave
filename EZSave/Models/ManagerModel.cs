using System.Collections.ObjectModel;

namespace EZSave.Core.Models
{
    public class ManagerModel
    {
        public ObservableCollection<JobModel> Jobs { get; private set; } = new();

        public ManagerModel()
        {
            Jobs = new ObservableCollection<JobModel>();
        }
    }
}
