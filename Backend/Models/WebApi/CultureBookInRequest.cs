using System;

namespace Solarponics.Models.WebApi
{
    public class CultureBookInRequest
    {
        public int SupplierId { get; set; }
        public CultureMediumType MediumType { get; set; }
        public DateTime OrderDate { get; set; }
        public string Strain { get; set; }
        public string Notes { get; set; }
    }
}
