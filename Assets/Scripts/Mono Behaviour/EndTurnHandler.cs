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
            endTurnButton.onClick.RemoveAllListeners();
            endTurnButton.onClick.AddListener(OnPassButtonPressed);
        }
        else
        {
            Debug.LogError("The button is not assigned.");
        }
    }

    private bool isButtonLocked = false;

public void OnPassButtonPressed()
{
    if (isButtonLocked)
    {
        Debug.LogWarning("Button press ignored. Already processing turn.");
        return;
    }

    isButtonLocked = true;

    if (turnManager != null)
    {
        turnManager.NextTurn();
    }
    else
    {
        Debug.LogError("TurnManager is not assigned.");
    }

    isButtonLocked = false;
}


}
