using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmosMusic.Models;

namespace CosmosMusic.Controllers
{
    public class UserController : Controller
    {

        korovin_idzEntities db = new korovin_idzEntities();

        //
        // GET: /User/

        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userName = HttpContext.User.Identity.Name;
                var user = db.Users.Where(x => x.username == userName).FirstOrDefault();
                return View(user);
            }
            return HttpNotFound();
        }

        public ActionResult Edit()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userName = HttpContext.User.Identity.Name;
                var user = db.Users.Where(x => x.username == userName).FirstOrDefault();
                if (user.Country.Count > 0)
                {
                    ViewBag.Countries = new MultiSelectList(db.Country, "id_country", "name", user.Country.Select(m => m.id_country));
                }
                else
                {
                    ViewBag.Countries = new MultiSelectList(db.Country, "id_country", "name");
                }
                return View(user);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = System.Data.Entity.EntityState.Modified;
                //There handle of string array goes
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }

    }
}
