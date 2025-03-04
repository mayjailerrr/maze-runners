

public class Collectible
{
    public CollectibleName Name { get; }
    public string Description { get; }
    public int TargetPlayerID { get; private set; } = -1;

    public Collectible(CollectibleName name, string description)
    {
        Name = name;
        Description = description;
    }
    public void AssignPlayerID(int playerID)
    {
        TargetPlayerID = playerID;
    }
}
