using System;

namespace Solarponics.Models
{
    public class Culture
    {
        public int Id { get; set; }
        public int? SupplierId { get; set; }
        public int? ParentCultureId { get; set; }
        public int? RecipeId { get; set; }
        public CultureMediumType MediumType { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int UserId { get; set; }
        public string Strain { get; set; }
        public string Notes { get; set; }

        public int? Generation { get; set; }
    }
}