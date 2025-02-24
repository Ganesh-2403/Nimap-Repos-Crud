using CrudApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CrudApp.Controllers
{
    public class ListingController : Controller
    {
        private readonly ListingDAL dal;

        public ListingController()
        {
            dal = new ListingDAL();
        }

        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            git init
            int totalRecords = dal.GetTotalProductCount(); 
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var products = dal.GetPagedProducts(page, pageSize); 

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(products);
        }

    }
}
