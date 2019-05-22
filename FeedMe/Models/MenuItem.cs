using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FeedMe.Models
{
    public class MenuItem
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement]
        public string name { get; set; }

        [BsonElement]
        public string type { get; set; }

        [BsonElement]
        public double price { get; set; }
    }
}
