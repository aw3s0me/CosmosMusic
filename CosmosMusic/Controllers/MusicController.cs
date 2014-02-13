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

       /* public ActionResult PartialAlbums(int page)
        {

        } */


        public ActionResult Albums(string id)
        {
            Guid ArtistGuid = Guid.Empty;
            ViewBag.alreadyFavorite = false;
            try
            {
                ArtistGuid = new Guid(id);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }

            var artist = AlbumsContext.Artists.Find(ArtistGuid);
            ViewBag.Albums = (from s in AlbumsContext.Song
                        from c in s.Artists
                        where c.artist_id == artist.artist_id
                        select s.Albums).Distinct().ToList();

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;
                var user = AlbumsContext.Users.Where(x => x.username == username).FirstOrDefault();

                if (user.Artists.Contains(artist))
                {
                    ViewBag.alreadyFavorite = true;
                }
            }


            return View(artist);
        }

        public ActionResult AlbumDetail(string id)
        {
            Guid AlbumGuid = Guid.Empty;
            try
            {
                AlbumGuid = new Guid(id);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }

            var album = AlbumsContext.Albums.Find(AlbumGuid);

            foreach (var sng in album.Song)
            {
                foreach (var artist in sng.Artists)
                {
                    album.Artists.Add(artist); //Should be unique
                }
            }
            
            foreach (var artist in album.Artists)
            {
                foreach (var genre in artist.Genre)
                {
                    album.Genres.Add(genre);
                }
            }


            return View(album);
        }

        [HttpPost]
        public JsonResult History()
        {
            try
            {
                var boolean = Request["isEnded"];
                var trackName = Request["trackname"];
                var albumId = Request["albumId"];
                var album = AlbumsContext.Albums.Find(new Guid(albumId));
                var song = album.Song.ToList().Where(x => x.song_name == trackName).FirstOrDefault();

                var historyEntry = new History();
                historyEntry.history_id = Guid.NewGuid();
                historyEntry.listening_time = DateTime.Now;
                historyEntry.Song = song;
                historyEntry.song_id = song.song_id;

                var username = User.Identity.Name;
                var user = AlbumsContext.Users.Where(x => x.username == username).FirstOrDefault();

                historyEntry.user_id = user.user_id;
                historyEntry.Users = user;

                AlbumsContext.History.Add(historyEntry);
                AlbumsContext.SaveChanges();

                var result = new { Success = "True", Message = "Success Message" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                var result = new { Success = "False", Message = "Error Message" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Favorite(string Id, bool remove)
        {
            try
            {

                //var artistId = Request["albumId"];
                var artist = AlbumsContext.Artists.Find(new Guid(Id));

                var userName = User.Identity.Name;
                var user = AlbumsContext.Users.Where(x => x.username == userName).FirstOrDefault();

                if (!remove)
                {
                    user.Artists.Add(artist);
                }
                else
                {
                    user.Artists.Remove(artist);
                }

                AlbumsContext.SaveChanges();
                var result = new { Success = "True", Message = "Success Message" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                var result = new { Success = "False", Message = "Error Message" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

    }
}
