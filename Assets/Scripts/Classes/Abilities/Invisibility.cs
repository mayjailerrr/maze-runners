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

        Action applyInvisibility = () => currentPiece.View.SetVisibility(false, currentPiece);
        Action revertInvisibility = () => currentPiece.View.SetVisibility(true, currentPiece);

        var invisibilityEffect = new ActionTemporaryEffect(
            target: currentPiece,
            applyAction: applyInvisibility,
            revertAction: revertInvisibility,
            duration: invisibilityTurns
        );

        invisibilityEffect.Apply();

        context.TurnManager.ApplyTemporaryEffect(invisibilityEffect);
        context.CurrentPlayer.RecordAbilityUse();
        GameEvents.TriggerInvisibilityUsed();

        Debug.Log($"{currentPiece.Name} is now invisible!");
        
        return true;
    }
}
