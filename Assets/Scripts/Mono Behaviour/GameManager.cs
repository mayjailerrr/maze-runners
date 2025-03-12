
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float GameStartTime { get; private set; }

    public int PlayerCount => Players.Count;
    public Dictionary<int, Player> Players = new Dictionary<int, Player>();
    private List<Movies> selectedMovies = new List<Movies>();
    
    public Context GameContext { get; private set; }
    private Board board;
    private BoardView boardView;
    private PieceGridView pieceGridView;
    private PieceController pieceController;
    private TurnManager turnManager;
    private EndTurnHandler endTurnHandler;
    
    public List<Collectible> playersCollectibles = new List<Collectible>();
    public CollectibleGridView collectibleGridView;
    public CollectibleViewManager collectibleViewManager;
    
    private int currentPlayerIndex = 0;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        boardView = FindObjectOfType<BoardView>();
    }

    public int GetCurrentPlayerIndex()
    {
        return currentPlayerIndex;
    }

    public bool AssignMovieToPlayer(Movies movie)
    {
        var player = CreatePlayer();
        AssignMovieToPlayer(movie, player);
        
        selectedMovies.Add(movie);
        Debug.Log($"Player {currentPlayerIndex + 1} selected {movie}.");
        
        return true;
    } 

    public void StartGame()
    {
        GameStartTime = Time.time;

        GenerateAllCollectibles();
        InitializeBoard();
        InitializeTurnManager();
        InitializeEndTurnHandler();
        
        boardView.InitializeTileBoardView(board); 
       
        InitializePieceGridView();
        InitializePieceController();
        GameContext.SetTurnManager(turnManager);
        GameContext.SetBoardView(boardView);
        GameContext.SetGameManager(this);

        collectibleViewManager = FindObjectOfType<CollectibleViewManager>();
        collectibleGridView.InitializeGrid(board, boardView, collectibleViewManager);

        turnManager.StartTurn();
    }

    public bool CanStartGame() => PlayerCount >= 2 && PlayerCount <= 4 && selectedMovies.Count == PlayerCount;
   
    private Player CreatePlayer()
    {
        if (!Players.ContainsKey(Players.Count))
            Players[Players.Count] = new Player(Players.Count);

        return Players[Players.Count - 1];
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

        foreach(var playerEntry in Players)
        {
            Player player = playerEntry.Value;
            Movies selectedMovie = selectedMovies[player.ID];
            
            List<Piece> playerPieces = PieceFactory.CreatePieces(selectedMovie);
            player.AssignPieces(playerPieces);
            allPieces.AddRange(playerPieces);
        }

        generator.GenerateBoard(playersCollectibles, allPieces);

        Player initialPlayer = Players.Values.First();
        GameContext = new Context(board, initialPlayer);  

    }

    private void GenerateAllCollectibles()
    {
        foreach (var playerEntry in Players)
        {
            Player player = playerEntry.Value;
            Movies selectedMovie = selectedMovies[player.ID];

            List<Collectible> collectibles = CollectibleFactory.CreateCollectibles(selectedMovie, player.ID);

            int collectibleIndex = 0;

            foreach (var collectible in collectibles)
            {
                collectibleViewManager.CreateCollectibleVisual(collectible);
                collectibleIndex++; 
            }

            player.AssignObjects(collectibles);
            playersCollectibles.AddRange(collectibles);
        }
    }

    private void InitializeTurnManager()
    {
        turnManager = new TurnManager(new List<Player>(Players.Values), GameContext);
    }

    private void InitializeEndTurnHandler()
    {
        endTurnHandler = gameObject.AddComponent<EndTurnHandler>();
        endTurnHandler.Initialize(turnManager);
    }

    private void InitializePieceController()
    {
        pieceController = FindObjectOfType<PieceController>();
        pieceController.InitializePieceController(board, turnManager, GameContext, pieceGridView);
    }

    private void InitializePieceGridView()
    {
        pieceGridView = FindObjectOfType<PieceGridView>();
        pieceGridView.InitializeGrid(board, boardView);
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

        var winnerMovie = selectedMovies[winner.ID];
        Sprite finalMeme = MemeManager.Instance.GetMemeForMovie(winnerMovie);

        endGameManager.EndGame(winner);
    }

}
