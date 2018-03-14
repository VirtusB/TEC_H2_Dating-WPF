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
using System.Windows.Shapes;

namespace TEC_H2_Dating
{
    /// <summary>
    /// Interaction logic for ChooseProfileInterests.xaml
    /// </summary>
    public partial class ChooseProfileInterests : Window
    {
        public ChooseProfileInterests()
        {
            InitializeComponent();
        }


        public int profileID = LoginScreen.profileID;
        public int userID = LoginScreen.userID;

        



        private void ChooseProfileInterestsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=localhost; Initial Catalog=TEC_H2_Dating; Integrated Security=True;");

            conn.Open();

            #region Tjek brugerens profileID
            // vi er nød til at få profileID fra databasen igen, hvis brugeren lige har oprettet en profil
            // hvis det ikke er brugerens først log ind, så kan vi bruge profileid fra LoginScreen
            if (profileID == 0)
            {
                SqlCommand profileExistence = new SqlCommand("SELECT profileID FROM Profiles WHERE Profiles.userID = @uID", conn);
                profileExistence.Parameters.Add(new SqlParameter("@uID", userID));

                object profileIDExist = profileExistence.ExecuteScalar();

                profileID = Convert.ToInt32(profileIDExist);
            }

            #endregion
        
            #region Få interesser

            var relationList = new List<int>();

            SqlCommand getProfileID = new SqlCommand("SELECT qRS.interestid FROM RS_ProfileInterests qRS FULL JOIN Interests qInt ON qInt.Interestid = qRS.interestID WHERE qRS.ProfileID = @pID", conn);
            getProfileID.Parameters.AddWithValue("@pID", profileID);


            SqlDataReader userRSreader = getProfileID.ExecuteReader();

            while (userRSreader.Read())
            {
                int rsID = userRSreader.GetInt32(0);
                relationList.Add(rsID);
            }

            userRSreader.Close();

            #endregion

            #region Hent interesser fra databasen ind i checkbokse
            //var interestList = new List<string>(); // liste af interesser

            var interestIDlist = new List<int>(); 

            

            SqlCommand getAllInterestsCmd = new SqlCommand("SELECT  interestName, interestID FROM Interests ORDER BY interestName ASC", conn);

            SqlDataReader interestReader = getAllInterestsCmd.ExecuteReader();

            var converter = new System.Windows.Media.BrushConverter();
            var thickconverter = new System.Windows.ThicknessConverter();




            while (interestReader.Read())
            {
                string interestName = interestReader.GetString(0);
                int interestID = interestReader.GetInt32(1);
                interestIDlist.Add(interestID);
           

                CheckBox box; 
                box = new CheckBox();
                box.Tag = interestID.ToString();
                box.Name = "boks" + interestID.ToString();
                box.Foreground = (Brush)converter.ConvertFromString("#FFF");
                box.Margin = (Thickness)thickconverter.ConvertFromString("15");
                box.Content = interestName;

                if (relationList.Contains(interestID))
                {
                    box.IsChecked = true;

                }

                interestsWrapPanel.Children.Add(box);

            }

            interestReader.Close();

            conn.Close();

            #endregion

            
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void btnProfileUpdateInterests_Click(object sender, RoutedEventArgs e)
        {
            #region Opdater interesser

            foreach (CheckBox tb in FindVisualChildren<CheckBox>(CPIwindow))
            {
                if (tb.IsChecked.HasValue && tb.IsChecked.Value)
                {
                    MessageBox.Show(tb.ToString());
                }
            }




            #endregion
        }

        


      
    }
}
