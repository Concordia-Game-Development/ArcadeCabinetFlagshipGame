using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
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
  public InputSystem_Actions InputActions {get; private set;}



  private Rigidbody2D _playerRigidbody;

  //direction of movement
  private Vector2 _movementInput;

  //so the stop and go isn't as jarring, smoothed out movement
  private Vector2 _smoothedMovementInput;
  //keeps track of velocity of the change
  private Vector2 _movementInputSmoothVelocity;
  //how fast the player will go from one velocity to another (acceleration?)
  [SerializeField] private float _velocityChangeSpeed;

  //speed of the player
  [SerializeField] private float _speed;

  
  //called when the scene is first initialized
  private void Awake()
  {    
    //gets the rigidbody from the player
    _playerRigidbody = GetComponent<Rigidbody2D>();

    InputActions = new InputSystem_Actions();


  }

  //happens every frame (relies on computer)
  private void Update()
  {
    //smooths the movement over a period of time
    _smoothedMovementInput = Vector2.SmoothDamp(_smoothedMovementInput, _movementInput, ref _movementInputSmoothVelocity, _velocityChangeSpeed);
    //this moves the player according to whatever input is given
    _playerRigidbody.linearVelocity = _smoothedMovementInput * _speed;
  }

  //happens 
  private void FixedUpdate()
  {
   
  }  

  //Deals with all player movements (does the movement action)
  private void MovePlayer(InputAction.CallbackContext context)
  {
    if(context.canceled){
      _movementInput = Vector2.zero;
    }
    else{
      _movementInput = context.ReadValue<Vector2>();
    }
    
    
  }


  // //called when input from player is given
  // private void OnMove(InputValue inputValue)
  // {
  //   _movementInput = inputValue.Get<Vector2>();
  // }

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
