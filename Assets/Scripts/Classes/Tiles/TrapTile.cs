
using MazeRunners;
using UnityEngine;

public class TrapTile : Tile
{

    public ITrapEffect TrapEffect { get; private set; }

    public TrapTile(int x, int y, ITrapEffect trapEffect) : base(x, y)
    {
        TrapEffect = trapEffect;
    }

    public void ActivateTrap(Piece piece, Context context)
    {
        if (TrapEffect != null)
        {
            TrapEffect?.ApplyEffect(context);
            Debug.Log("I passed by ActivateTrap");
        }
    }
}
