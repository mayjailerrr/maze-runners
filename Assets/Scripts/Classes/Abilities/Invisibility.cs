using System;
using UnityEngine;

public class InvisibilityAbility : IAbility
{
    public string Description => "Makes the piece invisible for a certain number of turns.";
    private int invisibilityTurns = 2;

    public bool Execute(Context context)
    {
        Player currentPlayer = context.CurrentPlayer;
        Piece currentPiece = context.CurrentPiece;

        if (currentPlayer == null || currentPiece == null || currentPiece.View == null)
        {
            Debug.LogError("No valid target pieces or player.");
            return false;
        }

        Action applyInvisibility = () => currentPiece.View.SetVisibility(false, currentPiece);
        Action revertInvisibility = () => currentPiece.View.SetVisibility(true, currentPiece);

        var invisibilityEffect = new ActionTemporaryEffect(
            targetPiece: currentPiece,
            applyAction: applyInvisibility,
            revertAction: revertInvisibility,
            duration: invisibilityTurns
        );

        invisibilityEffect.Apply();

        context.TurnManager.ApplyTemporaryEffect(invisibilityEffect);
        context.CurrentPlayer.RecordAbilityUse();
        context.CurrentPiece.View.PlayAbilitySound();

        return true;
    }
}
