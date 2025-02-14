using EZSave.Core.Models;
using System.Diagnostics;
using System.Linq;

namespace EZSave.Core.Services
{
  public class ProcessesService
  {
    public bool CheckProcess(string processName) // Check is processName is running on system
    {
      bool isRunning = Process.GetProcessesByName(processName).Any();
      return isRunning;
    }
  }
}
