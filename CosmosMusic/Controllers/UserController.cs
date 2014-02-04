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

                ViewBag.Countries = new MultiSelectList(db.Country, "id_country", "name", user.SelectedCountries);

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
                foreach (var country in users.SelectedCountries)
                {
                    var dbCountry = db.Country.Find(new Guid(country));
                    if (dbCountry != null)
                        users.Country.Add(dbCountry);
                }
                db.Entry(users).State = System.Data.Entity.EntityState.Modified;
                //There handle of string array goes
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }

    }
}
