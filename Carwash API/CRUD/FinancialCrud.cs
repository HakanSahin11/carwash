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
    public class FinancialCrud
    {
        //DB Initialazation, with input saved in launchSettings.json
        private readonly IMongoCollection<EncFinancialModel> _financial;
        public FinancialCrud(IDBElements settings)
        {
            try
            {
                var client = new MongoClient(settings.ConnectionString);
                var database = client.GetDatabase(settings.Database);
                _financial = database.GetCollection<EncFinancialModel>("Financial");
            }
            catch (Exception e)
            {
                throw new Exception($"Error Code 3.1 - Database connection establishment - {e.Message}");
            }
        }

        //Get All User Login info
        public async Task<List<EncFinancialModel>> Get() =>
           await Task.Run(() => _financial.Find(x => true).ToList());

        //Get a Specific user login by Id 
        public async Task<EncFinancialModel> GetLoginById(ObjectId id) =>
           await Task.Run(() => _financial.Find(x => x.Id == id).FirstOrDefault());

        //Add new user login to UserLogin DB
        public async void Create(EncFinancialModel content) =>
            await Task.Run(() => _financial.InsertOne(content));

        //Update exising Login
        public async Task Update(ObjectId id, EncFinancialModel updatedUser) =>
          await Task.Run(() => _financial.ReplaceOne(x => x.Id == id, updatedUser));

        //Delete existing Login
        public async Task Delete(ObjectId id) =>
           await Task.Run(() => _financial.DeleteOne(x => x.Id == id));
    }
}
