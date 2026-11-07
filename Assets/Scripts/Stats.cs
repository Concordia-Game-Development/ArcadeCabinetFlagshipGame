using System;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
	[SerializeField] private ClassStatsData _baseStats; // Starting values for this entity's class, assigned inside the prefab

	private readonly List<StatModifier> _modifiers = new List<StatModifier>(); // Every active modifier from items and buffs

	public event Action<StatType, float> OnStatChanged; // Fired with the stat and its new final value whenever it changes

	// returns the final value of a stat, (base + flat bonuses) * (1 + percent bonuses)
	public float GetStat(StatType statType)
	{
		if (_baseStats == null) // safety check
		{   Debug.LogError("Stats on " + name + " has no base stats assigned.");
			return 0f;}

		float flatTotal = 0f; 
		float percentTotal = 0f;

		foreach (StatModifier modifier in _modifiers)
		{
			if (modifier.StatType != statType)
			{
				continue;
			}

            // sorts each matching modifier into either the flat or percent total, depending on its type 
			if (modifier.IsPercent)
			{
				percentTotal += modifier.Value;
			}
			else
			{
				flatTotal += modifier.Value;
			}
		}

		float finalValue = (_baseStats.GetBaseValue(statType) + flatTotal) * (1f + percentTotal);
		return Mathf.Max(0f, finalValue); //to protect from going negative from a debuff
	}

	// Adds a modifier and notifies listeners that the stat changed
	public void AddModifier(StatModifier modifier)
	{
		_modifiers.Add(modifier);
		OnStatChanged?.Invoke(modifier.StatType, GetStat(modifier.StatType));
	}

	// Removes every modifier that came from the given source (e.g. a dropped item)
	public void RemoveModifiersFromSource(object source)
	{
		for (int i = _modifiers.Count - 1; i >= 0; i--)
		{
			if (_modifiers[i].Source != source)
			{
				continue;
			}

			StatType changedStat = _modifiers[i].StatType;
			_modifiers.RemoveAt(i);
			OnStatChanged?.Invoke(changedStat, GetStat(changedStat)); //if an item gives 2 buffs it removes with one call
		}
	}

	//swaps in a different class's base stats (used when a player picks a class)
	public void SetBaseStats(ClassStatsData newBaseStats)
	{
		_baseStats = newBaseStats;

		foreach (StatType statType in Enum.GetValues(typeof(StatType)))
		{
			OnStatChanged?.Invoke(statType, GetStat(statType));
		}
	}
}