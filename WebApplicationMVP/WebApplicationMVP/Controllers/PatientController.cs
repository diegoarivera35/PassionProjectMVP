using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using WebApplicationMVP.Models;
using WebApplicationMVP.Models.ViewModels;
using System.Web.Script.Serialization;


namespace WebApplicationMVP.Controllers
{
    public class PatientController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PatientController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44376/api/");
        }

        // GET: Patient/List
        public ActionResult List()
        {
            //objective: communicate with our Patient data api to retrieve a list of Patients
            //curl https://localhost:44376/api/Patientdata/listpatients


            string url = "patientdata/listpatients";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<PatientDto> Patients = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;
            //Debug.WriteLine("Number of Patients received : ");
            //Debug.WriteLine(Patients.Count());


            return View(Patients);
        }

        // GET: Patient/Details/5
        public ActionResult Details(int id)
        {
            DetailsPatient ViewModel = new DetailsPatient();

            //objective: communicate with our Patient data api to retrieve one Patient
            //curl https://localhost:44376/api/Patientdata/findpatient/{id}

            string url = "patientdata/findPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            PatientDto SelectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;
            Debug.WriteLine("Patient received : ");
            Debug.WriteLine(SelectedPatient.PatientFirstName);

            ViewModel.SelectedPatient = SelectedPatient;

            //show all medicalprocedures under the care of this patient
            url = "medicalproceduredata/listmedicalproceduresforpatient/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<MedicalProcedureDto> KeptMedicalProcedures = response.Content.ReadAsAsync<IEnumerable<MedicalProcedureDto>>().Result;

            ViewModel.KeptMedicalProcedures = KeptMedicalProcedures;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Patient/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        public ActionResult Create(Patient Patient)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Patient.PatientName);
            //objective: add a new Patient into our system using the API
            //curl -H "Content-Type:application/json" -d @Patient.json https://localhost:44376/api/Patientdata/addPatient 
            string url = "patientdata/addpatient";


            string jsonpayload = jss.Serialize(Patient);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Patient/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "patientdata/findpatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto selectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;
            return View(selectedPatient);
        }

        // POST: Patient/Update/5
        [HttpPost]
        public ActionResult Update(int id, Patient Patient)
        {

            string url = "patientdata/updatepatient/" + id;
            string jsonpayload = jss.Serialize(Patient);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Patient/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "patientdata/findpatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto selectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;
            return View(selectedPatient);
        }

        // POST: Patient/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "patientdata/deletepatient/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}