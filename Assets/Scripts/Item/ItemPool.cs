using System.Collections.Generic;
using UnityEngine;

// A list of every item that can show up as a reward
[CreateAssetMenu(fileName = "NewItemPool", menuName = "Inventory/Item Pool")]
public class ItemPool : ScriptableObject
{
    [SerializeField] private List<ItemData> _items = new List<ItemData>();

    // Returns up to count different items in random order
    public List<ItemData> GetRandomItems(int count)
    {
        List<ItemData> shuffled = new List<ItemData>(_items);
        shuffled.RemoveAll(item => item == null);

        //shuffling w fisheryates
        for (int i = shuffled.Count - 1; i > 0; i--){
            int j = Random.Range(0, i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }

        if (shuffled.Count < count){
            Debug.LogWarning(name + " only has " + shuffled.Count + " items, wanted " + count);
        }

        return shuffled.GetRange(0, Mathf.Min(count, shuffled.Count));
    }
}