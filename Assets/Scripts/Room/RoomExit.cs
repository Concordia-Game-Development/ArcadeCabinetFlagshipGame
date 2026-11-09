using UnityEngine;

// The exit trigger of the room.
public class RoomExit : MonoBehaviour
{
    // The amount of players required to be in the exit at the same time to exit the room.
    public static int targetPlayerCount = 1;
    // The room this exit belongs to.
    public Room room = null;
    // The exit direction. Doesn't do anything.
    public Room.ExitDirection direction = Room.ExitDirection.Up;
    // The exit's trigger hitbox.
    public Collider2D hitbox = null;


    // The number of players currently standing in the exit.
    private int playerCount = 0;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Registers all overlapping colliders.
        Collider2D[] colliderArray = new Collider2D[targetPlayerCount];
        hitbox.Overlap(ContactFilter2D.noFilter, colliderArray);

        playerCount = 0;
        foreach (Collider2D collider in colliderArray)
        {
            if (!collider)
            {
                break;
            }
            GameObject colliderOwner = collider.gameObject;
            if (colliderOwner.tag == "Player")
            {
                playerCount += 1;
            }
        }

        if (playerCount >= targetPlayerCount)
        {
            ExitRoom();
        }
    }


    // Called to exit the room.
    private void ExitRoom()
    {
        if (!room)
        {
            return;
        }

        // Triggers the room change.
        room.NextRoom();
    }
}