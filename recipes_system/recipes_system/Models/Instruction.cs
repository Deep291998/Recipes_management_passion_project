using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace recipes_system.Models
{
    public class Instruction
    {

        [Key]
        public int InstructionId { get; set; }
        public string InstructionName { get; set; }

        public string Description { get; set; }

        [ForeignKey("Recipes")]
        public int RecipeId { get; set; }

        public virtual Recipe Recipes { get; set; }
    }
    public class InstructionDto
    {
        public int InstructionId { get; set; }
        public string InstructionName { get; set; }

        public string Description { get; set; }
        public int RecipeId { get; set; }
    }
}