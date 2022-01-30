using System;
using System.Collections.Generic;
using System.Text;

namespace CarwashFront.Models
{
    public class ApiModel
    {
        public ApiModel(string json, string tokenId, string task)
        {
            Json = json;
            TokenId = tokenId;
            Task = task;
        }
        public string Json { get; set; }
        public string TokenId { get; set; }
        public string Task { get; set; }
    }
    public class LoginModel 
    {
        public LoginModel(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class NewUserModel
    {
        public NewUserModel(string email, string userName, string firstName, string lastName, string password, string userType, string number_Plate, string subscription)
        {
            Email = email;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            UserType = userType;
            Number_Plate = number_Plate;
            Subscription = subscription;
        }

        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string Number_Plate { get; set; }
        public string Subscription { get; set; }
    }

    public class UserModel 
    {
        public string UserType { get; set; }
        public string Email { get; set; }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
