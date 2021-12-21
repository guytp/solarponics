using Solarponics.Models;
using System;

namespace Solarponics.ProductionManager.LabelDefinitions
{
    public class FruitingBlockLabelDefinition : LabelDefinition
    {
        public FruitingBlockLabelDefinition(FruitingBlock fruitingBlock)
        {
            var label = string.Empty;

            if (fruitingBlock.Strain != null)
                label += $"Strn: {ShortenIfNeeded(fruitingBlock.Strain, -3)} / {fruitingBlock.MediumType.Value.ToString().Substring(0, 1)}{Environment.NewLine}";
            else
                label += "Strn: NONE" + Environment.NewLine;

            label += $"Wt:   {fruitingBlock.Weight}{Environment.NewLine}";
            label += $"Rec:  {ShortenIfNeeded(fruitingBlock.RecipeName)}{Environment.NewLine}";
            label += $"Ckd:  {fruitingBlock.CreateDate.DayOfYear} / {GetInitials(fruitingBlock.CreateUser)}{Environment.NewLine}";

            if (fruitingBlock.InnoculateDate.HasValue)
            {
                label += $"Inoc: {fruitingBlock.InnoculateDate.Value.DayOfYear} / {GetInitials(fruitingBlock.InnoculateUser)}{Environment.NewLine}";
            }

            Text = label;
            Barcode = $"FB{fruitingBlock.Id}";
            BarcodeSize = BarcodeSize.Medium;
            TextSize = TextSize.Medium;
            MaxTextWidth = 24;
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

        private string ShortenIfNeeded(string val, int maxLengthOffset = 0)
        {
            if (val.Length > (18 + maxLengthOffset))
                return GetInitials(val);
            return val;
        }
    }
}