public abstract class Piece
{
    // Ficha: Clase base para las fichas, con características como habilidad, velocidad y enfriamiento.

    // Propiedades: Speed, Cooldown, Habilidad, Position
    // Métodos: Move(), UseHabilidad()
    // Subclases posibles: VacaParacaidista, SoldadoTanque (o cualquier otra ficha específica con habilidades).

    public string Name { get; protected set; }
    public int Speed { get; protected set; }
    public int Cooldown { get; protected set; }
    public int CurrentCooldown { get; private set; }
    public (int x, int y) Position { get; set; }

    public Piece (string name, int speed, int cooldown)
    {
        Name = name;
        Speed = speed;
        Cooldown = cooldown;
        CurrentCooldown = 0;
    }

    public abstract void UseAbility();

    public void Move(int newX, int newY)
    {
        Position = (newX, newY);
        Console.WriteLine($"{Name} moved to ({newX}, {newY})");

    }

    public void UpdateCooldown()
    {
        if (CurrentCooldown > 0) CurrentCooldown--;
    }

    public bool CanUseAbility()
    {
        return CurrentCooldown == 0;
    }

    protected void ActivateAbility()
    {
        CurrentCooldown = Cooldown;
    }

}