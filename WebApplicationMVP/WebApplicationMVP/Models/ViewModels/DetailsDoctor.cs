using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationMVP.Models.ViewModels
{
    public class DetailsDoctor
    {
        //the species itself that we want to display
        public DoctorDto SelectedDoctor { get; set; }

        //all of the related animals to that particular species
        public IEnumerable<MedicalProcedureDto> RelatedMedicalProcedures { get; set; }
    }
}