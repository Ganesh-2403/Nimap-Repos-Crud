using CrudApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Collections.Generic;

namespace CrudApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductDAL dal;

        public HomeController()
        {
            dal = new ProductDAL();  
        }

        public IActionResult Index()
        {
            List<Product> products = dal.getAllProduct();

            if (products == null || products.Count == 0)
            {
                ViewBag.Message = "No products found.";  
            }

            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product prod)
        {
            try
            {
                int newId = dal.AddProduct(prod); // Get the new ProductId
                TempData["SuccessMessage"] = $"Product added successfully with ID: {newId}";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while adding the product.");
                return View(prod);
            }
        }




        public IActionResult Edit(int productId)
        {
            Product product = dal.getProdData(productId);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product prod)
        {
            try
            {
                dal.UpdateProduct(prod);
                return Redirect("Index");
            }
            catch
            {
                return View();
            }
           
        }

        [HttpGet]
        public IActionResult Delete(int productId)
        {
            var product = dal.getProdData(productId);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int productId)
        {
            dal.DeleteProduct(productId);
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
