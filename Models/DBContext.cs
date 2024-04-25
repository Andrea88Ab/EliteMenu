using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace EliteMenu.Models
{
    public partial class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
        {
        }

        public virtual DbSet<Allergeni> Allergeni { get; set; }
      
        public virtual DbSet<CommentiRistoranti> CommentiRistoranti { get; set; }
        public virtual DbSet<Ingredienti> Ingredienti { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Piatti> Piatti { get; set; }
        public virtual DbSet<Ristoranti> Ristoranti { get; set; }
        public virtual DbSet<Utenti> Utenti { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Allergeni>()
                .HasMany(e => e.Piatti)
                .WithMany(e => e.Allergeni)
                .Map(m => m.ToTable("PiattiAllergeni").MapLeftKey("AllergeneID").MapRightKey("PiattoID"));

            modelBuilder.Entity<Ingredienti>()
                .HasMany(e => e.Piatti)
                .WithMany(e => e.Ingredienti)
                .Map(m => m.ToTable("PiattiIngredienti").MapLeftKey("IngredienteID").MapRightKey("PiattoID"));

            modelBuilder.Entity<Menu>()
                .HasMany(e => e.Piatti)
                .WithRequired(e => e.Menu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Piatti>()
                .Property(e => e.Prezzo)
                .HasPrecision(10, 2);

           

            modelBuilder.Entity<Ristoranti>()
                .HasMany(e => e.CommentiRistoranti)
                .WithRequired(e => e.Ristoranti)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ristoranti>()
                .HasMany(e => e.Menu)
                .WithRequired(e => e.Ristoranti)
                .WillCascadeOnDelete(false);

           

            modelBuilder.Entity<Utenti>()
                .HasMany(e => e.CommentiRistoranti)
                .WithRequired(e => e.Utenti)
                .HasForeignKey(e => e.UtenteID)
                .WillCascadeOnDelete(false);
        }
    }
}
