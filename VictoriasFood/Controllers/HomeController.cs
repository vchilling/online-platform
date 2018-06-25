using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VictoriasFood.Models;
using System.Net.Mail;
using System.Web.Mvc.Html;
using System.Text;
using System.Threading;
using System.Data.Entity;
using System.Net;

namespace VictoriasFood.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        //End Access Data from Data Base
        // GET: Author        
        public ActionResult Index()
        {
            var emailAuthor = (string)(Session["AuthorEmail"]);
            var userIsAuthenticated = User.Identity.IsAuthenticated;
            if ( (userIsAuthenticated) && (emailAuthor == null) )
            { 
                var controller = DependencyResolver.Current.GetService<AccountController>();
                controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
                var logOut = controller.SessionExpired();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (emailAuthor != null)
                {
                    var author = _context.Authors.Single(c => c.email == emailAuthor);
                    Session["AuthorID"] = author.authorID;
                }
                return View();
            }            
        }

        public ActionResult About()
        {
            ViewBag.Message = "A few words about us";

            return View();
        }
       // [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Where to find us";           
            var contactMailModel = new ContactMailModel(); 
            return View(contactMailModel);
        }    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactMailModel contactMailModel)
        {

           // var currentContactMailModel = contactMailModel;
            
            MailMessage mm = new MailMessage("victoria1317@gmail.com",
                                            "victoriafoodrecipes@gmail.com",
                                            "Quarterly data report.",
                                            "See the attached spreadsheet.");            
            mm.Subject = "FOOD recipes contact form message";
            mm.Body = "From: " + contactMailModel.Name + 
                        "<br /><br />E-mail addres: " + contactMailModel.Email +
                        "<br /><br />Telephone number: " + contactMailModel.Telephone +
                        "<br /><br />Message:" + contactMailModel.Message;
            mm.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            NetworkCred.UserName = "victoriafoodrecipes@gmail.com";
            NetworkCred.Password = "Myproject1@";
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            try
            {
                smtp.Send(mm);
                
                //lblMessage.Text = "Съобщението ви беше изпратено успешно!";
               // txtName.Text = String.Empty;
                //txtSubject.Text = String.Empty;
               // txtEmail.Text = String.Empty;
               // txtBody.Text = String.Empty;

            }
            catch (Exception emailException)
            {
                Response.Write(emailException.Message);
            }

            return View(); 
        }

        public ActionResult PrivacyPolicy()
        {
            ViewBag.Message = "Our Privacy & Terms";

            return View();
        }      

    }
}