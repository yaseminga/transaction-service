using Microsoft.AspNetCore.Mvc;

namespace TransactionService.Api.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
