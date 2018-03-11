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

        private void filterProfilesButton_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=localhost; Initial Catalog=TEC_H2_Dating; Integrated Security=True;");
            conn.Open();

            int sexSelection1 = 5;
            int sexSelection2 = 5;

            if (sexSelect.Text == "Mænd")
            {
                sexSelection1 = 1;
                sexSelection2 = 1;
            }
            else if (sexSelect.Text == "Kvinder")
            {
                sexSelection1 = 0;
                sexSelection2 = 0;
            }
            else if (sexSelect.Text == "Begge")
            {
                sexSelection1 = 0;
                sexSelection2 = 1;
            }
            else
            {
                MessageBox.Show("Foretrukne køn er ikke et gyldigt valg");
                return;
            }

            string interestSelection = dashInterestsCombobox.Text;
            string zipSelection = zipSelect.Text;
            double ageSelection = dashboardAgeSlider.Value;

            SqlCommand loadFilterProfiles = new SqlCommand(@"SELECT profilefirstname, age, zipcode, qImg.imageFile, Profilebio from profiles qPro 
                                                            FULL JOIN Users qUse ON qPro.userID = qUse.userID 
                                                            FULL JOIN Images qImg ON qPro.userID = qImg.userID 
                                                            FULL JOIN RS_ProfileInterests qRS ON qRS.profileID = qPro.profileID 
                                                            FULL JOIN Interests qInt ON qInt.interestID = qRS.interestId 
                                                            WHERE qInt.interestID = (SELECT interestID FROM Interests WHERE interestName = @intSel) 
                                                            AND qPro.age = @ageSel AND qPro.zipcode = @zipSel AND qPro.age BETWEEN @sexSel1 AND @sexSel2", conn);                                         
            loadFilterProfiles.Parameters.AddWithValue("@intSel", interestSelection);
            loadFilterProfiles.Parameters.AddWithValue("@sexSel1", sexSelection1);
            loadFilterProfiles.Parameters.AddWithValue("@sexSel2", sexSelection2);
            loadFilterProfiles.Parameters.AddWithValue("@zipSel", zipSelection);
            loadFilterProfiles.Parameters.AddWithValue("@ageSel", ageSelection);
            loadFilterProfiles.ExecuteNonQuery();

            conn.Close(); // luk conn
        }
    }
}
