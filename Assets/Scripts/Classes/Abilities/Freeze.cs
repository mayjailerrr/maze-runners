
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

        var validTargets = nextPlayer.Pieces.Where(piece => !piece.IsShielded).ToList();

        if (validTargets.Count == 0)
        {
            Debug.LogWarning("All target pieces are shielded. No freeze applied.");
            return false;
        }

        System.Random random = new System.Random();
        selectedPieceIndex = random.Next(0, validTargets.Count);

        Piece targetPiece = validTargets[selectedPieceIndex];
        Piece currentPiece = context.CurrentPiece;

        int freezeTurns = 3;
        var turnManager = context.TurnManager;

        targetPiece.View.ShowFreezeIndicator(targetPiece);

        var freezeMovementEffect = new PropertyTemporaryEffect(targetPiece, "Speed", 0, freezeTurns);
        turnManager.ApplyTemporaryEffect(freezeMovementEffect);

        var freezeAbilitiesEffect = new ActionTemporaryEffect(
            targetPiece,
            () => targetPiece.AbilitiesBlocked = true,
            () =>
            {
                targetPiece.AbilitiesBlocked = false;
                targetPiece.View.HideFreezeIndicator(targetPiece);
            },
            freezeTurns
        );
        turnManager.ApplyTemporaryEffect(freezeAbilitiesEffect);

        context.CurrentPlayer.RecordAbilityUse();
        currentPiece.View.PlayAbilityEffect(new Color(0.68f, 0.85f, 1f, 0.8f));
        GameEvents.TriggerFreezeUsed();
        
        Debug.Log($"Piece {targetPiece.Name} has been frozen for {freezeTurns} turns.");
       
        return true;
    }
}
