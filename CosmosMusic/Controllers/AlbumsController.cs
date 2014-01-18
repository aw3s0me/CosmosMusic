using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmosMusic.Models;

namespace CosmosMusic2.Controllers
{
    public class AlbumsController : Controller
    {
        //
        // GET: /Album/

        private readonly korovin_idzEntities AlbumsContext = new korovin_idzEntities();

        public ActionResult Index()
        {
            var genres = AlbumsContext.Genre.ToList();

            return View(genres);
        }

        public ActionResult Browse(string genre)
        {
            //var albums = AlbumsContext.Artists.Where(p => p.Genre == genreEntity).ToList();
            var artistsByGenre = AlbumsContext.Genre.Where(p => p.name == genre).First().Artists.ToList();

            return View(artistsByGenre);
        }



    }
}
