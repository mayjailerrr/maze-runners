using UnityEngine;
using UnityEngine.SceneManagement;
using MazeRunners;
using UnityEngine.UI;

public class NextButton : MonoBehaviour
{
    public string mazeSceneName = "Game"; 
    public Button nextButton; 
    private void Update()
    {
        if (nextButton != null)
        {
            nextButton.interactable = GameManager.Instance.CanStartGame();
        }
    }

    public void LoadMazeScene()
    {
        if (GameManager.Instance.CanStartGame())
        {
            SceneManager.LoadScene(mazeSceneName);
        }
        else
        {
            Debug.LogWarning("The game cannot start yet. Make sure all players have selected a movie.");
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

