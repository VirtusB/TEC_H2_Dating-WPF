﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO.Compression;
using System.Collections.Generic;

namespace TEC_H2_Dating
{
    /// <summary>
    /// Interaction logic for MyProfilePage.xaml
    /// </summary>
    public partial class MyProfilePage : Page
    {
        public MyProfilePage()
        {
            InitializeComponent();
        }

        public bool isPictureChosen;

        private void MyProfilePage_Loaded(object sender, RoutedEventArgs e)
        {
            lblProfileUsername.Content = $"Bruger: {LoginScreen.usernamePublic}";

            #region Hent profil data fra databasen og sæt det ind i de korrekte tekstboks

            int userID = LoginScreen.userID; // lokalt user id



            SqlConnection conn = new SqlConnection(@"Data Source=localhost; Initial Catalog=TEC_H2_Dating; Integrated Security=True;");

            SqlCommand getProfileData = new SqlCommand(@"SELECT profileFirstName, profileLastName, country, city, qReg.RegionName, age, sex, profileBio 
                                                            FROM Profiles qPro
                                                            FULL JOIN Regions qReg ON qPro.regionId = qReg.regionID
                                                            WHERE qPro.userID = @uID", conn);

            getProfileData.Parameters.Add(new SqlParameter("@uID", userID));

            conn.Open();

            #region Hent regioner fra databasen ind i region-comboboxen
            var regionList = new List<string>(); // liste af interesser
            SqlCommand getAllRegionsCmd = new SqlCommand("SELECT regionName FROM Regions", conn);
            SqlDataReader regiontReader = getAllRegionsCmd.ExecuteReader();

            while (regiontReader.Read())
            {
                string regiontName = regiontReader.GetString(0);
                txtProfileRegion.Items.Add(regiontName);
                regionList.Add(regiontName);
            }

            regiontReader.Close();
            #endregion


            SqlDataReader profileReader = getProfileData.ExecuteReader();

            while (profileReader.Read())
            {
                txtProfileFirstName.Text = profileReader.GetString(0);
                txtProfileLastName.Text = profileReader.GetString(1);
                txtProfileCountry.Text = profileReader.GetString(2);
                txtProfileCity.Text = profileReader.GetString(3);
                txtProfileRegion.SelectedItem = profileReader.GetString(4);
                txtProfileAge.Text = profileReader.GetInt32(5).ToString();

                if (profileReader.GetBoolean(6) == false)
                {
                    checkboxProfileFemale.IsChecked = true;
                }
                else if (profileReader.GetBoolean(6) == true)
                {
                    checkboxProfileMale.IsChecked = true;
                }

                txtProfileBio.Text = profileReader.GetString(7);

            }

            profileReader.Close();
            profileReader.Dispose();


            // vis profil billede fra databasen



            SqlCommand command = new SqlCommand("SELECT TOP 1 imageFile FROM Images WHERE userID = @uID ORDER BY CREATED DESC", conn);
            command.Parameters.AddWithValue("@uID", userID);


            Byte[] content = command.ExecuteScalar() as Byte[];



            byte[] MyData = new byte[0];

            if (content != null)
            {
                MyData = content;

                MemoryStream stream = new MemoryStream(MyData);
                stream.Write(MyData, 0, MyData.Length);
                stream.Position = 0;
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = ms;
                bi.EndInit();



                profileImageBox.Source = bi;
            }












            conn.Close();
            conn.Dispose();

            #endregion


        }

        private void btnUpdateProfil_Click(object sender, RoutedEventArgs e)
        {
            #region Error checks

            #region firstname
            if (txtProfileFirstName.Text == "")
            {
                MessageBox.Show("Fornavn må ikke være tomt.");
                txtProfileFirstName.Focus();
                return;
            }
            else if (txtProfileFirstName.Text.Length < 2)
            {
                MessageBox.Show("Fornavn skal mindst være 2 karakterer.");
                txtProfileFirstName.Focus();
                return;
            }
            else if (txtProfileFirstName.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Fornavn må ikke indeholde tal.");
                txtProfileFirstName.Focus();
                return;
            }
            #endregion

            #region lastname
            if (txtProfileLastName.Text == "")
            {
                MessageBox.Show("Efternavn må ikke være tomt.");
                txtProfileLastName.Focus();
                return;
            }
            else if (txtProfileLastName.Text.Length < 2)
            {
                MessageBox.Show("Efternavn skal mindst være 2 karakterer.");
                txtProfileLastName.Focus();
                return;
            }
            else if (txtProfileLastName.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Efternavn må ikke indeholde tal.");
                txtProfileLastName.Focus();
                return;
            }
            #endregion

            #region Country
            if (txtProfileCountry.Text == "")
            {
                MessageBox.Show("Land må ikke være tomt.");
                txtProfileCountry.Focus();
                return;
            }
            else if (txtProfileCountry.Text.Length < 2)
            {
                MessageBox.Show("Land skal mindst være 2 karakterer.");
                txtProfileCountry.Focus();
                return;
            }
            else if (txtProfileCountry.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Land må ikke indeholde tal.");
                txtProfileCountry.Focus();
                return;
            }
            #endregion

            #region City
            if (txtProfileCity.Text == "")
            {
                MessageBox.Show("By må ikke være tomt.");
                txtProfileCity.Focus();
                return;
            }
            else if (txtProfileCity.Text.Length < 2)
            {
                MessageBox.Show("By skal mindst være 2 karakterer.");
                txtProfileCity.Focus();
                return;
            }
            else if (txtProfileCity.Text.Any(char.IsDigit))
            {
                MessageBox.Show("By må ikke indeholde tal.");
                txtProfileCity.Focus();
                return;
            }
            #endregion


            #region Age

            if (txtProfileAge.Text.Any(char.IsLetter))
            {
                MessageBox.Show("Alder må kun indeholde tal");
                txtProfileAge.Focus();
                return;
            }
            else if (txtProfileAge.Text == "")
            {
                MessageBox.Show("Du skal udfylde din alder");
                txtProfileAge.Focus();
                return;
            }

            else if (Convert.ToInt32(txtProfileAge.Text.ToString()) < 18 || Convert.ToInt32(txtProfileAge.Text.ToString()) > 99)
            {

                MessageBox.Show("Du skal være over 18 og under 99 for at oprette en profil");
                txtProfileAge.Focus();
                return;
            }


            #endregion

            #region Køn

            if (checkboxProfileMale.IsChecked == true && checkboxProfileFemale.IsChecked == true)
            {
                MessageBox.Show("Du kan ikke vælge begge køn.");
                return;
            }
            else if (checkboxProfileMale.IsChecked == false && checkboxProfileFemale.IsChecked == false)
            {
                MessageBox.Show("Du skal vælge dit køn.");
                return;
            }


            #endregion

            #region Bio

            if (txtProfileBio.Text.Length > 279)
            {
                MessageBox.Show("Beskrivelse må ikke være på mere end 280 karakterer.");
                txtProfileBio.Focus();
                return;
            }
            else if (txtProfileBio.Text.Length < 1)
            {
                MessageBox.Show("Beskrivelse må ikke være tom");
                txtProfileBio.Focus();
                return;
            }

            #endregion




            #endregion



            #region Indsæt profilen i databasen

            int userID = LoginScreen.userID; // hent userID fra LoginScreen

            // hent alle værdier fra profilen til lokale variabler
            string profileFornavn = HomePage.FirstCharToUpper(this.txtProfileFirstName.Text.ToLower());
            string profileEfternavn = HomePage.FirstCharToUpper(this.txtProfileLastName.Text.ToLower());
            string profileLand = HomePage.FirstCharToUpper(this.txtProfileCountry.Text.ToLower());
            string profileBy = HomePage.FirstCharToUpper(this.txtProfileCity.Text.ToLower());
            string profileRegion = this.txtProfileRegion.Text;
            int profileAlder = Convert.ToInt32(this.txtProfileAge.Text);
            bool profileSex; // hvis true, mand, hvis false, kvinde.
            string profileBio = this.txtProfileBio.Text;

            if (this.checkboxProfileMale.IsChecked == true)
            {
                profileSex = true;
            }
            else if (this.checkboxProfileFemale.IsChecked == true)
            {
                profileSex = false;
            }
            else
            {
                MessageBox.Show("Fejl relaretet til valg af køn");
                return;
            }



            SqlConnection conn = new SqlConnection(@"Data Source=localhost; Initial Catalog=TEC_H2_Dating; Integrated Security=True;");

            conn.Open();

            SqlCommand updateProfile = new SqlCommand("UPDATE Profiles SET profileFirstName = @fName, profileLastName = @lName, profileBio = @pBio, sex = @pSex, age = @pAge, country = @pCountry, city = @pCity, regionId = (SELECT RegionID from Regions WHERE regionName = @pRegion) WHERE Profiles.userID = @uID", conn);

            // tilføj alle lokale variabler til sql kommandoen
            updateProfile.Parameters.Add(new SqlParameter("@uID", userID));
            updateProfile.Parameters.Add(new SqlParameter("@fName", profileFornavn));
            updateProfile.Parameters.Add(new SqlParameter("@lName", profileEfternavn));
            updateProfile.Parameters.Add(new SqlParameter("@pBio", profileBio));
            updateProfile.Parameters.Add(new SqlParameter("@pSex", profileSex));
            updateProfile.Parameters.Add(new SqlParameter("@pAge", profileAlder));
            updateProfile.Parameters.Add(new SqlParameter("@pCountry", profileLand));
            updateProfile.Parameters.Add(new SqlParameter("@pCity", profileBy));
            updateProfile.Parameters.Add(new SqlParameter("@pRegion", profileRegion));


            if (updateProfile.ExecuteNonQuery() == 1)
            {
                MessageBox.Show($"Din profil blev opdateret, {profileFornavn} {profileEfternavn}");
                // TODO, reload siden                           
            }
            else
            {
                MessageBox.Show("Fejl under opdatering");
            }




            conn.Close();
            conn.Dispose();

            #endregion
        }




        private void btnChooseProfileImage_Click(object sender, RoutedEventArgs e)
        {
            int userID = LoginScreen.userID; // hent userID fra LoginScreen

            // file vindue

            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.Filter = "Billeder (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            //fileDialog.ShowDialog(); // bliver kaldt i error tjeket lige nede under

            // fil stream



            if (fileDialog.ShowDialog() == false)
            {
                MessageBox.Show("Billedet må ikke være tomt");
                return;
            }


            FileStream fs = new FileStream(fileDialog.FileName, FileMode.Open, FileAccess.Read);


            byte[] data = new byte[fs.Length];









            if (data.Length > 250000)
            {
                int imgSize = data.Length / 1000;
                MessageBox.Show($"Billedet er for stort. Max 250KB, det valgte billede fylder {imgSize}KB");
                return;
            }
            else if (data.Length < 10000)
            {
                int imgSize = data.Length / 1000;
                MessageBox.Show($"Billedet er for småt. Mindst 10KB, det valgte billede fylder {imgSize}KB");
                return;
            }

            fs.Read(data, 0, System.Convert.ToInt32(fs.Length));

            fs.Close(); // luk file stream


            // indsæt billede i databasen

            SqlConnection conn = new SqlConnection(@"Data Source=localhost; Initial Catalog=TEC_H2_Dating; Integrated Security=True;");

            conn.Open();

            SqlCommand savePictureCmd = new SqlCommand("INSERT INTO Images(userID, imageFile) VALUES (@uID, @imageData)", conn);

            savePictureCmd.Parameters.AddWithValue("@uID", userID);
            savePictureCmd.Parameters.AddWithValue("@imageData", data);
            savePictureCmd.ExecuteNonQuery();

            conn.Close(); // luk conn

            // vis billedet, ikke fra databasen i dette eksempel
            //ImageSourceConverter imgs = new ImageSourceConverter();
            //profileImageBox.SetValue(Image.SourceProperty, imgs.ConvertFromString(fileDialog.FileName.ToString()))   




            conn.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter sqa = new SqlDataAdapter("SELECT TOP 1 imageFile FROM Images WHERE userID = @uID ORDER BY CREATED DESC", conn);
            sqa.SelectCommand.Parameters.AddWithValue("@uID", userID);


            sqa.Fill(ds);




            byte[] DBdata = (byte[])ds.Tables[0].Rows[0][0];
            MemoryStream strm = new MemoryStream();
            strm.Write(data, 0, data.Length);
            strm.Position = 0;
            System.Drawing.Image img = System.Drawing.Image.FromStream(strm);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            profileImageBox.Source = bi;


            SqlCommand deleteAllPriorImg = new SqlCommand("DELETE FROM Images WHERE(Images.userID = @uID) AND(Images.created < (SELECT TOP 1 created FROM Images WHERE images.userID = @uID ORDER BY created DESC))", conn); // kommando som sletter alle images uploaded, andet end det nyeste billede
            deleteAllPriorImg.Parameters.AddWithValue("@uID", userID);

            deleteAllPriorImg.ExecuteNonQuery();


            conn.Close();

        }

        private void MyProfilePage_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnInteresserProfil_Click(object sender, RoutedEventArgs e)
        {
            ChooseProfileInterests CPS = new ChooseProfileInterests();
            CPS.Show();
        }
    }
}
