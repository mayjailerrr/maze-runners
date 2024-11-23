using MazeRunners;
using UnityEngine;

public class DamageTrap : Trap
{
    public int Damage { get; private set; } = 1;

    public DamageTrap()
    {
        Name = "Damage Trap";
        Description = "This trap deals damage to the piece that step on it.";
        IsReusable = true;
    }

    public override void Activate(Piece piece)
    {
       // piece.Health -= Damage;
        Debug.Log($"{piece.Name} takes {Damage} damage from a Damage Trap!");
    }
}