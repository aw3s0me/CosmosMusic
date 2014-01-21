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
    public class AlbumManagerController : Controller
    {
        private korovin_idzEntities db = new korovin_idzEntities();

        //
        // GET: /AlbumManager/

        public ActionResult Index()
        {
            var albums = db.Albums.Include(a => a.Users);
            return View(albums.ToList());
        }

        //
        // GET: /AlbumManager/Details/5

        public ActionResult Details(string id = null)
        {
            Guid AlbumGuid = Guid.Empty;
            try
            {
                AlbumGuid = new Guid(id);
            }
            catch (Exception ex)
            {

            }

            Albums albums = db.Albums.Find(AlbumGuid);
            if (albums == null)
            {
                return HttpNotFound();
            }
            return View(albums);
        }

        //
        // GET: /AlbumManager/Create

        public ActionResult Create()
        {
            ViewBag.user_id = new SelectList(db.Users, "user_id", "name");
            return View();
        }

        //
        // POST: /AlbumManager/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Albums albums)
        {
            if (ModelState.IsValid)
            {
                albums.album_id = Guid.NewGuid();
                db.Albums.Add(albums);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.user_id = new SelectList(db.Users, "user_id", "name", albums.user_id);
            return View(albums);
        }

        //
        // GET: /AlbumManager/Edit/5

        public ActionResult Edit(string id = null)
        {
            Guid AlbumGuid = Guid.Empty;
            try
            {
                AlbumGuid = new Guid(id);
            }
            catch (Exception ex)
            {

            }

            Albums albums = db.Albums.Find(AlbumGuid);
            if (albums == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_id = new SelectList(db.Users, "user_id", "name", albums.user_id);
            return View(albums);
        }

        //
        // POST: /AlbumManager/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Albums albums)
        {
            if (ModelState.IsValid)
            {
                db.Entry(albums).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.user_id = new SelectList(db.Users, "user_id", "name", albums.user_id);
            return View(albums);
        }

        //
        // GET: /AlbumManager/Delete/5

        public ActionResult Delete(string id = null)
        {
            Guid AlbumGuid = Guid.Empty;
            try
            {
                AlbumGuid = new Guid(id);
            }
            catch (Exception ex)
            {

            }

            Albums albums = db.Albums.Find(AlbumGuid);
            if (albums == null)
            {
                return HttpNotFound();
            }
            return View(albums);
        }

        //
        // POST: /AlbumManager/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Guid AlbumGuid = Guid.Empty;
            try
            {
                AlbumGuid = new Guid(id);
            }
            catch (Exception ex)
            {

            }

            Albums albums = db.Albums.Find(AlbumGuid);
            db.Albums.Remove(albums);
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