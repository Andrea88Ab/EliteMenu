using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EliteMenu.Models;


namespace EliteMenu.ViewModels
{
    public class RistoranteViewModel
    {

        public Ristoranti Ristorante { get; set; }
        //public List<string> FotoPiatti { get; set; }
        public List<PiattoViewModel> Piatti { get; set; } = new List<PiattoViewModel>();

    }
}