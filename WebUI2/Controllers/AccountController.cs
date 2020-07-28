using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity.Validation;
using WebUI2.Models;
using Model.Entities;
using Model;
using System.Security.Principal;
using Model.Abstracts;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Model.ExternalAuthentication;
using System.Diagnostics;
using System.Data.SqlClient;
using Model.EF;

namespace WebUI2.Controllers
{

    /// <summary>
    /// Processes registration of users, their login and log off requests. Processes user specific requests for 
    /// management of their specific profile data, pasword recovery and profile update.
    /// </summary>
    /// 
    [Authorize]
    public class AccountController : Controller
    {
        private string userRoleName = "user";
        private string adminRoleName = "admin";

        public string[] Roles { get { string[] vv = { userRoleName, adminRoleName }; return vv; } }



        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private ApplicationRoleManager ApplicationRoleManager { get { return new ApplicationRoleManager(); } }



        public AccountController(IRepository repository, IMailProcessor mailProcessor)
        {       
            this.repository = repository;
            this.mailProcessor = mailProcessor;
            //SetupRoles(Roles);
        }


        public void SetupRoles(string[] roles)
        {
            foreach (string role in roles)
            {
               if (!ApplicationRoleManager.RoleExists(role))
                 ApplicationRoleManager.Create(new ApplicationRole(role));
            }
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback",
                                                     "Account", new { ReturnUrl = returnUrl }));
        }



        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await repository.FindAsync(loginInfo.Login);

            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                Session["Profile"] = user;
                return RedirectToLocal(returnUrl, user);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation",
                       new LoginInfoNoPassword { Username = loginInfo.DefaultUserName });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmExternalLogin(LoginInfoNoPassword model, string returnUrl)
        {

            // Get the information about the user from the external login provider
            var info = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (ModelState.IsValid)
            {
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                var user = new ApplicationUser()
                {
                    UserName = model.Username,
                    Email = model.Email
                };

                var result = await repository.Create(user);
                if (result.Succeeded)
                {
                    result = await repository.AddLogin(user.Id, info.Login);// adds external login details to a separate datatable
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        Session["Profile"] = user;
                        await repository.AddToRole(user.Id, userRoleName);
                        return RedirectToLocal(returnUrl, user);
                    }
                }
                AddErrors(result);
            }
            ViewBag.LoginProvider = info.Login.LoginProvider;
            ViewBag.ReturnUrl = returnUrl;
            return View("ExternalLoginConfirmation", model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("error", error);
            }
        }





        private ActionResult RedirectToLocal(string returnUrl, ApplicationUser appUser)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Personal", "Account", appUser);
            }
        }







        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";



        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await repository.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        #region
        private class ChallengeResult : HttpUnauthorizedResult
        {
            // Used for XSRF protection when adding external logins
            private const string XsrfKey = "XsrfId";


            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }


            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }



            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }

                context.RequestContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        //..............................................................................................................................................

        private IRepository repository;
        private IMailProcessor mailProcessor;
        private string myBusinessName = "stesha.com.au";
        private int passwordLength = 8;
        private int numberOfNonAlphanumericCharacters = 0;





        [AllowAnonymous]
        public ViewResult Login()
        {
            return View();
        }





        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginInfo info, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await repository.Find(info.Username, info.Password);

                if (user != null)
                {
                    await SignInAsync(user, info.Persist);
                    Session["Profile"] = user;

                    if (await repository.IsInRole(user.Id, adminRoleName))
                    {
                        return Redirect(ReturnUrl ?? Url.Action("Index", "Admin"));
                    }
                    else
                    {
                        if (ReturnUrl == null) return View("Personal", user);
                        else return Redirect(ReturnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The username or password is incorrect");
                }
            }
            return View();
        }






        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ViewResult> Register(CustomerInfo custInfo)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user;
                InitializeUser(custInfo, out user);

                var result = await repository.Create(user, custInfo.Password);


                if (result.Succeeded)
                {
                    await SignInAsync(user, false);
                    Session["Profile"] = user;
                    await repository.AddToRole(user.Id, userRoleName);

                    TempData["Confirm"] =
                          "Your new account has been successfully created. A confirmation mail has been sent to the email address you provided";

                    SendConfirmationEmail(user, mailProcessor);
                    return View("Personal", user);

                }
                AddErrors(result);
                TempData["Fail"] = "Account Creation Failed";
                return View(custInfo);
            }
            else
            {
                // validation errors
                return View(custInfo);
            }
        }





        private void SendConfirmationEmail(ApplicationUser appUser, IMailProcessor sender)
        {
            string message = "Hi " + appUser.Name + "!";
            message += "\n\rYour profile with " + myBusinessName + " has been successfully created.";
            message += "\n\rYour username is " + appUser.UserName;
            message += "\n\rYou can log into your account using the following link http://stesha.com.au/Account/Login";

            InquiryInfo inq = new InquiryInfo
            {
                RecepientName = appUser.Name,
                RecepientAddress = appUser.Email,
                Subject = "new account at " + myBusinessName,
                FullMessage = message
            };
            sender.SendMail(inq);
        }

        [AllowAnonymous]
        public ViewResult Register()
        {
            // pass in an empty customer to be filled in
            return View(new CustomerInfo());
        }



        public ViewResult Personal()
        {
            ApplicationUser user = (ApplicationUser)Session["Profile"];
            return View(user);
        }



        public void InitializeUser(CustomerInfo info, out ApplicationUser user)
        {
            user = new ApplicationUser
            {
                UserName = info.Username,
                Surname = info.Surname,
                Name = info.Name,
                Email = info.Email
            };
        }



        public ActionResult Signoff()
        {
            AuthenticationManager.SignOut();

            if (Session["Profile"] != null)
            {
                Session["Profile"] = null;
            }

            TempData["Confirm"] = "You Have Successfully Logged off";

            return RedirectToAction("List", "Products", null);
        }



        [AllowAnonymous]
        public ViewResult Recover()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public ViewResult Recover(string username)
        {
            if (String.IsNullOrWhiteSpace(username))
            {
                ModelState.AddModelError("", "Please entern your username");
                return View();
            }
            var user = repository.FindByName(username);

            if (user == null)
            {
                TempData["Fail"] = "User with this username doesn't exist";
            }
            else
            {
                string newPassword = Membership.GeneratePassword(passwordLength, numberOfNonAlphanumericCharacters);
                repository.RemovePassword(user.Id);
                repository.AddPassword(user.Id, newPassword);
                SendNewPassword(newPassword, username);

                TempData["Confirm"] = "A new password has been sent to your email.";
            }

            return View();
        }




        private void SendNewPassword(string newPassword, string username)
        {
            InquiryInfo inq = new InquiryInfo();
            ApplicationUser user = repository.FindByName(username);
            inq.RecepientName = user.Name;
            inq.RecepientAddress = user.Email;
            inq.Subject = "Password reset";
            inq.FullMessage = "Hi " + user.Name + ", your new password is " + newPassword;

            mailProcessor.SendMail(inq);

        }



        public PartialViewResult GetPastOrders()
        {
            ApplicationUser prof = (ApplicationUser)Session["Profile"];
            List<string> list = repository.GetListOfPastOrders(prof.Id);
            return PartialView(list);
        }




        public ViewResult ShowOrderDetails(string transId)
        {
            Order order = repository.GetOrder(transId);

            return View(order);
        }




    }
}




















