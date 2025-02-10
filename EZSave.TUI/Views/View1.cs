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
              Console.Write(_viewModel.EnterJobName);
              string name = Console.ReadLine();

              Console.Write(_viewModel.EnterSource);
              string source = Console.ReadLine();

              Console.Write(_viewModel.EnterDestination);
              string destination = Console.ReadLine();

              Console.Write(_viewModel.EnterType);
              string type = Console.ReadLine();

              bool resultAdd = _viewModel.AddJob(name, source, destination, type);
              if (resultAdd)
                 {
                    Console.WriteLine(_viewModel.JobAdded);
                 }
              else
                 {
                    Console.WriteLine(_viewModel.JobNotAdded);
                 }
            }
            if (configChoice == 2)
            {
              Console.WriteLine(_viewModel.ListJobsPossibleModify);
              ShowJobs();
              Console.Write(_viewModel.EnterModifiedJob);
              string jobName = Console.ReadLine();

              Console.Write(_viewModel.EnterSource);
              string source = Console.ReadLine();

              Console.Write(_viewModel.EnterDestination);
              string destination = Console.ReadLine();

              Console.Write(_viewModel.EnterType);
              string type = Console.ReadLine();

              bool resultEdit = _viewModel.EditJob(jobName, source, destination, type);
              if (resultEdit)
              {
                Console.WriteLine(_viewModel.JobEdited);
              }
              else
              {
                Console.WriteLine(_viewModel.JobNotEdited);
              }
            }
            if (configChoice == 3)
            {
              Console.WriteLine(_viewModel.ListJobsPossibleDelete);
              ShowJobs();

              Console.Write(_viewModel.EnterDeletedJob);
              string jobName = Console.ReadLine();

              bool resultDelete = _viewModel.DeleteJob(jobName);
              if (resultDelete)
              {
                  Console.WriteLine(_viewModel.JobDeleted);
              }
              else
              {
                  Console.WriteLine(_viewModel.JobNotDeleted);
              }
                        }
            if (configChoice == 4)
            {
              Console.WriteLine(_viewModel.LogPathChanging);
              string dest = Console.ReadLine();
              _viewModel.ChangeLogPath(dest);
            }
            if (configChoice == 5)
            {
              Console.WriteLine(_viewModel.ConfigFilePathChanging);
              string dest = Console.ReadLine();
              _viewModel.ChangeConfigPath(dest);
            }
            if (configChoice == 6)
            {
              Console.WriteLine(_viewModel.StatusFilePathChanging);
              string dest = Console.ReadLine();
              _viewModel.ChangeStatusPath(dest);
            }
            else
            {
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
              bool value = _viewModel.ExecuteJobs();
              if (value)
              {
                Console.WriteLine(_viewModel.JobsExecutedSuccess);
              }
              else
              {
                Console.WriteLine(_viewModel.JobsExecutedFail);
              }

              //_viewModel.ExecuteMainOption(mainChoice);
            }
            else if (mainChoice == 2)
            {
              //_viewModel.ExecuteMainOption(mainChoice);
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
