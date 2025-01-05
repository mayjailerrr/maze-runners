using MazeRunners;

public class Collectible
{
    public string Name { get; }
    public string Description { get; }

    public Collectible(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public override bool Equals(object obj)
    {
        if (obj is Collectible other)
        {
            return Name == other.Name;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}
