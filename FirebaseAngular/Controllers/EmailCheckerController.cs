using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FirebaseAngular.Models;

namespace FirebaseAngular.Controllers
{
    public class EmailCheckerController : Controller
    {
        // GET: EmailChecker
        public ActionResult Index()
        {
           // bool value = true;
            using (EmailVerifier obj = new EmailVerifier())
            {
               // bool result = verifier.CheckExists(EmailText.Text);
               // bool output = obj.IsEmailVerified("raj.812@hotmail.co");
              
            }
            
            return View();
        }
    }
}