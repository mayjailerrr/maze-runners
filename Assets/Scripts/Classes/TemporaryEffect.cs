using System.Reflection;
using System;

public class TemporaryEffect
{
    private readonly Piece piece;
    private readonly string propertyName;
    private readonly int originalValue;
    private readonly int modifiedValue;
    private int remainingTurns;

    public bool HasExpired => remainingTurns <= 0;

    public TemporaryEffect(Piece piece, string propertyName, int modifiedValue, int duration)
    {
        this.piece = piece;
        this.propertyName = propertyName;

        var property = piece.GetType().GetProperty(propertyName);
        if (property == null)
        {
            throw new ArgumentException($"Property '{propertyName}' not found in Piece.");
        }

        originalValue = (int)property.GetValue(piece);
        this.modifiedValue = modifiedValue;
        this.remainingTurns = duration;
    }

    public void Apply()
    {
        SetPropertyValue(modifiedValue);
    }

    public void Revert()
    {
        SetPropertyValue(originalValue);
    }

    public void DecrementDuration()
    {
        remainingTurns--;
    }

    private void SetPropertyValue(int value)
    {
        var property = piece.GetType().GetProperty(propertyName);
        if (property != null)
        {
            property.SetValue(piece, value);
        }
    }
}
