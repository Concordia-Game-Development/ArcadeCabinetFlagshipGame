using UnityEngine;
using PrimeTween;
using Unity.VisualScripting;

public class Weapon : MonoBehaviour
{

    private float _damage;
    private float _speed;

    private Vector2Int _direction;

    private Transform _attackerTransform;

    private Transform _weaponTransform;

    public void CreateWeapon(Vector2Int direction, float speed, float damage, object attacker)
    {
        _damage = damage;
        _speed = speed;
        _direction = direction;
        _weaponTransform = GetComponent<Transform>();
        //get into start position
        Tween.Rotation(_weaponTransform, endValue: Quaternion.Euler(0, 90, 0), duration: _speed / 10f);
        _attackerTransform = attacker.ConvertTo<GameObject>().GetComponent<Transform>();
    }


    private void Update()
    {
        //makes sure weapon has been made
        if (_attackerTransform != null)
        {
            SwipeWeapon();
        }
    }

    //swipes the weapon in an arc in front of the player
    public void SwipeWeapon()
    {
        //make sure weapon follows the player
        _weaponTransform.position = _attackerTransform.position;



        //swipe the weapon
        //45 degrees counterclockwise
        //Vector3 rotatedDir = Quaternion.Euler(0, 45, 0) * originalDir
        //_weaponTransform.rotation = _direction;
        Tween.Rotation(_weaponTransform, endValue: Quaternion.Euler(0, 45, 0), duration: _speed);
    }

    //damage getter
    public float GetDamage()
    {
        return _damage;
    }
}
