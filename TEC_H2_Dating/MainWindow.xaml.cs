using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Globalization;
using System.Threading;
using System.Windows.Markup;
using WPFCustomMessageBox;

namespace TEC_H2_Dating
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show($"Velkommen, {LoginScreen.usernamePublic}");

            // lblPublicUsername.Content = LoginScreen.usernamePublic; // HUSK

            this.Title = $"Velkommen, {LoginScreen.usernamePublic}";

        }

        private void btnDashboardHome_Click(object sender, RoutedEventArgs e)
        {
            HomePageFrame.Content = new HomePage();
        }

        private void btnDashboardMyProfile_Click(object sender, RoutedEventArgs e)
        {
            HomePageFrame.Content = new MyProfilePage();
        }

        private void btnDashboardSettings_Click(object sender, RoutedEventArgs e)
        {
            HomePageFrame.Content = new ProfileSettingsPage();
        }

        private void btnDashboardLogout_Click(object sender, RoutedEventArgs e)
        {
            // standard windows messagebox som bruger system sproget, hvis system sproget er engelsk står der "yes", "no"
            //MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Er du sikker?", "Log ud", System.Windows.MessageBoxButton.YesNo);


            // custom messagebox, her kan vi selv vælge hvad der skal stå i stedet for yes/no
            MessageBoxResult logoutResult = CustomMessageBox.ShowYesNo("Er du sikker?", "Log ud", "Ja", "Nej");

            if (logoutResult == MessageBoxResult.Yes)
            {
                this.Close();
            }

            
        }
    }
}
