using System;

namespace Solarponics.Models.WebApi
{
    public class FruitingBlockShelfPlaceRequest
    {
        public int ShelfId { get; set; }
        public string AdditionalNotes { get; set; }
        public DateTime Date { get; set; }
    }
}