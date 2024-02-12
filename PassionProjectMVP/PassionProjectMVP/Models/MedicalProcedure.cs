using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PassionProjectMVP.Models;

namespace PassionProjectMVP.Models
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
        //A Medical Procedure can only have one patient
        [ForeignKey("Patient")]

        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        //A Doctor can have multiple Medical Procedures
        //A Medical Procedure can only have one Doctor
        [ForeignKey("Doctor")]

        public int DoctorID { get; set; }
        public virtual Doctor Doctor { get; set; }


    }

    public class MedicalProcedureDto
    {
        public int MedicalProcedureID { get; set; }

        public string MedicalProcedureName { get; set; }
        public string MedicalCenter { get; set; }
        public DateTime MedicalProcedureDate { get; set; }

        public int PatientID { get; set; }

        public int DoctorID { get; set; }

    }
}