using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// After a room is cleared: items launch out of the pan, players walk up and each claims exactly one
public class RewardPhase : MonoBehaviour
{
    [SerializeField] private ItemPool _itemPool; // Items that can be offered
    [SerializeField] private RewardItem _rewardItemPrefab; // The plate prefab spawned for each item
    [SerializeField] private int _itemCount = 6;
    [SerializeField] private GameObject _dimOverlay; // Optional dark sprite shown during the phase (sort it below the items)

    [Header("Launch")]
    [SerializeField] private float _launchDuration = 0.6f; // Seconds to fly from the pan to the landing spot
    [SerializeField] private float _launchArcHeight = 2f; // How high the arc goes
    [SerializeField] private float _launchStagger = 0.1f; // Delay between each item popping out

    [Header("Player Colors")]
    [SerializeField] private Color[] _playerColors =
    {
        new Color(0.2f, 0.6f, 1f), // Blue
        new Color(1f, 0.85f, 0.1f), // Yellow
        new Color(0.3f, 0.85f, 0.3f), // Green
        new Color(0.95f, 0.25f, 0.2f) // Red
    };
    [SerializeField] private Color _multiHoverColor = new Color(0.65f, 0.3f, 0.95f); // Purple, 2+ players at one item

    [Header("Testing")]
    [SerializeField] private RewardSpawnPoints _testRoom; // Used by the R key until the room system passes its own
    [SerializeField] private bool _debugStartKey = true;

    private readonly List<RewardItem> _items = new List<RewardItem>(); // Items still on the ground
    private readonly List<PlayerInventory> _players = new List<PlayerInventory>(); // Every player, index decides their color
    private readonly List<PlayerInventory> _playersStillPicking = new List<PlayerInventory>();

    public bool IsActive { get; private set; }
    public Color MultiHoverColor => _multiHoverColor;

    public event Action OnRewardPhaseComplete; // The room system listens to this to unlock the next room

    private void Update()
    {
        if (!IsActive && _debugStartKey && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            StartRewardPhase(_testRoom);
        }
    }

    // Call this when a room is cleared, passing that room's spawn points
    public void StartRewardPhase(RewardSpawnPoints room)
    {
        if (IsActive)
        {
            return;
        }

        if (room == null || room.Pan == null)
        {
            Debug.LogError("RewardPhase needs a room with a pan assigned");
            return;
        }

        int count = Mathf.Min(_itemCount, room.LandingSpots.Count);
        if (count < _itemCount)
        {
            Debug.LogWarning(room.name + " only has " + room.LandingSpots.Count + " landing spots, wanted " + _itemCount);
        }

        List<ItemData> offered = _itemPool.GetRandomItems(count);

        // Every player picks (dead players are revived on room clear)
        _players.Clear();
        _players.AddRange(FindObjectsByType<PlayerInventory>());
        _players.Sort((a, b) => string.Compare(a.name, b.name)); // Stable order so each player keeps the same color (replace with real player numbers later)

        _playersStillPicking.Clear();
        _playersStillPicking.AddRange(_players);

        IsActive = true;

        if (_dimOverlay != null)
        {
            _dimOverlay.SetActive(true);
        }

        for (int i = 0; i < offered.Count; i++)
        {
            RewardItem item = Instantiate(_rewardItemPrefab, room.Pan.position, Quaternion.identity);
            item.Setup(offered[i], this);
            item.Launch(room.Pan.position, room.LandingSpots[i].position, _launchDuration, _launchArcHeight, i * _launchStagger);
            _items.Add(item);
        }

        Debug.Log("Reward phase started: " + _items.Count + " items, " + _playersStillPicking.Count + " players picking");

        if (_playersStillPicking.Count == 0 || _items.Count == 0)
        {
            EndRewardPhase();
        }
    }

    public bool IsStillPicking(PlayerInventory player)
    {
        return _playersStillPicking.Contains(player);
    }

    public Color GetPlayerColor(PlayerInventory player)
    {
        int index = _players.IndexOf(player);
        if (index < 0 || _playerColors.Length == 0)
        {
            return Color.white;
        }

        return _playerColors[index % _playerColors.Length];
    }

    // A player presses interact at an item. Fails if another player who still needs to pick is at the same item
    public bool TryClaim(PlayerInventory player, RewardItem item)
    {
        if (!IsActive || !_items.Contains(item) || !IsStillPicking(player))
        {
            return false;
        }

        if (!item.CanBePickedBy(player))
        {
            return false;
        }

        if (!player.TryAdd(item.ItemData))
        {
            return false;
        }

        Debug.Log(player.name + " picked " + item.ItemData.itemName);

        _playersStillPicking.Remove(player);
        _items.Remove(item);
        Destroy(item.gameObject);

        if (_playersStillPicking.Count == 0 || _items.Count == 0)
        {
            EndRewardPhase();
        }

        return true;
    }

    // Clears leftover items and tells listeners the phase is over
    private void EndRewardPhase()
    {
        foreach (RewardItem item in _items)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }

        _items.Clear();
        IsActive = false;

        if (_dimOverlay != null)
        {
            _dimOverlay.SetActive(false);
        }

        Debug.Log("Reward phase complete");
        OnRewardPhaseComplete?.Invoke();
    }
}