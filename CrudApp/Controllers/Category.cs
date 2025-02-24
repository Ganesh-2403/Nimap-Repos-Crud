using CrudApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CrudApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryDAL _categoryDal;

        public CategoryController()
        {
            _categoryDal = new CategoryDAL();
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _categoryDal.GetAllCategories();
            return View(categories);
        }
 
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            try
            {
                Console.WriteLine("Category received: " + (category?.CategoryName ?? "NULL"));
                _categoryDal.AddCategory(category);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                ViewBag.ErrorMessage = ex.Message;
                return View(category);
            }
        }

        public IActionResult Edit(int id)
        {
            Category category = _categoryDal.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _categoryDal.UpdateCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public IActionResult Delete(int id)
        {
            Category category = _categoryDal.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _categoryDal.DeleteCategory(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
