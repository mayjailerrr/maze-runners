
using MazeRunners;
public class Context
{
    public Board Board { get; set; }
    public Piece CurrentPiece { get; set; }
    public Player CurrentPlayer { get; set; }
    public Tile CurrentTile { get; set; }
    public TurnManager TurnManager { get; private set; }
    public BoardView BoardView { get; set; }
    public Directions PlayerDirection { get; private set; }
    public Context(Board board, Player currentPlayer, Piece currentPiece = null, Tile currentTile = null)
    {
        if (board == null)
        {
            throw new System.ArgumentNullException(nameof(board), "Board cannot be null in Context.");
        }

        Board = board;
        CurrentPlayer = currentPlayer;
        CurrentPiece = currentPiece;
        CurrentTile = currentTile;

        PlayerDirection = Directions.Up;
    }

    public void SetTurnManager(TurnManager turnManager)
    {
        TurnManager = turnManager;
    }

    public void SetBoardView(BoardView boardView)
    {
        BoardView = boardView;
    }


    public void UpdateTileAndPosition(Tile tile)
    {
        CurrentTile = tile;
    }

    public void ResetContextForNewTurn(Player newPlayer)
    {
        CurrentPlayer = newPlayer;
        CurrentPiece = null;

        foreach (var piece in CurrentPlayer.Pieces)
        {
            piece.ResetTurn();
        }

        PlayerDirection = Directions.Up;
    }

    public bool AllPiecesMoved()
    {
        foreach (var piece in CurrentPlayer.Pieces)
        {
            if (!piece.HasMoved)
            {
                return false;
            }
        }

        return true;
    }

    public void SetPlayerDirection(Directions direction)
    {
        PlayerDirection = direction;
    }
}
