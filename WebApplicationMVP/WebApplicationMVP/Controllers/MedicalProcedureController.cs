using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using WebApplicationMVP.Models;
using System.Web.Script.Serialization;
using WebApplicationMVP.Models.ViewModels;



namespace WebApplicationMVP.Controllers
{
    public class MedicalProcedureController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static MedicalProcedureController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44376/api/");
        }

        // GET: MedicalProcedure/List
        public ActionResult List()
        {
            //objective: communicate with our medicalprocedure data api to retrieve a list of medicalprocedures
            //curl https://localhost:44376/api/medicalproceduredata/listmedicalprocedures


            string url = "medicalproceduredata/listmedicalprocedures";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<MedicalProcedureDto> medicalprocedures = response.Content.ReadAsAsync<IEnumerable<MedicalProcedureDto>>().Result;
            //Debug.WriteLine("Number of medicalprocedures received : ");
            //Debug.WriteLine(medicalprocedures.Count());


            return View(medicalprocedures);
        }

        // GET: MedicalProcedure/Details/5
        public ActionResult Details(int id)
        {
            DetailsMedicalProcedure ViewModel = new DetailsMedicalProcedure();

            //objective: communicate with our medicalprocedure data api to retrieve one medicalprocedure
            //curl https://localhost:44376/api/medicalproceduredata/findmedicalprocedure/{id}

            string url = "medicalproceduredata/findmedicalprocedure/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            MedicalProcedureDto SelectedMedicalProcedure = response.Content.ReadAsAsync<MedicalProcedureDto>().Result;
            Debug.WriteLine("medicalprocedure received : ");
            Debug.WriteLine(SelectedMedicalProcedure.MedicalProcedureName);

            ViewModel.SelectedMedicalProcedure = SelectedMedicalProcedure;

            //show associated patients with this medicalprocedure
            url = "patientdata/listpatientsformedicalprocedure/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<PatientDto> ResponsiblePatients = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;

            ViewModel.ResponsiblePatients = ResponsiblePatients;

            url = "patientdata/listpatientsnotcaringformedicalprocedure/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<PatientDto> AvailablePatients = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;

            ViewModel.AvailablePatients = AvailablePatients;


            return View(ViewModel);
        }


        //POST: MedicalProcedure/Associate/{MedicalProcedureId}/{PatientID}
        [HttpPost]
        public ActionResult Associate(int id, int PatientID)
        {
            Debug.WriteLine("Attempting to associate medicalprocedure :" + id + " with patient " + PatientID);

            //call our api to associate medicalprocedure with patient
            string url = "medicalproceduredata/associatemedicalprocedurewithpatient/" + id + "/" + PatientID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //Get: MedicalProcedure/UnAssociate/{id}?PatientID={patientID}
        [HttpGet]
        public ActionResult UnAssociate(int id, int PatientID)
        {
            Debug.WriteLine("Attempting to unassociate medicalprocedure :" + id + " with patient: " + PatientID);

            //call our api to associate medicalprocedure with patient
            string url = "medicalproceduredata/unassociatemedicalprocedurewithpatient/" + id + "/" + PatientID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        public ActionResult Error()
        {

            return View();
        }

        // GET: MedicalProcedure/New
        public ActionResult New()
        {
            //information about all doctor in the system.
            //GET api/doctordata/listdoctors

            string url = "doctordata/listdoctors";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DoctorDto> DoctorOptions = response.Content.ReadAsAsync<IEnumerable<DoctorDto>>().Result;

            return View(DoctorOptions);
        }

        // POST: MedicalProcedure/Create
        [HttpPost]
        public ActionResult Create(MedicalProcedure medicalprocedure)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(medicalprocedure.MedicalProcedureName);
            //objective: add a new medicalprocedure into our system using the API
            //curl -H "Content-Type:application/json" -d @medicalprocedure.json https://localhost:44376/api/medicalproceduredata/addmedicalprocedure 
            string url = "medicalproceduredata/addmedicalprocedure";


            string jsonpayload = jss.Serialize(medicalprocedure);
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

        // GET: MedicalProcedure/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateMedicalProcedure ViewModel = new UpdateMedicalProcedure();

            //the existing medicalprocedure information
            string url = "medicalproceduredata/findmedicalprocedure/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MedicalProcedureDto SelectedMedicalProcedure = response.Content.ReadAsAsync<MedicalProcedureDto>().Result;
            ViewModel.SelectedMedicalProcedure = SelectedMedicalProcedure;

            // all doctor to choose from when updating this medicalprocedure
            //the existing medicalprocedure information
            url = "doctordata/listdoctor/";
            response = client.GetAsync(url).Result;
            IEnumerable<DoctorDto> DoctorOptions = response.Content.ReadAsAsync<IEnumerable<DoctorDto>>().Result;

            ViewModel.DoctorOptions = DoctorOptions;

            return View(ViewModel);
        }

        // POST: MedicalProcedure/Update/5
        [HttpPost]
        public ActionResult Update(int id, MedicalProcedure medicalprocedure)
        {

            string url = "medicalproceduredata/updatemedicalprocedure/" + id;
            string jsonpayload = jss.Serialize(medicalprocedure);
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

        // GET: MedicalProcedure/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "medicalproceduredata/findmedicalprocedure/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MedicalProcedureDto selectedmedicalprocedure = response.Content.ReadAsAsync<MedicalProcedureDto>().Result;
            return View(selectedmedicalprocedure);
        }

        // POST: MedicalProcedure/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "medicalproceduredata/deletemedicalprocedure/" + id;
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