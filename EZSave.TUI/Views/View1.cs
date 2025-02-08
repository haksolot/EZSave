using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZSave.TUI.ViewModels;

namespace EZSave.TUI.Views
{
    public class View1
    {
        private readonly ViewModel1 _viewModel;

        public View1(ViewModel1 viewModel)
        {
            _viewModel = viewModel;
        }

        public void Display()
        {
            bool inConfigMode = false;

            while (true)
            {
                Console.Clear();
                if (inConfigMode)
                {
                    Console.WriteLine(" Mode Configuration :");
                    for (int i = 0; i < _viewModel.ConfigOptions.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {_viewModel.ConfigOptions[i]}");
                    }

                    Console.Write(" Votre choix : ");
                    if (int.TryParse(Console.ReadLine(), out int configChoice))
                    {
                        _viewModel.ExecuteConfigOption(configChoice);
                        if (configChoice == 7)
                        {
                            inConfigMode = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Choix invalide. Veuillez réessayer.");
                    }
                }
                else
                {
                    Console.Write(@"


 _____ ______ _____                 
|  ___|___  //  ___|                
| |__    / / \ `--.  __ ___   _____ 
|  __|  / /   `--. \/ _` \ \ / / _ \
| |___./ /___/\__/ / (_| |\ V /  __/
\____/\_____/\____/ \__,_| \_/ \___|
                                    
                                    
 
");
                    Console.WriteLine(" Menu Principal :");
                    for (int i = 0; i < _viewModel.MainOptions.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {_viewModel.MainOptions[i]}");
                    }

                    Console.Write(" Votre choix : ");
                    if (int.TryParse(Console.ReadLine(), out int mainChoice))
                    {
                        _viewModel.ExecuteMainOption(mainChoice);
                        if (mainChoice == 2)
                        {
                            inConfigMode = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Choix invalide. Veuillez réessayer.");
                    }
                }

                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
        }
    }
}
