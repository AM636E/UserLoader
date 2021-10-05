using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace UserLoader.DbModel.Entities
{
    public abstract class AbstractEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}