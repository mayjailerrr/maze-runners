using System;
using System.Collections.Generic;
using MazeRunners;
using UnityEngine;

public class AbsorbAbilitiesAbility : IAbility
{
    public string Description => "Absorbs the ability of the piece it attacks.";
    private int selectedPieceIndex = -1;

    public bool Execute(Context context)
    {
        Player nextPlayer = context.TurnManager.GetNextPlayer(context.CurrentPlayer);

        if (nextPlayer == null || nextPlayer.Pieces.Count == 0)
        {
            Debug.LogError("No valid target pieces.");
            return false;
        }

        System.Random random = new System.Random();
        selectedPieceIndex = random.Next(0, 3);

        Piece targetPiece = nextPlayer.Pieces[selectedPieceIndex];
        Debug.Log($"Target piece: {targetPiece?.Name}");

        if (targetPiece.Ability == null)
        {
            Debug.LogWarning("Target piece has no ability to absorb.");
            return false;
        }

        context.CurrentPiece.Ability = targetPiece.Ability;
        context.CurrentPlayer.RecordAbilityUse();

        Debug.Log($"Ability absorbed: {targetPiece.Ability.Description}");
        return true;
    }
}