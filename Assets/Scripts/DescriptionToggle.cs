using UnityEngine;
using UnityEngine.UI;

public class DescriptionToggle : MonoBehaviour
{
    [Header("Menu")]
    public GameObject targetMenu;

    [Header("DescriptionUI")]
    private ItemData data;

    //Gets the button in the scene 
    private void Awake()
    {
        Button myButton = GetComponent<Button>();

        if (myButton != null)
        {
            myButton.onClick.AddListener(ToggleMenu);
        }
    }

    //called when the item is picked up
    public void setItem(ItemData data){
        this.data = data;
    }
    //sets the activity to the opposite of what it is
    public void ToggleMenu()
    {
        if (targetMenu != null)
        {
            bool isActive = targetMenu.activeSelf;
            targetMenu.SetActive(!isActive);
            DescriptionUI.Instance.ShowDescription(data);
        }
    }
}
