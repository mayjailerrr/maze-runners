public class Player
{
    public int ID { get; private set; }
    public List<Piece> Pieces { get; private set; }

    public Player (int id)
    {
        ID = id;
        Pieces = new List<Piece>();
    }

    public void AddPiece(Piece piece)
    {
        Pieces.Add(piece);
    }

    public Piece ChoosePiece(int index)
    {
        if (index >= 0 && index < Pieces.Count)
        {
            return Pieces[index];
        }
       
        Console.WriteLine("Invalid piece index chosen");
        return null;
    }

    public void MovePiece(Piece piece, int newX, int newY, Board board)
    {
        if (board.IsValidMove(piece, newX, newY))
        {
            piece.Move(newX, newY);
        }
        else
        {
            Console.WriteLine("Invalid move");
        }
    }

    public void UsePieceAbility(Piece piece)
    {
        if (piece.CanUseAbility()) piece.UseAbility();
        else Console.WriteLine($"{piece.Name}'s ability is still on cooldown");
    }

}

