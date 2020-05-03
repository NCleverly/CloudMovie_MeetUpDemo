using CloudMovie.Web.Services;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace CloudMovie.Web.Helper
{
    public class JWTHelper
    {
		
		IKeyVaultService _kvService;
		IConfiguration _config;
		
		public JWTHelper (IKeyVaultService keyVault, IConfiguration config)
		{
			_kvService = keyVault;
			_config = config;
		}

		public async Task<string> GenerateToken(AuthenticateResult auth)
		{
			
			var mySecret = await _kvService.GetStringSecret(_config["KeyVault:JWTKey"]);
			var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
			var myIssuer = _config["JwtAuthentication:ValidAudience"];
			var myAudience = _config["JwtAuthentication:ValidIssuer"];


			var tokenHandler = new JwtSecurityTokenHandler();

			var subject = auth.Principal.Identities.FirstOrDefault();
			subject.AddClaim(new Claim("UserDBId", "123456", ClaimValueTypes.Integer));
			subject.AddClaim(new Claim("", "", ClaimValueTypes.String));

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = subject,
				Expires = DateTime.UtcNow.AddDays(7),
				Issuer = myIssuer,
				Audience = myAudience,
				SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public async Task<bool> ValidateCurrentToken(string token)
		{
			var mySecret = await _kvService.GetStringSecret(_config["KeyVault:JWTKey"]);
			var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
			var myIssuer = _config["JwtAuthentication:ValidAudience"];
			var myAudience = _config["JwtAuthentication:ValidIssuer"];

			var tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidIssuer = myIssuer,
					ValidAudience = myAudience,
					IssuerSigningKey = mySecurityKey
				}, out SecurityToken validatedToken);
			}
			catch
			{
				return false;
			}
			return true;
		}

		public string GetClaim(string token, string claimType)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

			var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
			return stringClaimValue;
		}
	}
}
