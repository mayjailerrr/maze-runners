
using UnityEngine;
using System.Linq;

public class BoardView : MonoBehaviour
{
    [Header("Tile Prefabs")]
    public GameObject horizontalTilePrefab; 
    public GameObject verticalTilePrefab; 
    public GameObject interiorObstaclePrefab;
    public GameObject bottomObstaclePrefab;
    public GameObject trapPrefab;
    public GameObject collectiblePrefab;

    private GameObject[,] tileObjects;

    public void InitializeTileBoardView(Board board)
    {
        int boardSize = board.Size;
        tileObjects = new GameObject[boardSize, boardSize];
        GenerateVisualBoard(board);
    }

    private void GenerateVisualBoard(Board board)
    {
        for (int x = 0; x < board.Size; x++)
        {
            for (int y = 0; y < board.Size; y++)
            {
                MazeRunners.Tile currentTile = board.TileGrid[x, y];
                GameObject prefab = GetPrefabForTile(board, currentTile);

                if (prefab != null)
                {
                    GameObject tileGO = Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                    tileGO.name = $"Tile ({x}, {y})";
                    tileObjects[x, y] = tileGO;
                }
            }
        }
    }

    private GameObject GetPrefabForTile(Board board, MazeRunners.Tile tile)
    {
        if (tile is ObstacleTile)
        {
            return interiorObstaclePrefab;
        }

        if (tile is TrapTile) return trapPrefab;
        if (tile is CollectibleTile) return collectiblePrefab;

        return DeterminePathTilePrefab(board, tile);
    }

    private GameObject DeterminePathTilePrefab(Board board, MazeRunners.Tile tile)
    {
        var neighbours = board.GetNeighbours(tile);
       
        bool hasTopNeighbour = neighbours.Any(n => n.Position.y == tile.Position.y - 1);
        bool hasBottomNeighbour = neighbours.Any(n => n.Position.y == tile.Position.y + 1);
        bool hasLeftNeighbour = neighbours.Any(n => n.Position.x == tile.Position.x - 1);
        bool hasRightNeighbour = neighbours.Any(n => n.Position.x == tile.Position.x + 1);

        if (hasTopNeighbour && hasBottomNeighbour && hasLeftNeighbour && hasRightNeighbour)
        {
            return horizontalTilePrefab;
        }

        if ((hasTopNeighbour || hasBottomNeighbour) && (hasLeftNeighbour || hasRightNeighbour))
        {
            return verticalTilePrefab;
        }

        return horizontalTilePrefab;
    }
}
