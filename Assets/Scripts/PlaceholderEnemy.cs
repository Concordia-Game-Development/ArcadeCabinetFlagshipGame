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
        if (collision.gameObject.GetComponent<Projectile>() != null)
        {
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            GetHurt(projectile.GetDamage());
            //destroy the projectile
            projectile.Die();
        }
        else if (collision.gameObject.GetComponent<Weapon>() != null)
        {
            Weapon weapon = collision.gameObject.GetComponent<Weapon>();
            GetHurt(weapon.GetDamage());
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
