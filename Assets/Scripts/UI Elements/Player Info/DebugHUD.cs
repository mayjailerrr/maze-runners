using UnityEngine;
using TMPro;

public class DebugHUD : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text dialogueText; 
    public GameObject character; 

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string formattedMessage = "";

        switch (type)
        {
            case LogType.Log:
                formattedMessage = $"<color=white>{logString}</color>";
                break;
            case LogType.Warning:
                formattedMessage = $"<color=yellow>{logString}</color>";
                break;
            case LogType.Error:
            case LogType.Exception:
                formattedMessage = $"<color=red>{logString}</color>";
                break;
        }

        ShowMessage(formattedMessage);
    }

    private void ShowMessage(string message)
    {
        dialogueText.text = message;

        character.SetActive(true);
    }
}
