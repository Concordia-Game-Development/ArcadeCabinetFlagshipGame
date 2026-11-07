using UnityEngine;
using UnityEngine.InputSystem;

// as mia intended to test, press E to put the item into the player's inventory
public class ItemTestPickup : MonoBehaviour
{
    public WorldItem worldItem;
    public PlayerInventory inventory;

    void Update()
    {
        if (!Keyboard.current.eKey.wasPressedThisFrame) {
            return;
        }

        if (worldItem == null || inventory == null) { return;}
        ItemData data = worldItem.ItemData;

        if (inventory.TryAdd(data)) {
            worldItem.PickUp();
            Debug.Log("Picked up: " + data.itemName);
        }
        else {
            Debug.Log("Inventory full, couldn't pick up " + data.itemName);
        }
    }
}