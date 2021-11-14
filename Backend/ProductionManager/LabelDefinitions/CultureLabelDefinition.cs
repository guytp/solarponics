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
                label += $"Strn: {ShortenIfNeeded(culture.Strain)}{Environment.NewLine}";
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
                    label += $"Gen:  {mediumTypePrefix} {culture.Generation}{Environment.NewLine}";
                else
                    label += $"Gen:  {parentMediumTypePrefix}->{mediumTypePrefix}{culture.Generation}{Environment.NewLine}";
            }
            else
                label += $"Strn: NONE/{culture.MediumType.ToString()[0]}{Environment.NewLine}";

            if (supplier != null)
                label += $"Sup:  {ShortenIfNeeded(supplier.Name)}{Environment.NewLine}";

            if (culture.InnoculateDate.HasValue)
                label += $"Inoc: {culture.InnoculateDate.Value.DayOfYear}/{GetInitials(culture.InnoculateUser)}{Environment.NewLine}";

            if (recipe != null)
                label += $"Rec:  {ShortenIfNeeded(recipe.Name)}{Environment.NewLine}";
            string createBookInPrefix = supplier == null ? "Ckd:  " : "BkIn: ";
            label += $"{createBookInPrefix}{culture.CreateDate.DayOfYear}/{GetInitials(culture.CreateUser)}";
            Text = label;
            Barcode = $"C{culture.Id}";
            BarcodeSize = BarcodeSize.Small;
            TextSize = TextSize.Medium;
            MaxTextWidth = 12;
            MaxLinesToPrint = 5;
        }

        private string GetInitials(string name)
        {
            var parts = name.Split(new char[] { ' ' });
            var initials = string.Empty;
            foreach (var part in parts)
                initials += part[0];
            return initials;
        }

        private string ShortenIfNeeded(string val)
        {
            if (val.Length > 6)
                return GetInitials(val);
            return val;
        }
    }
}