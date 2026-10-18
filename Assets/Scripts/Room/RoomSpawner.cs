using System;
using System.IO;
using System.Linq;
using Unity.Collections.Tests.CoreCLR.TestJobs;
using Unity.VisualScripting;
using UnityEngine;

// Room spawning logic.
public class RoomSpawner : MonoBehaviour
{
    // The room's size stored as a 2D vector.
    public Vector2 roomSize = Vector2.zero;


    // Start is called once before the first execution of Update after the MonoBehaviour is created.
    void Start()
    {
        SetRoomsArray();
    }

    // Update is called once per frame.
    void Update()
    {

    }


    // The method wrapping the room-switching mechanic. It can be called by the game manager or something after the room is cleared.
    public void SwitchRoom()
    {
        SetSpawnPosition();
        GameObject _room = PickRoom();

        SpawnRoom(_room);
    }


    #region Room Spawning
    // Spawns a new Room from a prefab.
    private GameObject SpawnRoom(GameObject room)
    {
        // Catches an error if a null value was passed.       
        if (!room)
        {
            Debug.LogWarning("SpawnRoom() called with null room reference !");
            return null;
        }

        // Instantiates the room at the spawn position.
        GameObject _instance = Instantiate(room, _spawnPosition, Quaternion.identity);

        // Catches an error if a non-Room prefab was passed.
        if (!_instance.GetComponent<Room>())
        {
            Destroy(_instance);
            Debug.LogWarning("SpawnRoom() called with something that is not a room !");
            return null;
        }

        // Returns a reference to the valid room which was created.
        return _instance;
    }
    #endregion


    #region Spawn Position
    // The position where the next room will be spawned.
    private Vector3 _spawnPosition = Vector3.zero;


    // Defines where the next room will be spawned.
    private void SetSpawnPosition()
    {
        return;
    }
    #endregion


    #region Room Picking
    // The folder where room prefabs are stored.
    [SerializeField] private string _roomsFolder;

    // The array that will store all room paths.
    private string[] _availableRooms = Array.Empty<string>();


    // Fills the _availableRooms array with the path for every spawnable room prefab.
    private void SetRoomsArray()
    {
        _availableRooms = Directory.GetFiles(_roomsFolder, "*.prefab");
        if (_availableRooms.Length == 0)
        {
            Debug.LogWarning("Rooms folder is empty !");
        }
        else
        {
            Debug.Log($"Available Rooms : {string.Join(", ", _availableRooms)}");
        }
    }


    // Picks a room to spawn.
    private GameObject PickRoom()
    {
        return null;
    }
    #endregion
}
