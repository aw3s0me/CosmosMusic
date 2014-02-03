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
    
    public partial class Users
    {
        public Users()
        {
            this.Albums = new HashSet<Albums>();
            this.History = new HashSet<History>();
            this.Country = new HashSet<Country>();
            this.Artists = new HashSet<Artists>();
        }
        [DisplayName("User ID")]
        public System.Guid user_id { get; set; }
        [DisplayName("Name")]
        public string name { get; set; }
        [DisplayName("Birth date")]
        public Nullable<System.DateTime> birth_date { get; set; }
        [DisplayName("Username")]
        public string username { get; set; }
        [DisplayName("Password")]
        public string password { get; set; }
        [DisplayName("Rights")]
        public System.Guid id_role { get; set; }
        [DisplayName("User Email")]
        public string email { get; set; }
        public bool isRemember { get; set; }
    
        public virtual ICollection<Albums> Albums { get; set; }
        public virtual ICollection<History> History { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Country> Country { get; set; }
        public virtual ICollection<Artists> Artists { get; set; }
    }
}
