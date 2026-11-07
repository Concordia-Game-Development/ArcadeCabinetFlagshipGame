using UnityEngine;
//logs every stat change to the Console so you can see items working
[RequireComponent(typeof(Stats))]
public class StatsDebugLogger : MonoBehaviour
{
    private Stats _stats;
    private void Awake()
    {
        _stats = GetComponent<Stats>();
    }

    private void OnEnable() {
        _stats.OnStatChanged += LogChange;
    }

    private void OnDisable()
    {
        _stats.OnStatChanged -= LogChange;
    }

    private void LogChange(StatType statType, float newValue)
    {Debug.Log(name + " " + statType + " is now " + newValue);}
}