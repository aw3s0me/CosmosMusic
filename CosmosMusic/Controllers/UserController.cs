using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmosMusic.Models;
using CosmosMusic.ViewModels;

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
                var userViewModel = new UserViewModel
                {
                    Username = user.username,
                    Name = user.name,
                    Country = user.Country,
                    DateofBirth = user.birth_date,
                    Email = user.email,
                    Artists = user.Artists
                    //SelectedArtists = user.Artists.Select(x => x.name).ToList()
                };

                ViewBag.Countries = new MultiSelectList(db.Country, "id_country", "name", userViewModel.SelectedCountries);
                
                ViewBag.Artists = new MultiSelectList(db.Artists, "artist_id", "name", userViewModel.SelectedArtists);
                /*foreach (var artist in userViewModel.Artists)
                {
                    (ViewBag.Countries as MultiSelectList).Items
                    artist.Selected = true;
                }
                foreach (var country in userViewModel.Country)
                {
                    country.Selected = true;
                } */
                return View(userViewModel);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                var userDb = db.Users.Where(x => x.username == userModel.Username).FirstOrDefault();
                foreach (var country in userModel.SelectedCountries)
                {
                    var dbCountry = db.Country.Find(new Guid(country));
                    if (dbCountry != null)
                        userDb.Country.Add(dbCountry);
                }
                foreach (var artist in userModel.SelectedArtists)
                {
                    var dbArtist = db.Artists.Find(new Guid(artist));
                    if (dbArtist != null)
                        userDb.Artists.Add(dbArtist);

                }

                userDb.email = userModel.Email;
                userDb.birth_date = userModel.DateofBirth;
                userDb.name = userModel.Name;
                userDb.username = userModel.Username;

                db.Entry(userDb).State = System.Data.Entity.EntityState.Modified;
                //There handle of string array goes
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userModel);
        }

    }
}
