
using UnityEngine;
using MazeRunners;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static int MaxPlayers = 4;
    public static int MinPlayers = 2;

    private int currentPlayerIndex = 0;
    private Dictionary<int, Player> players = new Dictionary<int, Player>();
    private List<Movies> selectedMovies = new List<Movies>();

    public int PlayerCount => players.Count;
    private Board board;
    public BoardController BoardController { get; private set; }
    public TurnManager TurnManager { get; private set; }
    public BoardView BoardView { get; set; }
   
    public Context GameContext { get; private set; }

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

        selectedMovies.Add(movie);

        Debug.Log($"Player {currentPlayerIndex + 1} selected {movie}.");
        return true;
    } 

    public void StartGame()
    {
        if (!CanStartGame())
        {
            Debug.LogError("Cannot start game. Not enough players or movies selected.");
            return;
        }

        board = new Board(10);

    
        foreach (var playerEntry in players)
        {
            Player player = playerEntry.Value;
            Movies selectedMovie = selectedMovies[player.ID];
            List<Piece> pieces = PieceFactory.CreatePieces(selectedMovie);
            player.AssignPieces(pieces);

            board.PlacePiecesRandomly(pieces);
        }

        Player initialPlayer = players.Values.First();  

        GameContext = new Context(board, initialPlayer);
        TurnManager = new TurnManager(new List<Player>(players.Values), GameContext);

        BoardController.ExternalInitialize(board, BoardController.GetComponent<BoardView>(), TurnManager, GameContext);

        Debug.Log("Game started!");
        TurnManager.StartTurn();
    }



    public bool CanStartGame()
    {
        return PlayerCount >= MinPlayers && selectedMovies.Count == PlayerCount;
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
        //to-do: collect the objects from th movies
    }

    
}
