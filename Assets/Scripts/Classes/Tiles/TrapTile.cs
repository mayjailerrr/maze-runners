
using MazeRunners;

public class TrapTile : Tile
{

    public ITrapEffect TrapEffect { get; private set; }
  
    public TrapTile(int x, int y, ITrapEffect trapEffect) : base(x, y)
    {
        TrapEffect = trapEffect;
    }

    public void ActivateTrap(Piece piece)
    {
        TrapEffect?.ApplyEffect(piece);
    }

    // public void ResetTrap()
    // {
    //     IsTriggered = false;
    //     TrapEffect.Reset();
    // }
}
