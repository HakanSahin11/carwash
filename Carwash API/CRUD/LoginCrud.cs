using Carwash_API.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Carwash_API.Models.UserModels;

namespace Carwash_API.CRUD
{
    public class LoginCrud
    {
        //DB Initialazation, with input saved in launchSettings.json
        private readonly IMongoCollection<LoginModel> _login;
        public LoginCrud(IDBElements settings)
        {
            try
            {
                var client = new MongoClient(settings.ConnectionString);
                var database = client.GetDatabase(settings.Database);
                _login = database.GetCollection<LoginModel>("Login");
            }
            catch (Exception e)
            {
                throw new Exception($"Error Code 1.1 - Database connection establishment  - {e.Message}");
            }
        }

        //Get All User Login info
        public async Task<List<LoginModel>> Get() =>
           await Task.Run(() => _login.Find(x => true).ToList());


        //Get Specific user by email (for login)
        public async Task<LoginModel> GetUser(string Email) =>
            await Task.Run(() => _login.Find(x => x.Email == Email).FirstOrDefault());


        //Get Specific user by username (for login)
        public async Task<LoginModel> GetUserByUserName(string userName) =>
            await Task.Run(() => _login.Find(x => x.UserName == userName).FirstOrDefault());

        //Get a Specific user login by Id 
        public async Task<LoginModel> GetLoginById(ObjectId id) =>
           await Task.Run(() => _login.Find(x => x.Id == id).FirstOrDefault());

        //Add new user login to UserLogin DB
        public async void Create(LoginModel content) =>
            await Task.Run(() => _login.InsertOne(content));

        //Update exising Login
        public async Task Update(string username, LoginModel updatedUser) =>
           await Task.Run(() => _login.ReplaceOne(x => x.UserName == username, updatedUser));

        //Delete existing Login
        public async Task Delete(ObjectId id) =>
           await Task.Run(() => _login.DeleteOne(x => x.Id == id));
    }
}
