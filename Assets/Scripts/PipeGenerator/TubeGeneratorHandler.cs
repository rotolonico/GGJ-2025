using System;
using Unity.VisualScripting;
using UnityEngine;

public class TubeGeneratorHandler : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private Vector2 roomSize;

    private void Start()
    {
        GenerateRooms();
    }


    private void GenerateRooms()
    {
        var roomDatas = TubeDataGenerator.GenerateRoomsData();
        
        for (var i = 0; i < roomDatas.Count; i++)
        {
            var roomData = roomDatas[i];

            // Generate test prefab separated based on size of the prefab
            var prefabPosition = new Vector3(roomData.posX * roomSize.x, roomData.posY * roomSize.y, 0);
            var newRoom = Instantiate(roomPrefab, prefabPosition, Quaternion.identity, parent).GetComponent<RoomHandler>();
            newRoom.InitializeRoom(roomData);
            
            foreach (var roomDataSecondaryRoom in roomData.secondaryRooms)
            {
                var secondaryRoomData = roomDataSecondaryRoom.Key;
                
                var secondaryRoomPosition = new Vector3(secondaryRoomData.posX * roomSize.x, secondaryRoomData.posY * roomSize.y, 0);
                var secondaryRoom = Instantiate(roomPrefab, secondaryRoomPosition, Quaternion.identity, parent).GetComponent<RoomHandler>();
                secondaryRoom.InitializeRoom(secondaryRoomData);
            }
        }
    }
}
