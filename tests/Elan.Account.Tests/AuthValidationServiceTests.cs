using Elan.Account.Models;
using Elan.Account.Services;
using Elan.Common.Exceptions;
using Elan.Data.Contracts;
using FakeItEasy;
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
            var dataService = A.Fake<IDataService>();
            var sut = new AuthValidationService(dataService);

            var model = new RegisterViewModel
            {
                UserName = userName,
                Password = password,
                Email = email
            };

            Assert.ThrowsAsync<RegistrationFailedException>(() => sut.ValidateRegisterViewModel(model));
        }

        [Theory]
        [InlineData("", "password")]
        [InlineData("properUserName", "")]
        public void ValidateSignInViewModel_InvalidCredentials_ThrowsRegistrationFailedException(string userName, string password)
        {
            var dataService = A.Fake<IDataService>();
            var sut = new AuthValidationService(dataService);

            var model = new SignInViewModel
            {
                UserName = userName,
                Password = password,
            };

            Assert.ThrowsAsync<SignInFailedException>(() => sut.ValidateSignInViewModel(model));
        }
    }
}
