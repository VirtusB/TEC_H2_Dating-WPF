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
    }
}
