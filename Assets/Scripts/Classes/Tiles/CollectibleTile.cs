using MazeRunners;
using UnityEngine;
public class CollectibleTile : Tile
{
    public Collectible Collectible { get; }

    public CollectibleTile(int x, int y, Collectible collectible) : base(x, y)
    {
        Collectible = collectible;
    }

    public void Interact(Piece piece, Player player)
    {
        player.CollectObject(Collectible);
    }
}
