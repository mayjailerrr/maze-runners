public class SlowTrap : Trap
{
    public int SpeedReduction { get; private set; } = 2;
    public SlowTrap()
    {
        Name = "Slow Trap";
        Description = "This trap slows down the piece that step on it for 2 turns.";
    }

    public override void Activate(Piece piece)
    {
        piece.Speed = Math.Max(1, piece.Speed - SpeedReduction);
        Console.WriteLine($"{piece.Name} fell into a Slow Trap! Speed reduced by {SpeedReduction}");
    }
}