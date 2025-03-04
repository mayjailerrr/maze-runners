using UnityEngine;

public class MemoryTrap : ITrapEffect
{
    private readonly int penaltyTurns = 2;
    private Piece affectedPiece;
    private Context gameContext;

    public string Description => "A memory challenge: fail and suffer a penalty.";

    public MemoryTrap() {}

    public void ApplyEffect(Context context)
    {
        affectedPiece = context.CurrentPiece;
        gameContext = context;

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
            Debug.Log("Trampa de memoria superada!");
            return;
        }

        Debug.Log("Fallaste! Penalización aplicada.");

        var penaltyEffect = new PropertyTemporaryEffect(affectedPiece, "Speed", affectedPiece.Speed - 1, penaltyTurns);
        gameContext.TurnManager.ApplyTemporaryEffect(penaltyEffect);
    }
}
