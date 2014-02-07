using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmosMusic.Models;

namespace CosmosMusic.Controllers
{
    public class ArtistManagerController : Controller
    {
        private korovin_idzEntities db = new korovin_idzEntities();

        //
        // GET: /ArtistManager/

        public ActionResult Index()
        {
            return View(db.Artists.ToList());
        }

        //
        // GET: /ArtistManager/Details/5

        public ActionResult Details(string id = null)
        {
            Guid ArtistGuid = Guid.Empty;
            try
            {
                ArtistGuid = new Guid(id);
            }
            catch (Exception ex)
            {

            }

            Artists artists = db.Artists.Find(ArtistGuid);
            if (artists == null)
            {
                return HttpNotFound();
            }
            return View(artists);
        }

        //
        // GET: /ArtistManager/Create

        public ActionResult Create()
        {
            var newArtist = new Artists();
            ViewBag.Countries = new MultiSelectList(db.Country, "id_country", "name", newArtist.SelectedCountries);
            ViewBag.Genres = new MultiSelectList(db.Genre, "id_genre", "name", newArtist.SelectedGenres);

            return View(newArtist);
        }

        //
        // POST: /ArtistManager/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Artists artists, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var pathToFolder = Server.MapPath("~/Content/Images/ArtistImages");
                    var fileName = file.FileName.Replace("+", "");
                    var totalPath = Path.Combine(pathToFolder, fileName);
                    if (file.ContentLength > 0)
                    {
                        string extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                        if (extension != ".jpg" && extension != ".png" && extension != ".gif")
                        {
                            ModelState.AddModelError("uploadError", "Supported file extensions: jpg, png, gif");
                            return View(artists);
                        }
                        artists.image = fileName;
                        file.SaveAs(totalPath);
                    }
                }

                foreach (var country in artists.SelectedCountries)
                {
                    var dbCountry = db.Country.Find(new Guid(country));
                    if (dbCountry != null)
                        artists.Country.Add(dbCountry);
                }
                foreach (var genre in artists.SelectedGenres)
                {
                    var dbGenre = db.Genre.Find(new Guid(genre));
                    if (dbGenre != null)
                        artists.Genre.Add(dbGenre);
                }

                artists.artist_id = Guid.NewGuid();



                db.Artists.Add(artists);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artists);
        }

        //
        // GET: /ArtistManager/Edit/5

        public ActionResult Edit(string id = null)
        {
            Guid ArtistGuid = Guid.Empty;
            try
            {
                ArtistGuid = new Guid(id);
            }
            catch (Exception ex)
            {

            }


            Artists artists = db.Artists.Find(ArtistGuid);
            if (artists == null)
            {
                return HttpNotFound();
            }
            return View(artists);
        }

        //
        // POST: /ArtistManager/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Artists artists)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artists).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artists);
        }

        //
        // GET: /ArtistManager/Delete/5

        public ActionResult Delete(string id = null)
        {
            Guid ArtistGuid = Guid.Empty;
            try
            {
                ArtistGuid = new Guid(id);
            }
            catch (Exception ex)
            {

            }


            Artists artists = db.Artists.Find(ArtistGuid);
            if (artists == null)
            {
                return HttpNotFound();
            }
            return View(artists);
        }

        //
        // POST: /ArtistManager/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Guid ArtistGuid = Guid.Empty;
            try
            {
                ArtistGuid = new Guid(id);
            }
            catch (Exception ex)
            {

            }

            Artists artists = db.Artists.Find(ArtistGuid);
            db.Artists.Remove(artists);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}