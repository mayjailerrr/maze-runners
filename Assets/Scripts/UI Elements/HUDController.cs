using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.Events;

public class HUDController : MonoBehaviour
{
    [Header("Collectibles")]
    public Transform collectibleContainer;
    public GameObject collectibleIconPrefab;

    [Header("Health")]
    public Image[] healthIcons; 

    [Header("Ability Icon")]
    public Image abilityIcon; 
    public Color brightColor = Color.white;
    public Color dimColor = new Color(1, 1, 1, 0.5f); 

    [Header("Speed")]
    public TMP_Text speedText;

    [Header("Cooldown")]
    public TMP_Text cooldownText;

    [Header("Ability Description")]
    public TMP_Text abilityDescriptionText;

    private Player currentPlayer;
    private Piece selectedPiece;

    public void UpdateHUD(Player player, Piece piece)
    {
        if (selectedPiece != null)
        {
            selectedPiece.OnAbilityStateChanged -= UpdateAbilityIcon;
            selectedPiece.OnMovesChanged -= UpdateSpeed;
            selectedPiece.OnHealthChanged -= UpdateHealth;
        }

        currentPlayer = player;
        selectedPiece = piece;

        if (selectedPiece != null)
        {
            selectedPiece.OnAbilityStateChanged += UpdateAbilityIcon;
            selectedPiece.OnMovesChanged += UpdateSpeed;
            selectedPiece.OnHealthChanged += UpdateHealth;
        }

        UpdateCollectibles(player);
        UpdateHealth();
        UpdateAbilityIcon();
        UpdateSpeed();
        UpdateCooldown();
        UpdateAbilityDescription();
    }

    private void UpdateCollectibles(Player player)
    {
        foreach (Transform child in collectibleContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var collectible in player.CollectedObjects)
        {
            GameObject collectibleGO = CollectibleViewManager.Instance.CreateCollectibleVisual(collectible);
            if (collectibleGO != null)
            {
                collectibleGO.transform.SetParent(collectibleContainer, false);
                collectibleGO.transform.localScale = Vector3.one;
            }
        }
    }

    private void UpdateHealth()
    {
        if (selectedPiece == null || healthIcons == null || healthIcons.Length == 0)
        {
            Debug.LogWarning("No piece selected or health icons not assigned.");
            return;
        }

        int health = Mathf.Clamp(selectedPiece.Health, 0, healthIcons.Length); 

        for (int i = 0; i < healthIcons.Length; i++)
        {
            healthIcons[i].enabled = i < health; 
        }
    }

    private void UpdateAbilityIcon()
    {
        if (selectedPiece == null || abilityIcon == null)
        {
            Debug.LogWarning("No piece selected or ability icon not assigned.");
            return;
        }

        abilityIcon.color = selectedPiece.CanUseAbility ? brightColor : dimColor;
    }

    private void UpdateSpeed()
    {
        if (selectedPiece == null || speedText == null)
        {
            Debug.LogWarning("No piece selected or speedText not assigned.");
            return;
        }

        speedText.text = $"Moves: {selectedPiece.MovesRemaining}";
    }

    private void UpdateCooldown()
    {
        if (selectedPiece == null || cooldownText == null)
        {
            Debug.LogWarning("No piece selected or cooldownText not assigned.");
            return;
        }

        cooldownText.text = $"Cooldown: {selectedPiece.Cooldown}";
    }

     private void UpdateAbilityDescription()
    {
        if (selectedPiece == null || abilityDescriptionText == null)
        {
            Debug.LogWarning("No piece selected or ability description text not assigned.");
            return;
        }

        abilityDescriptionText.text = selectedPiece.Ability != null ? $"Ability: {selectedPiece.Ability.Description}" : "No Ability";
    }
}
