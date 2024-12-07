using UnityEngine;
using MazeRunners;

public class MovieSelectionController : MonoBehaviour
{
    public void OnMovieButtonClicked(int movieIndex)
    {
        Movies selectedMovie = (Movies)movieIndex;
        int currentPlayerIndex = GameManager.Instance.GetCurrentPlayerIndex();

        bool success = GameManager.Instance.AssignMovieToPlayer(selectedMovie);

        if (success)
        {
            Debug.Log($"Player {currentPlayerIndex + 1} successfully selected {selectedMovie}");
            GameManager.Instance.NextPlayer();
        }
        else
        {
            Debug.LogError($"Player {currentPlayerIndex + 1} failed to select {selectedMovie}");
        }
    }
}
