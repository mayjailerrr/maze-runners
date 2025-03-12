
using UnityEngine;

public class AbsorbDamageAbility : IAbility
{
    public string Description => "Absorbs the damage of the own player pieces";

    public bool Execute(Context context)
    {
        Player currentPlayer = context.CurrentPlayer;

        foreach (Piece piece in currentPlayer.Pieces)
        {
            piece.Health = 3;
            piece.View.PlayAbilityEffect(new Color(1f, 0.4f, 0.4f, 0.8f));
        }

        context.CurrentPlayer.RecordAbilityUse();
        GameEvents.TriggerAbsorbDamageUsed();
        
        Debug.Log($"Player {currentPlayer.ID + 1} pieces have been settled to full health!");
       
        return true;
    }
}