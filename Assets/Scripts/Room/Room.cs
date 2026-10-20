using UnityEngine;

// Room class. Attaching this to a GameObject will make it a valid room.
public class Room : MonoBehaviour
{
    // The RoomSpawner responsible for this room.
    public RoomSpawner spawner = null;


    public void NextRoom()
    {
        if (!spawner)
        {
            return;
        }

        spawner.SwitchRoom();
    }
}
