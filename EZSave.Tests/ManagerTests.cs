using EZSave.Core.Models;
using EZSave.Core.Services;


namespace EZSave.Tests
{
    public class ManagerTests
    {
        private readonly string tempFilePath = Path.Combine(Path.GetTempPath(), "test_config.json");
        private readonly ConfigService configService = new ConfigService();


        [Fact]
        public void Adding()
        {
            var service = new ConfigService();
            var tempFilePath = Path.Combine(Path.GetTempPath(), "test_config.json");
            var model = new ConfigFileModel();

            service.SetConfigDestination(tempFilePath, model);

            var job = new JobModel { Name = "TestJob", Source = "C://test", Destination = "C://test2", Type = "differential" };

            service.SaveJob(job, model);
            //string json = File.ReadAllText(tempFilePath);
            service.LoadConfigFile(model);
            //var loadedConfig = JsonSerializer.Deserialize<ConfigFileModel>(json);


            var manager = new ManagerService();
                
            var managerModel = new ManagerModel();
                
            manager.Read(managerModel, model);
            
            Assert.NotEmpty(managerModel.Jobs);
            Assert.Contains(managerModel.Jobs, job2 => job2.Name == "TestJob");
        }

        [Fact]
        public void Removing()
        {
            var service = new ConfigService();
            var tempFilePath = Path.Combine(Path.GetTempPath(), "test_config.json");
            var model = new ConfigFileModel();

            service.SetConfigDestination(tempFilePath, model);

            var job = new JobModel { Name = "TestJob", Source = "C://test", Destination = "C://test2", Type = "differential" };

            service.SaveJob(job, model);
            service.LoadConfigFile(model);


            var manager = new ManagerService();

            var managerModel = new ManagerModel();

            manager.Read(managerModel, model);
           

            var result = manager.RemoveJob(job, managerModel); 

            Assert.True(result); 
            Assert.Empty(managerModel.Jobs); 
        }

        [Fact]
        public void Executing()
        {
            var service = new ConfigService();
            var tempFilePath = Path.Combine(Path.GetTempPath(), "test_config.json");
            var model = new ConfigFileModel();

            service.SetConfigDestination(tempFilePath, model);

            var job = new JobModel { Name = "TestJob", Source = "C://test", Destination = "C://test2", Type = "differential" };

            service.SaveJob(job, model);
            service.LoadConfigFile(model);


            var manager = new ManagerService();

            var managerModel = new ManagerModel();
            var confModel = new ConfigFileModel();
            manager.Read(managerModel, model);
            var result = manager.Execute(managerModel, confModel);

            //var result = manager.RemoveJob(job, managerModel);

            Assert.True(result);
            //Assert.Empty(managerModel.Jobs);
        }

        [Fact]
        public void VerifyThatReturnFalseIfJobSelectedNotInJobs()
        {
            var jobService = new JobService();
            var managerService = new ManagerService();
            var manager = new ManagerModel
            {
                Jobs = new List<JobModel>
                {
                    new JobModel { Name = "TestJob" }
                }
            };
            var config = new ConfigFileModel();

            var listeSelected = new List<string> { "TestJob", "Job2" };

            var result = managerService.ExecuteSelected(listeSelected, manager, config);

            Assert.False(result);
        }

        [Fact]
        public void VerifyListeSelectedIsEmpty()
        {
            var jobService = new JobService();
            var manager = new ManagerModel { Jobs = new List<JobModel>() };
            var config = new ConfigFileModel();
            var managerService = new ManagerService();
            var result = managerService.ExecuteSelected(new List<string>(), manager, config);

            Assert.False(result);
        }

        [Fact]
        public void VerifyAllJobsAreExecuted()
        {
            var jobService = new JobService();
            var managerService = new ManagerService();
            var manager = new ManagerModel
            {
                Jobs = new List<JobModel>
                {
                    new JobModel { Name = "Job1" },
                    new JobModel { Name = "Job2" }
                }
            };
            var config = new ConfigFileModel();
            var listeSelected = new List<string> { "Job1", "Job2" };

            var result = managerService.ExecuteSelected(listeSelected, manager, config);

            Assert.True(result);
        }
    }
}

