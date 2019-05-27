using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace FeedMe.Models
{
    public class Restaurant
    {
        SubMenuItem[] pizzaMenus;
        SubMenuItem[] burgerMenus;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("menu")]
        public BsonDocument menu { get; set; }
    
        public void menuGenerator()
        {
            foreach (var item in menu)
            {
                if (item.Name.ToLower() == "pizzas")
                {
                    Console.WriteLine("IM HERRRERERERERE : " + item.Name);
                }
            }
        }
    }

    class SubMenuItem
    {
        string id { get; set; }
        string name { get; set; }
        double price { get; set; }
    }
}
