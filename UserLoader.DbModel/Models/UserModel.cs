using System;

namespace UserLoader.DbModel.Models
{
    [Serializable]
    public class UserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
