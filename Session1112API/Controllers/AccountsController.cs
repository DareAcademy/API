using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Session1112API.Models;
using Session1112API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Session1112API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly IAccountService accountService;
		private readonly IConfiguration configuration;

		public AccountsController(IAccountService _accountService,IConfiguration _configuration)
		{
			accountService = _accountService;
			configuration = _configuration;
		}

		[HttpPost]
		[Route("SignUP")]
		public async Task<IActionResult> SignUP(SignUp signUp)
		{
			var result = await accountService.CreateAccount(signUp);

			if (result.Succeeded)
			{
				return Ok();
			}
			else
			{
				return StatusCode(500, result.Errors);
			}
		}

		[HttpPost]
		[Route("AddRole")]
		public async Task<IActionResult> AddRole(Role role)
		{
			var result = await accountService.AddRole(role);
			if (result.Succeeded)
			{
				return Ok();
			}
			else
			{
				return StatusCode(500, result.Errors);
			}
		}

		[HttpGet]
		[Route("UserList")]
		public IActionResult UserList()
		{
			List<UsersDTO> users = accountService.getUsers();
			return Ok(users);

		}

		[HttpGet]
		[Route("UserRoles")]
		public async Task<IActionResult> UserRoles(string UserId)
		{
		
			List<UserRoles> userRoles = await accountService.getRoles(UserId);
			return Ok(userRoles);
			
		}

		[HttpPost]
		[Route("UpdateRole")]
		public async Task<IActionResult> UpdateRole(List<UserRoles> userRoles)
		{
			await accountService.UpdateUserRoles(userRoles);
			userRoles = await accountService.getRoles(userRoles[0].UserId);

			return Ok(userRoles);
		}


		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login(SignIn signIn)
		{
			var result = await accountService.SignIn(signIn);
			if (result.Succeeded)
			{
				var authClaim = new List<Claim>
				{
					new Claim(ClaimTypes.Name,signIn.Username),
					new Claim("UniqueValue",Guid.NewGuid().ToString())
				};

				var user = await accountService.getUSerInfo(signIn.Username);

				var roles = accountService.getUserRoles(user);

				foreach (var item in roles)
				{
					authClaim.Add(new Claim(ClaimTypes.Role, item));
				}


				var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

				var token = new JwtSecurityToken(
							issuer: configuration["JWT:ValidIssuer"],
							audience: configuration["JWT:ValidAudience"],
							expires: DateTime.Now.AddDays(15),
							claims: authClaim,
							signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
							);

				return Ok(
						new
						{
							token = new JwtSecurityTokenHandler().WriteToken(token)
						});
			}
			else
			{
				return Unauthorized();
			}
		}

	}
}
