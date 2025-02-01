

public interface ITrapEffect
{
    void ApplyEffect(Piece piece, Context context);
    string Description { get; }
}