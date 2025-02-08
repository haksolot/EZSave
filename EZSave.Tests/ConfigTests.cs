////using EZSave.Core.Models;
////using EZSave.Core.Services;
////using System.Text.Json;

////namespace EZSave.Tests
////{
////    public class ConfigTests
////    {
////        [Fact]
////        public void LoadConfigFile()
////        {
////            var conf = new ConfigModel { ConfFileDestination = "test_config.json" };
////            File.WriteAllText(conf.ConfFileDestination, "{\"Jobs\":{\"job1\":\"Task1\"}}");
////            var config = new ConfigFileModel();
////            new ConfigService().LoadConfigFile(conf, config);
////            Assert.True(config.Jobs.ContainsKey("job1"));
////        }

//namespace EZSave.Tests
//{
//    public class ConfigTests
//    {
//        [Fact]
//        public void LoadConfigFile()
//        {
//            var conf = new ConfigFileModel { ConfFileDestination = "test_config.json" };
//            File.WriteAllText(conf.ConfFileDestination, "{\"Jobs\":{\"job1\":\"Task1\"}}");
//            var config = new ConfigFileModel();
//            new ConfigService().LoadConfigFile(conf);
//            Assert.True(config.Jobs.ContainsKey("job1"));
//        }

////        private readonly string tempFilePath = Path.Combine(Path.GetTempPath(), "test_config.json");
////        private readonly ConfigService configService = new ConfigService();

////        [Fact]
////        public void SaveJob_Should_Add_Job_To_Config()
////        {
////            // Arrange
////            var conf = new ConfigModel { ConfFileDestination = tempFilePath };
////            var config = new ConfigFileModel { Jobs = new Dictionary<string, JobModel>() };
////            var job = new JobModel { Name = "TestJob", Source = "C://test", Destination = "C://test2", Type = "differential"};

////            // Act
////            configService.SaveJob(job, conf);
////            string json = File.ReadAllText(tempFilePath);
////            var loadedConfig = JsonSerializer.Deserialize<ConfigFileModel>(json);
//        [Fact]
//        public void SaveJob_Should_Add_Job_To_Config()
//        {
//            // Arrange
//            //var conf = new ConfigFileModel { ConfFileDestination = tempFilePath };
//            //var config = new ConfigFileModel { Jobs = new Dictionary<string, JobModel>() };
//            var config = new ConfigFileModel { ConfFileDestination = tempFilePath, Jobs = new Dictionary<string, JobModel>() };
//            var job = new JobModel { Name = "TestJob", Source = "C://test", Destination = "C://test2", Type = "differential" };

//            // Act
//            configService.SaveJob(job, config);
//            string json = File.ReadAllText(tempFilePath);
//            var loadedConfig = JsonSerializer.Deserialize<ConfigFileModel>(json);
//>>>>>>> c17f8291484e173b36bc85c0e4e92908a36f71a0

////            // Assert
////            Assert.NotNull(loadedConfig);
////            Assert.True(loadedConfig.Jobs.ContainsKey("TestJob"));
////            Assert.Equal("C://test", loadedConfig.Jobs["TestJob"].Source);
////        }

////        [Fact]
////        public void DeleteJob_Should_Remove_Job_From_Config()
////        {
////            // Arrange
////            var conf = new ConfigModel { ConfFileDestination = tempFilePath };
////            var config = new ConfigFileModel { Jobs = new Dictionary<string, JobModel> { { "TestJob", new JobModel { Name = "TestJob", Source = "C://test", Destination = "C://test2", Type = "differential" } } } };
////            File.WriteAllText(tempFilePath, JsonSerializer.Serialize(config));

////            // Act
////            configService.DeleteJob(new JobModel { Name = "TestJob" }, conf);
////            string json = File.ReadAllText(tempFilePath);
////            var loadedConfig = JsonSerializer.Deserialize<ConfigFileModel>(json);
//        [Fact]
//        public void DeleteJob_Should_Remove_Job_From_Config()
//        {
//            // Arrange
//            //var conf = new ConfigFileModel { ConfFileDestination = tempFilePath };
//            //var config = new ConfigFileModel { Jobs = new Dictionary<string, JobModel> { { "TestJob", new JobModel { Name = "TestJob", Source = "C://test", Destination = "C://test2", Type = "differential" } } } };
//            var config = new ConfigFileModel { ConfFileDestination = tempFilePath, Jobs = new Dictionary<string, JobModel> { { "TestJob", new JobModel { Name = "TestJob", Source = "C://test", Destination = "C://test2", Type = "differential" } } } };

//            File.WriteAllText(tempFilePath, JsonSerializer.Serialize(config));

//            // Act
//            configService.DeleteJob(new JobModel { Name = "TestJob" }, config);
//            string json = File.ReadAllText(tempFilePath);
//            var loadedConfig = JsonSerializer.Deserialize<ConfigFileModel>(json);

////            // Assert
////            Assert.NotNull(loadedConfig);
////            Assert.False(loadedConfig.Jobs.ContainsKey("TestJob"));
////        }
////    }
////}