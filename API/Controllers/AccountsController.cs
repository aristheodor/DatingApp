using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entites;
using API.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Namespace;

namespace API.Controllers
{

    public class AccountsController : BaseApiController
    {

        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountsController(DataContext context, ITokenService tokenService)
        {

            _context = context;
            _tokenService = tokenService;
            _tokenService = tokenService;
        }

        [HttpPost("register")] //api/acounts/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {



            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");


            {
                using var hmac = new HMACSHA512();

                var user = new AppUser
                {
                    UserName = registerDto.Username.ToLower(),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                    PasswordSalt = hmac.Key
                };


                _context.Users.Add(user);
                await _context.SaveChangesAsync();


                return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)

            };
            }

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null) return Unauthorized("invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);


            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));


            for (int i = 0; i < computedHash.Length; i++)
            {

                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");

            }
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)

            };
        }

        private async Task<bool> UserExists(string username)
        {

            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }





    }




}