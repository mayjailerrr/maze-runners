
using UnityEngine;
using System.Linq;

public class HealthDamageAbility : IAbility
{
    private int selectedPieceIndex;
    public string Description => "Deals damage to the target piece.";

    public bool Execute(Context context)
    {
        Player nextPlayer = context.TurnManager.GetNextPlayer(context.CurrentPlayer);

        var validTargets = nextPlayer.Pieces.Where(piece => !piece.IsShielded).ToList();

        System.Random random = new System.Random();
        selectedPieceIndex = random.Next(0, validTargets.Count);

        Piece targetPiece = validTargets[selectedPieceIndex];
        Piece currentPiece = context.CurrentPiece;

        int damage = 1;
        targetPiece.TakeDamage(damage, context);

        context.CurrentPlayer.RecordAbilityUse();
        currentPiece.View.PlayAbilityEffect(new Color(0.65f, 0.16f, 0.16f, 0.8f));
        GameEvents.TriggerHealthDamageUsed();

        return true;
    }
}