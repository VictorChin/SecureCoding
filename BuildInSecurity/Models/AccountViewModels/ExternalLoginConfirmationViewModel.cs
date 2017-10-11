using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuildInSecurity.Models.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Favorite Airport")]
        [MaxLength(30, ErrorMessage = ("30 char max airport name"))]
        [Required]
        public string FavoriteAirport { get; set; }
    }
}
