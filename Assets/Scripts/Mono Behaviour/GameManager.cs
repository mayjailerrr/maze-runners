using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using MazeRunners;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int numPlayers;
    public int boardSize;

    public static int MaxPlayers = 4;
    private static int MinPlayers = 2;
    private int currentPlayerIndex = 0;

    private Dictionary<int, Player> players = new Dictionary<int, Player>();    
    public int PlayerCount => players.Count;

    private List<Movies> selectedMovies = new List<Movies>();
   
    public Board board;
    private Piece activePiece;
    public List<Piece> currentPlayerPieces;
    
    private bool movementComplete;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public int GetCurrentPlayerIndex()
    {
        return currentPlayerIndex;
    }

    public void NextPlayer()
    {
        currentPlayerIndex++;
    }
   
    // void Start()
    // {
    //     StartGame();
    // }

    // void Update()
    // {
    //     HandlePlayerTurn();
    // }

    public void AssignMovieToPlayer(int playerId, Movies movie)
    {
        if (players.Count >= MaxPlayers)
        {
            Debug.Log("Cannot assign movie: Maximum number of players reached");
            return;
        }

        if (!players.ContainsKey(playerId))
        {
            Player newPlayer = new Player(playerId);
            players.Add(playerId, newPlayer);
        }

        Player player = players[playerId];
        List<Piece> pieces = PieceFactory.CreatePieces(movie);
        player.AssignPieces(pieces);
        selectedMovies.Add(movie);

        Debug.Log($"Player {playerId} assigned {movie} with pieces: {string.Join(", ", pieces)}");
    }

    private void HandlePlayerTurn()
    {
        Player currentPlayer = players[currentPlayerIndex];
        currentPlayerPieces = currentPlayer.Pieces; //TO-DO: Change this to player input

        if (activePiece == null)
        {
            DisplayAvailablePieces(); //show pieces
            HandlePiecesSelection(currentPlayer); //let the player select
            return; //wait until is selected
        }

        Vector2Int direction = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W)) direction = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.S)) direction = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.A)) direction = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D)) direction = Vector2Int.right;

        if (direction != Vector2Int.zero)
        {
            Vector2Int targetPosition = new Vector2Int(
                activePiece.Position.x + direction.x,
                activePiece.Position.y + direction.y
            );

            if (MovePieceIfValid(currentPlayer, activePiece, targetPosition))
            {
                EndTurn();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentPlayer.UsePieceAbility(activePiece);
        }
    }

    private void DisplayAvailablePieces()
    {
        Debug.Log("Choose a piece to move:");

        for (int i = 0; i < currentPlayerPieces.Count; i++)
        {
            Piece piece = currentPlayerPieces[i];
            Debug.Log($"{i+1}: {piece.Name} at {piece.Position}");
        }
    }

    private void HandlePiecesSelection(Player currentPlayer)
    {
        for (int i = 0; i < currentPlayerPieces.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                activePiece = currentPlayerPieces[i];
                Debug.Log($"{activePiece.Name} selected");
                return;
            }
        }
    }

    private bool MovePieceIfValid(Player player, Piece piece, Vector2Int targetPosition)
    {
        if (board.IsValidMove(piece, targetPosition.x, targetPosition.y))
        {
          //  board.GetTileAtPosition(piece.Position).IsOccupied = false;
            //board.GetTileAtPosition(targetPosition).IsOccupied = true;

            player.MovePiece(piece, targetPosition.x, targetPosition.y, board);
            return true;
        }

        else
        {
            Debug.Log("Invalid move");
            return false;
        }
    }

    private void EndTurn()
    {
        Debug.Log($"Player {currentPlayerIndex} ends turn");

        activePiece = null;
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;

        Debug.Log($"Player {currentPlayerIndex}'s turn");
    }


    public void PlayTurn(Piece activePiece, int targetX, int targetY)
    {
        Player currentPlayer = players[currentPlayerIndex];

        currentPlayer.MovePiece(activePiece, targetX, targetY, board);
        currentPlayer.UsePieceAbility(activePiece);

        //if (CheckWinCondition(currentPlayer)) EndGame(currentPlayer);
        //else EndTurn();
    }

    public bool CanStartGame()
    {
        return players.Count >= MinPlayers && players.Count <= MaxPlayers;
    }

    public void StartGame()
    {
    //     board = new Board(boardSize);
    //     players = new Dictionary<int, Player>();

    //     for (int i = 0; i < numPlayers; i++)
    //     {
    //         Player player = new Player(i);
    //         //AssignPiecesToPlayer(player);
    //         players.Add(i, player);
    //     }

    //     board.GenerateBoard();
    //     board.PlaceObstacles();
    //     board.PlaceTraps();
    //    // board.PlaceExits();

    //     currentPlayerIndex = 0;

        if (players.Count >= MinPlayers && players.Count <= MaxPlayers)
        {
            Debug.Log("Game is starting...");
            InitializeBoard();
            currentPlayerIndex = 0;
        }

        else if (players.Count < MinPlayers)
            Debug.Log("Not enough players to start.");

        else Debug.Log("Too many players. Adjust the player count");
    }

    private void InitializeBoard()
    {
        Board board = new Board(boardSize);
        board.GenerateBoard();
        Debug.Log("Board initialized");
    }

    

    //TO-DO: CHANGE THIS
    // private void AssignPiecesToPlayerBasedOnMovie(Player player, string movie)
    // {
    //     if (movieCharacters.TryGetValue(movie, out List<string> characters))
    //     {
    //         foreach (string characterName in characters)
    //         {
    //             player.AddPiece(new Piece(characterName));
    //         }
    //     }
    //     else Debug.Log($"No characters found for movie");
       
    // }

    private void NextTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
    }

   

    // private bool CheckWinCondition(Player player)
    // {
    //     //IMPLEMENT: Condition for winning the game
    //    // return board.AreAllPiecesAtExits(player);
    // }
    
    private void EndGame(Player winner)
    {
        Debug.Log($"Player {winner.ID} wins!");
        //IMPLEMENT: Game over logic 
    }
   

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            StartGame();
        }
    }

}