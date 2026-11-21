using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// One reward on a plate: flies out of the pan, shows its info card when a player is near
// Needs a trigger Collider2D (its size is the pickup range)
[RequireComponent(typeof(Collider2D))]
public class RewardItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _iconRenderer; // The item icon drawn on the plate
    [SerializeField] private SpriteRenderer _outline; // Plate-shaped sprite slightly bigger than the plate, behind it
    [SerializeField] private RewardCard _infoCard; // World-space card with name and description

    private readonly List<PlayerInventory> _playersInRange = new List<PlayerInventory>();
    private RewardPhase _rewardPhase;
    private ItemData _itemData;
    private bool _hasLanded;

    public ItemData ItemData => _itemData;

    public void Setup(ItemData data, RewardPhase rewardPhase)
    {
        _itemData = data;
        _rewardPhase = rewardPhase;

        if (_iconRenderer != null)
        {
            _iconRenderer.sprite = data.itemIcon;
        }

        if (_infoCard != null)
        {
            _infoCard.Setup(data);
            _infoCard.gameObject.SetActive(false);
        }

        if (_outline != null)
        {
            _outline.enabled = false;
        }
    }

    public void Launch(Vector3 from, Vector3 to, float duration, float arcHeight, float delay)
    {
        StartCoroutine(LaunchRoutine(from, to, duration, arcHeight, delay));
    }

    // Moves in a straight line and adds a sine bump on top for the arc
    // Unscaled time so it still plays if the game gets paused during the reward phase
    private IEnumerator LaunchRoutine(Vector3 from, Vector3 to, float duration, float arcHeight, float delay)
    {
        transform.position = from;

        float waited = 0f;
        while (waited < delay)
        {
            waited += Time.unscaledDeltaTime;
            yield return null;
        }

        float progress = 0f;
        while (progress < 1f)
        {
            progress = Mathf.Clamp01(progress + Time.unscaledDeltaTime / Mathf.Max(duration, 0.01f));

            Vector3 position = Vector3.Lerp(from, to, progress);
            position.y += Mathf.Sin(progress * Mathf.PI) * arcHeight;
            transform.position = position;

            yield return null;
        }

        transform.position = to;
        _hasLanded = true;
    }

    // Only the player standing here alone (among players still picking) can take it
    public bool CanBePickedBy(PlayerInventory player)
    {
        if (!_hasLanded || !_playersInRange.Contains(player))
        {
            return false;
        }

        foreach (PlayerInventory other in _playersInRange)
        {
            if (other != player && other != null && _rewardPhase.IsStillPicking(other))
            {
                return false;
            }
        }

        return true;
    }

    public bool TryPick(PlayerInventory player)
    {
        return _rewardPhase != null && _rewardPhase.TryClaim(player, this);
    }

    // Outline: nobody picking nearby = off, one = their color, two or more = purple (blocked)
    private void Update()
    {
        if (_rewardPhase == null)
        {
            return;
        }

        _playersInRange.RemoveAll(player => player == null);

        int pickersNearby = 0;
        Color pickerColor = Color.white;

        foreach (PlayerInventory player in _playersInRange)
        {
            if (_rewardPhase.IsStillPicking(player))
            {
                pickersNearby++;
                pickerColor = _rewardPhase.GetPlayerColor(player);
            }
        }

        if (_outline != null)
        {
            _outline.enabled = _hasLanded && pickersNearby > 0;
            _outline.color = pickersNearby == 1 ? pickerColor : _rewardPhase.MultiHoverColor;
        }

        if (_infoCard != null)
        {
            _infoCard.gameObject.SetActive(_hasLanded && _playersInRange.Count > 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInventory player = other.GetComponentInParent<PlayerInventory>();
        if (player != null && !_playersInRange.Contains(player))
        {
            _playersInRange.Add(player);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerInventory player = other.GetComponentInParent<PlayerInventory>();
        if (player != null)
        {
            _playersInRange.Remove(player);
        }
    }
}