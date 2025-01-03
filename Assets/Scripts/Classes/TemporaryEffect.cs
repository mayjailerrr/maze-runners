using System.Reflection;
using System;

public class TemporaryEffect
{
    private readonly Piece _piece;
    private readonly string _propertyName;
    private readonly int _originalValue;
    private readonly int _modifiedValue;
    private int _remainingTurns;

    public bool HasExpired => _remainingTurns <= 0;

    public TemporaryEffect(Piece piece, string propertyName, int modifiedValue, int duration)
    {
        _piece = piece;
        _propertyName = propertyName;

        var property = piece.GetType().GetProperty(propertyName);
        if (property == null)
        {
            throw new ArgumentException($"Property '{propertyName}' not found in Piece.");
        }

        _originalValue = (int)property.GetValue(piece);
        _modifiedValue = modifiedValue;
        _remainingTurns = duration;
    }

    public void Apply()
    {
        SetPropertyValue(_modifiedValue);
    }

    public void Revert()
    {
        SetPropertyValue(_originalValue);
    }

    public void DecrementDuration()
    {
        _remainingTurns--;
    }

    private void SetPropertyValue(int value)
    {
        var property = _piece.GetType().GetProperty(_propertyName);
        if (property != null)
        {
            property.SetValue(_piece, value);
        }
    }
}
