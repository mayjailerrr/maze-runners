using UnityEngine;

public class BoardController : MonoBehaviour
{
    public int boardSize = 10;
    private Board board;
    public BoardView BoardView;
    private TurnManager turnManager;
    private Context gameContext;

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

    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            HandlePlayerInput();
        }
    }

    private void HandlePlayerInput()
    {
        Vector2 direction = GetInputDirection();
        if (direction == Vector2.zero) return;

        
        Player currentPlayer = gameContext.CurrentPlayer;
        Piece activePiece = gameContext.CurrentPiece;

        if (activePiece == null)
        {
            Debug.LogWarning("No active piece selected.");
            return;
        }

        int newX = activePiece.Position.Item1 + (int)direction.x;
        int newY = activePiece.Position.Item2 + (int)direction.y;

        if (board.IsValidMove(activePiece, newX, newY))
        {
            activePiece.Move(newX, newY);
            gameContext.UpdateTileAndPosition(board.GetTileAtPosition(newX, newY)); 
            BoardView.UpdatePiecePosition(activePiece); 
            Debug.Log($"Piece {activePiece.Name} moved to ({newX}, {newY})");
            turnManager.NextTurn();
            gameContext.ResetContextForNewTurn(turnManager.GetCurrentPlayer());
        }
        else
        {
            Debug.LogWarning("Invalid move.");
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

}
