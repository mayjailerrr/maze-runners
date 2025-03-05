using System;

public interface ITemporaryEffect
{
    bool HasExpired { get; }
    object Target { get; }
    void Apply();
    void Revert();
    void DecrementDuration();
}
