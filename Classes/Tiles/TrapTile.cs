public class TrapTile : Tile
{
    
    public Trap TrapEffect { get; private set; }
    public bool IsTriggered { get; private set; } = false;

    public TrapTile(int x, int y, Trap trapEffect) : base(x, y)
    {
        TrapEffect = trapEffect;
    }

    public void ActivateTrap(Piece piece)
    {
        if (!IsTriggered)
        {
            TrapEffect.Activate(piece);
            IsTriggered = true;
        }
    }

    public void ResetTrap()
    {
        IsTriggered = false;
        TrapEffect.Reset();
    }
}
