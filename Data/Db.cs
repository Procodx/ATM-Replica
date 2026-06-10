using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using ATM_Replica.Data.model;
using ATM_Replica.Data_folder.model_folder;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Web.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ATM_Replica.Data_folder
{
    public class Db
    {


        // Connect to MongoDB
        private const string ConnectionString = "mongodb+srv://local_user:P4arbhECPMpaMgaZ@cluster0.mripw6q.mongodb.net/";
        public const string DataBaseName = "Atm_Replica_DB";
        private const string TransactionCollection = "Transaction_chart";
        private const string UserCollection = "Users";
        private const string TransactionHistoryCollection = "Transaction_history";


        private IMongoCollection<T> ConnectToMongo<T>(string collection)
        {
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DataBaseName);
            return db.GetCollection<T>(collection);
        }

        
        public User GetUser(string email)
        {
            var userCollection = ConnectToMongo<User>(UserCollection);
            var userGotten = userCollection.Find(c => c.Email == email);
            return userGotten.FirstOrDefaultAsync().Result;
           
        }

        public Task CreateUser(User user)
        {
            var usersCollection = ConnectToMongo<User>(UserCollection);
            return usersCollection.InsertOneAsync(user);
        }

        public Task UpdateUserInfo(User user)
        {
            var usersCollection = ConnectToMongo<User>(UserCollection);
            var filter = Builders<User>.Filter.Eq("Id", user.Id);
            return usersCollection.ReplaceOneAsync(filter, user);
        }

        public Task UpdateUserBalance(User user) 
        {
            var usersCollection = ConnectToMongo<User>(UserCollection);
            var filter = Builders<User>.Filter.Eq(u => u.Email, user.Email);
            var update = Builders<User>.Update.Set(u => u.Balance, user.Balance);
            return usersCollection.ReplaceOneAsync(filter, user);

        }

        public Task CreateTransaction(Transaction transaction)
        {
            var transCollection = ConnectToMongo<Transaction>(TransactionCollection);
            return transCollection.InsertOneAsync(transaction);
        }

        public async Task<List<Transaction>> GetListOfTransForAUser(User user)
        {
            var transactionsCollection = ConnectToMongo<Transaction>(TransactionCollection);
            var results = await transactionsCollection.FindAsync(c => c.Id == user.Id);
            return results.ToList();
        }







        

























        //public static List<User> Users { get; } = new List<User>
        //{
        //    new User("User", "One", "user1@email.com", "colombuspark", User.AccountTypeEnum.CURRENT, 10000.00M),
        //    new User("User", "Two", "user2@email.com", "colombuspa", User.AccountTypeEnum.SAVINGS, 500.00M),
        //    new User("User", "Three", "user3@email.com", "colombusp", User.AccountTypeEnum.CURRENT, 3000.00M),
        //    new User("User", "Four", "user4@email.com", "colombus", User.AccountTypeEnum.CREDIT, 600.00M),
        //    new User("User", "Five", "user5@email.com", "colombu" , User.AccountTypeEnum.SAVINGS, 8000.00M)
        //};

        //public static User? GetUser (string? email, ModelStateDictionary modelState)
        //{
        //    foreach (var user in Users)
        //    {
        //        if (user.Email == email)
        //        {
        //            return user;
        //        }

        //    }
        //    modelState.AddModelError("", "Such account does not exist!!");
        //    return null;
        //}
    }
}
