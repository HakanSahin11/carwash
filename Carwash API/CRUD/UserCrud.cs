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
    public class UserCrud
    {
        //DB Initialazation, with input saved in launchSettings.json
        private readonly IMongoCollection<UserModel> _users;
        public UserCrud(IDBElements settings)
        {
            try
            {
                var client = new MongoClient(settings.ConnectionString);
                var database = client.GetDatabase(settings.Database);
                _users = database.GetCollection<UserModel>("User");
            }
            catch (Exception e)
            {
                throw new Exception($"Error Code 4.1 - Database connection establishment  - {e.Message}");

            }
        }

        //Get all users 
        public async Task<List<UserModel>> Get() =>
            await Task.Run(() =>
            _users.Find(x => true).ToList());

        //Get Specific user by Username (for loging mainly)
        public async Task<UserModel> GetUser(string username) =>
          await Task.Run(() =>
          _users.Find(x => x.UserName == username).FirstOrDefault());

        //Get Specific user by Email (for loging mainly)
        public async Task<UserModel> GetUserByEmail(string email) =>
          await Task.Run(() =>
          _users.Find(x => x.Email == email).FirstOrDefault());


        //Get specific user by Id
        public async Task<UserModel> GetUserById(ObjectId id) =>
           await Task.Run(() =>
           _users.Find(x => x.Id == id).FirstOrDefault());

        //Create new user
        public async Task Create(UserModel user) =>
            await Task.Run(() =>  _users.InsertOne(user));

        //Update existing user
        public async void Update(string username, UserModel updatedUser) =>
           await Task.Run(() =>
           _users.ReplaceOne(x => x.UserName == username, updatedUser));

        //Delete existing user
        public async void Delete(ObjectId id) =>
           await Task.Run(() =>
           _users.DeleteOne(x => x.Id == id));
    }
}
