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

namespace PassionProjectMVP.Controllers
{
    public class DoctorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DoctorData/ListDoctors
        [HttpGet]
        public IEnumerable<DoctorDto> ListDoctors()
        {
            List<Doctor> Doctors = db.Doctors.ToList();
            List<DoctorDto> DoctorDtos = new List<DoctorDto>();

            Doctors.ForEach(a => DoctorDtos.Add(new DoctorDto()
            {
                DoctorID = a.DoctorID,
                DoctorFirstName = a.DoctorFirstName,
                DoctorLastName = a.DoctorLastName,
                DoctorEmail = a.DoctorEmail,
                DoctorPhone = a.DoctorPhone,
                DoctorSpecialization = a.DoctorSpecialization,
            }));

            return DoctorDtos;
        }

        // GET: api/DoctorData/FindDoctor/5
        [ResponseType(typeof(Doctor))]
        [HttpGet]
        public IHttpActionResult FindDoctor(int id)
        {
            Doctor Doctor = db.Doctors.Find(id);
            DoctorDto DoctorDto = new DoctorDto()
            {
                DoctorID = Doctor.DoctorID,
                DoctorFirstName = Doctor.DoctorFirstName,
                DoctorLastName = Doctor.DoctorLastName,
                DoctorEmail = Doctor.DoctorEmail,
                DoctorPhone = Doctor.DoctorPhone,
                DoctorSpecialization = Doctor.DoctorSpecialization,
            };
            if (Doctor == null)
            {
                return NotFound();
            }

            return Ok(DoctorDto);
        }

        // POST: api/DoctorData/UpdateDoctor/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDoctor(int id, Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doctor.DoctorID)
            {
                return BadRequest();
            }

            db.Entry(doctor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
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

        // POST: api/DoctorData/AddDoctor
        [ResponseType(typeof(Doctor))]
        [HttpPost]
        public IHttpActionResult AddDoctor(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Doctors.Add(doctor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = doctor.DoctorID }, doctor);
        }

        // DELETE: api/DoctorData/DeleteAnimal/5
        [ResponseType(typeof(Doctor))]
        [HttpPost]
        public IHttpActionResult DeleteDoctor(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }

            db.Doctors.Remove(doctor);
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

        private bool DoctorExists(int id)
        {
            return db.Doctors.Count(e => e.DoctorID == id) > 0;
        }
    }
}