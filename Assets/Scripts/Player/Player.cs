using UnityEngine;

/*
*Holds the Character Scriptable object
 * That's it.
*
*/

public class Player : MonoBehaviour
{
    [SerializeField] private Characters character;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Characters getCharacter()
    {
        return character;
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
