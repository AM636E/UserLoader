namespace UserLoader.WebApi.Authentication
{
    public class UnknownUserException : System.Exception
    {
        public UnknownUserException(UserAuthenticationModel model)
        {
            Name = model.Name;
        }

        public string Name { get; }
    }
}
