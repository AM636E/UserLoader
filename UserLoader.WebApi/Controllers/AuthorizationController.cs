using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using UserLoader.Operations;
using UserLoader.WebApi.Authentication;

namespace UserLoader.WebApi.Controllers
{
    [AllowAnonymous]
    [Route("api/authorization")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserWriter _userWriter;
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(IUserService userService, IUserWriter userWriter, ILogger<AuthorizationController> logger)
        {
            _userService = userService;
            _userWriter = userWriter;
            _logger = logger;
        }

        [HttpPost("token")]
        public ActionResult<string> GenerateToken(UserAuthenticationModel model) =>
            _userService.GetUserToken(model)
                .Match<ActionResult<string>>(token => Ok(token), _ => NotFound());


        [HttpGet("smash")]
        public string Smash()
        {
            _userWriter.Insert(new DbModel.Models.UserModel { Name = "test" })
                .Match(_ => _logger.LogInformation("added"), ex => _logger.LogError(ex.Message));
            return "";
        }
    }
}
