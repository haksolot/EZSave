using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZSave.TUI.ViewModels
{
    public class ViewModel1
    {
        public List<string> MainOptions { get; set; }
        public List<string> ConfigOptions { get; set; }

        public ViewModel1()
        {
            MainOptions = new List<string>
        {
            "Exécuter les jobs",
            "Entrer en mode configuration"
        };

            ConfigOptions = new List<string>
        {
            "Ajouter un job",
            "Modifier un job",
            "Supprimer un job",
            "Changer le chemin de destination des logs",
            "Changer le chemin de destination de la configuration",
            "Changer le chemin de destination du statut",
            "Retour au menu principal"
        };
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
    }
}
