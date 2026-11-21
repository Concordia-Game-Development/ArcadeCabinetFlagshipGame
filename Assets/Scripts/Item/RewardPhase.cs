using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum AutoPickMode
{
    Random, // Timer runs out: give a random unclaimed item
    First // Timer runs out: give the first unclaimed item
}

// After a room is cleared: pauses the game, offers items on the reward screen, each player claims exactly one
public class RewardPhase : MonoBehaviour
{
    [SerializeField] private ItemPool _itemPool; // Items that can be offered
    [SerializeField] private RewardScreen _rewardScreen; // The popup UI
    [SerializeField] private int _itemCount = 6;

    [Header("Pick Timer")]
    [SerializeField] private float _pickTimeLimit = 20f; //seconds before remaining players get an item picked for them, 0 = no timer
    [SerializeField] private AutoPickMode _autoPickMode = AutoPickMode.Random;

    [Header("Testing")]
    [SerializeField] private bool _debugStartKey = true; //press R to start, until the room system calls StartRewardPhase

    private readonly List<ItemData> _offeredItems = new List<ItemData>();
    private readonly List<bool> _claimed = new List<bool>(); // Same order as _offeredItems
    private readonly List<PlayerInventory> _players = new List<PlayerInventory>(); // Every player, index decides their color
    private readonly List<PlayerInventory> _playersStillPicking = new List<PlayerInventory>();
    private float _previousTimeScale = 1f;

    public bool IsActive { get; private set; }
    public float TimeRemaining { get; private set; }
    public bool HasTimer => _pickTimeLimit > 0f;

    public event Action OnRewardPhaseComplete; // The room system listens to this to unlock the next room

    private void Update()
    {
        if (!IsActive){
            if (_debugStartKey && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame){
                StartRewardPhase();
            }
            return;
        }

        //unscaled time because the game is paused while picking
        if (HasTimer){
            TimeRemaining -= Time.unscaledDeltaTime;
            if (TimeRemaining <= 0f){
                AutoPickForRemainingPlayers();
            }
        }
    }

    // Call this when a room is cleared
    public void StartRewardPhase(){
        if (IsActive){
            return;
        }

        _offeredItems.Clear();
        _offeredItems.AddRange(_itemPool.GetRandomItems(_itemCount));

        _claimed.Clear();
        foreach (ItemData item in _offeredItems){
            _claimed.Add(false);
        }

        // Every player picks (dead players are revived on room clear)
        _players.Clear();
        _players.AddRange(FindObjectsByType<PlayerInventory>());
        _players.Sort((a, b) => string.Compare(a.name, b.name)); //stable order so each player keeps the same color (replace with real player numbers later)
        _playersStillPicking.Clear();
        foreach (PlayerInventory player in _players){
            if (player.IsFull){
                Debug.Log(player.name + " has a full inventory, skipping their pick");
                continue;
            }

            _playersStillPicking.Add(player);
        }

        IsActive = true;
        TimeRemaining = _pickTimeLimit;

        _previousTimeScale = Time.timeScale;
        Time.timeScale = 0f; //pause the game

        _rewardScreen.Show(this, _offeredItems, _players);
        Debug.Log("Reward phase started: " + _offeredItems.Count + " items, " + _playersStillPicking.Count + " players picking");

        if (_playersStillPicking.Count == 0 || _offeredItems.Count == 0){
            EndRewardPhase();
        }
    }

    public bool IsStillPicking(PlayerInventory player){
        return _playersStillPicking.Contains(player);
    }

    // A player tries to claim the item at this card index. Returns true if they got it
    public bool TryClaim(PlayerInventory player, int itemIndex){
        if (!IsActive || itemIndex < 0 || itemIndex >= _offeredItems.Count){
            return false;
        }

        if (_claimed[itemIndex] || !_playersStillPicking.Contains(player)){
            return false;
        }

        if (!player.TryAdd(_offeredItems[itemIndex])){
            return false;
        }

        _claimed[itemIndex] = true;
        _playersStillPicking.Remove(player);
        _rewardScreen.OnItemClaimed(itemIndex, player);
        Debug.Log(player.name + " picked " + _offeredItems[itemIndex].itemName);

        if (_playersStillPicking.Count == 0 || !HasUnclaimedItems()){
            EndRewardPhase();
        }

        return true;
    }

    // Timer ran out: give every player who hasn't picked an item
    private void AutoPickForRemainingPlayers(){
        List<PlayerInventory> remaining = new List<PlayerInventory>(_playersStillPicking);

        foreach (PlayerInventory player in remaining){
            int index = ChooseAutoPickIndex();
            if (index < 0)
            {
                break;
            }

            TryClaim(player, index);
        }

        if (IsActive){
            EndRewardPhase(); // Safety net so the game can never get stuck here
        }
    }

    private int ChooseAutoPickIndex()
    {
        List<int> unclaimed = new List<int>();
        for (int i = 0; i < _claimed.Count; i++){
            if (!_claimed[i])
            {
                unclaimed.Add(i);
            }
        }

        if (unclaimed.Count == 0){
            return -1;
        }

        if (_autoPickMode == AutoPickMode.First){
            return unclaimed[0];
        }

        return unclaimed[UnityEngine.Random.Range(0, unclaimed.Count)];
    }

    private bool HasUnclaimedItems(){
        return _claimed.Contains(false);
    }

    // Closes the screen, unpauses and tells listeners the phase is over
    private void EndRewardPhase(){
        IsActive = false;
        Time.timeScale = _previousTimeScale;
        _rewardScreen.Hide();

        Debug.Log("Reward phase complete");
        OnRewardPhaseComplete?.Invoke();
    }
}