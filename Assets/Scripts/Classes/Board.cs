using System.Collections.Generic;
using MazeRunners;
using UnityEngine;
using System;


public class Board
{

    public int Size { get; private set; }

    public Tile[,] grid;

    public Board(int size)
    {
        Size = size;
        grid = new Tile[size, size];
        GenerateBoard();
    }

    public void GenerateBoard()
    {
        CreateEmptyGrid();
        PlaceObstacles();
        PlaceTraps();
        EnsureReachability();
        PrintBoard();
        PlaceExists();
       
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
   

   public void PlaceObstacles()
{
    int obstacleCount = (int)(Size * Size * 0.4f); // 40% of the board will be obstacles
    System.Random random = new System.Random();

    for (int i = 0; i < obstacleCount; i++)
    {
        int x, y;

        do
        {
            x = random.Next(0, Size);
            y = random.Next(0, Size);
        } while (!CanPlaceObstacle(x, y));

        grid[x, y] = new ObstacleTile(x, y);
    }
}


private bool CanPlaceObstacle(int x, int y)
{
    if (grid[x, y] is ObstacleTile || grid[x, y] is TrapTile || grid[x, y] is ExitTile)
        return false;

    Tile originalTile = grid[x, y];
    grid[x, y] = new ObstacleTile(x, y);
   
    bool isReachable = CheckMazeConnectivity();

    grid[x, y] = originalTile;

    return isReachable;
}


  

    private bool CheckMazeConnectivity()
    {
        bool[,] visited = new bool[Size, Size];
        Queue<Tile> queue = new Queue<Tile>();

        Tile startTile = null;

        for (int x = 0; x < Size && startTile == null; x++)
        {
            for (int y = 0; y < Size && startTile == null; y++)
            {
                if (!(grid[x, y] is ObstacleTile))
                    startTile = grid[x, y];
            }
        }

        if(startTile == null) return false;

        queue.Enqueue(startTile);
        visited[startTile.Position.x, startTile.Position.y] = true;

        while (queue.Count > 0)
        {
            Tile currentTile = queue.Dequeue();

            foreach (Tile neighbour in GetNeighbours(currentTile))
            {
                int nx = neighbour.Position.x;
                int ny = neighbour.Position.y;

                if (!visited[nx, ny] && !(grid[nx, ny] is ObstacleTile))
                {
                    visited[nx, ny] = true;
                    queue.Enqueue(neighbour);
                }
            }
        }

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

    private bool CanPlaceTrapOrExit(int x, int y)
    {
        if (grid[x, y] is ObstacleTile || grid[x, y] is TrapTile || grid[x, y] is ExitTile)
            return false;

        foreach (Tile neighbor in GetNeighbours(grid[x, y]))
        {
            if (!(neighbor is ObstacleTile))
                return true; 
        }

        return false; 
    }

    public void PlaceTraps()
    {
        int trapCount = (int)(Size * Size * 0.1f); // 10% of the board will be traps
        System.Random random = new System.Random();

        
       for (int i = 0; i < trapCount; i++)
        {
            int x, y;

            do
            {
                x = random.Next(0, Size);
                y = random.Next(0, Size);
            } while (!CanPlaceTrapOrExit(x, y));

            Trap randomTrap = TrapFactory.CreateRandomTrap();

           grid[x, y] = new TrapTile(x, y, randomTrap);
        }
    }


    public void EnsureReachability()
    {
        bool[,] visited = new bool[Size, Size];
        Queue<Tile> queue = new Queue<Tile>();

       
        Tile startTile = null;
        for (int x = 0; x < Size && startTile == null; x++)
        {
            for (int y = 0; y < Size && startTile == null; y++)
            {
                if (!(grid[x, y] is ObstacleTile))
                {
                    startTile = grid[x, y];
                }
            }
        }

        //BFS algorithm
        if (startTile != null)
        {
            queue.Enqueue(startTile);
            visited[startTile.Position.x, startTile.Position.y] = true;

            while (queue.Count > 0)
            {
                Tile currentTile = queue.Dequeue();

                foreach (Tile neighbour in GetNeighbours(currentTile))
                {
                    int nx = neighbour.Position.x;
                    int ny = neighbour.Position.y;


                    if (!visited[nx, ny])
                    {
                        visited[nx, ny] = true;
                        queue.Enqueue(neighbour);
                    }
                }
            }
        }

        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                if (!visited[x, y] && !(grid[x, y] is ExitTile))
                {
                    grid[x, y] = new Tile(x, y);
                }
            }
        }
    }

    public void PrintBoard()
    {
        for (int y = Size - 1; y >= 0; y--)
        {
            string row = "";
            for (int x = 0; x < Size; x++)
            {
                if (grid[x, y] is ObstacleTile)
                    row += "O ";
                else if (grid[x, y] is TrapTile)
                    row += "T ";
                else if (grid[x, y] is ExitTile)
                    row += "E ";
                else
                    row += ". ";
            }
            Debug.Log(row);
        }
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

    private void PlaceExists()
    {
        System.Random random = new System.Random();
        int exitCount = 2; //just two exits, i can adjust this

        for (int i = 0; i < exitCount; i++)
        {
            int x, y;

            do 
            {
                x = random.Next(0, Size);
                y = random.Next(0, Size);
            } while (grid[x, y] is ObstacleTile || grid[x, y] is TrapTile || grid[x, y] is ExitTile);

            grid[x, y] = new ExitTile(x, y);
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

        return !targetTile.IsOccupied && !(targetTile is ObstacleTile);
    }
}


