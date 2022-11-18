using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nettatica.Models
{
    /// <summary>
    /// Se agrega el reservaview para complementar la documentacion del api
    /// </summary>


    public class ReservaView
    {
        public int IdReserva { get; set; }
        public int IdVuelo { get; set; }
        public DateTime FechaLlegada { get; set; }
        public string AeropuertoOrigen { get; set; }
        public string AeropuertoDestino { get; set; }
        public string Aerolinea { get; set; }
        public int IdCliente { get; set; }
        public string Precio { get; set; }

}
}