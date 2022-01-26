using CarwashFront.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

using static CarwashFront.Helper_Classes.APICall;

namespace CarwashFront
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var json = JsonConvert.SerializeObject( new LoginModel(txt_User_or_Email.Text, PW_Pass.Password));
            var response = SendRequest("POST", json, "Login");
            MessageBox.Show(response, "Login", MessageBoxButton.OK);
            if (response == "true")
            {
                new CarWashView().Show();
                this.Close();
            }
                
        }

        private void BtnSignup_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
