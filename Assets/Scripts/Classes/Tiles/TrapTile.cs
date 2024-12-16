
using MazeRunners;
using UnityEngine;

public class TrapTile : Tile
{

    public ITrapEffect TrapEffect { get; private set; }

    public TrapTile(int x, int y, ITrapEffect trapEffect) : base(x, y)
    {
        TrapEffect = trapEffect;
    }

    public void ActivateTrap(Piece piece, TurnManager turnManager)
    {
         if (TrapEffect != null)
        {
            Debug.Log($"Trap activated at ({piece.Position.x}, {piece.Position.y}) for piece {piece.Name}!");
            TrapEffect?.ApplyEffect(piece, turnManager);
        }
    }
}
