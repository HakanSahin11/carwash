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
    public class CarwashRunCrud
    {
        //DB Initialazation, with input saved in launchSettings.json
        private readonly IMongoCollection<CarwashRunModel> _CarwashRun;
        public CarwashRunCrud(IDBElements settings)
        {
            try
            {
                var client = new MongoClient(settings.ConnectionString);
                var database = client.GetDatabase(settings.Database);
                _CarwashRun = database.GetCollection<CarwashRunModel>("CarwashRun");
            }
            catch (Exception e)
            {
                throw new Exception($"Error Code 1.1 - Database connection establishment  - {e.Message}");
            }
        }

        //Get All User Carwash info
        public async Task<List<CarwashRunModel>> Get() =>
           await Task.Run(() => _CarwashRun.Find(x => true).ToList());

        //Get Specific user by username 
        public async Task<CarwashRunModel> GetCarwashByUsername(string username) =>
            await Task.Run(() => _CarwashRun.Find(x => x.UserName == username).FirstOrDefault());

        public async Task<CarwashRunModel> GetCarwashByNumber(int carwash) =>
            await Task.Run(() => _CarwashRun.Find(x => x.Carwash == carwash).FirstOrDefault());

        //Get a Specific user Carwash by Id 
        public async Task<CarwashRunModel> GetCarwashById(ObjectId id) =>
           await Task.Run(() => _CarwashRun.Find(x => x.Id == id).FirstOrDefault());

        //Add new Carwash 
        public async void Create(CarwashRunModel content) =>
            await Task.Run(() => _CarwashRun.InsertOne(content));

        //Update exising Carwash
        public async Task Update(int carwash, CarwashRunModel updatedCarwash) =>
           await Task.Run(() => _CarwashRun.ReplaceOne(x => x.Carwash == carwash, updatedCarwash));

        //Delete existing Carwash
        public async Task Delete(ObjectId id) =>
           await Task.Run(() => _CarwashRun.DeleteOne(x => x.Id == id));
    }
}
