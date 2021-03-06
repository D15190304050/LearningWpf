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
using System.Reflection;

namespace EffectsAndVisuals
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateWindow_Click(object sender, RoutedEventArgs e)
        {
            // Get the current button.
            Button cmd = (Button)e.OriginalSource;

            // Create an instance of the window named by the current button.
            Type type = this.GetType();
            Assembly assembly = type.Assembly;
            Window window = (Window)assembly.CreateInstance(type.Namespace + "." + cmd.Content);

            // Show the window.
            window.ShowDialog();
        }
    }
}
