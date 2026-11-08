using System;
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
        CreateExits();
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
        None = 0, // 0b0000
        Left = 1 << 0, // 0b0001
        Right = 1 << 1, // 0b0010
        Up = 1 << 2, // 0b0100
        Down = 1 << 3 // 0b1000
    }


    // The room's exit directions.
    public ExitDirection roomExits;


    // Returns whether the room possesses these exit directions.
    private bool HasExit(ExitDirection direction)
    {
        int _direction = (int)direction;
        return _direction != 0 && (_direction & (int)roomExits) == _direction;
    }


    // Creates the room's exit points depending on roomExits value.
    private void CreateExits()
    {
        foreach (ExitDirection direction in Enum.GetValues(typeof(ExitDirection)))
        {
            if (HasExit(direction))
            {
                SpawnExit(direction);
            }
        }
    }


    // Spawns a single exit.
    private void SpawnExit(ExitDirection direction)
    {
        // TODO : Spawning logic.
        Debug.Log(GetExitPosition(direction, Vector3.zero));
    }


    // Return a transform for the room's exit door, depending on its direction.
    private Pose GetExitPosition(ExitDirection direction, Vector3 offset)
    {
        if (!spawner)
        {
            return new Pose();
        }

        // The room size, as decided by the RoomSpawner.
        Vector3 _roomSize = (Vector3)spawner.roomSize;
        Vector3 _exitPosition = transform.position;
        Quaternion _exitRotation = new Quaternion();

        switch (direction)
        {
            case ExitDirection.Left:
                _exitPosition = -0.5f * new Vector3(_roomSize.x, 0) + offset;
                _exitRotation = Quaternion.Euler(0, 0, 180);
                break;
            case ExitDirection.Right:
                _exitPosition = 0.5f * new Vector3(_roomSize.x, 0) + offset;
                _exitRotation = Quaternion.Euler(0, 0, 0);
                break;
            case ExitDirection.Down:
                _exitPosition = -0.5f * new Vector3(0, _roomSize.y) + offset;
                _exitRotation = Quaternion.Euler(0, 0, -90);
                break;
            case ExitDirection.Up:
                _exitPosition = 0.5f * new Vector3(0, _roomSize.y) + offset;
                _exitRotation = Quaternion.Euler(0, 0, 90);
                break;
        }

        return new Pose(_exitPosition, _exitRotation);
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
