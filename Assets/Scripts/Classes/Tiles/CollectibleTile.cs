using MazeRunners;
using UnityEngine;
public class CollectibleTile : Tile
{
    public Collectible Collectible { get; }
    public int TargetPlayerID;

    public CollectibleTile(int x, int y, Collectible collectible) : base(x, y)
    {
        Collectible = collectible;
    }

    public bool Interact(Piece piece, Player player)
    {
        return player.CollectObject(Collectible);
    }

    public bool CanBeCollectedBy(Player player)
    {
        return player.ID == TargetPlayerID;
    }
}
