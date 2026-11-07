using System.Collections;
using UnityEngine;

// Room class. Attaching this to a GameObject will make it a valid room.
public class Room : MonoBehaviour
{
    // TEMP : This is just a temporary method that makes the room clear itself after 3 seconds, to showcase the room spawning and room transition effects.
    private IEnumerator ClearRoom()
    {
        yield return new WaitForSeconds(3f);
        NextRoom();
    }


    // The RoomSpawner responsible for this room.
    public RoomSpawner spawner = null;
    // The room transition overlay.
    public RoomSwitchOverlay overlay = null;


    void Start()
    {
        EnterRoom();
        // TEMP : See ClearRoom() definition.
        StartCoroutine(ClearRoom());
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


    // Return the position of the room's exit door.
    public Vector3 ExitPosition(Vector3 offset)
    {
        if (!spawner)
        {
            return Vector3.zero;
        }

        Vector3 _roomSize = new Vector3(0, spawner.roomSize.y);

        return 0.5f * _roomSize + offset + transform.position;
    }


    #region Room Transition
    // Coroutine for room transition.
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


    // Coroutine for entering room.
    private IEnumerator EnterRoomTransition()
    {
        if (overlay)
        {
            StartCoroutine(overlay.FadeIn());
        }
        yield break;
    }
    #endregion
}
