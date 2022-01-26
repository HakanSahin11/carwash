using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carwash_API.Models
{
    public class UserModels
    {
        public class NewUserModel : ILoginModel, IUserModel, IEncFinancialModel
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public ObjectId Id { get; set; }
            public string Email { get; set; }
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Password { get; set; }
            public string UserType { get; set; }
            public string Number_Plate { get; set; }
            public string Salt { get; set; }
            public string Subscription { get; set; }
            public string Transactions { get; set; }
            public string TotalAmount { get; set; }


        }

        public class UserModel : IUserModel
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public ObjectId Id { get; set; }
            public string UserType { get; set; }
            public string Email { get; set; }

            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class EncFinancialModel : IEncFinancialModel
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public ObjectId Id { get; set; }
            public string Number_Plate { get; set; }
            public string Transactions { get; set; }
            public string TotalAmount { get; set; }
            public string Subscription { get; set; }
        }

        public interface IEncFinancialModel
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public ObjectId Id { get; set; }
            public string Number_Plate { get; set; }
            public string Transactions { get; set; }
            public string TotalAmount { get; set; }
            public string Subscription { get; set; }
        }
        public class DecFinancialModel : IDecFinancialModel
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public ObjectId Id { get; set; }
            public string Number_Plate { get; set; }
            public List<Transactions> Transactions { get; set; }
            public double TotalAmount { get; set; }
            public bool Subscription { get; set; }

        }
        public interface IDecFinancialModel
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public ObjectId Id { get; set; }
            public string Number_Plate { get; set; }
            public List<Transactions> Transactions { get; set; }
            public double TotalAmount { get; set; }
            public bool Subscription { get; set; }
        }

        public class Transactions
        {
            public DateTime Transaction_Datetime { get; set; }
            public double Amount { get; set; }
            public bool Subscription { get; set; }
        }

        public interface IUserModel
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            ObjectId Id { get; set; }
            string UserType { get; set; }
            string Email { get; set; }
            string UserName { get; set; }
            string FirstName { get; set; }
            string LastName { get; set; }
        }

        public class LoginModel : ILoginModel
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public ObjectId Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Salt { get; set; }

        }
        public interface ILoginModel
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            ObjectId Id { get; set; }
            string UserName { get; set; }
            string Email { get; set; }

            string Password { get; set; }
            string Salt { get; set; }
        }
        public class SaltModel
        {
            public SaltModel(ObjectId id, string salt, string saltPass, List<string> recoveryKeysSalt)
            {
                Id = id;
                Salt = salt;
                SaltPass = saltPass;
                RecoveryKeysSalt = recoveryKeysSalt;
            }

            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public ObjectId Id { get; set; }
            public string Salt { get; set; }
            public string SaltPass { get; set; }
            public List<string> RecoveryKeysSalt { get; set; }


            public class SaltHash
            {
                public SaltHash(string pass, string salt)
                {
                    Pass = pass;
                    Salt = salt;
                }
                public string Pass { get; set; }
                public string Salt { get; set; }
            }
        }
        public class ApiModel
        {
            public ApiModel(string json, string tokenId)
            {
                Json = json;
                TokenId = tokenId;
            }
            public string Json { get; set; }
            public string TokenId { get; set; }
        }
    }
}