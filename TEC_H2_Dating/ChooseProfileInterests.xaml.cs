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




        private void ChooseProfileInterestsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            #region Hent interesser fra databasen ind i checkbokse
            var interestList = new List<string>(); // liste af interesser


            SqlConnection conn = new SqlConnection(@"Data Source=localhost; Initial Catalog=TEC_H2_Dating; Integrated Security=True;");

            SqlCommand getAllInterestsCmd = new SqlCommand("SELECT interestName FROM Interests", conn);

            conn.Open();

            SqlDataReader interestReader = getAllInterestsCmd.ExecuteReader();

            var converter = new System.Windows.Media.BrushConverter();
            var thickconverter = new System.Windows.ThicknessConverter();



            while (interestReader.Read())
            {
                string interestName = interestReader.GetString(0);
                interestList.Add(interestName);
                
                CheckBox box; 
                box = new CheckBox();
                box.Tag = interestName + "box";
                box.Foreground = (Brush)converter.ConvertFromString("#FFF");
                //box.Padding = (Thickness)thickconverter.ConvertFromString("2");
                box.Margin = (Thickness)thickconverter.ConvertFromString("15");
                box.Content = interestName;
                interestsWrapPanel.Children.Add(box);
                
                

            }

            interestReader.Close();

            conn.Close();

            #endregion
        }
    }
}
