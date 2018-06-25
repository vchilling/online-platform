using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VictoriasFood.Models;
using VictoriasFood.ViewModels;


namespace VictoriasFood.Controllers
{
    [Authorize]    
    public class SuitableForController : Controller
    {
        // Begin Access Data from Data Base
        private ApplicationDbContext _context;
        public SuitableForController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        //End Access Data from Data Base
        // GET: Suitable for
        [Authorize(Roles = "CanManageEverything")]
        public ActionResult Index()
        {
            var suitableForList = _context.SuitableForCategories.ToList();
            return View(suitableForList);
        }
        [Authorize(Roles = "CanManageEverything")]
        public ActionResult DetailsSuitableFor(int id)
        {
            var suitableFor = _context.SuitableForCategories.SingleOrDefault(c => c.suitableForID == id);
            if (suitableFor == null)
            {
                return View("Error");
            }
            return View(suitableFor);
        }
        //
        // Delete suitable for category
        //        
        [Authorize(Roles = "CanManageEverything")]
        public ActionResult DeleteSuitableFor(bool confirm, int id)
        {
            // if user confirm to delete then this action will fire
            // and you can pass true value. If not, then it is already not confirmed.
            var suitableFor = _context.SuitableForCategories.SingleOrDefault(c => c.suitableForID == id);
            if (suitableFor == null)
            {
                return View("Error");
            }
            var recipeSuitableForCategories = _context.RecipeSuitableForCategories.Where(c => c.suitableForID == id).ToList();
            if (recipeSuitableForCategories.Any())
            {
                foreach (var current in recipeSuitableForCategories)
                {
                    _context.RecipeSuitableForCategories.Remove(current);
                }
            }
            _context.SuitableForCategories.Remove(suitableFor);
            _context.SaveChanges();

            return RedirectToAction("Index", "SuitableFor");

        }
        [Authorize(Roles = "CanManageEverything")]
        public ActionResult EditSuitableFor(int id)
        {
            var suitableFor = _context.SuitableForCategories.SingleOrDefault(c => c.suitableForID == id);
            if (suitableFor == null)
            {
                return View("Error");
            }
            return View("EditSuitableFor", suitableFor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSuitableForInDatabase(SuitableFor suitableFor)
        {
            var suitableForInDb = _context.SuitableForCategories.Single(c => c.suitableForID == suitableFor.suitableForID);
            suitableForInDb.suitableForTitle = suitableFor.suitableForTitle;
            suitableForInDb.suitableForDescription = suitableFor.suitableForDescription;

            if (!ModelState.IsValid)
            {
                return View("EditSuitableFor", suitableForInDb);
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "SuitableFor");
        }
        [Authorize(Roles = "CanManageEverything")]
        public ActionResult CreateSuitableFor()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSuitableForInDatabase(SuitableFor newSuitableForInDB)
        {
            if (!ModelState.IsValid)
            {
                var newSuitableFor = new SuitableFor
                {
                    suitableForID = newSuitableForInDB.suitableForID,
                    suitableForTitle = newSuitableForInDB.suitableForTitle,
                    suitableForDescription = newSuitableForInDB.suitableForDescription

                };
                return View("CreateSuitableFor", newSuitableFor);
            }
            _context.SuitableForCategories.Add(newSuitableForInDB);
            _context.SaveChanges();
            return RedirectToAction("Index", "SuitableFor");
        }
        //
        // GET: All Suitable For Categories
        //
        [AllowAnonymous]
        public ActionResult AllSuitableForCategories()
        {
            var suitableForList = _context.SuitableForCategories.ToList();
            return View(suitableForList);
        }
        //
        // Add new recipe to suitable for category
        //
        public ActionResult CreateRecipeAddToSuitableFor(int id)
        {
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == id);
            if (recipe == null)
            {
                return View("Error");
            }
            var suitableForCategories = _context.SuitableForCategories.ToList();
            var recipeIESuitableForCategories = new RecipeIESuitableForCategories
            {
                RecipeID = id,
                SuitableForData = suitableForCategories
            };

            return View("CreateRecipeAddToSuitableFor", recipeIESuitableForCategories);
        }

        public ActionResult CreateNewRecipeSuitableForInDatabase(int[] ids)
        {
            var recipeId = ids[0];

            if (ids.Length <= 1)
            {
                var recipeIESuitableForRedirect = new RecipeIESuitableForCategories();
                recipeIESuitableForRedirect.RecipeID = ids[0];
                recipeIESuitableForRedirect.SuitableForData = _context.SuitableForCategories.ToList();
                return Json(new { success = true, responseText = "There's no any choosen suitable for category" }, JsonRequestBehavior.AllowGet);                
            }
            else
            {
                foreach (var selected in ids.Skip(1))
                {
                    var newRecipeSuitableForInDb = new RecipeSuitableFor();
                    newRecipeSuitableForInDb.recipeID = recipeId;
                    newRecipeSuitableForInDb.suitableForID = selected;
                    _context.RecipeSuitableForCategories.Add(newRecipeSuitableForInDb);
                    _context.SaveChanges();
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }
        //
        // Edit suitable for category for new recipe
        //
        public ActionResult CreateRecipeEditSuitableFor(int id)
        {
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == id);
            if (recipe == null)
            {
                return View("Error");
            }
            var suitableForCategories = _context.SuitableForCategories.ToList();
            var allRecipeSuitableFor = _context.RecipeSuitableForCategories.Where(c => c.recipeID == id);
            List<int> oldSuitableFor = new List<int>();
            foreach (var suitableFor in allRecipeSuitableFor)
            {
                oldSuitableFor.Add(suitableFor.suitableForID);
            }
            var recipeIESuitableForCategories = new RecipeIESuitableForCategories
            {
                RecipeID = id,
                SuitableForData = suitableForCategories,
                OldRecipeSuitableForData = oldSuitableFor
            };

            return View("CreateRecipeEditSuitableFor", recipeIESuitableForCategories);
        }
        public ActionResult UpdateNewRecipeSuitableForInDatabase(int[] ids)
        {
            var recipeId = ids[0];
            var allRecipeSuitableFor = _context.RecipeSuitableForCategories.Where(c => c.recipeID == recipeId).ToList();
            List<int> oldSuitableFor = new List<int>();
            foreach (var suitableFor in allRecipeSuitableFor)
            {
                oldSuitableFor.Add(suitableFor.suitableForID);
            }
            var newSuitableFor = ids.Skip(1);
            foreach (var selected in newSuitableFor)
            {
                if (!oldSuitableFor.Contains(selected))
                {
                    var newRecipeSuitableForInDb = new RecipeSuitableFor();
                    newRecipeSuitableForInDb.recipeID = recipeId;
                    newRecipeSuitableForInDb.suitableForID = selected;
                    _context.RecipeSuitableForCategories.Add(newRecipeSuitableForInDb);
                    _context.SaveChanges();
                }
            }
            foreach (var old in oldSuitableFor)
            {
                if (!newSuitableFor.Contains(old))
                {
                    var removeRecipeSuitableForInDb = _context.RecipeSuitableForCategories.Where(c => c.suitableForID == old).Single(c => c.recipeID == recipeId);
                    _context.RecipeSuitableForCategories.Remove(removeRecipeSuitableForInDb);
                    _context.SaveChanges();
                }
            }     
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        // Add recipe to suitable for category for existing recipe
        //
        public ActionResult AddRecipeToSuitableFor(int id)
        {
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == id);
            if (recipe == null)
            {
                return View("Error");
            }
            var sessionAuthorID = (int)Session["AuthorID"];
            if ((User.IsInRole("CanManageEverything")) || (sessionAuthorID == recipe.authorID))
            {
                var suitableForCategories = _context.SuitableForCategories.ToList();
                var recipeIESuitableForCategories = new RecipeIESuitableForCategories
                {
                    RecipeID = id,
                    SuitableForData = suitableForCategories
                };
                return View("AddRecipeToSuitableFor", recipeIESuitableForCategories);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }       
        public ActionResult CreateRecipeSuitableForInDatabase(int[] ids)
        {
            var recipeId = ids[0];

            if (ids.Length <= 1)
            {
                var recipeIESuitableForRedirect = new RecipeIESuitableForCategories();
                recipeIESuitableForRedirect.RecipeID = ids[0];
                recipeIESuitableForRedirect.SuitableForData = _context.SuitableForCategories.ToList();
                return Json(new { success = true, responseText = "There's no any choosen suitable for category" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                foreach (var selected in ids.Skip(1))
                {
                    var newRecipeSuitableForInDb = new RecipeSuitableFor();
                    newRecipeSuitableForInDb.recipeID = recipeId;
                    newRecipeSuitableForInDb.suitableForID = selected;
                    _context.RecipeSuitableForCategories.Add(newRecipeSuitableForInDb);
                    _context.SaveChanges();
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);                
            }
        }
        //
        // Edit recipe suitable for category for existing recipe
        //
        public ActionResult EditRecipeStep5(int id)
        {
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == id);
            if (recipe == null)
            {
                return View("Error");
            }
            var sessionAuthorID = (int)Session["AuthorID"];
            if ((User.IsInRole("CanManageEverything")) || (sessionAuthorID == recipe.authorID))
            {
                var suitableForCategories = _context.SuitableForCategories.ToList();
                var allRecipeSuitableFor = _context.RecipeSuitableForCategories.Where(c => c.recipeID == id).ToList();
                List<int> oldSuitableFor = new List<int>();
                foreach (var suitableFor in allRecipeSuitableFor)
                {
                    oldSuitableFor.Add(suitableFor.suitableForID);
                }
                var recipeIESuitableForCategories = new RecipeIESuitableForCategories
                {
                    RecipeID = id,
                    SuitableForData = suitableForCategories,
                    OldRecipeSuitableForData = oldSuitableFor
                };
                return View("EditRecipeStep5", recipeIESuitableForCategories);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult UpdateRecipeSuitableForInDatabase (int[] ids)
        {
            var recipeId = ids[0];          
            var allRecipeSuitableFor = _context.RecipeSuitableForCategories.Where(c => c.recipeID == recipeId).ToList();
            List<int> oldSuitableFor = new List<int>();
            foreach (var suitableFor in allRecipeSuitableFor)
            {
                oldSuitableFor.Add(suitableFor.suitableForID);
            }
            var newSuitableFor = ids.Skip(1);
            foreach (var selected in newSuitableFor)
            {
                if (!oldSuitableFor.Contains(selected)) {
                    var newRecipeSuitableForInDb = new RecipeSuitableFor();
                    newRecipeSuitableForInDb.recipeID = recipeId;
                    newRecipeSuitableForInDb.suitableForID = selected;
                    _context.RecipeSuitableForCategories.Add(newRecipeSuitableForInDb);
                    _context.SaveChanges();
                }                   
            }
            foreach (var old in oldSuitableFor)
            {
                if (!newSuitableFor.Contains(old))
                {
                    var removeRecipeSuitableForInDb = _context.RecipeSuitableForCategories.Where(c => c.suitableForID == old).Single(c => c.recipeID == recipeId);
                    _context.RecipeSuitableForCategories.Remove(removeRecipeSuitableForInDb);
                    _context.SaveChanges();   
                }                 
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}