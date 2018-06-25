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
    public class IngredientsController : Controller
    {
        private ApplicationDbContext _context;
        public IngredientsController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        //End Access Data from Data Base      
        //
        // Edit Ingredients List 
        //
        public ActionResult EditRecipeStep2(int id)
        {
            var recipeIngredients = _context.RecipeIngredientsList.SingleOrDefault(c => c.ingredientsID == id);
            if (recipeIngredients == null)
            {
                return View("Error");
            }

            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == recipeIngredients.recipeID);
            var sessionAuthorID = (int)Session["AuthorID"];
            if ((User.IsInRole("CanManageEverything")) || (sessionAuthorID == recipe.authorID))
            {
                
                var ingredients = _context.IngredientsList.SingleOrDefault(c => c.ingredientsID == id);
                if (ingredients == null)
                {
                    return View("Error");
                }
                var ingredientsIngredientLine = _context.IngredientsIngredientLines.Where(c => c.ingredientsID == ingredients.ingredientsID).ToList();
                var allIngredientLines = _context.IngredientLines.ToList();
                var ingredientLines = from firstItem in allIngredientLines
                                      join secondItem in ingredientsIngredientLine
                                      on firstItem.ingredientLineID equals secondItem.ingredientLineID
                                      select firstItem;

                var ingredientsIEIngredientLine = new IngredientsIEIngredientLine
                {
                    RecipeIngredientsData = recipeIngredients,
                    IngredientsData = ingredients,
                    IngredientLineData = ingredientLines,
                    IngredientsIngredientLineData = ingredientsIngredientLine
                };                
                return View("EditRecipeStep2", ingredientsIEIngredientLine);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateIngredientsInDatabase(IngredientsIEIngredientLine ingredientsIEIngredientLine)
        {
            var ingredientsInDb = _context.IngredientsList.Single(c => c.ingredientsID == ingredientsIEIngredientLine.IngredientsData.ingredientsID);
            ingredientsInDb.ingredientsCategoryTitle = ingredientsIEIngredientLine.IngredientsData.ingredientsCategoryTitle;
            ingredientsInDb.ingredientsDescription = ingredientsIEIngredientLine.IngredientsData.ingredientsDescription;

            var redirectIngredientsID = ingredientsIEIngredientLine.IngredientsData.ingredientsID;

            _context.SaveChanges();
            return RedirectToAction("EditRecipeStep2", "Ingredients", new { id = redirectIngredientsID });
        }
        //
        // Edit ingredient line
        // 
        public ActionResult EditRecipeStep2IngredientLine(int id)
        {
            var ingredientLine = _context.IngredientLines.Single(c => c.ingredientLineID == id);
            if (ingredientLine == null)
            {
                return View("Error");
            }
            var ingredientsId = _context.IngredientsIngredientLines.SingleOrDefault(c => c.ingredientLineID == id).ingredientsID;

            var recipeIngr = _context.RecipeIngredientsList.SingleOrDefault(c => c.ingredientsID == ingredientsId);
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == recipeIngr.recipeID);
            var sessionAuthorID = (int)Session["AuthorID"];
            if ((User.IsInRole("CanManageEverything")) || (sessionAuthorID == recipe.authorID))
            {
                var ingredientLineIngredientsID = new IngredientLineIngredientsID
                {
                    IngredientLineData = ingredientLine,
                    IngredientsIDData = ingredientsId
                };
                return View("EditRecipeStep2IngredientLine", ingredientLineIngredientsID);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateIngredientLineInDatabase(IngredientLineIngredientsID ingredientLineIngredientsID)
        {
            var ingredientLineInDb = _context.IngredientLines.SingleOrDefault(c => c.ingredientLineID == ingredientLineIngredientsID.IngredientLineData.ingredientLineID);
            ingredientLineInDb.itemTitle = ingredientLineIngredientsID.IngredientLineData.itemTitle;
            ingredientLineInDb.itemDescription = ingredientLineIngredientsID.IngredientLineData.itemDescription;
            ingredientLineInDb.baseQuantity = ingredientLineIngredientsID.IngredientLineData.baseQuantity;
            ingredientLineInDb.measurementMetricSystem = ingredientLineIngredientsID.IngredientLineData.measurementMetricSystem;
            ingredientLineInDb.calculatedQuantity = ingredientLineIngredientsID.IngredientLineData.calculatedQuantity;
            ingredientLineInDb.measurementImperialSystem = ingredientLineIngredientsID.IngredientLineData.measurementImperialSystem;
            ingredientLineInDb.unitConverter = ingredientLineIngredientsID.IngredientLineData.unitConverter;
        
            var redirectIngredientLineID = ingredientLineIngredientsID.IngredientLineData.ingredientLineID;

            _context.SaveChanges();
            return RedirectToAction("EditRecipeStep2IngredientLine", "Ingredients", new { id = redirectIngredientLineID });
        }
        //
        // Add ingredients line to recipe ingredient list - For existing recipe
        //
        public ActionResult EditRecipeStep2AddIngredientLine(int id)
        {
            var recipeID = (_context.RecipeIngredientsList.SingleOrDefault(c => c.ingredientsID == id)).recipeID;
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == recipeID);
            var sessionAuthorID = (int)Session["AuthorID"];
            if ((User.IsInRole("CanManageEverything")) || (sessionAuthorID == recipe.authorID))
            {
                var ingredientLineIngredientsIDRecipeID = new IngredientLineIngredientsIDRecipeID
                {
                    IngredientsIDData = id,
                    RecipeIDData = recipeID

                };
                return View("EditRecipeStep2AddIngredientLine", ingredientLineIngredientsIDRecipeID);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }            
        }       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIngredientLineForExistingRecipeInDatabase(IngredientLineIngredientsIDRecipeID ingredientLineIngredientsIDRecipeID)
        {
            int redirectIngredientsID;
            if (!ModelState.IsValid)
            {
                var redirectIngredientLineIngredientsIDRecipeID = new IngredientLineIngredientsIDRecipeID
                {
                    RecipeIDData = ingredientLineIngredientsIDRecipeID.RecipeIDData,
                    IngredientsIDData = ingredientLineIngredientsIDRecipeID.IngredientsIDData,
                    IngredientLineData = ingredientLineIngredientsIDRecipeID.IngredientLineData
                };
                return View("EditRecipeStep2AddIngredientLine", redirectIngredientLineIngredientsIDRecipeID);
            }
            else
            {
                var newIngredientLineInDB = new IngredientLine
                {
                    itemTitle = ingredientLineIngredientsIDRecipeID.IngredientLineData.itemTitle,
                    itemDescription = ingredientLineIngredientsIDRecipeID.IngredientLineData.itemDescription,
                    baseQuantity = ingredientLineIngredientsIDRecipeID.IngredientLineData.baseQuantity,
                    unitConverter = ingredientLineIngredientsIDRecipeID.IngredientLineData.unitConverter,
                    measurementMetricSystem = ingredientLineIngredientsIDRecipeID.IngredientLineData.measurementMetricSystem,
                    measurementImperialSystem = ingredientLineIngredientsIDRecipeID.IngredientLineData.measurementImperialSystem,
                    calculatedQuantity = ingredientLineIngredientsIDRecipeID.IngredientLineData.calculatedQuantity
                };
                _context.IngredientLines.Add(newIngredientLineInDB);
                _context.SaveChanges();

                var newIngredientsIngredientLineInDB = new IngredientsIngredientLine
                {
                    ingredientsID = ingredientLineIngredientsIDRecipeID.IngredientsIDData,
                    ingredientLineID = newIngredientLineInDB.ingredientLineID
                };
                _context.IngredientsIngredientLines.Add(newIngredientsIngredientLineInDB);
                _context.SaveChanges();

                redirectIngredientsID = newIngredientsIngredientLineInDB.ingredientsID;
                return RedirectToAction("EditRecipeStep2", "Ingredients", new { id = redirectIngredientsID });
            }
        }
        //
        // Add ingredients to recipe - Add recipe step 2 
        //
        public ActionResult CreateRecipeStep2(int id)
        {
            var recipeID = id;
            var allRecipeIngredients = _context.RecipeIngredientsList.Where(c => c.recipeID == id);
            var allIngredientList = _context.IngredientsList.ToList();
            var ingredientsData = from firstItem in allIngredientList
                                  join secondItem in allRecipeIngredients
                                  on firstItem.ingredientsID equals secondItem.ingredientsID
                                  select firstItem;
            var recipeIngredientsViewModel = new RecipeIEIngredientsViewModel
            {
                RecipeID = recipeID,
                IngredientsData = ingredientsData
            };
            return View("CreateRecipeStep2", recipeIngredientsViewModel);
        }
        //
        // Create ingredients list 
        //
        public ActionResult CreateIngredientsList(int id)
        {
            var recipeID = id;
            var recipeIngredientsViewModel = new RecipeIngredientsViewModel
            {
                RecipeID = recipeID
            };
            return View("CreateIngredientsList", recipeIngredientsViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIngredientsInDatabase(RecipeIngredientsViewModel recipeIngredientsViewModel)
        {
            int redirectIngredientsID;

            if (!ModelState.IsValid)
            {
                var redirectRecipeIngredientsViewModel = new RecipeIngredientsViewModel
                {
                    RecipeID = recipeIngredientsViewModel.RecipeID,
                    IngredientsData = recipeIngredientsViewModel.IngredientsData
                };
                return View("CreateIngredientsList", redirectRecipeIngredientsViewModel);
            }
            else
            {
                var newIngredientsInDB = new Ingredients
                {
                    ingredientsCategoryTitle = recipeIngredientsViewModel.IngredientsData.ingredientsCategoryTitle,
                    ingredientsDescription = recipeIngredientsViewModel.IngredientsData.ingredientsDescription
                };
                _context.IngredientsList.Add(newIngredientsInDB);
                _context.SaveChanges();

                var newRecipeIngredientsInDB = new RecipeIngredients
                {
                   recipeID = recipeIngredientsViewModel.RecipeID,
                   ingredientsID = newIngredientsInDB.ingredientsID
                };
                _context.RecipeIngredientsList.Add(newRecipeIngredientsInDB);
                _context.SaveChanges();
                redirectIngredientsID = newIngredientsInDB.ingredientsID;
                return RedirectToAction("CreateIngredientLine", "Ingredients", new { id = redirectIngredientsID });
            }
        }
        //
        // Add ingredients lines to recipe ingredient list - Add recipe step 2 
        //
        public ActionResult CreateIngredientLine(int id)
        {
            var recipeID = ( _context.RecipeIngredientsList.SingleOrDefault(c => c.ingredientsID == id) ).recipeID;
            var ingredientLineIngredientsIDRecipeID = new IngredientLineIngredientsIDRecipeID
            {
                IngredientsIDData = id,
                RecipeIDData = recipeID

            };
            return View("CreateIngredientLine", ingredientLineIngredientsIDRecipeID);
        }
        //
        // Create ingredient line
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIngredientLineInDatabase(IngredientLineIngredientsIDRecipeID ingredientLineIngredientsIDRecipeID)
        {
            int redirectIngredientsID;
            if (!ModelState.IsValid)
            {
                var redirectIngredientLineIngredientsIDRecipeID = new IngredientLineIngredientsIDRecipeID
                {
                    RecipeIDData = ingredientLineIngredientsIDRecipeID.RecipeIDData,
                    IngredientsIDData = ingredientLineIngredientsIDRecipeID.IngredientsIDData,
                    IngredientLineData = ingredientLineIngredientsIDRecipeID.IngredientLineData
                };                
                return View("CreateIngredientLine", redirectIngredientLineIngredientsIDRecipeID);
            }
            else
            {
                var newIngredientLineInDB = new IngredientLine
                {
                    itemTitle = ingredientLineIngredientsIDRecipeID.IngredientLineData.itemTitle,
                    itemDescription = ingredientLineIngredientsIDRecipeID.IngredientLineData.itemDescription,
                    baseQuantity = ingredientLineIngredientsIDRecipeID.IngredientLineData.baseQuantity,
                    unitConverter = ingredientLineIngredientsIDRecipeID.IngredientLineData.unitConverter,
                    measurementMetricSystem = ingredientLineIngredientsIDRecipeID.IngredientLineData.measurementMetricSystem,
                    measurementImperialSystem = ingredientLineIngredientsIDRecipeID.IngredientLineData.measurementImperialSystem,
                    calculatedQuantity = ingredientLineIngredientsIDRecipeID.IngredientLineData.calculatedQuantity
                };
                _context.IngredientLines.Add(newIngredientLineInDB);
                _context.SaveChanges();

                var newIngredientsIngredientLineInDB = new IngredientsIngredientLine
                {
                    ingredientsID = ingredientLineIngredientsIDRecipeID.IngredientsIDData,
                    ingredientLineID = newIngredientLineInDB.ingredientLineID
                };
                _context.IngredientsIngredientLines.Add(newIngredientsIngredientLineInDB);
                _context.SaveChanges();

                redirectIngredientsID = newIngredientsIngredientLineInDB.ingredientsID;
                return RedirectToAction("CreateIngredientLine", "Ingredients", new { id = redirectIngredientsID });
            }
        }
        //
        // Delete ingredient line
        // 
        public ActionResult DeleteIngredientLine(bool confirm, int id)
        {
            // if user confirm to delete then this action will fire
            // and you can pass true value. If not, then it is already not confirmed.
            var ingredientLine = _context.IngredientLines.Single(c => c.ingredientLineID == id);
            if (ingredientLine == null)
            {
                return View("Error");
            }
            var ingredientsId = _context.IngredientsIngredientLines.SingleOrDefault(c => c.ingredientLineID == id).ingredientsID;

            var recipeIngr = _context.RecipeIngredientsList.SingleOrDefault(c => c.ingredientsID == ingredientsId);
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == recipeIngr.recipeID);
            var sessionAuthorID = (int)Session["AuthorID"];
            if ((User.IsInRole("CanManageEverything")) || (sessionAuthorID == recipe.authorID))
            {
                var ingredientsIngredientLines = _context.IngredientsIngredientLines.SingleOrDefault(c => c.ingredientLineID == ingredientLine.ingredientLineID);
                _context.IngredientsIngredientLines.Remove(ingredientsIngredientLines);
                _context.IngredientLines.Remove(ingredientLine);
                _context.SaveChanges();
                return RedirectToAction("EditRecipeStep2", "Ingredients", new { id = ingredientsId });                
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //
        // Delete ingredients list
        // 
        public ActionResult DeleteIngredientsInExistingRecipeList(bool confirm, int id)
        {
            // if user confirm to delete then this action will fire
            // and you can pass true value. If not, then it is already not confirmed.
            var ingredientsList = _context.IngredientsList.Single(c => c.ingredientsID == id);
            if (ingredientsList == null)
            {
                return View("Error");
            }
            var recipeIngr = _context.RecipeIngredientsList.SingleOrDefault(c => c.ingredientsID == ingredientsList.ingredientsID);
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == recipeIngr.recipeID);
            var sessionAuthorID = (int)Session["AuthorID"];
            if ((User.IsInRole("CanManageEverything")) || (sessionAuthorID == recipe.authorID))
            {
                var recipeIngredientsList = _context.RecipeIngredientsList.SingleOrDefault(c => c.recipeID == id); 
                var ingredientsIngredientLines = _context.IngredientsIngredientLines.Where(c => c.ingredientsID == ingredientsList.ingredientsID).ToList();
                foreach (var currentIngredientsIngredientLines in ingredientsIngredientLines)
                {
                    var ingredientLine = _context.IngredientLines.SingleOrDefault(c => c.ingredientLineID == currentIngredientsIngredientLines.ingredientLineID);
                    if (ingredientLine != null)
                    {
                        _context.IngredientLines.Remove(ingredientLine);
                    }
                    _context.IngredientsIngredientLines.Remove(currentIngredientsIngredientLines);
                }                        
                _context.RecipeIngredientsList.Remove(recipeIngredientsList);
                _context.IngredientsList.Remove(ingredientsList);
                _context.SaveChanges();
                return RedirectToAction("EditRecipeStep1", "Recipe", new { id = recipe.recipeID });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}