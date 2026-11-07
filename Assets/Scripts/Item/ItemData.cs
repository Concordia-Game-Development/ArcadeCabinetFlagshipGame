using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/Item Data")] // !! to crewate a new item in project window. under create
public class ItemData : ScriptableObject
{
    //a script to hold item information
    public string itemName;
    [TextArea(3, 5)] //3 to 5 lines should be okay?
    public string itemDescription;
    public Sprite itemIcon;

    // stat bonuses this item gives while it's held, editable in the inspector though
    public List<ItemStatBonus> statBonuses = new List<ItemStatBonus>();
}

// One stat bonus as set up in the Inspector. Turned into a StatModifier when the item is picked up
[Serializable]
public class ItemStatBonus
{
    public StatType statType; // Which stat this bonus changes
    public float value; // Flat amount or a fraction if percent (0.2 = +20%) 
    public bool isPercent; // True for a percent bonus, false for a flat bonus
}