using DefaultNamespace;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    public static readonly Vector2 RoomSize = new(33.333333f, 33.333333f);
    
    public enum RoomType
    {
        SIMPLE_ROOM,
        ITEM_ROOM,
        PUZZLE_ROOM
    }
    
    [SerializeField] private RoomType roomType;
    
    [SerializeField] private GameObject upOpen;
    [SerializeField] private GameObject upClose;
    [SerializeField] private GameObject downOpen;
    [SerializeField] private GameObject downClose;
    [SerializeField] private GameObject leftOpen;
    [SerializeField] private GameObject leftClose;
    [SerializeField] private GameObject rightOpen;
    [SerializeField] private GameObject rightClose;

    [SerializeField] private GameObject startRoomTestFlag;
    [SerializeField] private GameObject bossRoomTestFlag;
    [SerializeField] private GameObject[] itemRoomsTestFlag;
    [SerializeField] private GameObject puzzleRoomTestFlag;
    [SerializeField] private GameObject secondaryRoomTestFlag;

    [SerializeField] private Transform rotatorParent;
    
    private RoomData _roomData;
    
    public RoomType GetRoomType() => roomType;

    public void InitializeRoom(RoomData roomData)
    {
        _roomData = roomData;
        CloseOpenDoors();
        
        if (roomData.IsStartRoom())
        {
            startRoomTestFlag.SetActive(true);
        }
        
        if (roomData.IsBossRoom())
        {
            bossRoomTestFlag.SetActive(true);
        }
        
        if (roomData.isSecondaryRoom)
        {
            secondaryRoomTestFlag.SetActive(true);
        }

        if (roomData.isPuzzleRoom)
        {
            puzzleRoomTestFlag.SetActive(true);
        }
        
        if (roomData.isItemRoom)
        {
            itemRoomsTestFlag[roomData.itemRoomIndex].SetActive(true);
        }
        
        RotateBasedOnExitDirection();
    }

    private void RotateBasedOnExitDirection()
    {
        rotatorParent.rotation = Quaternion.Euler(0, 0, _roomData.entranceDirection switch
        {
            TubeDataGenerator.DIRECTION.UP => 0,
            TubeDataGenerator.DIRECTION.DOWN => 180,
            TubeDataGenerator.DIRECTION.LEFT => 90,
            TubeDataGenerator.DIRECTION.RIGHT => -90,
            _ => 0
        });
    }

    private void CloseOpenDoors()
    {
        var up = _roomData.IsDirectionOpen(TubeDataGenerator.DIRECTION.UP);
        var down = _roomData.IsDirectionOpen(TubeDataGenerator.DIRECTION.DOWN);
        var left = _roomData.IsDirectionOpen(TubeDataGenerator.DIRECTION.LEFT);
        var right = _roomData.IsDirectionOpen(TubeDataGenerator.DIRECTION.RIGHT);
        
        upOpen.SetActive(up);
        upClose.SetActive(!up);
        
        downOpen.SetActive(down);
        downClose.SetActive(!down);
        
        leftOpen.SetActive(left);
        leftClose.SetActive(!left);
        
        rightOpen.SetActive(right);
        rightClose.SetActive(!right);
    }

    public object GetRoomData() => _roomData;

    public bool IsStartRoom() => _roomData.IsStartRoom();
    
    public bool IsBossRoom() => _roomData.IsBossRoom();
}
