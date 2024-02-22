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
using System.Diagnostics;

namespace PassionProjectMVP.Controllers
{
    public class MedicalProcedureDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all medicalprocedures in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all medicalprocedures in the database, including their associated doctors.
        /// </returns>
        /// <example>
        /// GET: api/MedicalProcedureData/ListMedicalProcedures
        /// </example>
        [HttpGet]
        [ResponseType(typeof(MedicalProcedureDto))]
        public IHttpActionResult ListMedicalProcedures()
        {
            List<MedicalProcedure> MedicalProcedures = db.MedicalProcedures.ToList();
            List<MedicalProcedureDto> MedicalProcedureDtos = new List<MedicalProcedureDto>();

            MedicalProcedures.ForEach(a => MedicalProcedureDtos.Add(new MedicalProcedureDto()
            {
                MedicalProcedureID = a.MedicalProcedureID,
                MedicalProcedureName = a.MedicalProcedureName,
                MedicalCenter = a.MedicalCenter,
                DoctorID = a.Doctor.DoctorID,
                DoctorFirstName = a.Doctor.DoctorFirstName,
                DoctorLastName = a.Doctor.DoctorLastName,
                PatientID = a.Patient.PatientID,
                PatientFirstName = a.Patient.PatientFirstName,
                PatientLastName = a.Patient.PatientLastName
            }));

            return Ok(MedicalProcedureDtos);
        }

        /// <summary>
        /// Gathers information about all medicalprocedures related to a particular doctors ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all medicalprocedures in the database, including their associated doctors matched with a particular doctors ID
        /// </returns>
        /// <param name="id">Doctors ID.</param>
        /// <example>
        /// GET: api/MedicalProcedureData/ListMedicalProceduresForDoctors/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(MedicalProcedureDto))]
        public IHttpActionResult ListMedicalProceduresForDoctor(int id)
        {
            //SQL Equivalent:
            //Select * from medicalprocedures where medicalprocedures.doctorsid = {id}
            List<MedicalProcedure> MedicalProcedures = db.MedicalProcedures.Where(a => a.DoctorID == id).ToList();
            List<MedicalProcedureDto> MedicalProcedureDtos = new List<MedicalProcedureDto>();

            MedicalProcedures.ForEach(a => MedicalProcedureDtos.Add(new MedicalProcedureDto()
            {
                MedicalProcedureID = a.MedicalProcedureID,
                MedicalProcedureName = a.MedicalProcedureName,
                MedicalCenter = a.MedicalCenter,
                DoctorID = a.Doctor.DoctorID,
                DoctorFirstName = a.Doctor.DoctorFirstName,
                DoctorLastName = a.Doctor.DoctorLastName,
                PatientID = a.Patient.PatientID,
                PatientFirstName = a.Patient.PatientFirstName,
                PatientLastName = a.Patient.PatientLastName
            }));

            return Ok(MedicalProcedureDtos);
        }

        /// <summary>
        /// Gathers information about medicalprocedures related to a particular patient
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all medicalprocedures in the database, including their associated doctors that match to a particular patient id
        /// </returns>
        /// <param name="id">Patient ID.</param>
        /// <example>
        /// GET: api/MedicalProcedureData/ListMedicalProceduresForPatient/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(MedicalProcedureDto))]
        public IHttpActionResult ListMedicalProceduresForPatient(int id)
        {
            //SQL equivalent:
            //select medicalprocedures.*, patientmedicalprocedures.* from medicalprocedures INNER JOIN 
            //patientmedicalprocedures on medicalprocedures.medicalprocedureid = patientmedicalprocedures.medicalprocedureid
            //where patientmedicalprocedures.patientid={KEEPERID}

            //all medicalprocedures that have patients which match with our ID
            List<MedicalProcedure> MedicalProcedures = db.MedicalProcedures.Where(
                a=> a.Patients.Any(
                    k => k.PatientID == id
                )).ToList();
            List<MedicalProcedureDto> MedicalProcedureDtos = new List<MedicalProcedureDto>();

            MedicalProcedures.ForEach(a => MedicalProcedureDtos.Add(new MedicalProcedureDto()
            {
                MedicalProcedureID = a.MedicalProcedureID,
                MedicalProcedureName = a.MedicalProcedureName,
                MedicalCenter = a.MedicalCenter,
                DoctorID = a.Doctor.DoctorID,
                DoctorFirstName = a.Doctor.DoctorFirstName,
                DoctorLastName = a.Doctor.DoctorLastName,
                PatientID = a.Patient.PatientID,
                PatientFirstName = a.Patient.PatientFirstName,
                PatientLastName = a.Patient.PatientLastName
            }));

            return Ok(MedicalProcedureDtos);
        }


        /// <summary>
        /// Associates a particular patient with a particular medicalprocedure
        /// </summary>
        /// <param name="medicalprocedureid">The medicalprocedure ID primary key</param>
        /// <param name="patientid">The patient ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/MedicalProcedureData/AssociateMedicalProcedureWithPatient/9/1
        /// </example>
        [HttpPost]
        [Route("api/MedicalProcedureData/AssociateMedicalProcedureWithPatient/{medicalprocedureid}/{patientid}")]
        public IHttpActionResult AssociateMedicalProcedureWithPatient(int medicalprocedureid, int patientid)
        {

            MedicalProcedure SelectedMedicalProcedure = db.MedicalProcedures.Include(a => a.Patients).Where(a => a.MedicalProcedureID == medicalprocedureid).FirstOrDefault();
            Patient SelectedPatient = db.Patients.Find(patientid);

            if (SelectedMedicalProcedure == null || SelectedPatient == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input medicalprocedure id is: " + medicalprocedureid);
            Debug.WriteLine("selected medicalprocedure name is: " + SelectedMedicalProcedure.MedicalProcedureName);
            Debug.WriteLine("input patient id is: " + patientid);
            Debug.WriteLine("selected patient name is: " + SelectedPatient.PatientFirstName);

            //SQL equivalent:
            //insert into patientmedicalprocedures (medicalprocedureid, patientid) values ({aid},{kid})

            SelectedMedicalProcedure.Patients.Add(SelectedPatient);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an association between a particular patient and a particular medicalprocedure
        /// </summary>
        /// <param name="medicalprocedureid">The medicalprocedure ID primary key</param>
        /// <param name="patientid">The patient ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/MedicalProcedureData/AssociateMedicalProcedureWithPatient/9/1
        /// </example>
        [HttpPost]
        [Route("api/MedicalProcedureData/UnAssociateMedicalProcedureWithPatient/{medicalprocedureid}/{patientid}")]
        public IHttpActionResult UnAssociateMedicalProcedureWithPatient(int medicalprocedureid, int patientid)
        {

            MedicalProcedure SelectedMedicalProcedure = db.MedicalProcedures.Include(a => a.Patients).Where(a => a.MedicalProcedureID == medicalprocedureid).FirstOrDefault();
            Patient SelectedPatient = db.Patients.Find(patientid);

            if (SelectedMedicalProcedure == null || SelectedPatient == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input medicalprocedure id is: " + medicalprocedureid);
            Debug.WriteLine("selected medicalprocedure name is: " + SelectedMedicalProcedure.MedicalProcedureName);
            Debug.WriteLine("input patient id is: " + patientid);
            Debug.WriteLine("selected patient name is: " + SelectedPatient.PatientFirstName);

            //todo: verify that the patient actually is keeping track of the medicalprocedure

            SelectedMedicalProcedure.Patients.Remove(SelectedPatient);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Returns all medicalprocedures in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An medicalprocedure in the system matching up to the medicalprocedure ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the medicalprocedure</param>
        /// <example>
        /// GET: api/MedicalProcedureData/FindMedicalProcedure/5
        /// </example>
        [ResponseType(typeof(MedicalProcedureDto))]
        [HttpGet]
        public IHttpActionResult FindMedicalProcedure(int id)
        {
            MedicalProcedure MedicalProcedure = db.MedicalProcedures.Find(id);
            MedicalProcedureDto MedicalProcedureDto = new MedicalProcedureDto()
            {
                MedicalProcedureID = MedicalProcedure.MedicalProcedureID,
                MedicalProcedureName = MedicalProcedure.MedicalProcedureName,
                MedicalCenter = MedicalProcedure.MedicalCenter,
                DoctorID = MedicalProcedure.Doctor.DoctorID,
                DoctorFirstName = MedicalProcedure.Doctor.DoctorFirstName,
                DoctorLastName = MedicalProcedure.Doctor.DoctorLastName,
                PatientID = MedicalProcedure.Patient.PatientID,
                PatientFirstName = MedicalProcedure.Patient.PatientFirstName,
                PatientLastName = MedicalProcedure.Patient.PatientLastName
            };
            if (MedicalProcedure == null)
            {
                return NotFound();
            }

            return Ok(MedicalProcedureDto);
        }

        /// <summary>
        /// Updates a particular medicalprocedure in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the MedicalProcedure ID primary key</param>
        /// <param name="medicalprocedure">JSON FORM DATA of an medicalprocedure</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/MedicalProcedureData/UpdateMedicalProcedure/5
        /// FORM DATA: MedicalProcedure JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateMedicalProcedure(int id, MedicalProcedure medicalprocedure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != medicalprocedure.MedicalProcedureID)
            {

                return BadRequest();
            }

            db.Entry(medicalprocedure).State = EntityState.Modified;

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

        /// <summary>
        /// Adds an medicalprocedure to the system
        /// </summary>
        /// <param name="medicalprocedure">JSON FORM DATA of an medicalprocedure</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: MedicalProcedure ID, MedicalProcedure Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/MedicalProcedureData/AddMedicalProcedure
        /// FORM DATA: MedicalProcedure JSON Object
        /// </example>
        [ResponseType(typeof(MedicalProcedure))]
        [HttpPost]
        public IHttpActionResult AddMedicalProcedure(MedicalProcedure medicalprocedure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MedicalProcedures.Add(medicalprocedure);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = medicalprocedure.MedicalProcedureID }, medicalprocedure);
        }

        /// <summary>
        /// Deletes an medicalprocedure from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the medicalprocedure</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/MedicalProcedureData/DeleteMedicalProcedure/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(MedicalProcedure))]
        [HttpPost]
        public IHttpActionResult DeleteMedicalProcedure(int id)
        {
            MedicalProcedure medicalprocedure = db.MedicalProcedures.Find(id);
            if (medicalprocedure == null)
            {
                return NotFound();
            }

            db.MedicalProcedures.Remove(medicalprocedure);
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

        private bool MedicalProcedureExists(int id)
        {
            return db.MedicalProcedures.Count(e => e.MedicalProcedureID == id) > 0;
        }
    }
}