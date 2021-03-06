//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CosmosMusic.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Artists
    {
        public Artists()
        {
            this.Song = new HashSet<Song>();
            this.Country = new HashSet<Country>();
            this.Users = new HashSet<Users>();
            this.Genre = new HashSet<Genre>();
        }
    
        public System.Guid artist_id { get; set; }

        [DisplayName("Name")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string name { get; set; }
        [DisplayName("Artist Image")]
        public string image { get; set; }
    
        public virtual ICollection<Song> Song { get; set; }
        public virtual ICollection<Country> Country { get; set; }
        public virtual ICollection<Users> Users { get; set; }
        public virtual ICollection<Genre> Genre { get; set; }

        public List<string> SelectedGenres { get; set; }
        public List<string> SelectedCountries { get; set; }
    }
}
