using DefaultNamespace;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    [SerializeField] private GameObject upOpen;
    [SerializeField] private GameObject upClose;
    [SerializeField] private GameObject downOpen;
    [SerializeField] private GameObject downClose;
    [SerializeField] private GameObject leftOpen;
    [SerializeField] private GameObject leftClose;
    [SerializeField] private GameObject rightOpen;
    [SerializeField] private GameObject rightClose;
    
    private RoomData _roomData;

    public void InitializeRoom(RoomData roomData)
    {
        _roomData = roomData;
        CloseOpenDoors();
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
}
