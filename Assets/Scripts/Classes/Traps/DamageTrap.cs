using MazeRunners;
using UnityEngine;

public class DamageTrap : ITrapEffect
{
    private readonly int damage;
    public string Description => $"Deals {damage} damage to the target piece.";

    public DamageTrap(int damage)
    {
        this.damage = damage;
    }
    public void ApplyEffect(Piece piece, Context context)
    {
        piece.TakeDamage(damage, context);
        
        Debug.Log($"{piece.Name} takes {damage} damage from a Damage Trap!");
    }
}