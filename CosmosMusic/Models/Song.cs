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
    
    public partial class Song
    {
        public Song()
        {
            this.History = new HashSet<History>();
            this.Artists = new HashSet<Artists>();
        }
    
        public System.Guid song_id { get; set; }
        public string song_path { get; set; }
        public System.Guid album_id { get; set; }
        public string song_name { get; set; }
    
        public virtual Albums Albums { get; set; }
        public virtual ICollection<History> History { get; set; }
        public virtual ICollection<Artists> Artists { get; set; }
    }
}
