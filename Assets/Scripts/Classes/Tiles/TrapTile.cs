
using MazeRunners;
using UnityEngine;

public class TrapTile : Tile
{
    public ITrapEffect TrapEffect { get; private set; }
    private bool isActivated = true;
    public TrapTile(int x, int y, ITrapEffect trapEffect) : base(x, y)
    {
        TrapEffect = trapEffect;
    }

    public void ActivateTrap(Piece piece, Context context)
    {
        if (!isActivated || piece.IsShielded) return;

        if (TrapEffect != null)
        {
            GameEvents.TriggerTrap();
            TrapEffect?.ApplyEffect(context);
            isActivated = false;
        }
    }
}
