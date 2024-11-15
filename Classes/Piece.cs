public abstract class Piece
{

    public string Name { get; protected set; }
    public int Speed { get; protected set; }
    public int Cooldown { get; protected set; }
    public int CurrentCooldown { get; private set; }
    public (int x, int y) Position { get; set; }
    public string Movie { get; private set; }

    public Piece (string name, int speed, int cooldown, string movie)
    {
        Name = name;
        Speed = speed;
        Cooldown = cooldown;
        CurrentCooldown = 0;
        Movie = movie;
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


