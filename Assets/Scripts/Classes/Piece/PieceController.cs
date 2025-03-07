using UnityEngine;
using System.Collections.Generic;
using MazeRunners;
using System.Collections;

public class PieceController : MonoBehaviour
{
    private int selectedPieceIndex = 0;
    private TurnManager turnManager;
    private Context gameContext;
    private Board board;
    private PieceGridView pieceGridView;
    private CollectibleViewManager collectibleViewManager;
    private HUDController hudController;

    private Queue<Vector2> inputQueue = new Queue<Vector2>();
    private bool isProcessingInput = false;

    private bool isInitialized = false;

    public void InitializePieceController(Board board, TurnManager turnManager, Context context, PieceGridView pieceGridView)
    {
        this.board = board;
        this.turnManager = turnManager;
        this.gameContext = context;
        this.pieceGridView = pieceGridView;
        this.collectibleViewManager = FindObjectOfType<CollectibleViewManager>();
        this.hudController = FindObjectOfType<HUDController>();

        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized) return;
        HandlePlayerInput();
    }

    private void HandlePlayerInput()
    {
        Player currentPlayer = gameContext.CurrentPlayer;
        if (currentPlayer == null || currentPlayer.Pieces == null || currentPlayer.Pieces.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CycleThroughPieces(currentPlayer.Pieces);
            return;
        }

        if (selectedPieceIndex < 0 || selectedPieceIndex >= currentPlayer.Pieces.Count)
            selectedPieceIndex = 0;
        Piece activePiece = currentPlayer.Pieces[selectedPieceIndex];

        
        Vector2 direction = GetInputDirection();
        if (direction != Vector2.zero)
        {
            inputQueue.Enqueue(direction);
        }

        if (!isProcessingInput && inputQueue.Count > 0)
        {
            Vector2 nextDirection = inputQueue.Dequeue();
            StartCoroutine(ProcessInput(nextDirection));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryUsePieceAbility(currentPlayer, activePiece);
        }
    }

    private Vector2 GetInputDirection()
    {
        if (Input.GetKeyDown(KeyCode.W)) return Vector2.left; // Up
        if (Input.GetKeyDown(KeyCode.A)) return Vector2.down; // Left
        if (Input.GetKeyDown(KeyCode.S)) return Vector2.right; // Down
        if (Input.GetKeyDown(KeyCode.D)) return Vector2.up; // Right
        return Vector2.zero;
    }

    private void CycleThroughPieces(List<Piece> pieces)
    {
        selectedPieceIndex = (selectedPieceIndex + 1) % pieces.Count;
        Piece selectedPiece = pieces[selectedPieceIndex];
        gameContext.CurrentPiece = selectedPiece;

        hudController.UpdateHUD(gameContext.CurrentPlayer, selectedPiece);

        Debug.Log($"Selected piece: {selectedPiece.Name}");
    }

    private IEnumerator ProcessInput(Vector2 direction)
    {
        isProcessingInput = true;

        Piece activePiece = gameContext.CurrentPlayer.Pieces[selectedPieceIndex];
        TryMovePiece(activePiece, direction);

        yield return new WaitForSeconds(0.5f);

        isProcessingInput = false;
    }

    private void TryMovePiece(Piece piece, Vector2 direction)
    {
        if (!piece.CanMoveMoreTiles())
        {
            Debug.LogWarning($"Piece {piece.Name} has no moves left this turn.");
            return;
        }

        Directions directionString = direction switch
        {
            Vector2 v when v == Vector2.up => Directions.Up,
            Vector2 v when v == Vector2.down => Directions.Down,
            Vector2 v when v == Vector2.left => Directions.Left,
            Vector2 v when v == Vector2.right => Directions.Right,
            _ => Directions.Right
        };

        if (direction == Vector2.left || direction == Vector2.right)
        {
            gameContext.SetPlayerDirection(directionString);
        }


        int newX = piece.Position.Item1 + (int)direction.x;
        int newY = piece.Position.Item2 + (int)direction.y;

        if (!board.IsWithinBounds(newX, newY))
        {
            Debug.LogWarning("Move out of bounds.");
            return;
        }

        Tile targetTile = board.GetTileAtPosition(newX, newY);

       if ((targetTile is CollectibleTile collectibleTile && collectibleTile.Collectible != null &&
            !collectibleTile.CanBeCollectedBy(gameContext.CurrentPlayer)) || targetTile.IsOccupied)
        {
            Debug.LogWarning("Movement blocked by another piece or collectible.");
            piece.View.UpdateAnimation(gameContext.PlayerDirection == Directions.Left ? Vector2.left : Vector2.right, false);

            return;
        }

        if (!board.IsValidMove(piece, newX, newY, gameContext))
        {
            piece.View.UpdateAnimation(Vector2.zero, false);
            Debug.LogWarning($"Invalid move for piece {piece.Name} to ({newX}, {newY}).");
            return;
        }

        var previousPosition = piece.Position;

        if(turnManager.PerformAction(ActionType.Move, piece, board, newX, newY, gameContext))
        {
           
            gameContext.UpdateTileAndPosition(board.GetTileAtPosition(newX, newY));

            pieceGridView.MovePiece(piece, previousPosition.x, previousPosition.y, newX, newY);
            piece.UpdatePosition((newX, newY));
            board.CleanPreviousTile(previousPosition.x, previousPosition.y);

            if (piece.View == null)
            {
                Debug.LogError("PieceView is null for piece.");
                return;
            }
            
            piece.View.UpdateAnimation(direction, true);
            HandleTileInteractions(piece, targetTile);
        }
    }

    private void HandleTileInteractions(Piece piece, Tile targetTile)
    {
        if (targetTile is TrapTile trapTile)
        {
            trapTile.ActivateTrap(piece, gameContext);
        }

        if (targetTile is CollectibleTile collectibleTile)
        {
            collectibleTile.Interact(piece, gameContext.CurrentPlayer);

            if (gameContext.CurrentPlayer.HasCollectedAllObjects())
            {
                GameManager.Instance.EndGame(gameContext.CurrentPlayer);
            }
        }
    }

    private void TryUsePieceAbility(Player player, Piece piece)
    {
        if (player.UsePieceAbility(piece, gameContext))
        {
            turnManager.CheckPieceExhausted();
        }
        else
        {
            Debug.LogWarning($"Ability of piece {piece.Name} is on cooldown or you haven't selected the piece previously.");
        }
    }
}
