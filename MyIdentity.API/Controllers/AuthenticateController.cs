using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyIdentity.API.Authentication.DTOModels;
using MyIdentity.API.Authentication.Models;
using MyIdentity.API.DataAccess;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyIdentity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUserData _userData;

        public AuthenticateController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IUserData userData)
        {
            _userData = userData;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] DTOLogin model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] DTORegister model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null) return StatusCode(StatusCodes.Status500InternalServerError, new DTOResponse { Status = "Error", Message = "User already exists" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Username,
                Id= Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return StatusCode(StatusCodes.Status500InternalServerError, new DTOResponse { Status = "Error", Message = "User creation failed" });

            //Create user in user table
            DTOUser dTOUser = new DTOUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.Email,
                Id = user.Id,
                CreatedDate = DateTime.Now
            };

            bool userCreated = _userData.CreateUser(dTOUser);
            if (!userCreated)
            {
                await _userManager.DeleteAsync(user);
                return StatusCode(StatusCodes.Status500InternalServerError, new DTOResponse { Status = "Error", Message = "User creation failed" });
            }

            return Ok(new DTOResponse { Status = "Success", Message = "User created successfully" });
        }
    }
}
