using System.Collections.Generic;
using UnityEngine;

// Put one in every room: where the pan is and where reward items land
public class RewardSpawnPoints : MonoBehaviour
{
    [SerializeField] private Transform _pan; // Items launch from here
    [SerializeField] private List<Transform> _landingSpots = new List<Transform>(); // One per item, keep them clear of doorways

    public Transform Pan => _pan;
    public IReadOnlyList<Transform> LandingSpots => _landingSpots;

    // Shows the pan and landing spots in the Scene view so they're easy to place
    private void OnDrawGizmos()
    {
        if (_pan == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_pan.position, 0.5f);

        foreach (Transform spot in _landingSpots)
        {
            if (spot == null)
            {
                continue;
            }

            Gizmos.DrawLine(_pan.position, spot.position);
            Gizmos.DrawWireSphere(spot.position, 0.4f);
        }
    }
}