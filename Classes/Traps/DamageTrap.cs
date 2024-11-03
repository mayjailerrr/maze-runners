public class DamageTrap : Trap
{
    public int Damage { get; private set; } = 1;

    public DamageTrap()
    {
        Name = "Damage Trap";
        Description = "This trap deals 1 damage to the piece that step on it.";
    }

    public override void Activate(Piece piece)
    {
        piece.Health -= Damage;
        Console.WriteLine($"{piece.Name} takes {Damage} damage from a Damage Trap!");
    }
}