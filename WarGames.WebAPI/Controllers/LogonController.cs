using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WarGames.WebAPI.Constants;
using WarGames.WebAPI.Models;

namespace WarGames.WebAPI.Controllers
{
	/*TODO: Move this to another web-api and demonstrate how
	 * you can have multiple APIs use the same Identity Provider
	 * to verify authentication and authorization using the same
	 * JWT token.
	 */

	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class LogonController : ControllerBase
	{
		private readonly IOptionsMonitor<AzureB2CConfig> providerDetails;
		private readonly IOptionsMonitor<ThisIsNotSecureExampleOnly> users;

		public LogonController
				(
					IOptionsMonitor<ThisIsNotSecureExampleOnly> users,
					IOptionsMonitor<AzureB2CConfig> providerDetails
				)
		{
			this.providerDetails = providerDetails;
			this.users = users;
		}

		[AllowAnonymous]
		[HttpGet("help")]
		public IActionResult Help()
		{
			return Ok("Here be dragons");
		}

		[AllowAnonymous]
		[HttpGet("games/list")]
		public IActionResult ListGames()
		{
			return Ok(Games.List);
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login(UserInfo userInfo)
		{
			if (VerifyUser(userInfo))
				return Ok(await BuildToken(userInfo));
			else
				return Unauthorized();
		}

		[AllowAnonymous]
		[HttpGet("providerDetails")]
		public IActionResult ProviderDetails()
		{
			return Ok(providerDetails.CurrentValue);
		}

		private async Task<string> BuildToken(UserInfo userInfo)
		{
			var claims = new[] {
						new Claim(JwtRegisteredClaimNames.Sub, users.CurrentValue.Jwt.Subject),
						new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
						new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
						new Claim("UserId", userInfo.UserId.ToString()),
						new Claim("DisplayName", userInfo.DisplayName ?? string.Empty),
						new Claim("UserName", userInfo.UserName?? string.Empty),
						new Claim("Email", userInfo.Email ?? string.Empty)
					};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(users.CurrentValue.Jwt.Key));
			var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(
				users.CurrentValue.Jwt.Issuer,
				users.CurrentValue.Jwt.Audience,
				claims,
				expires: DateTime.UtcNow.AddMinutes(10),
				signingCredentials: signIn);

			return await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(token));
		}

		private bool VerifyUser(UserInfo userInfo)
		{
			if (users.CurrentValue.Users.ContainsKey(userInfo.UserName ?? string.Empty))
				return users.CurrentValue.Users[userInfo.UserName ?? string.Empty] == userInfo.Password;
			return false;
		}
	}
}