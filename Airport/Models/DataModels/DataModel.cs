using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Nettatica.Models.DataModels
{
    public partial class DataModel : DbContext
    {
        public DataModel()
            : base("name=DataModel")
        {
        }

        public virtual DbSet<Aerolinea> Aerolinea { get; set; }
        public virtual DbSet<Aeropuerto> Aeropuerto { get; set; }
        public virtual DbSet<Reserva> Reserva { get; set; }
        public virtual DbSet<Vuelo> Vuelo { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aerolinea>()
                .Property(e => e.Nombre)
                .IsFixedLength();

            modelBuilder.Entity<Aerolinea>()
                .HasMany(e => e.Vuelo)
                .WithRequired(e => e.Aerolinea)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Aeropuerto>()
                .Property(e => e.Nombre)
                .IsFixedLength();

            modelBuilder.Entity<Aeropuerto>()
                .HasMany(e => e.Vuelo)
                .WithRequired(e => e.Aeropuerto)
                .HasForeignKey(e => e.AeropuertoDestino)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Aeropuerto>()
                .HasMany(e => e.Vuelo1)
                .WithRequired(e => e.Aeropuerto1)
                .HasForeignKey(e => e.AeropuertoOrigen)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Reserva>()
                .Property(e => e.Precio)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Vuelo>()
                .HasMany(e => e.Reserva)
                .WithRequired(e => e.Vuelo)
                .WillCascadeOnDelete(false);
        }
    }
}
