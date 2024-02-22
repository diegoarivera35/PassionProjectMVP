using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProjectMVP.Models.ViewModels
{
    public class DetailsMedicalProcedure
    {

        public MedicalProcedureDto SelectedMedicalProcedure { get; set; }

        public IEnumerable<PatientDto> PatientsTakingService { get; set; }

        public IEnumerable<PatientDto> PatientsIntestedInServices { get; set; }

        public IEnumerable<DoctorDto> DoctorsGivingService { get; set; }

        public IEnumerable<DoctorDto> DoctorListedForService { get; set; }



    }
}