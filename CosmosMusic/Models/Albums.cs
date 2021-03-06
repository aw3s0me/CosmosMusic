
 
namespace CosmosMusic.Models 
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;

      public partial class Albums
      {
          public Albums()
          {
            this.Song = new HashSet<Song>();
 
            this.Artists = new HashSet<Artists>();
            foreach (var sng in this.Song)
            {
                 foreach (var artist in sng.Artists)
                {
                     this.Artists.Add(artist); //Should be unique
                }
            }
 
             #region Genre_collection
             this.Genres = new HashSet<Genre>();
 
             foreach (var artist in this.Artists)
             {
                 foreach (var genre in artist.Genre)
                 {
                     this.Genres.Add(genre);
                 }
             }
 
             #endregion
 
        }
        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "Album id is required")]
     
        public System.Guid album_id { get; set; }
        
        [DisplayName("Album name")]
        [StringLength(50)]
        [Required(ErrorMessage = "Album name is required")]
        public string name { get; set; }

        //[Required(ErrorMessage = "Add date is required")]
        [DisplayName("Added at")]
        public Nullable<System.DateTime> add_date { get; set; }

        //[Required(ErrorMessage = "Rating is required")]
        [DisplayName("Rating")]
        [ScaffoldColumn(false)]        
        public Nullable<int> rating { get; set; }

        //[Required(ErrorMessage = "Cover name is required")]
        [DisplayName("Cover name")]
        [StringLength(200, ErrorMessage = "String length must be no longer than 200 symbols")]
        public string cover { get; set; }

        [DisplayName("User added")]
        [Required(ErrorMessage = "Username is required")]
        public System.Guid user_id { get; set; }
        [DisplayName("Year")]
        [Required(ErrorMessage = "Year is required")]
        [Range(1900, 2015, ErrorMessage = "Year must be between 1900 and 2015")]
        public Nullable<int> year { get; set; }

        //public HttpPostedFileBase imageFile { get; set; }

        #region collections
     
        public virtual Users Users { get; set; }
        public virtual ICollection<Song> Song { get; set; }
        public virtual ICollection<Artists> Artists { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual List<string> SelectedArtists { get; set; }
        #endregion
 
        #region funcs
        public void GenreInit()
        {
            foreach (var artist in this.Artists)
            {
               foreach (var genre in artist.Genre)
               {
                    this.Genres.Add(genre);
               }
            }
        }
        #endregion
      }
  }