using LanguageExt;

namespace UserLoader.WebApi.Authentication
{
    public interface IUserService
    {
        Either<UnknownUserException, string> GetUserToken(UserAuthenticationModel username);
    }
}
