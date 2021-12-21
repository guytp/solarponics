using System;

namespace Solarponics.Models.WebApi
{
    public class FruitingBlockInnoculateRequest
    {
        public int GrainSpawnId { get; set; }
        public string AdditionalNotes { get; set; }
        public DateTime Date { get; set; }
    }
}