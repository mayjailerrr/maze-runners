using System;

public class PropertyTemporaryEffect : ITemporaryEffect
{
    private readonly object _target;
    private readonly string _propertyName;
    private readonly object _originalValue;
    private readonly object _modifiedValue;
    private int _remainingTurns;

    public bool HasExpired => _remainingTurns <= 0;
    public object Target => _target;

    public PropertyTemporaryEffect(object target, string propertyName, object modifiedValue, int duration)
    {
        _target = target ?? throw new ArgumentNullException(nameof(target));
        _propertyName = propertyName;

        var property = _target.GetType().GetProperty(propertyName);
        if (property == null)
        {
            throw new ArgumentException($"Property '{propertyName}' not found in target object.");
        }

        _originalValue = property.GetValue(_target);
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
        var property = _target.GetType().GetProperty(_propertyName);
        if (property != null)
        {
            property.SetValue(_target, value);
        }
    }
}
