namespace Solarponics.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public RecipeType Type { get; set;}
        public int UnitsCreated { get; set; }
    }
}