using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;

using UserLoader.DbModel.Models;
using UserLoader.Operations;

namespace UserLoader.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserReader _userOperations;
        private readonly IUserWriter _writer;

        public UserController(IUserReader userOperations, IUserWriter writer)
        {
            _userOperations = userOperations;
            _writer = writer;
        }

        // GET: api/<UserController>
        [HttpGet]
        public ActionResult<IEnumerable<UserModel>> Get()
        {
            return _userOperations.GetAllUsers().Match(Ok, ex => StatusCode(500, ex));
        }

        // POST api/<UserController>
        [HttpPost]
        public ActionResult Post([FromBody] UserModel value)
        {
            return _writer.Insert(value).Match(m => Ok(value), ex => StatusCode(500, ex));
        }
    }
}
