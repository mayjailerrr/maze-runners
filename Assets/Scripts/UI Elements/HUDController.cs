using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

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
    public TMP_Text speedText; // Texto para movimientos restantes

    private Player currentPlayer;
    private Piece selectedPiece;

    public void UpdateHUD(Player player, Piece piece)
    {
        if (selectedPiece != null)
        {
            selectedPiece.OnAbilityStateChanged -= UpdateAbilityIcon; // Desuscribir de la ficha anterior
            selectedPiece.OnMovesChanged -= UpdateSpeed;
        }

        currentPlayer = player;
        selectedPiece = piece;

        if (selectedPiece != null)
        {
            selectedPiece.OnAbilityStateChanged += UpdateAbilityIcon; // Suscribir a la nueva ficha
            selectedPiece.OnMovesChanged += UpdateSpeed;
        }

        UpdateCollectibles(player);
        UpdateHealth(piece);
        UpdateAbilityIcon();
        UpdateSpeed();
    }

    private void UpdateCollectibles(Player player)
    {
        // Itera sobre el HashSet de coleccionables capturados por el jugador
        foreach (var collectible in player.CollectedObjects)
        {
            // Busca si ya existe un objeto visual para este coleccionable en el HUD
            Transform existingCollectible = collectibleContainer.Find(collectible.Name);

            if (existingCollectible == null)
            {
                // Si no existe, crea uno nuevo y añádelo al HUD
                GameObject collectibleGO = Instantiate(collectibleIconPrefab, collectibleContainer);
                collectibleGO.name = collectible.Name;

                // Opcional: Personaliza el aspecto del coleccionable
                var textComponent = collectibleGO.GetComponentInChildren<Text>();
                if (textComponent != null)
                {
                    textComponent.text = collectible.Name; // Muestra el nombre como texto
                }

                collectibleGO.transform.SetAsLastSibling(); // Asegura el orden visual
            }
        }

        // Elimina del HUD los coleccionables que ya no están en el HashSet
        foreach (Transform child in collectibleContainer)
        {
            if (!player.CollectedObjects.Any(c => c.Name == child.name))
            {
                Destroy(child.gameObject);
            }
        }
    }


    private void UpdateHealth(Piece piece)
    {
        int health = piece.Health;

        for (int i = 0; i < healthIcons.Length; i++)
        {
            healthIcons[i].enabled = i < health; // Activa o desactiva según la salud restante
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
}
