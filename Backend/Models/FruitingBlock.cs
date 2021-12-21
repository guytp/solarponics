using System;

namespace Solarponics.Models
{
    public class FruitingBlock
    {
        public int Id { get; set; }
        public int? GrainSpawnId { get; set; }
        public int RecipeId { get; set; }
        public int? IncubateShelfId { get; set; }
        public int? FruitingShelfId { get; set; }
        public int CreateUserId { get; set; }
        public int? InnoculateUserId { get; set; }
        public int? IncubateShelfPlaceUserId { get; set; }
        public int? FruitingShelfPlaceUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? InnoculateDate { get; set; }
        public DateTime? IncubateShelfPlaceDate { get; set; }
        public DateTime? FruitingShelfPlaceDate { get; set; }
        public decimal Weight { get; set; }
        public string Notes { get; set; }
        public string CreateUser { get; set; }
        public string InnoculateUser { get; set; }
        public string IncubateShelfPlaceUser { get; set; }
        public string FruitingShelfPlaceUser { get; set; }
        public string Strain { get; set; }
        public CultureMediumType? MediumType { get; set; }
        public string RecipeName { get; set; }
        public string IncubateShelfName { get; set; }
        public string FruitingShelfName { get; set; }
    }
}