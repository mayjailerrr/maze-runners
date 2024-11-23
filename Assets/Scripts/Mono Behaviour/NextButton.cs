using UnityEngine;
using UnityEngine.SceneManagement;

public class NextButton : MonoBehaviour
{
    public string mazeSceneName = "Game"; 

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
}
