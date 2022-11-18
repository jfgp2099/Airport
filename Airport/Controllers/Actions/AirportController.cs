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
using Nettatica.Models;
using Nettatica.Models.DataModels;

namespace Nettatica.Controllers
{
    [Authorize]
    public class AeropuertoesController : ApiController
    {
        private DataModel db = new DataModel();

        // GET: api/Aeropuertoes
        public IQueryable<Aeropuerto> GetAeropuerto()
        {
            return db.Aeropuerto;
        }

        // GET: api/Aeropuertoes/5
        [ResponseType(typeof(Aeropuerto))]
        public IHttpActionResult GetAeropuerto(int id)
        {
            Aeropuerto aeropuerto = db.Aeropuerto.Find(id);
            if (aeropuerto == null)
            {
                return NotFound();
            }

            return Ok(aeropuerto);
        }

        // PUT: api/Aeropuertoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAeropuerto(int id, Aeropuerto aeropuerto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aeropuerto.IdAeropuerto)
            {
                return BadRequest();
            }

            db.Entry(aeropuerto).State = EntityState.Modified;

            try
            {
                if (db.Aeropuerto.Where(a => a.Nombre == aeropuerto.Nombre && a.IdAeropuerto != id).FirstOrDefault() != null)
                {
                    return Ok(new MError() { Error = true, Mensaje = "Ya existe una aeropuerto con este nombre" });
                }
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AeropuertoExists(id))
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

        // POST: api/Aeropuertoes
        [ResponseType(typeof(Aeropuerto))]
        public IHttpActionResult PostAeropuerto(Aeropuerto aeropuerto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Aeropuerto.Add(aeropuerto);

            try
            {
                if (db.Aeropuerto.Where(a => a.Nombre == aeropuerto.Nombre).FirstOrDefault() != null)
                {
                    return Ok(new MError() { Error = true, Mensaje = "Ya existe un aeropuerto con este nombre" });
                }
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AeropuertoExists(aeropuerto.IdAeropuerto))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aeropuerto.IdAeropuerto }, aeropuerto);
        }

        // DELETE: api/Aeropuertoes/5
        [ResponseType(typeof(Aeropuerto))]
        public IHttpActionResult DeleteAeropuerto(int id)
        {
            Aeropuerto aeropuerto = db.Aeropuerto.Find(id);
            if (aeropuerto == null)
            {
                return NotFound();
            }

            db.Aeropuerto.Remove(aeropuerto);
            db.SaveChanges();

            return Ok(aeropuerto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AeropuertoExists(int id)
        {
            return db.Aeropuerto.Count(e => e.IdAeropuerto == id) > 0;
        }
    }
}