using UnityEngine;

// Room class. Attaching this to a GameObject will make it a valid room.
public class Room : MonoBehaviour
{
    // The RoomSpawner responsible for this room.
    public RoomSpawner spawner = null;


    void Start()
    {
        return;
    }


    void Update()
    {
        return;
    }


    // Going into the next room, triggered by the child RoomExit.
    public void NextRoom()
    {
        if (!spawner)
        {
            return;
        }

        spawner.SwitchRoom();
    }


    public Vector3 ExitPosition(Vector3 offset)
    {
        if (!spawner)
        {
            return Vector3.zero;
        }

        Vector3 _roomSize = new Vector3(0, spawner.roomSize.y);

        return 0.5f * _roomSize + offset;
    }
}
