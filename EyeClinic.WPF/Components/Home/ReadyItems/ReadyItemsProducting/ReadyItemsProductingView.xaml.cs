﻿using EyeClinic.WPF.Components.Home.CartoonForm;
using System;
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

namespace EyeClinic.WPF.Components.Home.ReadyItems.ReadyItemsProducting
{
    /// <summary>
    /// Interaction logic for ReadyItemsProductingView.xaml
    /// </summary>
    public partial class ReadyItemsProductingView : UserControl
    {
        public ReadyItemsProductingView()
        {
            InitializeComponent();
        }

        private void search(object sender, TextChangedEventArgs e)
        {
            ((ReadyItemsProductingViewModel)this.DataContext).SearchCommand.Execute(null);
        }

    
        private void ComboBox_Selected(object sender, SelectionChangedEventArgs e)
        {
            ((ReadyItemsProductingViewModel)this.DataContext).SortByCommand.Execute(null);
        }
    }
}
