using Kalbe.TechnicalAM.DataAccess.Services;
using Kalbe.TechnicalAM.Domain.Models;
using Justin.EntityFramework.Controller;
using Justin.EntityFramework.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kalbe.TechnicalAM.Api.Controllers {

    [ApiController]
    [Authorize]
    [Route("/api/[controller]")]
    public class UserController : BaseController<User> {

        private readonly IUserService _userService;
        public UserController(IUserService userService) : base(userService) {
            _userService = userService;
        }

        public override async Task<IActionResult> Get() {
            return Ok(new string("Hello World!"));
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]User user) {

            var res = await _userService.Save(user, saveChanges: true);
            return Ok(res);

        }

        [AllowAnonymous]
        [HttpGet("Login")]
        public async Task<IActionResult> Login(string username, string password) {
        
            var data = await _userService.AuthenticateUser(username, password);
            if(data.IsSuccess == false) {
                return BadRequest(data);
            }
            return Ok(data);
        
        }

    }
}
