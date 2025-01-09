
using UnityEngine;
using UnityEngine.UI;
using MazeRunners;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public Grid grid;
    Vector2Int boardSize;
    public static GameManager Instance { get; private set; }

    public List<Collectible> playersCollectibles = new List<Collectible>();

    public int PlayerCount => players.Count;
    private Dictionary<int, Player> players = new Dictionary<int, Player>();
    private List<Movies> selectedMovies = new List<Movies>();
    public Context GameContext { get; private set; }

    private Board board;
    public BoardController BoardController { get; private set; }
    public TurnManager TurnManager { get; private set; }
    public Button endTurnButton;
    private EndTurnHandler endTurnHandler;
   
    private int currentPlayerIndex = 0;
    public BoardView BoardView { get; set; }
    public PieceGridView PieceGridView { get; set; }
    public PieceController PieceController { get; set; }
   
  
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
       
    }


    private void Update()
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

        var player = GetOrCreatePlayer();
        AssignMovieToPlayer(movie, player);

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

        GenerateAllCollectibles();
        InitializeBoard();
        InitializeTurnManager();
        InitializeEndTurnButton();

        BoardController.ExternalInitialize(board, BoardController.GetComponent<BoardView>()); 
       
        InitializePieceGridView();
        InitializePieceController();
        GameContext.SetTurnManager(TurnManager);

        TurnManager.StartTurn();
    }

    public bool CanStartGame() => PlayerCount >= 2 && PlayerCount <= 4 && selectedMovies.Count == PlayerCount;
   
    private Player GetOrCreatePlayer()
    {
        if (!players.ContainsKey(players.Count))
            players[players.Count] = new Player(players.Count);

        return players[players.Count - 1];
    }

    private void AssignMovieToPlayer(Movies movie, Player player)
    {
        player.AssignPieces(PieceFactory.CreatePieces(movie));
        player.AssignObjects(CollectibleFactory.CreateCollectibles(movie));
    }

    private void InitializeBoard()
    {
        board = new Board(10, playersCollectibles);

        foreach(var playerEntry in players)
        {
            Player player = playerEntry.Value;
            Movies selectedMovie = selectedMovies[player.ID];
            
            player.AssignPieces(PieceFactory.CreatePieces(selectedMovie));
            board.PlacePiecesRandomly(player.Pieces);
        }

        Player initialPlayer = players.Values.First();
        GameContext = new Context(board, initialPlayer);  

    }

    private void GenerateAllCollectibles()
    {
        foreach (var playerEntry in players)
        {
            Player player = playerEntry.Value;
            Movies selectedMovie = selectedMovies[player.ID];

            List<Collectible> collectibles = CollectibleFactory.CreateCollectibles(selectedMovie);

            player.AssignObjects(collectibles);

            playersCollectibles.AddRange(collectibles);
        }
    }


    private void InitializeTurnManager()
    {
        TurnManager = new TurnManager(new List<Player>(players.Values), GameContext);
    }

     private void InitializeEndTurnButton()
    {
        endTurnHandler = endTurnButton.GetComponent<EndTurnHandler>();
        if (endTurnHandler == null)
        {
            endTurnHandler = endTurnButton.gameObject.AddComponent<EndTurnHandler>();
        }

        endTurnHandler.Initialize(TurnManager);
    }

    private void InitializePieceController()
    {
        if (PieceController == null)
        {
            PieceController = FindObjectOfType<PieceController>();
            if (PieceController == null)
            {
                Debug.LogError("PieceController not found in the scene.");
            }
            else
            {
               PieceController.InitializePieceController(board, TurnManager, GameContext, PieceGridView);
            }
        }
    }

    private void InitializePieceGridView()
    {
        if (PieceGridView == null)
        {
            PieceGridView = FindObjectOfType<PieceGridView>();
            if (PieceGridView == null)
            {
                Debug.LogError("PieceGridView not found in the scene.");
            }
            else
            {
                PieceGridView.InitializeGrid(board);
            }
        }
    }

    public void NextPlayer()
    {
      
        currentPlayerIndex++;
        if (currentPlayerIndex >= 4 || currentPlayerIndex >= PlayerCount + 1)
        {
            Debug.Log("All players have selected movies.");
            currentPlayerIndex = PlayerCount;
        }
    }

    public void EndGame(Player winner)
    {
        Debug.Log($"Player {winner.ID + 1} wins the game!");
        // to - do:
        // show message with winner
        // restart game or go back to main menu
        // optional: show all players' stats
    }

    
}
