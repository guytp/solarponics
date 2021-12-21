using System;

namespace Solarponics.Models.WebApi
{
    public class CultureInnoculateRequest
    {
        public int Id { get; set; }
        public int ParentCultureId { get; set; }
        public string AdditionalNotes { get; set; }
        public DateTime Date { get; set; }
    }
}