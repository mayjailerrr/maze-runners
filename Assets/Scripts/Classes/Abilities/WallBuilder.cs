
using UnityEngine;
using System.Linq;

public class WallBuilderAbility : IAbility
{
    public string Description => "Builds a wall that blocks the path of the other player.";
    private int selectedPieceIndex;

    public bool Execute(Context context)
    {
        Player nextPlayer = context.TurnManager.GetNextPlayer(context.CurrentPlayer);

        if (nextPlayer == null || nextPlayer.Pieces.Count == 0)
        {
            Debug.LogError("No valid target pieces.");
            return false;
        }

        var validTargets = nextPlayer.Pieces.Where(piece => !piece.IsShielded).ToList();

        if (validTargets.Count == 0)
        {
            Debug.LogWarning("All target pieces are shielded. No walls builded.");
            return false;
        }

        System.Random random = new System.Random();
        selectedPieceIndex = random.Next(0, validTargets.Count);

        Piece targetPiece = validTargets[selectedPieceIndex];
        Piece currentPiece = context.CurrentPiece;
        Debug.Log($"Target piece: {targetPiece?.Name}");

        var board = context.Board;
        var boardView = context.BoardView;
        var position = targetPiece.Position;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int targetX = position.x + x;
                int targetY = position.y + y;

                if (board.IsWithinBounds(targetX, targetY) && !(board.GetTileAtPosition(targetX, targetY) is ObstacleTile))
                {
                    var tile = board.GetTileAtPosition(targetX, targetY); 

                    bool hasPiece = tile.OccupyingPiece != null;
                    bool hasCollectible = tile is CollectibleTile collectibleTile && collectibleTile.Collectible != null;

                    if (!hasPiece && !hasCollectible)
                    {
                        board.ReplaceTile(targetX, targetY, new ObstacleTile(targetX, targetY));
                        ReplaceTileVisual(boardView, targetX, targetY, board);
                        
                        Debug.Log($"Wall built at ({targetX}, {targetY}).");
                        return true;
                    }
                    else
                    {
                        Debug.Log($"Cannot build wall at ({targetX}, {targetY}): Tile occupied.");
                    }
                }
            }
        }

        context.CurrentPlayer.RecordAbilityUse();
        currentPiece.View.PlayAbilityEffect(new Color(0f, 0.39f, 0f, 0.8f));
        currentPiece.View.PlayAbilitySound();

        Debug.LogWarning("No valid position to build a wall.");
        
        return false;
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