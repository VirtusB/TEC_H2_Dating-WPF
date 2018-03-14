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
using TEC_H2_Dating;

namespace TEC_H2_Dating
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        #region Default Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public LoginScreen()
        {
            InitializeComponent();
        }
        #endregion

        public static string usernamePublic = "";
        public static int userID = 0;
        public static int profileID = 0;

        // Tjek username og password
        public void btnLoginSubmit_Click(object sender, RoutedEventArgs e)
        {            
           
            #region Error checks
            if (txtPasswordLogin.Password == "")
            {
                MessageBox.Show("Password må ikke være tomt.");
                txtPasswordLogin.Focus();
                return;
            }
            else if (txtPasswordLogin.Password.Length < 4)
            {
                MessageBox.Show("Password skal mindst være 4 karakterer.");
                txtPasswordLogin.Focus();
                return;
            }

            #endregion

            #region Login handler

            //String hashedPassword = RegisterScreen.GenerateSHA256Hash(txtPasswordLogin.Password);

            String firstHalfSalt = "$ecure";
            String secondHalfSalt = "Pa$$w0rd!";

            String finalPassword = firstHalfSalt + txtPasswordLogin.Password + secondHalfSalt;

            String finalHashedPassword = RegisterScreen.GenerateSHA256Hash(finalPassword);


            SqlConnection conn = new SqlConnection(@"Data Source=localhost; Initial Catalog=TEC_H2_Dating; Integrated Security=True;");

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();

                    SqlCommand verifyLogin = new SqlCommand("SELECT userID FROM Users WHERE userpassword = @uPass AND username = @uName ", conn);

                    verifyLogin.Parameters.Add(new SqlParameter("@uPass", finalHashedPassword));
                    verifyLogin.Parameters.Add(new SqlParameter("@uName", txtUsernameLogin.Text));



                   

                    object infoCorrect = verifyLogin.ExecuteScalar();
                    

                    if (infoCorrect == null) // vis en besked hvis de indtaste informationer er forkerte
                    {                      
                        MessageBox.Show("Brugernavn eller adgangskode forkert");
                    }
                    else
                    {
                        userID = (int)infoCorrect;

                        SqlCommand profileExistence = new SqlCommand("SELECT profileID FROM Profiles WHERE Profiles.userID = @uID", conn);
                        profileExistence.Parameters.Add(new SqlParameter("@uID", userID));

                        object profileIDExist = profileExistence.ExecuteScalar();
                        //profileID = (int)profileIDExist; // virker kun hvis brugeren har en profil, ellers kommer der en fejl ved login
                        profileID = Convert.ToInt32(profileIDExist); // sætter profileid til 0, hvis brugeren ikke har en profil. dette fejl senere, da profileid bliver brugt andre steder i programmet


                        if (profileIDExist == null)
                        {
                            usernamePublic = txtUsernameLogin.Text;

                            NoProfile opretProfil = new NoProfile();
                            opretProfil.Show();
                            this.Close();
                        }
                        else
                        {
                            usernamePublic = txtUsernameLogin.Text;
                            MainWindow dashboard = new MainWindow();
                            dashboard.Show();
                            this.Close(); // luk LoginScreen vindue  
                        }
                        

                                  
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // hvis beskeden hvis der er en fejl relateret til vores sql connection

                // Get stack trace for the exception with source file information
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();

                MessageBox.Show("Line: " + line.ToString());
            }
            finally
            {
                conn.Close(); // luk sql connection når den ikke længere bruges
            }
            #endregion
        }
      

    }
}
