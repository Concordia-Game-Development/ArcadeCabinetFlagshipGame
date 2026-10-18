using System;
using Unity.Collections.Tests.CoreCLR.TestJobs;
using Unity.VisualScripting;
using UnityEngine;

// Room spawning logic.
public class RoomSpawner : MonoBehaviour
{
    // The position where the next room will be spawned.
    private Vector3 _spawnPosition = Vector2.zero;

    
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
    private Room SpawnRoom(GameObject room)
    {
        // Catches an error if a null value was passed.       
        if (!room)
        {
            return null;
        }
        
        // Instantiates the room at the spawn position.
        UnityEngine.Object _instance = Instantiate(room, _spawnPosition, Quaternion.identity);

        // Catches an error if a non-Room prefab was passed.
        if (_instance.GetComponent<Room>() != null)
        {
            Destroy(_instance);
            return null;
        }

        return null;
    }


    // Defines where the next room will be spawned.
    private void SetSpawnPosition()
    {
        
    }


    // Picks a room to spawn.
    private GameObject PickRoom()
    {
        return null;
    }
}
