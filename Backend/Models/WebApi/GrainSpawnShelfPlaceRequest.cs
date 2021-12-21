using System;

namespace Solarponics.Models.WebApi
{
    public class GrainSpawnShelfPlaceRequest
    {
        public int ShelfId { get; set; }
        public string AdditionalNotes { get; set; }
        public DateTime Date { get; set; }
    }
}