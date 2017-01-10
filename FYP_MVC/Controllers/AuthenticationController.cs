using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using FYP_MVC.Models.Auth;
using FYP_MVC.Models.DAO;
using System.Security.Cryptography;
using System.Text;

namespace FYP_MVC.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication

        private FYPEntities db = new FYPEntities();
        public ActionResult Index()
        {
            return View();
        }

        public AuthenticationController()
        {
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            //    case SignInStatus.Failure:
            //    default:
            //        ModelState.AddModelError("", "Invalid login attempt.");
            //        return View(model);
            //}

            MD5 md5Hash = MD5.Create();
            string hassedPassword = GetMd5Hash(md5Hash, model.Password);
            List<user> users = db.users.Where(u => u.email == model.Email && u.passwword == hassedPassword).ToList();


            if (users.Count == 1)
            {
                Session["user"] = users.FirstOrDefault();
                if (users.FirstOrDefault().userType == "Admin")
                {
                    return RedirectToAction("Home", "Admin");
                }
                else
                {
                    return RedirectToAction("Home", "Task");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);

            }

        }


        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                /*  var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                  var result = await UserManager.CreateAsync(user, model.Password);
                  if (result.Succeeded)
                  {
                      await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                      // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                      // Send an email with this link
                      // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                      // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                      // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                      return RedirectToAction("Index", "Home");
                  }
                  AddErrors(result);*/
                MD5 md5Hash = MD5.Create();
                string hassedPassword = GetMd5Hash(md5Hash, model.Password);
                user user = new user
                {
                    firstName = model.firstName,
                    lastName = model.lastName,
                    email = model.Email,
                    passwword = hassedPassword,
                    userType = "General user"

                };

                // Add the new object to the Orders collection.
                db.users.Add(user);

                // Submit the change to the database.
                try
                {
                    db.SaveChanges();
                    List<user> users = db.users.Where(u => u.email == model.Email).ToList();
                    Session["user"] = users.FirstOrDefault();
                    return RedirectToAction("Home", "Task");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ModelState.AddModelError("", "Errors while creating your account. Try again..");
                    return View(model);
                }





            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Invalid signup attempt!");
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {/*
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);*/
            return View();
        }
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            Session["user"] = null;

            return RedirectToAction("Login", "Authentication");
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}