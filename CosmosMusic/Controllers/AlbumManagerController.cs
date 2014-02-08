using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
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
            var emptyAlbum = new Albums();
            ViewBag.Artists = new MultiSelectList(db.Artists, "artist_id", "name", emptyAlbum.SelectedArtists);
            return View(emptyAlbum);
        } 

        /*
        [AcceptVerbs(HttpVerbs.Post)]
        [ActionName("Create")]
        public ActionResult Create_Post()
        {
            if (Request.Files.Count == 0)
            {
                ViewBag.user_id = new SelectList(db.Users, "user_id", "name");
                return View();
            }


            HttpPostedFileBase myFile = Request.Files["imageFile"];
            bool isUploaded = false;
            string message = "File upload failed";

            if (myFile != null && myFile.ContentLength != 0)
            {
                string pathForSaving = Server.MapPath("~/Content/Images/Covers");

                try
                {
                    myFile.SaveAs(Path.Combine(pathForSaving, myFile.FileName));
                    isUploaded = true;
                    message = "File uploaded successfully!";
                }
                catch (Exception ex)
                {
                    message = string.Format("File upload failed: {0}", ex.Message);
                }

            }
            ViewBag.user_id = new SelectList(db.Users, "user_id", "name");

            return Json(new { isUploaded = isUploaded, message = message }, "text/html");
        } */

        [HttpPost]
        public ActionResult Create(Albums albums, IEnumerable<HttpPostedFileBase> files)
        {
            
            if (ModelState.IsValid)
            {
                var songList = new List<Song>();
                albums.album_id = Guid.NewGuid();
                albums.add_date = DateTime.Now;
                albums.rating = 0;
                var choosedUser = db.Users.Find(albums.user_id);
                if (choosedUser == null) {

                }
                else {
                    albums.Users = choosedUser;
                }
                bool isImageUploaded = false;
                var pathToFolder = Server.MapPath("~/Content/Uploads");
                var folderNameFromAlbumName = MakeValidFileName(albums.name.Replace(" ", "_"));
                var totalPath = Path.Combine(pathToFolder, folderNameFromAlbumName);
                if (!CreateFolder(totalPath)){
                    ModelState.AddModelError("uploadError", "This album already exists");
                    return View(albums);
                }
                //var files = Request["file"] as IEnumerable<HttpPostedFileBase>;
                foreach (var file in files)
                {
                    int i = 0;
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        //string extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                        string extension = Path.GetExtension(file.FileName).ToLower();
                        if (extension != ".jpg" && extension != ".png" && extension != ".gif" && extension != ".mp3")
                        {
                            ModelState.AddModelError("uploadError", "Supported file extensions: pdf, doc, docx, rtf, txt");
                            return View(albums);
                        }

                        var path = Path.Combine(totalPath, fileName);

                        if (extension == ".jpg" || extension == ".png" || extension == ".gif")
                        {
                            if (isImageUploaded)
                            {
                                continue;
                            }
                            else
                            {
                                albums.cover = path;
                                isImageUploaded = true;
                            }
                        }

                        if (extension == ".mp3")
                        {
                            var newSong = new Song();
                            newSong.album_id = albums.album_id;
                            newSong.song_id = Guid.NewGuid();
                            newSong.song_name = fileName;
                            newSong.song_path = path;
                            if (albums.SelectedArtists != null)
                            {
                                foreach (var artist in albums.SelectedArtists)
                                {
                                    var dbArtist = db.Artists.Find(new Guid(artist));
                                    if (dbArtist != null)
                                        newSong.Artists.Add(dbArtist);
                                }
                            }
                            songList.Add(newSong);
                            //db.Song.Add(newSong);
                            //db.SaveChanges();
                        }

                        file.SaveAs(path);
                    }
                }




                try
                {
                    db.Albums.Add(albums);
                    db.SaveChanges();

                    if (songList.Count > 0)
                    {
                        foreach (var newSong in songList)
                        {
                            newSong.Albums = albums;
                            db.Song.Add(newSong);
                        }
                        db.SaveChanges();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                return RedirectToAction("Index");
            }

            ViewBag.user_id = new SelectList(db.Users, "user_id", "name", albums.user_id);
            ViewBag.Artists = new MultiSelectList(db.Artists, "artist_id", "name", albums.SelectedArtists);
            
            return RedirectToAction("Index");

        }

        //
        // POST: /AlbumManager/Create
/*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Albums albums)
        {
            if (ModelState.IsValid)
            {
                albums.album_id = Guid.NewGuid();
                albums.add_date = DateTime.Now;
                albums.rating = 0;
                db.Albums.Add(albums);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.user_id = new SelectList(db.Users, "user_id", "name", albums.user_id);
            
            return View(albums);
        }

        */

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

        #region helper

        private bool CreateFolder(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)

                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }

        public string MakeValidFileName(string name)
        {
            var builder = new StringBuilder();
            var invalid = System.IO.Path.GetInvalidFileNameChars();
            foreach (var cur in name)
            {
                if (!invalid.Contains(cur))
                {
                    builder.Append(cur);
                }
            }
            return builder.ToString();
        }

        #endregion



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}