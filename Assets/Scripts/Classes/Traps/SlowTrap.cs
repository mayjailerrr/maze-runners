using MazeRunners;
using System;

public class SlowTrap : Trap
{
    public int SlowTurns { get; private set; } = 2;


    public SlowTrap()
    {
        Name = "Slow Trap";
        Description = "This trap slows down the piece that step on it for 2 turns.";
        IsReusable = true;
    }

    public override void Activate(Piece piece)
    {
        piece.Speed = Math.Max(1, piece.Speed - SlowTurns);
        Console.WriteLine($"{piece.Name} fell into a Slow Trap! Speed reduced by {SlowTurns}");
    }
}