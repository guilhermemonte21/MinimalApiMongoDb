using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MinimalAPIMongo.Domains
{
    public class Order
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }


        [BsonElement("Date")]
        public DateTime Date { get; set; }

        [BsonElement("Status")]
        public string Status { get; set; }


        [BsonElement("productIds")]
        [BsonIgnore]
        public List<string>? ProductIds { get; set; }
        public List<Product>? Products { get; set; }

        [BsonElement("clientId")]
        public string ClientId { get; set; }
        public Client? Client { get; set; }
    }
}
