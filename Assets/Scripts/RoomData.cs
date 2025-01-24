namespace DefaultNamespace
{
    public class RoomData
    {
        public bool isTurn;
        public int posX;
        public int posY;
        
        public TubeDataGenerator.DIRECTION entranceDirection;
        public TubeDataGenerator.DIRECTION exitDirection;

        public bool IsDirectionOpen(TubeDataGenerator.DIRECTION direction)
        {
            return entranceDirection == direction || exitDirection == direction;
        }
    }
}