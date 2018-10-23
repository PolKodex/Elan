using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elan.Account.Contracts;
using Elan.Account.Models;
using Elan.Data;
using Microsoft.AspNetCore.Mvc;

namespace Elan.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthController: Controller
    {
        private readonly IAuthService _authService;
        private readonly ElanDbContext _db;

        public AuthController(IAuthService authService, ElanDbContext db)
        {
            _authService = authService;
            _db = db;
        }

        [HttpPost]
        public async Task Register([FromBody] RegisterViewModel model)
        {
            await _authService.Register(model);
        }

        [HttpGet]
        public JsonResult Get()
        {
            return Json(_db.Users.Select(x => x.UserName).ToList());
        }
    }
}
