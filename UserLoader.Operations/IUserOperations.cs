using LanguageExt;

using System.Collections.Generic;

using UserLoader.DbModel.Models;

namespace UserLoader.Operations
{
    public interface IUserOperations
    {
        Try<IEnumerable<UserModel>> GetAllUsers();

        Try<Unit> Insert(UserModel model);
    }
}
