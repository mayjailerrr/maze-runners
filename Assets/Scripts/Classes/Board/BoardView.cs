
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
                GameObject prefab = GetPrefabForTile(board, currentTile, x, y);

                if (prefab != null)
                {
                    GameObject tileGO = Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                    tileObjects[x, y] = tileGO;
                }
            }
        }
    }

    private GameObject GetPrefabForTile(Board board, MazeRunners.Tile tile, int x, int y)
    {
        if (tile is ObstacleTile)
        {
            var neighbors = board.GetNeighbours(tile);
            bool hasTopObstacle = neighbors.Any(n => n.Position.y > tile.Position.y && n is ObstacleTile);
            return hasTopObstacle ? interiorObstaclePrefab : bottomObstaclePrefab;
        }

        if (tile is TrapTile) return trapPrefab;
        if (tile is CollectibleTile) return collectiblePrefab;

        var tileNeighbors = board.GetNeighbours(tile);
        bool hasLeft = tileNeighbors.Any(n => n.Position.x < x && !(n is ObstacleTile));
        bool hasRight = tileNeighbors.Any(n => n.Position.x > x && !(n is ObstacleTile));
        bool hasAbove = tileNeighbors.Any(n => n.Position.y > y && !(n is ObstacleTile));
        bool hasBelow = tileNeighbors.Any(n => n.Position.y < y && !(n is ObstacleTile));

        if ((hasLeft && hasRight) && !(hasAbove || hasBelow))
            return horizontalTilePrefab;
        if ((hasAbove && hasBelow) && !(hasLeft || hasRight))
            return verticalTilePrefab;

        return horizontalTilePrefab; 
    }
}
