﻿using EZSave.Client.ViewModels;
using System.Windows;
using FontAwesome.WPF;

namespace EZSave.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void Button_Click()
        {

        }
    }
}