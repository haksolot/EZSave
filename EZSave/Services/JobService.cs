using EZSave.Core.Models;

namespace EZSave.Core.Services
{
  public class JobService
  {
    public void Start(JobModel job, StatusService statusService, LogService logService, ConfigFileModel configFileModel)
    {
      if (job.Type == "full")
      {
        FullBackup(job, logService, statusService, configFileModel);

      }
      else if (job.Type == "diff")
      {
        DifferentialBackup(job, statusService, logService, configFileModel);
      }
      else
      {

      }
    }

    private void FullBackup(JobModel job, LogService logService, StatusService statusService, ConfigFileModel configFileModel)
    {
      long copiedSize = 0;
      var startTime = DateTime.Now;
      long currentFileSize = 0;

      long totalSize = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories).Sum(file => new FileInfo(file).Length);

      int totalFiles = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories).Length;

      statusService.SaveStatus(new StatusModel
      {
        Name = job.Name,
        SourceFilePath = job.Source,
        TargetFilePath = job.Destination,
        State = "Activate",
        TotalFilesSize = totalSize,
        TotalFilesToCopy = totalFiles,
        FilesLeftToCopy = totalFiles,
        FilesSizeLeftToCopy = totalSize
      }, configFileModel);

      foreach (string file in Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories))
      {
        string relativePath = file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar);
        string destinationFile = Path.Combine(job.Destination, relativePath);
        string? directoryPath = Path.GetDirectoryName(destinationFile);
        long fileSize = new FileInfo(file).Length;

        if (!string.IsNullOrEmpty(directoryPath))
        {
          Directory.CreateDirectory(directoryPath);
        }
        File.Copy(file, destinationFile, true);


        totalSize -= fileSize;
        totalFiles--;

        statusService.SaveStatus(new StatusModel
        {
          Name = job.Name,
          SourceFilePath = job.Source,
          TargetFilePath = job.Destination,
          State = "End",
          TotalFilesSize = totalSize + fileSize,
          TotalFilesToCopy = totalFiles + 1,
          FilesLeftToCopy = totalFiles,
          FilesSizeLeftToCopy = totalSize
        }, configFileModel);

        var endTime = DateTime.Now;

        float transferTime = (float)(endTime - startTime).TotalSeconds;



        currentFileSize = new FileInfo(destinationFile).Length;

        copiedSize += currentFileSize;

        logService.WriteJSON(new LogModel
        {
          Name = job.Name,
          Timestamp = DateTime.Now,
          FileSource = file,
          FileDestination = destinationFile,
          FileSize = currentFileSize,
          FileTransferTime = transferTime
        }, configFileModel);
      }
    }




    private void DifferentialBackup(JobModel job, StatusService statusService, LogService logService, ConfigFileModel configFileModel)
    {
      long copiedSize = 0;
      var startTime = DateTime.Now;
      long currentFileSize = 0;

      long totalSize = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories).Sum(file => new FileInfo(file).Length);

      int totalFiles = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories).Length;

      statusService.SaveStatus(new StatusModel
      {
        Name = job.Name,
        SourceFilePath = job.Source,
        TargetFilePath = job.Destination,
        State = "Activate",
        TotalFilesSize = totalSize,
        TotalFilesToCopy = totalFiles,
        FilesLeftToCopy = totalFiles,
        FilesSizeLeftToCopy = totalSize
      }, configFileModel);

      foreach (string file in Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories))
      {
        string relativePath = file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar);
        string destinationFile = Path.Combine(job.Destination, relativePath);
        long fileSize = new FileInfo(file).Length;
        if (!File.Exists(destinationFile) || File.GetLastWriteTime(file) > File.GetLastWriteTime(destinationFile))
        {
          string? directoryPath = Path.GetDirectoryName(destinationFile);
          if (!string.IsNullOrEmpty(directoryPath))
          {
            Directory.CreateDirectory(directoryPath);
          }
          /*Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));*/
          File.Copy(file, destinationFile, true);

          totalSize -= fileSize;
          totalFiles--;

          statusService.SaveStatus(new StatusModel
          {
            Name = job.Name,
            SourceFilePath = job.Source,
            TargetFilePath = job.Destination,
            State = "End",
            TotalFilesSize = totalSize + fileSize,
            TotalFilesToCopy = totalFiles + 1,
            FilesLeftToCopy = totalFiles,
            FilesSizeLeftToCopy = totalSize
          }, configFileModel);

          var endTime = DateTime.Now;

          float transferTime = (float)(endTime - startTime).TotalSeconds;
          currentFileSize = new FileInfo(destinationFile).Length;

          copiedSize += currentFileSize;

          logService.WriteJSON(new LogModel
          {
            Name = job.Name,
            Timestamp = DateTime.Now,
            FileSource = file,
            FileDestination = destinationFile,
            FileSize = currentFileSize,
            FileTransferTime = transferTime
          }, configFileModel);
        }
      }
    }
  }
}

