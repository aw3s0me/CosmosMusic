using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmosMusic.Models;

namespace CosmosMusic.Views
{
    public class UserManagerController : Controller
    {
        private korovin_idzEntities db = new korovin_idzEntities();

        //
        // GET: /UserManager/

        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Role);
            return View(users.ToList());
        }

        //
        // GET: /UserManager/Details/5

        public ActionResult Details(Guid id = null)
        {
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        //
        // GET: /UserManager/Create

        public ActionResult Create()
        {
            ViewBag.id_role = new SelectList(db.Role, "id_role", "name");
            return View();
        }

        //
        // POST: /UserManager/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users users)
        {
            if (ModelState.IsValid)
            {
                users.user_id = Guid.NewGuid();
                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_role = new SelectList(db.Role, "id_role", "name", users.id_role);
            return View(users);
        }

        //
        // GET: /UserManager/Edit/5

        public ActionResult Edit(Guid id = null)
        {
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_role = new SelectList(db.Role, "id_role", "name", users.id_role);
            return View(users);
        }

        //
        // POST: /UserManager/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_role = new SelectList(db.Role, "id_role", "name", users.id_role);
            return View(users);
        }

        //
        // GET: /UserManager/Delete/5

        public ActionResult Delete(Guid id = null)
        {
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        //
        // POST: /UserManager/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
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