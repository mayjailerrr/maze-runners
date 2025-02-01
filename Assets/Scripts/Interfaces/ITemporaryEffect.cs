
public interface ITemporaryEffect
{
    bool HasExpired { get; }
    Piece TargetPiece { get; }
    void Apply();
    void Revert();
    void DecrementDuration();
}
