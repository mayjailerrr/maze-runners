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
        context.TurnManager.PauseTurns(true);

        CodeLockMinigame minigame = Object.FindObjectOfType<CodeLockMinigame>();
        if (minigame != null)
        {
            minigame.StartMinigame(OnMinigameResult);
        }
    }

    private void OnMinigameResult(bool success)
    {
        if (success)
        {
            Debug.Log("Trap deactivated successfully!");
            gameContext.TurnManager.PauseTurns(false);
            return;
        }

        Debug.Log("You failed! Piece unarmed for 2 turns.");

        var blockAbilities = new ActionTemporaryEffect(
            affectedPiece,
            () => affectedPiece.AbilitiesBlocked = true,
            () => affectedPiece.AbilitiesBlocked = false,
            stunTurns
        );
        gameContext.TurnManager.ApplyTemporaryEffect(blockAbilities);

        gameContext.TurnManager.PauseTurns(false);
    }
}
