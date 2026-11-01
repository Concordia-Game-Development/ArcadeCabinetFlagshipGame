using UnityEngine;

[CreateAssetMenu(fileName = "AttackRangedMain", menuName = "Attacks/Ranged Main")]
public class AttackRangedMain: ScriptableObject
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileForce;

   

    public void Attack(attackSpeed)
    {
        //matching the projectileForce to the attack speed
        projectileForce = attackSpeed;
        //spawing the projectiles
        GameObject projectile = Instantiate(projectilePrefab,firePoint.position, firePoint.rotation);
    }
}
