using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace recipes_system.Models
{
    public class Recipe
    {
        [Key]
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string Ingridents { get; set; }
        public string Categories { get; set; }

        public ICollection<Instruction> Instructions { get; set; }
    }
    public class RecipeDto
    {
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string Ingridents { get; set; }
        public string Categories { get; set; }
    }
}