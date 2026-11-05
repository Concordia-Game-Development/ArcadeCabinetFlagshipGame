using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacks : MonoBehaviour
{

    public InputSystem_Actions InputActions {get; private set;}

    private Player _player;


    private void Awake()
    {
        InputActions = GameInput.Instance.InputActions;
        
        _player = GetComponent<Player>();
    }
    
    //Primary Attack! projectile
    private void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        _player.getCharacter().primaryAttack();
    }

    //Secondary Attack! wide sweep
    private void OnSecondaryAttack(InputAction.CallbackContext context)
    {
        _player.getCharacter().secondaryAttack();
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


    

}
