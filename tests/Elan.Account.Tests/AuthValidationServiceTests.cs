using Elan.Account.Models;
using Elan.Account.Services;
using Elan.Common.Exceptions;
using Xunit;
namespace Elan.Account.Tests
{
    public class AuthValidationServiceTests
    {
        [Theory]
        [InlineData("", "password", "email")]
        [InlineData("properUserName", "", "email")]
        [InlineData("properUserName", "password", "")]
        public void ValidateRegisterViewModel_InvalidCredentials_ThrowsRegistrationFailedException(string userName, string password, string email)
        {
            var sut = new AuthValidationService();

            var model = new RegisterViewModel
            {
                UserName = userName,
                Password = password,
                Email = email
            };

            Assert.Throws<RegistrationFailedException>(() => sut.ValidateRegisterViewModel(model));
        }

        [Theory]
        [InlineData("", "password")]
        [InlineData("properUserName", "")]
        public void ValidateSignInViewModel_InvalidCredentials_ThrowsRegistrationFailedException(string userName, string password)
        {
            var sut = new AuthValidationService();

            var model = new SignInViewModel
            {
                UserName = userName,
                Password = password,
            };

            Assert.Throws<SignInFailedException>(() => sut.ValidateSignInViewModel(model));
        }
    }
}
