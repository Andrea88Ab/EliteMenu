namespace EliteMenu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Utenti")]
    public partial class Utenti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Utenti()
        {
            
            CommentiRistoranti = new HashSet<CommentiRistoranti>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(256)]
        public string PasswordHash { get; set; }

        [StringLength(50)]
        public string Ruolo { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DataCreazione { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DataUltimaModifica { get; set; }

        public int? CreatoDa { get; set; }

        public int? ModificatoDa { get; set; }


        [StringLength(255)]
        public string Avatar { get; set; }




        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommentiRistoranti> CommentiRistoranti { get; set; }
    }
}
