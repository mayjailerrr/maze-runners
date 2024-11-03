public class Tile
{
      // Tile: Clase base para cada casilla del tablero.

    // Propiedades: Position, IsOccupied, Type
    // Subclases: ObstacleTile, TrapTile, ExitTile
    // Métodos adicionales en subclases, como ActivateTrap() en TrapTile.


    //gepeto
    // Tile: Clase que representa una casilla del tablero.
    // Propiedades: Position, IsOccupied, Piece
    // Métodos: SetPiece(), RemovePiece()
    public Vector2Int Position { get; private set; }
    public bool IsOccupied { get; set; }

    public Tile(int x, int y)
    {
        Position = new Vector2Int(x, y);
    }

    public virtual void OnEnter(Piece piece) { }

}