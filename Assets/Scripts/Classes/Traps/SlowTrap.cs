
using UnityEngine;

public class SlowTrap : ITrapEffect
{
    private readonly int slowTurns;
    private readonly int speedReduction;
    public string Description => $"Reduces the piece speed for {slowTurns} turns.";

    public SlowTrap(int speedReduction, int slowTurns)
    {
        this.speedReduction = speedReduction;
        this.slowTurns = slowTurns;
    }

    public void ApplyEffect(Piece piece, Context context)
    {
        var slowTemporaryEffect = new PropertyTemporaryEffect(piece, "Speed", speedReduction, slowTurns);
        context.TurnManager.ApplyTemporaryEffect(slowTemporaryEffect);
        context.CurrentPlayer.RecordTrap();
        
        Debug.Log($"{piece.Name} fell into a Slow Trap! Speed reduced by {speedReduction} for {slowTurns} turn(s).");
    }
}
