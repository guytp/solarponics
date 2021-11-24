using Solarponics.Models;
using System;

namespace Solarponics.ProductionManager.LabelDefinitions
{
    public class GrainSpawnLabelDefinition : LabelDefinition
    {
        public GrainSpawnLabelDefinition(GrainSpawn grainSpawn)
        {
            var label = string.Empty;

            if (grainSpawn.Strain != null)
                label += $"Strn: {ShortenIfNeeded(grainSpawn.Strain, -3)} / {grainSpawn.MediumType.Value.ToString().Substring(0, 1)}{Environment.NewLine}";
            else
                label += "Strn: NONE" + Environment.NewLine;

            label += $"Wt:   {grainSpawn.Weight}{Environment.NewLine}";
            label += $"Rec:  {ShortenIfNeeded(grainSpawn.RecipeName)}{Environment.NewLine}";
            label += $"Ckd:  {grainSpawn.CreateDate.DayOfYear} / {GetInitials(grainSpawn.CreateUser)}{Environment.NewLine}";

            if (grainSpawn.InnoculateDate.HasValue)
            {
                label += $"Inoc: {grainSpawn.InnoculateDate.Value.DayOfYear} / {GetInitials(grainSpawn.InnoculateUser)}{Environment.NewLine}";
            }

            Text = label;
            Barcode = $"GS{grainSpawn.Id}";
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