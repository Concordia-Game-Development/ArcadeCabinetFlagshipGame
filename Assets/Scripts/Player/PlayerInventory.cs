using System;
using System.Collections.Generic;
using UnityEngine;

// holds the items a player has picked up and applies their stat bonuses
[RequireComponent(typeof(Stats))] //safetynet
public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int _maxItems = 10; // acc design doc, caps inventory at 8-10 items. can change that later

    private readonly List<InventoryItem> _items = new List<InventoryItem>(); // Items currently held
    private Stats _stats;

    public event Action OnInventoryChanged; // Fired whenever an item is added or removed (for the slot UI)

    public IReadOnlyList<InventoryItem> Items => _items; //all changes go through tryAdd and Remove
    public int MaxItems => _maxItems; //if we need to check later
    public bool IsFull => _items.Count >= _maxItems;

    private void Awake()
    {
        _stats = GetComponent<Stats>();
    }

    //adds an item and applies its stat bonuses. Returns false if the inventory is full
    public bool TryAdd(ItemData itemData)
    {
        if (itemData == null || IsFull) {
            return false;
        }
        InventoryItem item = new InventoryItem(itemData);
        _items.Add(item);

        //InventoryItem is the source. so two copies of the same item can be removed separately
        foreach (ItemStatBonus bonus in itemData.statBonuses) {
            _stats.AddModifier(new StatModifier(bonus.statType, bonus.value, bonus.isPercent, item));
        }
        OnInventoryChanged?.Invoke();
        return true;
    }

    //removes an item and every stat bonus it gave
    public bool Remove(InventoryItem item)
    {
        if (!_items.Remove(item)) {
            return false;
        }
        _stats.RemoveModifiersFromSource(item);
        OnInventoryChanged?.Invoke();
        return true;
    }
}

// one held copy of an item. Wraps the shared ItemData so each pickup is its own modifier source
public class InventoryItem
{
    public ItemData Data { get; }
    public InventoryItem(ItemData data) {
        Data = data;
    }
}