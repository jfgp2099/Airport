namespace Nettatica.Models.DataModels
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Aerolinea")]
    public partial class Aerolinea
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Aerolinea()
        {
            Vuelo = new HashSet<Vuelo>();
        }

        [Key]
        public int IdAerolinea { get; set; }

        [Required]
        [StringLength(20)]
        public string Nombre { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vuelo> Vuelo { get; set; }
    }
}
