using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _damage;
    private float _speed;
    private Vector2Int _direction;

    private Transform _projectileTransform;

    //where the projectile is first instantiated
    private Vector2 _spawnPoint;

    //TO DO!!!!!!!!!!!! THIS NEEDS TO BE OBTAINED FROM PLAYER STATS
    [SerializeField] private float _range;

    // sets the projectile's information when it is created
    public void CreateProjectile(Vector2Int direction, float speed, float damage)
    {
        _damage = damage;
        _speed = speed;
        _direction = direction;
        _spawnPoint = GetComponent<Transform>().position;
        _projectileTransform = GetComponent<Transform>();

    }

    private void Update()
    {
        //makes sure projectile has been made with create projectile function
        if (_projectileTransform != null)
        {
            MoveProjectile();
        }
    }

    //moves the projectile
    public void MoveProjectile()
    {
        //removes vector magnitude (length) to keep only direction
        Vector3 moveDirection = new Vector2(_direction.x, _direction.y).normalized;
        _projectileTransform.position += moveDirection * _speed * Time.deltaTime;

        CheckRange();
    }

    //damage getter
    public float GetDamage()
    {
        return _damage;
    }

    //destroys self
    public void Die()
    {
        Destroy(gameObject);
    }

    // kills any projectiles that get too far from their spawn point
    public void CheckRange()
    {

        //distance from spawn on the x axis
        float distanceX = Mathf.Abs(_projectileTransform.position.x - _spawnPoint.x);
        //distance from spawn on the y axis
        float distanceY = Mathf.Abs(_projectileTransform.position.y - _spawnPoint.y);

        //distance from spawn
        float distance = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY);

        Debug.Log(distance);
        // if outside the range, die
        if (distance > _range)
        {
            Die();
        }
    }
}
