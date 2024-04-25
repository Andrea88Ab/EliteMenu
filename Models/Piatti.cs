namespace EliteMenu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Piatti")]
    public partial class Piatti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Piatti()
        {
           
            Allergeni = new HashSet<Allergeni>();
            Ingredienti = new HashSet<Ingredienti>();
        }

        [Key]
        public int PiattoID { get; set; }

        public int MenuID { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }


        [StringLength(100)]

        public string Descrizione { get; set; }

        public decimal Prezzo { get; set; }

        [StringLength(100)]
        public string Sezione { get; set; }

        [StringLength(100)]
        public string Categoria { get; set; }

        [StringLength(100)]
        public string Foto { get; set; }

       

        public virtual Menu Menu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Allergeni> Allergeni { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ingredienti> Ingredienti { get; set; }
    }
}
