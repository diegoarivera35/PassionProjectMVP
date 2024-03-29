﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationMVP.Models
{
    public class MedicalProcedure
    {
        [Key]
        public int MedicalProcedureID { get; set; }

        public string MedicalProcedureName { get; set; }

        public string MedicalCenter { get; set; }

        [Display(Name = "Procedure Date")]
        public DateTime MedicalProcedureDate { get; set; } = DateTime.Now;

        //A Patient can have multiple Medical Procedures
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        //A Doctor can have multiple Medical Procedures
        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }
        public virtual Doctor Doctor { get; set; }

        public ICollection<Patient> Patients { get; set; }
    }

    public class MedicalProcedureDto
    {
        public int MedicalProcedureID { get; set; }

        public string MedicalProcedureName { get; set; }
        public string MedicalCenter { get; set; }
        public DateTime MedicalProcedureDate { get; set; }

        public int PatientID { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }

        public int DoctorID { get; set; }

        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }

    }

}