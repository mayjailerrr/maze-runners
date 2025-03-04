using UnityEngine;

public class CodeLockTrap : ITrapEffect
{
    private readonly int stunTurns = 2;
    private Piece affectedPiece;
    private Context gameContext;

    public string Description => "A pattern lock must be solved within 5 seconds or the piece is stunned.";

    public CodeLockTrap() {}

    public void ApplyEffect(Context context)
    {
        affectedPiece = context.CurrentPiece;
        gameContext = context;

        CodeLockMinigame minigame = Object.FindObjectOfType<CodeLockMinigame>();
        if (minigame != null)
        {
            minigame.StartMinigame(OnMinigameResult);
        }
        else
        {
            Debug.LogError("CodeLockMinigame not found in scene!");
        }
    }

    private void OnMinigameResult(bool success)
    {
        if (success)
        {
            Debug.Log("Trap deactivated successfully!");
            return;
        }

        Debug.Log("You failed! Piece unarmed for 2 turns.");

        var freezeEffect = new PropertyTemporaryEffect(affectedPiece, "Speed", 0, stunTurns);
        gameContext.TurnManager.ApplyTemporaryEffect(freezeEffect);

        var blockAbilities = new ActionTemporaryEffect(
            affectedPiece,
            () => affectedPiece.AbilitiesBlocked = true,
            () => affectedPiece.AbilitiesBlocked = false,
            stunTurns
        );
        gameContext.TurnManager.ApplyTemporaryEffect(blockAbilities);
    }
}
