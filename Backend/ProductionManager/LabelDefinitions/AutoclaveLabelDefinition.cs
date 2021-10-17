using Solarponics.Models;
using System;

namespace Solarponics.ProductionManager.LabelDefinitions
{
    public class AutoclaveLabelDefinition : LabelDefinition
    {
        public AutoclaveLabelDefinition(Autoclave autoclave)
            : base($"Name:       {autoclave.Name}{Environment.NewLine}Location:   {autoclave.LocationName}{Environment.NewLine}Room:    {autoclave.RoomName}{Environment.NewLine}{autoclave.Details}", $"AC{autoclave.Id}", maxTextWidth: 30)
        {
        }
    }
}