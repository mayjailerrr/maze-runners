

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
}
