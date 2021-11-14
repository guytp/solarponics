using Solarponics.Models;
using System;

namespace Solarponics.ProductionManager.Data
{
    public class CultureDataGridRowEntry
    {
        public CultureDataGridRowEntry(Culture culture, Culture parentCulture, Supplier supplier, Recipe recipe)
        {
            OriginalCulture = culture;
            Id = culture.Id;
            Supplier = supplier;
            ParentCulture = parentCulture;
            Recipe = recipe;
            MediumType = culture.MediumType;
            OrderDate = culture.OrderDate?.ToShortDateString();
            CreateDate = culture.CreateDate.ToShortDateString();
            Strain = culture.Strain;
            Notes = culture.Notes;
            CreateUser = culture.CreateUser;
            InnoculateUser = culture.InnoculateUser;
            InnoculateDate = culture.InnoculateDate?.ToShortDateString();
            ParentCultureId = parentCulture?.Id.ToString();
            RecipeName = recipe?.Name;
            SupplierName = supplier?.Name;
            Type = culture.InnoculateDate.HasValue ? "Innoculated" : Supplier != null ? "Booked In" : ParentCulture == null ? "Prepared" : "Unknown";
            Generation = MediumType.ToString().Substring(0, 1) + culture.Generation;
            if (parentCulture != null)
                Generation = parentCulture.MediumType.ToString().Substring(0, 1) + " -> " + Generation;
        }

        public Culture OriginalCulture { get; }
        public int Id { get; }
        public string Type { get; }
        public Supplier Supplier { get; }
        public string SupplierName { get; }
        public Culture ParentCulture { get; }
        public string ParentCultureId { get; }
        public Recipe Recipe { get; }
        public string RecipeName { get; }
        public CultureMediumType MediumType { get; }
        public string OrderDate { get; }
        public string CreateDate { get; }
        public string Strain { get; }
        public string Notes { get; }
        public string Generation { get; }
        public string CreateUser { get; }
        public string InnoculateUser { get; }
        public string InnoculateDate { get; }
    }
}