// Importing necessary namespaces
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using recipes_system.Models;
using System.Diagnostics;

// Defining the namespace for the controller
namespace Recipes_system.Controllers
{
    // Defining the RecipeDataController class which inherits from ApiController class
    public class RecipeDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext(); // DbContext for interacting with the database

        // GET: api/RecipeData/ListRecipes action method
        [HttpGet]
        [Route("api/RecipeData/ListRecipes")]
        public IEnumerable<RecipeDto> ListRecipes()
        {
            // Retrieves a list of recipes from the database
            List<Recipe> Recipes = db.Recipes.ToList();
            List<RecipeDto> RecipeDtos = new List<RecipeDto>();

            // Converts each Recipe object to RecipeDto object for API response
            Recipes.ForEach(a => RecipeDtos.Add(new RecipeDto()
            {
                RecipeId = a.RecipeId,
                Title = a.Title,
                Categories = a.Categories,
                Ingridents = a.Ingridents,
            }));

            return RecipeDtos; // Returns the list of RecipeDto objects
        }

        // GET: api/RecipeData/FindRecipe/5 action method
        [ResponseType(typeof(Recipe))]
        [HttpGet]
        [Route("api/RecipeData/FindRecipe/{id}")]
        public IHttpActionResult FindRecipe(int id)
        {
            // Finds a specific recipe by its ID
            Recipe Recipe = db.Recipes.Find(id);
            RecipeDto RecipeDto = new RecipeDto()
            {
                RecipeId = Recipe.RecipeId,
                Title = Recipe.Title,
                Categories = Recipe.Categories,
                Ingridents = Recipe.Ingridents,
            };
            if (Recipe == null)
            {
                return NotFound(); // Returns 404 Not Found if the recipe is not found
            }

            return Ok(RecipeDto); // Returns the RecipeDto object
        }

        // POST: api/RecipeData/UpdateRecipe/5 action method
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/RecipeData/UpdateRecipe/{id}")]
        public IHttpActionResult UpdateRecipe(int id, Recipe Recipe)
        {
            Debug.WriteLine("calling edit api!");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns 400 Bad Request if the model state is not valid
            }

            if (id != Recipe.RecipeId)
            {
                return BadRequest(); // Returns 400 Bad Request if the ID in the URL doesn't match the ID in the model
            }

            db.Entry(Recipe).State = EntityState.Modified; // Marks the recipe entity as modified

            try
            {
                db.SaveChanges(); // Saves changes to the database
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound(); // Returns 404 Not Found if the recipe is not found
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent); // Returns 204 No Content if the update is successful
        }

        // POST: api/RecipeData/AddRecipe action method
        [ResponseType(typeof(Recipe))]
        [HttpPost]
        [Route("api/RecipeData/AddRecipe")]
        public IHttpActionResult AddRecipe(Recipe Recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns 400 Bad Request if the model state is not valid
            }

            db.Recipes.Add(Recipe); // Adds the recipe to the database
            db.SaveChanges(); // Saves changes to the database

            return Ok(); // Returns 200 OK if the addition is successful
        }

        // POST: api/RecipeData/DeleteRecipe/5 action method
        [ResponseType(typeof(Recipe))]
        [HttpPost]
        [Route("api/RecipeData/DeleteRecipe/{id}")]
        public IHttpActionResult DeleteRecipe(int id)
        {
            Recipe Recipe = db.Recipes.Find(id); // Finds the recipe by its ID
            if (Recipe == null)
            {
                return NotFound(); // Returns 404 Not Found if the recipe is not found
            }

            db.Recipes.Remove(Recipe); // Removes the recipe from the database
            db.SaveChanges(); // Saves changes to the database

            return Ok(); // Returns 200 OK if the deletion is successful
        }

        // Disposes the DbContext when the controller is disposed
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose(); // Disposes the DbContext
            }
            base.Dispose(disposing);
        }

        // Checks if a recipe with the given ID exists in the database
        private bool RecipeExists(int id)
        {
            return db.Recipes.Count(e => e.RecipeId == id) > 0;
        }
    }
}
