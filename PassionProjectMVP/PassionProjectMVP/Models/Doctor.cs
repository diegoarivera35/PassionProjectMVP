using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PassionProjectMVP.Models;

namespace PassionProjectMVP.Models
{
    public class Doctor
    {
        [Key]

        public int DoctorID { get; set; }

        public string DoctorFirstName { get; set; }

        public string DoctorLastName { get; set; }

        public string DoctorEmail { get; set; }

        public int DoctorPhone { get; set; }

        public string DoctorSpecialization { get; set; }

        // Navigation property
        public virtual ICollection<MedicalProcedure> MedicalProcedures { get; set; }

    }

    public class DoctorDto
    {

        public int DoctorID { get; set; }

        public string DoctorFirstName { get; set; }

        public string DoctorLastName { get; set; }

        public string DoctorEmail { get; set; }

        public int DoctorPhone { get; set; }

        public string DoctorSpecialization { get; set; }

        // Navigation property
        public virtual ICollection<MedicalProcedure> MedicalProcedures { get; set; }

    }
}