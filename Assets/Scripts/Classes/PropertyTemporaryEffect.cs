using System;


public class PropertyTemporaryEffect : ITemporaryEffect
{
    private readonly Piece _piece;
    private readonly string _propertyName;
    private readonly object _originalValue;
    private readonly object _modifiedValue;
    private int _remainingTurns;

    public bool HasExpired => _remainingTurns <= 0;
    public Piece TargetPiece => _piece;

    public PropertyTemporaryEffect(Piece piece, string propertyName, object modifiedValue, int duration)
    {
        _piece = piece ?? throw new ArgumentNullException(nameof(piece));
        _propertyName = propertyName;

        var property = piece.GetType().GetProperty(propertyName);
        if (property == null)
        {
            throw new ArgumentException($"Property '{propertyName}' not found in Piece.");
        }

        _originalValue = property.GetValue(piece);
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

    private void SetPropertyValue(object value)
    {
        var property = _piece.GetType().GetProperty(_propertyName);
        if (property != null)
        {
            property.SetValue(_piece, value);
        }
    }
}
