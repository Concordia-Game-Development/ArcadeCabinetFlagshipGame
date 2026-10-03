using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{

  
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
  }

  private void FixedUpdate()
  {
    //smooths the movement over a period of time
    _smoothedMovementInput = Vector2.SmoothDamp(_smoothedMovementInput, _movementInput, ref _movementInputSmoothVelocity, _velocityChangeSpeed);
    //this moves the player according to whatever input is given
    _playerRigidbody.linearVelocity = _smoothedMovementInput * _speed;
  }  



  //called when input from player is given
  private void OnMove(InputValue inputValue)
  {
    _movementInput = inputValue.Get<Vector2>();
  }

}
