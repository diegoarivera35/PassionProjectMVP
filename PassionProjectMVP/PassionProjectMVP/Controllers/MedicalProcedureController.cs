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
    public class MedicalProcedureController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static MedicalProcedureController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44326/api/");
        }

        // GET: MedicalProcedure/List
        public ActionResult List()
        {
            //objective: communicate with our medicalprocedure data api to retrieve a list of medicalprocedures
            //curl https://localhost:44326/api/medicalproceduredata/listmedicalprocedures


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
            //curl https://localhost:44326/api/medicalproceduredata/findmedicalprocedure/{id}

            string url = "medicalproceduredata/findmedicalprocedure/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            MedicalProcedureDto SelectedMedicalProcedure = response.Content.ReadAsAsync<MedicalProcedureDto>().Result;
            Debug.WriteLine("medicalprocedure received : ");
            Debug.WriteLine(SelectedMedicalProcedure.MedicalProcedureName);

            ViewModel.SelectedMedicalProcedure = SelectedMedicalProcedure;
            response = client.GetAsync(url).Result;



            return View(ViewModel);
        }






        public ActionResult Error()
        {

            return View();
        }

        // GET: MedicalProcedure/New
        public ActionResult New()
        {
            return View();
        }


        // POST: MedicalProcedure/Create
        [HttpPost]
        public ActionResult Create(MedicalProcedure medicalprocedure)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(medicalprocedure.MedicalProcedureName);
            //objective: add a new medicalprocedure into our system using the API
            //curl -H "Content-Type:application/json" -d @medicalprocedure.json https://localhost:44326/api/medicalproceduredata/addmedicalprocedure 
            string url = "medicalproceduredata/addmedicalprocedure";


            string jsonpayload = jss.Serialize(medicalprocedure);
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

        // GET: MedicalProcedure/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateMedicalProcedure ViewModel = new UpdateMedicalProcedure();

            //the existing medicalprocedure information
            string url = "medicalproceduredata/findmedicalprocedure/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MedicalProcedureDto SelectedMedicalProcedure = response.Content.ReadAsAsync<MedicalProcedureDto>().Result;
            ViewModel.SelectedMedicalProcedure = SelectedMedicalProcedure;

            //the existing medicalprocedure information
            response = client.GetAsync(url).Result;


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
            Debug.WriteLine(medicalprocedure.MedicalProcedureID);
            Debug.WriteLine(medicalprocedure.MedicalProcedureName);
            Debug.WriteLine(medicalprocedure.MedicalProcedureDate);
            Debug.WriteLine(medicalprocedure.MedicalCenter);
            Debug.WriteLine(medicalprocedure.PatientID);
            Debug.WriteLine(medicalprocedure.DoctorID);
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