using System.Collections.Generic;
using MazeRunners;
using System;
using System.Linq;
using UnityEngine;
public class BoardGenerator
{
    private Board board;

    public BoardGenerator(Board board)
    {
        this.board = board;
    }

    public void GenerateBoard(List<Collectible> collectibles)
    {
        CreateEmptyTileGrid();
        PlaceObstacles();
        PlaceTraps();
        EnsureReachability();
        PlaceCollectibles(collectibles);
    }

    private void CreateEmptyTileGrid()
    {
        for (int x = 0; x < board.Size; x++)
        {
            for (int y = 0; y < board.Size; y++)
            {
                board.TileGrid[x, y] = new Tile(x, y);
            }
        }
    }

    public void PlaceObstacles()
    {
        int obstacleCount = (int)(board.Size * board.Size * 0.4f);
        PlaceTiles(obstacleCount, CanPlaceObstacle, (x, y) => new ObstacleTile(x, y));
    }

    public void PlaceTraps()
    {
        int trapCount = (int)(board.Size * board.Size * 0.1f);
        var predefinedTraps = TrapFactory.GetPredefinedTraps(trapCount).ToList();
        System.Random random = new System.Random();

        PlaceTiles(trapCount, CanPlaceTrapOrCollectible, (x, y) => 
        {
            var trapEffect = predefinedTraps[random.Next(predefinedTraps.Count)];
            return new TrapTile(x, y, trapEffect);
        });
    }

    public void EnsureReachability()
    {
        Tile startTile = GetFirstNonObstacleTile();
        if (startTile == null) return;

        bool[,] visited = PerformBFS(startTile);

        for (int x = 0; x < board.Size; x++)
        {
            for (int y = 0; y < board.Size; y++)
            {
                if (!visited[x, y] && !(board.TileGrid[x, y] is CollectibleTile))
                {
                    board.TileGrid[x, y] = new Tile(x, y);
                }
            }
        }
    }

    private Tile GetFirstNonObstacleTile()
    {
        for (int x = 0; x < board.Size; x++)
        {
            for (int y = 0; y < board.Size; y++)
            {
                if (!(board.TileGrid[x, y] is ObstacleTile))
                {
                    return board.TileGrid[x, y];
                }
            }
        }
        return null; 
    }

    public void PlaceCollectibles(List<Collectible> collectibles)
    {
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
    
    private bool CanPlaceObstacle(int x, int y)
    {
        return CanPlaceTile(x, y, tile =>
        {
            Tile originalTile = board.TileGrid[tile.Position.x, tile.Position.y];
            board.TileGrid[tile.Position.x, tile.Position.y] = new ObstacleTile(tile.Position.x, tile.Position.y);
            bool isReachable = CheckMazeConnectivity();
            board.TileGrid[tile.Position.x, tile.Position.y] = originalTile;
            return isReachable;
        });
    }

    private bool CanPlaceTrapOrCollectible(int x, int y)
    {
        return CanPlaceTile(x, y, tile =>
        {
            foreach (Tile neighbor in board.GetNeighbours(tile))
            {
                if (!(neighbor is ObstacleTile)) return true;
            }
            return false;
        });
    }

    private bool CanPlaceTile(int x, int y, Func<Tile, bool> additionalChecks = null)
    {
        if (board.TileGrid[x, y] is ObstacleTile || board.TileGrid[x, y] is TrapTile || board.TileGrid[x, y] is CollectibleTile)
            return false;

        if (additionalChecks != null && !additionalChecks(board.TileGrid[x, y]))
            return false;

        return true;
    }

    private void PlaceTiles<T>(int count, Func<int, int, bool> canPlace, Func<int, int, T> createTile) where T : Tile
    {
        System.Random random = new System.Random();

        for (int i = 0; i < count; i++)
        {
            int x, y;

            do
            {
                x = random.Next(0, board.Size);
                y = random.Next(0, board.Size);
            } while (!canPlace(x, y));

            board.TileGrid[x, y] = createTile(x, y);
        }
    }

    private bool[,] PerformBFS(Tile startTile, Func<Tile, bool> canVisit = null)
    {
        bool[,] visited = new bool[board.Size, board.Size];
        Queue<Tile> queue = new Queue<Tile>();

        queue.Enqueue(startTile);
        visited[startTile.Position.x, startTile.Position.y] = true;

        while (queue.Count > 0)
        {
            Tile currentTile = queue.Dequeue();

            foreach (Tile neighbour in board.GetNeighbours(currentTile))
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

        bool[,] visited = PerformBFS(startTile, tile => !(board.TileGrid[tile.Position.x, tile.Position.y] is ObstacleTile));

        for (int x = 0; x < board.Size; x++)
        {
            for (int y = 0; y < board.Size; y++)
            {
                if (!(board.TileGrid[x, y] is ObstacleTile) && !visited[x, y])
                    return false;
            }
        }

        return true;
    }
}