using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unity.Collections.Tests.CoreCLR.TestJobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

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
        ResetRoomSpawner();
        InvokeRepeating("SwitchRoom", 1, 2);
    }

    // Update is called once per frame.
    void Update()
    {
        return;
    }

    #region Public Methods
    // The method wrapping the room-switching mechanic. It can be called by the game manager or something after the room is cleared.
    public void SwitchRoom()
    {
        // Creates a new room.
        GameObject _roomObject = SpawnRoom(
            PickRoom()
        );

        if (!_roomObject)
        {
            return;
        }

        if (roomNumber > 1)
        {
            // Deletes the previous room.
            DeleteRoom(roomNumber - 2);
        }

        Room _room = _roomObject.GetComponent<Room>();

        if (!_room)
        {
            return;
        }

        _room.EnterRoom();
    }


    // The method to reset the room spawner. It can be called by the game manager or something when the entire party got obliterated.
    public void ResetRoomSpawner()
    {
        roomNumber = 0;
        DeleteAllRooms();
        SwitchRoom();
    }
    #endregion


    // Delete a single room, by its index in _spawnedRooms.
    private void DeleteRoom(int roomId)
    {
        // In case the game tries to remove an invalid room.
        if (!(roomId >= 0 && roomId < _spawnedRooms.Count))
        {
            Debug.LogError("Deleting a room that doesn't exist : " + roomId.ToString(), gameObject);
            return;
        }

        // Finding the room.
        GameObject _room = _spawnedRooms[roomId];

        if (!_room)
        {
            Debug.LogWarning("Already no room at " + roomId.ToString(), gameObject);
        }

        // Destroying the room.
        Destroy(_room);
    }


    // Deletes all spawned rooms with references within _spawnedRooms. Skips the hassle if there are no rooms.
    private void DeleteAllRooms()
    {
        if (_spawnedRooms.Count == 0)
        {
            return;
        }

        for (int id = 0; id < _spawnedRooms.Count; id += 1)
        {
            DeleteRoom(id);
        }
    }



    // Spawning the room and tracking the instances.
    #region Room Spawning
    // Keeps a reference of every spawned room.
    private List<GameObject> _spawnedRooms = new List<GameObject>();


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

        _spawnedRooms.Add(_instance);

        // Passes a reference of the spawner to the room.
        Room _spawnedRoom = _instance.GetComponent<Room>();
        _spawnedRoom.spawner = this;

        // Returns a reference to the room which was created.
        return _instance;
    }
    #endregion


    // Defining where to spawn the next room.
    #region Spawn Position
    // The position where the initial room is spawned.
    public Vector3 initialSpawnPosition = Vector3.zero;
    // The room's size in Unity's mystery units, stored as a 2D vector.
    public Vector2 roomSize = Vector2.zero;

    // The direction where the party exits a room after clearing it, as a normalized vector.
    public Vector3 exitDirection = Vector3.up;


    // The number of rooms already generated. Also works for the index (starting at 0) of the party's next room.
    private int roomNumber = 0;


    // Defines where the next room will be spawned.
    private Vector3 SetSpawnPosition()
    {
        /*
        // Moves the initialSpawnPosition to the location of the next room.
        Vector3 _spawnPosition = initialSpawnPosition + roomNumber * new Vector3(roomSize.x * exitDirection.x, roomSize.y * exitDirection.y);
        roomNumber += 1;
        return _spawnPosition;
        */

        roomNumber += 1;
        return Vector3.zero;
    }
    #endregion


    // Finding the room prefabs in their directory.
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
    }
    #endregion


    // Pickng a room to spawn.
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
