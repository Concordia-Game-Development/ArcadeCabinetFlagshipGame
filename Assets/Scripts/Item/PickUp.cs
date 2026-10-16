using UnityEngine;
using UnityEngine.InputSystem;

public class ItemTestPickup : MonoBehaviour
{
    public WorldItem worldItem;

    void Update()
    {
        // Press the 'E' key to test picking up the item
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (worldItem != null)
            {
                // This calls your pickup method, passes the data to the description toggle, and destroys the world object
                ItemData collectedData = worldItem.PickUp();
                Debug.Log("Picked up: " + collectedData.itemName);
            }
        }
    }
}