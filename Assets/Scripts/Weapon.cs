using UnityEngine;
using PrimeTween;
using Unity.VisualScripting;

public class Weapon : MonoBehaviour
{

    private float _damage;
    private float _haste;

    private Vector2Int _direction;

    private Transform _attackerTransform;

    private Transform _weaponTransform;

    public void CreateWeapon(Vector2Int direction, float haste, float damage, object attacker)
    {
        _damage = damage;
        _haste = haste;
        _direction = direction;
        _weaponTransform = GetComponent<Transform>();
        Debug.Log(direction);
        //get into start position (45 degrees counterclockwise)
        _weaponTransform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 45);
        _attackerTransform = attacker.ConvertTo<GameObject>().GetComponent<Transform>();

        SwipeWeapon();
    }

    void Update()
    {
        if (_weaponTransform != null)
        {
            //make sure weapon follows the player
            _weaponTransform.position = _attackerTransform.position;
        }
    }



    //swipes the weapon in an arc in front of the player
    public void SwipeWeapon()
    {


        //swipe the weapon
        //45 degrees counterclockwise
        //Vector3 rotatedDir = Quaternion.Euler(0, 45, 0) * originalDir
        //_weaponTransform.rotation = _direction;
        Debug.Log("Weapon rotation: " + _weaponTransform.rotation);
        Tween.Rotation(_weaponTransform, endValue: Quaternion.Euler(0, 0, Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 135f), duration: _haste, ease: Ease.Linear).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    //damage getter
    public float GetDamage()
    {
        return _damage;
    }
}
