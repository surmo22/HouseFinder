using HouseFinderBackEnd.Data;
using HouseFinderBackEnd.Services.AuthService;
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
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Indicates a successful registration with a token in the response
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Indicates a failed registration due to invalid model state
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
        [ProducesResponseType(StatusCodes.Status200OK)] // Indicates a successful login with a token in the response
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Indicates a failed login due to invalid model state
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Indicates a failed login due to invalid credentials
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
