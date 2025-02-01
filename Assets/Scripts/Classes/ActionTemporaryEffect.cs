using System;


public class ActionTemporaryEffect : ITemporaryEffect
{
    private readonly Piece _targetPiece;
    private readonly Action _applyAction;
    private readonly Action _revertAction;
    private int _remainingTurns;

    public bool HasExpired => _remainingTurns <= 0;
    public Piece TargetPiece => _targetPiece;

    public ActionTemporaryEffect(Piece targetPiece, Action applyAction, Action revertAction, int duration)
    {
        _targetPiece = targetPiece ?? throw new ArgumentNullException(nameof(targetPiece));
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
