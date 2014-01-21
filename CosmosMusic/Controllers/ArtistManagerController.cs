using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
            return View();
        }

        //
        // POST: /ArtistManager/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Artists artists)
        {
            if (ModelState.IsValid)
            {
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