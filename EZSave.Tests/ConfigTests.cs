//using EZSave.Core.Models;
//using EZSave.Core.Services;
//using System.Text.Json;
namespace EZSave.Tests
{
    public class ConfigTests
    {
        [Fact]
        public void LoadConfigFileTest()
        {
            var conf = new ConfigFileModel { ConfFileDestination = "test_config.json" };
            var expectedJob = new JobModel { Name = "job1", Source = "/src", Destination = "/dest", Type = "full" };

            var jsonContent = JsonSerializer.Serialize(new ConfigFileModel
            {
                Jobs = new Dictionary<string, JobModel> { { "job1", expectedJob } }
            }, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(conf.ConfFileDestination, jsonContent);

            new ConfigService().LoadConfigFile(conf);

            Assert.NotNull(conf.Jobs); 
            Assert.True(conf.Jobs.ContainsKey("job1"));
            Assert.Equal(expectedJob.Source, conf.Jobs["job1"].Source);
            Assert.Equal(expectedJob.Destination, conf.Jobs["job1"].Destination);
            Assert.Equal(expectedJob.Type, conf.Jobs["job1"].Type);
        }

        [Fact]
        public void SaveJob_Should_Add_Job_To_Config()
        {
            var service = new ConfigService();
            var tempFilePath = Path.Combine(Path.GetTempPath(), "test_config.json");
            var model = new ConfigFileModel();

            service.SetConfigDestination(tempFilePath, model);

            var job = new JobModel { Name = "TestJob", Source = "C://test", Destination = "C://test2", Type = "differential" };

            service.SaveJob(job, model);
            string json = File.ReadAllText(tempFilePath);
            var loadedConfig = JsonSerializer.Deserialize<ConfigFileModel>(json);

            Assert.NotNull(loadedConfig);
            Assert.NotNull(loadedConfig.Jobs);
            Assert.True(loadedConfig.Jobs.ContainsKey("TestJob"));
            Assert.Equal("C://test", loadedConfig.Jobs["TestJob"].Source);
        }

        [Fact]
        public void DeleteJob_Should_Remove_Job_From_Config()
        {
            var tempFilePath = Path.Combine(Path.GetTempPath(), "test_config.json");
            var service = new ConfigService();
            var model = new ConfigFileModel();
            var job = new JobModel { Name = "TestJob", Source = "C://test", Destination = "C://test2", Type = "differential" };

            service.SetConfigDestination(tempFilePath, model);
            service.SaveJob(job, model);

            service.DeleteJob(new JobModel { Name = "TestJob" }, model);
            string json = File.ReadAllText(tempFilePath);
            var loadedConfig = JsonSerializer.Deserialize<ConfigFileModel>(json);

            Assert.NotNull(loadedConfig);
            Assert.NotNull(loadedConfig.Jobs);
            Assert.False(loadedConfig.Jobs.ContainsKey("TestJob"));
        }
    }
}

