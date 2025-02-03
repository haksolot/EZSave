class Program
{
  static void Main()
  {
    string sourcePath = @"C:\Users\seanl\Desktop\testBackup";
    string destinationPath = @"C:\Users\seanl\Desktop\cool";

    /*Job fullBackup = new Job("Full Backup", sourcePath, destinationPath, "full");*/
    /*fullBackup.start();*/

    Job diffBackup = new Job("Test Backup", sourcePath, destinationPath, "differential");
    diffBackup.start();

    string confPath = @"C:\Users\seanl\Desktop\conf.json";
    /*fullBackup.save(confPath);*/
    diffBackup.save(confPath);
  }
}
