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
            Debug.LogError("The button is not assigned.");
        }
    }

    public void OnPassButtonPressed()
    {
        if (turnManager == null)
        {
            Debug.LogError("TurnManager is not assigned.");
            return;
        }
        
        turnManager.NextTurn();
    }
}
