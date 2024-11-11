public class Board
{
    

    public int Size { get; private set; }

    public Board(int size)
    {
        Size = size;
        grid = new Tile[size, size];
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        CreateEmptyGrid();
        PlaceObstacles();
        PlaceTraps();
        EnsureReachability();
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

    private void PlaceObstacles()
    {
        int obstacleCount = (int)(Size * Size * 0.2f); //20% of the board will be obstacles
        System.Random random = new System.Random();

        for (int i = 0; i < obstacleCount; i++)
        {
            int x, y;

            do
            {
                x = random.Next(0, Size);
                y = random.Next(0, Size);
            } while (grid[x, y] is ObstacleTile);

            grid[x, y] = new ObstacleTile(x, y);
        }
    }

    private void PlaceTraps()
    {
        int trapCount = (int)(Size * Size * 0.1f); //10% of the board will be traps
        System.Random random = new System.Random();

        for (int i = 0; i < trapCount; i++)
        {
            int x, y;

            do
            {
                x = random.Next(0, Size);
                y = random.Next(0, Size);
            } while (grid[x, y] is ObstacleTile || grid[x, y] is TrapTile); //avoids to put traps on obstacles or other traps

            int trapType = random.Next(1, 4);
            TrapTile trapEffect;

            switch (trapType)
            {
                case 1:
                    trapEffect = new SlowTrap(); //reduces the velocity of the pieces
                    break;
                case 2:
                    trapEffect = new DamageTrap(); //hurts the piece
                    break;
                case 3:
                    trapEffect = new FreezeTrap(); //inmovilize the piece temporally
                    break;
                default:
                    trapEffect = new SlowTrap(); //fallback
                    break;
            }

            grid[x, y] = new TrapTile(x, y, trapEffect);
        }
    }

    private void EnsureReachability()
    {
        bool[,] visited = new bool[Size, Size];
        Queue<Tile> queue = new Queue<Tile>();

        //starts in tile 0,0 that is not an obstacle
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

        //make a BFS from startTile to ensure all tiles are reachable
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


        //gets all the valid neighbours of a tile to BFS
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

    private IEnumerable<Tile> GetNeighbours(Tile tile)
    {
        int x = tile.Position.x;
        int y = tile.Position.y;

        int [,] directions = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

        foreach (var dir in directions)
        {
            int nx = x + dir[0];
            int ny = y + dir[1];

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
}

// public void GenerateVisualBoard(GameObject tilePrefab)
// {
//     for (int x = 0; x < Size; x++)
//     {
//         for (int y = 0; y < Size; y++)
//         {
//             GameObject tileGO = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity);
//             Tile tile = GetTileAtPosition(x, y);

//             // Configura el color o el aspecto visual del tile según el tipo
//             if (tile is ObstacleTile)
//             {
//                 // Cambia el color a gris, por ejemplo
//             }
//             else if (tile is TrapTile)
//             {
//                 // Cambia el color o agrega un ícono de trampa
//             }
//         }
//     }
// }
