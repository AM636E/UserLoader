using System;

using Serializer = System.Text.Json.JsonSerializer;

namespace UserLoader.Common
{
    public class JsonSerializer : ISerializer
    {
        public T Deserialize<T>(string data)
        {
            if(data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return (T)Serializer.Deserialize(data, typeof(T));
        }

        public string Serialize<T>(T data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return Serializer.Serialize(data, typeof(T));
        }
    }
}
