namespace Nettatica.Models.DataModels
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Reserva")]
    public partial class Reserva
    {
        [Key]
        public int IdReserva { get; set; }

        public int IdVuelo { get; set; }

        public int IdCliente { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Precio { get; set; }

        [JsonIgnore]
        public virtual Flight Vuelo { get; set; }
    }
}
