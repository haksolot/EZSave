using EZSave.Core.Models;
using System;
using System.IO;
using System.Windows;

namespace EZSave.GUI
{
    public partial class AddJobWindow : Window
    {
        private readonly ManagerModel _managerModel;
        private readonly JobModel _job;

        public AddJobWindow(ManagerModel managerModel)
        {
            InitializeComponent();
            _managerModel = managerModel;
            _job = new JobModel();
            DataContext = _job;
        }

        private void ValidateJob(object sender, RoutedEventArgs e)
        {
            try
            {
                // ✅ Vérifie que tous les champs sont remplis
                if (string.IsNullOrWhiteSpace(_job.Name) ||
                    string.IsNullOrWhiteSpace(_job.Source) ||
                    string.IsNullOrWhiteSpace(_job.Destination) ||
                    string.IsNullOrWhiteSpace(_job.Type))
                {
                    MessageBox.Show("Tous les champs doivent être remplis.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // ✅ Vérifie que le type est correct
                if (_job.Type != "full" && _job.Type != "diff")
                {
                    MessageBox.Show("Le type de sauvegarde doit être 'full' ou 'diff'.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // ✅ Vérifie que le dossier source existe
                if (!Directory.Exists(_job.Source))
                {
                    MessageBox.Show("Le dossier source n'existe pas.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // ✅ Vérifie que le dossier destination existe (ou le crée)
                if (!Directory.Exists(_job.Destination))
                {
                    Directory.CreateDirectory(_job.Destination);
                }

                // ✅ Ajoute le job à la liste
                _managerModel.Jobs.Add(_job);

                // ✅ Ferme la fenêtre après validation
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
