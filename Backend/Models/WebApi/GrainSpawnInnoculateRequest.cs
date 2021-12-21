using System;

namespace Solarponics.Models.WebApi
{
    public class GrainSpawnInnoculateRequest
    {
        public int CultureId { get; set; }
        public string AdditionalNotes { get; set; }
        public DateTime Date { get; set; }
    }
}