using Solarponics.Models;
using System;

namespace Solarponics.ProductionManager.LabelDefinitions
{
    public class ShelfLabelDefinition : LabelDefinition
    {
        public ShelfLabelDefinition(Shelf shelf)
            : base($"Name:       {shelf.Name}{Environment.NewLine}Location:   {shelf.LocationName}{Environment.NewLine}Room:       {shelf.RoomName}", $"SH{shelf.Id}", maxTextWidth: 30)
        {
        }
    }
}