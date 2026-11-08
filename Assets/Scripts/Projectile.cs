using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _damage;
    private float _speed;
    private Vector2Int _direction;

    private Transform _projectileTransform;

    // sets the projectile's information when it is created
    public void createProjectile(Vector2Int direction, float speed, float damage)
    {
        _damage = damage;
        _speed = speed;
        _direction = direction;
        _projectileTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        //makes sure projectile has been made with create projectile function
        if (_projectileTransform != null)
        {
            moveProjectile();
        }
    }

    //moves the projectile
    public void moveProjectile()
    {
        //removes vector magnitude (length) to keep only direction
        Vector3 moveDirection = new Vector2(_direction.x, _direction.y).normalized;
        _projectileTransform.position += moveDirection * _speed * Time.deltaTime;
    }
}
