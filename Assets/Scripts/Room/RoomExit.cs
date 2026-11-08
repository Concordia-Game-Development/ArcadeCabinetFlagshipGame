using UnityEngine;

// The exit trigger of the room.
public class RoomExit : MonoBehaviour
{
    public Room room = null;


    public Room.ExitDirection exitDirection = Room.ExitDirection.Up;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        return;
    }

    // Update is called once per frame
    void Update()
    {
        return;
    }


    #region Exiting Room
    // OnTriggerEnter is called when the Collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        GameObject _colliderOwner = other.gameObject;

        // Checks whether an actual player passed the exit.
        if (IsPlayer(_colliderOwner))
        {
            // TODO : Count players to validate room switching.
            ExitRoom();
        }
    }


    // Whatever happens when the party exits the room (calls RoomSpawner.SwitchRoom() somehow).
    private void ExitRoom()
    {
        if (!room)
        {
            return;
        }

        room.NextRoom();
    }
    #endregion


    // Checks whether colliderOwner is a player (the method is empty for now because idk what will tell that away).
    private bool IsPlayer(GameObject colliderOwner)
    {
        return colliderOwner.tag == "Player";
    }
}
