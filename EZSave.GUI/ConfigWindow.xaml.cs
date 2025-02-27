using EZSave.Core.Models;
using EZSave.Core.Services;
using EZSave.GUI.ViewModels;
using System.Windows;

namespace EZSave.GUI.Views
{
    public partial class ConfigWindow : Window
    {
        private readonly ConfigViewModel _viewModel;

        public ConfigWindow(ManagerModel managerModel, ConfigFileModel configFileModel, ManagerService managerService)
        {
            InitializeComponent();
            _viewModel = new ConfigViewModel(configFileModel, managerModel, managerService);
            DataContext = _viewModel;
        }
    }
}
