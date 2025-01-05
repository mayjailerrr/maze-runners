using MazeRunners;
using UnityEngine;

public class RampartBuilderAbility : IAbility
{
    public string Description => "Builds walls around the current piece.";
    private int selectedPieceIndex;
    public bool Execute(Context context)
    {
       Player nextPlayer = context.TurnManager.GetNextPlayer(context.CurrentPlayer);

        if (nextPlayer == null || nextPlayer.Pieces.Count == 0)
        {
            Debug.LogError("No valid target pieces.");
            return false;
        }

        System.Random random = new System.Random();
        selectedPieceIndex = random.Next(0, nextPlayer.Pieces.Count);

        Piece targetPiece = nextPlayer.Pieces[selectedPieceIndex];
        Debug.Log($"Target piece: {targetPiece?.Name}");

        if (targetPiece == null)
        {
            Debug.LogError("No target piece selected to freeze.");
            return false;
        }

        var board = context.Board;
        var position = targetPiece.Position;

        bool wallBuilt = false;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int targetX = position.x + x;
                int targetY = position.y + y;

                if (board.IsWithinBounds(targetX, targetY) && board.GetTileAtPosition(targetX, targetY) is Tile)
                {
                    board.ReplaceTile(targetX, targetY, new ObstacleTile(targetX, targetY));
                    Debug.Log($"Wall built at ({targetX}, {targetY}).");
                    wallBuilt = true; 
                }
            }
        }

        if (!wallBuilt)
        {
            Debug.LogWarning("No valid positions to build walls.");
        }

        return wallBuilt;
    }
}
