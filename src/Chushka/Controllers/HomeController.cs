using System.Diagnostics;
using System.Linq;
using Chushka.Data;
using Microsoft.AspNetCore.Mvc;
using Chushka.Models;
using Chushka.Web.ViewModels.Home;

namespace Chushka.Controllers
{
    public class HomeController : Controller
    {
        private ChushkaDbContext dbContext;

        public HomeController(ChushkaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var viewModel = new LoggedInViewModel
                {
                    Products = dbContext.Products.Select(p => 
                        new ProductIndexDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            ShortDescription = p.Description.Substring(0, 37) + "...", //s 50 (53) simvola izliza ot kvadratcheto
                            Price = p.Price
                        }).ToList()
                };

                return this.View("IndexLoggedIn", viewModel);
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
