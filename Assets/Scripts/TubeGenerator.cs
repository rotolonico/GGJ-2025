using DefaultNamespace;
using UnityEngine;

public static class TubeGenerator
{
    private const int maxRooms = 10;

    public static GameObject corridorRoomPrefab;
    public static GameObject turnRoomPrefab;
    
    private static int startingPosX = 0;
    private static int startingPosY = 0;
    
    public static RoomData[] GenerateRooms()
    {
        foreach (var room in rooms)
        {
            GameObject prefab = room.isTurn ? turnRoomPrefab : corridorRoomPrefab;
            GameObject roomObject = Object.Instantiate(prefab);
            roomObject.transform.position = new Vector3(room.posX, room.posY, 0);
        }
    }
    
}
