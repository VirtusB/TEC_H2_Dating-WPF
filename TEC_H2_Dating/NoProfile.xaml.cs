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
using System.Windows.Shapes;

namespace TEC_H2_Dating
{
    /// <summary>
    /// Interaction logic for NoProfile.xaml
    /// </summary>
    public partial class NoProfile : Window
    {
        public NoProfile()
        {
            InitializeComponent();
        }

        private void noProfileBtn_Click(object sender, RoutedEventArgs e)
        {
            ProfileCreation profilOprettelse = new ProfileCreation();
            profilOprettelse.Show();
            this.Close();       
        }
    }
}
