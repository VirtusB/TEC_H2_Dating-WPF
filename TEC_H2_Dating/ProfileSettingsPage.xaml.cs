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

namespace TEC_H2_Dating
{

    /// <summary>
    /// Interaction logic for ProfileSettingsPage.xaml
    /// </summary>
    public partial class ProfileSettingsPage : Page
    {
        public ProfileSettingsPage()
        {

            InitializeComponent();

            

        }

        public int cCount;

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {           
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }



        private void lightCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save(); // gemmer "lightTheme" setting
        }

        private void lightCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();     
        }

        

        public void lightCheckbox_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            
            MainWindow MW = new MainWindow(); // initialiser nyt mainwindow
            CloseMainWindowNow();   // luk det gamle mainwindow
            MW.Show(); // vis det nye mainwindow
            MW.HomePageFrame.Content = new ProfileSettingsPage(); // sæt content til profile settings page
            
        }


        public void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            cCount++;

        }







        public void CloseMainWindowNow()
        {
            var mainWindow = (Application.Current.MainWindow as MainWindow);
            if (mainWindow != null)
            {
                mainWindow.Close();
            }
        }
    }
    



}
