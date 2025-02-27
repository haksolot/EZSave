using EZSave.Core.Models;
using EZSave.Core.Services;
using EZSave.GUI.ViewModels;
using System.Windows;

namespace EZSave.GUI
{
    public partial class AddJobWindow : Window
    {
        private readonly AddJobViewModel _viewModel;

        public AddJobWindow(ManagerModel managerModel, ConfigFileModel configFileModel, ManagerService managerService)
        {
            InitializeComponent();
            _viewModel = new AddJobViewModel(managerModel, configFileModel, managerService);
            DataContext = _viewModel;
        }
    }
}
