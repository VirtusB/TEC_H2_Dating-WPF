using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace TEC_H2_Dating
{
    /// <summary>
    /// Interaction logic for RegisterScreen.xaml
    /// </summary>
    public partial class RegisterScreen : Window
    {
        public RegisterScreen()
        {
            InitializeComponent();
        }

        public static bool IsAllLettersOrDigits(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsLetterOrDigit(c))
                    return false;
            }
            return true;
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        public static String GenerateSHA256Hash(String input)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
            System.Security.Cryptography.SHA256Managed sha256hashstring = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sha256hashstring.ComputeHash(bytes);

            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }

        private void btnRegisterSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Error checks
            if (txtUsernameRegister.Text == "")
            {
                MessageBox.Show("Brugernavn må ikke være tomt.");
                txtUsernameRegister.Focus();
                return;
            }
            else if (txtUsernameRegister.Text.Length < 4)
            {
                MessageBox.Show("Brugernavn skal mindst være 4 karakterer.");
                txtUsernameRegister.Focus();
                return;
            }
            else if (IsAllLettersOrDigits(txtUsernameRegister.Text) != true)
            {
                MessageBox.Show("Brugernavn må ikke indeholde specielle tegn.");
                txtUsernameRegister.Focus();
                return;
            }
            else if (IsValidEmail(txtEmailRegister.Text) != true)
            {
                MessageBox.Show("Ugyldig email adresse");
                txtEmailRegister.Focus();
                return;
            }
            else if (txtPasswordRegister.Password == "")
            {
                MessageBox.Show("Password må ikke være tomt.");
                txtPasswordRegister.Focus();
                return;
            }
            else if (txtPasswordRegister.Password.Length < 4)
            {
                MessageBox.Show("Password skal mindst være 4 karakterer.");
                txtPasswordRegister.Focus();
                return;
            }
            else if (txtPasswordRegister.Password != txtConfirmPasswordRegister.Password)
            {
                MessageBox.Show("De to indtastede adgangskoder er ikke ens. Prøv igen.");
                txtPasswordRegister.Focus();
                return;
            }
            
            #endregion

            #region Tjek om brugeren eksisterer
            // Tjek om brugeren allerede eksisterer

            SqlConnection conn = new SqlConnection(@"Data Source=localhost; Initial Catalog=TEC_H2_Dating; Integrated Security=True;");
                
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();

                    SqlCommand checkExistenceCmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE username = @uName ", conn);
                  
                    checkExistenceCmd.Parameters.Add(new SqlParameter("@uName", txtUsernameRegister.Text));

                    int userExist = (int)checkExistenceCmd.ExecuteScalar();

                    if (userExist == 1)
                    {

                        MessageBox.Show("Brugernavn allerede taget. Vælg et andet.");
                        txtUsernameRegister.Focus();
                        return; // stop
                    }
                    


                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            #endregion

            #region Indsæt brugeren i databasen, password krypteret med SHA256

            //String hashedPassword = GenerateSHA256Hash(txtPasswordRegister.Password);

            String firstHalfSalt = "$ecure";
            String secondHalfSalt = "Pa$$w0rd!";

            String finalPassword = firstHalfSalt + txtPasswordRegister.Password + secondHalfSalt;

            String finalHashedPassword = GenerateSHA256Hash(finalPassword);

            

            SqlCommand addUserCmd = new SqlCommand("INSERT INTO Users (username, email, userpassword) VALUES (@usUsername, @usEmail, @usPassword)", conn);
            addUserCmd.Parameters.Add(new SqlParameter("@usUsername", txtUsernameRegister.Text));
            addUserCmd.Parameters.Add(new SqlParameter("@usEmail", txtEmailRegister.Text));
            addUserCmd.Parameters.Add(new SqlParameter("@usPassword", finalHashedPassword));

            addUserCmd.ExecuteNonQuery();

            MessageBox.Show($"Bruger blev oprettet: {txtUsernameRegister.Text}");

            conn.Close();

            // gå til login skærmen
            LoginScreen loginScreen = new LoginScreen();
            loginScreen.Show();
            this.Close();

            #endregion
        }

        
    }
}
