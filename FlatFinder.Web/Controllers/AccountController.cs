using FlatFinder.Contracts;
using FlatFinder.Contracts.Services;
using FlatFinder.Web.ActionFilters;
using FlatFinder.Web.Requests;
using FlatFinder.WebAPI.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FlatFinder.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [CustomExceptionFilter]
    [ValidateModel]
    public class AccountController : Controller
    {
        private readonly TokenValidationOptions _tokenValidationOptions;
        private readonly IUserService _userService;

        public AccountController(IUserService userService, IOptions<TokenValidationOptions> tokenValidationOptions)
        {
            _userService = userService;
            _tokenValidationOptions = tokenValidationOptions.Value;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<IActionResult> CreateToken([FromBody]CreateTokenRequest request)
        {
            if (await _userService.VerifyCredentials(request.Email, request.Password))
            {
                User user = await _userService.GetUser(request.Email);
                JwtSecurityToken token = CreateSecurityToken(user);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            throw new InvalidOperationException("Failed to login.");
        }

        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterUserRequest request)
        {
            await _userService.Register(request.Email, request.Password, request.PhoneNumber);
            return Ok();
        }

        private JwtSecurityToken CreateSecurityToken(User user)
        {
            IEnumerable<Claim> claims = GetUserClaims(user);
            return CreateTokenBasedOnClaims(claims);
        }

        private JwtSecurityToken CreateTokenBasedOnClaims(IEnumerable<Claim> claims)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenValidationOptions.IssuerSigningKey));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _tokenValidationOptions.Issuer,
                audience: _tokenValidationOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
            );
        }

        private IEnumerable<Claim> GetUserClaims(User user)
        {
            return new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
            }.Union(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
        }
    }
}