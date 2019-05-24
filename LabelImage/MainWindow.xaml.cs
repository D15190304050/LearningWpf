﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LabelImage
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Configuration configuration;

        public MainWindow()
        {
            InitializeComponent();

            
            //NavigationCommands
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            configuration = (Configuration) mainGrid.DataContext;
            file.FontSize = 40;
        }
    }
}