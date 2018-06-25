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
    public class DirectionController : Controller
    {
        private ApplicationDbContext _context;
        public DirectionController()
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
        // Edit Direction List 
        //
        public ActionResult EditRecipeStep3(int id)
        {
            var direction = _context.DirectionList.SingleOrDefault(c => c.directionID == id);
            if (direction == null)
            {
                return View("Error");
            }
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == direction.recipeID);
            var sessionAuthorID = (int)Session["AuthorID"];
            if ((User.IsInRole("CanManageEverything")) || (sessionAuthorID == recipe.authorID))
            {
                var directionLines = _context.DirectionLines.Where(c => c.directionID == id);
                var directionIEDirectionLine = new DirectionIEDirectionLine
                {
                    DirectionData = direction,
                    DirectionLineData = directionLines
                };            
                return View("EditRecipeStep3", directionIEDirectionLine);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDirectionInDatabase(DirectionIEDirectionLine directionIEDirectionLine)
        {
            var directionInDb = _context.DirectionList.Single(c => c.directionID == directionIEDirectionLine.DirectionData.directionID);
            directionInDb.directionTitle = directionIEDirectionLine.DirectionData.directionTitle;
            directionInDb.directionDescription = directionIEDirectionLine.DirectionData.directionDescription;

            var redirectDirectionID = directionIEDirectionLine.DirectionData.directionID;

            _context.SaveChanges();
            return RedirectToAction("EditRecipeStep3", "Direction", new { id = redirectDirectionID });
        }      
        public ActionResult EditRecipeStep3DirectionLine(int id)
        {
            var directionLine = _context.DirectionLines.Single(c => c.directionLineID == id);
            if (directionLine == null)
            {
                return View("Error");
            }
            var direction = _context.DirectionList.SingleOrDefault(c => c.directionID == directionLine.directionID);
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == direction.recipeID);
            var sessionAuthorID = (int)Session["AuthorID"];
            if ((User.IsInRole("CanManageEverything")) || (sessionAuthorID == recipe.authorID))
            {
                return View("EditRecipeStep3DirectionLine", directionLine);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDirectionLineInDatabase(DirectionLine directionLine)
        {
            var directionLineInDb = _context.DirectionLines.SingleOrDefault(c => c.directionLineID == directionLine.directionLineID);
            directionLineInDb.directionLineText = directionLine.directionLineText;
            directionLineInDb.directionLineDescription = directionLine.directionLineDescription;

            var redirectDirectionLineId = directionLine.directionLineID;

            _context.SaveChanges();
            return RedirectToAction("EditRecipeStep3DirectionLine", "Direction", new { id = redirectDirectionLineId });
        }
        //
        // Create direction lines for existing recipe
        //
        public ActionResult EditRecipeStep3AddDirectionLine(int id)
        {
            var direction = _context.DirectionList.SingleOrDefault(c => c.directionID == id);
            var directionLine = new DirectionLine();
            directionLine.directionID = id;
            var directionDirectionLine = new DirectionDirectionLine
            {
                DirectionData = direction,
                DirectionLineData = directionLine
            };
            return View("EditRecipeStep3AddDirectionLine", directionDirectionLine);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDirectionLineForExistingRecipeInDatabase(DirectionDirectionLine directionDirectionLine)
        {
            var redirectDirectionDirectionLine = new DirectionDirectionLine
            {
                DirectionData = directionDirectionLine.DirectionData,
                DirectionLineData = directionDirectionLine.DirectionLineData
            };
            if (!ModelState.IsValid)
            {
                return View("EditRecipeStep3AddDirectionLine", redirectDirectionDirectionLine);
            }
            else
            {
                var newDirectionLineInDB = new DirectionLine
                {
                    directionID = directionDirectionLine.DirectionData.directionID,
                    directionLineDescription = directionDirectionLine.DirectionLineData.directionLineDescription,
                    directionLineText = directionDirectionLine.DirectionLineData.directionLineText
                };
                _context.DirectionLines.Add(newDirectionLineInDB);
                _context.SaveChanges();

                return RedirectToAction("EditRecipeStep3", "Direction", new { id = newDirectionLineInDB.directionID });
            }
        }
        //
        // Add direction
        //
        public ActionResult CreateRecipeStep3(int id)
        {
            var direction = new Direction
            {
                recipeID = id
            };
            return View("CreateRecipeStep3", direction);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDirectionInDatabase(Direction direction)
        {
            int redirectDirectionID;
            var newDirection = new Direction
            {
                directionID = direction.directionID,
                directionTitle = direction.directionTitle,
                directionDescription = direction.directionDescription,
                recipeID = direction.recipeID
            };            

            if (!ModelState.IsValid)
            {               
                return View("CreateRecipeStep3", newDirection);
            }
            else
            {
                _context.DirectionList.Add(newDirection);
                _context.SaveChanges();
                redirectDirectionID = newDirection.directionID;
                return RedirectToAction("CreateDirectionLine", "Direction", new { id = redirectDirectionID });
            }
        }
        //
        // Create direction lines
        //
        public ActionResult CreateDirectionLine(int id)
        {
            var direction = _context.DirectionList.SingleOrDefault(c => c.directionID == id);
            var directionLine = new DirectionLine();
            directionLine.directionID = id;
            var directionDirectionLine = new DirectionDirectionLine
            {
                DirectionData = direction,
                DirectionLineData = directionLine
            };
            return View("CreateDirectionLine", directionDirectionLine);            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDirectionLineInDatabase(DirectionDirectionLine directionDirectionLine)
        {
            var redirectDirectionDirectionLine = new DirectionDirectionLine
            {
                DirectionData = directionDirectionLine.DirectionData,
                DirectionLineData = directionDirectionLine.DirectionLineData
            };
            if (!ModelState.IsValid)
            {                          
                return View("CreateDirectionLine", redirectDirectionDirectionLine);
            }
            else
            {
                var newDirectionLineInDB = new DirectionLine
                {
                    directionID = directionDirectionLine.DirectionData.directionID,
                    directionLineDescription = directionDirectionLine.DirectionLineData.directionLineDescription,
                    directionLineText = directionDirectionLine.DirectionLineData.directionLineText
                };
                _context.DirectionLines.Add(newDirectionLineInDB);
                _context.SaveChanges();

                return RedirectToAction("CreateDirectionLine", "Direction", new { id = newDirectionLineInDB.directionID });
            }
        }
        //
        // Delete direction line
        // 
        public ActionResult DeleteDirectionLine(bool confirm, int id)
        {
            // if user confirm to delete then this action will fire
            // and you can pass true value. If not, then it is already not confirmed.
            var directionLine = _context.DirectionLines.SingleOrDefault(c => c.directionLineID == id);
            if (directionLine == null)
            {
                return View("Error");
            }

            var direction = _context.DirectionList.SingleOrDefault(c => c.directionID == directionLine.directionID);
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == direction.recipeID);
            var sessionAuthorID = (int)Session["AuthorID"];
            if ((User.IsInRole("CanManageEverything")) || (sessionAuthorID == recipe.authorID))
            {
                _context.DirectionLines.Remove(directionLine);
                _context.SaveChanges();
                return RedirectToAction("EditRecipeStep3", "Direction", new { id = direction.directionID});
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        }
    }
}