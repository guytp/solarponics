using Solarponics.Models;
using System;

namespace Solarponics.ProductionManager.LabelDefinitions
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
                label += $"Supplier: {supplier.Name}{Environment.NewLine}";

            if (recipe != null)
                label += $"Recipe:   {recipe.Name}{Environment.NewLine}";

            if (culture.Strain != null)
                label += $"Strain:   {culture.Strain} (G {culture.Generation}){Environment.NewLine}";
            else
                label += $"Strain:   NOT INNOCULATED{Environment.NewLine}";

            label += $"Date:     {culture.CreateDate.ToShortDateString()}";
            if (!string.IsNullOrEmpty(culture.Notes))
            {
                label += $"{Environment.NewLine}{Environment.NewLine}{culture.Notes}";
            }
            Text = label;
            Barcode = $"C{culture.Id}";
            BarcodeSize = BarcodeSize.Small;
            TextSize = TextSize.Small;
            MaxTextWidth = 30;
            MaxLinesToPrint = 8;
        }
    }
}