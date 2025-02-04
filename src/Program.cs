using EZSave.Managers;
using EZSave.Jobs;

using System.Globalization;

class Program
{
  static void Main()
  {
    //string sourcePath = @"C:\Users\estel\Desktop\";
    //string destinationPath = @"C:\Users\estel\Desktop\";

    /*Job fullBackup = new Job("Full Backup", sourcePath, destinationPath, "full");*/
    /*fullBackup.start();*/

    Job diffBackup = new Job("Test Backup", "e", "e", "differential");
        //diffBackup.start();

        //string confPath = @"C:\Users\estel\Desktop\";
        /*fullBackup.save(confPath);*/
        //diffBackup.save(confPath);
        Manager manager = Manager.GetInstance(5, "e");
        manager.add(diffBackup);
            manager.show();
        manager.delete(diffBackup);

    }
}
