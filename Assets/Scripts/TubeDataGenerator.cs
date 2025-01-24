using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public static class TubeDataGenerator
{
    private const int maxRooms = 10;
    private const int borderLimit = 2;

    public enum DIRECTION
    {
        DOWN,
        LEFT,
        RIGHT,
        UP
    }

    public static List<RoomData> GenerateRoomsData()
    {
        int currentPositionX = 0;
        int currentPositionY = 0;

        var roomsData = new List<RoomData>
        {
            new RoomData
            {
                isTurn = false,
                posX = currentPositionX,
                posY = currentPositionY
            }
        };

        var lastTurned = true;
        DIRECTION currentDirection = DIRECTION.DOWN;

        while (roomsData.Count < maxRooms)
        {
            DIRECTION oldDirection = currentDirection;
            DIRECTION newDirection = lastTurned ? currentDirection : (DIRECTION) Random.Range(0, 3);

            // Ensure the new direction doesn't conflict with border limits or directly reverse
            if ((newDirection == DIRECTION.LEFT && currentPositionX <= -borderLimit) ||
                (newDirection == DIRECTION.RIGHT && currentPositionX >= borderLimit))
            {
                newDirection = DIRECTION.DOWN;
            }

            if ((oldDirection == DIRECTION.LEFT && newDirection == DIRECTION.RIGHT) ||
                (oldDirection == DIRECTION.RIGHT && newDirection == DIRECTION.LEFT))
            {
                newDirection = DIRECTION.DOWN;
            }

            // Determine if the tile is a turning point
            bool isTurning = oldDirection != newDirection;
            if (!isTurning) lastTurned = false;
            else lastTurned = true;

            // Update direction and position
            currentDirection = newDirection;
            switch (newDirection)
            {
                case DIRECTION.DOWN:
                    currentPositionY -= 1;
                    break;
                case DIRECTION.LEFT:
                    currentPositionX -= 1;
                    break;
                case DIRECTION.RIGHT:
                    currentPositionX += 1;
                    break;
            }

            // Add the new room data
            roomsData.Add(new RoomData
            {
                posX = currentPositionX,
                posY = currentPositionY
            });

            roomsData[^2].isTurn = isTurning;
        }

        // Add entrance and exit directions for each room
        for (int i = 0; i < roomsData.Count; i++)
        {
            if (i > 0)
            {
                roomsData[i].entranceDirection = GetDirection(roomsData[i],roomsData[i - 1], true);
            }

            if (i < roomsData.Count - 1)
            {
                roomsData[i].exitDirection = GetDirection(roomsData[i], roomsData[i + 1], false);
            }
        }

        return roomsData;
    }

    private static DIRECTION GetDirection(RoomData from, RoomData to, bool up)
    {
        if (to.posX > from.posX) return DIRECTION.RIGHT;
        if (to.posX < from.posX) return DIRECTION.LEFT;
        return up ? DIRECTION.UP : DIRECTION.DOWN;
    }
}
