using Application.DTOs;
using Application.Interfaces.Application.Contracts.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
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
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse<AuthResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                // Return a more detailed error response from ModelState
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ServiceResponse<AuthResponse>(false, "Validation failed", errors));
            }
            var result = await _authService.RegisterAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result.Data); // Return just the AuthResponse data on success
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse<AuthResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ServiceResponse<AuthResponse>(false, "Validation failed", errors));
            }
            var result = await _authService.LoginAsync(request);
            if (!result.Success)
            {
                // For login, it's common to return Unauthorized or a generic bad request
                // to avoid revealing whether an email exists.
                return Unauthorized(new ServiceResponse<AuthResponse>(false, result.Message));
            }
            return Ok(result.Data); // Return just the AuthResponse data on success
        }
    }
}
