using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProjectMVP.Models.ViewModels
{
    public class DetailsDoctor
    {

        public DoctorDto SelectedDoctor { get; set; }

        public IEnumerable<MedicalProcedureDto> RelatedMedicalProcedures { get; set; }


    }
}