using System;
using System.Collections.Generic;
using MazeRunners;
using UnityEngine;

public class TeleportAbility : IAbility
{
    public string Description => "Teleports to a random tile.";

    public bool Execute(Context context)
    {
        var random = new System.Random();
        var randomTile = context.Board.GetRandomTile();

        context.CurrentPiece.Position = (randomTile.Position.x, randomTile.Position.y);
        Debug.Log($"Piece teleported to ({randomTile.Position.x}, {randomTile.Position.y})");
        return true;
    }
}