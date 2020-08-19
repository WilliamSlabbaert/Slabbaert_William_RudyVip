using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPF_RudyVip
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        DispatcherTimer dt = new DispatcherTimer();
        public SplashScreen()
        {
            InitializeComponent();
            dt.Tick += new EventHandler(Dt_tick);
            dt.Interval = new TimeSpan(0, 0, 5);
            dt.Start();
        }
        private void Dt_tick(object sender,EventArgs e)
        {
            Reservations x = new Reservations();
            x.Show();
            x.Close();
            new MainWindow().Show();
            dt.Stop();

            this.Close();
        }
    }
}
