using System;

public class ActionTemporaryEffect : ITemporaryEffect
{
    private readonly object _target;
    private readonly Action _applyAction;
    private readonly Action _revertAction;
    private int _remainingTurns;

    public bool HasExpired => _remainingTurns <= 0;
    public object Target => _target;

    public ActionTemporaryEffect(object target, Action applyAction, Action revertAction, int duration)
    {
        _target = target ?? throw new ArgumentNullException(nameof(target));
        _applyAction = applyAction ?? throw new ArgumentNullException(nameof(applyAction));
        _revertAction = revertAction ?? throw new ArgumentNullException(nameof(revertAction));
        _remainingTurns = duration;
    }

    public void Apply()
    {   
        _applyAction.Invoke();
    }

    public void Revert()
    {
        _revertAction.Invoke();
    }

    public void DecrementDuration()
    {
        _remainingTurns--;
    }
}
