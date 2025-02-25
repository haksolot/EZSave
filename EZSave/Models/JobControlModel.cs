using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZSave.Core.Models
{
    public class JobControlModel
    {
        public string Name { get; set; }
        public Dictionary<string, (Thread Thread, CancellationTokenSource Cts, ManualResetEvent PauseEvent, string Status)> selected { get; set; }
    }

    public class PlayJobModel
    {
        public string Name { get; set; }
        public ObservableCollection<string> selected { get; set; }
    }
}
