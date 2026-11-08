using System.Collections;
using Unity.VisualScripting;
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


    #region Room Exit
    /*
        The possible positions for an exit, relative to the room's borders.
        Also serves as a bit field to list out the room's exits.
    */
    [System.Flags]
    public enum ExitDirection
    {
        None = 0,
        Left = 1 << 0,
        Right = 1 << 1,
        Up = 1 << 2,
        Down = 1 << 3
    }


    // The room's exit directions.
    public ExitDirection roomExits;


    // Spawns the room's exit points depending on roomExits value.
    private void SpawnExits()
    {
        // TODO : Fill this function (especially the part where it checks the flags in roomExits).
    }


    // Return the position of the room's exit door, depending on its direction.
    private Vector3 GetExitPosition(ExitDirection direction, Vector3 offset)
    {
        if (!spawner)
        {
            return Vector3.zero;
        }

        // The room size, as decided by the RoomSpawner.
        Vector3 _roomSize = (Vector3)spawner.roomSize;
        Vector3 _exitPosition = transform.position;

        switch (direction)
        {
            case ExitDirection.Left:
                _exitPosition = -0.5f * new Vector3(_roomSize.x, 0) + offset;
                break;
            case ExitDirection.Right:
                _exitPosition = 0.5f * new Vector3(_roomSize.x, 0) + offset;
                break;
            case ExitDirection.Down:
                _exitPosition = -0.5f * new Vector3(0, _roomSize.y) + offset;
                break;
            case ExitDirection.Up:
                _exitPosition = 0.5f * new Vector3(0, _roomSize.y) + offset;
                break;
        }

        return _exitPosition;
    }
    #endregion


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
