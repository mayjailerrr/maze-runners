using MazeRunners;
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

    public void ApplyEffect(Piece piece, TurnManager turnManager)
    {
        var slowTemporaryEffect = new PropertyTemporaryEffect(piece, "Speed", speedReduction, slowTurns);
        turnManager.ApplyTemporaryEffect(slowTemporaryEffect);
        Debug.Log($"{piece.Name} fell into a Slow Trap! Speed reduced for {slowTurns} turns.");
    }
}
