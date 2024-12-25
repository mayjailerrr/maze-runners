using UnityEngine;
using UnityEngine.UI;

public class EndTurnHandler : MonoBehaviour
{
    public Button endTurnButton;
    private TurnManager turnManager;

    public void Initialize(TurnManager turnManager)
    {
        this.turnManager = turnManager;

        if (endTurnButton != null)
        {
            endTurnButton.onClick.AddListener(OnPassButtonPressed);
        }
        else
        {
            Debug.LogError("El botón de fin de turno no está asignado.");
        }
    }

    public void OnPassButtonPressed()
    {
        if (turnManager == null)
        {
            Debug.LogError("TurnManager is not assigned.");
            return;
        }

        var currentPlayer = turnManager.GetCurrentPlayer();
        turnManager.NextTurn();
    }
}
