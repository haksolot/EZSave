using EZSave.Core.Models;
using EZSave.Core.Services;

namespace EZSave.TUI.ViewModels
{
  public class ViewModel1
  {
    private readonly ResourcesService _resourcesService;
    //private readonly ManagerService _managerService;
    public List<string> MainOptions { get; set; }
    public List<string> ConfigOptions { get; set; }

    public List<string> LanguageOptions { get; set; }
    public string MainMenuTitle { get; set; }
    public string ChoiceTitle { get; set; }
    public string InvalidChoiceTitle { get; set; }
    public string ChooseChoiceTitle { get; set; }
    public string ConfModeTitle { get; set; }
    public string EnterJobName { get; set; }
    public string EnterSource { get; set; }
    public string EnterDestination { get; set; }
    public string EnterType { get; set; }
    public string EnterModifiedJob { get; set; }
    public string EnterDeletedJob { get; set; }
    public string ListJobsPossibleDelete { get; set; }
    public string ListJobsPossibleModify { get; set; }
    public string LogPathChanging { get; set; }
    public string LogTypeChanging { get; set; }
    public string ConfigFilePathChanging { get; set; }
    public string StatusFilePathChanging { get; set; }
    public string JobsExecutedSuccess { get; set; }
    public string JobsExecutedFail { get; set; }
    public string JobAdded { get; set; }
    public string JobNotAdded { get; set; }
    public string JobDeleted { get; set; }
    public string JobNotDeleted { get; set; }
    public string ListJobsPossibleExecute { get; set; }
    public string JobExecutedSuccess { get; set; }
    public string EnterJobExecute { get; set; }
    public string JobEdited { get; set; }
    public string JobNotEdited { get; set; }
    public string Message { get; set; }

    public ManagerModel managerModel { get; set; }
    public ConfigFileModel configFileModel { get; set; }

    public ViewModel1(ResourcesService resourcesService)
    {
      _resourcesService = resourcesService;
      //_managerService = new ManagerService ();
      MainOptions = new List<string>();
      ConfigOptions = new List<string>();
      LanguageOptions = new List<string>();
      LoadStrings();
    }

    public void Initialize()
    {
      configFileModel = new ConfigFileModel();
      managerModel = new ManagerModel();
      var configService = new ConfigService();
      var managerService = new ManagerService();
      configService.SetConfigDestination("conf.json", configFileModel);
      configService.LoadConfigFile(configFileModel);

      managerService.Read(managerModel, configFileModel);
      Message = "Fichier de configuration appliqué avec succés !";
    }
    public void LoadStrings()
    {
      MainOptions.Clear();
      MainOptions.Add(_resourcesService.GetString("MainOption1"));
      MainOptions.Add(_resourcesService.GetString("MainOption2"));
      MainOptions.Add(_resourcesService.GetString("MainOption3"));
      MainOptions.Add(_resourcesService.GetString("MainOption4"));
      MainMenuTitle = _resourcesService.GetString("MainMenuTitle");
      ChoiceTitle = _resourcesService.GetString("ChoiceTitle");
      InvalidChoiceTitle = _resourcesService.GetString("InvalidChoiceTitle");
      ChooseChoiceTitle = _resourcesService.GetString("ChooseChoiceTitle");
      ConfModeTitle = _resourcesService.GetString("ConfModeTitle");
      EnterJobName = _resourcesService.GetString("EnterJobName");
      EnterSource = _resourcesService.GetString("EnterSource");
      EnterDestination = _resourcesService.GetString("EnterDestination");
      EnterType = _resourcesService.GetString("EnterType");
      EnterModifiedJob = _resourcesService.GetString("EnterModifiedJob");
      EnterDeletedJob = _resourcesService.GetString("EnterDeletedJob");
      ListJobsPossibleDelete = _resourcesService.GetString("ListJobsPossibleDelete");
      ListJobsPossibleModify = _resourcesService.GetString("ListJobsPossibleModify");
      LogPathChanging = _resourcesService.GetString("LogPathChanging");
      LogTypeChanging = _resourcesService.GetString("LogTypeChanging");
      ConfigFilePathChanging = _resourcesService.GetString("ConfigFilePathChanging");
      StatusFilePathChanging = _resourcesService.GetString("StatusFilePathChanging");
      JobsExecutedSuccess = _resourcesService.GetString("JobsExecutedSuccess");
      JobsExecutedFail = _resourcesService.GetString("JobsExecutedFail");
      JobAdded = _resourcesService.GetString("JobAdded");
      JobNotAdded = _resourcesService.GetString("JobNotAdded");
      JobDeleted = _resourcesService.GetString("JobDeleted");
      JobNotDeleted = _resourcesService.GetString("JobNotDeleted");
      JobEdited = _resourcesService.GetString("JobEdited");
      JobNotEdited = _resourcesService.GetString("JobNotEdited");
      ConfigOptions.Clear();
      ConfigOptions.Add(_resourcesService.GetString("ConfigOption1"));
      ConfigOptions.Add(_resourcesService.GetString("ConfigOption2"));
      ConfigOptions.Add(_resourcesService.GetString("ConfigOption3"));
      ConfigOptions.Add(_resourcesService.GetString("ConfigOption4"));
      ConfigOptions.Add(_resourcesService.GetString("ConfigOption5"));
      ConfigOptions.Add(_resourcesService.GetString("ConfigOption6"));
      ConfigOptions.Add(_resourcesService.GetString("ConfigOption7"));
      ConfigOptions.Add(_resourcesService.GetString("ConfigOption8"));
      LanguageOptions.Clear();
      LanguageOptions.Add(_resourcesService.GetString("LanguageOption1"));
      LanguageOptions.Add(_resourcesService.GetString("LanguageOption2"));
      ListJobsPossibleExecute = _resourcesService.GetString("ListJobsPossibleExecute");
      JobExecutedSuccess = _resourcesService.GetString("JobExecutedSuccess");
      EnterJobExecute = _resourcesService.GetString("EnterJobExecute");
    }


    public bool ExecuteJobs()
    {
      var managerService = new ManagerService();
      var configModel = new ConfigFileModel();
      configModel.LogFileDestination = "Log";
      configModel.StatusFileDestination = "Status";
      bool isExecuted = managerService.Execute(managerModel, configModel);
      return isExecuted;
    }

    public bool ExecuteJob(string jobName)
    {
      var managerService = new ManagerService();
      var configModel = new ConfigFileModel();
      configModel.LogFileDestination = "Log";
      configModel.StatusFileDestination = "Status";
      var selected = new List<string>();
      selected.Add(jobName);
      bool isExecuted = managerService.ExecuteSelected(selected, managerModel, configModel);
      return isExecuted;
    }

    public string GetJobs()
    {
      var Jobs = managerModel.Jobs;
      Message = "";
      foreach (JobModel job in Jobs)
      {
        Message += "- " + job.Name + "\n";
      }
      return Message;
    }
    public bool AddJob(string name, string source, string destination, string type)
    {
      var job = new JobModel();
      job.Name = name;
      job.Source = source;
      job.Destination = destination;
      job.Type = type;

      var managerService = new ManagerService();
      var configService = new ConfigService();
      bool isAdded = managerService.Add(job, managerModel);
      configService.SaveJob(job, configFileModel);

      Message = "Job ajouté et sauvegardé dans config !";
      return isAdded;
    }

    public bool EditJob(string name, string source, string destination, string type)
    {
      var job = new JobModel();
      job.Name = name;
      job.Source = source;
      job.Destination = destination;
      job.Type = type;

      var managerService = new ManagerService();
      var configService = new ConfigService();

      managerService.RemoveJob(job, managerModel);
      managerService.Add(job, managerModel);
      bool isEDited = configService.SaveJob(job, configFileModel);

      Message = "Job ajouté et sauvegardé dans config !";
      return isEDited;
    }


    public bool DeleteJob(string name)
    {
      var job = new JobModel();
      job.Name = name;
      var managerService = new ManagerService();
      var configService = new ConfigService();

      bool isRemoved = managerService.RemoveJob(job, managerModel);
      configService.DeleteJob(job, configFileModel);
      return isRemoved;
    }

    public void ChangeLogPath(string dest)
    {
            configFileModel.LogFileDestination = dest;
      var configService = new ConfigService();
      configService.SetLogDestination(dest, configFileModel);
    }

    public void ChangeLogType(string type)
    {
            configFileModel.LogType = type;
      var configService = new ConfigService();
      configService.SetLogType(type, configFileModel);
    }

    public void ChangeConfigPath(string dest)
    {
            configFileModel.ConfFileDestination = dest;
      var configService = new ConfigService();
      configService.SetConfigDestination(dest, configFileModel);
    }

    public void ChangeStatusPath(string dest)
    {
            configFileModel.StatusFileDestination = dest;
      var configService = new ConfigService();
      configService.SetStatusDestination(dest, configFileModel);
    }

    public void ChangeLanguage(string languageCode)
    {
      _resourcesService.ChangeLanguage(languageCode);
      LoadStrings();
    }
  }


}
