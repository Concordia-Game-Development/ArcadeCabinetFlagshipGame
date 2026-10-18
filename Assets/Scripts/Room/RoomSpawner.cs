using System;
using Unity.Collections.Tests.CoreCLR.TestJobs;
using Unity.VisualScripting;
using UnityEngine;

// Room spawning logic.
public class RoomSpawner : MonoBehaviour
{
    // The position where the next room will be spawned.
    private Vector3 _spawnPosition = Vector3.zero;


    // The room's size stored as a 2D vector.
    public Vector2 roomSize = Vector2.zero;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnRoom(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


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


    // Defines where the next room will be spawned.
    private void SetSpawnPosition()
    {
        return;
    }


    // Picks a room to spawn.
    private GameObject PickRoom()
    {
        return null;
    }
}
