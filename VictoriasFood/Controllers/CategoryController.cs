using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using VictoriasFood.Models;
using VictoriasFood.ViewModels;

namespace VictoriasFood.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        // Begin Access Data from Data Base
        private ApplicationDbContext _context;
        public CategoryController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        //End Access Data from Data Base
        //
        // GET: Category
        //
        [Authorize(Roles = "CanManageEverything")]
        public ActionResult Index()
        {
            var categoryList = _context.Categories.ToList();
            return View(categoryList);
        }
        [Authorize(Roles = "CanManageEverything")]
        public ActionResult DetailsCategory(int id)
        {
            var category = _context.Categories.SingleOrDefault(c => c.categoryID == id);
            var subcategories = _context.Subcategories.Where(c => c.categoryID == id).ToList();

            if (category == null)
            {
                //return HttpNotFound();
                return View("Error");
            }
            var categoryIESubcategory = new CategoryIESubcategory
            {
                CategoryData = category,
                SubcategoryData = subcategories
            };
            //for delete button
            if (subcategories.Count != 0)
            {
                ViewBag.CanDeleteStatus = false;
            }
            else
            {
                ViewBag.CanDeleteStatus = true;
            }

            return View(categoryIESubcategory);
        }
        //
        // Delete category
        //        
        [Authorize(Roles = "CanManageEverything")]
        public ActionResult DeleteCategory(bool confirm, int id)
        {
            // if user confirm to delete then this action will fire
            // and you can pass true value. If not, then it is already not confirmed.
            var category = _context.Categories.SingleOrDefault(c => c.categoryID == id);
            if (category == null)
            {
                return View("Error");
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("Index", "Category");

        }
        [Authorize(Roles = "CanManageEverything")]
        public ActionResult EditCategory(int id)
        {
            var category = _context.Categories.SingleOrDefault(c => c.categoryID == id);
            if (category == null)
            {
                return View("Error");
            }
            return View("EditCategory", category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCategoryInDatabase(Category category)
        {
            var categoryInDb = _context.Categories.Single(c => c.categoryID == category.categoryID);
            categoryInDb.categoryTitle = category.categoryTitle;
            categoryInDb.categoryDescription = category.categoryDescription;

            if (!ModelState.IsValid)
            {                
                return View("EditCategory", categoryInDb);
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Category");
        }
        [Authorize(Roles = "CanManageEverything")]
        public ActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategoryInDatabase(Category newCategoryInDB)
        {
            if (!ModelState.IsValid)
            {
                var newCategory = new Category
                {
                    categoryID = newCategoryInDB.categoryID,
                    categoryTitle = newCategoryInDB.categoryTitle,
                    categoryDescription = newCategoryInDB.categoryDescription

                };
                return View("CreateCategory", newCategory);
            }
            _context.Categories.Add(newCategoryInDB);
            _context.SaveChanges();
            return RedirectToAction("Index", "Category");
        }
        //
        // GET: All Categories
        //
        [AllowAnonymous]
        public ActionResult AllCategories()
        {
            var categoryList = _context.Categories.ToList();
            return View(categoryList);
        }
        //
        // GET: All Subcategories
        //
        [AllowAnonymous]
        public ActionResult AllSubcategories(int id)
        {
            var category = _context.Categories.SingleOrDefault(c => c.categoryID == id);
            var subcategories = _context.Subcategories.Where(c => c.categoryID == id);

            if (category == null)
            {
                return View("Error");
            }
            var categoryIESubcategory = new CategoryIESubcategory
            {
                CategoryData = category,
                SubcategoryData = subcategories.ToList()
            };
            return View(categoryIESubcategory);
        }
        //
        // GET: Subcategory
        //
        [Authorize(Roles = "CanManageEverything")]
        public ActionResult IndexSubcategory()
        {
            var subcategoryList = _context.Subcategories.Include(c => c.Categories).ToList();
            return View(subcategoryList);
        }
        [Authorize(Roles = "CanManageEverything")]
        public ActionResult DetailsSubcategory(int id)
        {
            var subcategory = _context.Subcategories.SingleOrDefault(c => c.subcategoryID == id);
            var category = _context.Categories.SingleOrDefault(c => c.categoryID == subcategory.categoryID);

            if (subcategory == null)
            {
                return View("Error");

            }
            var categorySubcategory = new CategorySubcategory
            {
                SubcategoryData = subcategory,
                CategoryData = category
            };
            //for delete button
            var recipes = _context.Recipes.Where(c => c.subcategoryID == id).ToList();
            if (recipes.Count != 0)
            {
                ViewBag.CanDeleteStatus = false;
            }
            else {
                ViewBag.CanDeleteStatus = true;
            }

            return View(categorySubcategory);
        }
        //
        // Delete subcategory
        //        
        [Authorize(Roles = "CanManageEverything")]
        public ActionResult DeleteSubcategory(bool confirm, int id)
        {
            // if user confirm to delete then this action will fire
            // and you can pass true value. If not, then it is already not confirmed.
            
            var subcategory = _context.Subcategories.SingleOrDefault(c => c.subcategoryID == id);
            if (subcategory == null)
            {
                return View("Error");
            }
            
            _context.Subcategories.Remove(subcategory);
            _context.SaveChanges();

            return RedirectToAction("IndexSubcategory", "Category");
            
        }
        [Authorize(Roles = "CanManageEverything")]
        public ActionResult EditSubcategory(int id)
        {
            var subcategory = _context.Subcategories.SingleOrDefault(c => c.subcategoryID == id);
            if (subcategory == null)
            {
                return View("Error");

            }
            var subcategoryIECategories = new SubcategoryIECategories
            {
                SubcategoryData = subcategory,
                CategoriesData = _context.Categories.ToList()
            };  
            return View("EditSubcategory", subcategoryIECategories);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSubcategoryInDatabase(SubcategoryIECategories subcategoryIECategories)
        {
            var subcategoryInDb = _context.Subcategories.Single(c => c.subcategoryID == subcategoryIECategories.SubcategoryData.subcategoryID);
            subcategoryInDb.subcategoryTitle = subcategoryIECategories.SubcategoryData.subcategoryTitle;
            subcategoryInDb.subcategoryDescription = subcategoryIECategories.SubcategoryData.subcategoryDescription;
            subcategoryInDb.categoryID = subcategoryIECategories.SubcategoryData.categoryID;

            if (String.IsNullOrEmpty(subcategoryInDb.subcategoryTitle) || (subcategoryInDb.categoryID == 0))
            {
                var newSubcategoryIECategories = new SubcategoryIECategories
                {
                    CategoriesData = _context.Categories.ToList(),
                    SubcategoryData = subcategoryInDb
                };
                return View("EditSubcategory", newSubcategoryIECategories);
            }
            _context.SaveChanges();
            return RedirectToAction("IndexSubcategory", "Category");
        }
        [Authorize(Roles = "CanManageEverything")]
        public ActionResult CreateSubcategory()
        {
            var subcategoryIECategories = new SubcategoryIECategories
            {
                CategoriesData = _context.Categories.ToList()
            };
            return View("CreateSubcategory", subcategoryIECategories);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSubcategoryInDatabase(SubcategoryIECategories newSubcategoryIECategoriesInDB)
        {
            var newSubcategoryInDB = new Subcategory
            {
                subcategoryTitle = newSubcategoryIECategoriesInDB.SubcategoryData.subcategoryTitle,
                subcategoryDescription = newSubcategoryIECategoriesInDB.SubcategoryData.subcategoryDescription,
                categoryID = newSubcategoryIECategoriesInDB.SubcategoryData.categoryID
            };

            if (String.IsNullOrEmpty(newSubcategoryIECategoriesInDB.SubcategoryData.subcategoryTitle) || (newSubcategoryIECategoriesInDB.SubcategoryData.categoryID == 0))
            {                
                var newSubcategoryIECategories = new SubcategoryIECategories
                {
                    CategoriesData = _context.Categories.ToList(),
                    SubcategoryData = newSubcategoryInDB
                };
                return View("CreateSubcategory", newSubcategoryIECategories);
            }

            _context.Subcategories.Add(newSubcategoryInDB);
            _context.SaveChanges();
            return RedirectToAction("IndexSubcategory", "Category");
        }

    }
}