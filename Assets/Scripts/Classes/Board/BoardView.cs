
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
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

    public Dictionary<Piece, Coroutine> activeMovements = new Dictionary<Piece, Coroutine>();

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
                    tileGO.name = $"Tile ({x}, {y})";
                    tileObjects[x, y] = tileGO;
                }
            }
        }
    }

    public GameObject GetTileObject(int x, int y)
    {
        return tileObjects[x, y];
    }

    public void UpdateTileReference(int x, int y, GameObject newTile)
    {
        tileObjects[x, y] = newTile;
    }

    public GameObject GetPrefabForTile(Board board, MazeRunners.Tile tile, int x, int y)
    {
        if (tile is ObstacleTile)
        {
            var neighbors = board.GetNeighbours(tile);

            bool hasLeftNeighbor = neighbors.Any(n => n.Position.x < tile.Position.x && n is ObstacleTile);
            bool hasRightNeighbor = neighbors.Any(n => n.Position.x > tile.Position.x && n is ObstacleTile);
            bool hasTopNeighbor = neighbors.Any(n => n.Position.y > tile.Position.y && n is ObstacleTile);
            bool hasBottomNeighbor = neighbors.Any(n => n.Position.y < tile.Position.y && n is ObstacleTile);

            if (hasRightNeighbor)
            {
                return interiorObstaclePrefab;
            }

            if(hasTopNeighbor && hasRightNeighbor)
            {
                return bottomObstaclePrefab;
            }

            if (hasLeftNeighbor && hasRightNeighbor)
            {
                return bottomObstaclePrefab;
            }

            if (!hasLeftNeighbor && !hasRightNeighbor)
            {
                return bottomObstaclePrefab;
            }

            return bottomObstaclePrefab; 
        }

        if (tile is TrapTile) return trapPrefab;
        if (tile is CollectibleTile) return collectiblePrefab;

        return horizontalTilePrefab; 
    }

    public void ResetPositionWithFeedback(Piece piece)
    {
        if (activeMovements.TryGetValue(piece, out Coroutine coroutine))
        {
            StopCoroutine(coroutine);
            activeMovements.Remove(piece);
        }

        var initialTileGO = GetTileObject(piece.InitialPosition.Value.x, piece.InitialPosition.Value.y);

        var pieceView = piece.View;

        pieceView.UpdateAnimation(Vector2.zero, false); 

        LeanTween.scale(pieceView.gameObject, Vector3.zero, 0.3f)
            .setEaseInBack()
            .setOnComplete(() =>
            {
                pieceView.transform.SetParent(initialTileGO.transform, false);
                pieceView.transform.localPosition = Vector3.zero;

                LeanTween.scale(pieceView.gameObject, Vector3.one, 0.5f)
                    .setEaseOutElastic()
                    .setOnComplete(() => {
                        pieceView.transform.localScale = Vector3.one;
                        pieceView.UpdateAnimation(Vector2.zero, false); 
                    });
            });

        LeanTween.cancel(pieceView.gameObject, true);
       
        pieceView.transform.localScale = Vector3.one;
        pieceView.transform.localPosition = Vector3.zero;
        pieceView.transform.localRotation = Quaternion.identity;


        LeanTween.scale(pieceView.gameObject, Vector3.zero, 0.3f)
        .setEaseInBack()
        .setOnComplete(() =>
        {
            pieceView.transform.SetParent(initialTileGO.transform, false);
            pieceView.transform.localPosition = Vector3.zero;

            LeanTween.scale(pieceView.gameObject, Vector3.one, 0.5f)
                .setEaseOutElastic()
                .setOnComplete(() => 
                {
                    pieceView.transform.localScale = Vector3.one;
                });
        });
    }

}
