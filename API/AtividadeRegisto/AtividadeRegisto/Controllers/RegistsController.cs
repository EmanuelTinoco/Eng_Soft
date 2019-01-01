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
using AtividadeDataAcess;

namespace AtividadeRegisto.Controllers
{
    public class RegistsController : ApiController
    {
        private registoatividadeEntities db = new registoatividadeEntities();

        // GET: api/Regists
        public IQueryable<Registo> GetRegistoes()
        {
            return db.Registoes;
        }

        // GET: api/Regists/5
        [ResponseType(typeof(Registo))]
        public IHttpActionResult GetRegisto(int id)
        {
            Registo registo = db.Registoes.Find(id);
            if (registo == null)
            {
                return NotFound();
            }

            return Ok(registo);
        }

        // PUT: api/Regists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRegisto(int id, Registo registo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != registo.id)
            {
                return BadRequest();
            }

            db.Entry(registo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistoExists(id))
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

        // POST: api/Regists
        [ResponseType(typeof(Registo))]
        public IHttpActionResult PostRegisto(Registo registo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Registoes.Add(registo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = registo.id }, registo);
        }

        // DELETE: api/Regists/5
        [ResponseType(typeof(Registo))]
        public IHttpActionResult DeleteRegisto(int id)
        {
            Registo registo = db.Registoes.Find(id);
            if (registo == null)
            {
                return NotFound();
            }

            db.Registoes.Remove(registo);
            db.SaveChanges();

            return Ok(registo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RegistoExists(int id)
        {
            return db.Registoes.Count(e => e.id == id) > 0;
        }
    }
}