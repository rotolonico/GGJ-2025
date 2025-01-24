using System;
using Unity.VisualScripting;
using UnityEngine;

public class TubeGeneratorHandler : MonoBehaviour
{
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
            Debug.Log($"isTurn: {roomData.isTurn}, posX: {roomData.posX}, posY: {roomData.posY}");

            // Generate test prefab separated based on size of the prefab
            var prefabPosition = new Vector3(roomData.posX * roomSize.x, roomData.posY * roomSize.y, 0);
            var newRoom = Instantiate(roomPrefab, prefabPosition, Quaternion.identity).GetComponent<RoomHandler>();
            newRoom.InitializeRoom(roomData);
        }
    }
}
