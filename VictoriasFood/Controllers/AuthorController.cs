using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using VictoriasFood.Models;

namespace VictoriasFood.Controllers
{
    [Authorize]
    public class AuthorController : Controller
    {
        // Begin Access Data from Data Base
        private ApplicationDbContext _context;
        public AuthorController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        //End Access Data from Data Base
        // GET: Author
        [Authorize(Roles = "CanManageEverything")]
        public ActionResult Index()
        {
            var authorList = _context.Authors.ToList();
            return View(authorList);
        }
        public ActionResult DetailsAuthor(int id)
        {
            var author = _context.Authors.SingleOrDefault(c => c.authorID == id);
            if (author == null)
            {
                return View("Error");
            }
            return View(author);
        }        
        public ActionResult EditAuthor(int id)
        {
            var author = _context.Authors.SingleOrDefault(c => c.authorID == id);
            if (author == null)
            {
                return View("Error");
            }

            var sessionAuthorID = (int)Session["AuthorID"];
            if ((User.IsInRole("CanManageEverything")) || (sessionAuthorID == author.authorID))
            {                
                return View("EditAuthor", author);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAuthorInDatabase(Author author)
        {
            var authorInDb = _context.Authors.Single(c => c.authorID == author.authorID);
            authorInDb.authorFirstName = author.authorFirstName;
            authorInDb.authorSecondName = author.authorSecondName;
            authorInDb.authorLastName = author.authorLastName;
            authorInDb.addressLine1 = author.addressLine1;
            authorInDb.addressLine2 = author.addressLine2;
            authorInDb.telephoneNumber = author.telephoneNumber;
            authorInDb.authorCv = author.authorCv;
            authorInDb.facebook = author.facebook;
            authorInDb.twitter = author.twitter;
            authorInDb.instagram = author.instagram;
            authorInDb.website = author.website;
            authorInDb.email = author.email;

            if (!ModelState.IsValid)
            {
                return View("EditAuthor", authorInDb);
            }

            _context.SaveChanges();
            return RedirectToAction("DetailsAuthor", "Author", new { id = authorInDb.authorID });
        }
        public ActionResult CreateAuthor(Author authorReg)
        {
            return View("CreateAuthor", authorReg);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAuthorInDatabase(Author newAuthorInDB)
        {
            if (!ModelState.IsValid)
            {
                var newAuthor = new Author
                {
                    authorFirstName = newAuthorInDB.authorFirstName,
                    authorSecondName = newAuthorInDB.authorSecondName,
                    authorLastName = newAuthorInDB.authorLastName,
                    addressLine1 = newAuthorInDB.addressLine1,
                    addressLine2 = newAuthorInDB.addressLine2,
                    telephoneNumber = newAuthorInDB.telephoneNumber,
                    authorCv = newAuthorInDB.authorCv,
                    facebook = newAuthorInDB.facebook,
                    twitter = newAuthorInDB.twitter,
                    instagram = newAuthorInDB.instagram,
                    website = newAuthorInDB.website, 
                    email = newAuthorInDB.email
                };
                return View("CreateAuthor", newAuthor);
            }
            Session["AuthorEmail"] = newAuthorInDB.email;
            _context.Authors.Add(newAuthorInDB);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "CanManageEverything")]
        public ActionResult AdminPanel()
        {
            return View();
        }
    }
}