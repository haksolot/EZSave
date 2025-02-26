using System.Windows;
using EZSave.Client.ViewModels;
using EZSave.Core.Models;
using EZSave.Core.Services;

namespace EZSave.Client.Views
{
    public partial class ConfigWindow : Window
    {
        public ConfigWindow(ManagerModel manager, ConfigFileModel config, SocketClientService socket)
        {
            InitializeComponent();
            DataContext = new ConfigViewModel(config, manager, socket);

            //DataContext = viewModel;
        }
    }
}
