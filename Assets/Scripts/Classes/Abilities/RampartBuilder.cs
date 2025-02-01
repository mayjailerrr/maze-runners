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
        var boardView = context.BoardView;
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

                    ReplaceTileVisual(boardView, targetX, targetY, board);
                }
            }
        }

        if (!wallBuilt)
        {
            Debug.LogWarning("No valid positions to build walls.");
        }

        context.CurrentPlayer.RecordAbilityUse();
        context.CurrentPiece.View.PlayAbilitySound();

        return wallBuilt;
    }

    private void ReplaceTileVisual(BoardView boardView, int x, int y, Board board)
    {
        var tileGO = boardView.GetTileObject(x, y);
        if (tileGO == null)
        {
            Debug.LogError($"No tile found at ({x}, {y}).");
            return;
        }

        int siblingIndex = tileGO.transform.GetSiblingIndex();
        GameObject.Destroy(tileGO);

        var newTileGO = GameObject.Instantiate(boardView.GetPrefabForTile(board, board.GetTileAtPosition(x, y), x, y), boardView.transform);

        newTileGO.transform.localPosition = Vector3.zero;
        newTileGO.transform.localScale = new Vector3(1f, 0f, 1f);
        newTileGO.transform.localRotation = Quaternion.identity; 
        newTileGO.name = $"Tile ({x}, {y})";
        newTileGO.transform.SetSiblingIndex(siblingIndex);

        LeanTween.scaleY(newTileGO, 1f, 0.5f).setEaseOutElastic();
    }
}
