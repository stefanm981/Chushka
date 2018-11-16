using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chushka.Data;
using Chushka.Web.ViewModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chushka.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class OrdersController : Controller
    {
        private ChushkaDbContext dbContext;

        public OrdersController(ChushkaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult All()
        {
            var viewModel = new AllOrdersViewModel
            {
                Orders = this.dbContext.Orders.Select(o => new OrderDto
                {
                    OrderId = o.Id,
                    ProductName = o.Product.Name,
                    CustomerName = o.Client.UserName,
                    OrderedOn = o.OrderedOn
                }).ToList()
            };

            return View(viewModel);
        }
    }
}