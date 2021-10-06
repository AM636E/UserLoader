using LanguageExt;

using UserLoader.DbModel.Models;

namespace UserLoader.Operations
{
    public interface IUserWriter
    {
        Try<Unit> Insert(UserModel model);
    }
}
