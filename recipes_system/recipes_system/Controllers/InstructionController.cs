using recipes_system.Models; // Importing the necessary model(s)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Instructions_system.Controllers
{
    public class InstructionController : Controller
    {
        // GET: Instruction
        public ActionResult Index()
        {
            return View(); // Returns the Index view
        }

        private static readonly HttpClient client; // HTTP client for making API requests
        private JavaScriptSerializer jss = new JavaScriptSerializer(); // JSON serializer

        static InstructionController()
        {
            client = new HttpClient(); // Instantiating the HTTP client
            client.BaseAddress = new Uri("https://localhost:44354/api/Instructiondata/"); // Setting the base URI for API requests
        }

        // GET: Instruction/List
        public ActionResult List()
        {
            // Retrieves a list of instructions from the API
            string url = "ListInstructions"; // API endpoint
            HttpResponseMessage response = client.GetAsync(url).Result; // Sends a GET request to the API

            // Deserialize the response JSON into a list of InstructionDto objects
            IEnumerable<InstructionDto> Instruction = response.Content.ReadAsAsync<IEnumerable<InstructionDto>>().Result;

            return View(Instruction); // Returns the List view with the list of instructions
        }

        // GET: Instruction/Details/5
        public ActionResult Details(int id)
        {
            // Retrieves details of a specific instruction from the API
            string url = "findInstruction/" + id; // API endpoint for finding an instruction by ID
            HttpResponseMessage response = client.GetAsync(url).Result; // Sends a GET request to the API

            InstructionDto selectedInstruction = response.Content.ReadAsAsync<InstructionDto>().Result; // Deserialize the response JSON into an InstructionDto object

            return View(selectedInstruction); // Returns the Details view with the selected instruction
        }

        public ActionResult Error()
        {
            return View(); // Returns the Error view
        }

        // GET: Instruction/New
        public ActionResult New()
        {
            return View(); // Returns the New view for creating a new instruction
        }

        // POST: Instruction/Create
        [HttpPost]
        public ActionResult Create(Instruction Instruction)
        {
            // Adds a new instruction into the system using the API
            string url = "addInstruction"; // API endpoint for adding an instruction

            string jsonpayload = jss.Serialize(Instruction); // Serializes the instruction object into JSON

            HttpContent content = new StringContent(jsonpayload); // Creates HTTP content with the JSON payload
            content.Headers.ContentType.MediaType = "application/json"; // Sets the content type to JSON

            // Sends a POST request to the API to add the instruction
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List"); // Redirects to the List action if successful
            }
            else
            {
                return RedirectToAction("Error"); // Redirects to the Error action if unsuccessful
            }
        }

        // GET: Instruction/Edit/5
        public ActionResult Edit(int id)
        {
            // Retrieves details of a specific instruction for editing
            string url = "findInstruction/" + id; // API endpoint for finding an instruction by ID
            HttpResponseMessage response = client.GetAsync(url).Result; // Sends a GET request to the API

            InstructionDto selectedInstruction = response.Content.ReadAsAsync<InstructionDto>().Result; // Deserialize the response JSON into an InstructionDto object

            return View(selectedInstruction); // Returns the Edit view with the selected instruction
        }

        // POST: Instruction/Update/5
        [HttpPost]
        public ActionResult Update(int id, Instruction Instruction)
        {
            try
            {
                // Updates an existing instruction in the system using the API
                string url = "UpdateInstruction/" + id; // API endpoint for updating an instruction by ID

                string jsonpayload = jss.Serialize(Instruction); // Serializes the instruction object into JSON

                HttpContent content = new StringContent(jsonpayload); // Creates HTTP content with the JSON payload
                content.Headers.ContentType.MediaType = "application/json"; // Sets the content type to JSON

                // Sends a POST request to the API to update the instruction
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                return RedirectToAction("Details/" + id); // Redirects to the Details action after updating
            }
            catch
            {
                return View();
            }
        }

        // GET: Instruction/Delete/5
        public ActionResult Delete(int id)
        {
            return View(); // Returns the Delete view for confirming deletion
        }

        // POST: Instruction/Delete/5
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
