namespace EliteMenu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ingredienti")]
    public partial class Ingredienti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ingredienti()
        {
            Piatti = new HashSet<Piatti>();
        }

        [Key]
        public int IngredienteID { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public string Descrizione { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Piatti> Piatti { get; set; }
    }
}
