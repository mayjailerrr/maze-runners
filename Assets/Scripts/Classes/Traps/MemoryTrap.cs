using UnityEngine;

public class MemoryTrap : ITrapEffect
{
    private Piece affectedPiece;
    private Context gameContext;

    public string Description => "A memory challenge: fail and suffer a penalty.";

    public MemoryTrap() {}

    public void ApplyEffect(Context context)
    {
        affectedPiece = context.CurrentPiece;
        gameContext = context;
        context.TurnManager.PauseTurns(true);

        MemoryMinigame minigame = Object.FindObjectOfType<MemoryMinigame>();
        if (minigame != null)
        {
            minigame.StartMinigame(OnMinigameResult);
        }
        else
        {
            Debug.LogError("MemoryMinigame not found in scene!");
        }
    }

   private void OnMinigameResult(bool success)
    {
        if (success)
        {
            Debug.Log("Memory trap deactivated!");
            gameContext.TurnManager.PauseTurns(false);
            return;
        }

        Debug.Log("You failed the memory challenge! Losing 1 health point.");

        affectedPiece.TakeDamage(1, gameContext);

        gameContext.TurnManager.PauseTurns(false);
    }

}
