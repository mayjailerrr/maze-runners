using UnityEngine;
using MazeRunners;

public class BoardView : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject obstaclePrefab;
    public GameObject trapPrefab;
    public GameObject exitPrefab;

    private GameObject[,] tileObjects;

    public void InitializeView(Board board)
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
                Tile tile = board.GetTileAtPosition(x, y);
                GameObject prefab = GetPrefabForTile(tile);

                if (prefab != null)
                {
                    GameObject tileGO = Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                    tileObjects[x, y] = tileGO;
                }
            }
        }
    }

    private GameObject GetPrefabForTile(Tile tile)
    {
        if (tile is ObstacleTile) return obstaclePrefab;
        if (tile is TrapTile) return trapPrefab;
        if (tile is ExitTile) return exitPrefab;

        return tilePrefab;
    }

    public void UpdatePiecePosition(Piece piece)
    {
        PieceView pieceView = GetPieceView(piece);
        if (pieceView != null)
        {
            Vector3 newPosition = new Vector3(piece.Position.x, piece.Position.y, 0);
            pieceView.AnimateMove(newPosition);
        }
    }

    private PieceView GetPieceView(Piece piece)
    {
        return FindObjectOfType<PieceView>();
    }
}
