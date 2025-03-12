
using System.Collections.Generic;
using MazeRunners;
using UnityEngine;

public class Board
{
    public int Size { get; private set; }
    public Tile[,] TileGrid;

    public Board(int size)
    {
        Size = size;
        TileGrid = new Tile[size, size];
    }


    public Tile GetTileAtPosition(int x, int y)
    {
        if (IsWithinBounds(x, y)) return TileGrid[x, y];

        return null;
    }

    public Tile GetRandomTile()
    {
        List<Tile> neutralTiles = new List<Tile>();

        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                Tile tile = TileGrid[x, y];
                if (!(tile is ObstacleTile) && !(tile is TrapTile) && !(tile is CollectibleTile) && !tile.IsOccupied)
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

    public IEnumerable<Tile> GetNeighbours(Tile tile)
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
                yield return TileGrid[nx, ny];
            }
        }
    }

    public bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < Size && y >= 0 && y < Size;
    }

    public bool IsValidMove(int targetX, int targetY)
    {
        if (!IsWithinBounds(targetX, targetY)) return false;

        Tile targetTile = GetTileAtPosition(targetX, targetY);

        if (targetTile == null || targetTile.IsOccupied)
        {
            return false;
        }

        if (targetTile is ObstacleTile)
        {
            return false;
        }

        return true;
    }

    public void ReplaceTile(int x, int y, Tile newTile)
    {
        if (!IsWithinBounds(x, y))
        {
            Debug.LogError($"Position ({x}, {y}) is out of bounds.");
            return;
        }

        TileGrid[x, y] = newTile;
    }

    public void CleanPreviousTile(int x, int y)
    {
        TileGrid[x, y].OccupyingPiece = null;
    }

    public (int x, int y)? FindNearestFreeTile((int x, int y) start)
    {
        var directions = new (int x, int y)[]
        {
            (0, 1), (1, 0), (0, -1), (-1, 0), 
            (1, 1), (1, -1), (-1, 1), (-1, -1)
        };

        var visited = new HashSet<(int, int)>();
        var queue = new Queue<((int x, int y) position, int distance)>();
        queue.Enqueue((start, 0));

        while (queue.Count > 0)
        {
            var (current, _) = queue.Dequeue();

            if (visited.Contains(current)) continue;
            visited.Add(current);

            if (IsWithinBounds(current.x, current.y))
            {
                var tile = GetTileAtPosition(current.x, current.y);

                bool isTileFree = tile != null && !(tile is TrapTile) && !(tile is ObstacleTile) &&
                                !tile.IsOccupied &&
                                (!(tile is CollectibleTile collectibleTile) || collectibleTile.Collectible == null);

                if (isTileFree)
                {
                    return current;
                }
            }

            foreach (var direction in directions)
            {
                var neighbor = (current.x + direction.x, current.y + direction.y);
                if (!visited.Contains(neighbor) && IsWithinBounds(neighbor.Item1, neighbor.Item2))
                {
                    queue.Enqueue((neighbor, 0));
                }
            }
        }

        return null;
    }

}