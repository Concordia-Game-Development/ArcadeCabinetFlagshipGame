using UnityEngine;

[CreateAssetMenu(fileName = "NewClassStats", menuName = "Stats/Class Stats")]
public class ClassStatsData : ScriptableObject
{
	[SerializeField] private float _damage = 10f; // Base damage of main and secondary attacks
	[SerializeField] private float _health = 100f; // Base max health
	[SerializeField] private float _haste = 1f; // Base attack speed and ability cooldown rate
	[SerializeField] private float _speed = 5f; // Base movement speed
	[SerializeField] private float _range = 3f; // Base range of AOE attacks

	// Returns the base value for the requested stat
	public float GetBaseValue(StatType statType)
	{
		switch (statType)
		{
			case StatType.Damage:
				return _damage;
			case StatType.Health:
				return _health;
			case StatType.Haste:
				return _haste;
			case StatType.Speed:
				return _speed;
			case StatType.Range:
				return _range;
			default:
				return 0f;
		}
	}
}