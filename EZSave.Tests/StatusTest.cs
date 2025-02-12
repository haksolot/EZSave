using System.Text.Json;
using EZSave.Core.Models;
using EZSave.Core.Services;

namespace EZSave.Tests
{
    public class StatusTest
    {
        [Fact]
        public void SaveStatusTest()
        {
            var teststatus = new StatusModel
            {
                Name = "Save2",
                SourceFilePath = "",
                TargetFilePath = "",
                State = "Activate",
                TotalFilesSize = 1400050,
                TotalFilesToCopy = 1450000,
                Progression = 0,
                FilesLeftToCopy = 1450000,
                FilesSizeLeftToCopy = 1400050
            };
            var testconfigfile = new ConfigFileModel
            {
                ConfFileDestination = "test",
                LogFileDestination = "test",
                StatusFileDestination = "statustest"
            };
            var status = new StatusService();
            status.SaveStatus(teststatus, testconfigfile);

            string statusDirectory = Path.Combine(testconfigfile.StatusFileDestination, "_status.json");

            // Vérification fichier créé
            Assert.True(File.Exists(statusDirectory), $"Le fichier de log n'a pas été créé : {statusDirectory}");

            // Vérification du contenu du fichier
            string fileContent = File.ReadAllText(statusDirectory);
            var liststatus = JsonSerializer.Deserialize<Dictionary<string, StatusModel>>(fileContent);

            // Vérifier que le statut a bien été ajouté
            Assert.NotNull(liststatus);
            Assert.True(liststatus.ContainsKey(teststatus.Name), "Le statut n'a pas été trouvé dans le fichier JSON");

            // Récupérer le statut enregistré
            var savedStatus = liststatus[teststatus.Name];

            // Vérifier que les valeurs sont bien enregistrées
            Assert.Equal(teststatus.Name, savedStatus.Name);
            Assert.Equal(teststatus.State, savedStatus.State);
            Assert.Equal(teststatus.TotalFilesSize, savedStatus.TotalFilesSize);
            Assert.Equal(teststatus.TotalFilesToCopy, savedStatus.TotalFilesToCopy);

            teststatus.FilesLeftToCopy = 50;
            teststatus.FilesSizeLeftToCopy = 250;

            status.SaveStatus(teststatus, testconfigfile);
            Assert.Equal(teststatus.FilesLeftToCopy, 50);
            Assert.Equal(teststatus.FilesSizeLeftToCopy, 250);

        }
    }
}
