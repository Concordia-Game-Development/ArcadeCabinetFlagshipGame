using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
  private Rigidbody2D _playerRigidbody;
  //direction of movement
  private Vector2 _movementInput;

  //called when the scene is first initialized
  private void Awake()
  {
    //gets the rigidbody from the player
    _playerRigidbody = GetComponent<Rigidbody2D>();
  }

  private void FixedUpdate()
  {
    _playerRigidbody.linearVelocity = _movementInput;
  }  



  //called when input from player is given
  private void OnMove(InputValue inputValue)
  {
    _movementInput = inputValue.Get<Vector2>();
  }

}
