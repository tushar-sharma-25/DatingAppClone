using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Dto;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly DataContext _dataContext;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext dataContext, ITokenService tokenService)
        {
            _dataContext = dataContext;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await userExists(registerDto.UserName)) 
                return BadRequest("User Already exists");

            using var hmac = new HMACSHA512();

            var user = new AppUser{
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _dataContext.AppUsers.Add(user); // begins tracking of data in EF , does not add them to db
            await _dataContext.SaveChangesAsync();    // save tracked changes to db.

            var userDto = new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

            return Ok(userDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _dataContext.AppUsers.SingleOrDefaultAsync(x => x.UserName==loginDto.UserName);
            if(user==null)
                return Unauthorized("User does not exist");

            var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(var i=0;i<computedHash.Length;i++)
                if(computedHash[i]!=user.PasswordHash[i])
                    return Unauthorized("Password do not match");

            var userDto = new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

            return Ok(userDto);
        }

        private async Task<bool> userExists(string username)
        {
            return await _dataContext.AppUsers.AnyAsync(x=> x.UserName == username.ToLower());
        }

    }
}