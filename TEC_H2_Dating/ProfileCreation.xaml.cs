using System;
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
using System.Windows.Shapes;


namespace TEC_H2_Dating
{
    /// <summary>
    /// Interaction logic for ProfileCreation.xaml
    /// </summary>
    public partial class ProfileCreation : Window
    {
        public ProfileCreation()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblProfileUsername.Content = $"Profil oprettelse for {LoginScreen.usernamePublic}";

        }

        // Tjek username og password
        public void btnProfileCreate_Click(object sender, RoutedEventArgs e)
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

            #region Zip

            if (txtProfileZipCode.Text.Length != 4)
            {
                MessageBox.Show("Postnummer skal være 4 karakterer.");
                txtProfileZipCode.Focus();
                return;
            }
            else if (txtProfileZipCode.Text.Any(char.IsLetter))
            {
                MessageBox.Show("Postnummer må kun indeholde tal");
                txtProfileZipCode.Focus();
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

            else if (Convert.ToInt32(txtProfileAge.Text.ToString()) < 18 || Convert.ToInt32(txtProfileAge.Text.ToString()) > 99 )
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

            #endregion

            #endregion

            int userID = LoginScreen.userID; // hent userID fra LoginScreen

            // hent alle værdier fra profilen til lokale variabler
            string profileFornavn = this.txtProfileFirstName.Text;
            string profileEfternavn = this.txtProfileLastName.Text;
            string profileLand = this.txtProfileCountry.Text;
            string profileBy = this.txtProfileCity.Text;
            int profilePostnummer = Convert.ToInt32(this.txtProfileZipCode.Text);
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

            SqlCommand insertProfile = new SqlCommand("INSERT INTO Profiles (userID, profileFirstName, profileLastName, profileBio, sex, age, country, city, zipcode) VALUES (@uID, @fName, @lName, @pBio, @pSex, @pAge, @pCountry, @pCity, @pZip)", conn);

            // tilføj alle lokale variabler til sql kommandoen
            insertProfile.Parameters.Add(new SqlParameter("@uID", userID));
            insertProfile.Parameters.Add(new SqlParameter("@fName", profileFornavn));
            insertProfile.Parameters.Add(new SqlParameter("@lName", profileEfternavn));
            insertProfile.Parameters.Add(new SqlParameter("@pBio", profileBio));
            insertProfile.Parameters.Add(new SqlParameter("@pSex", profileSex));
            insertProfile.Parameters.Add(new SqlParameter("@pAge", profileAlder));
            insertProfile.Parameters.Add(new SqlParameter("@pCountry", profileLand));
            insertProfile.Parameters.Add(new SqlParameter("@pCity", profileBy));
            insertProfile.Parameters.Add(new SqlParameter("@pZip", profilePostnummer));


            if (insertProfile.ExecuteNonQuery() == 1)
            {
                MessageBox.Show($"Din profil blev oprettet, {profileFornavn} {profileEfternavn}");
                MainWindow dashboard = new MainWindow();
                dashboard.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Fejl under oprettelse");
            }



            conn.Close();
        }

        private void btnChooseProfileImage_Click(object sender, RoutedEventArgs e)
        {
            int userID = LoginScreen.userID; // hent userID fra LoginScreen

            // file vindue

            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            //fileDialog.ShowDialog(); // bliver kaldt i error tjeket lige nede under

            // fil stream

            

            if (fileDialog.ShowDialog() == false)
            {
                MessageBox.Show("Billedet må ikke være tomt");
                return;
            }

            FileStream fs = new FileStream(fileDialog.FileName, FileMode.Open, FileAccess.Read);

            
            

            byte[] data = new byte[fs.Length];
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


            SqlCommand deleteAllPriorImg = new SqlCommand("INSERT INTO Images(userID, imageFile) VALUES (@uID, @imageData)", conn); // kommando som sletter alle images uploaded, andet end det nyeste billede



            conn.Close();
            
       

            

        }
    }
}