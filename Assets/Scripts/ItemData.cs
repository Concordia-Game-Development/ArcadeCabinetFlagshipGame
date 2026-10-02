using UnityEngine;

public class ItemData : ScriptableObject
{
    //a script to hold item information
    public string itemName;
    [TextArea(3, 5)]
    public string itemDescription;
    public Sprite itemIcon;
}