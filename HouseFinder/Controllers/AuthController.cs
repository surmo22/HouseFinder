using HouseFinderBackEnd.Data;
using HouseFinderBackEnd.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HouseFinderBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var token = await _authService.Register(model);
                return Ok(new { Token = token });
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new { message = "User creation failed" });
            }
           
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var token = await _authService.Login(model);
                return Ok(new { Token = token });
            }
            catch (InvalidOperationException)
            {
                return Unauthorized();
            }
        }
    }
}
