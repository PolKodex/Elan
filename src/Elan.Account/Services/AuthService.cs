using Elan.Account.Contracts;
using Elan.Account.Models;
using Elan.Common.Exceptions;
using Elan.Data.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Elan.Account.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ElanUser> _userManager;
        private readonly SignInManager<ElanUser> _signInManager;
        private readonly IAuthValidationService _authValidationService;
        private readonly IConfiguration _configuration;
        private readonly IUserSettingsService _userSettings;

        public AuthService(
            UserManager<ElanUser> userManager,
            SignInManager<ElanUser> signInManager,
            IAuthValidationService authValidationService,
            IUserSettingsService userSettings,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _userSettings = userSettings;
            _signInManager = signInManager;
            _authValidationService = authValidationService;
            _configuration = configuration;
        }

        public async Task<string> Register(RegisterViewModel model)
        {
            await _authValidationService.ValidateRegisterViewModel(model);

            var newUser = new ElanUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PasswordHintQuestion = model.Question,
                PasswordHintAnswer = model.Answer,
                Gender = model.Gender
            };
            
            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                throw new RegistrationFailedException(
                    $"An error occured while registering user: {result.Errors.Select(e => e.Description).Join(", ")}");
            }

            await _userSettings.AddSettings(newUser);

            await _signInManager.SignInAsync(newUser, false);
            return GetToken(newUser);
        }

        public async Task<string> SignIn(SignInViewModel model)
        {
            await _authValidationService.ValidateSignInViewModel(model);

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (!result.Succeeded)
            {
                throw new SignInFailedException(
                    $"An error occured while signing in user: {model.UserName}");
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            return GetToken(user);
        }

        public async Task<string> GetPasswordHintQuestion(string userName)
        {
            await _authValidationService.ValidatePasswordHintQuestion(userName);
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new PasswordHintException($"Account with login: {userName} not found!");
            }

            return user.PasswordHintQuestion;
        }

        public async Task<string> ChangePassword(ChangePasswordViewModel model)
        {
            await _authValidationService.ValidateChangePasswordViewModel(model);

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                throw new PasswordHintException($"Account with login: {model.UserName} not found!");
            }

            if (user.PasswordHintAnswer != model.Answer)
            {
                throw new PasswordHintException("Incorrect answer!");
            }

            await _userManager.RemovePasswordAsync(user);

            var result = await _userManager.AddPasswordAsync(user, model.Password);
            if (!result.Succeeded)
            {
                throw new PasswordHintException(
                    $"An error occured while changing password: {result.Errors.Select(e => e.Description).Join(", ")}");
            }
            await _signInManager.SignInAsync(user, false);

            return GetToken(user);
        }

        private string GetToken(ElanUser user)
        {
            var utcNow = DateTime.UtcNow;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString(CultureInfo.InvariantCulture))
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Tokens:Key")));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddSeconds(_configuration.GetValue<int>("Tokens:Lifetime"))
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
