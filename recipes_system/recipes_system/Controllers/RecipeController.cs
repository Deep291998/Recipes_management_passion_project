// Importing necessary namespaces
using recipes_system.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

// Defining the namespace for the controller
namespace recipe_system.Controllers
{
    // Defining the RecipeController class which inherits from Controller class
    public class RecipeController : Controller
    {
        // GET: Recipe/Index action method
        public ActionResult Index()
        {
            return View(); // Returns the Index view
        }

        // Static readonly HttpClient for making HTTP requests
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        // Static constructor to initialize the HttpClient
        static RecipeController()
        {
            client = new HttpClient(); // Instantiates the HttpClient
            client.BaseAddress = new Uri("https://localhost:44354/api/recipedata/"); // Sets the base address for API requests
        }

        // GET: Recipe/List action method
        public ActionResult List()
        {
            // Retrieves a list of recipes from the API
            string url = "ListRecipes"; // API endpoint for listing recipes
            HttpResponseMessage response = client.GetAsync(url).Result; // Sends a GET request to the API

            IEnumerable<RecipeDto> recipe = response.Content.ReadAsAsync<IEnumerable<RecipeDto>>().Result; // Deserializes the response JSON into a list of RecipeDto objects

            return View(recipe); // Returns the List view with the list of recipes
        }

        // GET: Recipe/Details/5 action method
        public ActionResult Details(int id)
        {
            // Retrieves details of a specific recipe from the API
            string url = "findrecipe/" + id; // API endpoint for finding a recipe by ID
            HttpResponseMessage response = client.GetAsync(url).Result; // Sends a GET request to the API

            RecipeDto selectedrecipe = response.Content.ReadAsAsync<RecipeDto>().Result; // Deserializes the response JSON into a RecipeDto object

            return View(selectedrecipe); // Returns the Details view with the selected recipe
        }

        // GET: Recipe/Error action method
        public ActionResult Error()
        {
            return View(); // Returns the Error view
        }

        // GET: Recipe/New action method
        public ActionResult New()
        {
            return View(); // Returns the New view for creating a new recipe
        }

        // POST: Recipe/Create action method
        [HttpPost]
        public ActionResult Create(Recipe recipe)
        {
            // Adds a new recipe into the system using the API
            string url = "addrecipe"; // API endpoint for adding a recipe

            string jsonpayload = jss.Serialize(recipe); // Serializes the recipe object into JSON

            HttpContent content = new StringContent(jsonpayload); // Creates HTTP content with the JSON payload
            content.Headers.ContentType.MediaType = "application/json"; // Sets the content type to JSON

            HttpResponseMessage response = client.PostAsync(url, content).Result; // Sends a POST request to the API

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List"); // Redirects to the List action if successful
            }
            else
            {
                return RedirectToAction("Error"); // Redirects to the Error action if unsuccessful
            }
        }

        // GET: Recipe/Edit/5 action method
        public ActionResult Edit(int id)
        {
            // Retrieves details of a specific recipe for editing
            string url = "findrecipe/" + id; // API endpoint for finding a recipe by ID
            HttpResponseMessage response = client.GetAsync(url).Result; // Sends a GET request to the API

            RecipeDto selectedrecipe = response.Content.ReadAsAsync<RecipeDto>().Result; // Deserializes the response JSON into a RecipeDto object

            return View(selectedrecipe); // Returns the Edit view with the selected recipe
        }

        // POST: Recipe/Update/5 action method
        [HttpPost]
        public ActionResult Update(int id, Recipe recipe)
        {
            try
            {
                // Updates an existing recipe in the system using the API
                string url = "UpdateRecipe/" + id; // API endpoint for updating a recipe by ID

                string jsonpayload = jss.Serialize(recipe); // Serializes the recipe object into JSON

                HttpContent content = new StringContent(jsonpayload); // Creates HTTP content with the JSON payload
                content.Headers.ContentType.MediaType = "application/json"; // Sets the content type to JSON

                HttpResponseMessage response = client.PostAsync(url, content).Result; // Sends a POST request to the API

                return RedirectToAction("Details/" + id); // Redirects to the Details action after updating
            }
            catch
            {
                return View();
            }
        }

        // GET: Recipe/Delete/5 action method
        public ActionResult Delete(int id)
        {
            return View(); // Returns the Delete view for confirming deletion
        }

        // POST: Recipe/Delete/5 action method
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index"); // Redirects to the Index action
            }
            catch
            {
                return View();
            }
        }
    }
}
