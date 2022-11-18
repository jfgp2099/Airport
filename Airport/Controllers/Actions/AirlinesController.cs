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
    public class AerolineasController : ApiController
    {
        private DataModel db = new DataModel();

        // GET: api/Aerolineas
        public IQueryable<Aerolinea> GetAerolinea()
        {
            return db.Aerolinea;
        }

        // GET: api/Aerolineas/5
        [ResponseType(typeof(Aerolinea))]
        public IHttpActionResult GetAerolinea(int id)
        {
            Aerolinea aerolinea = db.Aerolinea.Find(id);
            if (aerolinea == null)
            {
                return NotFound();
            }

            return Ok(aerolinea);
        }

        // PUT: api/Aerolineas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAerolinea(int id, Aerolinea aerolinea)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aerolinea.IdAerolinea)
            {
                return BadRequest();
            }

            db.Entry(aerolinea).State = EntityState.Modified;

            try
            {
                if (db.Aerolinea.Where(a => a.Nombre == aerolinea.Nombre && a.IdAerolinea!= id).FirstOrDefault() != null)
                {
                    return Ok(new MError() { Error = true, Mensaje = "Ya existe el registro" });
                }
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AerolineaExists(id))
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

        // POST: api/Aerolineas
        [ResponseType(typeof(Aerolinea))]
        public IHttpActionResult PostAerolinea(Aerolinea aerolinea)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Aerolinea.Add(aerolinea);

            try
            {
                if(db.Aerolinea.Where(a=>a.Nombre == aerolinea.Nombre).FirstOrDefault() != null)
                {
                    return Ok(new MError() { Error = true, Mensaje = "Ya existe el registro con ese nombre" });
                }
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AerolineaExists(aerolinea.IdAerolinea))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aerolinea.IdAerolinea }, aerolinea);
        }

        // DELETE: api/Aerolineas/5
        [ResponseType(typeof(Aerolinea))]
        public IHttpActionResult DeleteAerolinea(int id)
        {
            Aerolinea aerolinea = db.Aerolinea.Find(id);
            if (aerolinea == null)
            {
                return NotFound();
            }

            db.Aerolinea.Remove(aerolinea);
            db.SaveChanges();

            return Ok(aerolinea);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AerolineaExists(int id)
        {
            return db.Aerolinea.Count(e => e.IdAerolinea == id) > 0;
        }
    }
}