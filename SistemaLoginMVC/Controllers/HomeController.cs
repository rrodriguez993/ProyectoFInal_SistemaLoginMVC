using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaLoginMVC.Models;
using System.Diagnostics;

namespace SistemaLoginMVC.Controllers
{
    [Authorize] // Bloquea el acceso a cualquier usuario que no haya iniciado sesión
    public class HomeController : Controller

    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
