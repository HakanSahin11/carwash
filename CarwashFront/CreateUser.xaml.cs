using CarwashFront.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static CarwashFront.Helper_Classes.APICall;

namespace CarwashFront
{
    /// <summary>
    /// Interaction logic for CreateUser.xaml
    /// </summary>
    public partial class CreateUser : Window
    {
        public CreateUser()
        {
            InitializeComponent();
        }

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            var user = new NewUserModel(
                txt_Email.Text,
                txt_Username.Text,
                txt_Firstname.Text,
                txt_Lastname.Text,
                PW_Pass.Password,
                "Admin",
                txt_Numberplate.Text,
                "false"
                );
            SendRequest("POST", JsonConvert.SerializeObject(user), "User", "");
            new CarWashView().Show();
            this.Close();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
