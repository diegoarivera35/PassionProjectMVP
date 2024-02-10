using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProjectMVP.Models.ViewModels
{
    public class UpdatePatient
    {
        //This viewmodel is a class which stores information that we need to present to /Patient/Update/{}

        //the existing patient information

        public PatientDto SelectedPatient { get; set; }


    }
}