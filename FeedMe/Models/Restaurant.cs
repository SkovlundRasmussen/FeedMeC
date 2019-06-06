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

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("menu")]
        public BsonDocument[] menu { get; set; }

    //    public IEnumerable<SubMenuItem> subMenu {get; set;}

    }

    class SubMenuItem
    {
        string id { get; set; }
        string name { get; set; }
        double price { get; set; }
    }
}
