using Solarponics.Models;

namespace Solarponics.ProductionManager.Data
{
    public class RoomLabelDefinition : LabelDefinition
    {
        public RoomLabelDefinition(Room room)
            : base(room.Name, $"R{room.Id}", maxTextWidth: 30)
        {

        }
    }
}