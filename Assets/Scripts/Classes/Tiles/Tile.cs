using UnityEngine;

namespace MazeRunners
{
    public class Tile
    {
        public (int x, int y) Position { get; private set; }
        public bool IsOccupied { get; set; }

        public Tile(int x, int y)
        {
            Position = (x, y);
        }

        public virtual void OnEnter(Piece piece) { }

    }
}
