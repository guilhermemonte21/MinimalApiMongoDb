using MinimalAPIMongo.Domains;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace MinimalAPIMongo.ViewModel
{
    public class OrderViewModel
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }


        [BsonElement("Date")]
        public DateTime Date { get; set; }

        [BsonElement("Status")]
        public string Status { get; set; }


        [BsonElement("productIds")]
        public List<string>? ProductIds { get; set; }
        [BsonIgnore]
        [JsonIgnore]
        public List<Product>? Products { get; set; }

        [BsonElement("clientId")]
        public string? ClientId { get; set; }
        [BsonIgnore]
        [JsonIgnore]
        public Client? Client { get; set; }
    }
}
