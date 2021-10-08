namespace Solarponics.Models.WebApi
{
    public class CultureCreateFromReciptRequest
    {
        public int RecipeId { get; set; }
        public CultureMediumType MediumType { get; set; }
        public string Notes { get; set; }
    }
}