using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using VictoriasFood.Models;
using VictoriasFood.ViewModels;
using System.IO;
using System.Text.RegularExpressions;

namespace VictoriasFood.Controllers
{
    [Authorize]
    public class RecipeController : Controller
    {
        // Begin Access Data from Data Base
        private ApplicationDbContext _context;
        public RecipeController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        //End Access Data from Data Base
        // GET: Category
        [Authorize(Roles = "CanManageEverything")]
        public ActionResult Index()
        {
            var recipeList = _context.Recipes.Include(c => c.Authors).Include(c => c.Subcategories).ToList();
            return View(recipeList);
        }
        [AllowAnonymous]
        public ActionResult DetailsRecipe(int id)
        {
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == id);

            if (recipe == null)
            {
                return View("Error");
            }

            var author = _context.Authors.SingleOrDefault(c => c.authorID == recipe.authorID);
            var subcategories = _context.Subcategories.ToList();
            var subcategory = _context.Subcategories.SingleOrDefault(c => c.subcategoryID == recipe.subcategoryID);
            var category = _context.Categories.SingleOrDefault(c => c.categoryID == subcategory.categoryID);

            var allSuitableForCategories = _context.SuitableForCategories.ToList();
            var matchingSuitableForIDs = (_context.RecipeSuitableForCategories.Where(c => c.recipeID == id)).ToList();
            var querySuitableForCategories = from firstItem in allSuitableForCategories
                                             join secondItem in matchingSuitableForIDs
                                             on firstItem.suitableForID equals secondItem.suitableForID
                                             select firstItem;

            var allIngedients = _context.IngredientsList.ToList();
            var matchingIngredientsIDs = (_context.RecipeIngredientsList.Where(c => c.recipeID == id)).ToList();
            var queryIngredients = from firstItem in allIngedients
                                   join secondItem in matchingIngredientsIDs
                                   on firstItem.ingredientsID equals secondItem.ingredientsID
                                   select firstItem;

            var allIngredientIngredientLines = _context.IngredientsIngredientLines.ToList();
            var queryIngredientIngredientLines = from firstItem in allIngredientIngredientLines
                                                 join secondItem in queryIngredients
                                                 on firstItem.ingredientsID equals secondItem.ingredientsID
                                                 select firstItem;
            var allIngredientLines = _context.IngredientLines.ToList();
            var queryIngredientLines = from firstItem in allIngredientLines
                                       join secondItem in queryIngredientIngredientLines
                                        on firstItem.ingredientLineID equals secondItem.ingredientLineID
                                       select firstItem;

            var direction = _context.DirectionList.SingleOrDefault(c => c.recipeID == id);
            var directionLines = (_context.DirectionLines.Where(c => c.directionID == direction.directionID)).ToList();

            var tip = _context.Tips.SingleOrDefault(c => c.recipeID == id);

            var reviews = _context.Reviews.Where(c => c.recipeID == id).ToList();

            var allAuthors = _context.Authors.ToList();
            var queryReviewsAuthors = from firstItem in allAuthors
                                      join secondItem in reviews
                                       on firstItem.authorID equals secondItem.authorID
                                      select firstItem;

            ViewBag.IsFavourited = false;
            if ((Session["AuthorID"] != null) && ((int)Session["AuthorID"] != 0))
            {
                var authorIDFromSession = (int)(Session["AuthorID"]);
                var allFavouritesOfAuthor = _context.Favourites.Where(c => c.authorID == authorIDFromSession);
                if (allFavouritesOfAuthor.Any())
                {
                    var isThisRecipeFavourite = allFavouritesOfAuthor.SingleOrDefault(c => c.recipeID == id);
                    if (isThisRecipeFavourite != null)
                    {
                        ViewBag.IsFavourited = true;
                    }
                }
            }                    

            var recipeAllInformation = new RecipeAllInformation
            {
                RecipeData = recipe,
                AuthorData = author,
                SubcategoryData = subcategory,
                SubcategoriesData = subcategories,
                CategoryData = category,
                SuitableForData = querySuitableForCategories,
                IngredientsData = queryIngredients,
                IngredientLineData = queryIngredientLines,
                IngredientsIngredientLineData = queryIngredientIngredientLines,
                DirectionData = direction,
                DirectionLineData = directionLines,
                TipData = tip,
                ReviewData = reviews,
                ReviewAuthorData = queryReviewsAuthors
            };            
            return View(recipeAllInformation);
        }
        //
        // Delete recipe
        // 
        public ActionResult DeleteRecipe(bool confirm, int id)
        {
            // if user confirm to delete then this action will fire
            // and you can pass true value. If not, then it is already not confirmed.
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == id);
            if (recipe == null)
            {
                return View("Error");
            }

            var sessionAuthorID = (int)Session["AuthorID"];
            if ((User.IsInRole("CanManageEverything")) || (sessionAuthorID == recipe.authorID))
            {
                var imagePath = recipe.recipeImage.Replace("../../Content/uploads/recipes/", ""); ;
                imagePath = Path.Combine(Server.MapPath("~/Content/uploads/recipes/"), imagePath);

                var reviews = _context.Reviews.Where(c => c.recipeID == id).ToList();
                if (reviews.Any())
                {
                    foreach (var current in reviews)
                    {
                        _context.Reviews.Remove(current);
                    }
                }
                var favourites = _context.Favourites.Where(c => c.recipeID == id).ToList();
                if (favourites.Any())
                {
                    foreach (var current in favourites)
                    {
                        _context.Favourites.Remove(current);
                    }
                }
                var recipeSuitableForCategories = _context.RecipeSuitableForCategories.Where(c => c.recipeID == id).ToList();
                if (recipeSuitableForCategories.Any())
                {
                    foreach (var current in recipeSuitableForCategories)
                    {
                        _context.RecipeSuitableForCategories.Remove(current);
                    }
                }
                var tip = _context.Tips.SingleOrDefault(c => c.recipeID == id);
                if (tip != null)
                {
                    _context.Tips.Remove(tip);
                }
                var directionList = _context.DirectionList.SingleOrDefault(c => c.recipeID == id);
                if (directionList != null)
                {
                    //lines are null recipe id 11
                    var directionLines = _context.DirectionLines.Where(c => c.directionID == directionList.directionID).ToList();
                    if (directionLines.Any())
                    {
                        foreach (var current in directionLines)
                        {
                            _context.DirectionLines.Remove(current);
                        }
                    }
                    _context.DirectionList.Remove(directionList);
                }
                var recipeIngredientsList = _context.RecipeIngredientsList.Where(c => c.recipeID == id).ToList();
                if (recipeIngredientsList.Any())
                {
                    foreach (var currentRecipeIngredientsList in recipeIngredientsList)
                    {
                        var ingredientList = _context.IngredientsList.SingleOrDefault(c => c.ingredientsID == currentRecipeIngredientsList.ingredientsID);
                        if (ingredientList != null)
                        {
                            var ingredientsIngredientLines = _context.IngredientsIngredientLines.Where(c => c.ingredientsID == ingredientList.ingredientsID).ToList();
                            foreach (var currentIngredientsIngredientLines in ingredientsIngredientLines)
                            {
                                var ingredientLine = _context.IngredientLines.SingleOrDefault(c => c.ingredientLineID == currentIngredientsIngredientLines.ingredientLineID);
                                if (ingredientLine != null)
                                {                                    
                                    _context.IngredientLines.Remove(ingredientLine);                                    
                                }
                                _context.IngredientsIngredientLines.Remove(currentIngredientsIngredientLines);
                            }
                            _context.IngredientsList.Remove(ingredientList);
                        }
                        _context.RecipeIngredientsList.Remove(currentRecipeIngredientsList);
                    }                    
                }
                _context.Recipes.Remove(recipe);
                _context.SaveChanges();
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //
        // Edit Recipe
        //
        public ActionResult EditRecipeStep1(int id)
        {
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == id);
            if (recipe == null)
            {
                return View("Error");
            }

            var sessionAuthorID = (int)Session["AuthorID"];
            if ((User.IsInRole("CanManageEverything")) || (sessionAuthorID == recipe.authorID))
            {
                var subcategory = _context.Subcategories.SingleOrDefault(c => c.subcategoryID == recipe.subcategoryID);
                var subcategories = _context.Subcategories.ToList();
                var allIngedients = _context.IngredientsList.ToList();
                var matchingIngredientsIDs = _context.RecipeIngredientsList.Where(c => c.recipeID == id).ToList();
                var queryIngredients = from firstItem in allIngedients
                                       join secondItem in matchingIngredientsIDs
                                       on firstItem.ingredientsID equals secondItem.ingredientsID
                                       select firstItem;

                var direction = _context.DirectionList.SingleOrDefault(c => c.recipeID == id);
                var tip = _context.Tips.SingleOrDefault(c => c.recipeID == id);

                var allSuitableForCategories = _context.SuitableForCategories.ToList();
                var matchingSuitableForIDs = (_context.RecipeSuitableForCategories.Where(c => c.recipeID == id)).ToList();
                var querySuitableForCategories = from firstItem in allSuitableForCategories
                                                 join secondItem in matchingSuitableForIDs
                                                 on firstItem.suitableForID equals secondItem.suitableForID
                                                 select firstItem;

                var recipeAllInformation = new RecipeAllInformation
                {
                    RecipeData = recipe,
                    SubcategoryData = subcategory,
                    SubcategoriesData = subcategories,
                    IngredientsData = queryIngredients,
                    DirectionData = direction,
                    SuitableForData = querySuitableForCategories,
                    TipData = tip
                };

                return View("EditRecipeStep1", recipeAllInformation);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            } 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateRecipeInDatabase(RecipeAllInformation recipeAllInformation, HttpPostedFileBase file)
        {
            var redirectRecipeID = recipeAllInformation.RecipeData.recipeID;
            var recipeInDb = _context.Recipes.SingleOrDefault(c => c.recipeID == recipeAllInformation.RecipeData.recipeID);
            recipeInDb.recipeTitle = recipeAllInformation.RecipeData.recipeTitle;
            recipeInDb.recipeDescription = recipeAllInformation.RecipeData.recipeDescription;
            recipeInDb.recipeNumberOfServings = recipeAllInformation.RecipeData.recipeNumberOfServings;
            recipeInDb.recipeReadyIn = recipeAllInformation.RecipeData.recipeReadyIn;
            recipeInDb.subcategoryID = recipeAllInformation.RecipeData.subcategoryID;

            var webpagePath = "../../Content/uploads/recipes/";
            if (file != null && file.ContentLength > 0)
            {
                string[] validImageTypes = { "image/png", "image/jpg", "image/jpeg" };
                if (validImageTypes.Contains(file.ContentType))
                {
                    var fileName = Path.GetFileName(file.FileName.ToLower());
                    var path = Path.Combine(Server.MapPath("~/Content/uploads/recipes/"), fileName);
                    var oldImage = recipeInDb.recipeImage.Replace("../../Content/uploads/recipes/", "");
                    oldImage = Path.Combine(Server.MapPath("~/Content/uploads/recipes/"), oldImage);                    
                    try
                    {
                        while (System.IO.File.Exists(path))
                        {                            
                            Random rnd = new Random();
                            int rndnumber = rnd.Next();
                            fileName = rndnumber + fileName;
                            path = Path.Combine(Server.MapPath("~/Content/uploads/recipes/"), fileName);
                        }
                        file.SaveAs(path);
                        webpagePath += fileName;
                        recipeInDb.recipeImage = webpagePath;
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }

            }           
            
            bool isRecipeModelInvalid = false;
            if (
                (recipeInDb.authorID == 0) ||
                (recipeInDb.subcategoryID == 0) ||
                (recipeInDb.recipeNumberOfServings == 0) ||
                (String.IsNullOrEmpty(recipeInDb.recipeTitle)) ||
                (String.IsNullOrEmpty(recipeInDb.recipeDescription)) ||
                (String.IsNullOrEmpty(recipeInDb.recipeImage)) ||
                (recipeInDb.recipeReadyIn.ToString("HH:mm") == "00:00")
                )
            {
                isRecipeModelInvalid = true;
            }
            if (isRecipeModelInvalid)
            {
                var subcategory = _context.Subcategories.SingleOrDefault(c => c.subcategoryID == recipeInDb.subcategoryID);
                var subcategories = _context.Subcategories.ToList();
                var allIngedients = _context.IngredientsList.ToList();
                var matchingIngredientsIDs = _context.RecipeIngredientsList.Where(c => c.recipeID == recipeInDb.recipeID).ToList();
                var queryIngredients = from firstItem in allIngedients
                                       join secondItem in matchingIngredientsIDs
                                       on firstItem.ingredientsID equals secondItem.ingredientsID
                                       select firstItem;

                var direction = _context.DirectionList.SingleOrDefault(c => c.recipeID == recipeInDb.recipeID);
                var tip = _context.Tips.SingleOrDefault(c => c.recipeID == recipeInDb.recipeID);

                var allSuitableForCategories = _context.SuitableForCategories.ToList();
                var matchingSuitableForIDs = (_context.RecipeSuitableForCategories.Where(c => c.recipeID == recipeInDb.recipeID)).ToList();
                var querySuitableForCategories = from firstItem in allSuitableForCategories
                                                 join secondItem in matchingSuitableForIDs
                                                 on firstItem.suitableForID equals secondItem.suitableForID
                                                 select firstItem;
                var recipeAllInformationRedirect = new RecipeAllInformation
                {
                    RecipeData = recipeInDb,
                    SubcategoryData = subcategory,
                    SubcategoriesData = subcategories,
                    IngredientsData = queryIngredients,
                    DirectionData = direction,
                    SuitableForData = querySuitableForCategories,
                    TipData = tip
                };
                return View("EditRecipeStep1", recipeAllInformationRedirect);
            }
            
            _context.SaveChanges();
            return RedirectToAction("EditRecipeStep1", "Recipe", new { id = redirectRecipeID });
        }
        //
        // Create new recipe
        //              
        public ActionResult CreateRecipeStep1()
        {
            var recipeIESubcategories = new RecipeIESubcategories
            {
                SubcategoryData = _context.Subcategories.ToList()
            };
            return View("CreateRecipeStep1", recipeIESubcategories);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRecipeInDatabase(RecipeIESubcategories newRecipeIESubcategoriesInDB, HttpPostedFileBase file)
        {
            int redirectRecipeID;
            var authorIDFromSession = (int)(Session["AuthorID"]);
            newRecipeIESubcategoriesInDB.RecipeData.authorID = authorIDFromSession;
            var webpagePath = "../../Content/uploads/recipes/";
            newRecipeIESubcategoriesInDB.SubcategoryData = _context.Subcategories.ToList();

            if (file != null && file.ContentLength > 0)
            {
                string[] validImageTypes = { "image/png", "image/jpg", "image/jpeg" };

                if (validImageTypes.Contains(file.ContentType))
                {
                    var fileName = Path.GetFileName(file.FileName.ToLower());
                    var path = Path.Combine(Server.MapPath("~/Content/uploads/recipes/"), fileName);
                    try
                    {
                        while (System.IO.File.Exists(path))
                        {
                            Random rnd = new Random();
                            int rndnumber = rnd.Next();
                            fileName = rndnumber + fileName;
                            path = Path.Combine(Server.MapPath("~/Content/uploads/recipes/"), fileName);
                        }
                        file.SaveAs(path);
                        webpagePath += fileName;
                        newRecipeIESubcategoriesInDB.RecipeData.recipeImage = webpagePath;
                    }
                    catch (Exception e)
                    {
                        throw e;
                        //ModelState.AddModelError("uploadError", "Can't save file to disk");
                    }
                }

            }

            bool isRecipeModelInvalid = false;
            if (
                (newRecipeIESubcategoriesInDB.RecipeData.authorID == 0) ||
                (newRecipeIESubcategoriesInDB.RecipeData.subcategoryID == 0) ||
                (newRecipeIESubcategoriesInDB.RecipeData.recipeNumberOfServings == 0) ||
                (String.IsNullOrEmpty(newRecipeIESubcategoriesInDB.RecipeData.recipeTitle)) ||
                (String.IsNullOrEmpty(newRecipeIESubcategoriesInDB.RecipeData.recipeDescription)) ||
                (String.IsNullOrEmpty(newRecipeIESubcategoriesInDB.RecipeData.recipeImage)) ||
                (newRecipeIESubcategoriesInDB.RecipeData.recipeReadyIn.ToString("HH:mm") == "00:00")
                )
            {
                isRecipeModelInvalid = true;
            }
            if (isRecipeModelInvalid)
            {
                if (!(String.IsNullOrEmpty(newRecipeIESubcategoriesInDB.RecipeData.recipeImage)))
                {
                    var imageToRemove = newRecipeIESubcategoriesInDB.RecipeData.recipeImage.Replace("../../Content/uploads/recipes/", "");
                    imageToRemove = Path.Combine(Server.MapPath("~/Content/uploads/recipes/"), imageToRemove);
                    if (System.IO.File.Exists(imageToRemove))
                    {
                        System.IO.File.Delete(imageToRemove);
                    }
                }
                var recipeIESubcategories = new RecipeIESubcategories
                {
                    RecipeData = newRecipeIESubcategoriesInDB.RecipeData,
                    SubcategoryData = newRecipeIESubcategoriesInDB.SubcategoryData
                };
                return View("CreateRecipeStep1", recipeIESubcategories);
            }
            else
            {
                var newRecipe = new Recipe
                {
                    recipeID = newRecipeIESubcategoriesInDB.RecipeData.recipeID,
                    recipeTitle = newRecipeIESubcategoriesInDB.RecipeData.recipeTitle,
                    recipeDescription = newRecipeIESubcategoriesInDB.RecipeData.recipeDescription,
                    recipeNumberOfServings = newRecipeIESubcategoriesInDB.RecipeData.recipeNumberOfServings,
                    recipeReadyIn = newRecipeIESubcategoriesInDB.RecipeData.recipeReadyIn,
                    recipeImage = newRecipeIESubcategoriesInDB.RecipeData.recipeImage,
                    authorID = newRecipeIESubcategoriesInDB.RecipeData.authorID,
                    subcategoryID = newRecipeIESubcategoriesInDB.RecipeData.subcategoryID
                };
                _context.Recipes.Add(newRecipe);
                _context.SaveChanges();
                redirectRecipeID = newRecipe.recipeID;
                return RedirectToAction("CreateRecipeStep2", "Ingredients", new { id = redirectRecipeID });
            }
        }
        //
        // Create new recipe step 4 final
        //              
        public ActionResult CreateRecipeStep4(int id)
        {
            var recipe = _context.Recipes.SingleOrDefault(c => c.recipeID == id);
            if (recipe == null)
            {
                return View("Error");
            }

            var tip = _context.Tips.SingleOrDefault(c => c.recipeID == id);
            var allSuitableForCategories = _context.SuitableForCategories.ToList();
            var matchingSuitableForIDs = (_context.RecipeSuitableForCategories.Where(c => c.recipeID == id)).ToList();
            var querySuitableForCategories = from firstItem in allSuitableForCategories
                                             join secondItem in matchingSuitableForIDs
                                             on firstItem.suitableForID equals secondItem.suitableForID
                                             select firstItem;
            var recipeTipIESuitableFor = new RecipeTipIESuitableFor
            {
                RecipeData = recipe,    
                SuitableForData = querySuitableForCategories,
                TipData = tip
            };
            return View("CreateRecipeStep4", recipeTipIESuitableFor);
        }
        //
        // Add recipe to favourites 
        //       
        public ActionResult AddRecipeToFavourites(int idRecipe, int idAuthor)
        {
            var newFavouriteInDB = new Favourite {
                authorID = idAuthor, 
                recipeID = idRecipe
            };
            _context.Favourites.Add(newFavouriteInDB);
            _context.SaveChanges();

            return RedirectToAction("DetailsRecipe", "Recipe", new { id = idRecipe });   
        }
        //
        // Delete recipe from favourites 
        // 
        public ActionResult DeleteRecipeFromFavourites(int idRecipe, int idAuthor)
        {
            var allFavouritesForAuthor = _context.Favourites.Where(c => c.authorID == idAuthor);
            var recipeForRemove = allFavouritesForAuthor.SingleOrDefault(c => c.recipeID == idRecipe);

            _context.Favourites.Remove(recipeForRemove);
            _context.SaveChanges();

            return RedirectToAction("DetailsRecipe", "Recipe", new { id = idRecipe });
        }
        //
        // Add review for recipe
        //
        [HttpPost]
        public ActionResult CreateReviewInDatabase(Review newReviewInDb)
        {
            var recipeId = newReviewInDb.recipeID;
            
            Review ajaxReview = new Review
            {
                recipeID = newReviewInDb.recipeID,
                authorID = newReviewInDb.authorID,
                reviewRate = newReviewInDb.reviewRate,
                reviewDateTime = newReviewInDb.reviewDateTime,
                reviewText = newReviewInDb.reviewText
            };
            if ((ajaxReview.authorID == 0)) {
                //return false - Anonymous user
                return Json(new { success = false, responseText = "If you want to add review, please log in!" }, JsonRequestBehavior.AllowGet);
            }
            if ((ajaxReview.reviewRate <= 0) || (ajaxReview.reviewRate > 5))
            {
                //return false
                return Json(new { success = false, responseText = "Failed. Your review was not added. Raiting is mandatory field!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _context.Reviews.Add(ajaxReview);
                _context.SaveChanges();
                // return success
                return Json(new { success = true, responseText = "Your review was successfully added!" }, JsonRequestBehavior.AllowGet);
            } 
        } 
        //
        // Show recipes by subcategory
        //
        [AllowAnonymous]
        public ActionResult SubcategoryRecipes(int id)
        {
            var subcategory = _context.Subcategories.SingleOrDefault(c => c.subcategoryID == id);
            var recipes = _context.Recipes.Where(c => c.subcategoryID == id).ToList();

            if (subcategory == null)
            {
                return View("Error");
            }

            var subcategoryRecipes = new SubcategoryRecipes
            {
                SubcategoryData = subcategory,
                RecipeData = recipes
            };
            
            return View(subcategoryRecipes);
        }
        //
        // Show recipes by suitable for category
        //
        [AllowAnonymous]
        public ActionResult SuitableForRecipes(int id)
        {
            var suitableForCategory = _context.SuitableForCategories.SingleOrDefault(c => c.suitableForID == id);
            var recipeSuitableForCategories = (_context.RecipeSuitableForCategories.Where(c => c.suitableForID == id)).ToList();
            var allRecipe = _context.Recipes.ToList();
            var recipes = from firstItem in allRecipe
                          join secondItem in recipeSuitableForCategories
                          on firstItem.recipeID equals secondItem.recipeID
                          select firstItem;

            if (suitableForCategory == null)
            {
                return View("Error");
            }

            var suitableForIERecipe = new SuitableForIERecipe
            {
                SuitableForData = suitableForCategory,
                RecipeSuitableForData = recipeSuitableForCategories,
                RecipeData = recipes
            };
                        
            return View(suitableForIERecipe);
        }
        //
        // Recipes by author
        //
        [AllowAnonymous]
        public ActionResult RecipesByAuthor(int id)
        {
            var author = _context.Authors.SingleOrDefault(c => c.authorID == id);
            var recipesByAuthor = _context.Recipes.Where(c => c.authorID == id).ToList();

            if (author == null)
            {
                return View("Error");
            }

            var authorIERecipes = new AuthorIERecipes
            {
                AuthorData = author,
                RecipeData = recipesByAuthor
            };
            return View(authorIERecipes);
        }
        //
        // Favourite Recipes by author
        //
        public ActionResult FavouriteRecipes(int id)
        {
            var author = _context.Authors.SingleOrDefault(c => c.authorID == id);

            if (author == null) {
                return View("Error");
            }

            var sessionAuthorID = (int)Session["AuthorID"];
            if ((User.IsInRole("CanManageEverything")) || (sessionAuthorID == id))
            {
                var authorFavourites = _context.Favourites.Where(c => c.authorID == id).ToList();
                var allRecipes = _context.Recipes.ToList();
                var authorFavouritesRecipes = from firstItem in allRecipes
                                              join secondItem in authorFavourites
                                              on firstItem.recipeID equals secondItem.recipeID
                                              select firstItem;

                var authorIERecipes = new AuthorIERecipes
                {
                    AuthorData = author,
                    RecipeData = authorFavouritesRecipes
                };
                return View(authorIERecipes);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        //
        //test
        //
        [AllowAnonymous]

        public ActionResult AllRecipes()
        {
            return View();
        }
        //
        // Search recipes
        //  
        [AllowAnonymous]
        public ActionResult Search(List<Recipe> sortedSearchResult)
        {
            return View(sortedSearchResult);
        }
        static string[] GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w']*\b");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select TrimSuffix(m.Value);

            return words.ToArray();
        }
        static string TrimSuffix(string word)
        {
            int apostropheLocation = word.IndexOf('\'');
            if (apostropheLocation != -1)
            {
                word = word.Substring(0, apostropheLocation);
            }

            return word;
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult SearchRecipes(string searchInputText)
        {
            var searchString = searchInputText.ToLower();
            var sortedSearchResult = new List<Recipe>();
            if (String.IsNullOrEmpty(searchString)) {
               
                return View("Search", sortedSearchResult);
            }
            var separatedWords = GetWords(searchString);
            var allRecipes = _context.Recipes.ToList();            
           
            foreach (var recipe in allRecipes)
            {
                var numberOfMatches = 0;
                foreach (var word in separatedWords)
                {
                    if ( recipe.recipeTitle.ToLower().Contains(word) )
                    {
                        numberOfMatches++;
                    }                  
                }
                if (numberOfMatches != 0)
                {
                    sortedSearchResult.Add(recipe);                    
                }
            }            

            return View("Search", sortedSearchResult);
        }
    }
}