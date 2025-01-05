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
        var position = targetPiece.Position;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                int targetX = position.x + x;
                int targetY = position.y + y;

                if (board.IsWithinBounds(targetX, targetY))  //to-do: check if necesary
                {
                    var tile = board.GetTileAtPosition(targetX, targetY);

                    if (tile != null)
                    {
                        if (tile is ObstacleTile || tile is TrapTile)
                        {
                            board.ReplaceTile(targetX, targetY, new Tile(targetX, targetY)); 
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
}