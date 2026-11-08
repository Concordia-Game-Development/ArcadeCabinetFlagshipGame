using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{

  //publicly available to other classes
  /*
  public InputSystem_Actions InputActions
{
  get {
    return InputActions;
  }
  //private 
  set {
    InputActions = value;
  }
}
  */
  public InputSystem_Actions InputActions { get; private set; }

  //player object
  private Player _player;

  private Rigidbody2D _playerRigidbody;

  // Direction can be get from other files but only set in this file
  public Vector2Int Direction { private set; get; }

  //direction of movement
  private Vector2 _movementInput;

  //so the stop and go isn't as jarring, smoothed out movement
  private Vector2 _smoothedMovementInput;
  //keeps track of velocity of the change
  private Vector2 _movementInputSmoothVelocity;
  //how fast the player will go from one velocity to another (acceleration?)
  [SerializeField] private float acceleration;

  //speed of the player
  [SerializeField] private float speed;


  //called when the scene is first initialized
  private void Awake()
  {
    _player = GetComponent<Player>();
    //gets the rigidbody from the player
    _playerRigidbody = GetComponent<Rigidbody2D>();

    InputActions = GameInput.Instance.InputActions;

    Direction = Vector2Int.one;


  }

  //happens every frame (relies on computer)
  private void Update()
  {
    //smooths the movement over a period of time
    _smoothedMovementInput = Vector2.SmoothDamp(_smoothedMovementInput, _movementInput, ref _movementInputSmoothVelocity, acceleration);
    //this moves the player according to whatever input is given
    _playerRigidbody.linearVelocity = _smoothedMovementInput * speed;
  }


  //Deals with all player movements (does the movement action)
  private void MovePlayer(InputAction.CallbackContext context)
  {

    if (context.canceled)
    {
      _movementInput = Vector2.zero;
    }
    else
    {
      Debug.Log(_movementInput);
      _movementInput = context.ReadValue<Vector2>();
      Direction = Vector2Int.RoundToInt(_movementInput);
    }


  }


  //----------------------FOR THE INPUT SYSTEM---------------------//
  #region input stuff

  //runs when player becomes enabled 
  private void OnEnable()
  {
    InputActions.Player.Enable();
    InputActions.Player.Move.performed += MovePlayer;
    InputActions.Player.Move.canceled += MovePlayer;
  }
  //runs when player becomes disabled (death?)
  private void OnDisable()
  {
    InputActions.Player.Disable();
    InputActions.Player.Move.performed -= MovePlayer;
    InputActions.Player.Move.canceled -= MovePlayer;
  }


  #endregion input stuff


}
