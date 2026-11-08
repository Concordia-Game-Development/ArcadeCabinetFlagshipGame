using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * STORES CHARACTER CLASS INFORMATION
 * This is the parent class for all characters
 * A Scriptable objects allows us to make copies of it and modify the specific copies
 * It is mainly used to store data
 * Instances are created in the unity editor (not through code)
 * We don't want any instances of this one tho
 * We will be making 4 children of this one, one for each character class. (they will inherit)
 * The children will get instances
 * 
 */

[CreateAssetMenu(fileName = "Characters", menuName = "Scriptable Objects/Characters")]
public class Characters : ScriptableObject
{
    //the character class will hold a scriptable object of its stats
    [SerializeField] private ClassStatsData classStats;
    [SerializeField] private CharacterClass characterClass;

    [SerializeField] private GameObject projectilePrefab;
    private AllAttacks allAttacks;

    //Constructor
    public Characters()
    {
        allAttacks = new AllAttacks();
    }

    //getter for classStats
    public ClassStatsData GetClassStats()
    {
        return classStats;
    }

    //primary attack
    public void PrimaryAttack(Dictionary<string, object> attackOptions)
    {
        Debug.Log("primary attack");

        //add speed and damage to the attack options dictionary
        attackOptions["speed"] = classStats.GetBaseValue(StatType.Haste);
        attackOptions["damage"] = classStats.GetBaseValue(StatType.Damage);
        attackOptions["projectilePrefab"] = projectilePrefab;

        allAttacks.ExecutePrimaryAttack(characterClass, attackOptions);
    }

    //secondary attack
    public void SecondaryAttack(Dictionary<string, object> attackOptions)
    {
        Debug.Log("secondary attack");
        allAttacks.ExecuteSecondaryAttack(characterClass, attackOptions);
    }
}
