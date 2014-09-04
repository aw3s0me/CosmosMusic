using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmosMusic.Models;

namespace CosmosMusic.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        korovin_idzEntities db = new korovin_idzEntities();

        public ActionResult Index()
        {
            try
            {
                //var albums = db.Albums.ToList().OrderBy(x => x.add_date).Skip(Math.Max(0, db.Albums.ToList().Count() - 5));
                var albums = db.Albums.ToList().OrderBy(x => x.add_date).Take(5);
                foreach (var album in albums)
                {
                    foreach (var sng in album.Song)
                    {
                        foreach (var artist in sng.Artists)
                        {
                            album.Artists.Add(artist); //Should be unique
                        }
                    }
                }
                return View(albums);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }

            
        }

    }
}
