
namespace MazeRunners
{
    public class Tile
    {
        public (int x, int y) Position { get; private set; }
        public Piece OccupyingPiece { get; set; } 

        public bool IsOccupied => OccupyingPiece != null;

        public Tile(int x, int y)
        {
            Position = (x, y);
        }
    }
}
