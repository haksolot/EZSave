using EZSave.Core.Models;
using EZSave.GUI.ViewModels;
using System.Windows;

namespace EZSave.GUI
{
    /// <summary>
    /// Logique d'interaction pour AddJobWindow.xaml
    /// </summary>
    public partial class AddJobWindow : Window
    {
        public AddJobWindow(ManagerModel manager)
        {
            InitializeComponent();
            DataContext = new AddJobViewModel(manager);

        }
    }
}