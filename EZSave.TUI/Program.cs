using EZSave.Core.Services;
using EZSave.TUI.ViewModels;
using EZSave.TUI.Views;

namespace EZSave.TUI
{
    public class Program
    {
        static void Main(string[] args)
        {
            ResourcesService resourcesService = new ResourcesService();
            ViewModel1 viewModel = new ViewModel1(resourcesService);
            View1 view = new View1(viewModel, resourcesService);
            view.Display();
        }
    }
}
