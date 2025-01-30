using System;
using MazeRunners;
using UnityEngine;

public class HealthDamageAbility : IAbility
{
    private int selectedPieceIndex;
    public string Description => "Deals damage to the target piece.";

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

        if (targetPiece == null)
        {
            Debug.LogError("No target piece selected to deal damage.");
            return false;
        }

        int damage = 3;
        targetPiece.TakeDamage(damage, context);

        return true;
    }
}