using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using recipes_system.Models;

namespace Instructions_system.Controllers
{
    public class InstructionDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext(); // DbContext for interacting with the database

        // GET: api/InstructionData/ListInstructions
        [HttpGet]
        [Route("api/InstructionData/ListInstructions")]
        public IEnumerable<InstructionDto> ListInstructions()
        {
            // Retrieves a list of instructions from the database
            List<Instruction> Instructions = db.Instructions.ToList();
            List<InstructionDto> InstructionDtos = new List<InstructionDto>();

            // Converts each Instruction object to InstructionDto object for API response
            Instructions.ForEach(a => InstructionDtos.Add(new InstructionDto()
            {
                InstructionId = a.InstructionId,
                Description = a.Description,
                RecipeId = a.RecipeId,
                InstructionName = a.InstructionName,
            }));

            return InstructionDtos; // Returns the list of InstructionDto objects
        }

        // GET: api/InstructionData/FindInstruction/5
        [ResponseType(typeof(Instruction))]
        [HttpGet]
        [Route("api/InstructionData/FindInstruction/{id}")]
        public IHttpActionResult FindInstruction(int id)
        {
            // Finds a specific instruction by its ID
            Instruction Instruction = db.Instructions.Find(id);
            InstructionDto InstructionDto = new InstructionDto()
            {

                InstructionId = Instruction.InstructionId,
                Description = Instruction.Description,
                RecipeId = Instruction.RecipeId,
                InstructionName = Instruction.InstructionName,
            };

            if (Instruction == null)
            {
                return NotFound(); // Returns 404 Not Found if the instruction is not found
            }

            return Ok(InstructionDto); // Returns the InstructionDto object
        }

        // POST: api/InstructionData/UpdateInstruction/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/InstructionData/UpdateInstruction/{id}")]
        public IHttpActionResult UpdateInstruction(int id, Instruction Instruction)
        {
            Debug.WriteLine("calling edit api!");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns 400 Bad Request if the model state is not valid
            }

            if (id != Instruction.InstructionId)
            {
                return BadRequest(); // Returns 400 Bad Request if the ID in the URL doesn't match the ID in the model
            }

            db.Entry(Instruction).State = EntityState.Modified; // Marks the instruction entity as modified

            try
            {
                db.SaveChanges(); // Saves changes to the database
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstructionExists(id))
                {
                    return NotFound(); // Returns 404 Not Found if the instruction is not found
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent); // Returns 204 No Content if the update is successful
        }

        // POST: api/InstructionData/AddInstruction
        [ResponseType(typeof(Instruction))]
        [HttpPost]
        [Route("api/InstructionData/AddInstruction")]
        public IHttpActionResult AddInstruction(Instruction Instruction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns 400 Bad Request if the model state is not valid
            }

            db.Instructions.Add(Instruction); // Adds the instruction to the database
            db.SaveChanges(); // Saves changes to the database

            return Ok(); // Returns 200 OK if the addition is successful
        }

        // POST: api/InstructionData/DeleteInstruction/5
        [ResponseType(typeof(Instruction))]
        [HttpPost]
        [Route("api/InstructionData/DeleteInstruction/{id}")]
        public IHttpActionResult DeleteInstruction(int id)
        {
            Instruction Instruction = db.Instructions.Find(id); // Finds the instruction by its ID
            if (Instruction == null)
            {
                return NotFound(); // Returns 404 Not Found if the instruction is not found
            }

            db.Instructions.Remove(Instruction); // Removes the instruction from the database
            db.SaveChanges(); // Saves changes to the database

            return Ok(); // Returns 200 OK if the deletion is successful
        }

        /// <summary>
        /// Gathers information about animals related to a particular keeper
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all animals in the database, including their associated species that match to a particular keeper id
        /// </returns>
        /// <param name="id">Instruction ID.</param>
        /// <example>
        /// GET: api/AnimalData/ListInstructionsForRecipe/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(InstructionDto))]
        [Route("api/InstructionData/ListInstructionsForRecipe/{id}")]
        public IHttpActionResult ListInstructionsForRecipe(int id)
        {

            //all animals that have keepers which match with our ID
           
            List<Instruction> instructions = new List<Instruction>();
            instructions = db.Instructions.Where(a=>a.RecipeId== id).ToList();
            List<InstructionDto> InstructionDtos = new List<InstructionDto>();

            instructions.ForEach(a => InstructionDtos.Add(new InstructionDto()
            {
               RecipeId = a.RecipeId,
               Description = a.Description, 
               InstructionName = a.InstructionName, 
               InstructionId = a.InstructionId
            }));

            return Ok(InstructionDtos);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose(); // Disposes the DbContext
            }
            base.Dispose(disposing);
        }

        private bool InstructionExists(int id)
        {
            return db.Instructions.Count(e => e.InstructionId == id) > 0; // Checks if an instruction with the given ID exists in the database
        }
    }
}
