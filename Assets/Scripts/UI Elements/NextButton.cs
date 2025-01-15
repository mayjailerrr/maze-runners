using UnityEngine;
using UnityEngine.SceneManagement;
using MazeRunners;
using UnityEngine.UI;

public class NextButton : MonoBehaviour
{
    public string mazeSceneName = "Game"; 
    public Button nextButton; 
    public Canvas canvasComponent;

    private void Update()
    {
        if (nextButton != null)
        {
            nextButton.interactable = GameManager.Instance.CanStartGame();
        }
    }

    void Start()
    {
        nextButton.onClick.AddListener(OnNextButtonPressed);
    }

    void OnNextButtonPressed()
    {
        if (canvasComponent != null)
        {
            canvasComponent.enabled = true; 
        }
    }

    public void OnStartGameButtonClicked()
    {
        if (GameManager.Instance.CanStartGame())
        {
            GameManager.Instance.StartGame();
        }
        else
        {
            Debug.LogWarning("Cannot start game. Not all players have selected their movies.");
        }
    }

}

