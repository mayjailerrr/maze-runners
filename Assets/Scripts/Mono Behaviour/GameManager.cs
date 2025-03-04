
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float GameStartTime { get; private set; }

    public List<Collectible> playersCollectibles = new List<Collectible>();

    public int PlayerCount => players.Count;
    private Dictionary<int, Player> players = new Dictionary<int, Player>();
    public IReadOnlyDictionary<int, Player> Players => players;
    
    private List<Movies> selectedMovies = new List<Movies>();
    
    public Context GameContext { get; private set; }

    private Board board;
    public BoardController BoardController { get; private set; }
    public TurnManager TurnManager { get; private set; }
    
    public CollectibleGridView collectibleGridView;
    public CollectibleViewManager collectibleViewManager;
    
    private EndTurnHandler endTurnHandler;
   
    private int currentPlayerIndex = 0;
    public PieceGridView PieceGridView { get; set; }
    public PieceController PieceController { get; set; }
    private BoardView boardView;
    
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
        boardView = BoardController.GetComponent<BoardView>();

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

        GameStartTime = Time.time;

        GenerateAllCollectibles();
        InitializeBoard();
        InitializeTurnManager();
        InitializeEndTurnHandler();

        BoardController.ExternalInitialize(board, boardView); 
       
        InitializePieceGridView();
        InitializePieceController();
        GameContext.SetTurnManager(TurnManager);
        GameContext.SetBoardView(boardView);
        GameContext.SetGameManager(this);

        collectibleViewManager = FindObjectOfType<CollectibleViewManager>();
        collectibleGridView.InitializeGrid(board, boardView, collectibleViewManager);

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
        player.AssignObjects(CollectibleFactory.CreateCollectibles(movie, player.ID));
    }

    private void InitializeBoard()
    {
        board = new Board(13);
        BoardGenerator generator = new BoardGenerator(board);

        List<Piece> allPieces = new List<Piece>();

        foreach(var playerEntry in players)
        {
            Player player = playerEntry.Value;
            Movies selectedMovie = selectedMovies[player.ID];
            
            List<Piece> playerPieces = PieceFactory.CreatePieces(selectedMovie);
            player.AssignPieces(playerPieces);
            allPieces.AddRange(playerPieces);
        }

        generator.GenerateBoard(playersCollectibles, allPieces);

        Player initialPlayer = players.Values.First();
        GameContext = new Context(board, initialPlayer);  

    }

    private void GenerateAllCollectibles()
    {
        if (collectibleViewManager == null)
        {
            Debug.LogError("CollectibleViewManager is null. Ensure it is properly assigned.");
            return;
        }

        foreach (var playerEntry in players)
        {
            Player player = playerEntry.Value;
            Movies selectedMovie = selectedMovies[player.ID];

            List<Collectible> collectibles = CollectibleFactory.CreateCollectibles(selectedMovie, player.ID);

            int collectibleIndex = 0;

            foreach (var collectible in collectibles)
            {
                if (collectibleViewManager == null)
                {
                    Debug.LogError("CollectibleViewManager not found in the scene.");
                    return;
                }

                if (collectible is null)
                {
                    Debug.LogError("Collectible is null.");
                }

                collectibleViewManager.CreateCollectibleVisual(collectible);

                collectibleIndex++; 
            }

            player.AssignObjects(collectibles);

            playersCollectibles.AddRange(collectibles);
        }
    }

    private void InitializeTurnManager()
    {
        TurnManager = new TurnManager(new List<Player>(players.Values), GameContext);
    }

    private void InitializeEndTurnHandler()
    {
        endTurnHandler = gameObject.GetComponent<EndTurnHandler>();
        if (endTurnHandler == null)
        {
            endTurnHandler = gameObject.AddComponent<EndTurnHandler>();
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
                PieceGridView.InitializeGrid(board, BoardController.GetComponent<BoardView>());
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

    public Movies GetSelectedMovieForPlayer(int playerId)
    {
        return selectedMovies[playerId];
    }

    public void EndGame(Player winner)
    {
        Debug.Log($"Player {winner.ID + 1} wins the game!");

        EndGameManager endGameManager = FindObjectOfType<EndGameManager>();
        if (endGameManager == null)
        {
            Debug.LogError("EndGameManager not found in the scene.");
            return;
        }

        var winnerMovie = selectedMovies[winner.ID];
        Sprite finalMeme = MemeManager.Instance.GetMemeForMovie(winnerMovie);

        endGameManager.EndGame(winner);
    }

}
