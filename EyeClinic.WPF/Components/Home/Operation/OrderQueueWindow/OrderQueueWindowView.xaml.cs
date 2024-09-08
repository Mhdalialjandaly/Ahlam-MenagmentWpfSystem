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
using System.Windows.Threading;

namespace EyeClinic.WPF.Components.Home.Operation.OrderQueueWindow
{
    /// <summary>
    /// Interaction logic for OrderQueueWindowView.xaml
    /// </summary>
    public partial class OrderQueueWindowView : UserControl
    {
        public OrderQueueWindowView()
        {
            InitializeComponent();
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += TimerOnTick;
            timer.Start();
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
