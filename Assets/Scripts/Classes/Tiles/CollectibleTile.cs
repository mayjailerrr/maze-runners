using MazeRunners;
public class CollectibleTile : Tile
{
    public Collectible Collectible { get; set; }

    public CollectibleTile(int x, int y, Collectible collectible) : base(x, y)
    {
        Collectible = collectible;
    }

    public bool Interact(Piece piece, Player player)
    {
        if (Collectible == null) return false;

        bool collected = player.CollectObject(Collectible);
        if (collected)
        {
            CollectibleViewManager.Instance?.MoveToHUD(Collectible);
            Collectible = null;
        }
        return collected;
    }

    public bool CanBeCollectedBy(Player player)
    {
        return Collectible != null && Collectible.TargetPlayerID == player.ID;
    }
}
