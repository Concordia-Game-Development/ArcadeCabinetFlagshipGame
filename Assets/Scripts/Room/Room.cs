using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// Room class. Attaching this to a GameObject will make it a valid room.
public class Room : MonoBehaviour
{
    // TEMP : This is just a temporary method that makes the room clear itself after 3 seconds, to showcase the room spawning and room transition effects.
    private IEnumerator TempClearRoom()
    {
        yield return new WaitForSeconds(2f);
        ClearRoom();
    }


    // The RoomSpawner responsible for this room.
    public RoomSpawner spawner = null;
    // The opposite direction from which the room was entered (ExitDirection.Right means that the room was entered from the left).
    public ExitDirection entryDirection = ExitDirection.None;
    // The direction from which the room was exited.
    public ExitDirection exitDirection = ExitDirection.None;


    void Start()
    {
        TeleportPlayer();
        EnterRoom();
        // TEMP : See TempClearRoom() definition.
        StartCoroutine(TempClearRoom());
    }


    #region Public Methods
    // Going into the next room, triggered by the child RoomExit.
    public void NextRoom()
    {
        StartCoroutine(NextRoomTransition());
    }


    // Called upon completing the room (defeating all enemies and claiming the items or something).
    public void ClearRoom()
    {
        // Any behavior that happens when the room is completed (opening doors, playing sounds and stuff) goes here.
        CreateExits();
    }
    #endregion


    // Upon creating the room.
    #region Room Initialization


    // Putting the players at the entrance.
    private void TeleportPlayer()
    {

    }


    // Called upon entering the room.
    private void EnterRoom()
    {
        StartCoroutine(EnterRoomTransition());
    }
    #endregion


    #region Room Structure
    #endregion


    // Managing the room's exit triggers.
    #region Room Exits
    /*
        The possible positions for an exit, relative to the room's borders.
        Also serves as a bit field to list out the room's exits.
    */
    [Flags]
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
        // Iterates through the possible exit positions.
        foreach (ExitDirection direction in Enum.GetValues(typeof(ExitDirection)))
        {
            // If the room is supposed to have an exit at this position, it spawns one.
            if (HasExit(direction))
            {
                SpawnExit(direction);
            }
        }
    }


    // Spawns a single exit.
    private void SpawnExit(ExitDirection direction)
    {
        // Skips the method if there is no prefab to use.
        if (!(spawner && spawner.roomExitPrefab))
        {
            return;
        }

        // Gets the transform information for this exit.
        Pose _exitTransform = GetExitPosition(direction, Vector3.zero);

        // Creates an exit.
        GameObject _instance = Instantiate(spawner.roomExitPrefab, transform);
        _instance.transform.position = _exitTransform.position;
        _instance.transform.rotation = _exitTransform.rotation;

        // Destroys the "exit" if it turns out not to be an exit.
        if (!_instance.GetComponent<RoomExit>())
        {
            Destroy(_instance);
            Debug.LogError("The loaded prefab " + _instance.name + " passed to SpawnExit() is not a RoomExit !", gameObject);
            return;
        }

        // Passes a reference of this room to the exit.
        RoomExit _roomExit = _instance.GetComponent<RoomExit>();
        _roomExit.room = this;
        _roomExit.direction = direction;
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


    // Calling transitions between rooms.
    #region Room Transition
    // The room transition overlay.
    public RoomSwitchOverlay overlay = null;


    // Coroutine for exiting room.
    private IEnumerator NextRoomTransition()
    {
        if (!spawner)
        {
            yield break;
        }

        // TODO : Freeze inputs

        if (overlay)
        {
            yield return StartCoroutine(overlay.FadeOut());
        }

        spawner.SwitchRoom(exitDirection);
    }


    // Coroutine for entering room.
    private IEnumerator EnterRoomTransition()
    {
        if (overlay)
        {
            yield return StartCoroutine(overlay.FadeIn());
        }

        // TODO : Unfreeze inputs
    }
    #endregion
}
