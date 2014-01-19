using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmosMusic.Models;

namespace CosmosMusic2.Controllers
{
    public class AlbumComparer : IEqualityComparer<Albums>
    {
        public bool Equals(Albums x, Albums y)
        {
            if (x.album_id == y.album_id)
            {
                return true;
            }
            else
                return false;
        }

        public int GetHashCode(Albums obj)
        {
            return obj.album_id.GetHashCode();
        }
    }

    public class MusicController : Controller
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

        public ActionResult Albums(string id)
        {
            Guid AlbumGuid = Guid.Empty;
            try
            {
                AlbumGuid = new Guid(id);
            }
            catch (Exception ex)
            {

            }

            var songsFound = AlbumsContext.Artists.Find(AlbumGuid).Song;
            //var albums = songsFound.ToList().GroupBy(p => p.Albums);
            /*var albums = from s in AlbumsContext.Artists
                         join m in AlbumsContext.Song on s.artist_id equals m. */
            //var albums = songsFound.To
            var albums = from u in AlbumsContext.A

            return View(albums);
        }

        public ActionResult AlbumDetail(string id)
        {
            Guid AlbumGuid = Guid.Empty;
            try
            {
                AlbumGuid = new Guid(id);
            }
            catch (Exception ex)
            {
             
            }


            return View();
        }

    }
}
