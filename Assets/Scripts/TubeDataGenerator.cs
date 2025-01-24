using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public static class TubeDataGenerator
{
    private const int maxRooms = 10;
    private const int borderLimit = 2;
    private const int maxSecondaryRooms = 5;
    private const int numberOfItemRooms = 2;

    public enum DIRECTION
    {
        DOWN,
        LEFT,
        RIGHT,
        UP,
        NONE
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

        List<KeyValuePair<RoomData, DIRECTION>> possibleSecondaryRooms = new List<KeyValuePair<RoomData, DIRECTION>>();

        while (roomsData.Count < maxRooms)
        {
            DIRECTION oldDirection = currentDirection;
            DIRECTION newDirection = lastTurned ? currentDirection : (DIRECTION)Random.Range(0, 3);

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

            roomsData[^1].primaryRoomIndex = roomsData.Count - 1;
            roomsData[^2].isTurn = isTurning;
        }

        // Add entrance and exit directions for each room
        for (int i = 0; i < roomsData.Count; i++)
        {
            if (i > 0)
            {
                roomsData[i].entranceDirection = GetDirection(roomsData[i], roomsData[i - 1], true);
            }

            if (i < roomsData.Count - 1)
            {
                roomsData[i].exitDirection = GetDirection(roomsData[i], roomsData[i + 1], false);
            }

            if (i > 1)
            {
                var i1 = i;
                var possibleSecRooms = GetPossibleSecondaryRoomDirections(roomsData[i - 1])
                    .Select(d => new KeyValuePair<RoomData, DIRECTION>(roomsData[i1 - 1], d));
                foreach (var possibleSecRoom in possibleSecRooms)
                {
                    // Check if this room would overlap another possible secondary room
                    var possibleSecondaryRoomPosition =
                        AddDirection(new Vector2Int(possibleSecRoom.Key.posX, possibleSecRoom.Key.posY),
                            possibleSecRoom.Value);
                    if (possibleSecondaryRooms.Any(s =>
                            AddDirection(new Vector2Int(s.Key.posX, s.Key.posY), s.Value) ==
                            possibleSecondaryRoomPosition))
                    {
                        continue;
                    }

                    possibleSecondaryRooms.Add(
                        new KeyValuePair<RoomData, DIRECTION>(possibleSecRoom.Key, possibleSecRoom.Value));
                }
            }
        }

        // Add one random puzzle room in the second half of the rooms
        int puzzleRoomIndex = Random.Range(roomsData.Count / 2, roomsData.Count - 1);
        roomsData[puzzleRoomIndex].isPuzzleRoom = true;

        Shuffle(possibleSecondaryRooms);

        // Add numberOfItemRooms item secondary rooms before the puzzle

        for (int i = 0; i < numberOfItemRooms; i++)
        {
            var itemRoomIndex = possibleSecondaryRooms.FindIndex(r => r.Key.primaryRoomIndex < puzzleRoomIndex);
            var itemRoom = possibleSecondaryRooms[itemRoomIndex];
            possibleSecondaryRooms.RemoveAt(itemRoomIndex);

            var newItemRoom = AddSecondaryRoom(itemRoom);
            newItemRoom.isItemRoom = true;
            newItemRoom.itemRoomIndex = i;
        }

        // Add other secondary rooms until max secondary room limit is reached
        var otherSecondaryRooms = possibleSecondaryRooms.Take(maxSecondaryRooms - numberOfItemRooms).ToList();
        foreach (var otherSecondaryRoom in otherSecondaryRooms)
        {
            AddSecondaryRoom(otherSecondaryRoom);
        }

        return roomsData;
    }

    private static DIRECTION GetDirection(RoomData from, RoomData to, bool up)
    {
        if (to.posX > from.posX) return DIRECTION.RIGHT;
        if (to.posX < from.posX) return DIRECTION.LEFT;
        return up ? DIRECTION.UP : DIRECTION.DOWN;
    }

    private static DIRECTION GetOppositeDirection(DIRECTION direction)
    {
        switch (direction)
        {
            case DIRECTION.DOWN:
                return DIRECTION.UP;
            case DIRECTION.LEFT:
                return DIRECTION.RIGHT;
            case DIRECTION.RIGHT:
                return DIRECTION.LEFT;
            case DIRECTION.UP:
                return DIRECTION.DOWN;
            default:
                return DIRECTION.NONE;
        }
    }

    private static Vector2Int AddDirection(Vector2Int position, DIRECTION direction)
    {
        switch (direction)
        {
            case DIRECTION.DOWN:
                return new Vector2Int(position.x, position.y - 1);
            case DIRECTION.LEFT:
                return new Vector2Int(position.x - 1, position.y);
            case DIRECTION.RIGHT:
                return new Vector2Int(position.x + 1, position.y);
            case DIRECTION.UP:
                return new Vector2Int(position.x, position.y + 1);
            default:
                return position;
        }
    }

    private static DIRECTION[] GetPossibleSecondaryRoomDirections(RoomData roomData)
    {
        var possibleDirections = new List<DIRECTION> { DIRECTION.DOWN, DIRECTION.LEFT, DIRECTION.RIGHT, DIRECTION.UP };

        possibleDirections.Remove(roomData.entranceDirection);
        possibleDirections.Remove(roomData.exitDirection);

        return possibleDirections.ToArray();
    }

    private static RoomData AddSecondaryRoom(KeyValuePair<RoomData, DIRECTION> itemRoom)
    {
        var primaryRoomData = itemRoom.Key;
        var secondaryRoomPos = AddDirection(new Vector2Int(primaryRoomData.posX, primaryRoomData.posY), itemRoom.Value);
        var secondaryRoomData = new RoomData
        {
            isSecondaryRoom = true,
            posX = secondaryRoomPos.x,
            posY = secondaryRoomPos.y,
            entranceDirection = GetOppositeDirection(itemRoom.Value)
        };

        primaryRoomData.secondaryRooms.Add(new KeyValuePair<RoomData, DIRECTION>(secondaryRoomData, itemRoom.Value));

        return secondaryRoomData;
    }

    private static void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            // Generate a random index between 0 and i
            int j = Random.Range(0, i + 1);

            // Swap elements at i and j
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}