using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public static class TubeGenerator
{
    private const int maxRooms = 10;

    public static GameObject corridorRoomPrefab;
    public static GameObject turnRoomPrefab;
    
    private static int startingPosX = 0;
    private static int startingPosY = 0;
    
    public static List<RoomData> GenerateRoomsData()
    {
        return new List<RoomData>();
    }
    
}
