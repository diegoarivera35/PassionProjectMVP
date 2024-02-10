﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using WebAppPassionProjectMVP.Models;

namespace WebAppPassionProjectMVP.Controllers
{
    public class PatientController : Controller
    {
        private static readonly HttpClient client;
        static PatientController()
        {
            client = new HttpClient();
        }
        // GET: Patient/list
        public ActionResult List()
        {
            //objective: communicate with our patient data api to retrieve a list of patients
            //curl https://localhost:44308/api/patientdata/listpatients

            
            string url = "https://localhost:44308/api/patientdata/listpatients";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The responce code is: ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<PatientDto> patients = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;
            Debug.WriteLine("Number of Patients received: ");
            Debug.WriteLine(patients.Count());

            return View(patients);
        }

        // GET: Patient/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our patient data api to retrieve a list of patients
            //curl https://localhost:44308/api/patientdata/findpatient/{id}

            
            string url = "https://localhost:44308/api/patientdata/findpatient/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The responce code is: ");
            Debug.WriteLine(response.StatusCode);

            PatientDto selectedpatient = response.Content.ReadAsAsync<PatientDto>().Result;
            Debug.WriteLine("Number of Patients received: ");
            Debug.WriteLine(selectedpatient.PatientFirstName);


            return View(selectedpatient);
        }

        // GET: Patient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Patient/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Patient/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
