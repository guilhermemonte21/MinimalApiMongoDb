﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MinimalAPIMongo.Domains
{
    public class Product
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("Price")]
        public decimal Price { get; set; }

        public Dictionary<string,string> AdditionalAttributes { get; set; }

        public Product()
        {
            AdditionalAttributes = new Dictionary<string,string>();
        }
    }
}
