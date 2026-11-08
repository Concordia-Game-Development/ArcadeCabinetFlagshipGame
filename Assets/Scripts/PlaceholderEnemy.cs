using UnityEngine;

/*
*This is the placeholder enemy's script
* It was made to receive attacks
*
*/

public class PlaceholderEnemy : MonoBehaviour
{

    [SerializeField] private int health;

    //If something hits the enemy
    private void OnCollision2D(Collider2D collider)
    {
        // if (collider.GetComponent<Projectile>() != null)
        // {
        //     GameObject projectile = collider.GetComponent<GameObject>();

        // }


    }



}
