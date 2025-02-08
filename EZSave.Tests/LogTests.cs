using System;
using EZSave.Core.Models;
using EZSave.Core.Services;

namespace EZSave.Tests
{
	public class LogTest
	{
		[Fact]
		public void WriteLogTest()
		{
			var testlog = new LogModel
			{
				Name = "test1",
				Timestamp = DateTime.Now,
				FileSource = "C:\\Users\\Utilisateur",
				FileDestination = "C:\\Users\\Utilisateur\\source\\repos\\haksolot\\EZSave",
				FileSize = 1024,
				FileTransferTime = 10000
			};
            var testlog2 = new LogModel
            {
                Name = "test2",
                Timestamp = DateTime.Now,
                FileSource = "C:\\Users\\Utilisateur",
                FileDestination = "C:\\Users\\Utilisateur\\source\\repos\\haksolot\\EZSave",
                FileSize = 1024,
                FileTransferTime = 10000
            };
            var testconfigfile = new ConfigFileModel
			{
				ConfFileDestination = "config",
				LogFileDestination = "logs"
			};

			
            var logService = new LogService();
			logService.Write(testlog, testconfigfile);
            logService.Write(testlog2, testconfigfile);

            string logDirectory = Path.Combine(testconfigfile.LogFileDestination);
            string expectedFilePath = Path.Combine(logDirectory, DateTime.Now.ToString("yyyyMMdd") + "_log.json");
            

            // Vérification fichier créé
            Assert.True(File.Exists(expectedFilePath), $"Le fichier de log n'a pas été créé : {expectedFilePath}");
			
			// Vérification du contenu du fichier
			string fileContent = File.ReadAllText(expectedFilePath);
			string expectedContent = testlog.Name; // Vérifie si "test" est dans le fichier
			Assert.Contains(expectedContent, fileContent);
            string expectedContent2 = testlog2.Name; 
            Assert.Contains(expectedContent2, fileContent);

        }

	}
}