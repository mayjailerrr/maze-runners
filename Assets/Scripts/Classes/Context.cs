using System.Collections.Generic;
using MazeRunners;
public class Context
{
    public Board Board { get; set; }
    public Piece CurrentPiece { get; set; }
    public Player CurrentPlayer { get; set; }
    public Tile CurrentTile { get; set; }
    public TurnManager TurnManager { get; private set; }

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
    }

    public void SetTurnManager(TurnManager turnManager)
    {
        TurnManager = turnManager;
    }


    public void UpdateTileAndPosition(Tile tile)
    {
        CurrentTile = tile;
    }

    public void ResetContextForNewTurn(Player player)
    {
        CurrentPlayer = player;
        CurrentPiece = null;
        CurrentTile = null;
    }
}
