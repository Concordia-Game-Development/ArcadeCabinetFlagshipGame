using UnityEngine;
using UnityEngine.UI;

public class DescriptionToggle : MonoBehaviour
{
    [Header("Menu")]
    public GameObject targetMenu;
    public static DescriptionToggle currentButton;

    [Header("DescriptionUI")]
    private ItemData data;
    [SerializeField] private Image iconImage;

     

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
        if (data != null && iconImage != null)
        {
            iconImage.sprite = data.itemIcon;
            iconImage.gameObject.SetActive(true); 
        }
    }
    //sets the activity to the opposite of what it is
    public void ToggleMenu()
    {

        if (targetMenu == null || data == null){
            return;
        }

        if(currentButton == this){
            targetMenu.SetActive(false);
            currentButton = null;
        }else{
            if (currentButton != null && currentButton.targetMenu != null && currentButton.targetMenu != targetMenu){
                currentButton.targetMenu.SetActive(false);
            }
            targetMenu.SetActive(true);
            DescriptionUI.Instance.ShowDescription(data);
            currentButton = this;
        }


        
        
    }

    private void OnDestroy()
    {
        Debug.Log(gameObject.name + " was destroyed by something!", this);
    }
}
