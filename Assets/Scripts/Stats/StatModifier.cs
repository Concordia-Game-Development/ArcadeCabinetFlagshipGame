// Describes one bonus applied to a stat, tagged with its source so it can be removed later
public class StatModifier
{
	private readonly StatType _statType; // Which stat this modifier changes
	private readonly float _value; // Flat amount, or a fraction if percent (0.2 = +20%)
	private readonly bool _isPercent; // True for a percent bonus, false for a flat bonus
	private readonly object _source; // What applied this modifier (usually an item), used for removal

	// Creates a new modifier
	public StatModifier(StatType statType, float value, bool isPercent, object source)
	{
		_statType = statType;
		_value = value;
		_isPercent = isPercent;
		_source = source;
	}

	public StatType StatType => _statType;
	public float Value => _value;
	public bool IsPercent => _isPercent;
	public object Source => _source;
}