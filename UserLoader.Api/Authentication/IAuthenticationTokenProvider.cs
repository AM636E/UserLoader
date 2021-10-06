namespace UserLoader.WebApi.Authentication
{
    public interface IAuthenticationTokenProvider
    {
        string GenerateAccessToken(string name);
    }
}