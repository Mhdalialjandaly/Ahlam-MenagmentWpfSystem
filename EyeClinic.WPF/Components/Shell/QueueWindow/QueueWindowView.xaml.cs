using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EyeClinic.WPF.Components.Shell.QueueWindow
{
    /// <summary>
    /// Interaction logic for QueueWindowView.xaml
    /// </summary>
    public partial class QueueWindowView : Window
    {
        public QueueWindowView()
        {
            InitializeComponent();

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += TimerOnTick;
            timer.Start();
        }

        private void CloseThis(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeThis(object sender, RoutedEventArgs e)
        {
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.WindowState = WindowState.Normal;
        }

        private void MaximizeTihs(object sender, RoutedEventArgs e)
        {
            this.WindowState= WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            
            if (DateTime.Now.TimeOfDay.Seconds % 2 == 0)
            {
                CurrentTextView.Visibility = Visibility.Visible;
                
            }
            else
            {
                CurrentTextView.Visibility = Visibility.Hidden;
            }
        }
    }
}
