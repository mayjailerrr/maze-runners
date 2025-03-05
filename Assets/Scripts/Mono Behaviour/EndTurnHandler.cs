using UnityEngine;

public class EndTurnHandler : MonoBehaviour
{
    private TurnManager turnManager;
    private bool isButtonLocked = false;

    public void Initialize(TurnManager turnManager)
    {
        this.turnManager = turnManager;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OnPassButtonPressed();
        }
    }

    private void OnPassButtonPressed()
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
            Debug.LogWarning("Cannot skip your turn right now.");
        }

        isButtonLocked = false;
    }

}
