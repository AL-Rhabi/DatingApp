using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Enitites;
using Azure.Core.Pipeline;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUsers>> Register(RegisterDto registerDto)
        {
            if( await UserExists(registerDto.Username))return BadRequest("Username is taken");
            using var hmac = new HMACSHA512();
            var user = new AppUsers
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUsers>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username); 
            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            
            for(int i=0;i< ComputeHash.Length;i++)
            {
                if(ComputeHash[i] != user.PasswordHash[i]) return  Unauthorized("Invalid Password");

            }

            return user;
           

        }

        private async Task<bool> UserExists(string Username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == Username.ToLower());
        }

        
        
    }
}