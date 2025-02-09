﻿using EZSave.Core.Models;
using EZSave.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
    public string EnterSource {  get; set; }
    public string EnterDestination { get; set; }
    public string EnterType { get; set; }
    public string EnterModifiedJob {  get; set; }
    public string EnterDeletedJob { get; set; }
    public string ListJobsPossibleDelete { get; set; }
    public string ListJobsPossibleModify {  get; set; }
    public string Message { get; set; }

    public ManagerModel managerModel { get; set; }
    public ConfigFileModel configFileModel { get; set; }

    //public JobModel JobModel { get; set; } = new JobModel();

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
      ConfigOptions.Clear();
      ConfigOptions.Add(_resourcesService.GetString("ConfigOption1"));
      ConfigOptions.Add(_resourcesService.GetString("ConfigOption2"));
      ConfigOptions.Add(_resourcesService.GetString("ConfigOption3"));
      ConfigOptions.Add(_resourcesService.GetString("ConfigOption4"));
      ConfigOptions.Add(_resourcesService.GetString("ConfigOption5"));
      ConfigOptions.Add(_resourcesService.GetString("ConfigOption6"));
      ConfigOptions.Add(_resourcesService.GetString("ConfigOption7"));
      LanguageOptions.Clear();
      LanguageOptions.Add(_resourcesService.GetString("LanguageOption1"));
      LanguageOptions.Add(_resourcesService.GetString("LanguageOption2"));
    }
    public void ExecuteMainOption(int choice)
    {
      switch (choice)
      {
        case 1:
          ExecuteJobs();
          break;
        case 2:
          EnterConfigMode();
          break;
        case 3:
          ChangeLanguage("fr");
          break;
        default:
          Console.WriteLine("Choix invalide. Veuillez réessayer.");
          break;
      }
    }

    public void ExecuteConfigOption(int choice)
    {
      switch (choice)
            {
        case 1:
          //AddJob(string name, string source, string destination, string type);
          break;
        case 2:
          //ModifyJob();
          break;
        case 3:
          //DeleteJob();
          break;
        case 4:
          ChangeLogPath();
          break;
        case 5:
          ChangeConfigPath();
          break;
        case 6:
          ChangeStatusPath();
          break;
        case 7:
          //Console.WriteLine("Retour au menu principal.");
          break;
        default:
          //Console.WriteLine("Choix invalide. Veuillez réessayer.");
          break;
      }
    }

    public void ExecuteJobs()
    {
            var managerService = new ManagerService();
            var configModel = new ConfigFileModel();
            bool isExecuted = managerService.Execute(managerModel, configModel);
            //Console.WriteLine("Exécution des jobs...");
            if (isExecuted)
            {
                Message = "Tous les jobs ont été exécutés avec succès!";
                Console.WriteLine("Tous les jobs ont été exécutés avec succès!");
            }
            else
            {
                Message = "Aucun job à exécuter, la liste est vide.";
                Console.WriteLine("Aucun job à exécuter, la liste est vide.");
            }
        }

    private void EnterConfigMode()
    {
      //Console.WriteLine("Entrée en mode configuration...");
      // Logique pour entrer en mode configuration
    }



    public string GetJobs()
        {
            var Jobs = managerModel.Jobs;
            Message = "";
            foreach(JobModel job in Jobs)
            {
                Message += "- " + job.Name + "\n"; 
            }
            return Message;
        }
    public void AddJob(string name, string source, string destination, string type)
    {
            var job = new JobModel();
            job.Name = name;
            job.Source = source;
            job.Destination = destination;
            job.Type = type;

            var managerService = new ManagerService();
            var configService = new ConfigService();
            managerService.Add(job, managerModel);
            configService.SaveJob(job, configFileModel);

            Message = "Job ajouté et sauvegardé dans config !";
    }

    public void EditJob(string name, string source, string destination, string type)
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
        configService.SaveJob(job, configFileModel);

        Message = "Job ajouté et sauvegardé dans config !";
    }


    public void DeleteJob(string name)
    {
            var job = new JobModel();
            job.Name = name;
            var managerService = new ManagerService();
            var configService = new ConfigService();

            managerService.RemoveJob(job, managerModel);
            configService.DeleteJob(job, configFileModel);
    }

    private void ChangeLogPath()
    {
      Console.WriteLine("Changement du chemin de destination des logs...");
      // Logique pour changer le chemin des logs
    }

    private void ChangeConfigPath()
    {
      Console.WriteLine("Changement du chemin de destination de la configuration...");
      // Logique pour changer le chemin de la configuration
    }

    private void ChangeStatusPath()
    {
      Console.WriteLine("Changement du chemin de destination du statut...");
      // Logique pour changer le chemin du statut
    }

    public void ChangeLanguage(string languageCode)
    {
      _resourcesService.ChangeLanguage(languageCode);
      LoadStrings();
    }
  }


}
