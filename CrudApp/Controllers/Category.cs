using CrudApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq; // Required for Any() method

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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            if (_categoryDal.GetAllCategories().Any(c => c.CategoryName.ToLower() == category.CategoryName.ToLower()))
            {
                ModelState.AddModelError("CategoryName", "Category name already exists.");
                return View(category);
            }

            _categoryDal.AddCategory(category);
            return RedirectToAction("Index");
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
            var category = _categoryDal.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int CategoryId)
        {
            _categoryDal.DeleteCategory(CategoryId);
            return RedirectToAction("Index");
        }


    }
}
