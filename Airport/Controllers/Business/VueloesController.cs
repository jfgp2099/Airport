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
    public class VueloesController : ApiController
    {
        private DataModel db = new DataModel();

        // GET: api/Vueloes
        public IQueryable<Vuelo> GetVuelo()
        {
            return db.Vuelo;
        }

        // GET: api/Vueloes/5
        [ResponseType(typeof(Vuelo))]
        public IHttpActionResult GetVuelo(int id)
        {
            Vuelo vuelo = db.Vuelo.Find(id);
            if (vuelo == null)
            {
                return NotFound();
            }

            return Ok(vuelo);
        }

        // PUT: api/Vueloes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVuelo(int id, Vuelo vuelo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vuelo.IdVuelo)
            {
                return BadRequest();
            }

            db.Entry(vuelo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VueloExists(id))
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

        // POST: api/Vueloes
        [ResponseType(typeof(Vuelo))]
        public IHttpActionResult PostVuelo(Vuelo vuelo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Vuelo.Add(vuelo);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (VueloExists(vuelo.IdVuelo))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = vuelo.IdVuelo }, vuelo);
        }

        // DELETE: api/Vueloes/5
        [ResponseType(typeof(Vuelo))]
        public IHttpActionResult DeleteVuelo(int id)
        {
            Vuelo vuelo = db.Vuelo.Find(id);
            if (vuelo == null)
            {
                return NotFound();
            }

            db.Vuelo.Remove(vuelo);
            db.SaveChanges();

            return Ok(vuelo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VueloExists(int id)
        {
            return db.Vuelo.Count(e => e.IdVuelo == id) > 0;
        }
    }
}