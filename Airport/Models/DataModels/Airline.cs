namespace Nettatica.Models.DataModels
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Airline")]
    public partial class Airline
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Airline()
        {
            Vuelo = new HashSet<Flight>();
        }

        [Key]
        public int IdAerolinea { get; set; }

        [Required]
        [StringLength(20)]
        public string Nombre { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Flight> Vuelo { get; set; }
    }
}
