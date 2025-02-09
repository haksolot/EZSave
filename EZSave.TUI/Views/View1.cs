using System;
using System.Collections.Generic;
using System.Xml.Linq;
using EZSave.Core.Services;
using EZSave.TUI.ViewModels;

namespace EZSave.TUI.Views
{
    public class View1
    {
        private readonly ViewModel1 _viewModel;
        private readonly ResourcesService _resourcesService;

        public View1(ViewModel1 viewModel, ResourcesService resourcesService)
        {
            _viewModel = viewModel;
            _resourcesService = resourcesService;
            viewModel.Initialize();
        }

        public void ShowJobs()
        {
            Console.Write(_viewModel.GetJobs());
        }
        public void Display()
        {
            bool inConfigMode = false;

            while (true)
            {
                Console.Clear();
                if (inConfigMode)
                {
                    Console.WriteLine(_viewModel.ConfModeTitle);
                    for (int i = 0; i < _viewModel.ConfigOptions.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {_viewModel.ConfigOptions[i]}");
                    }

                    Console.Write(_viewModel.ChoiceTitle);
                    if (int.TryParse(Console.ReadLine(), out int configChoice))
                    {
                        if (configChoice == 1)
                        {
                            // Collecter les détails du job
                            Console.Write("Entrez le nom du job : ");
                            string name = Console.ReadLine();

                            Console.Write("Entrez la source du job : ");
                            string source = Console.ReadLine();

                            Console.Write("Entrez la destination du job : ");
                            string destination = Console.ReadLine();

                            Console.Write("Entrez le type du job : ");
                            string type = Console.ReadLine();

                            // Appeler la méthode AddJob du ViewModel
                            _viewModel.AddJob(name, source, destination, type);
                        }
                        if (configChoice == 2)
                        {
                            Console.WriteLine("Jobs pouvants être modifiés :");
                            ShowJobs();
                            Console.Write("Entrez le job à modifier : ");
                            string jobName = Console.ReadLine();

                            Console.Write("Source : ");
                            string source = Console.ReadLine();

                            Console.Write("Destination : ");
                            string destination = Console.ReadLine();

                            Console.Write("Type de backup : ");
                            string type = Console.ReadLine();

                            _viewModel.EditJob(jobName, source, destination, type);
                        }
                        if (configChoice == 3)
                        {
                            Console.WriteLine("Jobs pouvants être supprimés :");
                            ShowJobs();

                            Console.Write("Entrez le job à supprimer : ");
                            string jobName = Console.ReadLine();

                            _viewModel.DeleteJob(jobName);
                        }
                        else
                        {
                            _viewModel.ExecuteConfigOption(configChoice);
                            if (configChoice == 7)
                            {
                                inConfigMode = false;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine(_viewModel.InvalidChoiceTitle);
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
                    Console.WriteLine(_viewModel.MainMenuTitle);
                    for (int i = 0; i < _viewModel.MainOptions.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {_viewModel.MainOptions[i]}");
                    }

                    Console.Write(_viewModel.ChoiceTitle);
                    if (int.TryParse(Console.ReadLine(), out int mainChoice))
                    {
                        if (mainChoice == 1)
                        {
                            _viewModel.ExecuteMainOption(mainChoice);
                        }
                        else if (mainChoice == 2)
                        {
                            _viewModel.ExecuteMainOption(mainChoice);
                            inConfigMode = true;
                        }
                        else if (mainChoice == 3)
                        {
                            for (int i = 0; i < _viewModel.LanguageOptions.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {_viewModel.LanguageOptions[i]}");
                            }
                            Console.Write(_viewModel.ChoiceTitle);
                            if (int.TryParse(Console.ReadLine(), out int languageChoice))
                            {
                                if (languageChoice == 1)
                                {
                                    _viewModel.ChangeLanguage("fr");
                                }
                                else if (languageChoice == 2)
                                {
                                    _viewModel.ChangeLanguage("en");
                                }
                            }
                            else
                            {
                                Console.WriteLine(_viewModel.InvalidChoiceTitle);
                            }
                        }
                        else
                        {
                            Console.WriteLine(_viewModel.InvalidChoiceTitle);
                        }
                    }
                    else
                    {
                        Console.WriteLine(_viewModel.InvalidChoiceTitle);
                    }
                }

                Console.WriteLine(_viewModel.ChooseChoiceTitle);
                Console.ReadKey();
            }
        }
    }
}
