using Solarponics.Models;
using System;

namespace Solarponics.ProductionManager.LabelDefinitions
{
    public class CultureLabelDefinition : LabelDefinition
    {
        public CultureLabelDefinition(Culture culture, Supplier supplier)
            : this(culture, null, supplier, null)
        {
        }
        public CultureLabelDefinition(Culture culture, Culture parentCulture, Recipe recipe)
            : this(culture, parentCulture, null, recipe)
        {
        }

        private CultureLabelDefinition(Culture culture, Culture parentCulture, Supplier supplier, Recipe recipe)
        {
            var label = string.Empty;
            if (culture.Strain != null)
            {
                label += $"Strain:   {culture.Strain}{Environment.NewLine}";
                string mediumTypePrefix;
                switch (culture.MediumType)
                {
                    case CultureMediumType.Agar:
                        mediumTypePrefix = "A";
                        break;
                    case CultureMediumType.Liquid:
                        mediumTypePrefix = "L";
                        break;
                    default:
                        throw new NotImplementedException();
                }
                string parentMediumTypePrefix = null;
                if (parentCulture != null)
                {
                    switch (parentCulture.MediumType)
                    {
                        case CultureMediumType.Agar:
                            parentMediumTypePrefix = "A";
                            break;
                        case CultureMediumType.Liquid:
                            parentMediumTypePrefix = "L";
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                if (parentMediumTypePrefix == null)
                    label += $"Gen:      {mediumTypePrefix} {culture.Generation}{Environment.NewLine}";
                else
                    label += $"Gen:      {parentMediumTypePrefix} -> {mediumTypePrefix} {culture.Generation}{Environment.NewLine}";
            }
            else
                label += $"Strain:   NOT INNOCULATED{Environment.NewLine}";

            if (supplier != null)
                label += $"Supplier: {supplier.Name}{Environment.NewLine}";

            if (culture.InnoculateDate.HasValue)
                label += $"Innoc:    {culture.InnoculateDate.Value.DayOfYear} by {culture.InnoculateUser}{Environment.NewLine}";

            if (recipe != null)
                label += $"Recipe:   {recipe.Name}{Environment.NewLine}";
            string createBookInPrefix = supplier == null ? "Cooked:   " : "Bk In:    ";
            label += $"{createBookInPrefix}{culture.CreateDate.DayOfYear} by {culture.CreateUser}{Environment.NewLine}";
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