using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZSave.Core.Models;
using EZSave.Core.Services;
using System.Text.Json;

namespace EZSave.Tests
{
    public class StatusTest
    {
        [Fact]
        public void SaveStatusTest()
        {
            var teststatus = new StatusModel
            {
                Name = "Save",
                State = "",
                Progress = 0,
                TotalFilesSize =  1400050,
                TotalFilesToCopy = 1450000,
            };
            var testconfigfile = new ConfigFileModel
            {
                ConfFileDestination = "test",
                LogFileDestination="test",
                StatusFileDestination="statustest"
            };
            var status = new StatusService();
            status.SaveStatus(teststatus, testconfigfile);

            string statusDirectory = Path.Combine(testconfigfile.StatusFileDestination,"_status.json");

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
            Assert.Equal(teststatus.Progress, savedStatus.Progress);
            Assert.Equal(teststatus.TotalFilesSize, savedStatus.TotalFilesSize);
            Assert.Equal(teststatus.TotalFilesToCopy, savedStatus.TotalFilesToCopy);
            
            teststatus.Progress = 50;

            status.SaveStatus(teststatus, testconfigfile);
            Assert.Equal(teststatus.Progress, 50);

        }
    }
}
