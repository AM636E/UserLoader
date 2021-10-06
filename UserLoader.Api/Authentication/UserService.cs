using LanguageExt;
using static LanguageExt.Prelude;

using System.Collections.Generic;

namespace UserLoader.WebApi.Authentication
{
    public class UserService : IUserService
    {
        private readonly IEnumerable<UserAuthenticationModel> _users;
        private readonly IAuthenticationTokenProvider _tokenProvider;

        public UserService(IEnumerable<UserAuthenticationModel> users, IAuthenticationTokenProvider authenticationTokenProvider)
        {
            _users = users;
            _tokenProvider = authenticationTokenProvider;
        }

        public Either<UnknownUserException, string> GetUserToken(UserAuthenticationModel model) =>
            _users.Find(user => user.Name == model.Name && user.Password == model.Password)
                  .Match<Either<UnknownUserException, string>>(
                    model => Right(_tokenProvider.GenerateAccessToken(model.Name)),
                    () => Left(new UnknownUserException(model))
                  );
    }
}
