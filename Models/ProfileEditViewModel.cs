using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EliteMenu.Models
{
    public class ProfileEditViewModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }

        public string Avatar { get; set; }

        public HttpPostedFileBase AvatarFile { get; set; }
    }
}