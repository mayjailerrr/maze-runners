using UnityEngine;
using MazeRunners;
using System.Collections.Generic;

public class BoardView : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject obstaclePrefab;
    public GameObject trapPrefab;
    public GameObject exitPrefab;

    private GameObject[,] tileObjects;

    public GameObject piecePrefab;
    private Dictionary<Piece, GameObject> pieceGameObjects = new Dictionary<Piece, GameObject>();

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
        if (pieceGameObjects.TryGetValue(piece, out GameObject pieceGO))
        {
            Vector3 newPosition = new Vector3(piece.Position.Item1, piece.Position.Item2, 0);
            pieceGO.transform.position = newPosition;
        }
        else
        {
            Debug.LogError($"No GameObject found for piece {piece.Name}.");
        }
    }

    public void InitializePieces(IEnumerable<Piece> pieces)
    {
        foreach (var piece in pieces)
        {
            if (pieceGameObjects.ContainsKey(piece))
            {
                Debug.LogWarning($"Piece {piece.Name} already has a GameObject.");
                continue;
            }

            GameObject pieceGO = Instantiate(piecePrefab, new Vector3(piece.Position.Item1, piece.Position.Item2, 0), Quaternion.identity, transform);
            pieceGO.name = piece.Name;

            pieceGameObjects.Add(piece, pieceGO);
        }
    }

    private PieceView GetPieceView(Piece piece)
    {
        return FindObjectOfType<PieceView>();
    }
}
