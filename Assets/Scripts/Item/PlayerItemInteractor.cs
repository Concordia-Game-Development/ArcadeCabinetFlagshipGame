using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Lets a player pick up the closest reward item they're standing at
// Needs a Collider2D + Rigidbody2D on the player
[RequireComponent(typeof(PlayerInventory))]
public class PlayerItemInteractor : MonoBehaviour
{
    [SerializeField] private Key _testInteractKey = Key.E; // TEMPORARY: give each test player a different key until per-player input exists

    private readonly List<RewardItem> _itemsInRange = new List<RewardItem>();
    private PlayerInventory _inventory;

    private void Awake()
    {
        _inventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        if (_testInteractKey == Key.None || Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current[_testInteractKey].wasPressedThisFrame)
        {
            TryInteract();
        }
    }

    // Call this from the player's interact button once real input exists
    public void TryInteract()
    {
        _itemsInRange.RemoveAll(item => item == null); // Clears out items that were picked or removed

        RewardItem closest = null;
        float closestDistance = float.MaxValue;

        foreach (RewardItem item in _itemsInRange)
        {
            float distance = Vector2.Distance(transform.position, item.transform.position);
            if (distance < closestDistance)
            {
                closest = item;
                closestDistance = distance;
            }
        }

        if (closest != null)
        {
            closest.TryPick(_inventory);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        RewardItem item = other.GetComponentInParent<RewardItem>();
        if (item != null && !_itemsInRange.Contains(item))
        {
            _itemsInRange.Add(item);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        RewardItem item = other.GetComponentInParent<RewardItem>();
        if (item != null)
        {
            _itemsInRange.Remove(item);
        }
    }
}