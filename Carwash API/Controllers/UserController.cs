using Carwash_API.CRUD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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
    public class UserController : ControllerBase
    {
        private readonly UserCrud _users;
        private readonly LoginCrud _login;
        private readonly FinancialCrud _financial;

        public UserController(UserCrud users, LoginCrud login, FinancialCrud financial)
        {
            _users = users;
            _login = login;
            _financial = financial;
        }


        //Get all users
        [HttpGet]
        public ActionResult<List<UserModel>> Get() =>
           _users.Get().Result;

        [HttpGet("{username}")]
        public ActionResult Get(string username) =>
           Ok(_users.GetUser(username).Result);
        /*
        //Create
        [HttpPost]
        public ActionResult Create([FromBody] JsonElement jsUser)
        {
            try
            {
                var decryptedUser = JsonConvert.DeserializeObject<NewUserModel>(jsUser.GetRawText());
                if (_users.GetUser(decryptedUser.UserName).Result != null)
                    return BadRequest("UserName already exists!");

                if (_users.GetUserByEmail(decryptedUser.Email).Result != null)
                    return BadRequest("Email already exists!");


                var hash = HashSalt(decryptedUser.Password, null);

                //add encryption of all data, GDPR

                Crypt crypt = new Crypt();
                var FirstName    = Task.Run(() => crypt.Encrypter(decryptedUser.FirstName, hash.Salt));
                var LastName     = Task.Run(() => crypt.Encrypter(decryptedUser.LastName,  hash.Salt));
                var UserType     = Task.Run(() => crypt.Encrypter(decryptedUser.UserType,  hash.Salt));
                var Number_Plate = Task.Run(() => crypt.Encrypter(decryptedUser.Number_Plate, hash.Salt));
                var Subscription = Task.Run(() => crypt.Encrypter(decryptedUser.Subscription.ToString(), hash.Salt));
                var Transactions = Task.Run(() => crypt.Encrypter(string.Join(",", new List<Transactions>()), hash.Salt));
                var TotalAmount  = Task.Run(() => crypt.Encrypter(0.ToString(), hash.Salt));
                Task.WaitAll();



                var newUser = new UserModel
                {
                    UserName    = decryptedUser.UserName,
                    Email       = decryptedUser.Email,
                    FirstName   = FirstName.Result,
                    LastName    = LastName.Result,
                    UserType    = UserType.Result
                };


                _users.Create(newUser).Wait();

                var id = _users.GetUserByEmail(newUser.Email).Result.Id;
                var login = new LoginModel
                {
                    Id       = id,
                    UserName = decryptedUser.UserName,
                    Email    = decryptedUser.Email,
                    Password = hash.Pass,
                    Salt     = hash.Salt
                };

                var financial = new EncFinancialModel
                {
                    Id = id,
                    Number_Plate = Number_Plate.Result,
                    Subscription = Subscription.Result,
                    Transactions = Transactions.Result,
                    TotalAmount = TotalAmount.Result
                };

                _login.Create(login).Wait();
                _financial.Create(financial).Wait();

                return Ok(true);
            }
            catch
            {
                //  return BadRequest();
                throw new Exception("Error code 3.2 - UserController Post Create user error");
            }
        }
        */
        [HttpPost]
        public ActionResult Create([FromBody] JsonElement json)
        {
            try
            {

                var EncContent = JsonConvert.DeserializeObject<ApiModel>(json.GetRawText());
                if (EncContent.TokenId != "1666723Dx")
                    return Ok("Wrong Token");

                Crypt crypt = new Crypt();
                var DecContent = Task.Run(() => crypt.Decrypter(EncContent.Json, "13334448853")).Result;


                var decryptedUser = JsonConvert.DeserializeObject<NewUserModel>(DecContent);
                if (_users.GetUser(decryptedUser.UserName).Result != null)
                    return BadRequest("UserName already exists!");

                if (_users.GetUserByEmail(decryptedUser.Email).Result != null)
                    return BadRequest("Email already exists!");


                var hash = HashSalt(decryptedUser.Password, null);

                //add encryption of all data, GDPR

                var FirstName    = Task.Run(() => crypt.Encrypter(decryptedUser.FirstName, hash.Salt));
                var LastName     = Task.Run(() => crypt.Encrypter(decryptedUser.LastName,  hash.Salt));
                var UserType     = Task.Run(() => crypt.Encrypter(decryptedUser.UserType,  hash.Salt));
                var Number_Plate = Task.Run(() => crypt.Encrypter(decryptedUser.Number_Plate, hash.Salt));
                var Subscription = Task.Run(() => crypt.Encrypter(decryptedUser.Subscription.ToString(), hash.Salt));
                var Transactions = Task.Run(() => crypt.Encrypter(string.Join(",", new List<Transactions>()), hash.Salt));
                var TotalAmount  = Task.Run(() => crypt.Encrypter(0.ToString(), hash.Salt));
                Task.WaitAll();

                var newUser = new UserModel
                {
                    UserName    = decryptedUser.UserName,
                    Email       = decryptedUser.Email,
                    FirstName   = FirstName.Result,
                    LastName    = LastName.Result,
                    UserType    = UserType.Result
                };

                _users.Create(newUser).Wait();

                var id = _users.GetUserByEmail(newUser.Email).Result.Id;
                var login = new LoginModel
                {
                    Id       = id,
                    UserName = decryptedUser.UserName,
                    Email    = decryptedUser.Email,
                    Password = hash.Pass,
                    Salt     = hash.Salt
                };

                var financial = new EncFinancialModel
                {
                    Id = id,
                    Number_Plate = Number_Plate.Result,
                    Subscription = Subscription.Result,
                    Transactions = Transactions.Result,
                    TotalAmount = TotalAmount.Result
                };

                _login.Create(login);
                _financial.Create(financial);
                Task.WaitAll();

                return Ok(true);
            }
            catch
            {
                //  return BadRequest();
                throw new Exception("Error code 3.2 - UserController Post Create user error");
            }
        }

        [HttpPut]
        public ActionResult Update(string username, JsonElement jsUser)
        {
            try
            {
                var UserIn = JsonConvert.DeserializeObject<UserModel>(jsUser.GetRawText());

                if (_users.GetUser(username).Result == null)
                    return NotFound();
                _users.Update(username, UserIn);
                return NoContent();
            }
            catch (Exception e)
            {
                throw new Exception($"Error code 3.3 - UserController Put Update user error - {e.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(ObjectId id)
        {
            //Checks if user exists
            if (_users.GetUserById(id).Result == null)
                return NotFound();
            Task.Run(() => _users.Delete(id));
            Task.Run(() => _login.Delete(id));
            return NoContent();
        }
    }
}

