using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationMVP.Models.ViewModels
{
    public class DetailsMedicalProcedure
    {

        public MedicalProcedureDto SelectedMedicalProcedure { get; set; }
        public IEnumerable<PatientDto> ResponsiblePatients { get; set; }

        public IEnumerable<PatientDto> AvailablePatients { get; set; }
    }
}