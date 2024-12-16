using MazeRunners;

public interface ITrapEffect
{
    void ApplyEffect(Piece piece, TurnManager turnManager);
    string Description { get; }
}