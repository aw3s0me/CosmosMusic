﻿using System;
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
            Guid ArtistGuid = Guid.Empty;
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
            var result = new { Success = "True", Message = "Error Message" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
