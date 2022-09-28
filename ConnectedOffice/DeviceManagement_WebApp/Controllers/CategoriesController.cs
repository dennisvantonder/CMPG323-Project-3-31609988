using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeviceManagement_WebApp.Data;
using DeviceManagement_WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using DeviceManagement_WebApp.Repository;

namespace DeviceManagement_WebApp.Controllers
{
    // Authorize key word used to implement security on controller
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: Categories - returns all items from the catgories table
        public IActionResult Index()
        {
            return View(_categoryRepository.GetAll());
        }

        // GET: Categories/Details/5 - returns 1 category
        public IActionResult Details(Guid id)
        {
            var category = GetCategory(id);
            if (category == null)
            {
                return RedirectToAction(nameof(Error));
            }

            return View(category);
        }

        // GET: Categories/Create - opens create view to add a new category
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create - Creates new category from user input
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CategoryId,CategoryName,CategoryDescription,DateCreated")] Category category)
        {
            category.CategoryId = Guid.NewGuid();
            _categoryRepository.Add(category);
            _categoryRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET: Categories/Edit/5 - returns edit view to user to edit a category
        public IActionResult Edit(Guid id)
        {
            var category = GetCategory(id);
            if (category == null)
            {
                return RedirectToAction(nameof(Error));
            }

            return View(category);
        }

        // POST: Categories/Edit/5 - updates a category
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("CategoryId,CategoryName,CategoryDescription,DateCreated")] Category category)
        {
            if (id != category.CategoryId)
            {
                return RedirectToAction(nameof(Error));
            }
            try
            {
                _categoryRepository.Update(category);
                _categoryRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.CategoryId))
                {
                    return RedirectToAction(nameof(Error));
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Categories/Delete/5 - returns delete view to user to delete a category
        public IActionResult Delete(Guid id)
        {
            var category = GetCategory(id);
            if (category == null)
            {
                return RedirectToAction(nameof(Error));
            }

            return View(category);
        }

        // POST: Categories/Delete/5 - delete a category
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var category = _categoryRepository.GetById(id);
            _categoryRepository.Remove(category);
            _categoryRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        // Method checks if a category exists
        private bool CategoryExists(Guid id)
        {
            return _categoryRepository.Exists(e => e.CategoryId == id);
        }

        // Method is used to get a category by id (used method to simplify code and for DRY principle)
        private Category GetCategory(Guid id)
        {
            var cat = _categoryRepository.GetById(id);
            if (id == null)
                return null;
            else if (cat == null)
                return null;
            else
                return cat;
        }

        // Sorts the categories in ascending order
        public IActionResult Sort()
        {
            return View(_categoryRepository.Sort(e => e.CategoryName));
        }

        // Gets the most recent item of categories by looking at the dat created and returns a new view
        public IActionResult GetRecent()
        {
            return View(_categoryRepository.GetMostRecentCategory());
        }

        // opens search page
        public IActionResult Search()
        {
            return View();
        }

        // opens error not found page
        public IActionResult Error()
        {
            return View();
        }

        // returns a category from a category name else not found
        public IActionResult SearchView([Bind("CategoryName")] Category category)
        {
            var c = _categoryRepository.Find(e => e.CategoryName == category.CategoryName);
            if (c == null)
            {
                return RedirectToAction(nameof(Error));
            }
            return View(c);
        }
    }
}
