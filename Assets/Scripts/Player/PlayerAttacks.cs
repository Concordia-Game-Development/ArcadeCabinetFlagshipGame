using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacks : MonoBehaviour
{

    public InputSystem_Actions InputActions {get; private set;}

    private void Awake()
    {
        InputActions = GameInput.Instance.InputActions;
    }

   //runs when player becomes enabled 
  private void OnEnable()
  {
    InputActions.Player.Enable();
    InputActions.Player.AttackPrimary.performed += OnAttackPrimary;
    InputActions.Player.AttackSecondary.performed += OnAttackSecondary;
  }
  //runs when player becomes disabled (death?)
  private void OnDisable()
  {
    InputActions.Player.Disable();
    InputActions.Player.AttackPrimary.performed -= OnAttackPrimary;
    InputActions.Player.AttackSecondary.performed -= OnAttackSecondary;
  }


    //Primary Attack! projectile
    private void OnAttackPrimary(InputAction.CallbackContext context)
    {

    }

    //Secondary Attack! wide sweep
    private void OnAttackSecondary(InputAction.CallbackContext context)
    {

    }

}
