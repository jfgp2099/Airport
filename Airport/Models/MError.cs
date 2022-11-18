using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nettatica.Models
{
    /// <summary>
    /// Se agrega el modelo MError para el manejo de errores del API
    /// </summary>
    public class MError
    {
        public bool Error { get; set; }
        public string Mensaje { get; set; }
    }
}