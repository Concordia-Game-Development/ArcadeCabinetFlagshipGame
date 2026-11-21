using TMPro;
using UnityEngine;
using UnityEngine.UI;

// One item card on the reward screen
public class RewardCard : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _outline; //slightly bigger image behind the card, shown when hovered
    [SerializeField] private GameObject _claimedOverlay; //grey image covering the card, shown once claimed
    [SerializeField] private Image _claimedMarker; //small image tinted with the color of the player who claimed it

    public bool IsClaimed { get; private set; }
    public RectTransform Rect => (RectTransform)transform;

    public void Setup(ItemData item)
    {
        IsClaimed = false;

        if (_icon != null){
            _icon.sprite = item.itemIcon;
            _icon.enabled = item.itemIcon != null;
        }
        if (_nameText != null){
            _nameText.text = item.itemName;
        }
        if (_descriptionText != null){
            _descriptionText.text = item.itemDescription;
        }

        SetHighlight(false, Color.white);

        if (_claimedOverlay != null){
            _claimedOverlay.SetActive(false);
        }

        if (_claimedMarker != null){
            _claimedMarker.enabled = false;
        }
    }

    public void SetHighlight(bool show, Color color)
    {
        if (_outline == null){
            return;
        }
        _outline.enabled = show && !IsClaimed; // Claimed cards never light up
        _outline.color = color;
    }

    public void SetClaimed(Color playerColor)
    {
        IsClaimed = true;
        SetHighlight(false, Color.white);

        if (_claimedOverlay != null){
            _claimedOverlay.SetActive(true);
        }

        if (_claimedMarker != null){
            _claimedMarker.enabled = true;
            _claimedMarker.color = playerColor;
        }
    }
}
