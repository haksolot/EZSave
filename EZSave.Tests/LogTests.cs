using EZSave.Core.Models;
using EZSave.Core.Services;

namespace EZSave.Tests
{
    public class LogTest
    {
        [Fact]
        public void WriteLogTestJSON()
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
            var testconfigfileJSON = new ConfigFileModel
            {
                ConfFileDestination = "config",
                LogFileDestination = "logs",
                LogType = "json"
            };

            string logDirectory = testconfigfileJSON.LogFileDestination;
            var logService = new LogService();

            string expectedContent = testlog.Name; // Vérifie si "test" est dans le fichier
            string expectedContent2 = testlog2.Name;

            logService.Write(testlog, testconfigfileJSON);
            logService.Write(testlog2, testconfigfileJSON);

            string expectedFilePathJSON = Path.Combine(logDirectory, DateTime.Now.ToString("yyyyMMdd") + "_log.json");

            // Vérification fichier créé
            Assert.True(File.Exists(expectedFilePathJSON), $"Le fichier de log n'a pas été créé : {expectedFilePathJSON}");

            // Vérification du contenu du fichier
            string fileContentJSON = File.ReadAllText(expectedFilePathJSON);
        }

        [Fact]
        public void WriteLogTestXML()
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
            var testconfigfileXML = new ConfigFileModel
            {
                ConfFileDestination = "config",
                LogFileDestination = "logs",
                LogType = "xml"
            };

            string logDirectory = testconfigfileXML.LogFileDestination;
            var logService = new LogService();

            string expectedContent = testlog.Name; // Vérifie si "test" est dans le fichier
            string expectedContent2 = testlog2.Name;

            logService.Write(testlog, testconfigfileXML);
            logService.Write(testlog2, testconfigfileXML);

            string expectedFilePathXML = Path.Combine(logDirectory, DateTime.Now.ToString("yyyyMMdd") + "_log.xml");

            //Vérification fichier créé
            Assert.True(File.Exists(expectedFilePathXML), $"Le fichier de log n'a pas été créé : {expectedFilePathXML}");

            //Vérification du contenu du fichier
            string fileContentXML = File.ReadAllText(expectedFilePathXML);

            Assert.Contains(expectedContent, fileContentXML);
            Assert.Contains(expectedContent2, fileContentXML);
        }
    }
}