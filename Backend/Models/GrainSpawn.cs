using System;

namespace Solarponics.Models
{
    public class GrainSpawn
    {
        public int Id { get; set; }
        public int? CultureId { get; set; }
        public int RecipeId { get; set; }
        public int? ShelfId { get; set; }
        public int CreateUserId { get; set; }
        public int? InnoculateUserId { get; set; }
        public int? ShelfPlaceUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? InnoculateDate { get; set; }
        public DateTime? ShelfPlaceDate { get; set; }
        public decimal Weight { get; set; }
        public string Notes { get; set; }
        public string CreateUser { get; set; }
        public string InnoculateUser { get; set; }
        public string ShelfPlaceUser { get; set; }
        public string Strain { get; set; }
        public CultureMediumType? MediumType { get; set; }
        public string RecipeName { get; set; }
        public string ShelfName { get; set; }
    }
}