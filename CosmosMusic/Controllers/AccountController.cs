﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using CosmosMusic.Models;

namespace CosmosMusic.Controllers
{
    public class AccountController : Controller
    {

        private korovin_idzEntities db = new korovin_idzEntities();

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model/*, string returnUrl*/)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(x => x.username == model.UserName && x.password == model.Password).FirstOrDefault();
                
                if (user != null)
                {
                    user.isRemember = model.RememberMe;
                    db.SaveChanges();
                    ViewBag.UserName = model.UserName;
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    FormsAuthentication.RedirectFromLoginPage(model.UserName, false); 
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user

                var user = db.Users.Where(x => x.email == model.Email).FirstOrDefault();
                if (user != null)
                {
                    ModelState.AddModelError("", "The user with this email already exists.");
                }
                else
                {
                    user = null;
                    user = db.Users.Where(x => x.username == model.UserName).FirstOrDefault();
                    if (user != null)
                    {
                        ModelState.AddModelError("", "The user with this username already exists.");
                    }
                    else
                    {
                        var newuser = new Users();
                        newuser.username = model.UserName;
                        newuser.password = model.Password;
                        newuser.user_id = Guid.NewGuid();
                        newuser.isRemember = false;
                        newuser.id_role = db.Role.Where( x=> x.name == "user").FirstOrDefault().id_role;
                        newuser.email = model.Email;
                        db.Users.Add(newuser);
                        db.SaveChanges();
                        //Place for authorize cookie ?
                        FormsAuthentication.SetAuthCookie(model.UserName, false);
                        //return RedirectToAction("LogOn","Account");
                        
                        return RedirectToAction("Home", "Index");
                    }

                }
               /* MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, "question", "answer", true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false );
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                } */
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

        #region Helpers
            



        #endregion

    }
}
