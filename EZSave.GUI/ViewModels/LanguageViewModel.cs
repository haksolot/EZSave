using EZSave.GUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace EZSave.GUI.ViewModels
{
    public class LanguageViewModel: INotifyPropertyChanged
    {
        public ICommand ChangeLanguageCommand { get; }

        public LanguageViewModel()
        {
            ChangeLanguageCommand = new RelayCommand<string>(ChangeLanguage);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void ChangeLanguage(string languageCode)
        {
            CultureInfo newCulture = new CultureInfo(languageCode);
            CultureInfo.CurrentCulture = newCulture;
            CultureInfo.CurrentUICulture = newCulture;
            Resources.Culture = newCulture;

            UpdateDataGridHeaders();

            OnPropertyChanged("DataGridNom");
            OnPropertyChanged("DataGridSource");
            OnPropertyChanged("DataGridDestination");
            OnPropertyChanged("DataGridType");
            OnPropertyChanged(string.Empty); 

        }



        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateDataGridHeaders()
        {
            foreach (var window in Application.Current.Windows)
            {
                if (window is Window w)
                {
                    foreach (var child in LogicalTreeHelper.GetChildren(w))
                    {
                        if (child is DataGrid dataGrid)
                        {
                            foreach (var column in dataGrid.Columns)
                            {
                                BindingOperations.GetBindingExpressionBase(column, DataGridTextColumn.HeaderProperty)?.UpdateTarget();
                            }
                        }
                    }
                }
            }
        }
        public string ButtonAdd => Resources.ConfigOption1;
        public string ButtonRefresh => Resources.RefreshJobs;
        public string ButtonConfig => Resources.ConfModeTitle;
        public string ButtonExecuteAll => Resources.ExecuteAllJobs;
        public string ButtonExecuteOne => Resources.ExecuteOneJob;
        public string DataGridNom => Resources.JobName;
        public string DataGridSource => Resources.JobSource;
        public string DataGridDestination => Resources.JobDestination;
        public string DataGridType => Resources.JobType;
    }
}
