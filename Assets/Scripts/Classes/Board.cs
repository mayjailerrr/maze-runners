using System.Collections.Generic;
using MazeRunners;
using UnityEngine;
using System;
using System.Linq;


public class Board
{
    public int Size { get; private set; }

    public Tile[,] grid;
    private List<Piece> pieces = new List<Piece>();
    private List<Collectible> collectibles;

    public void AddPiece(Piece piece)
    {
        if (piece != null)
        {
            pieces.Add(piece);
        }
    }

    public IEnumerable<Piece> GetAllPieces()
    {
        return pieces;
    }

    public void PlacePiecesRandomly(IEnumerable<Piece> piecesToPlace)
    {
        System.Random random = new System.Random();
        List<Tile> neutralTiles = new List<Tile>();


        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                Tile tile = grid[x, y];
                if (!(tile is ObstacleTile) && !(tile is TrapTile) && !(tile is CollectibleTile))
                {
                    neutralTiles.Add(tile);
                }
            }
        }

        foreach (Piece piece in piecesToPlace)
        {
            if (neutralTiles.Count == 0)
            {
                Debug.LogError("No more neutral tiles available to place pieces.");
                return;
            }

            int randomIndex = random.Next(neutralTiles.Count);
            Tile selectedTile = neutralTiles[randomIndex];
            neutralTiles.RemoveAt(randomIndex);

           
            piece.Position = (selectedTile.Position.x, selectedTile.Position.y);
            AddPiece(piece);

            Debug.Log($"Piece {piece.Name} placed at ({selectedTile.Position.x}, {selectedTile.Position.y}).");
        }
    }

    public Tile GetRandomTile()
    {
        List<Tile> neutralTiles = new List<Tile>();

        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                Tile tile = grid[x, y];
                if (!(tile is ObstacleTile) && !(tile is TrapTile) && !(tile is CollectibleTile))
                {
                    neutralTiles.Add(tile);
                }
            }
        }

        if (neutralTiles.Count == 0)
        {
            Debug.LogError("No neutral tiles available.");
            return null;
        }

        System.Random random = new System.Random();
        int randomIndex = random.Next(neutralTiles.Count);
        return neutralTiles[randomIndex];
    }
    
    public Board(int size, List<Collectible> collectibles)
    {
        Size = size;
        this.collectibles = collectibles;
        grid = new Tile[size, size];
        GenerateBoard();
    }

    public void GenerateBoard()
    {
        CreateEmptyGrid();
        PlaceObstacles();
        PlaceTraps();
        EnsureReachability();
        PlaceCollectibles(collectibles);
    }

    private void CreateEmptyGrid()
    {
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                grid[x, y] = new Tile(x, y);
            }
        }
    }

    private bool CanPlaceTile(int x, int y, Func<Tile, bool> additionalChecks = null)
    {
        if (grid[x, y] is ObstacleTile || grid[x, y] is TrapTile || grid[x, y] is CollectibleTile)
            return false;

        if (additionalChecks != null && !additionalChecks(grid[x, y]))
            return false;

        return true;
    }

    private bool CanPlaceObstacle(int x, int y)
    {
        return CanPlaceTile(x, y, tile =>
        {
            Tile originalTile = grid[tile.Position.x, tile.Position.y];
            grid[tile.Position.x, tile.Position.y] = new ObstacleTile(tile.Position.x, tile.Position.y);
            bool isReachable = CheckMazeConnectivity();
            grid[tile.Position.x, tile.Position.y] = originalTile;
            return isReachable;
        });
    }

    private bool CanPlaceTrapOrCollectible(int x, int y)
    {
        return CanPlaceTile(x, y, tile =>
        {
            foreach (Tile neighbor in GetNeighbours(tile))
            {
                if (!(neighbor is ObstacleTile)) return true;
            }
            return false;
        });
    }

    private bool[,] PerformBFS(Tile startTile, Func<Tile, bool> canVisit = null)
    {
        bool[,] visited = new bool[Size, Size];
        Queue<Tile> queue = new Queue<Tile>();

        queue.Enqueue(startTile);
        visited[startTile.Position.x, startTile.Position.y] = true;

        while (queue.Count > 0)
        {
            Tile currentTile = queue.Dequeue();

            foreach (Tile neighbour in GetNeighbours(currentTile))
            {
                int nx = neighbour.Position.x;
                int ny = neighbour.Position.y;

                if (!visited[nx, ny] && (canVisit == null || canVisit(neighbour)))
                {
                    visited[nx, ny] = true;
                    queue.Enqueue(neighbour);
                }
            }
        }

        return visited;
    }

    private bool CheckMazeConnectivity()
    {
        Tile startTile = GetFirstNonObstacleTile();
        if (startTile == null) return false;

        bool[,] visited = PerformBFS(startTile, tile => !(grid[tile.Position.x, tile.Position.y] is ObstacleTile));

        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                if (!(grid[x, y] is ObstacleTile) && !visited[x, y])
                    return false;
            }
        }

        return true;
    }
  
    public void EnsureReachability()
    {
        Tile startTile = GetFirstNonObstacleTile();
        if (startTile == null) return;

        bool[,] visited = PerformBFS(startTile);

        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                if (!visited[x, y] && !(grid[x, y] is CollectibleTile))
                {
                    grid[x, y] = new Tile(x, y);
                }
            }
        }
    }

    private Tile GetFirstNonObstacleTile()
    {
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                if (!(grid[x, y] is ObstacleTile))
                {
                    return grid[x, y];
                }
            }
        }
        return null; 
    }

    private IEnumerable<Tile> GetNeighbours(Tile tile)
    {
        int x = tile.Position.x;
        int y = tile.Position.y;

        int [,] directions = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int nx = x + directions[i, 0];
            int ny = y + directions[i, 1];

            if (IsWithinBounds(nx, ny))
            {
                yield return grid[nx, ny];
            }
        }
    }

    public bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < Size && y >= 0 && y < Size;
    }

    public Tile GetTileAtPosition(int x, int y)
    {
        if (IsWithinBounds(x, y)) return grid[x, y];

        return null;

    }

    public bool IsValidMove(Piece piece, int targetX, int targetY)
    {
        if (!IsWithinBounds(targetX, targetY)) return false;

        Tile targetTile = GetTileAtPosition(targetX, targetY);

        return !(targetTile is null) && !(targetTile is ObstacleTile);
    }

    private void PlaceTiles<T>(int count, Func<int, int, bool> canPlace, Func<int, int, T> createTile) where T : Tile
    {
        System.Random random = new System.Random();

        for (int i = 0; i < count; i++)
        {
            int x, y;

            do
            {
                x = random.Next(0, Size);
                y = random.Next(0, Size);
            } while (!canPlace(x, y));

            grid[x, y] = createTile(x, y);
        }
    }

    public void PlaceObstacles()
    {
        int obstacleCount = (int)(Size * Size * 0.4f);
        PlaceTiles(obstacleCount, CanPlaceObstacle, (x, y) => new ObstacleTile(x, y));
    }

    public void PlaceTraps()
    {
        int trapCount = (int)(Size * Size * 0.1f);
        var predefinedTraps = TrapFactory.GetPredefinedTraps(trapCount).ToList();
        System.Random random = new System.Random();

        PlaceTiles(trapCount, CanPlaceTrapOrCollectible, (x, y) => 
        {
            var trapEffect = predefinedTraps[random.Next(predefinedTraps.Count)];
            return new TrapTile(x, y, trapEffect);
        });
    }

   public void PlaceCollectibles(List<Collectible> collectibles)
    {
        Debug.Log($"Placing {collectibles.Count} collectibles.");
        foreach (var collectible in collectibles)
        {
            PlaceTiles(1, 
                (x, y) => CanPlaceTile(x, y), 
                (x, y) => {
                    Debug.Log($"Placing collectible: {collectible.Name} at ({x}, {y}).");
                    return new CollectibleTile(x, y, collectible);
                });
        }
    }
}


