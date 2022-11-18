using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Nettatica.Models.DataModels;

namespace Nettatica.Controllers
{
    [Authorize]
    public class ReservasController : ApiController
    {
        private DataModel db = new DataModel();

        // GET: api/Reservas
        public IQueryable<Reserva> GetReserva()
        {
            return db.Reserva;
        }

        // GET: api/Reservas/5
        [ResponseType(typeof(Reserva))]
        public IHttpActionResult GetReserva(int id)
        {
            Reserva reserva = db.Reserva.Find(id);
            if (reserva == null)
            {
                return NotFound();
            }

            return Ok(reserva);
        }

        // PUT: api/Reservas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReserva(int id, Reserva reserva)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reserva.IdReserva)
            {
                return BadRequest();
            }

            db.Entry(reserva).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Reservas
        [ResponseType(typeof(Reserva))]
        public IHttpActionResult PostReserva(Reserva reserva)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Reserva.Add(reserva);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ReservaExists(reserva.IdReserva))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = reserva.IdReserva }, reserva);
        }

        // DELETE: api/Reservas/5
        [ResponseType(typeof(Reserva))]
        public IHttpActionResult DeleteReserva(int id)
        {
            Reserva reserva = db.Reserva.Find(id);
            if (reserva == null)
            {
                return NotFound();
            }

            db.Reserva.Remove(reserva);
            db.SaveChanges();

            return Ok(reserva);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReservaExists(int id)
        {
            return db.Reserva.Count(e => e.IdReserva == id) > 0;
        }
    }
}