using PassionProjectMVP.Models;
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
using PassionProjectMVP.Migrations;

namespace PassionProjectMVP.Controllers
{
    public class MedicalProcedureDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/MedicalProcedureData/ListMedicalProcedures
        [HttpGet]
        public IEnumerable<MedicalProcedureDto> ListMedicalProcedures()
        {
            List<MedicalProcedure> MedicalProcedures = db.MedicalProcedures.ToList();
            List<MedicalProcedureDto> MedicalProcedureDtos = new List<MedicalProcedureDto>();

            MedicalProcedures.ForEach(a => MedicalProcedureDtos.Add(new MedicalProcedureDto()
            {
                MedicalProcedureID = a.MedicalProcedureID,
                MedicalProcedureName = a.MedicalProcedureName,
                MedicalCenter = a.MedicalCenter,
                MedicalProcedureDate = a.MedicalProcedureDate,
                PatientID = a.Patient.PatientID,
                DoctorID = a.Doctor.DoctorID,
            }));

            return MedicalProcedureDtos;
        }

        // GET: api/MedicalProcedureData/FindMedicalProcedure/5

        [ResponseType(typeof(MedicalProcedure))]
        [HttpGet]
        public IHttpActionResult FindMedicalProcedure(int id)
        {
            MedicalProcedure MedicalProcedure = db.MedicalProcedures.Find(id);
            MedicalProcedureDto MedicalProcedureDto = new MedicalProcedureDto()
            {
                MedicalProcedureID = MedicalProcedure.MedicalProcedureID,
                MedicalProcedureName = MedicalProcedure.MedicalProcedureName,
                MedicalCenter = MedicalProcedure.MedicalCenter,
                MedicalProcedureDate = MedicalProcedure.MedicalProcedureDate,
                PatientID = MedicalProcedure.Patient.PatientID,
                DoctorID = MedicalProcedure.Doctor.DoctorID,
            };
            if (MedicalProcedure == null)
            {
                return NotFound();
            }

            return Ok(MedicalProcedureDto);
        }

        // POST: api/MedicalProcedureData/UpdateMedicalProcedure/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateMedicalProcedure(int id, MedicalProcedure medicalProcedure)
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

        // POST: api/MedicalProcedureData/AddMedicalProcedure
        [ResponseType(typeof(MedicalProcedure))]
        [HttpPost]
        public IHttpActionResult AddMedicalProcedure(MedicalProcedure medicalProcedure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MedicalProcedures.Add(medicalProcedure);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = medicalProcedure.MedicalProcedureID }, medicalProcedure);
        }

        // POST: api/MedicalProcedureData/DeleteMedicalProcedure/5
        [ResponseType(typeof(MedicalProcedure))]
        [HttpPost]
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