using System;
using System.Collections.Generic;
using System.Text;

namespace CarwashFront.Models
{
    class ApiModel
    {
        public ApiModel(string json, string tokenId)
        {
            Json = json;
            TokenId = tokenId;
        }
        public string Json { get; set; }
        public string TokenId { get; set; }
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
    
}
