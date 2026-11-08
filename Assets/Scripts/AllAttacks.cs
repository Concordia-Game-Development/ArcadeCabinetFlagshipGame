using System.Collections.Generic;
using UnityEngine;

/*
 * EVERY SINGLE POSSIBLE ATTACK
 * This script holds the attack functions for all classes
 * The Character scriptable will call the corresponding attack
 * A C# empty script is only accessible through other scripts
 * (so we will access this through each character script)
 * 
 */

public class AllAttacks
{
    public void ExecutePrimaryAttack(CharacterClass id, Dictionary<string, object> attackOptions)
    {
        switch (id)
        {
            case CharacterClass.Healer:
                {
                    HealerPrimary(attackOptions);
                    break;
                }
            case CharacterClass.Tank:
                {
                    TankPrimary(attackOptions);
                    break;
                }
            case CharacterClass.Melee:
                {
                    MeleePrimary(attackOptions);
                    break;
                }
            case CharacterClass.Range:
                {
                    RangePrimary(attackOptions);
                    break;
                }
            case CharacterClass.Test:
                {
                    RangePrimary(attackOptions);
                    break;
                }
        }
    }

    public void ExecuteSecondaryAttack(CharacterClass id, Dictionary<string, object> attackOptions)
    {
        switch (id)
        {
            case CharacterClass.Healer:
                {
                    HealerSecondary(attackOptions);
                    break;
                }
            case CharacterClass.Tank:
                {
                    TankSecondary(attackOptions);
                    break;
                }
            case CharacterClass.Melee:
                {
                    MeleeSecondary(attackOptions);
                    break;
                }
            case CharacterClass.Range:
                {
                    RangeSecondary(attackOptions);
                    break;
                }
            case CharacterClass.Test:
                {
                    TankSecondary(attackOptions);
                    break;
                }
        }

    }

    //----------- HEALER ATTACKS ----------------//
    public void HealerPrimary(Dictionary<string, object> attackOptions)
    {
        Debug.Log("HealerPrimary");
    }

    public void HealerSecondary(Dictionary<string, object> attackOptions)
    {
        Debug.Log("HealerSecondary");
    }

    //----------- TANK ATTACKS ----------------//
    public void TankPrimary(Dictionary<string, object> attackOptions)
    {
        Debug.Log("TankPrimary");
    }

    public void TankSecondary(Dictionary<string, object> attackOptions)
    {
        Debug.Log("TankSecondary");
    }

    //----------- MELEE ATTACKS ----------------//
    public void MeleePrimary(Dictionary<string, object> attackOptions)
    {
        Debug.Log("MeleePrimary");
    }

    public void MeleeSecondary(Dictionary<string, object> attackOptions)
    {
        Debug.Log("MeleeSecondary");
    }

    //----------- RANGE ATTACKS ----------------//

    //takes a dictionary containing the shooter's information
    public void RangePrimary(Dictionary<string, object> attackOptions)
    {
        Debug.Log("RangePrimary");
        //where projectile is birthed
        Debug.Log(attackOptions["position"]);
        Vector2 position = (Vector2)attackOptions["position"];
        //spawns in a projectile prefab 
        GameObject projectile = Object.Instantiate((GameObject)attackOptions["projectilePrefab"], position, Quaternion.identity);
        projectile.GetComponent<Projectile>().createProjectile((Vector2Int)attackOptions["direction"], (float)attackOptions["speed"], (float)attackOptions["damage"]);
    }

    public void RangeSecondary(Dictionary<string, object> attackOptions)
    {
        Debug.Log("RangeSecondary");
    }
}
