public static class PieceFactory
{
    public static List<Piece> CreatePieces(string movieName)
    {
        List<Piece> pieces = new List<Piece>();
        
        switch (movieName)
        {
            case "Spirited Away":
                pieces.Add(new Piece("Chihiro", cooldown: 5, ability: "Invisibility"));
                pieces.Add(new Piece("Haku", cooldown: 3, ability: "Fly"));
                pieces.Add(new Piece("No-Face", cooldown: 4, ability: "Consume"));
                break;

            case "My Neighbor Totoro":
                pieces.Add(new Piece("Totoro", cooldown: 6, ability: "Grow Tree"));
                pieces.Add(new Piece("Catbus", cooldown: 4, ability: "Teleport"));
                pieces.Add(new Piece("Satsuki", cooldown: 3, ability: "Speed Boost"));
                break;

            default:
                Debug.LogError("Movie not found in PieceFactory.");
                break;
        }

        return pieces;
    }
}
