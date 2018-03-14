﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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

        

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }


        public List<Profile> listOfProfiles = new List<Profile>(); // liste af profiler


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

            String queryChoice = "EMPTY";

            SqlCommand loadFilterProfiles;



            if (zipSelect.Text == "Alle" && dashInterestsCombobox.Text != "Alle") // alle postnumre, specifik interesse
            {
                queryChoice = @"SELECT profilefirstname, age, city, qImg.imageFile, Profilebio from profiles qPro 
                                                            FULL JOIN Users qUse ON qPro.userID = qUse.userID 
                                                            FULL JOIN Images qImg ON qPro.userID = qImg.userID 
                                                            FULL JOIN RS_ProfileInterests qRS ON qRS.profileID = qPro.profileID 
                                                            FULL JOIN Interests qInt ON qInt.interestID = qRS.interestId 
                                                            WHERE qInt.interestID = (SELECT interestID FROM Interests WHERE interestName = @intSel) 
                                                            AND qPro.age = @ageSel AND qPro.sex BETWEEN @sexSel1 AND @sexSel2";
            }
            else if (zipSelect.Text == "Alle" && dashInterestsCombobox.Text == "Alle") // alle postnumre, alle interesser
            {
                queryChoice = @"SELECT profilefirstname, age, city, qImg.imageFile, Profilebio from profiles qPro 
                                                            FULL JOIN Users qUse ON qPro.userID = qUse.userID 
                                                            FULL JOIN Images qImg ON qPro.userID = qImg.userID 
                                                            FULL JOIN RS_ProfileInterests qRS ON qRS.profileID = qPro.profileID 
                                                            FULL JOIN Interests qInt ON qInt.interestID = qRS.interestId 
                                                            WHERE qPro.age = @ageSel AND qPro.sex BETWEEN @sexSel1 AND @sexSel2";
            }
            else if (zipSelect.Text != "Alle" && dashInterestsCombobox.Text == "Alle") // specifikt postnummer, alle interesser
            {
                queryChoice = @"SELECT profilefirstname, age, city, qImg.imageFile, Profilebio from profiles qPro 
                                                            FULL JOIN Users qUse ON qPro.userID = qUse.userID 
                                                            FULL JOIN Images qImg ON qPro.userID = qImg.userID 
                                                            FULL JOIN RS_ProfileInterests qRS ON qRS.profileID = qPro.profileID 
                                                            FULL JOIN Interests qInt ON qInt.interestID = qRS.interestId 
                                                            WHERE qPro.age = @ageSel AND qPro.zipcode = @zipSel AND qPro.sex BETWEEN @sexSel1 AND @sexSel2";
            }
            else if (zipSelect.Text != "Alle" && dashInterestsCombobox.Text != "Alle") // specifikt postnummer, specifik interesse
            {
                queryChoice = @"SELECT profilefirstname, age, city, qImg.imageFile, Profilebio from profiles qPro 
                                                            FULL JOIN Users qUse ON qPro.userID = qUse.userID 
                                                            FULL JOIN Images qImg ON qPro.userID = qImg.userID 
                                                            FULL JOIN RS_ProfileInterests qRS ON qRS.profileID = qPro.profileID 
                                                            FULL JOIN Interests qInt ON qInt.interestID = qRS.interestId 
                                                            WHERE qInt.interestID = (SELECT interestID FROM Interests WHERE interestName = @intSel) 
                                                            AND qPro.age = @ageSel AND qPro.zipcode = @zipSel AND qPro.sex BETWEEN @sexSel1 AND @sexSel2";
            }

            string interestSelection = dashInterestsCombobox.Text;
            string zipSelection = zipSelect.Text;
            double ageSelection = dashboardAgeSlider.Value;


            loadFilterProfiles = new SqlCommand(queryChoice, conn);
            loadFilterProfiles.Parameters.AddWithValue("@intSel", interestSelection);
            loadFilterProfiles.Parameters.AddWithValue("@sexSel1", sexSelection1);
            loadFilterProfiles.Parameters.AddWithValue("@sexSel2", sexSelection2);
            loadFilterProfiles.Parameters.AddWithValue("@zipSel", zipSelection);
            loadFilterProfiles.Parameters.AddWithValue("@ageSel", ageSelection);



            SqlDataReader profileReader = loadFilterProfiles.ExecuteReader();



            






            if (!profileReader.HasRows)
            {
                MessageBox.Show("Ingen profiler fundet, nedsæt søgekrititer");
                return;
            }
            else
            {
                while (profileReader.Read())
                {
                    listOfProfiles.Add(new Profile
                    {
                        FirstName = profileReader.GetString(0),
                        Age = profileReader.GetInt32(1),
                        City = profileReader.GetString(2),
                        ProfileBio = profileReader.GetString(4),
                        ProfileImage = (byte[])profileReader["imageFile"]
                    });
                }


                        
            }

            MemoryStream strm = new MemoryStream(listOfProfiles[0].ProfileImage);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            strm.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = strm;
            bi.EndInit();
            hpProfileImage.Source = bi;
        
            txtProfileBio.Text = listOfProfiles[0].ProfileBio; // sæt beskrivelse
            txtProfileInfo.Text = $"{listOfProfiles[0].FirstName}, {listOfProfiles[0].Age.ToString()}, {listOfProfiles[0].City}"; // sæt fornavn, alder, by
            txtTextBtn.Text = $"Vis {listOfProfiles[0].FirstName}'s Profil"; // sæt teksten som står under vis profil knappen, altså f.eks. "Vis Camilla's Profil"


            conn.Close(); // luk conn
            conn.Dispose();
        }

        private void HomePage_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnHPimage_Click(object sender, RoutedEventArgs e)
        {
            // vis fulde billede i nyt vindue
            FullPictureView FPV = new FullPictureView();
            FPV.FPVimage.Source = hpProfileImage.Source;
            FPV.Show();
        }

        public int _I = 0; // bruges til indeksering af Profile liste

        public void btnIncrementSearch_Click(object sender, RoutedEventArgs e)
        {
            _I++;
            if (_I > listOfProfiles.Count - 1)
            {
                _I--;
                MessageBox.Show("Ikke flere matches");
                return;
            }
            else
            {
                txtProfileBio.Text = listOfProfiles[_I].ProfileBio;
                txtProfileInfo.Text = $"{listOfProfiles[_I].FirstName}, {listOfProfiles[_I].Age.ToString()}, {listOfProfiles[_I].City}";
                txtTextBtn.Text = $"Vis {listOfProfiles[_I].FirstName}'s Profil";

                // profil billede
                MemoryStream strm = new MemoryStream(listOfProfiles[_I].ProfileImage);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                strm.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = strm;
                bi.EndInit();
                hpProfileImage.Source = bi;
            }
    
            
        }

        public void btnDecrementSearch_Click(object sender, RoutedEventArgs e)
        {
            _I--;

            if (_I < 0)
            {
                _I++;
                MessageBox.Show("Ikke flere matches");
                return;
            }
            else
            {
                txtProfileBio.Text = listOfProfiles[_I].ProfileBio;
                txtProfileInfo.Text = $"{listOfProfiles[_I].FirstName}, {listOfProfiles[_I].Age.ToString()}, {listOfProfiles[_I].City}";
                txtTextBtn.Text = $"Vis {listOfProfiles[_I].FirstName}'s Profil";

                // profil billede
                MemoryStream strm = new MemoryStream(listOfProfiles[_I].ProfileImage);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                strm.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = strm;
                bi.EndInit();
                hpProfileImage.Source = bi;
            }

         
            
        }
    }
}
