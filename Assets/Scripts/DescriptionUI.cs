using UnityEngine;
using TMPro;

public class DescriptionUI : MonoBehaviour
{
    //Instance of the ui, get means anyone can look, pivate set, mean only yhis cahne change it
    public static DescriptionUI Instance {
        get;
        private set;
    }
    private TMP_Text itemName;
    private TMP_Text itemDescription;

    //gets instance and makes sure that everythign is hidden
    void Awake()
    {
        Instance = this;
    }

    //shows description
    public void ShowDescription(ItemData data){
        if (data == null || itemName ==null || itemDescription == null){
            return;
        }
        itemName.text = data.itemName;
        itemDescription.text = data.itemDescription;

    }


   
}
