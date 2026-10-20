using UnityEngine;

// The exit trigger of the room.
public class RoomExit : MonoBehaviour
{
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


    // OnTriggerEnter is called when the Collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        ExitRoom();
    }


    // Whatever happens when the party exits the room (call RoomSpawner.SwitchRoom() somehow).
    private void ExitRoom()
    {
        return;
    }
}
