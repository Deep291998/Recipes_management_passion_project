using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace recipes_system.Models.ViewModel
{
    public class DetailsRecipe
    {
        public RecipeDto SelectedRecipe { get; set; }
        public IEnumerable<InstructionDto> RecipeInstructions { get; set; }
    }
}