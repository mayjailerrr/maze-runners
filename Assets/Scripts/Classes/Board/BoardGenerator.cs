using System.Collections.Generic;
using MazeRunners;
using System;
using System.Linq;
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

    private void PlaceObstacles()
    {
        int obstacleCount = (int)(board.Size * board.Size * 0.4f);
        PlaceTiles(obstacleCount, (x, y) => board.TileGrid[x, y] is null, (x, y) => new ObstacleTile(x, y));
    }

    private void PlaceTraps()
    {
        int trapCount = (int)(board.Size * board.Size * 0.1f);
        var predefinedTraps = TrapFactory.GetPredefinedTraps(trapCount).ToList();
        System.Random random = new System.Random();

        PlaceTiles(trapCount, (x, y) => board.TileGrid[x, y] is null, (x, y) =>
        {
            var trapEffect = predefinedTraps[random.Next(predefinedTraps.Count)];
            return new TrapTile(x, y, trapEffect);
        });
    }

    private void EnsureReachability()
    {
        // Lógica de aseguramiento de conectividad
    }

    private void PlaceCollectibles(List<Collectible> collectibles)
    {
        foreach (var collectible in collectibles)
        {
            PlaceTiles(1, (x, y) => board.TileGrid[x, y] is null, (x, y) => new CollectibleTile(x, y, collectible));
        }
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
}