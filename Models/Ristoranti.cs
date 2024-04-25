namespace EliteMenu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ristoranti")]
    public partial class Ristoranti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ristoranti()
        {
            CommentiRistoranti = new HashSet<CommentiRistoranti>();
            Menu = new HashSet<Menu>();
        }

        [Key]
        public int RistoranteID { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(255)]
        public string Indirizzo { get; set; }

        [StringLength(25)]
        public string Telefono { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        public string Descrizione { get; set; }

        [StringLength(100)]
        public string Foto { get; set; }

       

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommentiRistoranti> CommentiRistoranti { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Menu> Menu { get; set; }
    }
}
