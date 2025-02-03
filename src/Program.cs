class Program
{
    static void Main()
    {
        string sourcePath = @"C:\Users\seanl\Desktop\testBackup"; 
        string destinationPath = @"C:\Users\seanl\Desktop\noice"; 

        Job fullBackup = new Job("Full Backup", sourcePath, destinationPath, "full");
        fullBackup.start();

        Job diffBackup = new Job("Differential Backup", sourcePath, destinationPath, "differential");
        diffBackup.start();
    }
}
