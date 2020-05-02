using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CloudMovie.Web.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CloudMovie.Web
{
    [Route("mobileauth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        const string callbackScheme = "cloudmovie.mobile";
        JWTHelper _helper;

        public AuthController(JWTHelper helper)
        {
            _helper = helper;
        }

        [HttpGet("{scheme}")]
        public async Task Get([FromRoute]string scheme)
        {
            AuthenticateResult auth = await Request.HttpContext.AuthenticateAsync(scheme);

            if (!auth.Succeeded
                || auth?.Principal == null
                || !auth.Principal.Identities.Any(id => id.IsAuthenticated)
                || string.IsNullOrEmpty(auth.Properties.GetTokenValue("access_token")))
            {
                // Not authenticated, challenge
                await Request.HttpContext.ChallengeAsync(scheme);
            }
            else
            {
                // Get parameters to send back to the callback
                var qs = new Dictionary<string, string>
            {
                { "access_token", await _helper.GenerateToken(auth) },
                { "refresh_token", auth.Properties.GetTokenValue("refresh_token") ?? string.Empty },
                { "expires", (auth.Properties.ExpiresUtc?.ToUnixTimeSeconds() ?? -1).ToString() }
            };
                // Build the result url
                var url = callbackScheme + "://#" + string.Join(
                    "&",
                    qs.Where(kvp => !string.IsNullOrEmpty(kvp.Value) && kvp.Value != "-1")
                    .Select(kvp => $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}"));

                // Redirect to final url
                Request.HttpContext.Response.Redirect(url);
            }
        }

        [HttpPost("postlogin")]
        public async Task Post([FromBody]string loginDetails)
        {
            //var qs = new Dictionary<string, string>
            //{
            //    { "access_token", auth.Properties.GetTokenValue("access_token") },
            //    { "refresh_token", auth.Properties.GetTokenValue("refresh_token") ?? string.Empty },
            //    { "expires", (auth.Properties.ExpiresUtc?.ToUnixTimeSeconds() ?? -1).ToString() }
            //};

            //// Build the result url
            //var url = callbackScheme + "://#" + string.Join(
            //    "&",
            //    qs.Where(kvp => !string.IsNullOrEmpty(kvp.Value) && kvp.Value != "-1")
            //    .Select(kvp => $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}"));

            //// Redirect to final url
            //Request.HttpContext.Response.Redirect(url);
        }
    }
}
