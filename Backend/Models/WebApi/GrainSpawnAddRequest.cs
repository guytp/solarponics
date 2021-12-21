using System;

namespace Solarponics.Models.WebApi
{
    public class GrainSpawnAddRequest
    {
        public int RecipeId { get; set; }
        public decimal Weight { get; set; }
        public string Notes { get; set; }

        public DateTime Date { get; set; }
    }
}