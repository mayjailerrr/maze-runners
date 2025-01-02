using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using MazeRunners;

public class PieceController : MonoBehaviour
{
    private int selectedPieceIndex = 0;
    private TurnManager turnManager;
    private Context gameContext;
    private Board board;
    private PieceGridView PieceGridView;

    private bool isInitialized = false;

    public void InitializePieceController(Board board, TurnManager turnManager, Context context, PieceGridView pieceGridView)
    {
        this.board = board;
        this.turnManager = turnManager;
        this.gameContext = context;
        this.PieceGridView = pieceGridView;

        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized) return;

        if (Input.anyKeyDown)
        {
            HandlePlayerInput();
        }
    }

    public void HandlePlayerInput()
    {
        Player currentPlayer = gameContext.CurrentPlayer;
        List<Piece> pieces = new List<Piece>(currentPlayer.Pieces);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CycleThroughPieces(pieces);
            return;
        }

        Piece activePiece = pieces[selectedPieceIndex];
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
        if (Input.GetKeyDown(KeyCode.W)) return Vector2.left;
        if (Input.GetKeyDown(KeyCode.A)) return Vector2.right;
        if (Input.GetKeyDown(KeyCode.S)) return Vector2.down;
        if (Input.GetKeyDown(KeyCode.D)) return Vector2.up;
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
        if (piece == null)
        {
            Debug.LogError("Piece is null in TryMovePiece.");
        }

        if(board == null)
        {
            Debug.LogError("Board is null in TryMovePiece.");
        }

        if(PieceGridView == null)
        {
            Debug.LogError("PieceGridView is null in TryMovePiece.");
        }

        if (!piece.CanMoveMoreTiles())
        {
            Debug.LogWarning($"Piece {piece.Name} has no moves left this turn.");
            return;
        }

        int newX = piece.Position.Item1 + (int)direction.x;
        int newY = piece.Position.Item2 + (int)direction.y;
        
        bool isMoving = board.IsValidMove(piece, newX, newY);
        if (isMoving)
        {
            if (turnManager.PerformAction(ActionType.Move, piece, board, newX, newY, gameContext))
            {
                Tile targetTile = board.GetTileAtPosition(newX, newY);
                gameContext.UpdateTileAndPosition(board.GetTileAtPosition(newX, newY));
                piece.UpdatePosition((newX, newY));
                //PieceGridView.UpdatePiecePosition(piece);

                if(piece.View == null)
                {
                    Debug.LogError("PieceView is null for piece.");
                    return;
                }

                piece.View.UpdateAnimation(direction, true);

                if (targetTile is TrapTile trapTile)
                {
                    trapTile.ActivateTrap(piece, turnManager);
                }

                else if (targetTile is CollectibleTile collectibleTile)
                {
                    collectibleTile.Interact(piece, gameContext.CurrentPlayer);
                    if (gameContext.CurrentPlayer.HasCollectedAllObjects())
                    {
                        GameManager.Instance.EndGame(gameContext.CurrentPlayer);
                    }
                }
            }   
        }

        else
        {
            piece.View.UpdateAnimation(Vector2.zero, false);
            Debug.LogWarning($"Invalid move for piece {piece.Name} to ({newX}, {newY}).");
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