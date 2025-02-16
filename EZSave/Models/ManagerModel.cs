using System.Collections.ObjectModel;

namespace EZSave.Core.Models
{
  public class ManagerModel
  {
    public ObservableCollection<JobModel> Jobs { get; set; } = new ObservableCollection<JobModel>();
    public int Limit { get; set; }
  }
}
