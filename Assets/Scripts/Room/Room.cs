using System.Collections;
using UnityEngine;

// Room class. Attaching this to a GameObject will make it a valid room.
public class Room : MonoBehaviour
{
    // The RoomSpawner responsible for this room.
    public RoomSpawner spawner = null;
    // The room transition overlay.
    public RoomSwitchOverlay overlay = null;


    void Start()
    {
        EnterRoom();
    }


    void Update()
    {
        return;
    }


    // Called upon entering the room.
    public void EnterRoom()
    {
        StartCoroutine(EnterRoomTransition());
    }


    // Going into the next room, triggered by the child RoomExit.
    public void NextRoom()
    {
        StartCoroutine(NextRoomTransition());
    }


    public Vector3 ExitPosition(Vector3 offset)
    {
        if (!spawner)
        {
            return Vector3.zero;
        }

        Vector3 _roomSize = new Vector3(0, spawner.roomSize.y);

        return 0.5f * _roomSize + offset + transform.position;
    }


    private IEnumerator NextRoomTransition()
    {
        if (!spawner)
        {
            yield break;
        }

        if (overlay)
        {
            yield return StartCoroutine(overlay.FadeOut());
        }

        spawner.SwitchRoom();
    }


    private IEnumerator EnterRoomTransition()
    {
        if (overlay)
        {
            StartCoroutine(overlay.FadeIn());
        }
        yield break;
    }
}
