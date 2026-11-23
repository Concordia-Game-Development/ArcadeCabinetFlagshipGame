using UnityEngine;

public class GameLoop : MonoBehaviour
{
    // Pulled the contents of this region straight from Stack Exchange. Might or might not work yet (imma test it).
    #region Singleton Behavior
    // The GameLoop instance serving as a singleton.
    public static GameLoop loop
    {
        get
        {
            return _instance;
        }
    }

    private static GameLoop _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion


    // The "state machine" part of the GameLoop class.
    #region Game State
    public GameState state = GameState.Null;
    #endregion


    // The "manager" part of the GameLoop class.
    #region GameManager
    // TODO : Players references.
    #endregion
}
