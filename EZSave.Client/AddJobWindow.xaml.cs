using EZSave.Core.Models;
using EZSave.Client.ViewModels;
using System.Windows;
using EZSave.Core.Services;

namespace EZSave.Client
{
    public partial class AddJobWindow : Window
    {
        public AddJobWindow(ManagerModel manager, ConfigFileModel config, SocketClientService _socketClient)
        {
            InitializeComponent();
            DataContext = new AddJobViewModel(manager, config, _socketClient);
        }
    }
}
