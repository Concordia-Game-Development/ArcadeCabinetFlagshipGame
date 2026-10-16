using UnityEngine;
using TMPro;

public class DescriptionUI : MonoBehaviour
{
    //Instance of the ui, get means anyone can look, pivate set, mean only yhis cahne change it
    public static DescriptionUI Instance {
        get;
        private set;
    }
    [Header("UI Refrences")]
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private Sprite itemIcon;

    //gets instance and makes sure that everythign is hidden
    //uses a singleton pattern in order to make clicking a differnt oobject show differnt text and for scalability
    void Awake()
    {
        if (Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    //shows description
    public void ShowDescription(ItemData data){
        if (data == null || itemName ==null || itemDescription == null){
            return;
        }
        itemName.text = data.itemName;
        itemDescription.text = data.itemDescription;
        itemIcon = data.itemIcon;

    }


   
}
