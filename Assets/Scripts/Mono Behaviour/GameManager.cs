
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
    public BoardController BoardController { get; private set; }
    public TurnManager TurnManager { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        BoardController = FindObjectOfType<BoardController>();

        if (BoardController == null)
        {
            Debug.LogError("BoardController not found in scene.");
        }
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

    public bool CanStartGame()
    {
        return PlayerCount >= MinPlayers && selectedMovies.Count == PlayerCount;
    }

    public void StartGame()
    {
        if (!CanStartGame())
        {
            Debug.LogError("Cannot start game. Not enough players or movies selected.");
            return;
        }

        TurnManager = new TurnManager(new List<Player>(players.Values));
        BoardController.Initialize();
       // TurnManager.StartTurn();    //to-do
        Debug.Log("Game started!");
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

    public void EndGame(Player winner)
    {
        Debug.Log($"Player {winner.ID} wins the game!");
        //to-do
    }

    
}
