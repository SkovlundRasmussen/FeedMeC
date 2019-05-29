using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FeedMe.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("user_id")]
        public string UserId { get; set; }

        [BsonElement("restaurant_id")]
        public string RestaurantId { get; set; }

        [BsonElement("order_status")]
        public string Status { get; set; }

        [BsonElement("items")]
        public BsonDocument[] Items { get; set; }
    }
}