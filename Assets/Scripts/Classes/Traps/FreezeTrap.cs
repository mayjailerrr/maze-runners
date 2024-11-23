using MazeRunners;
using UnityEngine;
public class FreezeTrap : Trap
{
    public int FreezeTurns { get; private set; } = 1;

    public FreezeTrap()
    {
        Name = "Freeze Trap";
        Description = "This trap freezes the piece that step on it for 1 turn.";
        IsReusable = false;
    }

    public override void Activate(Piece piece)
    {
        piece.CurrentCooldown += FreezeTurns;
        Debug.Log($"{piece.Name} is frozen for {FreezeTurns} turns by a Freeze Trap!");
    }
}