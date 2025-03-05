
using UnityEngine;

public class DamageTrap : ITrapEffect
{
    private readonly int damage;
    public string Description => $"Deals {damage} damage to the target piece.";

    public DamageTrap(int damage)
    {
        this.damage = damage;
    }
    public void ApplyEffect(Context context)
    {
        Piece targetPiece = context.CurrentPiece;
        targetPiece.TakeDamage(damage, context);
        context.CurrentPlayer.RecordTrap();
        
        Debug.Log($"{targetPiece.Name} takes {damage} damage from a Damage Trap!");
    }
}