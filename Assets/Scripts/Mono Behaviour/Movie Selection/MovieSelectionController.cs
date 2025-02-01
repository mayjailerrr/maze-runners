using UnityEngine;
using MazeRunners;

public class MovieSelectionController : MonoBehaviour
{
    private MovieSelectionModel model;

    private void Start()
    {
        model = new MovieSelectionModel();
    }

    public void OnMovieButtonClicked(int movieIndex)
    {
        if (!model.CanSelectMovie())
        {
            Debug.Log("Maximum number of players reached. No more movies can be selected.");
            return;
        }

        Movies selectedMovie = (Movies)movieIndex;
        int currentPlayerIndex = GameManager.Instance.GetCurrentPlayerIndex();

        if (!model.IsMovieAvailable(selectedMovie))
        {
            Debug.Log("This movie is not available.");
            return;
        }

        model.AssignMovieToPlayer(currentPlayerIndex, selectedMovie);
        GameManager.Instance.AssignMovieToPlayer(selectedMovie);

        Debug.Log($"Player {currentPlayerIndex + 1} selected {selectedMovie}");

        GameManager.Instance.NextPlayer();
    }
}
