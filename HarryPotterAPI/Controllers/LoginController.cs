using HarryPotterAPI.AuthorizationAndAuthentication;
using HarryPotterAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HarryPotterAPI.Controllers
{    
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly GenerateToken _generateToken;

        public LoginController(IConfiguration configuration, GenerateToken generateToken)
        {            
            _configuration = configuration;
            _generateToken = generateToken;
        }

        [HttpPost]
        public async Task<ActionResult<Character>> Login([FromBody] Authenticate user)
        {
            var username = _configuration["UserAuthentication:login"];
            var password = _configuration["UserAuthentication:senha"];
            //var username = _configuration.GetSection("UserAuthentication").GetSection("login").Value;
            //var password = _configuration.GetSection("UserAuthentication").GetSection("senha").Value;

            if (username != user.Username || password != user.Password)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var validUser = new User() { Username = username, Password = "" };
            var token = _generateToken.GenerateJwt(validUser);

            return Ok(new { user = validUser, token = token });
        }
    }
}
