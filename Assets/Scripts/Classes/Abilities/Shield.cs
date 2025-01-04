using System;
using MazeRunners;
using UnityEngine;

public class ShieldAbility : IAbility
{
    public string Description => "Shield the piece from damage for a specified number of turns.";

    private int shieldTurns = 3;

    public bool Execute(Context context)
    {
        var targetPiece = context.CurrentPiece;
        if (targetPiece == null)
        {
            Debug.LogError("No piece selected to clone.");
            return false;
        }

        Debug.Log($"Applying shield to piece {targetPiece.Name} for {shieldTurns} turns.");

        var shieldEffect = new ActionTemporaryEffect(
            targetPiece,
            () => targetPiece.IsShielded = true,  
            () => targetPiece.IsShielded = false,
            shieldTurns
        );

        context.TurnManager.ApplyTemporaryEffect(shieldEffect);

        Debug.Log($"{targetPiece.Name} is now shielded for {shieldTurns} turns.");
        return true;
    }
}
