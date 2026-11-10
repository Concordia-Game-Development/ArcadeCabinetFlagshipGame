using UnityEngine;

/*
*This is the placeholder enemy's script
* It was made to receive attacks
*
*/

public class PlaceholderEnemy : MonoBehaviour
{

    [SerializeField] private float health;

    //If something hits the enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("touched");
        if (collision.gameObject.GetComponent<Projectile>() != null)
        {
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            GetHurt(projectile.GetDamage());
            //destroy the projectile
            projectile.Die();
        }
    }

    //reduces the enemy's health
    private void GetHurt(float damage)
    {

        Debug.Log("ow");

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    //enemy gets destroyed
    private void Die()
    {
        Destroy(gameObject);
    }



}
