using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using PassionProjectMVP.Models;
using PassionProjectMVP.Models.ViewModels;
using System.Web.Script.Serialization;


namespace PassionProjectMVP.Controllers
{
    public class DoctorController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DoctorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44324/api/");
        }

        // GET: Doctor/List
        public ActionResult List()
        {
            //objective: communicate with our Doctor data api to retrieve a list of Doctors
            //curl https://localhost:44324/api/Doctordata/listdoctors


            string url = "doctordata/listdoctors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<DoctorDto> Doctors = response.Content.ReadAsAsync<IEnumerable<DoctorDto>>().Result;
            //Debug.WriteLine("Number of Doctors received : ");
            //Debug.WriteLine(Doctors.Count());


            return View(Doctors);
        }

        // GET: Doctor/Details/5
        public ActionResult Details(int id)
        {
            DetailsDoctor ViewModel = new DetailsDoctor();

            //objective: communicate with our Doctor data api to retrieve one Doctor
            //curl https://localhost:44324/api/Doctordata/finddoctor/{id}

            string url = "doctordata/findDoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            DoctorDto SelectedDoctor = response.Content.ReadAsAsync<DoctorDto>().Result;
            Debug.WriteLine("Doctor received : ");
            Debug.WriteLine(SelectedDoctor.DoctorFirstName);

            ViewModel.SelectedDoctor = SelectedDoctor;

            //show all medicalprocedures under the care of this doctor
            url = "medicalproceduredata/listmedicalproceduresfordoctor/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<MedicalProcedureDto> RelatedMedicalProcedures = response.Content.ReadAsAsync<IEnumerable<MedicalProcedureDto>>().Result;

            ViewModel.RelatedMedicalProcedures = RelatedMedicalProcedures;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Doctor/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Doctor/Create
        [HttpPost]
        public ActionResult Create(Doctor Doctor)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Doctor.DoctorName);
            //objective: add a new Doctor into our system using the API
            //curl -H "Content-Type:application/json" -d @Doctor.json https://localhost:44324/api/Doctordata/addDoctor 
            string url = "doctordata/adddoctor";


            string jsonpayload = jss.Serialize(Doctor);
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

        // GET: Doctor/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "doctordata/finddoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DoctorDto selectedDoctor = response.Content.ReadAsAsync<DoctorDto>().Result;
            return View(selectedDoctor);
        }

        // POST: Doctor/Update/5
        [HttpPost]
        public ActionResult Update(int id, Doctor Doctor)
        {

            string url = "doctordata/updatedoctor/" + id;
            string jsonpayload = jss.Serialize(Doctor);
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

        // GET: Doctor/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "doctordata/finddoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DoctorDto selectedDoctor = response.Content.ReadAsAsync<DoctorDto>().Result;
            return View(selectedDoctor);
        }

        // POST: Doctor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "doctordata/deletedoctor/" + id;
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