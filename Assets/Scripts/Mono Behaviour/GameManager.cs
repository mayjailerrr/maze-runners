
using UnityEngine;
using MazeRunners;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static int MaxPlayers = 4;
    public static int MinPlayers = 2;

    private int currentPlayerIndex = 0;
    private Dictionary<int, Player> players = new Dictionary<int, Player>();
    private List<Movies> selectedMovies = new List<Movies>();

    public int PlayerCount => players.Count;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public int GetCurrentPlayerIndex()
    {
        return currentPlayerIndex;
    }

    public bool AssignMovieToPlayer(Movies movie)
    {

        if (selectedMovies.Contains(movie))
        {
            Debug.LogError($"Movie {movie} is already selected. Choose a different one.");
            return false;
        }

        if (!players.ContainsKey(currentPlayerIndex))
        {
            Player newPlayer = new Player(currentPlayerIndex);
            players.Add(currentPlayerIndex, newPlayer);
        }

        Player currentPlayer = players[currentPlayerIndex];
        List<Piece> pieces = PieceFactory.CreatePieces(movie);
        currentPlayer.AssignPieces(pieces);
        selectedMovies.Add(movie);

        Debug.Log($"Player {currentPlayerIndex + 1} selected {movie}.");
        return true;
    }

    public void NextPlayer()
    {
      
        currentPlayerIndex++;
        if (currentPlayerIndex >= MaxPlayers || currentPlayerIndex >= PlayerCount + 1)
        {
            Debug.Log("All players have selected movies.");
            currentPlayerIndex = PlayerCount; 
        }
    }

    public bool CanStartGame()
    {
        return PlayerCount >= MinPlayers && selectedMovies.Count == PlayerCount;
    }
}
