namespace Nettatica.Models.DataModels
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Vuelo")]
    public partial class Vuelo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vuelo()
        {
            Reserva = new HashSet<Reserva>();
        }

        [Key]

        public int IdVuelo { get; set; }

        public DateTime FechaSalida { get; set; }

        public int AeropuertoOrigen { get; set; }

        public DateTime FechaLlegada { get; set; }

        public int AeropuertoDestino { get; set; }

        public int IdAerolinea { get; set; }

        [JsonIgnore]
        public virtual Aerolinea Aerolinea { get; set; }

        [JsonIgnore]
        public virtual Aeropuerto Aeropuerto { get; set; }

        [JsonIgnore]
        public virtual Aeropuerto Aeropuerto1 { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reserva> Reserva { get; set; }
    }
}
