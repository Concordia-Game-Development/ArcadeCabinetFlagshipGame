using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unity.Collections.Tests.CoreCLR.TestJobs;
using Unity.VisualScripting;
using UnityEngine;

// Room spawning logic.
public class RoomSpawner : MonoBehaviour
{
    // Calls SetRoomsArray().
    void Awake()
    {
        SetRoomsArray();
    }

    // Called in the beginning.
    void Start()
    {
        SwitchRoom();
    }

    // Update is called once per frame.
    void Update()
    {

    }


    // The method wrapping the room-switching mechanic. It can be called by the game manager or something after the room is cleared.
    public void SwitchRoom()
    {
        GameObject _room = PickRoom();

        _room = SpawnRoom(_room);

        if (_room)
        {
            Debug.Log("Successfully switched to room " + _room.name, gameObject);
        }
    }


    #region Room Spawning
    // Spawns a new Room from a prefab.
    private GameObject SpawnRoom(GameObject room)
    {
        // Catches an error if a null value was passed.       
        if (!room)
        {
            Debug.LogWarning("SpawnRoom() called with null room reference !", gameObject);
            return null;
        }

        // Defines the position where the next room will be spawned.
        Vector3 _spawnPosition = SetSpawnPosition();

        // Instantiates the room at the spawn position.
        GameObject _instance = Instantiate(room, _spawnPosition, Quaternion.identity);

        if (!_instance.GetComponent<Room>())
        {
            Destroy(_instance);
            Debug.LogError("The loaded prefab " + _instance.name + " passed to SpawnRoom() is not a room !", gameObject);
            return null;
        }

        // Returns a reference to the room which was created.
        return _instance;
    }
    #endregion


    #region Spawn Position
    // The position where the initial room is spawned.
    public Vector3 spawnPosition = Vector3.zero;
    // The room's size stored as a 2D vector.
    public Vector2 roomSize = Vector2.zero;

    // The direction where the party exits a room after clearing it, as a normalized vector.
    public Vector3 exitDirection = Vector3.up;


    // Defines where the next room will be spawned.
    private Vector3 SetSpawnPosition()
    {
        // Moves the spawnPosition to the location of the next room.
        spawnPosition += new Vector3(roomSize.x * exitDirection.x, roomSize.y * exitDirection.y);
        Debug.Log(spawnPosition);
        return spawnPosition;
    }
    #endregion


    #region Room Directory
    // The directory containing the room prefabs.
    [SerializeField] private string _roomsFolder = "Assets/Resources/Room Prefabs";

    // The array that will store all room paths.
    private string[] _availableRooms = Array.Empty<string>();


    // Fills the _availableRooms array with the path (relative to the Resources folder) for every spawnable room prefab. Called with Start()
    private void SetRoomsArray()
    {
        // Gets an array of all file paths in the _roomsFolder directory.
        string[] _roomPaths = Directory.GetFiles(_roomsFolder, "*.prefab");

        // Catches a warning if the folder contains no prefab.
        if (_roomPaths.Length == 0)
        {
            Debug.LogWarning("Rooms folder is empty !", gameObject);
            return;
        }

        // Cuts each path to remove the part up to the Resources directory.
        for (int id = 0; id < _roomPaths.Length; id += 1)
        {
            string _path = _roomPaths[id];
            _path = _path[(_path.IndexOf("Resources") + "Resources/".Length)..];
            _path = Path.ChangeExtension(_path, null);
            _roomPaths[id] = _path;
        }

        _availableRooms = _roomPaths;
        Debug.Log("_availableRooms = [" + string.Join(", ", _availableRooms) + "]");
    }
    #endregion


    #region Room Picking
    // Picks a room to spawn from _availableRooms.
    private GameObject PickRoom()
    {
        // Warning if there are no available room prefabs to pick.
        if (_availableRooms.Length == 0)
        {
            Debug.LogWarning("No available room to pick !", gameObject);
            return null;
        }

        // Picks a random path in _availableRooms.
        string _roomPath = _availableRooms[UnityEngine.Random.Range(0, _availableRooms.Length)];

        GameObject _room = Resources.Load<GameObject>(_roomPath);

        // Warning if the room could not be loaded.
        if (!_room)
        {
            Debug.LogError("Prefab at path " + _roomPath + " could not be loaded in PickRoom() !", gameObject);
            return null;
        }

        // Returns the loaded room.
        return _room;
    }
    #endregion
}
