using EZSave.Core.Models;
using EZSave.GUI.ViewModels;
using System.Windows;

namespace EZSave.GUI
{
    public partial class AddJobWindow : Window
    {
        public AddJobWindow(ManagerModel manager, ConfigFileModel config)
        {
            InitializeComponent();
            DataContext = new AddJobViewModel(manager, config);

        }
    }
}