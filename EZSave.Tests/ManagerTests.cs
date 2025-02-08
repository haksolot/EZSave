using EZSave.Core.Models;
using EZSave.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZSave.Tests
{
    public class ManagerTests
    {
        private readonly string tempFilePath = Path.Combine(Path.GetTempPath(), "test_config.json");
        private readonly ConfigService configService = new ConfigService();

        [Fact]
        public void SaveJob_ShouldPersistJobAndLoadConfigCorrectly()
        {
            // Arrange
            string tempFilePath = Path.GetTempFileName();
            var conf = new ConfigFileModel { ConfFileDestination = tempFilePath };
            var job = new JobModel { Name = "TestJob", Source = "C://test", Destination = "C://test2", Type = "differential" };

            var configService = new ConfigService();
            var config = new ConfigFileModel();

            try
            {
                // Act
                configService.SaveJob(job, conf);
                configService.LoadConfigFile(conf, config);

                // Assert - Vérifie que le job a bien été sauvegardé
                Assert.True(config.Jobs.ContainsKey("TestJob"));

                var manager = new ManagerService();
                var managerModel = new ManagerModel();
                manager.Read(managerModel, config);

                // Vérifie que le manager a bien récupéré les jobs du fichier de configuration
                Assert.NotEmpty(managerModel.Jobs);
            }
            finally
            {
                // Cleanup
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }

    }
}
