using CarwashFront.Helper_Classes;
using CarwashFront.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    /// Interaction logic for CarWashView.xaml
    /// </summary>
    public partial class CarWashView : Window
    {

        public CarWashView()
        {
            InitializeComponent();

            Task.Run(() => PanelUpdate());
        }

        private void PanelUpdate()
        {
            GetIncomingRequests();
        }

        private void GetIncomingRequests()
        {
            var Carwashes = GetRequest("CarwashRun");
            //Creates Carwashes on new devices
            if (Carwashes.Count == 0)
            {
                TaskSetup_Create(1);
                TaskSetup_Create(2);
                TaskSetup_Create(3);
                TaskSetup_Create(4);
                Carwashes = GetRequest("CarwashRun");
            }

            
            var task = SendRequest("GET", null, "User", null);
            var MakeTestUsers = JsonConvert.DeserializeObject<List<UserModel>>(task);
            if (MakeTestUsers.Count < 4)
            {
                var user = new NewUserModel("TestMail@test.com", "TestUser", "TestFirst", "TestLast", "TestPass", "User", "DD55575", "true");
                SendRequest("POST", JsonConvert.SerializeObject(user), "User", "");

                user.Email = "TestMail2@Test.com";
                user.UserName = "TestUser2";
                SendRequest("POST", JsonConvert.SerializeObject(user), "User", "");

                user.Subscription = "false";
                user.Email = "TestMail3@Test.com";
                user.UserName = "TestUser3";
                SendRequest("POST", JsonConvert.SerializeObject(user), "User", "");

            }

            foreach (var item in Carwashes)
            {
                UpdatePanels( item.Carwash, item.Status, item.Numberplate, item.StartTime);
            }

            DateTime requestTime = DateTime.Now.AddSeconds(10);
            while (true)
            {
                if(requestTime <= DateTime.Now)
                {
                    var username = "TestMail@test.com";
                    var numberplate = "DD552243";
                    var getSubscription = SendRequest("POST", JsonConvert.SerializeObject(new CarwashRunModel {  UserName = username}), "Financial", "GetIncomingRequests");
                    var result = "no subscribtion";
                    if (getSubscription == "true")
                        result = "subscription";
                    var dialogbox = MessageBox.Show($"User {username} with numberplate '{numberplate}' requests to start a carwash with {result}\nWould you like to add the user into the queue", "Alert", MessageBoxButton.YesNo);
                    if(dialogbox == MessageBoxResult.Yes)
                    {
                        //add to queue with apicall
                        SendRequest("POST", JsonConvert.SerializeObject(new CarwashRunModel {  UserName = username, Numberplate = numberplate}), "Financial", "UserBoughtCarwashTime");
                    }
                    Carwashes = GetRequest("CarwashRun");
                    foreach (var item in Carwashes)
                    {
                        UpdatePanels(item.Carwash, item.Status, item.Numberplate, item.StartTime);
                    }
                    requestTime = DateTime.Now.AddSeconds(10);
                }
            }
        }
        
        private List<CarwashRunModel> GetRequest(string api)
        {
            var task = SendRequest("GET", null, api, null);
            return JsonConvert.DeserializeObject<List<CarwashRunModel>>(task);
        }

        private void UpdatePanels(int carwash, string status, string numberplate, string datetimeAdded)
        {
            this.Dispatcher.Invoke(() =>
            {

                var lb = (Label)FindName($"LB_Car_Wash_Status_{carwash}");
                lb.Content = status;

                var lb2 = (Label)FindName($"Lb_Carwash_CurrentCar_{carwash}");
                lb2.Content = numberplate;

                var lb3 = (Label)FindName($"Lb_Carwash_TimeAdded_{carwash}");
                lb3.Content = datetimeAdded;
            });
        }

        private void Btn_Carwash_Start_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var currenCarwash = Convert.ToInt32(string.Concat(btn.Name.Where(char.IsDigit)));
            CarwashMainSetup(currenCarwash, "DD12312", "TestUser", "Running", DateTime.Now.ToString());
        }

         private void CarwashMainSetup(int currentCarwash, string numberplate, string username, string status, string startTime)
        {
            var msg = JsonConvert.SerializeObject(
                new CarwashRunModel() { Status = status, Numberplate = numberplate, UserName = username, StartTime = startTime.ToString(), Carwash = currentCarwash }) ;
            var request = SendRequest("POST", msg, "CarwashRun", "Change");
            var response = JsonConvert.DeserializeObject< ApiModel>( request);

            if(response.TokenId != "1666723Dx")
            {
                MessageBox.Show("Error Token");
                return;
            }

            var lb = (Label)FindName($"LB_Car_Wash_Status_{currentCarwash}");
            lb.Content = response.Json;

            var lb2 = (Label)FindName($"Lb_Carwash_CurrentCar_{currentCarwash}");
            lb2.Content = numberplate;

            var lb3 = (Label)FindName($"Lb_Carwash_TimeAdded_{currentCarwash}");
            lb3.Content = startTime;

            MessageBox.Show($"Carwash{currentCarwash} Successfully {response.Json}!");
        }

        private void TaskSetup_Create(int Currentcarwash)
        {
            var msg = JsonConvert.SerializeObject(
                new CarwashRunModel { Status = "Closed", StartTime = DateTime.Now.ToString(), Carwash = Currentcarwash });
            SendRequest("POST", msg, "CarwashRun", "Create");
        }

        private void Btn_Carwash_Stop_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var currenCarwash = Convert.ToInt32(string.Concat(btn.Name.Where(char.IsDigit)));
            CarwashMainSetup(currenCarwash, "", "", "Closed", "-");
        }
    }
}
