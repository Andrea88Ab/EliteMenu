namespace EliteMenu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Menu")]
    public partial class Menu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Menu()
        {
            Piatti = new HashSet<Piatti>();
        }

        public int MenuID { get; set; }

        public int RistoranteID { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public string Descrizione { get; set; }

        [StringLength(100)]
        public string Foto { get; set; }

        public virtual Ristoranti Ristoranti { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Piatti> Piatti { get; set; }
    }
}
