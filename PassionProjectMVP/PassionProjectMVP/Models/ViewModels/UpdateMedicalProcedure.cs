using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProjectMVP.Models.ViewModels
{
    public class UpdateMedicalProcedure
    {
        //This viewmodel is a class which stores information that we need to present to /MedicalProcedure/Update/{}

        //the existing Medical Procedure information

        public MedicalProcedureDto SelectedMedicalProcedure { get; set; }


    }
}