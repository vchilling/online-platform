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
    public class TipController : Controller
    {
        private ApplicationDbContext _context;
        public TipController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        //End Access Data from Data Base
        // GET: Direction list 
        //
        //
        // Add Tip for new recipe
        //
        public ActionResult CreateRecipeAddTip(int id)
        {
            var recipeID = id;
            var recipeTip = new RecipeTip
            {
                RecipeID = recipeID
            };

            return View("CreateRecipeAddTip", recipeTip);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTipForNewRecipeInDatabase(RecipeTip recipeTip)
        {
            if (!ModelState.IsValid)
            {
                var redirectRecipeTip = new RecipeTip
                {
                    RecipeID = recipeTip.RecipeID,
                    TipData = recipeTip.TipData
                };
                return View("CreateRecipeAddTip", redirectRecipeTip);
            }
            var newTip = new Tip
            {
                tipID = recipeTip.TipData.tipID,
                tipTitle = recipeTip.TipData.tipTitle,
                tipDescription = recipeTip.TipData.tipDescription,
                recipeID = recipeTip.RecipeID
            };
            _context.Tips.Add(newTip);
            _context.SaveChanges();
            return RedirectToAction("CreateRecipeStep4", "Recipe", new { id = recipeTip.RecipeID });

        }
        //
        // Edit tip for new recipe
        public ActionResult CreateRecipeEditTip(int id)
        {
            var tip = _context.Tips.SingleOrDefault(c => c.tipID == id);

            if (tip == null)
            {
                return View("Error");
            }
            return View("CreateRecipeEditTip", tip);
        }
        public ActionResult UpdateTipForNewRecipeInDatabase(Tip tip)
        {

            var tipInDb = _context.Tips.Single(c => c.tipID == tip.tipID);
            tipInDb.tipTitle = tip.tipTitle;
            tipInDb.tipDescription = tip.tipDescription;

            if (!ModelState.IsValid)
            {

                return View("CreateRecipeAddTip", tipInDb);
            }

            var redirectTipID = tip.tipID;
            _context.SaveChanges();
            return RedirectToAction("CreateRecipeStep4", "Recipe", new { id = tipInDb.recipeID });
        }
        //
        // Edit tip of existing recipe
        //
        public ActionResult EditRecipeStep4(int id)
        {
            var tip = _context.Tips.SingleOrDefault(c => c.tipID == id);  
            if (tip == null)
            {
                return View("Error");
            }              
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == tip.recipeID);
            var sessionAuthorID = (int)Session["AuthorID"];
            if ((User.IsInRole("CanManageEverything")) || (sessionAuthorID == recipe.authorID))
            {
                return View("EditRecipeStep4", tip);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            } 
        }
        public ActionResult UpdateTipInDatabase(Tip tip)
        {
            var tipInDb = _context.Tips.Single(c => c.tipID == tip.tipID);
            tipInDb.tipTitle = tip.tipTitle;
            tipInDb.tipDescription = tip.tipDescription; 
            _context.SaveChanges();
            var redirectRecipeID = tipInDb.recipeID;
            return RedirectToAction("EditRecipeStep1", "Recipe", new { id = redirectRecipeID });
        }
        //
        // Add Tip 
        //
        public ActionResult CreateTip (int id)
        {
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == id);
            if (recipe == null)
            {
                return View("Error");
            }
            var recipeTip = new RecipeTip
            {
                RecipeID = id
            };
            
            var sessionAuthorID = (int)Session["AuthorID"];
            if ((User.IsInRole("CanManageEverything")) || (sessionAuthorID == recipe.authorID))
            {
                return View("CreateTip", recipeTip);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTipInDatabase(RecipeTip recipeTip)
        {
            var redirectRecipeID = recipeTip.RecipeID;
            var newTip = new Tip
            {
                tipID = recipeTip.TipData.tipID,
                tipTitle = recipeTip.TipData.tipTitle,
                tipDescription = recipeTip.TipData.tipDescription,
                recipeID = redirectRecipeID
            };
            recipeTip.TipData = newTip;             

            if (!ModelState.IsValid)
            {       
                return View("CreateTip", recipeTip);
            }
            else
            {                   
                _context.Tips.Add(newTip);
                _context.SaveChanges();
                return RedirectToAction("EditRecipeStep1", "Recipe", new { id = redirectRecipeID });
            }
        }
        //
        // Delete tip for existing recipe
        // 
        public ActionResult DeleteTipForExistingRecipe(bool confirm, int id)
        {
            // if user confirm to delete then this action will fire
            // and you can pass true value. If not, then it is already not confirmed.
            var tip = _context.Tips.Single(c => c.tipID == id);
            if (tip == null)
            {
                return View("Error");
            }           
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == tip.recipeID);
            var sessionAuthorID = (int)Session["AuthorID"];
            if ((User.IsInRole("CanManageEverything")) || (sessionAuthorID == recipe.authorID))
            {                
                _context.Tips.Remove(tip);
                _context.SaveChanges();
                return RedirectToAction("EditRecipeStep1", "Recipe", new { id = recipe.recipeID });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //
        // Delete tip for existing recipe
        // 
        public ActionResult DeleteTip(bool confirm, int id)
        {
            // if user confirm to delete then this action will fire
            // and you can pass true value. If not, then it is already not confirmed.
            var tip = _context.Tips.Single(c => c.tipID == id);
            if (tip == null)
            {
                return View("Error");
            }
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == tip.recipeID);
            var sessionAuthorID = (int)Session["AuthorID"];
            if ((User.IsInRole("CanManageEverything")) || (sessionAuthorID == recipe.authorID))
            {
                _context.Tips.Remove(tip);
                _context.SaveChanges();
                return RedirectToAction("CreateRecipeStep4", "Recipe", new { id = recipe.recipeID });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}