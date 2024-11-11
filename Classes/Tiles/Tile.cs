public class Tile
{
    
    public Vector2Int Position { get; private set; }
    public bool IsOccupied { get; set; }

    public Tile(int x, int y)
    {
        Position = new Vector2Int(x, y);
    }

    public virtual void OnEnter(Piece piece) { }

}