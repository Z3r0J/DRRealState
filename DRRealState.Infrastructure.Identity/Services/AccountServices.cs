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
using Microsoft.AspNetCore.WebUtilities;
using DRRealState.Core.Application.DTOS.Email;
using DRRealState.Core.Application.Helpers;

namespace DRRealState.Infrastructure.Identity.Services
{
    public class AccountServices : IAccountServices
    {

        private readonly UserManager<RealStateUser> _userManager;
        private readonly SignInManager<RealStateUser> _signInManager;
        private readonly JWTSettings _jWTSettings;
        private readonly IEmailServices _emailService;

        public AccountServices(UserManager<RealStateUser> userManager,
            SignInManager<RealStateUser> signInManager,
            IOptions<JWTSettings> jWTSettings,
            IEmailServices emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jWTSettings = jWTSettings.Value;
            _emailService = emailService;
        }

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
            var RoleList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            if (RoleList.Any(r => r == "AGENT" || r=="CLIENT"))
            {
                response.HasError = true;
                response.Error = $"You cannot access here. Please use the Web APP";
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
            response.Documents = user.Documents;

            response.Roles = RoleList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            var generateRefreshToken = GenerateRefreshToken();

            response.RefreshToken = generateRefreshToken.Token;

            return response;
        }

        public async Task<AuthenticationResponse> AuthenticateWebAppAsync(AuthenticationRequest request)
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

            var RoleList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            if (RoleList.Any(r => r == "DEVELOPER"))
            {
                response.HasError = true;
                response.Error = $"You cannot access here. Please use the API";
                return response;
            }

            response.Id = user.Id;
            response.FirstName = user.Name;
            response.LastName = user.LastName;
            response.Email = user.Email;
            response.UserName = user.UserName;
            response.Phone = user.PhoneNumber;
            response.PhotoUrl = user.PhotoUrl;
            response.Documents = user.Documents;
            response.Roles = RoleList.ToList();
            response.IsVerified = user.EmailConfirmed;

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
                Phone = user.PhoneNumber,
                Documents = user.Documents,
                Code = user.Code
                };

                accounts.Add(account);
            }

            return accounts;
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterResponse> RegisterClientAsync(RegisterRequest request,string origin) {

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userWithSameEmail != null)
            {
                return new() { HasError = true, Error = $"This email {request.Email} already used." };
            }

            var userWithSameUsername = await _userManager.FindByNameAsync(request.Username);

            if (userWithSameUsername != null)
            {
                return new() { HasError = true, Error = "This username has been taken." };
            }

            var user = new RealStateUser()
            {
                Email = request.Email,
                Name = request.Name,
                EmailConfirmed =false,
                LastName = request.LastName,
                PhoneNumber = request.Phone,
                PhotoUrl = request.PhotoUrl,
                UserName = request.Username
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.CLIENT.ToString());
                var verificationUri = await SendVerificationEmailUri(user, origin);

                await _emailService.SendAsync(new EmailRequest()
                {
                    To = user.Email,
                    Body = $" <table align='center' class='m_8434762703074949762container' style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;width:580px;margin:0 auto;Margin:0 auto;float:none;text-align:center;background: #ffffff;' width='580' valign='top'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <td style='word-wrap:break-word;vertical-align:top;font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;font-size:16px;line-height:1.3;color:#fff;border-collapse:collapse!important' valign='top' align='left'> <table class='m_8434762703074949762row' style='border-spacing:0;border-collapse:collapse;vertical-align:top;text-align:left;padding:0;width:100%;display:table' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th class='m_8434762703074949762small-12 m_8434762703074949762columns' style='font-family:Courier;font-weight:normal;padding:0;text-align:left;font-size:16px;line-height:1.3;color:#fff;margin:0 auto;Margin:0 auto;width:564px;padding-bottom:0!important;padding-left:0!important;padding-right:0!important' width='564' align='left'> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th style='font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;font-size:16px;line-height:1.3;color:#fff' align='left'></th> <td align='right' style='padding:0px;margin:0px auto;font-size:0px;line-height:1px;padding:0px;font-size:0px;line-height:100%;padding:0px'> <a href='#' style='text-decoration:none;border-style:none;border:0px;padding:0px;margin:0px;color:#FD2828;text-decoration:none' target='_blank'> <img src='https://i.imgur.com/zYferb7.png' width='180' alt='SmartNetwork' title='SmartNetwork' style='margin:0px;padding:0px;display:inline-block;border:none;outline:none' class='CToWUd'> </a> </td></tr></tbody> </table> </th> </tr></tbody> </table> </td></tr></tbody> </table> <table align='center' class='m_8434762703074949762container' style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;background:#000000;;width:580px;margin:0 auto;Margin:0 auto;float:none;text-align:center' width='580' valign='top'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <td style='word-wrap:break-word;vertical-align:top;font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;font-size:16px;line-height:1.3;color:#fff;border-collapse:collapse!important' valign='top' align='left'> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <td height='18px' style='word-wrap:break-word;vertical-align:top;font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;color:#fff;font-size:18px;line-height:18px;border-collapse:collapse!important' valign='top' align='left'>&nbsp;</td></tr></tbody> </table> <table class='m_8434762703074949762row' style='border-spacing:0;border-collapse:collapse;vertical-align:top;text-align:left;padding:0;width:100%;display:table' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th class='m_8434762703074949762body-message m_8434762703074949762small-12 m_8434762703074949762columns' style='font-family:Courier;font-weight:normal;padding:0;text-align:left;font-size:16px;line-height:1.3;color:#fff;margin:0 auto;Margin:0 auto;width:564px;padding-bottom:0!important;padding-left:0!important;padding-right:0!important' width='564' align='left'> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th style='font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;font-size:16px;line-height:1.3;color:#fff' align='left'> <table class='m_8434762703074949762row' style='border-spacing:0;border-collapse:collapse;vertical-align:top;text-align:left;background:#000000;padding:0;display:table;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th class='m_8434762703074949762small-12 m_8434762703074949762columns' style='font-family:Courier;font-weight:normal;padding:0;text-align:left;font-size:16px;line-height:1.3;color:#fff;margin:0 auto;Margin:0 auto;width:100%;padding-bottom:0!important;padding-left:22px!important;padding-right:22px!important' width='100%' align='left'> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th style='font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;font-size:16px;line-height:1.3;color:#fff' align='left'> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <td height='20px' style='word-wrap:break-word;vertical-align:top;font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;color:#fff;font-size:20px;line-height:20px;border-collapse:collapse!important' valign='top' align='left'> &nbsp; </td></tr></tbody> </table> <table class='m_8434762703074949762row' style='border-spacing:0;border-collapse:collapse;vertical-align:top;text-align:left;padding:0;display:table;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th class='m_8434762703074949762small-12 m_8434762703074949762columns' style='font-family:Courier;font-weight:normal;padding:0;text-align:left;font-size:16px;line-height:1.3;color:#fff;margin:0 auto;Margin:0 auto;width:100%;padding-bottom:0!important;padding-left:0!important;padding-right:0!important' width='100%' align='left'> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th style='font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;font-size:16px;line-height:1.3;color:#fff' align='left'> <h5 style='padding:0;margin:0;Margin:0;word-wrap:normal;font-family:Courier;Margin-bottom:10px;font-size: 24px;font-weight:normal;font-style:normal;font-stretch:normal;line-height:1.12;letter-spacing:normal;text-align:center;color:#fff;margin-bottom:16px;/* border-bottom:solid 1px #d0cfd2; */'>Hello,{user.Name} {user.LastName}!</h5> </th> <th class='m_8434762703074949762expander' style='font-family:Courier;font-weight:normal;margin:0;Margin:0;text-align:left;font-size:16px;line-height:1.3;color:#fff;width:0;padding:0!important' align='left'></th> </tr></tbody> </table> </th> </tr></tbody> </table> <table class='m_8434762703074949762row' style='border-spacing:0;border-collapse:collapse;vertical-align:top;text-align:left;padding:0;display:table;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th class='m_8434762703074949762small-12 m_8434762703074949762columns' style='font-family:Courier;font-weight:normal;padding:0;text-align:left;font-size:16px;line-height:1.3;color:#fff;margin:0 auto;Margin:0 auto;width:100%;padding-bottom:0!important;padding-left:0!important;padding-right:0!important' width='100%' align='left'> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th style='font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align: center;font-size:16px;line-height:1.3;color:#fff;' align='left'> <p style='font-family:Courier;padding:0;Margin:0;text-align:left;line-height:1.3;margin-bottom:10px;Margin-bottom:10px;color:#fff;font-size:16px;font-weight:400;margin:0'> Welcome to DR Real Estate, here is the link to activate your account: </p><br><br><span style='color:#FD2828; font-size:23px;'>To activate your account click the button below: </span> <br><br/> <br/> <a href={verificationUri} style='font-family:Courier;font-weight:normal;padding: 20px;top: 0px;/* margin:0; *//* Margin:0; */text-align:center;line-height:1.3;/* color:#FD2828; */text-decoration:underline;color:#FFF;width: 50px;height: 67px!important;background: #FD2828;border-radius: 9px;text-decoration: none;' target='_blank'>Click here</a><br><br></th> <th class='m_8434762703074949762expander' style='font-family:Courier;font-weight:normal;margin:0;Margin:0;text-align:left;font-size:16px;line-height:1.3;color:#fff;width:0;padding:0!important' align='left'></th> </tr></tbody> </table> </th> </tr></tbody> </table> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <td height='30px' style='word-wrap:break-word;vertical-align:top;font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;color:#fff;font-size:30px;line-height:30px;border-collapse:collapse!important' valign='top' align='left'> &nbsp; </td></tr></tbody> </table> </th> </tr></tbody> </table> </th> </tr></tbody> </table> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <td height='16px' style='word-wrap:break-word;vertical-align:top;font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;color:#fff;font-size:16px;line-height:16px;border-collapse:collapse!important' valign='top' align='left'> &nbsp; </td></tr></tbody> </table> <table class='m_8434762703074949762row' style='border-spacing:0;border-collapse:collapse;vertical-align:top;text-align:left;padding:0;display:table;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th class='m_8434762703074949762small-12 m_8434762703074949762columns' style='font-family:Courier;font-weight:normal;padding:0;text-align:left;font-size:16px;line-height:1.3;color:#fff;margin:0 auto;Margin:0 auto;width:100%;padding-bottom:0!important;padding-left:0!important;padding-right:0!important' width='100%' align='left'> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th style='font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;font-size:16px;line-height:1.3;color:#fff' align='left'> <p style='font-family:Courier;padding:0;Margin:0;line-height:1.3;Margin-bottom:10px;text-align:center;font-size:16px;font-weight:400;margin:0;color:#797979;margin-bottom:10px'> Please, give us a feedback on the updates <img data-emoji='💛' class='an1' alt='💛' aria-label='💛' src='https://fonts.gstatic.com/s/e/notoemoji/14.0/1f49b/32.png' loading='lazy'><br>Write an email to <a href='mailto:drrealestate695@gmail.com' style='font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;line-height:1.3;color:#FD2828;text-decoration:underline' target='_blank'>drrealestate695@gmail.com.do</a> or comment on the latest post on our social media. Cheers! <img data-emoji='🍻' class='an1' alt='🍻' aria-label='🍻' src='https://fonts.gstatic.com/s/e/notoemoji/14.0/1f37b/32.png' loading='lazy'> </p><table align='center' class='m_8434762703074949762row m_8434762703074949762menu' style='border-spacing:0;border-collapse:collapse;vertical-align:top;padding:0;margin:0 auto;Margin:0 auto;float:none;text-align:center;display:table;width:auto!important' valign='top'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th class='m_8434762703074949762small-4 m_8434762703074949762columns' style='font-family:Courier;font-weight:normal;padding:0;text-align:left;font-size:16px;line-height:1.3;color:#fff;margin:0 auto;Margin:0 auto;width:33.33333%;padding-bottom:0!important;padding-left:0!important;padding-right:0!important' width='33.33333%' align='left'> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th style='font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;font-size:16px;line-height:1.3;color:#fff' align='left'> <a href='#' style='font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;line-height:1.3;color:#FD2828;text-decoration:underline;margin-right:16px;display:inline-block' target='_blank'> <img src='https://icon-library.com/images/facebook-icon-png-32x32/facebook-icon-png-32x32-0.jpg' alt='fb' style='outline:none;text-decoration:none;width:auto;width:32px; height:32px;max-width:100%;clear:both;display:block;border:none' class='CToWUd'> </a> </th> </tr></tbody> </table> </th> <th class='m_8434762703074949762small-4 m_8434762703074949762columns' style='font-family:Courier;font-weight:normal;padding:0;text-align:left;font-size:16px;line-height:1.3;color:#fff;margin:0 auto;Margin:0 auto;width:33.33333%;padding-bottom:0!important;padding-left:0!important;padding-right:0!important' width='33.33333%' align='left'> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th style='font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;font-size:16px;line-height:1.3;color:#fff' align='left'> <a href='#' style='font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;line-height:1.3;color:#FD2828;text-decoration:underline;margin-right:16px;display:inline-block' target='_blank'> <img src='https://images.vexels.com/media/users/3/137419/isolated/lists/b1a3fab214230557053ed1c4bf17b46c-logotipo-de-icono-de-twitter.png' alt='twitter' style='outline:none;text-decoration:none;width:32px; height:32px;max-width:100%;clear:both;display:block;border:none' class='CToWUd'> </a> </th> </tr></tbody> </table> </th> <th class='m_8434762703074949762small-4 m_8434762703074949762columns' style='font-family:Courier;font-weight:normal;padding:0;text-align:left;font-size:16px;line-height:1.3;color:#fff;margin:0 auto;Margin:0 auto;width:33.33333%;padding-bottom:0!important;padding-left:0!important;padding-right:0!important' width='33.33333%' align='left'> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th style='font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;font-size:16px;line-height:1.3;color:#fff' align='left'> <a href='#' style='font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;line-height:1.3;color:#FD2828;text-decoration:underline' target='_blank'> <img src='https://cdn.iconscout.com/icon/free/png-256/instagram-1946323-1646407.png' alt='instagram' style='outline:none;text-decoration:none;width:32px;height:32px;max-width:100%;clear:both;display:block;border:none' class='CToWUd'> </a> </th> </tr></tbody> </table> </th> </tr></tbody> </table> </th> </tr></tbody> </table> </th> </tr></tbody> </table> </th> </tr></tbody> </table> </th> </tr></tbody> </table> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <td height='16px' style='word-wrap:break-word;vertical-align:top;font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;color:#fff;font-size:16px;line-height:16px;border-collapse:collapse!important' valign='top' align='left'>&nbsp;</td></tr></tbody> </table> <table class='m_8434762703074949762row' style='border-spacing:0;border-collapse:collapse;vertical-align:top;text-align:left;background:#FD2828;padding:0;width:100%;display:table' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th class='m_8434762703074949762small-12 m_8434762703074949762columns' style='font-family:Courier;font-weight:normal;padding:0;text-align:left;font-size:16px;line-height:1.3;color:#fff;margin:0 auto;Margin:0 auto;width:564px;padding-bottom:0!important;padding-left:32px!important;padding-right:32px!important' width='564' align='left'> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th style='font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;font-size:16px;line-height:1.3;color:#fff' align='left'> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <td height='15px' style='word-wrap:break-word;vertical-align:top;font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;color:#fff;font-size:15px;line-height:15px;border-collapse:collapse!important' valign='top' align='left'> &nbsp; </td></tr></tbody> </table> <table class='m_8434762703074949762row' style='border-spacing:0;border-collapse:collapse;vertical-align:top;text-align:left;padding:0;display:table;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th class='m_8434762703074949762mgb-sm-16 m_8434762703074949762small-12 m_8434762703074949762columns' style='font-family:Courier;font-weight:normal;padding:0;text-align:left;font-size:16px;line-height:1.3;color:#fff;margin:0 auto;Margin:0 auto;width:33.33333%;padding-bottom:0!important;margin-bottom:16px!important;padding-left:0!important;padding-right:0!important' width='33.33333%' align='left'> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th style='font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;font-size:16px;line-height:1.3;color:#fff' align='left'> </th> </tr></tbody> </table> </th> <th class='m_8434762703074949762mgb-sm-16 m_8434762703074949762small-12 m_8434762703074949762columns' style='font-family:Courier;font-weight:normal;padding:0;text-align:left;font-size:16px;line-height:1.3;color:#fff;margin:0 auto;Margin:0 auto;width:33.33333%;padding-bottom:0!important;margin-bottom:16px!important;padding-left:0!important;padding-right:0!important' width='33.33333%' align='left'> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:center;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <th style='font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:center;font-size:16px;line-height:1.3;color:#fff' align='center'> <p class='m_8434762703074949762small-text-center' style='font-family:Courier;padding:0;margin:0;Margin:0;Margin-bottom:10px;font-size:16px;font-weight:400;line-height:1.3;text-align:center;text-transform:uppercase;margin-bottom:0;color:#fff!important'> © {DateTime.Now.Year} Copyright - All right reserved to (TEAM J). </p></th> </tr></tbody> </table> </th> </tr></tbody> </table> <table style='border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%' width='100%' valign='top' align='left'> <tbody> <tr style='padding:0;vertical-align:top;text-align:left' valign='top' align='left'> <td height='23px' style='word-wrap:break-word;vertical-align:top;font-family:Courier;font-weight:normal;padding:0;margin:0;Margin:0;text-align:left;color:#fff;font-size:23px;line-height:23px;border-collapse:collapse!important' valign='top' align='left'> &nbsp; </td></tr></tbody> </table> </th> </tr></tbody> </table> </th> </tr></tbody> </table> </td></tr></tbody> </table>",
                    Subject = "DR Real State - Confirm Account"
                });
            }
            else
            {
                return new()
                {
                    HasError = true,
                    Error = $"An error occurred trying to register the user."
                };
            }

            return new() { Id = await _userManager.GetUserIdAsync(user), HasError = false };
        }

        private async Task<string> SendVerificationEmailUri(RealStateUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ConfirmEmail";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", code);

            return verificationUri;
        }

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return $"No accounts registered with this user";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return $"Account confirmed for {user.Email}. You can now use the app";
            }
            else
            {
                return $"An error occurred while confirming {user.Email}.";
            }
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
            UserName = request.Username,
            Documents = request.Documents
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
        public async Task<RegisterResponse> RegisterDeveloperAsync(RegisterRequest request) {

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
            UserName = request.Username,
            Documents = request.Documents
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.DEVELOPER.ToString());
            }
            else
            {
                return new() { HasError=true,Error= $"An Error ocurred, please try again." };
            }

            return new() { HasError=false};

        }
        public async Task<RegisterResponse> RegisterAgentAsync(RegisterRequest request) {

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
                EmailConfirmed = false,
                PhotoUrl = request.PhotoUrl,
                PhoneNumber = request.Phone,
                UserName = request.Username,
                Code = GenerateRandomCode.GenerateCode()
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.AGENT.ToString());
            }
            else
            {
                return new() { HasError=true,Error= $"An Error ocurred, please try again." };
            }

            

            return new() {Id= await _userManager.GetUserIdAsync(user), HasError =false};

        }

        public async Task<EditResponse> EditAgentAsync(EditRequest request) {

            var user = await _userManager.FindByIdAsync(request.Id);

            user.Name = request.Name;
            user.LastName = request.LastName;
            user.PhoneNumber = request.Phone;
            user.PhotoUrl = request.PhotoURL;

            var response = await _userManager.UpdateAsync(user);

            if (!response.Succeeded)
            {
                foreach (var item in response.Errors)
                {
                    return new() { HasError = true, Error = item.Description };
                }
            }

            return new() { HasError = false };
        }

        public async Task<PasswordResponse> ChangePasswordAsync(PasswordRequest request)
        {

            var user = await _userManager.FindByIdAsync(request.UserId);

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var response = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            PasswordResponse passwordResponse = new();

            if (response.Succeeded)
            {
                passwordResponse.HasError = false;
            }
            if (response.Errors.Count() > 0)
            {
                foreach (var error in response.Errors)
                {
                    passwordResponse.HasError = true;
                    passwordResponse.Error = error.Description;
                }
            }

            return passwordResponse;
        }


        public async Task<EditResponse> EditAsync(EditRequest request) {

            var user = await _userManager.FindByIdAsync(request.Id);

            user.Name = request.Name;
            user.LastName = request.LastName;
            user.Documents = request.Documents;
            user.Email = request.Email;
            user.NormalizedEmail = request.Email.ToUpper();
            user.UserName = request.Username;
            user.NormalizedUserName = request.Username.ToUpper();


            var response = await _userManager.UpdateAsync(user);

            if (!response.Succeeded)
            {
                foreach (var item in response.Errors)
                {
                    return new() { HasError = true, Error = item.Description };
                }
            }

            return new() { HasError = false };
        }

        public async Task<RegisterResponse> AddPhotoAsync(string photoUrl,string Id) {

            var user = await _userManager.FindByIdAsync(Id);

            user.PhotoUrl = photoUrl;

            var response = await _userManager.UpdateAsync(user);

            if (response.Succeeded)
            {
                return new() { HasError = false };
            }
            else
            {
                return new() { Error = "Oops, an error occurred try again", HasError = true };
            }
        
        }

        public async Task<ActivateResponse> ActivateAsync(ActivateRequest request)
        {

            ActivateResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"The user with this Id {request.UserId} doesn't exist";
                return response;
            }

            user.EmailConfirmed = true;

            await _userManager.UpdateAsync(user);

            return response;

        }

        public async Task<ActivateResponse> DeactivateAsync(ActivateRequest request)
        {

            ActivateResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"The user with this Id {request.UserId} doesn't exist";
                return response;
            }

            user.EmailConfirmed = false;

            await _userManager.UpdateAsync(user);

            return response;

        }

        public async Task<DeleteResponse> DeleteUserAsync(string Id) {

            var user = await _userManager.FindByIdAsync(Id);

            if (user==null)
            {
                return new() { Error = "Not user with this Id", HasError = true };
            }

            var deleteResponse = await _userManager.DeleteAsync(user);

            if (deleteResponse.Errors.Count() > 0)
            {
                foreach (var error in deleteResponse.Errors)
                {
                    return new() { HasError = true,
                    Error = error.Description};
                }
            }

            return new() { HasError = false };

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
