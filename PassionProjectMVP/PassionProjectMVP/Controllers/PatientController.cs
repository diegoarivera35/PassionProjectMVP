using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using PassionProjectMVP.Models;
using System.Web.Script.Serialization;


namespace PassionProjectMVP.Controllers
{
    public class PatientController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static PatientController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44326/api/patientdata/");
        }
        // GET: Patient/list
        public ActionResult List()
        {
            //objective: communicate with our patient data api to retrieve a list of patients
            //curl https://localhost:44326/api/patientdata/listpatients


            string url = "listpatients";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The responce code is: ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<PatientDto> patients = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;
            //Debug.WriteLine("Number of Patients received: ");
            //Debug.WriteLine(patients.Count());

            return View(patients);
        }

        // GET: Patient/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our patient data api to retrieve a list of patients
            //curl https://localhost:44326/api/patientdata/findpatient/{id}


            string url = "findpatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The responce code is: ");
            //Debug.WriteLine(response.StatusCode);

            PatientDto selectedpatient = response.Content.ReadAsAsync<PatientDto>().Result;
            //Debug.WriteLine("Number of Patients received: ");
            //Debug.WriteLine(selectedpatient.PatientFirstName);


            return View(selectedpatient);
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
        public ActionResult Create(Patient patient)
        {
           
                Debug.WriteLine("the json payload is:");
                Debug.WriteLine(patient.PatientFirstName);
                //objective: add a new patient into our system using the API
                //curl -H "Content-Type:application/json" -d @patient.json https://localhost:44326/api/patientdata/addpatient
                string url = "addpatient";

               
                string jsonpayload = jss.Serialize(patient);

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



        // POST: Patient/Edit/5
        [HttpPost]
        public ActionResult Edit(int id)
        {


            return View();
        }

        // GET: Patient/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Patient/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}