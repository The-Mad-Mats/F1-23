using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace F1
{
    /// <summary>
    /// Interaction logic for SelectDrivers.xaml
    /// </summary>
    public partial class SelectDrivers : Window
    {

        //public StackPanel innerStack;
        public SelectDrivers()
        {
            InitializeComponent();
        }
        private void SelectedDrivers_Loaded(object sender, RoutedEventArgs e)
        {
            //innerStack = new StackPanel
            //{
            //    Orientation = Orientation.Vertical
            //};

            //foreach (var c in AllDrivers)
            //{
            //    CheckBox cb = new CheckBox();
            //    cb.Name = c.Item1.ToString();
            //    cb.Content = c.Item1.ToString();
            //    cb.IsChecked = c.Item2;
            //    innerStack.Children.Add(cb);
            //}
            //Grid.SetColumn(innerStack, 0);
            //Grid.SetRow(innerStack, 0);
            //Grid.SetColumnSpan(innerStack, 1);
            //Grid.SetRowSpan(innerStack, 1);

            //SelectedDrivers.Children.Add(innerStack);

        }
    }
}
