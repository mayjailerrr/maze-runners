using System;
using System.Collections.Generic;
using MazeRunners;
using UnityEngine;

public class WallBombAbility : IAbility
{
    public string Description => "Explodes, destroying all walls in a 3x3 area around the piece.";

    public bool Execute(Context context)
    {
        var targetPiece = context.CurrentPiece;
        if (targetPiece == null)
        {
            Debug.LogError("No piece selected to clone.");
            return false;
        }

        var board = context.Board;
        var boardView = context.BoardView;
        var position = targetPiece.Position;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                int targetX = position.x + x;
                int targetY = position.y + y;

                if (board.IsWithinBounds(targetX, targetY))
                {
                    var tile = board.GetTileAtPosition(targetX, targetY);

                    if (tile != null)
                    {
                        if (tile is ObstacleTile || tile is TrapTile)
                        {
                            board.ReplaceTile(targetX, targetY, new Tile(targetX, targetY)); 
                            ReplaceTileVisual(boardView, targetX, targetY, board);
                        }
                        else if (tile is CollectibleTile)
                        {
                            continue;
                        }
                    }
                }

                Debug.Log($"WallBomb destroyed wall at {targetX}, {targetY}");
            }
        }

        return true;
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
       
        LeanTween.scale(tileGO, Vector3.zero, 0.4f).setEaseInBack();
        LeanTween.color(tileGO, Color.red, 0.4f)
        .setOnComplete(() =>
        {
            GameObject.Destroy(tileGO);

            var newTileGO = GameObject.Instantiate(boardView.horizontalTilePrefab, boardView.transform);
            newTileGO.transform.localPosition = Vector3.zero;
            newTileGO.transform.localScale = Vector3.zero;
            newTileGO.transform.localRotation = Quaternion.identity;
            newTileGO.name = $"Tile ({x}, {y})";
            newTileGO.transform.SetSiblingIndex(siblingIndex);

            LeanTween.scale(newTileGO, Vector3.one, 0.5f).setEaseOutElastic();
        });
    }
}