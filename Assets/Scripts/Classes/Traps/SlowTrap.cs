using MazeRunners;
using System;

public class SlowTrap : ITrapEffect
{
    public int SlowTurns { get; private set; } = 2;


    public SlowTrap()
    {
      
    }

    public void ApplyEffect(Piece piece)
    {
        piece.Speed = Math.Max(1, piece.Speed - SlowTurns);
        Console.WriteLine($"{piece.Name} fell into a Slow Trap! Speed reduced by {SlowTurns}");
    }
}