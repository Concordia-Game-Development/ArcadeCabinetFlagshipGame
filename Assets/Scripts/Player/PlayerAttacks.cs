using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacks : MonoBehaviour
{

  public InputSystem_Actions InputActions { get; private set; }

  private Player _player;

  private Dictionary<string, object> attackOptions;


  private void Awake()
  {
    InputActions = GameInput.Instance.InputActions;

    _player = GetComponent<Player>();

    attackOptions = new Dictionary<string, object>();

    //getting the whole player/enemy 
    attackOptions["attacker"] = _player;

  }

  //Primary Attack! projectile
  private void OnPrimaryAttack(InputAction.CallbackContext context)
  {
    UpdateAttackOptions();
    _player.GetCharacter().PrimaryAttack(attackOptions);
  }

  //Secondary Attack! wide sweep
  private void OnSecondaryAttack(InputAction.CallbackContext context)
  {
    UpdateAttackOptions();
    _player.GetCharacter().SecondaryAttack(attackOptions);
  }

  //runs when player becomes enabled 
  private void OnEnable()
  {
    InputActions.Player.Enable();
    InputActions.Player.AttackPrimary.performed += OnPrimaryAttack;
    InputActions.Player.AttackSecondary.performed += OnSecondaryAttack;
  }
  //runs when player becomes disabled (death?)
  private void OnDisable()
  {
    InputActions.Player.Disable();
    InputActions.Player.AttackPrimary.performed -= OnPrimaryAttack;
    InputActions.Player.AttackSecondary.performed -= OnSecondaryAttack;
  }

  private void UpdateAttackOptions()
  {

    attackOptions["position"] = new Vector2(_player.transform.position.x, _player.transform.position.y)
    ;
    //getting the last direction the player has faced
    attackOptions["direction"] = _player.GetComponent<PlayerMovement>().Direction;

  }


}
