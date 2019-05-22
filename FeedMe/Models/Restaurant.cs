using System;
using MongoDB.Bson.Serialization.Attributes;

namespace FeedMe.Models
{
    public class Restaurant
    {
        [BsonId]
        private int Id { get; set; }

        [BsonElement]
        private string name { get; set; }
    }
}
