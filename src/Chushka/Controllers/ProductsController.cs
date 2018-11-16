
using System;
using System.Linq;
using Chushka.Data;
using Chushka.Models;
using Chushka.Models.Enums;
using Chushka.Web.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chushka.Web.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private const string adminRole = "Administrator";
        private ChushkaDbContext dbContext;

        public ProductsController(ChushkaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Details(int id)
        {
            var product = dbContext.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return this.BadRequest("Invalid product id.");
            }
            return View(product);
        }

        [Authorize(Roles = adminRole)]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = adminRole)]
        public IActionResult Create(ProductInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }

            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                Type = (ProductType) model.ProductType
            };

            dbContext.Products.Add(product);
            try
            {
                dbContext.SaveChanges();
                return this.RedirectToAction("Details", new { Id = product.Id });
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [Authorize(Roles = adminRole)]
        public IActionResult Edit(int id)
        {
            var product = this.dbContext.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return this.BadRequest("Invalid product id.");
            }

            var viewModel = new ProductInputModel
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ProductType = (int)product.Type
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = adminRole)]
        public IActionResult Edit(ProductInputModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var product = this.dbContext.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return this.BadRequest("Invalid product id.");
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Type = (ProductType) model.ProductType;
            try
            {
                this.dbContext.SaveChanges();
                return this.RedirectToAction("Details", new {Id = id});
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }         
        }

        [Authorize(Roles = adminRole)]
        public IActionResult Delete(int id)
        {
            var product = this.dbContext.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return this.BadRequest("Invalid product id.");
            }

            var viewModel = new ProductInputModel
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ProductType = (int)product.Type
            };

            return this.View(viewModel);
        }

        [HttpPost("/products/delete/{id}")]
        [Authorize(Roles = adminRole)]
        public IActionResult DoDelete(int id)
        {
            var product = this.dbContext.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return this.BadRequest("Invalid product id.");
            }

            this.dbContext.Products.Remove(product);
            try
            {
                this.dbContext.SaveChanges();
                return this.RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        public IActionResult Order(int id)
        {
            var product = this.dbContext.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return this.BadRequest("Invalid product id.");
            }

            var user = this.dbContext.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            if (user == null)
            {
                return this.BadRequest("Invalid user.");
            }

            var order = new Order
            {
                Client = user,
                Product = product,
                OrderedOn = DateTime.UtcNow
            };

            this.dbContext.Orders.Add(order);
            try
            {
                this.dbContext.SaveChanges();
                if (this.User.IsInRole("Administrator"))
                {
                    return this.RedirectToAction("All", "Orders");
                }

                return this.RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}