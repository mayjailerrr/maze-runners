using UnityEngine;
using System.Collections.Generic;

public class BoardController : MonoBehaviour
{
    public int boardSize = 10;
    private Board board;
    public BoardView BoardView;
    private TurnManager turnManager;
    private Context gameContext;

    private int selectedPieceIndex = 0;
    private bool isInitialized = false;

    public void ExternalInitialize(Board board, BoardView boardView, TurnManager turnManager, Context context)
    {
        this.board = board;
        this.BoardView = boardView;
        this.turnManager = turnManager;
        this.gameContext = context;

        if (boardView != null)
        {
            boardView.InitializeView(board);
        }
        else
        {
            Debug.LogError("BoardView is not assigned or is not in the same GameObject.");
        }

        GameObject piecePrefab = Resources.Load<GameObject>("Prefabs/Piece");

        BoardView.InitializePieces(board.GetAllPieces());
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

    private void HandlePlayerInput()
    {
        Player currentPlayer = gameContext.CurrentPlayer;
        List<Piece> pieces = new List<Piece>(currentPlayer.Pieces);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CycleThroughPieces(pieces);
            return;
        }

        Piece activePiece = pieces[selectedPieceIndex];
        if (activePiece == null)
        {
            Debug.LogWarning("No active piece selected.");
            return;
        }


        Vector2 direction = GetInputDirection();
        if (direction != Vector2.zero)
        {
            TryMovePiece(activePiece, direction);
            return; 
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryUsePieceAbility(activePiece);
        }
    }

    private Vector2 GetInputDirection()
    {
        if (Input.GetKeyDown(KeyCode.W)) return Vector2.left; //up
        if (Input.GetKeyDown(KeyCode.A)) return Vector2.down; //left
        if (Input.GetKeyDown(KeyCode.S)) return Vector2.right;  //down
        if (Input.GetKeyDown(KeyCode.D)) return Vector2.up; //right
        return Vector2.zero;
    }

    private void CycleThroughPieces(List<Piece> pieces)
    {
        selectedPieceIndex = (selectedPieceIndex + 1) % pieces.Count;
        Piece selectedPiece = pieces[selectedPieceIndex];
        gameContext.CurrentPiece = selectedPiece;

        // to-do: highlight the selected piece visually 
        //BoardView.HighlightPiece(selectedPiece);

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

        if (board.IsValidMove(piece, newX, newY))
        {
            piece.Move(newX, newY);
            gameContext.UpdateTileAndPosition(board.GetTileAtPosition(newX, newY));
            BoardView.UpdatePiecePosition(piece);

            if (gameContext.AllPiecesMoved())
            {
                turnManager.NextTurn();
            }
        }
        else
        {
            Debug.LogWarning($"Invalid move for piece {piece.Name} to ({newX}, {newY}).");
        }
    }

    private void TryUsePieceAbility(Piece piece)
    {
        if (piece.CanUseAbility)
        {
            bool success = piece.UseAbility(gameContext);
            if (success)
            {
                if (gameContext.AllPiecesMoved())
                {
                    turnManager.NextTurn();
                }
            }
        }
        else
        {
            Debug.LogWarning($"Ability of piece {piece.Name} is on cooldown.");
        }
    }


}
