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


        public String GenerateSHA256Hash(String input)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
            System.Security.Cryptography.SHA256Managed sha256hashstring = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sha256hashstring.ComputeHash(bytes);

            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }

        private void btnRegisterSubmit_Click(object sender, RoutedEventArgs e)
        {
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



            //String hashedPassword = GenerateSHA256Hash(txtPasswordRegister.Password);
            
        }

        
    }
}
