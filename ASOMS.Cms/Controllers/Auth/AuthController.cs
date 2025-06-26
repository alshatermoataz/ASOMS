using ASOMS.Core.DTOs.Auth;
using ASOMS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASOMS.Cms.Controllers.Auth;

        [ApiController]
        [Route("api/[controller]")]
        public class AuthController(IAuthService _authService) : ControllerBase
        {

            [HttpPost("register")]
            public async Task<IActionResult> Register(RegisterRequest request)
            {
                try
                {
                    var result = await _authService.RegisterAsync(request);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login(LoginRequest request)
            {
                try
                {
                    var result = await _authService.LoginAsync(request);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return Unauthorized(new { message = ex.Message });
                }
            }
        }
    
    

