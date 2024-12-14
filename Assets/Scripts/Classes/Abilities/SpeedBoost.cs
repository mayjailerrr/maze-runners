using System;
using System.Collections.Generic;
using MazeRunners;
using UnityEngine;

public class SpeedBoostAbility : IAbility
{
    public string Description => "Increases the piece's speed for one turn.";

    public bool Execute(Context context)
    {
        context.CurrentPiece.Speed += 2;
        Debug.Log($"Piece speed increased to {context.CurrentPiece.Speed}");
        return true;
    }
}