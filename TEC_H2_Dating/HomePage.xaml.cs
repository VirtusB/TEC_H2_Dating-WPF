using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            #region Hent interesser fra databasen ind i interesse-comboboxen
            var interestList = new List<string>(); // liste af interesser




            SqlConnection conn = new SqlConnection(@"Data Source=localhost; Initial Catalog=TEC_H2_Dating; Integrated Security=True;");

            SqlCommand getAllInterestsCmd = new SqlCommand("SELECT interestName FROM Interests", conn);

            conn.Open();

            SqlDataReader interestReader = getAllInterestsCmd.ExecuteReader();

            while (interestReader.Read())
            {
                string interestName = interestReader.GetString(0);
                dashInterestsCombobox.Items.Add(interestName);
                interestList.Add(interestName);
            }

            interestReader.Close();

            conn.Close();

            #endregion

        }

        
    }
}
