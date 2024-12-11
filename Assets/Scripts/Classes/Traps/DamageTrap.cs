using MazeRunners;
using UnityEngine;

public class DamageTrap : ITrapEffect
{
    public int Damage { get; private set; } = 1;

    public DamageTrap()
    {
    
    }

    public void ApplyEffect(Piece piece)
    {
       // piece.Health -= Damage;
        Debug.Log($"{piece.Name} takes {Damage} damage from a Damage Trap!");
    }
}