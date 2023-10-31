using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BC_club_manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AP_resource_suggestion APWindow;
        Competition CompetitionWindow;
        public MainWindow()
        {
            InitializeComponent();
            APWindow = new AP_resource_suggestion();
            APWindow.Closed += APWindow_Closed;
            CompetitionWindow = new Competition();
            CompetitionWindow.Closed += CompetitionWindow_Closed;
        }

        private void CompetitionWindow_Closed(object? sender, EventArgs e)
        {
            CompetitionWindow = new Competition();
            CompetitionWindow.Closed += CompetitionWindow_Closed;
        }

        private void APWindow_Closed(object? sender, EventArgs e)
        {
            APWindow = new AP_resource_suggestion();
            APWindow.Closed += APWindow_Closed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //AP
            APWindow.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //比赛
            CompetitionWindow.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
