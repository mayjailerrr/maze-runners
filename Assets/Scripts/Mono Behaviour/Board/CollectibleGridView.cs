using UnityEngine;
using UnityEngine.UI;

public class CollectibleGridView : MonoBehaviour
{
    [Header("Prefabs & Configuration")]
    public GameObject transparentTilePrefab; 
    public Transform boardParent;            
    private GridLayoutGroup gridLayoutGroup; 

    [Header("Board Params")]
    private Board board;
    private int boardSize;
    private GameObject[,] tiles;            
    private GameObject[,] collectibles;     

    public CollectibleViewManager collectibleViewManager;

    public void InitializeGrid(Board board)
    {

        this.board = board;
        boardSize = board.Size;
        tiles = new GameObject[boardSize, boardSize];
        collectibles = new GameObject[boardSize, boardSize];

        gridLayoutGroup = boardParent.GetComponent<GridLayoutGroup>();
        GenerateVisualBoard();
    }

    private void GenerateVisualBoard()
    {
        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                CreateTransparentTile(x, y);

                MazeRunners.Tile tile = board.GetTileAtPosition(x, y);
                if (tile is CollectibleTile collectibleTile)
                {
                    PlaceCollectible(x, y, collectibleTile.Collectible);
                }
            }
        }
    }

    private void CreateTransparentTile(int x, int y)
    {
        Vector3 position = ConvertGridToWorldPosition(x, y);

        GameObject transparentTileGO = Instantiate(transparentTilePrefab, position, Quaternion.identity, boardParent);
        transparentTileGO.name = $"TransparentTile ({x}, {y})";
        tiles[x, y] = transparentTileGO;
    }

    private void PlaceCollectible(int x, int y, Collectible collectible)
    {
        GameObject collectibleGO = collectibleViewManager.CreateCollectibleVisual(collectible);
        if (collectibleGO == null) return;

        GameObject transparentTile = tiles[x, y];
        collectibleGO.transform.SetParent(transparentTile.transform, false);
        collectibleGO.transform.localPosition = Vector3.zero;

        collectibles[y, x] = collectibleGO;
    }

    private Vector3 ConvertGridToWorldPosition(int x, int y)
    {
        float offsetX = -boardSize / 2.0f;
        float offsetY = -boardSize / 2.0f;

        return new Vector3(x + offsetX, y + offsetY, 0f);
    }

    public void RemoveCollectibleAt(int x, int y)
    {
        if (collectibles[x, y] != null)
        {
            Destroy(collectibles[x, y]);
            collectibles[x, y] = null;
        }
    }
}
