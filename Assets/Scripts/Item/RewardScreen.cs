using System.Collections.Generic;
using TMPro;
using UnityEngine;

// The reward popup: builds the item cards, spawns a cursor per player, handles hover outlines
// Put this on the Canvas (not on the panel, since the panel gets hidden)
public class RewardScreen : MonoBehaviour
{
    [SerializeField] private GameObject _panel; // Shown while picking, hidden otherwise
    [SerializeField] private Transform _cardContainer; // Has a Grid Layout Group (3 columns)
    [SerializeField] private RewardCard _cardPrefab;
    [SerializeField] private RectTransform _cursorLayer; // Full screen area above the cards, cursors live here
    [SerializeField] private PlayerCursor _cursorPrefab;
    [SerializeField] private TMP_Text _timerText; // Optional countdown
    [SerializeField] private Camera _uiCamera; // Leave empty for a Screen Space - Overlay canvas

    [Header("Colors")]
    [SerializeField] private Color[] _playerColors =
    {
        new Color(0.2f, 0.6f, 1f), // Blue
        new Color(1f, 0.85f, 0.1f), // Yellow
        new Color(0.3f, 0.85f, 0.3f), // Green
        new Color(0.95f, 0.25f, 0.2f) // Red
    };
    [SerializeField] private Color _multiHoverColor = new Color(0.65f, 0.3f, 0.95f); // Purple, when 2+ players hover one card

    private RewardPhase _rewardPhase;
    private readonly List<RewardCard> _cards = new List<RewardCard>();
    private readonly List<PlayerCursor> _cursors = new List<PlayerCursor>();

    public Camera UICamera => _uiCamera;

    private void Awake(){
        _panel.SetActive(false);
    }

    public void Show(RewardPhase rewardPhase, List<ItemData> items, List<PlayerInventory> players){
        _rewardPhase = rewardPhase;
        ClearScreen();

        foreach (ItemData item in items){
            RewardCard card = Instantiate(_cardPrefab, _cardContainer);
            card.Setup(item);
            _cards.Add(card);
        }

        for (int i = 0; i < players.Count; i++){
            if (!rewardPhase.IsStillPicking(players[i])){
                continue;
            }

            PlayerCursor cursor = Instantiate(_cursorPrefab, _cursorLayer);
            cursor.Setup(players[i], GetPlayerColor(i), i, this, _cursorLayer);
            _cursors.Add(cursor);
        }

        _panel.SetActive(true);
    }

    public void Hide(){
        ClearScreen();
        _panel.SetActive(false);
    }

    private void Update(){
        if (_rewardPhase == null || !_rewardPhase.IsActive){return;}
        UpdateHighlights();
        if (_timerText != null){
            _timerText.text = _rewardPhase.HasTimer ? Mathf.CeilToInt(Mathf.Max(0f, _rewardPhase.TimeRemaining)).ToString() : "";
        }
    }

    // No cursor: no outline. One cursor: their color. Two or more: purple
    private void UpdateHighlights(){
        for (int i = 0; i < _cards.Count; i++)
        {
            int hoverCount = 0;
            Color hoverColor = Color.white;

            foreach (PlayerCursor cursor in _cursors)
            {
                if (cursor.gameObject.activeSelf && GetCardIndexUnder(cursor) == i)
                {
                    hoverCount++;
                    hoverColor = cursor.Color;
                }
            }

            if (hoverCount == 0)
            {
                _cards[i].SetHighlight(false, Color.white);
            }
            else
            {
                _cards[i].SetHighlight(true, hoverCount == 1 ? hoverColor : _multiHoverColor);
            }
        }
    }

    // returns the index of the card under the cursor's tip, or -1
    public int GetCardIndexUnder(PlayerCursor cursor){
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(_uiCamera, cursor.transform.position);
        for (int i = 0; i < _cards.Count; i++){
            if (RectTransformUtility.RectangleContainsScreenPoint(_cards[i].Rect, screenPoint, _uiCamera))
            {
                return i;
            }
        }
        return -1;
    }

    // A cursor pressed its confirm button
    public void OnCursorConfirm(PlayerCursor cursor){
        int index = GetCardIndexUnder(cursor);
        if (index >= 0){
            _rewardPhase.TryClaim(cursor.Player, index);
        }
    }

    // Greys out the card and hides the cursor of the player who picked
    public void OnItemClaimed(int itemIndex, PlayerInventory player){
        foreach (PlayerCursor cursor in _cursors){
            if (cursor.Player == player){
                _cards[itemIndex].SetClaimed(cursor.Color);
                cursor.gameObject.SetActive(false);
                return;
            }
        }
 _cards[itemIndex].SetClaimed(Color.white);
    }

    private Color GetPlayerColor(int playerIndex){
        if (_playerColors.Length == 0){
            return Color.white;
        }
        return _playerColors[playerIndex % _playerColors.Length];
    }

    private void ClearScreen(){
        foreach (RewardCard card in _cards){
            Destroy(card.gameObject);
        }
        foreach (PlayerCursor cursor in _cursors){
            Destroy(cursor.gameObject);
        }
        _cards.Clear();
        _cursors.Clear();
    }
}