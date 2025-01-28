
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

        public virtual void OnEnter(Piece piece)
        {
            // Lógica personalizada al entrar en un tile
        }

        public virtual void OnExit(Piece piece)
        {
            // Lógica personalizada al salir de un tile
        }
    }
}
