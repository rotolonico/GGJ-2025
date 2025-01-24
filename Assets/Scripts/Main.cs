using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public GameObject testPrefab;
    public GameObject xPrefab;
    public GameObject oPrefab;
    
    private List<GameObject> rooms = new List<GameObject>();
    
    [ContextMenu("Generate Rooms")]
    private void Start()
    {
        var test = TubeGenerator.GenerateRoomsData();

        foreach (var room in rooms)
        {
            Destroy(room);
        }
        
        rooms.Clear();
        
        for (var i = 0; i < test.Count; i++)
        {
            var roomData = test[i];
            Debug.Log($"isTurn: {roomData.isTurn}, posX: {roomData.posX}, posY: {roomData.posY}");

            // Generate test prefab separated based on size of the prefab
            var prefabSize = testPrefab.GetComponent<SpriteRenderer>().bounds.size;
            var prefabPosition = new Vector3(roomData.posX * prefabSize.x, roomData.posY * prefabSize.y, 0);
            var newPrefab = Instantiate(testPrefab, prefabPosition, Quaternion.identity);
            var sr = newPrefab.GetComponent<SpriteRenderer>();
            sr.color = roomData.isTurn ? Color.red : Color.green;
            
            rooms.Add(newPrefab);

            // Add xPrefab and oPrefab near entrances and exits
            if (i != 0)
            {
                Vector3 entrancePosition = prefabPosition + GetOffset(roomData.entranceDirection, prefabSize);
                Instantiate(xPrefab, entrancePosition, Quaternion.identity);
            }

            if (i != test.Count - 1)
            {
                Vector3 exitPosition = prefabPosition + GetOffset(roomData.exitDirection, prefabSize);
                Instantiate(oPrefab, exitPosition, Quaternion.identity);
            }
        }
    }

    private Vector3 GetOffset(TubeGenerator.DIRECTION direction, Vector3 prefabSize)
    {
        switch (direction)
        {
            case TubeGenerator.DIRECTION.DOWN:
                return new Vector3(0, -prefabSize.y / 2, 0);
            case TubeGenerator.DIRECTION.UP:
                return new Vector3(0, prefabSize.y / 2, 0);
            case TubeGenerator.DIRECTION.LEFT:
                return new Vector3(-prefabSize.x / 2, 0, 0);
            case TubeGenerator.DIRECTION.RIGHT:
                return new Vector3(prefabSize.x / 2, 0, 0);
            default:
                return Vector3.zero;
        }
    }
}
