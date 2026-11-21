using System;
using System.Collections.Generic;
using UnityEngine;

// Holds the items a player has picked up and applies their stat bonuses (no item limit)
[RequireComponent(typeof(Stats))]
public class PlayerInventory : MonoBehaviour
{
    private readonly List<InventoryItem> _items = new List<InventoryItem>(); // Items currently held
    private Stats _stats;

    public event Action OnInventoryChanged; // Fired whenever an item is added or removed (for the slot UI)

    public IReadOnlyList<InventoryItem> Items => _items;

    private void Awake()
    {
        _stats = GetComponent<Stats>();
    }

    // Adds an item and applies its stat bonuses. Returns false if there's no item
    public bool TryAdd(ItemData itemData)
    {
        if (itemData == null)
        {
            return false;
        }

        InventoryItem item = new InventoryItem(itemData);
        _items.Add(item);

        // The InventoryItem is the source, so two copies of the same item can be removed separately
        foreach (ItemStatBonus bonus in itemData.statBonuses)
        {
            _stats.AddModifier(new StatModifier(bonus.statType, bonus.value, bonus.isPercent, item));
        }

        OnInventoryChanged?.Invoke();
        return true;
    }

    // Removes an item and every stat bonus it gave
    public bool Remove(InventoryItem item)
    {
        if (!_items.Remove(item))
        {
            return false;
        }

        _stats.RemoveModifiersFromSource(item);
        OnInventoryChanged?.Invoke();
        return true;
    }
}

// One held copy of an item. Wraps the shared ItemData so each pickup is its own modifier source
public class InventoryItem
{
    public ItemData Data { get; }

    public InventoryItem(ItemData data)
    {
        Data = data;
    }
}