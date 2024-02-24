using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationMVP.Models.ViewModels
{
    public class UpdateMedicalProcedure
    {
        //This viewmodel is a class which stores information that we need to present to /MedicalProcedure/Update/{}

        //the existing medicalprocedure information

        public MedicalProcedureDto SelectedMedicalProcedure { get; set; }

        // all species to choose from when updating this medicalprocedure

        public IEnumerable<DoctorDto> DoctorOptions { get; set; }
    }
}