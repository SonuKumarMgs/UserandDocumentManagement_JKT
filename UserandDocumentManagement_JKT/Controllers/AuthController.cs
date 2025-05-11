using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserandDocumentManagement_JKT.Data;
using UserandDocumentManagement_JKT.DTOs;
using UserandDocumentManagement_JKT.Models;
using UserandDocumentManagement_JKT.ResponseTypes;
using UserandDocumentManagement_JKT.Sevices.Interfaces;

namespace UserandDocumentManagement_JKT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISignupService _signupService;
        private readonly AppDbCotext _context;
        private readonly IConfiguration _config;
        public AuthController(ISignupService signupService, AppDbCotext appDbCotext, IConfiguration config)
        {
            _signupService = signupService;
            _context = appDbCotext;
            _config = config;
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody] UserDto userdto)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            var passwordHasher = new PasswordHasher<object>();
            string hashedPassword = passwordHasher.HashPassword(null, userdto.Password);
            userdto.Password = hashedPassword;
            string data=await _signupService.UserSingup(userdto);
            return Ok(data);
        }

        [HttpPost("login")]
        public IActionResult Login(Login req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var passwordHasher = new PasswordHasher<object>();
            var user = _context.Users.SingleOrDefault(u => u.Username == req.Username);
            var result = passwordHasher.VerifyHashedPassword(null, user.Password, req.Password);
            bool isValid = result == PasswordVerificationResult.Success;
            if (user == null || isValid==false)
                return Unauthorized("Invalid credentials");

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {           
            return Ok(new ApiResponse<string>("Logged out successfully", 200));
        }
    }
}
