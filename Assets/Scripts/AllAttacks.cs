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
    public void executePrimaryAttack(CharacterClass id)
    {
        switch (id)
        {
            case CharacterClass.Healer:
            {
                healerPrimary();
                break;
            }
            case CharacterClass.Tank:
            {
                tankPrimary();
                break;
            }
            case CharacterClass.Melee:
            {
                meleePrimary();
                break;
            }
            case CharacterClass.Range:
            {
                rangePrimary();
                break;
            }
            case CharacterClass.Test:
            {
                rangePrimary();
                break;
            }
        }
    }

    public void executeSecondaryAttack(CharacterClass id)
    {
        switch (id)
        {
            case CharacterClass.Healer:
            {
                healerSecondary();
                break;
            }
            case CharacterClass.Tank:
            {
                tankSecondary();
                break;
            }
            case CharacterClass.Melee:
            {
                meleeSecondary();
                break;
            }
            case CharacterClass.Range:
            {
                rangeSecondary();
                break;
            }
            case CharacterClass.Test:
            {
                tankSecondary();
                break;
            }
        }

    }

    //----------- HEALER ATTACKS ----------------//
    public void healerPrimary()
    {
        Debug.Log("healerPrimary");
    }

    public void healerSecondary()
    {
        Debug.Log("healerSecondary");
    }

    //----------- TANK ATTACKS ----------------//
    public void tankPrimary()
    {
        Debug.Log("tankPrimary");
    }

    public void tankSecondary()
    {
        Debug.Log("tankSecondary");
    }
    
    //----------- MELEE ATTACKS ----------------//
    public void meleePrimary()
    {
        Debug.Log("meleePrimary");
    }

    public void meleeSecondary()
    {
        Debug.Log("meleeSecondary");
    }

    //----------- RANGE ATTACKS ----------------//
    
    public void rangePrimary()
    {
        Debug.Log("rangePrimary");
    }

    public void rangeSecondary()
    {
        Debug.Log("rangeSecondary");
    }
}
