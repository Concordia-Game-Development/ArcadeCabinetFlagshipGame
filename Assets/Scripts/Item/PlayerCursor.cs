using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// TEMPORARY test controls, picked by player number until per-player input exists
public enum CursorTestInput
{
    None, // Driven only by SetMoveInput / Confirm (real input)
    Mouse, // Follows the mouse, left click to pick
    WASD, // WASD to move, Space to pick
    Arrows // Arrow keys to move, Enter to pick
}

// One player's cursor on the reward screen
// The pivot of the prefab should sit on the arrow's tip, that's the point used for hovering
public class PlayerCursor : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private float _speed = 900f; // Canvas units per second at full stick

    private RectTransform _rectTransform;
    private RectTransform _bounds;
    private RewardScreen _screen;
    private CursorTestInput _testInput;
    private Vector2 _moveInput;

    public PlayerInventory Player { get; private set; }
    public Color Color { get; private set; }

    public void Setup(PlayerInventory player, Color color, int playerIndex, RewardScreen screen, RectTransform bounds){
        _rectTransform = (RectTransform)transform;
        _screen = screen;
        _bounds = bounds;
        Player = player;
        Color = color;

        if (_image != null){
            _image.color = color;
        }

        // Player 1 mouse, player 2 WASD, player 3 arrows, player 4 mbappe special
        switch (playerIndex){
            case 0: _testInput = CursorTestInput.Mouse; break;
            case 1: _testInput = CursorTestInput.WASD; break;
            case 2: _testInput = CursorTestInput.Arrows; break;
            default: _testInput = CursorTestInput.None; break;
        }

        //start near the middle, spread out so cursors don't overlap
        _rectTransform.localPosition = _bounds.rect.center + new Vector2((playerIndex - 1.5f) * 60f, 0f);
    }

    //call these from the players real input once it exists
    public void SetMoveInput(Vector2 input){
        _moveInput = Vector2.ClampMagnitude(input, 1f);
    }

    public void Confirm(){
        _screen.OnCursorConfirm(this);
    }

    private void Update(){
        if (_testInput == CursorTestInput.Mouse){
            FollowMouse();
            return;
        }
        ReadKeyboardTestInput();

        // Unscaled time because the game is paused while picking
        Vector3 position = _rectTransform.localPosition + (Vector3)(_moveInput * _speed * Time.unscaledDeltaTime);

        // Keep the cursor on screen
        Rect area = _bounds.rect;
        position.x = Mathf.Clamp(position.x, area.xMin, area.xMax);
        position.y = Mathf.Clamp(position.y, area.yMin, area.yMax);
        _rectTransform.localPosition = position;
    }

    private void FollowMouse()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null){
            return;
        }

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_bounds, mouse.position.ReadValue(), _screen.UICamera, out Vector2 localPoint)){
            _rectTransform.localPosition = localPoint;
        }

        if (mouse.leftButton.wasPressedThisFrame){
            Confirm();
        }
    }

    private void ReadKeyboardTestInput(){
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null || _testInput == CursorTestInput.None){
            return;
        }

        Vector2 input = Vector2.zero;
        bool confirmPressed = false;

        if (_testInput == CursorTestInput.WASD){
            if (keyboard.wKey.isPressed) input.y += 1f;
            if (keyboard.sKey.isPressed) input.y -= 1f;
            if (keyboard.dKey.isPressed) input.x += 1f;
            if (keyboard.aKey.isPressed) input.x -= 1f;
            confirmPressed = keyboard.spaceKey.wasPressedThisFrame;
        }
        else if (_testInput == CursorTestInput.Arrows){
            if (keyboard.upArrowKey.isPressed) input.y += 1f;
            if (keyboard.downArrowKey.isPressed) input.y -= 1f;
            if (keyboard.rightArrowKey.isPressed) input.x += 1f;
            if (keyboard.leftArrowKey.isPressed) input.x -= 1f;
            confirmPressed = keyboard.enterKey.wasPressedThisFrame;
        }

        SetMoveInput(input);
        if (confirmPressed){
            Confirm();
        }
    }
}