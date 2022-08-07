using DRRealState.Core.Application.DTOS.Account;
using DRRealState.Core.Application.Enums;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DRRealState.Core.Domain.Settings;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace DRRealState.Infrastructure.Identity.Services
{
    public class AccountServices : IAccountServices
    {

        private readonly UserManager<RealStateUser> _userManager;
        private readonly SignInManager<RealStateUser> _signInManager;
        private readonly JWTSettings _jWTSettings;

        public AccountServices(UserManager<RealStateUser> userManager, 
            SignInManager<RealStateUser> signInManager, 
            IOptions<JWTSettings> jWTSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jWTSettings = jWTSettings.Value;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {

            AuthenticationResponse response = new();

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"You don't have an account with this email {request.Email}";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Invalid credential for {request.Email}";
                return response;
            }

            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"Your Account is disable, Contact an Administrator.";
                return response;
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

            response.Id = user.Id;
            response.FirstName = user.Name;
            response.LastName = user.LastName;
            response.Email = user.Email;
            response.UserName = user.UserName;
            response.Phone = user.PhoneNumber;
            response.PhotoUrl = user.PhotoUrl;

            var RoleList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = RoleList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            var generateRefreshToken = GenerateRefreshToken();

            response.RefreshToken = generateRefreshToken.Token;

            return response;
        }

        public async Task<List<AccountResponse>> GetUsersAsync() {

            List<AccountResponse> accounts = new();

            var response = await _userManager.Users.ToListAsync();

            foreach (RealStateUser user in response)
            {
               var roles = await _userManager.GetRolesAsync(user);

                AccountResponse account = new() { 
                Id = user.Id,
                FirstName=user.Name,
                LastName=user.LastName,
                PhotoUrl = user.PhotoUrl,
                Email = user.Email,
                Roles = roles.ToList(),
                UserName=user.UserName,
                IsVerified = user.EmailConfirmed,
                Phone = user.PhoneNumber
                };

                accounts.Add(account);
            }

            return accounts;
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<RegisterResponse> RegisterAdministratorAsync(RegisterRequest request) {

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userWithSameEmail !=null )
            {
                return new() { HasError = true, Error = $"This email {request.Email} already used." };
            }
            
            var userWithSameUsername = await _userManager.FindByNameAsync(request.Username);

            if (userWithSameUsername !=null )
            {
                return new() { HasError = true, Error = "This username has been taken." };
            }

            var user = new RealStateUser {
            Email = request.Email,
            LastName = request.LastName,
            Name = request.Name,
            PhotoUrl = request.PhotoUrl,
            EmailConfirmed = true,
            PhoneNumber = request.Phone,
            UserName = request.Username
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.ADMINISTRATOR.ToString());
            }
            else
            {
                return new() { HasError=true,Error= $"An Error ocurred, please try again." };
            }

            return new() { HasError=false};

        }

        #region Private Methods

        private async Task<JwtSecurityToken> GenerateJWToken(RealStateUser user) { 
        
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var roleClaim = new List<Claim>();

            foreach (string roles in userRoles)
            {
                roleClaim.Add(new("roles", roles));
            }

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid",user.Id),

            }
            .Union(userClaims)
            .Union(roleClaim);


            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTSettings.Key));

            var signInCredentials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jWTSettings.Issuer,
                audience: _jWTSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jWTSettings.DurationInMinutes),
                signingCredentials: signInCredentials
                );

            return jwtSecurityToken;

        }

        private RefreshToken GenerateRefreshToken() {

            return new() {

                Token= RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,

            
            };
        
        }

        private string RandomTokenString() {

            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();

            var randomBytes = new byte[40];

            rngCryptoServiceProvider.GetBytes(randomBytes);

            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        #endregion
    }
}
