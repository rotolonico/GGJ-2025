using System.Collections.Generic;
using System.Linq;

namespace DefaultNamespace
{
    public class RoomData
    {
        public int primaryRoomIndex;

        public bool isTurn;
        public int posX;
        public int posY;

        public bool isPuzzleRoom;
        public bool isSecondaryRoom;
        public bool isValveRoom;
        public bool isHealingRoom;

        public bool isItemRoom;
        public int itemRoomIndex;

        public List<KeyValuePair<RoomData, TubeDataGenerator.DIRECTION>> secondaryRooms = new();

        public TubeDataGenerator.DIRECTION entranceDirection = TubeDataGenerator.DIRECTION.NONE;
        public TubeDataGenerator.DIRECTION exitDirection = TubeDataGenerator.DIRECTION.NONE;

        public bool IsStartRoom()
        {
            return entranceDirection == TubeDataGenerator.DIRECTION.NONE;
        }

        public bool IsBossRoom()
        {
            return exitDirection == TubeDataGenerator.DIRECTION.NONE && !isSecondaryRoom;
        }

        public bool IsDirectionOpen(TubeDataGenerator.DIRECTION direction)
        {
            return entranceDirection == direction || exitDirection == direction ||
                   secondaryRooms.Any(s => s.Value == direction);
        }
    }
}