using Nettatica.Models;
using Nettatica.Models.DataModels;
using Nettatica.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Nettatica.Controllers
{
    /// <summary>
    /// Esta Controllador soluciona todas las peticiones de la prueba
    /// </summary>
    /// <remarks>
    /// Requiere autorizacion por token por seguridad (JWT)
    /// </remarks>
    [Authorize]
    public class PeticionesController : ApiController
    {
        private DataModel db = new DataModel();

        //POST: api/CreateVuelo
        
        /// <summary>
        /// Metodo para crear un vuelo
        /// </summary>
        /// <param name="vuelo">Un vuelo para crearlo en Bd</param>
        /// /// <returns>
        /// Un mensaje indicando si la creacion del vuelo fue exitosa
        /// </returns>
        [HttpPost]
        [ResponseType(typeof(MError))]
        [Route("api/CreateVuelo/")]
        public IHttpActionResult CreateVuelo(Vuelo vuelo)
        {
            // Se valida el modelo del request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MError mError= ValidarVuelo(vuelo);
            if (!mError.Error)
            {
                try
                {
                    //Se almacena el vuelo
                    db.Vuelo.Add(vuelo);
                    db.SaveChanges();
                    //Se guarda un mensaje en el modelo indicando que el vuelo se creo correctamente
                    mError.Mensaje = "El Vuelo se creo correctamente con el id "+(vuelo.IdVuelo);
                }
                catch (DbUpdateException e)
                {
                    //Se informa que hubo un error
                    mError.Error = true;
                    mError.Mensaje = "Ocurrio un error inesperado";
                }
            }
            return Ok(mError);
        }

        //POST: api/CreateReserva

        /// <summary>
        /// Metodo para crear una reserva
        /// </summary>
        /// <param name="reserva">Una reserva para crearla en Bd</param>
        /// /// <returns>
        /// Un mensaje indicando si la creacion de la reserva fue exitosa
        /// </returns>

        [HttpPost]
        [ResponseType(typeof(MError))]
        [Route("api/CreateReserva/")]
        public IHttpActionResult CreateReserva(Reserva reserva)
        {
            // Se valida el modelo del request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                
                MError mError = new MError();
                //Se valida si existe un vuelo con el id de vuelo de la reserva
                if (db.Vuelo.Count(e=>e.IdVuelo==reserva.IdVuelo) == 0)
                {
                    return Ok(new MError() {Error=true , Mensaje="No existe el numero de vuelo indicado." });
                }
                try
                {
                    //Se almacena la reserva
                    db.Reserva.Add(reserva);
                    db.SaveChanges();
                    //Se guarda un mensaje en el modelo indicando que la reserva se creo correctamente
                    mError.Mensaje = "La reserva se creo correctamente con el id " + (reserva.IdReserva);
                }
                catch (DbUpdateException e)
                {
                    //Se informa que hubo un error
                    mError.Error = true;
                    mError.Mensaje = "Ocurrio un error inesperado";
                }
            return Ok(mError);
        }

        //GET: api/GetReserva

        /// <summary>
        /// Metodo para crear una reserva
        /// </summary>
        /// <param name="idReserva">Identificador de la reserva que se quiere buscar</param>
        /// /// <returns>
        /// Los datos de la reserva extraidos de un view de la base de datos
        /// </returns>
        
        [HttpGet]
        [ResponseType(typeof(ReservaView))]
        [Route("api/GetReserva/")]
        public HttpResponseMessage GetReserva(int idReserva)
        {
            // Se valida el modelo del request
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            try
            {

                // Se valida si existe una reserva con id enviado
                if (db.Reserva.Count(r => r.IdReserva == idReserva) == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new MError() { Error = true, Mensaje = "No existe reserva con este id" });
                }
                
                DataTable table = null;
                //Se crea una instancia de clsprocedures para traer los datos de la reserva
                using (ClsProcedures procedimiento = new ClsProcedures())
                {
                    table = procedimiento.GetReservaView(1, idReserva.ToString());
                }
                //Se retornan los datos de la reserva
                return Request.CreateResponse(HttpStatusCode.OK, table);
            }
            catch(Exception e)
            {
                //Se informa que hubo un error
                return Request.CreateResponse(HttpStatusCode.OK, new MError() { Error = true, Mensaje = "Ocurrio un error inesperado" }) ;
            }
        }

        //GET: api/FilterReserva

        /// <summary>
        /// Metodo para buscar dentro de la reserva un dato de una columna especifica (filtro)
        /// </summary>
        /// <param name="idcolumn">Identificador de la columna que se quiere buscar</param>
        /// 1 IdReserva 
        /// 2 IdVuelo
        /// 3 FechaLlegada
        /// 4 AeropuertoOrigen
        /// 5 AeropuertoDestino
        /// 6 Aerolinea
        /// 7 IdCliente
        /// 8 Precio
        /// <param name="value">Valor a buscar</param>
        /// /// <returns>
        /// Los datos de la reserva extraidos de un view de la base de datos filtrados
        /// </returns>


        [HttpGet]
        [ResponseType(typeof(ReservaView))]
        [Route("api/FilterReserva/")]
        public HttpResponseMessage FilterReserva(int idcolumn, string value)
        {
            // Se valida el modelo del request
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            try
            {

                DataTable table = null;
                //Se crea una instancia de clsprocedures para traer los datos de la reserva
                using (ClsProcedures procedimiento = new ClsProcedures())
                {
                    table = procedimiento.GetReservaView(idcolumn, value);
                }
                //Se valida si hay datos en la tabla retornada
                if(table.Rows.Count == 0)
                {
                    //Se responde que no hay datos
                    return Request.CreateResponse(HttpStatusCode.OK, new MError() { Error = true, Mensaje = "No hay datos de esta busqueda" });
                }

                //Se retornan los datos de la vista
                return Request.CreateResponse(HttpStatusCode.OK, table);
            }
            catch (Exception e)
            {
                //Se informa que hubo un error
                return Request.CreateResponse(HttpStatusCode.OK, new MError() { Error = true, Mensaje = "Ocurrio un error inesperado" });
            }
        }

        /// <summary>
        /// Metodo para validar si los parametros del vuelo son coherentes.
        /// </summary>
        /// <param name="vuelo">Vuelo para ejecutar validaciones</param>
        /// /// <returns>
        /// Modelo informando si tiene error y cual
        /// </returns>

        public MError ValidarVuelo(Vuelo vuelo)
        {
            MError error = new MError();
            DateTime FechaActual = DateTime.Today;

            //Se valida que las fechas de llegada y de salida sean superiores a las actuales
            if (vuelo.FechaLlegada < FechaActual || vuelo.FechaSalida < FechaActual)
            {
                error.Error = true;
                error.Mensaje = "No se asignar fechas del pasado";
                return error;
            }
            //Se valida que la fecha de salida sea mayor que la de llegada
            if (vuelo.FechaLlegada <= vuelo.FechaSalida)
            {
                error.Error = true;
                error.Mensaje = "La fecha de llegada no puede ser menor o igual que la de salida";
                return error;
            }

            //Se valida que el aeropuerto de origen y destino sean distintos
            if (vuelo.AeropuertoOrigen == vuelo.AeropuertoDestino)
            {
                error.Error = true;
                error.Mensaje = "El aeropuerto de origen y destino deben ser diferentes";
                return error;
            }

            //Se valida que el aeropuerto de origen exista
            if (db.Aeropuerto.Count(e => e.IdAeropuerto == vuelo.AeropuertoOrigen) == 0)
            {
                error.Error = true;
                error.Mensaje = "El aeropuerto de origen no existe";
                return error;
            }

            //Se valida que el aeropuerto de destino exista
            if (db.Aeropuerto.Count(e => e.IdAeropuerto == vuelo.AeropuertoDestino) == 0)
            {
                error.Error = true;
                error.Mensaje = "El aeropuerto de destino no existe";
                return error;
            }

            //Se valida que la aerolinea exista
            if (db.Aerolinea.Count(e => e.IdAerolinea == vuelo.IdAerolinea) == 0)
            {
                error.Error = true;
                error.Mensaje = "La aerolinea no existe";
                return error;
            }
            return error;

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
