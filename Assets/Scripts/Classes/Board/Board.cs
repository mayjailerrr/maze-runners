using System;
using MazeRunners;
using System.Collections.Generic;
public class Board
{
    public int Size { get; private set; }
    public Tile[,] TileGrid { get; private set; }
    public Piece[,] PieceGrid { get; private set; }

    private BoardGenerator generator;
    private BoardRules rules;

    private List<Tile> cachedNeutralTiles;
    private bool isTileCacheDirty = true;

    public event Action<Tile> OnTileChanged;
    public event Action<Piece, (int x, int y)> OnPieceMoved;

    public Board(int size, List<Collectible> collectibles)
    {
        Size = size;
        TileGrid = new Tile[size, size];
        PieceGrid = new Piece[size, size];

        // Inicialización de subclases
        generator = new BoardGenerator(this);
        rules = new BoardRules(this);

        generator.GenerateBoard(collectibles);
    }

    public bool MovePiece(Piece piece, int targetX, int targetY)
    {
        if (rules.IsValidMove(piece, targetX, targetY))
        {
            PieceGrid[piece.Position.x, piece.Position.y] = null;
            PieceGrid[targetX, targetY] = piece;

            piece.UpdatePosition((targetX, targetY));
            OnPieceMoved?.Invoke(piece, (targetX, targetY));

            return true;
        }
        return false;
    }

    public void ReplaceTile(int x, int y, Tile newTile)
    {
        if (!rules.IsWithinBounds(x, y))
        {
            throw new ArgumentOutOfRangeException($"Position ({x}, {y}) is out of bounds.");
        }

        TileGrid[x, y] = newTile;
        isTileCacheDirty = true; // Actualizar caché de tiles neutrales
        OnTileChanged?.Invoke(newTile);
    }

    public List<Tile> GetNeutralTiles()
    {
        if (isTileCacheDirty)
        {
            cachedNeutralTiles = new List<Tile>();

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    var tile = TileGrid[x, y];
                    if (tile != null && !(tile is ObstacleTile) && !(tile is TrapTile) && !(tile is CollectibleTile))
                    {
                        cachedNeutralTiles.Add(tile);
                    }
                }
            }

            isTileCacheDirty = false;
        }

        return cachedNeutralTiles;
    }

     public IEnumerable<Tile> GetNeighbours(Tile tile)
    {
        int x = tile.Position.x;
        int y = tile.Position.y;

        int [,] directions = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int nx = x + directions[i, 0];
            int ny = y + directions[i, 1];

            if (rules.IsWithinBounds(nx, ny))
            {
                yield return TileGrid[nx, ny];
            }
        }
    }
}