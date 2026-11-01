using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    // This class is a singleton, so there's only one instance in the entire game
    public static GameInput Instance;

    //getting the game's input system

    public InputSystem_Actions InputActions{ get; private set; } = new InputSystem_Actions();

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}
