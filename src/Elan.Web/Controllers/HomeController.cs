using Elan.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Elan.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public ActionResult Index()
        {
            var testModel = new HomeModel {Title = "testing title"};
            return View("Index", testModel);
        }
    }
}
