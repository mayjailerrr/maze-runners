using System;
using MazeRunners;
using UnityEngine;

public class FreezeAbility : IAbility
{
    private int selectedPieceIndex;
    public string Description => "Freezes the target piece, preventing movement and abilities for a specified number of turns.";

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
            Debug.LogError("No target piece selected to freeze.");
            return false;
        }

        int freezeTurns = 2;
        var turnManager = context.TurnManager;

        turnManager.ApplyTemporaryEffect(targetPiece, "Speed", 0, freezeTurns);

        Debug.Log($"Piece {targetPiece.Name} has been frozen for {freezeTurns} turns.");
        return true;
    }
}
