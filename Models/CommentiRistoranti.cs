namespace EliteMenu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommentiRistoranti")]
    public partial class CommentiRistoranti
    {
        [Key]
        public int CommentoID { get; set; }

        public int RistoranteID { get; set; }

        public int UtenteID { get; set; }

        [Required]
        public string Testo { get; set; }

        public int? Valutazione { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DataOra { get; set; }

        [Required]

        public string Titolo { get; set; }


        public virtual Ristoranti Ristoranti { get; set; }

        public virtual Utenti Utenti { get; set; }
    }
}
