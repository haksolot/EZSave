using System.Windows;
using EZSave.GUI.ViewModels;
using EZSave.Core.Models;

namespace EZSave.GUI.Views
{
    public partial class ConfigWindow : Window
    {
        public ConfigWindow(ConfigFileModel configFileModel)
        {
            InitializeComponent();
            DataContext = new ConfigViewModel(configFileModel);
        }
    }
}
