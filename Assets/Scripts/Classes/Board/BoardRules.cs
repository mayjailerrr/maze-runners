using MazeRunners;
using System.Collections.Generic;
public class BoardRules
{
    private Board board;
    private bool[,] bfsCache;
    private Tile lastStartTile;

    public BoardRules(Board board)
    {
        this.board = board;
    }

    public bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < board.Size && y >= 0 && y < board.Size;
    }

    public bool IsValidMove(Piece piece, int targetX, int targetY)
    {
        if (!IsWithinBounds(targetX, targetY)) return false;

        Tile targetTile = board.TileGrid[targetX, targetY];
        return targetTile != null && !(targetTile is ObstacleTile);
    }

    public bool[,] PerformBFS(Tile startTile)
    {
        if (bfsCache != null && startTile == lastStartTile)
        {
            return bfsCache; // Reutilizar caché existente
        }

        bfsCache = new bool[board.Size, board.Size];
        Queue<Tile> queue = new Queue<Tile>();

        queue.Enqueue(startTile);
        bfsCache[startTile.Position.x, startTile.Position.y] = true;

        while (queue.Count > 0)
        {
            Tile currentTile = queue.Dequeue();

            foreach (Tile neighbour in board.GetNeighbours(currentTile))
            {
                int nx = neighbour.Position.x;
                int ny = neighbour.Position.y;

                if (!bfsCache[nx, ny] && !(board.TileGrid[nx, ny] is ObstacleTile))
                {
                    bfsCache[nx, ny] = true;
                    queue.Enqueue(neighbour);
                }
            }
        }

        lastStartTile = startTile;
        return bfsCache;
    }
}
