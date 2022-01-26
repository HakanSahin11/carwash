using Carwash_API.CRUD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static Carwash_API.Models.UserModels;
using static Carwash_API.Help_Classes.Salting;
using Carwash_API.Help_Classes;

namespace Carwash_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginCrud _login;
        public LoginController(LoginCrud login)
        {
            _login = login;
        }

        //Get list of all users
        [HttpGet]
        public ActionResult<List<LoginModel>> Get() =>
                _login.Get().Result;

        [HttpPost]
        public ActionResult LoginConfirm([FromBody] JsonElement LoginJson)
        {

            try
            {
                bool loginResult = false;

                //Without crypt
                var EncContent = JsonConvert.DeserializeObject<ApiModel>(LoginJson.GetRawText());
                if (EncContent.TokenId != "1666723Dx")
                    return BadRequest();

                Crypt crypt = new Crypt();
                var DecContent = crypt.Decrypter(EncContent.Json, "13334448853");
                var Content = JsonConvert.DeserializeObject<LoginModel>(DecContent);


                LoginModel SavedContent = _login.GetUserByUserName(Content.UserName).Result;
                if (SavedContent == null)
                    SavedContent = _login.GetUser(Content.UserName).Result;
                if (SavedContent == null)
                    return NotFound(loginResult);

                if (Content.UserName ==  SavedContent.Email || Content.UserName == SavedContent.UserName)
                {
                    if (HashSalt(Content.Password, Convert.FromBase64String(SavedContent.Salt)).Pass == SavedContent.Password)
                        loginResult = true;
                }
                return Ok(loginResult);
            }
            catch (Exception e)
            {
                throw new Exception($"Error Code 1.2 - Error at HTTPPOST - {e.Message}");
            }
        }
    }
}
