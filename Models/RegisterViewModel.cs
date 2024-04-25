using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EliteMenu.Models
{
    public class RegisterViewModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Il {0} deve essere almeno di {2} caratteri.", MinimumLength = 2)]
        public string Nome { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Le password non corrispondono.")]
        public string ConfirmPassword { get; set; }

        // Aggiungi un campo per il ruolo
        [Required]
        public string Ruolo { get; set; } = "user";

        public HttpPostedFileBase AvatarFile { get; set; }
    }
}