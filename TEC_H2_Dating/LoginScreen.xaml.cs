using System;
using System.Collections.Generic;
using System.Data;
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

        #region Tjek username og password
        /// <summary>
        /// Når der klikkes på btnLoginSubmit, prøv at logge ind.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoginSubmit_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=localhost; Initial Catalog=TEC_H2_Dating; Integrated Security=True;");

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();

                    SqlCommand verifyLogin = new SqlCommand("SELECT COUNT(*) FROM Users WHERE userpassword = @uPass AND username = @uName ", conn);

                    verifyLogin.Parameters.Add(new SqlParameter("@uPass", txtPasswordLogin.Password));
                    verifyLogin.Parameters.Add(new SqlParameter("@uName", txtUsernameLogin.Text));



                    int infoCorrect = (int)verifyLogin.ExecuteScalar();

                    if (infoCorrect != 1) // vis en besked hvis de indtaste informationer er forkerte
                    {                      
                        MessageBox.Show("Username or password is incorrect");
                    }
                    else
                    {
                        MainWindow dashboard = new MainWindow();
                        dashboard.Show();
                        this.Close(); // luk LoginScreen vindue
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // hvis beskeden hvis der er en fejl relateret til vores sql connection
            }
            finally
            {
                conn.Close(); // luk sql connection når den ikke længere bruges
            }
            #endregion
        }

        private void userAuth(object sender, RoutedEventArgs e)
        {

        }

    }
}
