using EZSave.Core.Models;
using System.Diagnostics;
using System.Linq;

namespace EZSave.Core.Services
{
  public static class ProcessesService
  {
    public static bool CheckProcess(string processName) // Check is processName is running on system
    {
      bool isRunning = Process.GetProcessesByName(processName).Any();
      return isRunning;
    }
  }
}
