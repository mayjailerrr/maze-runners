
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MazeRunners;

public enum Movies
{
    Ponyo,
    HowlsMovingCastle,
    PrincessMononoke,
    KikisDeliveryService,
    SpiritedAway,
    MyNeighborTotoro,
    PorcoRosso,
    Arietty
}


public class MovieSelectionMenu : MonoBehaviour
{
    public List<Movies> availableMovies = new List<Movies> 
    { 
        Movies.Ponyo,
        Movies.HowlsMovingCastle,
        Movies.PrincessMononoke,
        Movies.KikisDeliveryService,
        Movies.SpiritedAway,
        Movies.MyNeighborTotoro,
        Movies.PorcoRosso,
        Movies.Arietty
    };

    public void SelectMovie(Movies movie)
    {
        int currentPlayerIndex = GameManager.Instance.GetCurrentPlayerIndex();
      
        if (currentPlayerIndex >= GameManager.MaxPlayers)
        {
            Debug.Log("Maximum number of players reached. Can't select more movies.");
            return;
        }

        if (availableMovies.Contains(movie))
        {
            GameManager.Instance.AssignMovieToPlayer(currentPlayerIndex, movie);
            availableMovies.Remove(movie);

            Debug.Log($"Player {currentPlayerIndex + 1} selected {movie}");
            
            GameManager.Instance.NextPlayer();

        }

        else  Debug.Log("Movie not available or already selected.");
    }

    public bool CanStartGame()
    {
        return GameManager.Instance.PlayerCount >= 2;
    }

    public void OnMovieButtonClicked(int movieIndex)
    {
        if (movieIndex >= 0 && movieIndex < availableMovies.Count)
        {
            SelectMovie((Movies)movieIndex);
        }
        else    Debug.Log("Invalid movie selection.");
    }

}

