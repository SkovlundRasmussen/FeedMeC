using System;
using MongoDB.Bson.Serialization.Attributes;

namespace FeedMe.Models
{
    public class Restaurant
    {
        [BsonId]
        public int Id { get; set; }

        [BsonElement]
        public string name { get; set; }
    }
}
