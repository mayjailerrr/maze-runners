using System;
using System.Collections.Generic;
using MazeRunners;
using UnityEngine;

public class SpeedBoostAbility : IAbility
{
    public string Description => "Increases the piece's speed for one turn.";

    public bool Execute(Context context)
    {
        var targetPiece = context.CurrentPiece;
        if (targetPiece == null)
        {
            Debug.LogError("No piece selected to clone.");
            return false;
        }

        targetPiece.Speed += 2;
        context.CurrentPlayer.RecordAbilityUse();
        context.CurrentPiece.View.PlayAbilitySound();
        
        Debug.Log($"Piece speed increased to {targetPiece.Speed}");
        return true;
    }
}