using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Minedu.AprendoEnCasaOffLine.Contenido.Core;
using Minedu.IS4.Security.Auth;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Api.Controllers
{
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        public AccountController(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }
        [HttpPost]
        [Route("token")]
        [AllowAnonymous]
        public TokenResponse Token()
        {
            var tr = new TokenResponse();
            var ts = _appSettings.Token;

            //Encoding.ASCII.GetBytes("username:password1234");

            if (!Request.Headers.ContainsKey("Authorization"))
            {
                tr.error = "validation";
                tr.error_description = "Authorization Header is required";
                return tr;

            }
            if (!Request.Headers.ContainsKey("client_id"))
            {
                tr.error = "validation";
                tr.error_description = "client_id Header is required";
                return tr;

            }
            else
            {

                string client_id = Request.Headers["client_id"];
                if (string.IsNullOrWhiteSpace(client_id) || (ts.TOKEN_CLIENT_ID.ToUpper() != client_id.ToUpper()))
                {
                    tr.error = "validation";
                    tr.error_description = "client_id is not valid";
                    return tr;
                }

            }
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            if (authHeader.Scheme != "Basic")
            {
                tr.error = "validation";
                tr.error_description = "Authorization Basic Header is required";
                return tr;

            }
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
            var username = credentials[0];
            var password = credentials[1];

            //validate user and password
            if (!(ts.TOKEN_USER == username && ts.TOKEN_PASSWORD == password))
            {
                tr.error = "validation";
                tr.error_description = "The username or password is incorrect";
                return tr;
            }

            //Prepare token
            int expiresDays = ts.TOKEN_EXPIRES_DAYS;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ts.TOKEN_SECRET);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, ts.TOKEN_USER)
                }),

                Expires = DateTime.Now.AddDays(expiresDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            tr.token_type = "bearer";
            tr.access_token = tokenString;
            var timeSpan = tokenDescriptor.Expires.Value - DateTime.Now;

            tr.expires_in = Convert.ToInt32(timeSpan.TotalDays);

            return tr;
        }

    }
}
