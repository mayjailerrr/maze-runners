using MazeRunners;
using UnityEngine;
public class FreezeTrap : ITrapEffect
{
    private readonly int freezeTurns;
    public string Description => $"Freezes the piece for {freezeTurns} turns.";

    public FreezeTrap(int freezeTurns)
    {
        this.freezeTurns = freezeTurns;
    }

    public void ApplyEffect(Piece piece, TurnManager turnManager)
    {
        var freezeTemporaryEffect = new PropertyTemporaryEffect(piece, "Speed", 0, freezeTurns);
        turnManager.ApplyTemporaryEffect(freezeTemporaryEffect);

        Debug.Log($"{piece.Name} is frozen for {freezeTurns} turns by a Freeze Trap!");
    }
}