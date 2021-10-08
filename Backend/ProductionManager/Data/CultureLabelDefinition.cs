using Solarponics.Models;
using System;

namespace Solarponics.ProductionManager.Data
{
    public class CultureLabelDefinition : LabelDefinition
    {
        public CultureLabelDefinition(Culture culture, Supplier supplier)
            : this(culture, supplier, null)
        {
        }
        public CultureLabelDefinition(Culture culture, Recipe recipe)
            : this(culture, null, recipe)
        {
        }

        private CultureLabelDefinition(Culture culture, Supplier supplier, Recipe recipe)
        {
            var label = string.Empty;
            if (supplier != null)
            {
                label += $"Supplier: {supplier.Name}";
            }

            if (recipe != null)
            {
                label += $"Recipe:   {recipe.Name}";
            }
            
            label += $"{Environment.NewLine}Strain:   {culture.Strain}{Environment.NewLine}Date:     {culture.CreateDate.ToShortDateString()}";
            if (!string.IsNullOrEmpty(culture.Notes))
            {
                label += $"{Environment.NewLine}{Environment.NewLine}{culture.Notes}";
            }
            Text = label;
            Barcode = $"C{culture.Id}";
            BarcodeSize = BarcodeSize.Small;
            TextSize = TextSize.Small;
            MaxTextWidth = 30;
        }
    }
}