
using UnityEngine;
using System.Linq;

public class AbsorbAbilitiesAbility : IAbility
{
    public string Description => "Absorbs the ability of the piece it attacks.";
    private int selectedPieceIndex = -1;

    public bool Execute(Context context)
    {
        Player nextPlayer = context.TurnManager.GetNextPlayer(context.CurrentPlayer);

        var validTargets = nextPlayer.Pieces.Where(piece => !piece.IsShielded).ToList();

        System.Random random = new System.Random();
        selectedPieceIndex = random.Next(0, validTargets.Count);

        Piece targetPiece = validTargets[selectedPieceIndex];
        Piece currentPiece = context.CurrentPiece;

        if (targetPiece.Ability == null)
        {
            Debug.LogWarning("Target piece has no ability to absorb.");
            return false;
        }

        currentPiece.Ability = targetPiece.Ability;
        
        context.CurrentPlayer.RecordAbilityUse();
        currentPiece.View.PlayAbilityEffect(new Color(1f, 0.65f, 0f, 0.8f)); 
        GameEvents.TriggerAbsorbUsed();

        Debug.Log($"Ability absorbed: {targetPiece.Ability.Description}");
    
        return true;
    }
}