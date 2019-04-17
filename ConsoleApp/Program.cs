using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ConsoleApp
{
    class Program
    {
        private static string connectionString;
        private static IMongoDatabase database;
        static void Main(string[] args)
        {
            connectionString = ConfigurationManager.ConnectionStrings["Mongo"].ConnectionString;
            database = new MongoClient(connectionString).GetDatabase("BookstoreDb");
            //db = db.getSiblingDB('BookstoreDb');
            var script = "  db.Books.find({_id:ObjectId('5c9f11ab2ade1053fc1ab7fb')})";
            var doc = new BsonDocument()
            {
                { "eval", script }
            };
            var command = new BsonDocumentCommand<BsonDocument>(doc);

            var response = database.RunCommand(command);

            var ele = response.Values;
            var vlaue = (IEnumerable<BsonValue>)ele;
            Console.WriteLine(vlaue);

            Console.WriteLine("Hello World!");
        }
    }
}
