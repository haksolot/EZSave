using System.Windows;
using EZSave.Client.ViewModels;
using EZSave.Core.Models;

namespace EZSave.Client.Views
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
