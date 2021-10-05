using System;

namespace UserLoader.DbModel
{
    public class CollectionNameAttribute : Attribute
    {
        public string Name { get;  }

        public CollectionNameAttribute(string collectionName)
        {
            Name = collectionName;
        }
    }
}