using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CosmosMusic.Models;

namespace CosmosMusic.ViewModels
{
    public class UserViewModel
    {
        [DisplayName("Name")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Username")]
        [Required]
        [StringLength(20, ErrorMessage = "Less than 20 characters")]
        public string Username { get; set; }
        [DisplayName("Birth date")]
        [Required]
        public DateTime? DateofBirth { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email Format is wrong")]
        [StringLength(20, ErrorMessage = "Less than 20 characters")]
        public string Email { get; set; }

        public virtual ICollection<Country> Country { get; set; }
        public virtual ICollection<Artists> Artists { get; set; }
        public virtual List<string> SelectedCountries { get; set; }
        public virtual List<string> SelectedArtists { get; set; }
 
    }
}