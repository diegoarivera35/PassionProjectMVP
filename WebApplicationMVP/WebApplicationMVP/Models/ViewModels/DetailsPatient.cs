using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationMVP.Models.ViewModels
{
    public class DetailsPatient
    {

        public PatientDto SelectedPatient { get; set; }
        public IEnumerable<MedicalProcedureDto> KeptMedicalProcedures { get; set; }
    }
}