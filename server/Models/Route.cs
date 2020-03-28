using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Server.Models
{
    public class Route
    {
        [BsonId]
        [BsonElement("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("from")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string from { get; set; }
        
        [BsonElement("to")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string to { get; set; }
        
        [BsonElement("stops")]
        [BsonRepresentation(BsonType.Array)]
        public BsonArray stops { get; set; }
        
        [BsonElement("startDate")]
        [BsonRepresentation(BsonType.DateTime)]
        public BsonDateTime startDate { get; set; }
        
        [BsonElement("ticketsLeft")]
        [BsonRepresentation(BsonType.Int32)]
        public int ticketsLeft { get; set; }
        
    }
}