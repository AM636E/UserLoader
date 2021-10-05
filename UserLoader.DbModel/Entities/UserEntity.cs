using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using System;

using UserLoader.DbModel.Entities;

namespace UserLoader.DbModel
{
    [CollectionName("users")]
    public class UserEntity : AbstractEntity
    {
        public string Name { get; set; }

        [BsonDateTimeOptions(DateOnly = true, Kind = DateTimeKind.Utc, Representation = BsonType.DateTime)]
        public DateTime CreatedDate { get; set; }
    }
}
