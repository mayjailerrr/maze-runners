using System;
using System.Collections.Generic;
using MazeRunners;
using UnityEngine;

public class WallDestroyerAbility : IAbility
{
    public string Description => "Destroys a wall that blocks the path of the actual player.";

    public bool Execute(Context context)
    {
        var currentPiece = context.CurrentPiece;
        if (currentPiece == null)
        {
            Debug.LogError("No piece selected to destroy walls.");
            return false;
        }

        var board = context.Board;
        var boardView = context.BoardView;
        var currentPosition = currentPiece.Position;

        (int dx, int dy) direction = GetFacingDirection(context);
        int targetX = currentPosition.x + direction.dx;
        int targetY = currentPosition.y + direction.dy;

        if (board.IsWithinBounds(targetX, targetY))
        {
            var targetTile = board.GetTileAtPosition(targetX, targetY);

            if (targetTile is ObstacleTile)
            {
                board.ReplaceTile(targetX, targetY, new Tile(targetX, targetY));
                ReplaceTileVisual(boardView, targetX, targetY, board);

                Debug.Log($"Wall destroyed at ({targetX}, {targetY}).");
                return true;
            }
            else
            {
                Debug.LogWarning($"No wall to destroy at ({targetX}, {targetY}).");
                return false;
            }
        }

        Debug.LogError($"Target position ({targetX}, {targetY}) is out of bounds.");
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
        
        LeanTween.scale(tileGO, Vector3.zero, 0.4f).setEaseInBack()
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

    private (int dx, int dy) GetFacingDirection(Context context)
    {
        var direction = context.PlayerDirection;
        return direction switch
        {
            "Up" => (0, -1),
            "Down" => (0, 1),
            "Left" => (-1, 0),
            "Right" => (1, 0),
            _ => (0, 0),
        };
    }
}
