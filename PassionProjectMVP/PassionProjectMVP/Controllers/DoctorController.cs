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
            client.BaseAddress = new Uri("https://localhost:44326/api/");
        }

        // GET: Doctor/List
        public ActionResult List()
        {
            //objective: communicate with our doctor data api to retrieve a list of doctors
            //curl https://localhost:44326/api/doctordata/listdoctors


            string url = "doctordata/listdoctors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<DoctorDto> doctors = response.Content.ReadAsAsync<IEnumerable<DoctorDto>>().Result;
            //Debug.WriteLine("Number of doctors received : ");
            //Debug.WriteLine(doctors.Count());


            return View(doctors);
        }

        // GET: Doctor/Details/5
        public ActionResult Details(int id)
        {
            DetailsDoctor ViewModel = new DetailsDoctor();

            //objective: communicate with our doctor data api to retrieve one doctor
            //curl https://localhost:44326/api/doctordata/finddoctor/{id}

            string url = "doctordata/finddoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            DoctorDto SelectedDoctor = response.Content.ReadAsAsync<DoctorDto>().Result;
            Debug.WriteLine("doctor received : ");
            Debug.WriteLine(SelectedDoctor.DoctorFirstName);

            ViewModel.SelectedDoctor = SelectedDoctor;

            response = client.GetAsync(url).Result;



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
        public ActionResult Create(Doctor doctor)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(doctor.DoctorFirstName);
            //objective: add a new doctor into our system using the API
            //curl -H "Content-Type:application/json" -d @doctor.json https://localhost:44326/api/doctordata/adddoctor 
            string url = "doctordata/adddoctor";


            string jsonpayload = jss.Serialize(doctor);
            //Debug.WriteLine(jsonpayload);

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
            UpdateDoctor ViewModel = new UpdateDoctor();

            //the existing doctor information
            string url = "doctordata/finddoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DoctorDto SelectedDoctor = response.Content.ReadAsAsync<DoctorDto>().Result;
            ViewModel.SelectedDoctor = SelectedDoctor;

            // all species to choose from when updating this doctor
            //the existing doctor information

            response = client.GetAsync(url).Result;


            return View(ViewModel);
        }

        // POST: Doctor/Update/5
        [HttpPost]
        public ActionResult Update(int id, Doctor doctor)
        {

            string url = "doctordata/updatedoctor/" + id;
            string jsonpayload = jss.Serialize(doctor);
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
            DoctorDto selecteddoctor = response.Content.ReadAsAsync<DoctorDto>().Result;
            return View(selecteddoctor);
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