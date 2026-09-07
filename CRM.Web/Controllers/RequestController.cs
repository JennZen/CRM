using Microsoft.AspNetCore.Mvc;

namespace CRM.Web.Controllers
{
    public class RequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
