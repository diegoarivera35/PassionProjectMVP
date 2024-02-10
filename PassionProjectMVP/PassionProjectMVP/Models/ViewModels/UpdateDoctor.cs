using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProjectMVP.Models.ViewModels
{
    public class UpdateDoctor
    {
        //This viewmodel is a class which stores information that we need to present to /Doctor/Update/{}

        //the existing doctor information

        public DoctorDto SelectedDoctor { get; set; }


    }
}