using System.Diagnostics;
using EZSave.Core.Services;
using Xunit;

namespace EZSave.Tests
{
  public class ProcessesTests
  {
    [Fact]
    public void testCheckProcess()
    {
      using (Process calc = Process.Start("calc.exe"))
      {
        try
        {
          System.Threading.Thread.Sleep(500);
          bool isRunning = ProcessesService.CheckProcess("CalculatorApp");
          Assert.True(isRunning);
        }
        finally
        {
          calc.Kill();
          calc.WaitForExit();
        }
      }
    }
  }
}
