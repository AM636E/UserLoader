using LanguageExt;

using System.Collections.Generic;

using UserLoader.DbModel.Models;

namespace UserLoader.Operations
{
    public interface IUserReader
    {
        Try<IEnumerable<UserModel>> GetAllUsers();
    }
}
