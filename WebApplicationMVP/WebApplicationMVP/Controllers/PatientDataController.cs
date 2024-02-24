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
using WebApplicationMVP.Models;
using System.Diagnostics;

namespace WebApplicationMVP.Controllers
{
    public class PatientDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Patients in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Patients in the database, including their associated doctor.
        /// </returns>
        /// <example>
        /// GET: api/PatientData/ListPatients
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PatientDto))]
        public IHttpActionResult ListPatients()
        {
            List<Patient> Patients = db.Patients.ToList();
            List<PatientDto> PatientDtos = new List<PatientDto>();

            Patients.ForEach(k => PatientDtos.Add(new PatientDto()
            {
                PatientID = k.PatientID,
                PatientFirstName = k.PatientFirstName,
                PatientLastName = k.PatientLastName,
                PatientNationality = k.PatientNationality,
                PatientPhone = k.PatientPhone,
                PatientEmail = k.PatientEmail,

            }));

            return Ok(PatientDtos);
        }

        /// <summary>
        /// Returns all Patients in the system associated with a particular medicalprocedure.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Patients in the database taking care of a particular medicalprocedure
        /// </returns>
        /// <param name="id">MedicalProcedure Primary Key</param>
        /// <example>
        /// GET: api/PatientData/ListPatientsForMedicalProcedure/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PatientDto))]
        public IHttpActionResult ListPatientsForMedicalProcedure(int id)
        {

            //SQL Equivalent:
            //select patients.*, patientmedicalprocedures.* from medicalprocedures inner join patientmedicalprocedures on patientmedicalprocedures.patientid = patients.patientid where
            //patientmedicalprocedures.medicalprocedureid = {id}

            List<Patient> Patients = db.Patients.Where(
                k => k.MedicalProcedures.Any(
                    a => a.MedicalProcedureID == id)
                ).ToList();
            List<PatientDto> PatientDtos = new List<PatientDto>();

            Patients.ForEach(k => PatientDtos.Add(new PatientDto()
            {
                PatientID = k.PatientID,
                PatientFirstName = k.PatientFirstName,
                PatientLastName = k.PatientLastName,
                PatientNationality = k.PatientNationality,
                PatientPhone = k.PatientPhone,
                PatientEmail = k.PatientEmail
            }));

            return Ok(PatientDtos);
        }


        /// <summary>
        /// Returns Patients in the system not caring for a particular medicalprocedure.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Patients in the database not taking care of a particular medicalprocedure
        /// </returns>
        /// <param name="id">MedicalProcedure Primary Key</param>
        /// <example>
        /// GET: api/PatientData/ListPatientsNotCaringForMedicalProcedure/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PatientDto))]
        public IHttpActionResult ListPatientsNotCaringForMedicalProcedure(int id)
        {
            List<Patient> Patients = db.Patients.Where(
                k => !k.MedicalProcedures.Any(
                    a => a.MedicalProcedureID == id)
                ).ToList();
            List<PatientDto> PatientDtos = new List<PatientDto>();

            Patients.ForEach(k => PatientDtos.Add(new PatientDto()
            {
                PatientID = k.PatientID,
                PatientFirstName = k.PatientFirstName,
                PatientLastName = k.PatientLastName,
                PatientNationality = k.PatientNationality,
                PatientPhone = k.PatientPhone,
                PatientEmail = k.PatientEmail
            }));

            return Ok(PatientDtos);
        }

        /// <summary>
        /// Returns all Patients in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Patient in the system matching up to the Patient ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Patient</param>
        /// <example>
        /// GET: api/PatientData/FindPatient/5
        /// </example>
        [ResponseType(typeof(PatientDto))]
        [HttpGet]
        public IHttpActionResult FindPatient(int id)
        {
            Patient Patient = db.Patients.Find(id);
            PatientDto PatientDto = new PatientDto()
            {
                PatientID = Patient.PatientID,
                PatientFirstName = Patient.PatientFirstName,
                PatientLastName = Patient.PatientLastName,
                PatientNationality = Patient.PatientNationality,
                PatientPhone = Patient.PatientPhone,
                PatientEmail = Patient.PatientEmail
            };
            if (Patient == null)
            {
                return NotFound();
            }

            return Ok(PatientDto);
        }

        /// <summary>
        /// Updates a particular Patient in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Patient ID primary key</param>
        /// <param name="Patient">JSON FORM DATA of an Patient</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/PatientData/UpdatePatient/5
        /// FORM DATA: Patient JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePatient(int id, Patient Patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Patient.PatientID)
            {

                return BadRequest();
            }

            db.Entry(Patient).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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

        /// <summary>
        /// Adds an Patient to the system
        /// </summary>
        /// <param name="Patient">JSON FORM DATA of an Patient</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Patient ID, Patient Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/PatientData/AddPatient
        /// FORM DATA: Patient JSON Object
        /// </example>
        [ResponseType(typeof(Patient))]
        [HttpPost]
        public IHttpActionResult AddPatient(Patient Patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Patients.Add(Patient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Patient.PatientID }, Patient);
        }

        /// <summary>
        /// Deletes an Patient from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Patient</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/PatientData/DeletePatient/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Patient))]
        [HttpPost]
        public IHttpActionResult DeletePatient(int id)
        {
            Patient Patient = db.Patients.Find(id);
            if (Patient == null)
            {
                return NotFound();
            }

            db.Patients.Remove(Patient);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientExists(int id)
        {
            return db.Patients.Count(e => e.PatientID == id) > 0;
        }
    }
}