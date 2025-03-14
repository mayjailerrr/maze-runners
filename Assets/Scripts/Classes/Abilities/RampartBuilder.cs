using MazeRunners;
using UnityEngine;
using System.Linq;

public class RampartBuilderAbility : IAbility
{
    public string Description => "Builds walls around the current piece.";
    private int selectedPieceIndex;
    public bool Execute(Context context)
    {
        Player nextPlayer = context.TurnManager.GetNextPlayer(context.CurrentPlayer);

        var validTargets = nextPlayer.Pieces.Where(piece => !piece.IsShielded).ToList();

        System.Random random = new System.Random();
        selectedPieceIndex = random.Next(0, validTargets.Count);

        Piece targetPiece = validTargets[selectedPieceIndex];
        Piece currentPiece = context.CurrentPiece;

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

                if (board.IsWithinBounds(targetX, targetY))
                {
                    var tile = board.GetTileAtPosition(targetX, targetY);

                    bool hasPiece = tile.OccupyingPiece != null;
                    bool hasCollectible = tile is CollectibleTile collectibleTile && collectibleTile.Collectible != null;

                    if (tile is Tile && !hasPiece && !hasCollectible)
                    {
                        board.ReplaceTile(targetX, targetY, new ObstacleTile(targetX, targetY));
                        wallBuilt = true;

                        ReplaceTileVisual(boardView, targetX, targetY, board);
                    }
                    else
                    {
                        Debug.Log($"Cannot build wall at ({targetX}, {targetY}): Tile occupied.");
                    }
                }
            }
        }

        if (!wallBuilt)
        {
            Debug.LogWarning("No valid positions to build walls.");
        }

        context.CurrentPlayer.RecordAbilityUse();
        currentPiece.View.PlayAbilityEffect(new Color(0.6f, 0.2f, 1f, 0.8f));
        GameEvents.TriggerRampartBuilderUsed();

        Debug.Log($"A rampart has been built around {targetPiece.Name}.");

        return wallBuilt;
    }

    private void ReplaceTileVisual(BoardView boardView, int x, int y, Board board)
    {
        var tileGO = boardView.GetTileObject(x, y);

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
