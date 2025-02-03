
using UnityEngine;

public class SpeedBoostAbility : IAbility
{
    public string Description => "Increases the piece's speed for one turn.";

    public bool Execute(Context context)
    {
        var targetPiece = context.CurrentPiece;
        var currentPiece = context.CurrentPiece;
        if (targetPiece == null)
        {
            Debug.LogError("No piece selected to clone.");
            return false;
        }

        targetPiece.Speed += 2;

        context.CurrentPlayer.RecordAbilityUse();
        currentPiece.View.PlayAbilityEffect(new Color(0.6f, 1f, 0.6f, 0.8f));
        currentPiece.View.PlayAbilitySound();
        
        Debug.Log($"Piece speed increased to {targetPiece.Speed}");
        return true;
    }
}