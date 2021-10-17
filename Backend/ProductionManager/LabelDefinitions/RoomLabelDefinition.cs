using Solarponics.Models;

namespace Solarponics.ProductionManager.LabelDefinitions
{
    public class RoomLabelDefinition : LabelDefinition
    {
        public RoomLabelDefinition(Room room)
            : base(room.Name, $"R{room.Id}", maxTextWidth: 30)
        {

        }
    }
}