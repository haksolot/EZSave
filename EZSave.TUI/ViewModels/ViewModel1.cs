using EZSave.Core.Models;
using EZSave.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    AddJob();
                    break;
                case 2:
                    ModifyJob();
                    break;
                case 3:
                    DeleteJob();
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
                    Console.WriteLine("Retour au menu principal.");
                    break;
                default:
                    Console.WriteLine("Choix invalide. Veuillez réessayer.");
                    break;
            }
        }

        private void ExecuteJobs()
        {
            Console.WriteLine("Exécution des jobs...");
            // Logique pour exécuter les jobs
        }

        private void EnterConfigMode()
        {
            Console.WriteLine("Entrée en mode configuration...");
            // Logique pour entrer en mode configuration
        }

        private void AddJob()
        {
            Console.WriteLine("Ajout d'un job...");
            //JobModel = _managerService.Add(JobModel, _managerService);
            // Logique pour ajouter un job
        }

        private void ModifyJob()
        {
            Console.WriteLine("Modification d'un job...");
            // Logique pour modifier un job
        }

        private void DeleteJob()
        {
            Console.WriteLine("Suppression d'un job...");
            // Logique pour supprimer un job
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
