using System.Net;
using System.Threading.Tasks;
using Elan.Account.Contracts;
using Elan.Account.Models;
using Elan.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Elan.Web.Controllers
{
    public class AuthController : ElanBaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<JsonResult> Register([FromBody] RegisterViewModel model)
        {
            try
            {
                var token = await _authService.Register(model);
                return Json(token);
            }
            catch (RegistrationFailedException ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Login([FromBody] SignInViewModel model)
        {
            try
            {
                var token = await _authService.SignIn(model);
                return Json(token);
            }
            catch (SignInFailedException ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
    }
}
