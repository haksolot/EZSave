using System.Windows;
using EZSave.GUI.ViewModels;
using EZSave.Core.Models;

namespace EZSave.GUI.Views
{
    public partial class ConfigWindow : Window
    {
        public ConfigWindow(ManagerModel manager, ConfigFileModel config)
        {
            InitializeComponent();
            DataContext = new ConfigViewModel(config, manager);

            //DataContext = viewModel;
        }
    }
}
