using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly ITokenRepository tokenRepository;

		public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
			this.userManager = userManager;
			this.tokenRepository = tokenRepository;
		}

        // POST /api/auth/register
        [HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
		{
			var user = new IdentityUser { UserName = registerRequestDTO.Username,Email=registerRequestDTO.Username };
			var result = await userManager.CreateAsync(user, registerRequestDTO.Password);

			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}
			// Add roles to user
			if(registerRequestDTO.Roles!=null)
			{
				foreach (var role in registerRequestDTO.Roles)
				{
					result = await userManager.AddToRoleAsync(user, role);
					if (!result.Succeeded)
					{
						return BadRequest(result.Errors);
					}
				}
			}
			return Ok("User was registered successfully. Please login!");
		}

		// POST /api/auth/login
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
		{
			var user = await userManager.FindByEmailAsync(loginRequestDTO.Username);
			if (user == null)
			{
				return BadRequest("Incorrect Username and/or Password");
			}
			var result = await userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
			if (!result)
			{
				return BadRequest("Incorrect Username and/or Password");
			}
			// Get roles
			var roles = await userManager.GetRolesAsync(user);
			if (roles == null)
			{
				return BadRequest("User has no roles");
			}

			// Create Token
			var token = tokenRepository.CreateJwtToken(user, roles.ToList());

			var response = new LoginResponseDTO
			{
				JwtToken = token
			};

			return Ok(response);
		}
	}
}
