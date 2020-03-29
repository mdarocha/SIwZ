using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Server.Models
{
    public class TrainStop
    {
        [BsonId]
        [BsonElement("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("city")]
        [BsonRepresentation(BsonType.String)]
        public string City { get; set; }

        [BsonElement("name")]
        [BsonRepresentation((BsonType.String))]
        public string Name { get; set; }
    }
}