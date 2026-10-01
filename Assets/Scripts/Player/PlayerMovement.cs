using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
  private Rigidbody2D _playerRigidbody;

  //called when the scene is first initialized
  private void Awake()
  {
    //gets the rigidbody from the player
    _playerRigidbody = GetComponent<Rigidbody2D>();
  }
  
}
