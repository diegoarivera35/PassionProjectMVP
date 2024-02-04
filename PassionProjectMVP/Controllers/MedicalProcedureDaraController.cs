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
using PassionProjectMVP.Models;

namespace PassionProjectMVP.Controllers
{
    public class MedicalProcedureDaraController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/MedicalProcedureDara
        public IQueryable<MedicalProcedure> GetMedicalProcedures()
        {
            return db.MedicalProcedures;
        }

        // GET: api/MedicalProcedureDara/5
        [ResponseType(typeof(MedicalProcedure))]
        public IHttpActionResult GetMedicalProcedure(int id)
        {
            MedicalProcedure medicalProcedure = db.MedicalProcedures.Find(id);
            if (medicalProcedure == null)
            {
                return NotFound();
            }

            return Ok(medicalProcedure);
        }

        // PUT: api/MedicalProcedureDara/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMedicalProcedure(int id, MedicalProcedure medicalProcedure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != medicalProcedure.MedicalProcedureID)
            {
                return BadRequest();
            }

            db.Entry(medicalProcedure).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicalProcedureExists(id))
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

        // POST: api/MedicalProcedureDara
        [ResponseType(typeof(MedicalProcedure))]
        public IHttpActionResult PostMedicalProcedure(MedicalProcedure medicalProcedure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MedicalProcedures.Add(medicalProcedure);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = medicalProcedure.MedicalProcedureID }, medicalProcedure);
        }

        // DELETE: api/MedicalProcedureDara/5
        [ResponseType(typeof(MedicalProcedure))]
        public IHttpActionResult DeleteMedicalProcedure(int id)
        {
            MedicalProcedure medicalProcedure = db.MedicalProcedures.Find(id);
            if (medicalProcedure == null)
            {
                return NotFound();
            }

            db.MedicalProcedures.Remove(medicalProcedure);
            db.SaveChanges();

            return Ok(medicalProcedure);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MedicalProcedureExists(int id)
        {
            return db.MedicalProcedures.Count(e => e.MedicalProcedureID == id) > 0;
        }
    }
}