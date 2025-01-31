using MazeRunners;

public class Collectible
{
    public string Name { get; }
    public string Description { get; }
    public int TargetPlayerID { get; private set; } = -1;

    public Collectible(string name, string description)
    {
        Name = name;
        Description = description;
    }
    public void AssignPlayerID(int playerID)
    {
        TargetPlayerID = playerID;
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
