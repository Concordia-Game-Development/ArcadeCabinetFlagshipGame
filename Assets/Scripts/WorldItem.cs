using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] DescriptionToggle tog;


    //allows the item to be picked up
    public ItemData PickUp()
    {
        tog.setItem(itemData);
        Destroy(gameObject);
        return itemData;
    }
}
