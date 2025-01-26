using UnityEngine;
using System.Collections.Generic;
using MazeRunners;

public class PieceController : MonoBehaviour
{
    private int selectedPieceIndex = 0;
    private TurnManager turnManager;
    private Context gameContext;
    private Board board;
    private PieceGridView pieceGridView;

    private bool isInitialized = false;

    public void InitializePieceController(Board board, TurnManager turnManager, Context context, PieceGridView pieceGridView)
    {
        this.board = board;
        this.turnManager = turnManager;
        this.gameContext = context;
        this.pieceGridView = pieceGridView;

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

        Piece activePiece = currentPlayer.Pieces[selectedPieceIndex];
        Vector2 direction = GetInputDirection();

        if (direction != Vector2.zero)
        {
            TryMovePiece(activePiece, direction);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryUsePieceAbility(currentPlayer, activePiece);
        }
    }

    private Vector2 GetInputDirection()
    {
        if (Input.GetKeyDown(KeyCode.W)) return Vector2.up;
        if (Input.GetKeyDown(KeyCode.A)) return Vector2.left;
        if (Input.GetKeyDown(KeyCode.S)) return Vector2.down;
        if (Input.GetKeyDown(KeyCode.D)) return Vector2.right;
        return Vector2.zero;
    }

    private void CycleThroughPieces(List<Piece> pieces)
    {
        selectedPieceIndex = (selectedPieceIndex + 1) % pieces.Count;
        Piece selectedPiece = pieces[selectedPieceIndex];
        gameContext.CurrentPiece = selectedPiece;

        Debug.Log($"Selected piece: {selectedPiece.Name}");
    }

    private void TryMovePiece(Piece piece, Vector2 direction)
    {
        if (!piece.CanMoveMoreTiles())
        {
            Debug.LogWarning($"Piece {piece.Name} has no moves left this turn.");
            return;
        }

        int newX = piece.Position.Item1 + (int)direction.x;
        int newY = piece.Position.Item2 + (int)direction.y;

        Tile targetTile = board.TileGrid[newX, newY];
        if (targetTile == null || targetTile.IsOccupied || !board.IsValidMove(piece, newX, newY))
        {
            Debug.LogWarning($"Invalid move for piece {piece.Name} to ({newX}, {newY}).");
            piece.View.UpdateAnimation(Vector2.zero, false);
            return;
        }

        if (turnManager.PerformAction(ActionType.Move, piece, board, newX, newY, gameContext))
        {
            UpdatePiecePosition(piece, targetTile, direction);
        }
    }

    private void UpdatePiecePosition(Piece piece, Tile targetTile, Vector2 direction)
    {
        Tile currentTile = board.TileGrid[piece.Position.Item1, piece.Position.Item2];

        currentTile.OccupyingPiece = null;
        targetTile.OccupyingPiece = piece;

        gameContext.UpdateTileAndPosition(targetTile);
        piece.UpdatePosition(targetTile.Position);

        if (piece.View != null)
        {
            pieceGridView.MovePiece(piece, targetTile.Position.x, targetTile.Position.y);
            piece.View.UpdateAnimation(direction, true);
        }
        else
        {
            Debug.LogError($"PieceView is null for piece {piece.Name}.");
        }

        HandleTileInteractions(piece, targetTile);
    }

    private void HandleTileInteractions(Piece piece, Tile targetTile)
    {
        if (targetTile is TrapTile trapTile)
        {
            trapTile.ActivateTrap(piece, turnManager);
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
            Debug.LogWarning($"Ability of piece {piece.Name} is on cooldown.");
        }
    }
}
