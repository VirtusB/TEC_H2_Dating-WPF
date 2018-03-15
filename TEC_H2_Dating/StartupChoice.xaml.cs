using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Windows;
using System.IO;
using System.Data.SqlClient;


namespace TEC_H2_Dating
{
    /// <summary>
    /// Interaction logic for StartupChoice.xaml
    /// </summary>
    public partial class StartupChoice : Window
    {
        public StartupChoice()
        {
            InitializeComponent();
        }

        private void startupLoginChoice_Click(object sender, RoutedEventArgs e)
        {
            LoginScreen loginScreen = new LoginScreen();
            loginScreen.Show();
            this.Close();
        }

        private void startupRegisterChoice_Click(object sender, RoutedEventArgs e)
        {
            RegisterScreen registerScreen = new RegisterScreen();
            registerScreen.Show();
            this.Close();
        }

        private void btnRunDBscript_Click(object sender, RoutedEventArgs e)
        {
            Load_Script(sender, e);
        }


        public void Load_Script(object sender, RoutedEventArgs e)
        {
            string sqlConnectionString = @"Data Source=localhost; Initial Catalog=master; Integrated Security=True;";

            string script = File.ReadAllText(@".\..\..\DBScript\TEC_H2_Dating.sql");

            SqlConnection conn = new SqlConnection(sqlConnectionString);

            Server server = new Server(new ServerConnection(conn));

            int scriptCheck = server.ConnectionContext.ExecuteNonQuery(script);

            try
            {
                if (scriptCheck != 0)
                {
                    MessageBox.Show("Scriptet blev kørt");
                    MessageBox.Show("Rows affected: " + scriptCheck.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            


        }



    }
}
