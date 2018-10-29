using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Elan.Account.Contracts;
using Elan.Account.Models;
using Elan.Common.Exceptions;
using Elan.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elan.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ElanDbContext _db;

        public AuthController(IAuthService authService, ElanDbContext db)
        {
            _authService = authService;
            _db = db;
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

        [HttpGet]
        [Authorize]
        public JsonResult Get()
        {
            return Json(_db.Users.Select(x => x.UserName).ToList());
        }
    }
}
