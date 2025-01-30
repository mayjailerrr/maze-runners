using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CollectibleGridView : MonoBehaviour
{
    [Header("Prefabs & Configuration")]
    public GameObject transparentTilePrefab; 
    private BoardView boardView; 

    private Board board;
    public CollectibleViewManager collectibleViewManager;

    public void InitializeGrid(Board board, BoardView boardView, CollectibleViewManager collectibleViewManager)
    {
        this.board = board;
        this.boardView = boardView;
        this.collectibleViewManager = collectibleViewManager;

        PlaceAllCollectibles();
    }

    private void PlaceAllCollectibles()
    {
        for (int x = 0; x < board.Size; x++)
        {
            for (int y = 0; y < board.Size; y++)
            {
                MazeRunners.Tile tile = board.GetTileAtPosition(x, y);
                if (tile is CollectibleTile collectibleTile)
                {
                    PlaceCollectible(x, y, collectibleTile.Collectible);
                }
            }
        }
    }

    public void PlaceCollectible(int x, int y, Collectible collectible)
    {
        GameObject tileObject = boardView.GetTileObject(x, y);

        if (tileObject == null)
        {
            Debug.LogError($"No tile found at ({x}, {y}) to place collectible {collectible.Name}.");
            return;
        }

        GameObject collectibleGO = collectibleViewManager.CreateCollectibleVisual(collectible);
        if (collectibleGO == null) return;

        collectibleGO.transform.SetParent(tileObject.transform, false);
        collectibleGO.transform.localPosition = Vector3.zero;
        collectibleGO.transform.localScale = Vector3.one * 0.6f;
    }

}
