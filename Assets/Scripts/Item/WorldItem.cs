using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] DescriptionToggle tog; // optional- items spawned from code won't have one

    public ItemData ItemData => itemData;
    //allows the item to be picked up
    public ItemData PickUp()
    {
        if (tog != null)
        {
            tog.setItem(itemData);
        }

        Destroy(gameObject);
        return itemData;
    }
}