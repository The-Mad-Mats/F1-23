using System;
using System.Collections.Generic;
using System.IO;
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

namespace F1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool DoSwitch = false;
        public bool DoSwitchOrg = false;
        public MainWindow()
        {
            InitializeComponent();
            int sec = 5;
            var info = new FileInfo("Names\\Settings.txt");
            using (StreamReader reader = info.OpenText())
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        break;
                    var lines = line.Split(';');
                    DoSwitch = Convert.ToBoolean(lines[1]);
                    DoSwitchOrg = DoSwitch;
                    sec = Convert.ToInt32(lines[2]);
                }
            }
            if (DoSwitch)
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(sec);
                timer.Tick += timer_Tick;
                timer.Start();
            }
            DispatcherTimer timer2 = new DispatcherTimer();
            timer2.Interval = TimeSpan.FromMilliseconds(500);
            timer2.Tick += timer2_Tick;
            timer2.Start();
        }


        void timer_Tick(object sender, EventArgs e)
        {
            if (DoSwitch)
            {
                var x = Tab.SelectedIndex;
                if (x == 0)
                    x = 1;
                else
                    x = 0;
                Dispatcher.BeginInvoke((Action)(() => Tab.SelectedIndex = x));
            }
        }
        void timer2_Tick(object sender, EventArgs e)
        {
            if (ViewModel.UdpAction1Pressed)
            {
                var x = Tab.SelectedIndex;
                if (x == 0)
                    x = 1;
                else
                    x = 0;
                Dispatcher.BeginInvoke((Action)(() => Tab.SelectedIndex = x));
                ViewModel.UdpAction1Pressed = false;
            }
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Tab.SelectedIndex > 1)
            {
                DoSwitch = false;
            }
            else
            {
                DoSwitch = DoSwitchOrg;
            }
        }
    }
}
